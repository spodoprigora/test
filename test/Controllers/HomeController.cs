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

            //var viewModel = new MessagesViewModel()
            //{
            //    Messages = msgs.Select(m => new Messages() { Text = m.Text }),
            //    Attachment = users.Select(u => new UserModel() { Name = u.Name })
            //};

            IEnumerable <MessageModel>  messList  = from m in msgs
                                  join u in users on m.IdUser equals u.Id
                                  select new MessageModel(m.Id, m.IdUser, u.Name, m.Text, m.MessageDate, m.LikeCount);
            
            List<MessagesViewModel> model = new List<MessagesViewModel>();
            foreach (MessageModel mess in messList){
                int mesId = mess.Id;
                IEnumerable<AttachmentModel> atach = from a in attachments
                                                     where a.IdMessage == mesId 
                                                     select new AttachmentModel(a.Link);
                var ViewModel = new MessagesViewModel
                {
                    Message = mess,
                    Attachment = atach
                };
                model.Add(ViewModel);
            }

          

             
              
           
           




           
            return View();
        }
    }
}