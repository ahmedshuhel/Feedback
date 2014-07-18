namespace ComplaintBox.Web.Models
{
    public  class AdminViewModel
    {

        public AdminViewModel()
        {
            SubjectTitle = "Topic";
        }

        public string UserName { get; set; }
        public int NumberOfPendingTopics { get; set; }
        public int NumbeOfResolvedFeedbacksToday { get; set; }
        public int OrganizationId { get; set; }
        public string SubjectTitle { get; set; }
    }
}