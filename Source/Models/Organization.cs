using System.ComponentModel.DataAnnotations;

namespace ComplaintBox.Web.Models
{
    public class Organization
    {
        public int Id { get; set; }        
        public string FullName { get; set; }
        public string UserName { get; set; }                
        public string PhoneNumber { get; set;}        
        public string EmailAddress { get; set; }
        public string Address { get; set; }

    }
}