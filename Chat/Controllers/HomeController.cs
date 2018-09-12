using Chat.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace Chat.Controllers
{
    //Попасть в этот контроллер могут только авторизованные пользователи
    [Authorize]
    public class HomeController : Controller
    {
        //Выводим список всех зарегистрированных пользователей в чате, кроме себя самого
        public ActionResult Index()
        {
            List<User> users = null;
            using (Context Db = new Context())
            {
                //Формируем список пользователей из базы
                users = new List<Models.User>(Db.Users);

                //Ищем в нем себя
                User A = users.FirstOrDefault(a => a.Login == User.Identity.Name);
                //Удаляем себя
                users.Remove(A);
            }
            //Выводим полученный список на экран
            return View(users);
        }

        //Окно выведения информации об ошибке
        public ActionResult Error(string message, string head)
        {
            //Формируем модель ошибки
            ErrorModel M = new ErrorModel { Head = head, Message = message };
            //Выводим
            return View(M);
        }

        //Открывает страницу диалога, где Id - идентификатор пользователя, с которым базарим
        public ActionResult Dialog(int Id)
        {
            DialogModel dialog = null;
            using (Context Db = new Context())
            {
                User A = Db.Users.FirstOrDefault(a => a.Id == Id);
                User Me1 = Db.Users.FirstOrDefault(b => b.Login == User.Identity.Name);

                //Если пользователя не нашли (он удалился) - выводим окно ошибки
                if (A == null) return RedirectToAction("Index", new { message = "Такого пользователя не существует или он удалил свою страницу", head = "Ошибка" });

                //Объявляем общую коллекцию
                List<Message> messages;

                //Отбираем сообщения, которые отправил я
                List<Message> MyMessage = Db.Messages.Include(x=>x.Author).Include(x=>x.Recipient).Where(y => y.AuthorId == Me1.Id).ToList();
                //Отбираем из них те, которые предназначены выбранному пользователю
                MyMessage = MyMessage.Where(k => k.RecipientId == A.Id).ToList();


                //Отбираем сообщения, отправленные выбранным пользователем
                List<Message> FriendMessage = Db.Messages.Include(x => x.Author).Include(x => x.Recipient).Where(y => y.AuthorId == A.Id).ToList();
                //Отбираем из них те, которые предназначены мне
                FriendMessage = FriendMessage.Where(k => k.RecipientId == Me1.Id).ToList();


                //Склеиваем обе коллекции в одну
                messages = MyMessage;
                messages.AddRange(FriendMessage);
                //Сортируем по дате, чтобы красиво все было
                messages = messages.OrderBy(x => x.Date).ToList();

                //Формируем модель диалога
                dialog = new DialogModel { Friend = A, Messages = messages, Me = Me1 };
            }
            //Выводим диалог
            return View(dialog);
        }


        //Метод отправления сообщения пользователю, возвращает обратно в диалог к нему
        [HttpPost]
        public ActionResult SendMessage(string mess_text, int friend_id, int my_id)
        {
            //Формируем сообщение
            Message mess = new Message { AuthorId = my_id, RecipientId = friend_id, Text = mess_text, Date = DateTime.Now };
            using (Context Db = new Context())
            {
                //Добавляем его в базу
                Db.Messages.Add(mess);
                //Сохраняем изменения
                Db.SaveChanges();
            }
            //Выкидываем обратно в диалог
            return RedirectToAction("Dialog", new { Id = friend_id });
        }

    }
}