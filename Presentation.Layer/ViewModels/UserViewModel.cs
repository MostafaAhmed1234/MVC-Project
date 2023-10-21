using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Presentation.Layer.ViewModels
{
	public class UserViewModel
	{
		//string => because class Identity User have Id property with string type
		public string Id { get; set; }
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
		public string PhoneNumber { get; set; }

		//each user has some roles
		public IEnumerable<string> Roles { get; set; }

        //----------------------------------------------------------
        //---if you will create user, you must create id like Identity User class-----
        public UserViewModel()
        {
			Id =Guid.NewGuid().ToString();
		}
    }
}
