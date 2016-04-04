using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using test.Entities.Abstract;

namespace test.Entities
{
    public class User : EntityBase
    {
        public string Name { get; set; }
    }
}