using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace test.Models
{
    public class AttachmentModel
    {
        public string Attachment { get; set; }

        public AttachmentModel(string link)
        {
            Attachment = link;
        }
      
    }
}