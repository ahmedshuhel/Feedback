using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ComplaintBox.Web.Models
{
    public class Settings
    {
        public int Id { get; set; }
        public string SubjectTitle { get; set; }
        public Organization organaization { get; set; }
        public int OrganizationId { get; set; }
    }
}