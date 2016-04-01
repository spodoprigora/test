using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace test.Models
{
    public class AttachmentModel
    {
        public AttachmentModel(string link)
        {
            this.Attachment = link;
        }
        //  public int Id { get; set; }
        public string Attachment { get; set; }
       // public int IdMessage { get; set; }
    }
}