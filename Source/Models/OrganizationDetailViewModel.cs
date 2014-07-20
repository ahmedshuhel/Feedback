using System.Web.Mvc;

namespace ComplaintBox.Web.Models
{
    public class OrganizationDetailViewModel : AdminViewModel
    {
        [HiddenInput]
        public int Id { get; set; }
        public string OrganizationName { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }

    }
}