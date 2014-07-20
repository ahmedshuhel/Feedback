using System.Collections.Generic;
using ComplaintBox.Web.Controllers;

namespace ComplaintBox.Web.Models
{
    public class RecentFeedBackViewModel
    {
        public string Organization { get; set; }
        public IList<RecentFeedback> RecentFeedbacks { get; set; }
    }
}