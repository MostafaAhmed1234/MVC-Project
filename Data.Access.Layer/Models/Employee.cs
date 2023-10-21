using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Access.Layer.Models
{
    public class Employee
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        public int? Age { get; set; }
        public string Address { get; set; }

        public decimal Currency { get; set; }

        public bool IsActive { get; set; }
        public string Email { get; set; }

        public string Phone { get; set; }

        public DateTime HireDate { get; set; }

        public DateTime CreationDate { get; set; } = DateTime.Now;
        public string ImageName { get; set; }

        //-----------------------------------
        [ForeignKey("Department")]
        public int? Dept_ID { get; set; }
        public Department? Department { get; set; }

    }
}
