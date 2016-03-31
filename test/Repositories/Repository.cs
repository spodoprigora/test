using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using test.Entities;
using System.Configuration;
using System.Data.SqlClient;



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

    }
}