using AutoMapper;
using Data.Access.Layer.Models;
using Presentation.Layer.ViewModels;
using System;

namespace Presentation.Layer.MappingProfiles
{
    public class EmployeeProfile : Profile
    {
        public EmployeeProfile()
        {
            CreateMap<EmployeeViewModel, Employee>().ReverseMap();
        }

    }
}
