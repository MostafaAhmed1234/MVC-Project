using AutoMapper;
using Data.Access.Layer.Models;
using Presentation.Layer.ViewModels;

namespace Presentation.Layer.MappingProfiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap < ApplicationUser, UserViewModel>().ReverseMap();
        }
    }
}
