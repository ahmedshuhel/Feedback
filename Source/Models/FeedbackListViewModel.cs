using System;
using MvcPaging;

namespace ComplaintBox.Web.Models
{
    public class FeedbackListViewModel : AdminViewModel
    {
        public IPagedList<FeedBackViewModel> FeedBackViewModels { get; set; }
    }

    public class FeedBackViewModel
    {
        public string FeedbackUser { get; set; }
        public string Email { get; set; }
        public DateTime FeedbackDate { get; set; }
        public string Feedback { get; set; }
        public string Status { get; set; }
        public string Topic { get; set; }
        public int FeedbackId { get; set; }
    }



    public class FeedbackDetailsViewModel : AdminViewModel
    {
        public string FeedbackUser { get; set; }
        public string Email { get; set; }
        public DateTime FeedbackDate { get; set; }
        public string Feedback { get; set; }
        public string Status { get; set; }
        public string Topic { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }

        public string Comment { get; set; }
        public int FeedbackId { get; set; }
    }
}