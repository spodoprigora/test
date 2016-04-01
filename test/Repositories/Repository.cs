using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using test.Entities;
using System.Configuration;
using System.Data.SqlClient;
using test.ViewModel;
using test.Models;



namespace test.Repositories
{
    public static class Repository
    {

        private static readonly string ConnectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        public static IEnumerable<User> GetUsers()
        {
            var users = new List<User>();
            try
            {
                using (var connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();
                    // Работа с базой данных

                    SqlCommand command = new SqlCommand("SELECT Id, Name FROM users", connection);
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        var user = new User
                        {
                            Id = (int)reader[0],
                            Name = (string)reader[1]
                        };
                        users.Add(user);

                    }
                    reader.Close();
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
            }

            return users;
        }

        public static IEnumerable<Message> GetMessages()
        {
           var messages = new List<Message>();
            try
            {
                using (var connection = new SqlConnection(ConnectionString))
                {
                     connection.Open();
                    // Работа с базой данных

                    SqlCommand command = new SqlCommand("SELECT Id, IdUser, Text, MessageDate, LikeCount Name FROM Messages", connection);
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        var message = new Message
                        {
                            Id = (int)reader[0],
                            IdUser = (int)reader[1],
                            Text = (string)reader[2],
                            MessageDate = reader[3].ToString(),
                            LikeCount = (int)reader[4]
                        };
                       messages.Add(message);
                    }
                    reader.Close();
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
            }

            return messages;
        }

        public static IEnumerable<Attachment> GetAttachments()
        {
            var attachments = new List<Attachment>();
            try
            {
                using (var connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();
                    // Работа с базой данных

                    SqlCommand command = new SqlCommand("SELECT Id, Attachment, IdMessage FROM Attachments", connection);
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        var attachment = new Attachment {
                            Id = (int)reader[0],
                            Link =(string)reader[1],
                            IdMessage = (int)reader[2]
                        };
                        attachments.Add(attachment);
                    }
                    reader.Close();
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
            }

            return attachments;
        }

        //public static List<MessagesViewModel> GetViewModel(IEnumerable<Message> msgs, IEnumerable<User> users, IEnumerable<Attachment> attachments){
        //    IEnumerable<MessageModel> messList = from m in msgs
        //                                         join u in users on m.IdUser equals u.Id
        //                                         select new MessageModel(m.Id, m.IdUser, u.Name, m.Text, m.MessageDate, m.LikeCount);

        //    List<MessagesViewModel> model = new List<MessagesViewModel>();

        //    foreach (MessageModel mess in messList)
        //    {
        //        int mesId = mess.Id;

        //        IEnumerable<AttachmentModel> atach = from a in attachments
        //                                             where a.IdMessage == mesId
        //                                             select new AttachmentModel(a.Link);

        //        var ViewModel = new MessagesViewModel
        //        {
        //            Message = mess,
        //            Attachment = atach.ToList()
        //        };
        //        model.Add(ViewModel);
        //    }

        //    return model;
        //}


        public static List<MessageWithAttachemtns> GetViewModel(IEnumerable<Message> msgs, IEnumerable<User> users, IEnumerable<Attachment> attachments)
        {
            IEnumerable<MessageModel> messList = from m in msgs
                                                 join u in users on m.IdUser equals u.Id
                                                 select new MessageModel(m.Id, m.IdUser, u.Name, m.Text, m.MessageDate, m.LikeCount);

            List<MessageWithAttachemtns> model = new List<MessageWithAttachemtns>();

            foreach (MessageModel mess in messList)
            {
                int mesId = mess.Id;

                IEnumerable<AttachmentModel> atach = from a in attachments
                                                     where a.IdMessage == mesId
                                                     select new AttachmentModel(a.Link);

                var ViewModel = new MessageWithAttachemtns
                {
                    Message = mess,
                    Attachment = atach.ToList()
                };
                model.Add(ViewModel);
            }

            return model;
        }


    }
}