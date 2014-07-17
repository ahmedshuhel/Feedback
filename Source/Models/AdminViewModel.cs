namespace ComplaintBox.Web.Models
{
    public  class AdminViewModel
    {
        public string UserName { get; set; }
        public int NumberOfPendingTopics { get; set; }
        public int NumbeOfResolvedFeedbacksToday { get; set; }
    }
}