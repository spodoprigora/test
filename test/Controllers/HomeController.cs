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
using test.ResolveServerUrl;


namespace test.Controllers
{
    public class HomeController : Controller
    {
        protected int PageSize = 5;
        // GET: Home
        public ActionResult Index(int page = 0)
        {
            IEnumerable<Message> msgs = Repository.GetMessages(page, PageSize);
            IEnumerable<User> users = Repository.GetUsers();
            IEnumerable<Attachment> attachments = Repository.GetAttachments();
            var totalMessagesCount = Repository.GetTotalItemsCount();
            List<MessageWithAttachemtns> model = Repository.GetViewModel(msgs, users, attachments);

            // получаем куки
            var cookie = Request.Cookies["name"];
            if (cookie != null)
            {
                var cookieVal = cookie.Value;
                ViewBag.userId = Convert.ToInt32(cookieVal);
            }

            var viewModel = new MessagesViewModel
            {
                Messages = model,
                Paging = new PagingModel(page, PageSize, totalMessagesCount)
            };
           if (Request.IsAjaxRequest())
                return PartialView("MessageDetails", viewModel);
           else
               return View(viewModel);
       }

        [HttpPost]
        public ActionResult SaveMessage(string Name, string Message, IEnumerable<string> Link)
        {
            List<string> attachList = new List<string>();
            foreach (var att in Link)
            {
                attachList.Add(att);
            }

            foreach (string file in Request.Files)
            {
                var upload = Request.Files[file];
                if (upload != null && upload.FileName != "")
                {
                    // получаем имя файла
                    string fileName = System.IO.Path.GetFileName(upload.FileName);
                    var nameArr = fileName.Split('.');
                    string AbsolurFile = nameArr[0] + "~" + Guid.NewGuid() + "." + nameArr[1];
                    string path = Server.MapPath("~/UploadFiles/" + AbsolurFile);
                    // сохраняем файл в папку UploadFiles в проекте
                    upload.SaveAs(path);
                    string ServerPath = Resolve.ResolveServerUrl(VirtualPathUtility.ToAbsolute("~/UploadFiles/" + AbsolurFile), false);
                    attachList.Add(ServerPath);
                }
            }






            var cookie = Request.Cookies["name"];
            if (cookie != null)
            {
                var cookieVal = Convert.ToInt32(cookie.Value);
                int userId = Repository.SaveMessage(Name, Message, attachList, cookieVal);
            }
            else
            {
                var userId = Repository.SaveMessage(Name, Message, attachList);
                {
                    cookie = new HttpCookie("name", userId.ToString());
                    cookie.Expires = DateTime.Now.AddDays(10);
                    Response.SetCookie(cookie);
                }
            }







            return new HttpStatusCodeResult(200);
        }

        [HttpPost]
        public ActionResult DellMessage(int messadgeId)
        {
            bool res = Repository.Dell(messadgeId);
            if (res)
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