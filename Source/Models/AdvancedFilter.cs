using System;

namespace ComplaintBox.Web.Models
{
    public class AdvancedFilter
    {
        public int TopicId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string Status { get; set; }
    }
}