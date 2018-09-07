using Chat.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Chat.Controllers
{
    public class HomeController : Controller
    {
        [Authorize]
        public ActionResult Index()
        {
            List<User> users = null;
            using (Context Db = new Context())
            {
                users = new List<Models.User>(Db.Users);
            }
            return View(users);
        }
    }
}