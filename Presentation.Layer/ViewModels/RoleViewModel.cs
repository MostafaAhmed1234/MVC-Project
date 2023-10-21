using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;

namespace Presentation.Layer.ViewModels
{
    public class RoleViewModel
    {
        public string Id { get; set; }
        [Display(Name= "Role Name")]
        public string RoleName { get; set; }

        //to use in create action
        public RoleViewModel()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}
