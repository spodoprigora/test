using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using test.Entities;
using System.Configuration;
using System.Data.SqlClient;
using test.ViewModel;
using test.Models;
using System.Data;


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

        public static IEnumerable<Message> GetMessages(int page, int pageSize)
        {
           var messages = new List<Message>();
            try
            {
                using (var connection = new SqlConnection(ConnectionString))
                {
                     connection.Open();

                     SqlCommand command = new SqlCommand(" SELECT Id, IdUser, Text, MessageDate, LikeCount from Messages ORDER BY Id DESC OFFSET(@PageSize * @Page) ROWS FETCH NEXT @PageSize ROWS ONLY", connection);
                     SqlParameter PageSize = new SqlParameter("PageSize", SqlDbType.Int);
                     PageSize.Value = pageSize;
                     command.Parameters.Add(PageSize);
                     SqlParameter Page = new SqlParameter("Page", SqlDbType.Int);
                     Page.Value = page;
                     command.Parameters.Add(Page);
 
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        var tempData = reader[3].ToString();
                        var dataArr = tempData.Split(' ');
                        var message = new Message
                        {
                            Id = (int)reader[0],
                            IdUser = (int)reader[1],
                            Text = (string)reader[2],

                            MessageDate = dataArr[0],
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

       public static int SaveMessage(string name, string Message, List<string> Links, int? Id = null)
       {
           int userId = 0;
           try
           {
               using (var connection = new SqlConnection(ConnectionString))
               {
                   connection.Open();
                   // Работа с базой данных
                    if(Id == null){
                        SqlCommand command = new SqlCommand("INSERT INTO Users (Name) VALUES (@Name)", connection);
                        SqlParameter Name = new SqlParameter("Name", SqlDbType.NVarChar);
                        if (name != null)
                            Name.Value = name;
                        else
                            Name.Value = "Аноним";
                        command.Parameters.Add(Name);

                        command.ExecuteNonQuery();

                        SqlCommand command1 = new SqlCommand(" SELECT MAX(Id) FROM users", connection);
                        userId = (int)command1.ExecuteScalar();
                    }
                    else                    
                        userId = (int)Id;
                  
           
                   SqlCommand command2 = new SqlCommand("INSERT INTO Messages (IdUser, Text, MessageDate, LikeCount) VALUES (@IdUser, @Text, GETDATE(), 0)", connection);
                   SqlParameter IdUser = new SqlParameter("IdUser", SqlDbType.Int);
                   IdUser.Value = userId;
                   command2.Parameters.Add(IdUser);
                   SqlParameter Text = new SqlParameter("Text", SqlDbType.NVarChar);
                   Text.Value = Message;
                   command2.Parameters.Add(Text);

                   command2.ExecuteNonQuery();


                   SqlCommand command3 = new SqlCommand("SELECT MAX(Id) FROM Messages", connection);
                   var messagesId = (int)command3.ExecuteScalar();

                  

                   foreach (var link in Links)
                   {
                       SqlCommand command4 = new SqlCommand("INSERT INTO Attachments (Attachment, IdMessage) VALUES (@Attachment, @IdMessage)", connection);
                       SqlParameter IdMessage = new SqlParameter("IdMessage", SqlDbType.Int);
                       IdMessage.Value = messagesId;
                       command4.Parameters.Add(IdMessage);
                       SqlParameter Attachment = new SqlParameter("Attachment", SqlDbType.NVarChar);
                       Attachment.Value = link;
                       command4.Parameters.Add(Attachment);
                       command4.ExecuteNonQuery();
                   }

                  connection.Close();
               }
           }
           catch (Exception ex)
           {
           }
           return userId;
         
       }

       public static bool IncLike(int id)
       {
           try
           {
               using (var connection = new SqlConnection(ConnectionString))
               {
                   connection.Open();
                   // Работа с базой данных

                   SqlCommand command = new SqlCommand("UPDATE Messages SET LikeCount = LikeCount + 1 WHERE Id= @Id", connection);
                   SqlParameter Id = new SqlParameter("Id", SqlDbType.Int);
                   Id.Value = id;
                   command.Parameters.Add(Id);
                   command.ExecuteNonQuery();

                   
                   
                   connection.Close();
               }
           }
           catch (Exception ex)
           {
               return false;
           }
           return true;
          
       }

       public static bool Dell(int id)
       {
           try
           {
               using (var connection = new SqlConnection(ConnectionString))
               {
                   connection.Open();
                   // Работа с базой данных

                   SqlCommand command = new SqlCommand("DELETE FROM Attachments WHERE IdMessage= @Idmess", connection);
                   SqlParameter Idmess = new SqlParameter("Idmess", SqlDbType.Int);
                   Idmess.Value = id;
                   command.Parameters.Add(Idmess);
                   command.ExecuteNonQuery();

                   SqlCommand command1 = new SqlCommand("DELETE FROM Messages WHERE Id = @Id", connection);
                   SqlParameter Id = new SqlParameter("Id", SqlDbType.Int);
                   Id.Value = id;
                   command1.Parameters.Add(Id);
                   command1.ExecuteNonQuery();


                   connection.Close();
               }
           }
           catch (Exception ex)
           {
               return false;
           }
           return true;
       }

       public static int GetTotalItemsCount()
       {
           int count = default(int);
           try
           {
               using (var connection = new SqlConnection(ConnectionString))
               {
                   connection.Open();

                   SqlCommand command = new SqlCommand("SELECT COUNT(*) FROM Messages", connection);
                   count = (int)command.ExecuteScalar();

                   connection.Close();
               }
           }
           catch (Exception ex)
           {
           }

           return count;
       }

    }
}