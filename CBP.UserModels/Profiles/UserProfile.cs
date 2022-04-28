using AutoMapper;
using CBP.UserModels.User;
using System;
using System.Collections.Generic;
using System.Text;

namespace CBP.UserModels.Profiles
{
    class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserModel, UserMapModel>().ReverseMap();
        }
    }
}