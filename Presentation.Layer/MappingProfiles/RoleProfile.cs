using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Presentation.Layer.ViewModels;

namespace Presentation.Layer.MappingProfiles
{
    public class RoleProfile :Profile
    {
        public RoleProfile()
        {
            CreateMap<RoleViewModel,IdentityRole>()
            .ForMember(R=>R.Name,o=>o.MapFrom(s=>s.RoleName))
            .ReverseMap();
        }
    }
}
