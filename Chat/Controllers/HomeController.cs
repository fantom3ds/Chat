using Chat.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace Chat.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            List<User> users = null;
            using (Context Db = new Context())
            {
                users = new List<Models.User>(Db.Users);

                User A = users.FirstOrDefault(a => a.Login == User.Identity.Name);
                users.Remove(A);
            }
            return View(users);
        }

        public ActionResult Error(string message, string head)
        {
            ErrorModel M = new ErrorModel { Head = head, Message = message };
            return View(M);
        }

        public ActionResult Dialog(int Id)
        {
            DialogModel dialog = null;
            using (Context Db = new Context())
            {
                User A = Db.Users.FirstOrDefault(a => a.Id == Id);
                User Me1 = Db.Users.FirstOrDefault(b => b.Login == User.Identity.Name);

                if (A == null) return RedirectToAction("Index", new { message = "Такого пользователя не существует или он удалил свою страницу", head = "Ошибка" });

                List<Message> messages;

                List<Message> MyMessage = Db.Messages.Include(x=>x.Author).Include(x=>x.Recipient).Where(y => y.AuthorId == Me1.Id).ToList();
                MyMessage = MyMessage.Where(k => k.RecipientId == A.Id).ToList();

                List<Message> FriendMessage = Db.Messages.Include(x => x.Author).Include(x => x.Recipient).Where(y => y.AuthorId == A.Id).ToList();
                FriendMessage = FriendMessage.Where(k => k.RecipientId == Me1.Id).ToList();

                messages = MyMessage;
                messages.AddRange(FriendMessage);
                messages = messages.OrderBy(x => x.Date).ToList();

                dialog = new DialogModel { Friend = A, Messages = messages, Me = Me1 };
            }

            return View(dialog);
        }



        [HttpPost]
        public ActionResult SendMessage(string mess_text, int friend_id, int my_id)
        {
            Message mess = new Message { AuthorId = my_id, RecipientId = friend_id, Text = mess_text, Date = DateTime.Now };
            using (Context Db = new Context())
            {
                Db.Messages.Add(mess);
                Db.SaveChanges();
            }
            return RedirectToAction("Dialog", new { Id = friend_id });
        }

    }
}