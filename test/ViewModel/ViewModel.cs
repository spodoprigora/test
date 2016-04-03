using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using test.Models;

namespace test.ViewModel
{
    public class MessagesViewModel
    {
        public List<MessageWithAttachemtns> Messages { get; set; }
        public PagingModel Paging { get; set; }
    }

    public class MessageWithAttachemtns
    {
        public MessageModel Message { get; set; }
        public IEnumerable<AttachmentModel> Attachment { get; set; }
    }
    public class PagingModel
    {
        public int PageSize { get; set; }

        public int TotalItemCount { get; set; }

        public int PageNumber { get; set; }
    }


}