using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ComplaintBox.Web.Models
{
    public class Subject
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public  Organization Organization { get; set; }
        public int OrganizationId { get; set; }        
    }
}