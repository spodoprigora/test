using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using test.Models;

namespace test.ViewModel
{
    public class MessageWithAttachemtns
    {
        public MessageModel Message { get; set; }
        public IEnumerable<AttachmentModel> Attachment { get; set; }
    }
}