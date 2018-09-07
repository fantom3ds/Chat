using Chat.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Chat.Controllers
{
    public class AccountController : Controller
    {
        #region Авторизация

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(AuthModel model)
        {
            if (ModelState.IsValid)
            {
                // поиск пользователя в бд
                User user = null;
                using (Context db = new Context())
                {
                    Guid pass = EncoderGuid.PasswordToGuid.Get(model.Password);
                    user = db.Users.FirstOrDefault(u => u.Login == model.Login && u.Password == pass);

                }
                if (user != null)
                {
                    FormsAuthentication.SetAuthCookie(model.Login, true);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Пользователя с таким логином и паролем нет");
                }
            }

            return View(model);
        }

        #endregion


        #region Регистрация. Если что - можно нахер выпилить
        public ActionResult Register()
        {
            return View();
        }

        //Post-метод для регистрации пользователя
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                User user = null;
                using (Context db = new Context())
                {
                    user = db.Users.FirstOrDefault(u => u.Login == model.Login);
                }
                if (user == null)
                {
                    // создаем нового пользователя
                    using (Context db = new Context())
                    {
                        Guid pass = EncoderGuid.PasswordToGuid.Get(model.Password);
                        db.Users.Add(new User { Login = model.Login, Password = pass, DateOfBirth = model.DateOfBirth, DateRegister = DateTime.Now, FirstName = model.FirstName, LastName = model.LastName, About = model.Hobbies, Hide = model.Hide });
                        db.SaveChanges();

                        user = db.Users.Where(u => u.Login == model.Login && u.Password == pass).FirstOrDefault();
                    }
                    // если пользователь удачно добавлен в бд
                    if (user != null)
                    {
                        FormsAuthentication.SetAuthCookie(model.Login, true);
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Пользователь с таким логином уже существует");
                }
            }

            return View(model);
        }

        public ActionResult Logoff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }

        #endregion
    }
}