using AutoMapper;
using CBP.Permission.Models.Role;
using System;
using System.Collections.Generic;
using System.Text;

namespace CBP.Permission.Models.Profiles
{
    public class RoleProfile : Profile
    {
        public RoleProfile()
        {
            CreateMap<RoleModel, RoleMapModel>().ReverseMap();
            CreateMap<RoleFunctionModel, RoleFunctionMapModel>().ReverseMap();
        }
    }
}
