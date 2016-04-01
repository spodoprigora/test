using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using test.Entities;
using test.Repositories;
using test.ViewModel;
using test.Models;

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
            List<MessagesViewModel> model = Repository.GetViewModel(msgs, users,  attachments);



            return View(model);
        }
    }
}