using BitirmeProjesiUI.Identity;
using DataAcsessLayer.Concrete.EFCore;
using EntityLayer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BitirmeProjesiUI.Controllers
{
    public class PostController : Controller
    {
        private UserManager<Identity.User> _userManager;
        private SignInManager<Identity.User> _signInManager;


        public PostController(UserManager<Identity.User> userManager, SignInManager<Identity.User> signInManager)
        {

            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult AddPost()
        {
            var user = _userManager.Users.Where(i => i.Id == _userManager.GetUserId(User)).FirstOrDefault();
            ViewBag.User = user;
            using (var x = new Context())
            {
                var countFollower = x.Follows.Where(i => i.BuyerToken == user.Id).Where(i => i.Status == 1).Count();
                ViewBag.countFollower = countFollower;
            }
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> AddPost(Post model, IFormFile image)
        {
            try
            {
                var user = _userManager.Users.Where(i => i.Id == _userManager.GetUserId(User)).FirstOrDefault();
                ViewBag.User = user;
                var date = DateTime.Now;

                if (image != null)
                {
                    var extension = Path.GetExtension(image.FileName);
                    var newImageName = Guid.NewGuid() + extension;
                    var location = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/PostImg/", newImageName);
                    var stream = new FileStream(location, FileMode.Create);
                    image.CopyTo(stream);

                    using (var db = new Context())
                    {
                        var post = new Post()
                        {
                            Description = model.Description,
                            Title = "",
                            CountComment = 0,
                            CountLike = 0,
                            CountSeen = 0,
                            CountDislike = 0,
                            UserId = user.Id,
                            UserName = user.UserName,
                            Created_at = date,
                            Pp = user.ProfilePhoto,
                            ImageUrl = newImageName,
                        };
                        db.Posts.Add(post);
                        db.SaveChanges();
                        var result = await _userManager.UpdateAsync(user);

                        if (result.Succeeded)
                        {
                            return Json(post);
                        }
                        else
                        {
                            return Json(new { success = false, error = "User update failed." });
                        }
                    }
                }
                else
                {
                    using (var db = new Context())
                    {
                        var post = new Post()
                        {
                            Description = model.Description,
                            Title = model.Title,
                            CountComment = 0,
                            CountLike = 0,
                            CountSeen = 0,
                            CountDislike = 0,
                            UserId = user.Id,
                            UserName = user.UserName,
                            Created_at = date,
                            Pp = user.ProfilePhoto,
                            ImageUrl = "123.jpg",
                        };
                        db.Posts.Add(post);
                        db.SaveChanges();

                        var result = await _userManager.UpdateAsync(user);

                        if (result.Succeeded)
                        {
                            return Json(new { success = true, redirectUrl = "~/Account/ProfilePage" });
                        }
                        else
                        {
                            return Json(new { success = false, error = "User update failed." });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex.Message });
            }
        }



        public JsonResult GetPost()
        {
            var userId = _userManager.GetUserId(User);

            using (var db = new Context())
            {
                var posts = db.Posts.Where(i => i.UserId == userId).OrderByDescending(i => i.Created_at).ToList();

                return Json(posts);
            }

        }
        public JsonResult GetProfileFriendGetPost(string id)
        {
            var userId = id;

            using (var db = new Context())
            {
                var posts = db.Posts.Where(i => i.UserId == userId).OrderByDescending(i => i.Created_at).ToList();

                return Json(posts);
            }

        }

    }
}
