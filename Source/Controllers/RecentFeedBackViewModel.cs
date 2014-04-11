using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ComplaintBox.Web.Controllers
{
    public class RecentFeedBackViewModel
    {
        public string Organization { get; set; }
        public IList<RecentFeedback> RecentFeedbacks { get; set; }
    }


    public class RecentFeedback
    {
        public string Subject { get; set; }
        public string Feedback { get; set; }
        public DateTime FeedbackDate { get; set; }
        public string CustomerName { get; set; }    
    }
}
