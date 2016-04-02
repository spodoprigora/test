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
            List<MessageWithAttachemtns> model = Repository.GetViewModel(msgs, users, attachments);

            var viewModel = new MessagesViewModel()
            {
                Messages = model,
                Paging = new PagingModel()
            };


            return View(viewModel);
        }

        [HttpPost]
        public ActionResult SaveMessage(string Name, string Message, string Link )
        {
            if(Name!=null){
                var cookie = new HttpCookie()
                {
                    Name = "name",
                    Value = Name,
                    Expires = DateTime.Now.AddMinutes(10)
                };
                Response.SetCookie(cookie);
            }
           
       
       
        //// получаем куки
        //    var cookie1 = Request.Cookies["test_cookie"];



            return View();
        }
        public ActionResult DellMessage()
        {
           
            return View();
        }

        [HttpPost]
        public JsonResult Upload()
        {
            string fileList = null;
            foreach (string file in Request.Files)
            {
                var upload = Request.Files[file];
                
                if (upload != null)
                {
                    // получаем имя файла
                    string fileName = System.IO.Path.GetFileName(upload.FileName);
                    // сохраняем файл в папку Files в проекте
                    upload.SaveAs(Server.MapPath("~/TempUploadFiles/" + fileName));
                    fileList += fileName + ",";
                }
            }
            return Json(fileList);
        }

    }
}