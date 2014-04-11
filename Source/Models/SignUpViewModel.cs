using System.ComponentModel.DataAnnotations;

namespace ComplaintBox.Web.Models
{
    public class SignUpViewModel
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password did not match")]
        public string ConfirmPassword { get; set; }
    }
}