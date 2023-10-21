using AutoMapper;
using Business.Logic.Layer.Interfaces;
using Business.Logic.Layer.Repositories;
using Data.Access.Layer.Models;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Mvc;
using Presentation.Layer.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace Presentation.Layer.Controllers
{
    // DepartmentController depends on any class implments IDepartmentRepository
    public class DepartmentController : Controller
    {
        //private readonly IDepartmentRepository _departmentRepository;
        private readonly IUintOfWork _uintOfWork;
        private readonly IMapper _mapper;

        public DepartmentController(/*IDepartmentRepository departmentRepository*/IUintOfWork uintOfWork,IMapper mapper) // Ask ClR for Creating object from class implmenting interface "IDepartmentRepository" 
        {
            //_departmentRepository = new DepartmentRepository(); //don't have parameter less constructor, and i want to make this for clR
            //_departmentRepository = departmentRepository;
            _uintOfWork = uintOfWork;
            _mapper = mapper;
        }

        //------------------------------
        public async Task<IActionResult> Index()
        {
            var departments = await _uintOfWork.DepartmentRepository.GetAll();
            var mappedDepts = _mapper.Map<IEnumerable<Department>, IEnumerable<DepartmentViewModel>>(departments);
            return View(mappedDepts);
        }
        //=============================

        [HttpGet] //to go to view Create New department
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost] // after submit form, this action will implment
        public async Task<IActionResult> Create(DepartmentViewModel departmentVM)
        {
            //must check validation in front end [server side validation] that in Model Class
            if (ModelState.IsValid)
            {
                var mappedDept = _mapper.Map<DepartmentViewModel, Department>(departmentVM);

                await _uintOfWork.DepartmentRepository.Add(mappedDept);
                //update
                //delete
                int count = await _uintOfWork.Complete();
                //check no. of department created
                if (count>0)
                   TempData["Message"] = "Department is Created Successfully";

                return RedirectToAction(nameof(Index));
            }
            // if there is error in validation => return same view with same data, that he tried to input
            return View(departmentVM);
        }
        //=====================================================

        // /Department/Details/1
        // /Department/Details
        //[HttpGet]
        public async Task<IActionResult> Details(int? id,string ViewName = "Details")
        {
            if (id is null)
                return BadRequest();
            //  HTTP status code is 400. The status code 400 indicates a client error

            var department = await _uintOfWork.DepartmentRepository.GetById(id.Value); //Nullable types give me 2 properties[Value,hasValue]

            if (department is null)
                return NotFound();
            //HTTP status code 404.The status code 404 indicates that the requested resource was not found on the server
            //----------------------
            var mappedDept = _mapper.Map<Department, DepartmentViewModel>(department);
            return View(ViewName, mappedDept);
        }

        //================================================
        // /Department/Edit/1
        // /Department/Edit
        //[HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            ///if (id is null)
            ///    return BadRequest();
            ///var department = _departmentRepository.GetById(id.Value);
            ///if (department is null)
            ///    return NotFound();
            ///return View(department);
            
            return await Details( id,"Edit");
        }

        [HttpPost]
        [ValidateAntiForgeryToken] // to prevent edit from way [not my website] like postman
        public async Task<IActionResult> Edit([FromRoute]int id, DepartmentViewModel departmentVM)
        {      //I want to make sure read id from segmant[i sent it in (asp-route-id="@department.Id")]
               // to make sure not changing id in bad way inspect in browser
            if (id != departmentVM.Id)
                return BadRequest();

            if (ModelState.IsValid)
            {
                //must use try catch to be sure there is no error in updating in data base
                try
                {
                    var mappedDept = _mapper.Map<DepartmentViewModel, Department>(departmentVM);
                    _uintOfWork.DepartmentRepository.Update(mappedDept);
                    int count = await _uintOfWork.Complete();
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    //1.Log Exception
                    //2.Friendly message for user
                    //----------------------------
                    // show error in  <div asp-validation-summary="All"></div> 
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            return View(departmentVM);
        }
        //======================================
        // /Department/Delete/1
        // /Department/Delete
        //[HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            return await Details(id, "Delete");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([FromRoute] int id, DepartmentViewModel departmentVM)
        {
            if (id != departmentVM.Id)
                return BadRequest();
            try
            {
                var mappedDept = _mapper.Map<DepartmentViewModel, Department>(departmentVM);
                _uintOfWork.DepartmentRepository.Delete(mappedDept);
                int count =await _uintOfWork.Complete();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            { 
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(departmentVM);
            }
         
        }
    }
       
}
