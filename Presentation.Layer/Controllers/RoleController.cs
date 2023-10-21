using Data.Access.Layer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Presentation.Layer.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;

namespace Presentation.Layer.Controllers
{
    [Authorize(Roles = "Admin")]
    //[Authorize(Roles = ("Admin,HR"))]
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;

        public RoleController(RoleManager<IdentityRole> roleManager,IMapper mapper,UserManager<ApplicationUser> userManager)
        {
            _roleManager = roleManager;
            _mapper = mapper;
            _userManager = userManager;
        }
        //--------------------------------
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(RoleViewModel roleVM)
        {
            if (ModelState.IsValid)
            {
                var mappedRole = _mapper.Map<RoleViewModel,IdentityRole>(roleVM);
                 await _roleManager.CreateAsync(mappedRole);
                return RedirectToAction("Index");
            }
            return View(roleVM);
        }
        //--------------------------------
        public async Task<IActionResult> Index(string nameSearchValue)
        {
            if (string.IsNullOrEmpty(nameSearchValue))
            {
                var roles = await _roleManager.Roles.Select(R => new RoleViewModel()
                {
                    Id = R.Id,
                    RoleName = R.Name
                }).ToListAsync();
                return View(roles);
            }
            else
            {
                var role = await _roleManager.FindByNameAsync(nameSearchValue);
                if(role is not null)
                {
                    var mappedRole = new RoleViewModel()
                    {
                        Id = role.Id,
                        RoleName = role.Name
                    };
                    //IEnumerable<RoleViewModel> mappedRolesModels = new List<RoleViewModel>()
                    //{
                    //	mappedRole
                    //};
                    return View(/*mappedRolesModels*/new List<RoleViewModel>() { mappedRole });
                }
              return View(Enumerable.Empty<RoleViewModel>());
            }
        }
        //-------------------------------------------------

        // /Role/Details/Guid
        // /Role/Details
        //[HttpGet]
        public async Task<IActionResult> Details(string id, string ViewName = "Details")
        {
            if (id is null)
                return BadRequest();
            //  HTTP status code is 400. The status code 400 indicates a client error
            var role = await _roleManager.FindByIdAsync(id);
            if (role is null)
                return NotFound();
            //HTTP status code 404.The status code 404 indicates that the requested resource was not found on the server
            //-------
            var mappedRloe = _mapper.Map<IdentityRole, RoleViewModel>(role);
            return View(ViewName, mappedRloe);
        }
        //================================================
        // /Role/Edit/Guid
        //[HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            return await Details(id, "Edit");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] string id, RoleViewModel roleUpdatedVM)
        {
            if (id != roleUpdatedVM.Id)
                return BadRequest();

            if (ModelState.IsValid)
            {
                try
                {
                    var role = await _roleManager.FindByIdAsync(id);
                    role.Name = roleUpdatedVM.RoleName;
                    //-------
                    await _roleManager.UpdateAsync(role);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            return View(roleUpdatedVM);
        }
        //======================================
        // /Role/Delete/Guid
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
                var role = await _roleManager.FindByIdAsync(id);
                await _roleManager.DeleteAsync(role);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return RedirectToAction("Error", "Home");
            }

        }
        //=====================Add or Remove Users in Role====================
        public async Task<IActionResult> AddOrRemoveUsers(string RoleId)
        {
            //1.get Role by RoleID and send it for return view
            var role = await _roleManager.FindByIdAsync(RoleId);
            ViewData["RoleId"] = RoleId;
            //2.Check Role if null
            if (role is  null) 
                return NotFound();
            //3.create list from UserInRoleViewModel
            ICollection< UserInRoleViewModel > usersVM = new List<UserInRoleViewModel>();
            //4.loop in all users in data base 
            foreach(var user in _userManager.Users)
            {
                //5.mapping ApplicationUser to UserInRoleViewModel
                var UserInRoleVM = new UserInRoleViewModel()
                {
                    UserId = user.Id,
                    UserName = user.UserName,
                };
                //6.check in each user has this Role and put value in IsSelected property
                if (await _userManager.IsInRoleAsync(user, role.Name))
                    UserInRoleVM.IsSelected = true;
                else
                    UserInRoleVM.IsSelected = false;
                //7.Add user after mapped in ist from UserInRoleViewModel
                usersVM.Add(UserInRoleVM);
            }

            return View(usersVM);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrRemoveUsers(List<UserInRoleViewModel> models, string RoleId)
        {
            //1.get Role by RoleID
            var role = await _roleManager.FindByIdAsync(RoleId);
            //2.Check Role if null
            if (role is null)
                return BadRequest();
            //3.check validation for models
            if (ModelState.IsValid)
            {
                foreach (var item in models)
                {
                    //4.Get User by UserId
                    var user = await _userManager.FindByIdAsync(item.UserId);
                    if (user is not null)
                    {
                        if (item.IsSelected && !(await _userManager.IsInRoleAsync(user, role.Name)))
                            await _userManager.AddToRoleAsync(user, role.Name);
                        else if (!item.IsSelected && (await _userManager.IsInRoleAsync(user, role.Name)))
                            await _userManager.RemoveFromRoleAsync(user, role.Name);
                    }
                }
                return RedirectToAction("Edit", new { id = RoleId });
            }
            return View(models);
        }

    }
}
