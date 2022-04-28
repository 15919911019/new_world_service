using AutoMapper;
using CBP.Permission.Models.Menu;
using System;
using System.Collections.Generic;
using System.Text;

namespace CBP.Permission.Models.Profiles
{
    public class MenuProfile : Profile
    {
        public MenuProfile()
        {
            CreateMap<MenuModel, MenuMapModel>().ReverseMap();
        }
    }
}
