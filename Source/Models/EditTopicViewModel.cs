namespace ComplaintBox.Web.Models
{
    public class EditTopicViewModel : AdminViewModel
    {
        public int TopicId { get; set; }
        public string TopicTitle { get; set; }
        public int OrgId { get; set; }
    }
}