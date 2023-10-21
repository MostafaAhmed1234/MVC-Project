using Data.Access.Layer.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;
using Microsoft.AspNetCore.Http;

namespace Presentation.Layer.ViewModels
{
    public class EmployeeViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is Required !!")]
        [MaxLength(50, ErrorMessage = "Max Length of Name is 50")]
        [MinLength(5, ErrorMessage = "Mix Length of Name is 5")]
        public string Name { get; set; }

        [Range(22, 30, ErrorMessage = "Age must be from 22~30 years")]
        public int? Age { get; set; }

        [RegularExpression(@"^[0-9]{1,3}-[a-zA-Z]{5,10}-[a-zA-Z]{4,10}-[a-zA-Z]{5,10}$",
            ErrorMessage = "Address must be like 123-Street-City-Country")]
        public string Address { get; set; }

        [DataType(DataType.Currency)]
        public decimal Currency { get; set; }

        public bool IsActive { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [Phone]
        public string Phone { get; set; }

        public DateTime HireDate { get; set; }
        //---- File Settings----
        public IFormFile Image { get; set; }
        public string ImageName { get; set; }

        //-----------------------------------
        [ForeignKey("Department")]
        public int? Dept_ID { get; set; }
        public Department? Department { get; set; }
    }
}
