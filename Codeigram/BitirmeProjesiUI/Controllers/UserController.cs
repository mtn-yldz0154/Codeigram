using BitirmeProjesiUI.Identity;
using BuisnessLayer.Abstract;
using DataAcsessLayer.Concrete.EFCore;
using EntityLayer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BitirmeProjesiUI.Controllers
{
    public class UserController : Controller
    {
		private UserManager<User> _userManager;
		private SignInManager<User> _signInManager;
		private INotificationService _notificationService;


        public UserController(UserManager<User> userManager, SignInManager<User> signInManager, INotificationService notificationService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
			_notificationService = notificationService;

        }
		public IActionResult Index()
        {
            return View();
        }

		public JsonResult GetUser(string username)
		{
            var user = _userManager.Users.Where(i => i.Id == _userManager.GetUserId(User)).FirstOrDefault();

            // Kullanıcı bilgilerini getir (örneğin, UserName içinde username geçenleri filtrele)
            var users = _userManager.Users
					.Where(i => i.UserName.Contains(username) && i.Id!=user.Id)
					.ToList();

				return Json(users);
			
		}

		public JsonResult GetNotification()
		{
			var userId = _userManager.GetUserId(User);

			using (var db=new Context())
			{

				var notifications = db.Notifications.Where(i => i.BuyerToken == userId).OrderByDescending(i=>i.Created_at).ToList();


				

				return Json(notifications);	

			}

		}

		public JsonResult Seen()
		{
			var userId = _userManager.GetUserId(User);

			using (var db = new Context())
			{

				var notifications = db.Notifications.Where(i => i.BuyerToken == userId).ToList();


				foreach (var item in notifications)
				{
					item.Seen = 1;
					db.Notifications.Update(item);
				}

				db.SaveChanges();

				return Json(200);

			}

		}

		public JsonResult GetNotificationNew()
		{
			var userId = _userManager.GetUserId(User);

			using (var db=new Context())
			{
			  var count=db.Notifications.Where(i => i.BuyerToken == userId && i.Seen == 0).Count();

				return Json(count);
			}
		}

		public JsonResult GetNotificationAccept(int id)
		{
			var buyerId=_userManager.GetUserId(User);

			using (var db=new Context())
			{
				var notification = db.Notifications.Where(i => i.Id == id).FirstOrDefault();
				var user = _userManager.Users.Where(i => i.UserName == notification.Username).FirstOrDefault();
				db.Notifications.Remove(notification);

			

				var bildirim = new Notification()
				{
					BuyerToken = buyerId,
					Message = "Sizi Takip Etmeye Başladı",
					Created_at = DateTime.Now,
					NotificationType = 2,
					PP = user.ProfilePhoto,
					Seen = 0,
					Status = 1,
					Username = user.UserName,

				};

				db.Notifications.Add(bildirim);

				var follow = db.Follows.Where(i => i.BuyerToken == buyerId && i.SenderToken == user.Id).FirstOrDefault();

				follow.Status = 1;
				db.Follows.Update(follow);

				db.SaveChanges();
				return Json(200);

			}






		}



	}
}
