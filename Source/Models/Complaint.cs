using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ComplaintBox.Web.Models
{
    public class Complaint
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Name")]
        public string Complainer { get; set; }
        
        [Display(Name = "Phone")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Address")]
        public string Address { get; set; }

        [Required]
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        public string EmailAddress { get; set; }

        public string Comment { get; set; }

        public Subject Subject { get; set; }

        [Display(Name = "Subject")]
        [Required]
        public int SubjectId { get; set; }

        public Organization Organization { get; set; }

        [Display(Name = "Organization")]
        [Required]
        public int OrganizationId { get; set; }

        [Required]
        [Display(Name = "Your Complain")]        
        [StringLength(100, MinimumLength = 10, ErrorMessage=  "Your Complain must be atleast 10 character long and maximum of 100 characters.")]
        public string Description { get; set; }

        [Required]   
        [Range(1,10,ErrorMessage=  "please Rate this Service between 10. ")]
        public int Rating { get; set; }

        public DateTime ComplainDate { get; set; }

        public string Status { get; set; }
        
    }
}