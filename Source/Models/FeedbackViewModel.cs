namespace ComplaintBox.Web.Models
{
    public class FeedbackViewModel
    {

        public FeedbackViewModel()
        {
            IsAdditionalInfo = false;
        }

        public int OrganizationId { get; set; }
        public string Name { get; set; }
        public string Feedback { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }

        public bool IsAdditionalInfo { get; set; }
    }
}