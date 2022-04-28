using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

using CBP.Permission.Models.Function;

namespace CBP.Permission.Models.Profiles
{
    public class FunctionProfile : Profile
    {
        public FunctionProfile()
        {
            CreateMap<FunctionModel, FunctionMapModel>().ReverseMap();
        }
    }
}
