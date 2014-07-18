using System.Collections.Generic;

namespace ComplaintBox.Web.Models
{
    public class ListTopicViewModel : AdminViewModel
    {
       public IList<Topic> Topics { get; set; } 
    }



    public class Topic
    {
        public int TopicId { get; set; }
        public string TopicTitle { get; set; }
        public int OrgId { get; set; }
    }

}