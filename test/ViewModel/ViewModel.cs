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
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }

        public int TotalItems { get; set; }

        public int TotalPages { get { return (int)Math.Ceiling((double)TotalItems / PageSize); } }

        public PagingModel(int page, int pageSize, int totalMessagesCount)
        {
            CurrentPage = page;
            PageSize = pageSize;
            TotalItems = totalMessagesCount;
        }
    }


}