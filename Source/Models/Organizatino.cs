using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ComplaintBox.Web.Models
{
    public class Organization
    {
        public int Id { get; set; }
        
        [Required]
        [Display(Name = "Full Name")]
        public string FullName { get; set; }

        [Display(Name = "Phone")]
        public string PhoneNumber { get; set;}
        
        [Required]
        [Display(Name = "User Name")]
        [MinLength(8, ErrorMessage= "User Name must be 8 character long.")]
        public string UserName { get; set; }
                
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        [Required]
        public string EmailAddress { get; set; }

        [Compare("ComfirmPasswrod")]
        [DataType(DataType.Password)]        
        [Required]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Required]
        public string ComfirmPasswrod { get; set; }

    }
}