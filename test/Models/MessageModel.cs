using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace test.Models
{
    public class MessageModel
    {
        int Id { get; set; }
        public int IdUser { get; set; }
        public string Text { get; set; }
        public int IdAttachment { get; set; }
        public string Data { get; set; }
        public int LikeCount { get; set; }

    }
}