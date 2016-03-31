using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using test.Entities;
using test.Repositories;

namespace test.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            IEnumerable<Message> msgs = Repository.GetMessages();
            IEnumerable<User> users = Repository.GetUsers();
            IEnumerable<Attachment> attachments = Repository.GetAttachments();


            int hour = DateTime.Now.Hour;
            ViewBag.Greeting = hour < 12 ? "Доброе утро" : "Доброго дня";
            return View();
        }
    }
}