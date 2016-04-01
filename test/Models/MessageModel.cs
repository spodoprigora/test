using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace test.Models
{
    public class MessageModel
    {
        public MessageModel(int Id, int IdUser, string Name, string Text, string Date, int LikeCount)
        {
            this.Id = Id;
            this.IdUser = IdUser;
            this.Name = Name;
            this.Text = Text;
            this.Date = Date;
            this.LikeCount = LikeCount;
        }
        public int Id { get; set; }
        public int IdUser { get; set; }
        public string Name { get; set; }
        public string Text { get; set; }
        public string Date { get; set; }
        public int LikeCount { get; set; }

    }
}