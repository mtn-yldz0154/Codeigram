using BitirmeProjesiUI.Identity;
using BitirmeProjesiUI.Models;
using DataAcsessLayer.Concrete.EFCore;
using EntityLayer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace BitirmeProjesiUI.Controllers
{

    public class AccountController : Controller
    {
        private UserManager<Identity.User> _userManager;
        private SignInManager<Identity.User> _signInManager;


        public AccountController(UserManager<Identity.User> userManager, SignInManager<Identity.User> signInManager)
        {

            _userManager = userManager;
            _signInManager = signInManager;
        }


        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(AccountModel model)
        {


            var user = await _userManager.FindByNameAsync(model.LoginModel.UserName);
            if (user == null)
            {
                ModelState.AddModelError("", "Kullanıcı Bulunamadı");
                return View(model);
            }



            var result = await _signInManager.PasswordSignInAsync(user, model.LoginModel.Password, true, false);

            if (result.Succeeded)
            {
                return Redirect("/user/index");
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(AccountModel model)
        {


            var user = new Identity.User
            {

                FirstName = model.RegisterModel.Name,
                LastName = model.RegisterModel.LastName,
                Email = model.RegisterModel.Email,
                UserName = model.RegisterModel.UserName,
                ImageUrl = "arkaPlan.png",
                ProfilePhoto = "pp.jpg",


            };
            var result = await _userManager.CreateAsync(user, model.RegisterModel.Password);

            if (result.Succeeded)
            {


                return RedirectToAction("login", "account");
            }
            ModelState.AddModelError("", "Bilinmeyen Bir Hata Oluştu Tekrar Deneyiniz");
            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction("index", "user");
        }

        public IActionResult Accessdenied()
        {
            return View();
        }

        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (userId != null && token != null)
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user != null)
                {
                    var result = await _userManager.ConfirmEmailAsync(user, token);
                    if (result.Succeeded)
                    {
                        return View();
                    }
                }
                return RedirectToAction("index", "user");
            }
            return RedirectToAction("index", "user");

        }

        [Authorize]
        public IActionResult ProfilePage()
        {

            var user = _userManager.Users.Where(i => i.Id == _userManager.GetUserId(User)).FirstOrDefault();

            using (var x = new Context())
            {
                var countFollower = x.Follows.Where(i => i.BuyerToken == user.Id).Where(i => i.Status == 1).Count();
                ViewBag.countFollower = countFollower;
                var likePost = x.PostLikes.ToList();

                ViewBag.likePost = likePost;

                var posts = x.Posts.Where(i => i.UserId == _userManager.GetUserId(User)).OrderByDescending(i => i.Created_at).ToList();

                var yorums = x.Yorums.Where(i => i.MainYorumId == 0).OrderByDescending(i => i.Created_at).ToList();
                var yorumYorums = x.Yorums.Where(i => i.MainYorumId != 0).OrderByDescending(i => i.Created_at).ToList();
                ViewBag.Yorums = yorums;
                ViewBag.YorumYorums = yorumYorums;

                ViewBag.Posts = posts;


                var Follow = x.Follows.Where(i => i.SenderToken == user.Id).Where(i => i.Status == 1).ToList();

                List<Post> Followss = new List<Post>();

                foreach (var a in Follow)
                {
                    var post = x.Posts.Where(i => i.UserId == a.BuyerToken).FirstOrDefault();
                    
                    Followss.Add(post);

                }

                ViewBag.Follows = Followss;
            }

			ViewBag.userId = user.Id;
			return View(user);
        }


        public IActionResult GetProfileFriend(string id)
        {
            using (var db = new Context())
            {
                var userId = _userManager.GetUserId(User);

                var follow = db.Follows.Where(i => i.SenderToken == userId && i.BuyerToken == id).FirstOrDefault();

                if (follow != null)
                {
                    if (follow.Status == 0)
                    {
                        ViewBag.follow = 0;
                    }
                    else if (follow.Status == 1)
                    {
                        ViewBag.follow = 1;
                    }
                    else
                    {
                        ViewBag.follow = 2;
                    }
                }
                else
                {
                    ViewBag.follow = -1;
                }

                var user = _userManager.Users.Where(i => i.Id == id).FirstOrDefault();
                ViewBag.UserId = user.Id;
                return View(user);
            }
        }
        public JsonResult GetProfileFriendGetPost()
        {
            var userId = _userManager.GetUserId(User);

            using (var db = new Context())
            {

                var posts = db.Posts.Where(i => i.UserId == userId).OrderByDescending(i => i.Created_at).ToList();

                return Json(posts);

            }

        }



        public IActionResult EditProfile()
        {
            var user = _userManager.Users.Where(i => i.Id == _userManager.GetUserId(User)).FirstOrDefault();
            return View(user);

        }


        [HttpPost]
        public async Task<IActionResult> EditProfile(User model, IFormFile image, IFormFile image2)
        {
            var user = _userManager.Users.Where(i => i.Id == model.Id).FirstOrDefault();

            if (image != null && image2 != null)
            {
                var extension = Path.GetExtension(image.FileName);
                var newImageName = Guid.NewGuid() + extension;
                var location = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/PP/", newImageName);
                var stream = new FileStream(location, FileMode.Create);
                image.CopyTo(stream);

                var extension2 = Path.GetExtension(image2.FileName);
                var newImageName2 = Guid.NewGuid() + extension2;
                var location2 = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Tamplate/images/", newImageName2);
                var stream2 = new FileStream(location2, FileMode.Create);
                image2.CopyTo(stream2);

                user.ProfilePhoto = newImageName;
                user.ImageUrl = newImageName2;
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.Email = model.Email;
                user.UserName = model.UserName;


                var result = await _userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    using (var db = new Context())
                    {
                        var yorums = db.Yorums.Where(i => i.UserId == user.Id).ToList();
                        if (yorums != null)
                        {
                            foreach (var item in yorums)
                            {
                                item.UserImg = newImageName;
                                db.Yorums.Update(item);
                                db.SaveChanges();
                            }
                        }
                    }

                    return Redirect("ProfilePage");

                }
                else
                {
                    return View(model);
                }

            }
            else if (image != null && image2 == null)
            {

                var extension = Path.GetExtension(image.FileName);
                var newImageName = Guid.NewGuid() + extension;
                var location = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/PP/", newImageName);
                var stream = new FileStream(location, FileMode.Create);
                image.CopyTo(stream);

                user.ProfilePhoto = newImageName;
                user.ImageUrl = user.ImageUrl;
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.Email = model.Email;
                user.UserName = model.UserName;


                var result = await _userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    using (var db = new Context())
                    {
                        var yorums = db.Yorums.Where(i => i.UserId == user.Id).ToList();
                        if (yorums != null)
                        {
                            foreach (var item in yorums)
                            {
                                item.UserImg = newImageName;
                                db.Yorums.Update(item);
                                db.SaveChanges();
                            }
                        }
                    }

                    return Redirect("ProfilePage");

                }
                else
                {
                    return View(model);
                }

            }
            else if (image == null && image2 != null)
            {

                var extension2 = Path.GetExtension(image2.FileName);
                var newImageName2 = Guid.NewGuid() + extension2;
                var location2 = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Tamplate/images/", newImageName2);
                var stream2 = new FileStream(location2, FileMode.Create);
                image2.CopyTo(stream2);

                user.ProfilePhoto = user.ProfilePhoto;
                user.ImageUrl = newImageName2;
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.Email = model.Email;
                user.UserName = model.UserName;


                var result = await _userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    using (var db = new Context())
                    {
                        var yorums = db.Yorums.Where(i => i.UserId == user.Id).ToList();
                        if (yorums != null)
                        {
                            foreach (var item in yorums)
                            {
                                ;
                                db.Yorums.Update(item);
                                db.SaveChanges();
                            }
                        }
                    }

                    return Redirect("ProfilePage");

                }
                else
                {
                    return View(model);
                }

            }
            else
            {
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.Email = model.Email;
                user.UserName = model.UserName;

                var result = await _userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    return Redirect("ProfilePage");
                }
                else
                {
                    return View(model);
                }
            }
        }
        public IActionResult Friends()
        {
            var user = _userManager.Users.Where(i => i.Id == _userManager.GetUserId(User)).FirstOrDefault();

            using (var db = new Context())
            {
                var Follows = db.Follows.Where(i => i.BuyerToken == user.Id).Where(i => i.Status == 1).ToList();
                var FollowsCount = db.Follows.Where(i => i.BuyerToken == user.Id).Where(i => i.Status == 1).Count();


                List<Post> Followsss = new List<Post>();

                foreach (var a in Follows)
                {
                    var x = db.Posts.Where(i => i.UserId == a.SenderToken).FirstOrDefault();

                    Followsss.Add(x);

                }

                var Follow = db.Follows.Where(i => i.SenderToken == user.Id).Where(i => i.Status == 1).ToList();
                var FollowCount = db.Follows.Where(i => i.SenderToken == user.Id).Where(i => i.Status == 1).Count();

				List<Post> Followss = new List<Post>();

				foreach (var a in Follow)
                {
                    var x = db.Posts.Where(i => i.UserId == a.BuyerToken).FirstOrDefault();

					Followss.Add(x);

				}

				ViewBag.Follows = Followsss;
				ViewBag.FollowsCount = FollowsCount;

				ViewBag.Follow = Followss;
                ViewBag.FollowCount = FollowCount;

				var users = _userManager.Users.Where(i => i.Id == _userManager.GetUserId(User)).FirstOrDefault();
                using (var x = new Context())
                {
                    var countFollower = x.Follows.Where(i => i.BuyerToken == user.Id).Where(i => i.Status == 1).Count();
                    ViewBag.countFollower = countFollower;
                }
                return View(users);
			}

		}

    }
}

