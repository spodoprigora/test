using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using test.Entities;
using test.Repositories;
using test.ViewModel;
using test.Models;
using System.IO;

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

            // получаем куки
            var cookie = Request.Cookies["name"];
            if (cookie != null)
            {
                var cookieVal = cookie.Value;
                ViewBag.userId = Convert.ToInt32(cookieVal);
           }
            

            if (Request.IsAjaxRequest())
            {
                return PartialView("MessageDetails", model);
            }
            else
            {
                var viewModel = new MessagesViewModel()
                {
                    Messages = model,
                    Paging = new PagingModel()
                };
                return View(viewModel);
            }

        }

        [HttpPost]
       public ActionResult SaveMessage(string Name, string Message, string Link)
        {
            List<string> attachList = new List<string>();
            attachList.Add(Link);
            foreach (string file in Request.Files)
            {
               var upload = Request.Files[file];
               if (upload != null && upload.FileName!="")
               {
                 // получаем имя файла
                 string fileName = System.IO.Path.GetFileName(upload.FileName);
                 var nameArr = fileName.Split('.');
                 string path = Server.MapPath("~/UploadFiles/" + nameArr[0] + "~" + Guid.NewGuid() + "." + nameArr[1]);
                 // сохраняем файл в папку Files в проекте
                 upload.SaveAs(path);
                 attachList.Add(path);
               }
 
            }
            int userId = Repository.SaveMessage(Name, Message, attachList);
            if (Name != null)
            {
                var cookie = new HttpCookie("name", userId.ToString());
                cookie.Expires = DateTime.Now.AddDays(10);
                Response.SetCookie(cookie);
            }
            //return new HttpStatusCodeResult(200);
            return null;
        }

    [HttpPost]
        public ActionResult DellMessage(int Id)
        {
            bool res = Repository.Dell(Id);
        if(res)
            return new HttpStatusCodeResult(200);
        else
            return new HttpStatusCodeResult(400);
   
        }

        [HttpPost]
    public ActionResult LikeInc(int messadgeId)
        {

            bool res = Repository.IncLike(messadgeId);
            if (res)
                return new HttpStatusCodeResult(200);
            else
                return new HttpStatusCodeResult(400);
        }
       

    }
}