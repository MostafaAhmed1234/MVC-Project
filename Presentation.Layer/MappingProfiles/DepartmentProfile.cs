using AutoMapper;
using Data.Access.Layer.Models;
using Presentation.Layer.ViewModels;

namespace Presentation.Layer.MappingProfiles
{
    public class DepartmentProfile :Profile
    {
        public DepartmentProfile()
        {
            CreateMap<DepartmentViewModel,Department>().ReverseMap();
        }
    }
}
