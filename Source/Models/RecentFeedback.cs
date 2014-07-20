using System;

namespace ComplaintBox.Web.Models
{
    public class RecentFeedback
    {
        public string Subject { get; set; }
        public string Feedback { get; set; }
        public DateTime FeedbackDate { get; set; }
        public string CustomerName { get; set; }    
    }
}
