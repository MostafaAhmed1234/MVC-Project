using AutoMapper;
using Business.Logic.Layer.Interfaces;
using Business.Logic.Layer.Repositories;
using Data.Access.Layer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Presentation.Layer.Helpers;
using Presentation.Layer.ViewModels;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;

namespace Presentation.Layer.Controllers
{
    public class EmployeeController : Controller
    {
        //private readonly IEmployeeRepository _employeeRepository;
        //private readonly IDepartmentRepository _departmentRepository;
        private readonly IUintOfWork _uintOfWork;
        private readonly IMapper _mapper;

        public EmployeeController(IUintOfWork uintOfWork,IMapper mapper /*,IDepartmentRepository departmentRepository,IEmployeeRepository employeeRepository*/) // Ask ClR for Creating object from class implmenting interface "IDepartmentRepository" 
        {
            //_employeeRepository = employeeRepository;
            //_departmentRepository = departmentRepository;
            _uintOfWork = uintOfWork;
            _mapper = mapper;
        }

        //------------------------------
        //you can use another Action to search
        public async Task<IActionResult> Index(string searchValue)
        {
            IEnumerable<Employee> employees;

            if (string.IsNullOrEmpty(searchValue))
                 employees = await _uintOfWork.EmployeeRepository.GetAll();
            else
                employees = _uintOfWork.EmployeeRepository.GetEmployeesByName(searchValue);

            var mappedEmps = _mapper.Map<IEnumerable<Employee>, IEnumerable<EmployeeViewModel>>(employees);
            return View(mappedEmps);
        }
        //=============================

        [HttpGet] //to go to view Create New department
        public async Task<IActionResult> Create()
        {
           var departments = await _uintOfWork.DepartmentRepository.GetAll();
            //-----send data not related for this Model from Action to View-----
            ViewBag.Departments = departments;
            return View();
        }

        [HttpPost] // after submit form, this action will implment
        public async Task<IActionResult> Create(EmployeeViewModel employeeVM)
        {
            if (ModelState.IsValid)
            {
                #region Manual Mapping
                //Employee employee = new Employee()
                //{
                //    Name = employeeVM.Name,
                //    Age = employeeVM.Age,
                //    Currency = employeeVM.Currency,
                //    Address = employeeVM.Address,
                //    IsActive = employeeVM.IsActive,
                //    Email = employeeVM.Email,
                //    Phone = employeeVM.Phone,
                //    HireDate = employeeVM.HireDate,
                //};
                //---------------------
                //using OperatorDeclarationSyntax overloading to casting
                //Employee employee = (Employee) employeeVM;
                #endregion

                //--get filename and store file in server
                employeeVM.ImageName= await DocumentSettings.uploadFile(employeeVM.Image, "images");

                #region AutoMapping
                var mappedEmp = _mapper .Map<EmployeeViewModel,Employee>(employeeVM);
                #endregion

               await _uintOfWork.EmployeeRepository.Add(mappedEmp);
                //update
                //delete
                await _uintOfWork.Complete();

                TempData["Message"] = "Employee is Created Successfully";
                return RedirectToAction(nameof(Index));
            }
            return View(employeeVM);
        }
        //=====================================================

        // /Employee/Details/1
        // /Employee/Details
        //[HttpGet]
        public async Task<IActionResult> Details(int? id, string ViewName = "Details")
        {
            if (id is null)
                return BadRequest();
            //  HTTP status code is 400. The status code 400 indicates a client error

            var employee = await _uintOfWork.EmployeeRepository.GetById(id.Value); //Nullable types give me 2 properties[Value,hasValue]

            if (employee is null)
                return NotFound();
            //HTTP status code 404.The status code 404 indicates that the requested resource was not found on the server
            var departments = await _uintOfWork.DepartmentRepository.GetAll();
            ViewBag.Departments = departments;
            //--------------------
            var mappedEmp = _mapper.Map<Employee, EmployeeViewModel >(employee);
            return View(ViewName, mappedEmp);
        }

        //================================================
        // /Employee/Edit/1
        // /Employee/Edit
        //[HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            var departments = await _uintOfWork.DepartmentRepository.GetAll();
            ViewBag.Departments = departments;
            return await Details(id, "Edit");
        }

        [HttpPost]
        [ValidateAntiForgeryToken] 
        public async Task<IActionResult> Edit([FromRoute] int id, EmployeeViewModel employeeVM)
        { 
            if (id != employeeVM.Id)
                return BadRequest();

            if (ModelState.IsValid)
            {
                try
                {
                    var mappedEmp = _mapper.Map<EmployeeViewModel, Employee>(employeeVM);
                    _uintOfWork.EmployeeRepository.Update(mappedEmp);
                    await _uintOfWork.Complete();
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            return View(employeeVM);
        }
        //======================================
        // /Employee/Delete/1
        // /Employee/Delete
        //[HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            var departments =await _uintOfWork.DepartmentRepository.GetAll();
            ViewBag.Departments = departments;
            return await Details(id, "Delete");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAsync([FromRoute] int id, EmployeeViewModel employeeVM)
        {
            if (id != employeeVM.Id)
                return BadRequest();
            try
            {
                var mappedEmp = _mapper.Map<EmployeeViewModel, Employee>(employeeVM);
                _uintOfWork.EmployeeRepository.Delete(mappedEmp);
                int count = await _uintOfWork.Complete();

                if (count > 0 && employeeVM.ImageName is not null)
                    DocumentSettings.DeleteFile(employeeVM.ImageName, "images");

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(employeeVM);
            }

        }
    }
}
