using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using test.Entities.Abstract;

namespace test.Entities
{
    public class Attachment : EntityBase
    {
        public string Link { get; set; }
        public int IdMessage { get; set; }
    }
}