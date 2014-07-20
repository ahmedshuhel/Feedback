using System.Collections.Generic;
using System.Web.Mvc;

namespace ComplaintBox.Web.Models
{
    public class FeedbackViewModel
    {

        public FeedbackViewModel()
        {
            IsAdditionalInfo = false;
            TopicTitle = "Topic";
        }

        public int OrganizationId { get; set; }
        public string Name { get; set; }
        public string Feedback { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public int TopicId { get; set; }
        public bool IsAdditionalInfo { get; set; }

        public IEnumerable<SelectListItem> Subjects { get; set; }
        public string TopicTitle { get; set; }
    }
}