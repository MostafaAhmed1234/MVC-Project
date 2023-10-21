using AutoMapper;
using Data.Access.Layer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Presentation.Layer.Helpers;
using Presentation.Layer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Presentation.Layer.Controllers
{
    [Authorize(Roles ="Admin")]
    public class UserController : Controller
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IMapper _mapper;

        public UserController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,IMapper mapper)
        {
			_userManager = userManager;
			_signInManager = signInManager;
            _mapper = mapper;
        }
		//-------------------------
		//admin who can use this view
		public async Task<IActionResult> Index(string emailsearchValue)
		{
			if (string.IsNullOrEmpty(emailsearchValue))
			{
				var users = await _userManager.Users.Select(U=> new UserViewModel()
				{
					Id = U.Id,
					fName =U.fName,
					lName =U.lName,
					Email =U.Email,
					PhoneNumber =U.PhoneNumber,
					Roles = _userManager.GetRolesAsync(U).Result // to work syncronous, becauce we can't use await inside lamda ex.
				}).ToListAsync();
				return View(users);
			}
			else
			{
				var user= await _userManager.FindByEmailAsync(emailsearchValue);
				var mappedUsr = new UserViewModel()
				{
					Id = user.Id,
					fName = user.fName,
					lName = user.lName,
					Email = user.Email,
					PhoneNumber = user.PhoneNumber,
					Roles = _userManager.GetRolesAsync(user).Result
				};
				//IEnumerable<UserViewModel> mappedUsrsModels = new List<UserViewModel>()
				//{
				//	mappedUsr
				//};
				return View(/*mappedUsrsModels*/new List<UserViewModel>(){mappedUsr});
			}
        }
        //-------------------------------------------------

        // /User/Details/Guid
        // /User/Details
        //[HttpGet]
        public async Task<IActionResult> Details(string id, string ViewName = "Details")
        {
            if (id is null)
                return BadRequest();
            //  HTTP status code is 400. The status code 400 indicates a client error
            var user = await _userManager.FindByIdAsync(id);
            if (user is null)
                return NotFound();
			//HTTP status code 404.The status code 404 indicates that the requested resource was not found on the server
			//-------
			var mappedUser = _mapper.Map<ApplicationUser, UserViewModel>(user);
            return View(ViewName, mappedUser);
        }
        //================================================
        // /User/Edit/Guid
        //[HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            return await Details(id, "Edit");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] string id, UserViewModel userUpdatedVM)
        {
            if (id != userUpdatedVM.Id)
                return BadRequest();

            if (ModelState.IsValid)
            {
                try
                {
                    //this way not work, because this object from ApplicationUser not tracked
                    //,, so the solution get the tracked object from database then update it
                    //var mappedEmp = _mapper.Map<UserViewModel, ApplicationUser>(userUpdatedVM);
                    //-------
                    var user = await _userManager.FindByIdAsync(id);
                    user.fName = userUpdatedVM.fName;
                    user.lName = userUpdatedVM.lName;
                    user.PhoneNumber = userUpdatedVM.PhoneNumber;
                    user.Email = userUpdatedVM.Email;
                    //SecurityStamp=>>a random value that must change, when change password or email
                    user.SecurityStamp=Guid.NewGuid().ToString();
                    
                    //-------
                    await _userManager.UpdateAsync(user);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            return View(userUpdatedVM);
        }
        //======================================
        // /Employee/Delete/1
        // /Employee/Delete
        //[HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            return await Details(id, "Delete");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmDelete(string id)
        {
            try
            {
                //var mappedEmp = _mapper.Map < UserViewModel, ApplicationUser> (userDeletedVM);
                //--------
                var user = await _userManager.FindByIdAsync(id);
                await _userManager.DeleteAsync(user);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return RedirectToAction("Error", "Home");
            }

        }
    }
}
