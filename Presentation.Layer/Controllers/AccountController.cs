using Data.Access.Layer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Presentation.Layer.Helpers;
using Presentation.Layer.ViewModels;
using System.Threading.Tasks;

namespace Presentation.Layer.Controllers
{
    public class AccountController : Controller
    {
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly SignInManager<ApplicationUser> _signInManager;

		//Ask ClR to Create object from UserManager and SignInManager
		public AccountController(UserManager<ApplicationUser> userManager ,SignInManager<ApplicationUser> signInManager)
        {
			_userManager = userManager;
            _signInManager = signInManager;
		}

        //--------------------------------------------
        #region Register

        // only to return view of Register
        // /Account/Register
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
		public async Task<IActionResult> Register(RegisterViewModel registerVM)
		{
            if(ModelState.IsValid) //server side validation
            {
                var user = new ApplicationUser()
                {
                    fName= registerVM.fName,
                    lName= registerVM.lName,
					UserName = registerVM.Email.Split('@')[0],
                    Email = registerVM.Email,
                    IsAgree = registerVM.IsAgree,   
				};
                var result = await _userManager.CreateAsync(user, registerVM.Password);

                if (result.Succeeded)
                    return RedirectToAction(nameof(Login));
                
                foreach (var error in result.Errors)
                    ModelState.AddModelError(string.Empty, error.Description);
                
            }
			return View(registerVM);
		}

		#endregion

		#region LogIn
        public IActionResult Login()
        { 
            return View(); 
        }

        [HttpPost]
		public async Task<IActionResult> Login(LoginViewModel loginVM)
		{
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(loginVM.Email);
                if(user is not null)
                {
                    var check = await _userManager.CheckPasswordAsync(user, loginVM.Password);
                    if (check)
                    {
                      await _signInManager.PasswordSignInAsync(user, loginVM.Password, loginVM.RememberMe, false);
                        return RedirectToAction("Index","Home");
                    }
					ModelState.AddModelError(string.Empty, "Invalid Password");
				}
                ModelState.AddModelError(string.Empty, "Email is not Exsited");

			}
			return View(loginVM);
		}
        #endregion

        #region SignOut
        public new async Task<IActionResult> SignOut()
        {
            //remove token generated above from cookiee
            await _signInManager?.SignOutAsync();
            return RedirectToAction(nameof(Login));
        }
		#endregion

		#region Forget Password
        //get view for send email to reset password
		public  IActionResult ForgetPassword()
		{
			return View();
		}

        //send link to your email to reset your pass through it
        [HttpPost]
        public async Task<IActionResult> SendEmail(ForgetPasswordViewModel forgetPasswordVM)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(forgetPasswordVM.Email);
                if (user is not null)
                {
					//generate Token =>> valid for this user only one-time
                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);
					//generate Body of Email[contain url that will make user to go for "ResetPassword" View ]
					var passwordResetLink = Url.Action("ResetPassword", "Account", new {email= user.Email,token= token },Request.Scheme);
					//https://localhost:44362/Account/ResetPassword?email=pola@gmail.com&token=hnfhn0h525
					//---------------------
					//create email for user that will send to user 
					var Email = new Email()
                    {
                        Subject = "Reset Password",
						To = user.Email,
						Body = $"To Change Your Password, Go through his Link:: \n {passwordResetLink}"
                   };
					//SendEmail
					EmailSettings.sendEmail(Email);
                    //go to view "check your Inbox"
                    return RedirectToAction(nameof(checkyourInbox));
				}
				ModelState.AddModelError(string.Empty, "Email isn't Existed");
            }
			return View("ForgetPassword", forgetPasswordVM);
		}

		//go to view "check your Inbox"
		public IActionResult checkyourInbox()
        {
            return View();
        }
        #endregion

        #region Reset Password
        public IActionResult ResetPassword(string email,string token)
        {
            TempData["email"] = email;
            TempData["token"] = token;
			return View();
        }
        [HttpPost]
		public async Task<IActionResult> ResetPassword(ResetPasswordViewModel resetPasswordVM)
		{
          if(ModelState.IsValid)
            {
				string email = TempData["email"] as string;
				string token = TempData["token"] as string;
				var user = await _userManager.FindByEmailAsync(email);
				//------------------
				var result = await _userManager.ResetPasswordAsync(user, token, resetPasswordVM.NewPassword);
				if (result.Succeeded)
				{
					return RedirectToAction(nameof(Login));
				}
                foreach(var error in result.Errors)
                {
					ModelState.AddModelError(string.Empty, error.Description);
				}
			}
			return View(resetPasswordVM);
		}
		#endregion
	}
}
