// ChatHub.cs

using BitirmeProjesiUI.Identity;
using DataAcsessLayer.Concrete.EFCore;
using EntityLayer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

public class ChatHub : Hub
{
	private UserManager<User> _userManager;

	public ChatHub(UserManager<User> userManager)
	{
		_userManager = userManager;
	}
	public async Task JoinGroup(string groupName)
	{
		await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
	}

	public async Task LeaveGroup(string groupName)
	{
		await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
	}

	public async Task SendMessage(string toToken, string userToken, string messageContent)
	{
		// Bu kısımda veritabanına mesajı ekleyin.

		// Örnek olarak Entity Framework kullanıyorsanız:
		using (var dbContext = new Context())
		{
			var room = dbContext.ChatRooms.Where(i => (i.UserToken == toToken && i.ToToken == userToken) || (i.ToToken == toToken && i.UserToken == userToken)).FirstOrDefault();

			var message = new ChatMessages
			{
				RoomId = room.Id,
				SenderToken = userToken,
				Messages = messageContent,
				// Diğer gerekli alanları doldurun
			};

			dbContext.ChatMessages.Add(message);
			await dbContext.SaveChangesAsync();
		}

		// Burada gelen mesajı istemcilere gönderin
		await Clients.All.SendAsync("ReceiveMessage", toToken, userToken, messageContent);
	}
}
