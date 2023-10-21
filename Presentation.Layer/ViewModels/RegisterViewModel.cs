using System.ComponentModel.DataAnnotations;

namespace Presentation.Layer.ViewModels
{
	public class RegisterViewModel
	{
		[Required(ErrorMessage = "First Name is Required !")]
		[MinLength(3, ErrorMessage = "MinLength of First Name 3 letter")]
		[MaxLength(50, ErrorMessage = "MaxLength of First Name 50 letter")]
		public string fName { get; set; }

		[Required(ErrorMessage = "Last Name Required !")]
		[MaxLength(50, ErrorMessage = "MaxLength of Last Name 50 letter")]
		[MinLength(3, ErrorMessage = "MinLength of Last Name 3 letter")]
		public string lName { get; set; }

		[Required(ErrorMessage = "Email is Required !")]
		[EmailAddress(ErrorMessage = "Invalid Email !!")]
		public string Email { get; set; }

		[Required(ErrorMessage = "Password is Required !")]
		[DataType(DataType.Password)]
		public string Password { get; set; }

		[Compare("Password",ErrorMessage = "Confirm Password doesn't match Password")]
		[Required(ErrorMessage = "Confirm Password is Required !")]
		[DataType(DataType.Password)]
		public string ConfirmPassword { get; set; }

		public bool IsAgree { get; set; }
	}
}
