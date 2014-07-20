using System.Collections.Generic;

namespace ComplaintBox.Web.Models
{
    public class DashboardViewModel : AdminViewModel
    {
        public IList<FeedBackViewModel> AllFeedbacks { get; set; }
        public IList<FeedBackViewModel> Resolved { get; set; }
        public IList<FeedBackViewModel> Pendings { get; set; }

    }
}