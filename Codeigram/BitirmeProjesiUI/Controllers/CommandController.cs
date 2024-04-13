using BitirmeProjesiUI.Identity;
using DataAcsessLayer.Concrete.EFCore;
using EntityLayer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;

namespace BitirmeProjesiUI.Controllers
{
    public class CommandController : Controller
    {
        private UserManager<User> _usermanager;
        public CommandController(UserManager<User> usermanager)
        {
            _usermanager = usermanager;
        }

        [HttpPost]
        public JsonResult AddCommand(Yorum model)
        {
            var user = _usermanager.Users.Where(i => i.Id == _usermanager.GetUserId(User)).FirstOrDefault();


            using (var db = new Context())
            {
                var yorum = new Yorum()
                {
                    CountLike = 0,
                    CountDislike = 0,
                    Created_at = DateTime.Now,
                    Comment = model.Comment,
                    MainYorumId = 0,
                    Status = 1,
                    PostId = model.PostId,
                    UserImg = user.ProfilePhoto,
                    UserId = user.Id,
                    Usernmame = user.UserName
                };
                

                var post = db.Posts.Where(i => i.Id == model.PostId).FirstOrDefault();
                post.CountComment++;

                db.Yorums.Add(yorum);
                db.SaveChanges();
                return Json(yorum);

            }
        }


        [HttpPost]
        public JsonResult AddCommandCommands(Yorum model)
        {
            var user = _usermanager.Users.Where(i => i.Id == _usermanager.GetUserId(User)).FirstOrDefault();
            using (var db = new Context())
            {

                var Anayorum = db.Yorums.Where(i => i.Id == model.MainYorumId).FirstOrDefault();


                var yorum = new Yorum()
                {
                    CountLike = 0,
                    CountDislike = 0,
                    Created_at = DateTime.Now,
                    Comment = model.Comment,
                    MainYorumId = model.MainYorumId,
                    Status = 1,
                    PostId = Anayorum.PostId,
                    UserImg = user.ProfilePhoto,
                    UserId = user.Id,
                    Usernmame = user.UserName
                };

                var post = db.Posts.Where(i => i.Id == Anayorum.PostId).FirstOrDefault();
                post.CountComment++;

                db.Yorums.Add(yorum);
                db.SaveChanges();

                return Json(yorum);

            }
        }

        public JsonResult GetCommands(int postId)
        {

            using (var db = new Context())
            {
                var comments = db.Yorums
              .Where(i => i.PostId == postId && i.MainYorumId == 0)
              .ToList();

                return Json(comments);
            }

        }
        public JsonResult GetCommandsChild(int commentId)
        {
            using (var db = new Context())
            {
                var comments = db.Yorums.Where(i => i.MainYorumId == commentId).ToList();

                return Json(comments);

            }
        }

        [HttpPost]
        public JsonResult PostLike(int id)
        {
            var user = _usermanager.Users.Where(i => i.Id == _usermanager.GetUserId(User)).FirstOrDefault();
            using (var db = new Context())
            {
                var chechk = db.PostLikes.Where(i => i.UserId == user.Id && i.PostId==id).Count();

                if (chechk == 0)
                {
                    var post = db.Posts.Where(i => i.Id == id).FirstOrDefault();
                    post.CountLike++;


                    var postLike = new PostLike()
                    {
                        PostId = id,
                        UserId = user.Id
                    };


                    db.Posts.Update(post);
                    db.PostLikes.Add(postLike);
                    db.SaveChanges();
                    return Json(post);

                }
                else
                {
                    var post = db.Posts.Where(i => i.Id == id).FirstOrDefault();
                    post.CountLike--;
                    db.Posts.Update(post);
                    var postLike = db.PostLikes.Where(i => i.PostId == id && i.UserId == user.Id).FirstOrDefault();
                    db.PostLikes.Remove(postLike);
                    db.SaveChanges();
                    return Json(post);
                    //burada başka kullanıcının postu beğenip beğenmediğini de kontrol etmemiz lazım

                }



            }



        }


        public JsonResult GetLikes()
        {
            var userId = _usermanager.GetUserId(User);

            using (var db = new Context())
            {
                var postLikes = db.PostLikes.Where(i => i.UserId == userId).ToList();

                return Json(postLikes);
            }
        }
    }
}
