using BuisnessLayer.Abstract;
using DataAcsessLayer.Concrete.EFCore;
using EntityLayer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IO.Compression;

namespace BitirmeProjesiUI.Controllers
{
    public class FollowController : Controller
    {


        private UserManager<Identity.User> _userManager;
        private SignInManager<Identity.User> _signInManager;
        private IFollowService _followService;
        private INotificationService _notificationService;


        public FollowController(INotificationService notificationService, UserManager<Identity.User> userManager, SignInManager<Identity.User> signInManager, IFollowService followService)
        {

            _userManager = userManager;
            _signInManager = signInManager;
            _followService = followService;
            _notificationService = notificationService;
        }


        public IActionResult AddFollow(string id)
        {
            var senderId = _userManager.GetUserId(User);
            var buyerId = id;
            var senderName = _userManager.Users.Where(i => i.Id == senderId).Select(i => i.UserName).FirstOrDefault();
            var sender = _userManager.Users.Where(i => i.Id == senderId).FirstOrDefault();
            var buyerName = _userManager.Users.Where(i => i.Id == id).Select(i => i.UserName).FirstOrDefault();
            var created_at = DateTime.Now;

            var follow = new Follow()
            {
                BuyerName = buyerName,
                SenderName = senderName,
                SenderToken = senderId,
                BuyerToken = buyerId,
                Created_At = created_at,
                Status = 0

            };

            var notification = new Notification()
            {
                Created_at = created_at,
                Username = sender.UserName,
                PP = sender.ProfilePhoto,
                NotificationType = 1,
                Seen = 0,
                Status = 1,
                Message = "Sizi Takip Etmek İstiyor",
                BuyerToken = buyerId,
            };


            _followService.addFollow(follow);
            _notificationService.Create(notification);


            return Redirect("/account/getProfileFriend/" + id);
        }

        public IActionResult DeleteFollow(string id)
        {
            var senderToken = _userManager.GetUserId(User);
            var buyerToken = id;

            using (var db = new Context())
            {
                var follow = db.Follows.Where(i => i.SenderToken == senderToken && i.BuyerToken == buyerToken).FirstOrDefault();

                follow.Status = 2;

                db.Follows.Update(follow);
                db.SaveChanges();

                return Redirect("/account/getProfileFriend/" + id);
            }

        }
        public IActionResult UpdateFollow(string id)
        {
            var senderToken = _userManager.GetUserId(User);
            var buyerToken = id;

            using (var db = new Context())
            {
                var follow = db.Follows.Where(i => i.SenderToken == senderToken && i.BuyerToken == buyerToken).FirstOrDefault();

                follow.Status = 0;

                db.Follows.Update(follow);
                db.SaveChanges();

                return Redirect("/account/getProfileFriend/" + id);
            }

        }


    }
}
