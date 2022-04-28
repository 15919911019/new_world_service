using AutoMapper;
using CBP.Permission.Models.UserRole;
using System;
using System.Collections.Generic;
using System.Text;

namespace CBP.Permission.Models.Profiles
{
    public class UserRoleProfile : Profile
    {
        public UserRoleProfile()
        {
            CreateMap<UserRoleModel, UserRoleMapModel>().ReverseMap();
        }
    }
}
