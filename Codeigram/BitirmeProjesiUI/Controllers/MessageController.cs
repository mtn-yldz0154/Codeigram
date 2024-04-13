using BitirmeProjesiUI.Identity;
using BitirmeProjesiUI.Models;
using DataAcsessLayer.Concrete.EFCore;
using EntityLayer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

public class MessageController : Controller
{
	private readonly UserManager<User> _userManager;
	private readonly IHubContext<ChatHub> _hubContext;


	public MessageController(UserManager<User> userManager, IHubContext<ChatHub> hubContext)
	{
		_userManager = userManager;
		_hubContext = hubContext;

	}

	public IActionResult Index()
	{
		var userId = _userManager.GetUserId(User);
		var users = _userManager.Users.Where(i => i.Id == userId).FirstOrDefault();

		ViewBag.userId = userId;

		using (var db = new Context())
		{
			var chatRooms = db.ChatRooms.Where(i => i.UserToken == userId || i.ToToken == userId).ToList();

			var ModelList = new List<BitirmeProjesiUI.Models.ChatRoom>();
			var user = new User();

			foreach (var item in chatRooms)
			{
				if (item.UserToken == userId)
				{
					user = _userManager.Users.Where(i => i.Id == item.ToToken).FirstOrDefault();

				}
				else
				{
					user = _userManager.Users.Where(i => i.Id == item.UserToken).FirstOrDefault();

				}



				var model = new BitirmeProjesiUI.Models.ChatRoom()
				{
					Id = item.Id,
					ToToken = item.ToToken,
					BlockUserToken = item.BlockUserToken,
					BlocToken = item.BlocToken,
					Created_at = item.Created_at,
					LastMessage = item.LastMessage,
					LastTimeMessage = item.LastTimeMessage,
					Status = item.Status,
					UserToken = item.UserToken,
					PP = user.ProfilePhoto,
					Username = user.UserName
				};

				ModelList.Add(model);
			}

			return View(ModelList);
		}
	}

	public async Task<JsonResult> GetMessage(int id)
	{
		var userId = _userManager.GetUserId(User);

		using (var db = new Context())
		{
			var messages = db.ChatMessages.Where(i => i.RoomId == id).ToList();

			// İstemciye mesajları gönder
			await _hubContext.Clients.Client(userId).SendAsync("ReceiveMessages", messages);

			return Json(messages);
		}
	}

	public async Task SendMessage(int personId, string userId, string messageContent)
	{
		using (var db = new Context())
		{
			var senderId = _userManager.GetUserId(User);

			// Mesajı veritabanına kaydet
			var message = new ChatMessages
			{
				RoomId = personId,
				SenderToken = senderId,
				Messages = messageContent,
				Created_at = DateTime.Now
			};

			db.ChatMessages.Add(message);
			await db.SaveChangesAsync();

			// SignalR ile mesajları gönder
			await _hubContext.Clients.Group(personId.ToString()).SendAsync("ReceiveMessage", message);
		}

	}
}
