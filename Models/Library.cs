using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StudentELibrary.Models
{
    public class Library
    {
        public int Id { get; set; }
        public virtual List<Resource> Resources { get; set; }
    }
}