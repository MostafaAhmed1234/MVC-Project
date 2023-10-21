using System.ComponentModel.DataAnnotations;

namespace Presentation.Layer.ViewModels
{
    public class ResetPasswordViewModel
    {

        [Required(ErrorMessage = "Password is Required !")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Compare("NewPassword", ErrorMessage = "Confirm Password doesn't match Password")]
        [Required(ErrorMessage = "Confirm Password is Required !")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}
