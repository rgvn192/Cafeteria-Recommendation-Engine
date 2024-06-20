using AutoMapper;
using RecommendationEngine.Data.Entities;
using Server.Models.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Models.DTO.Profiles
{
    public class MenuItemProfile : Profile
    {
        public MenuItemProfile()
        {
            CreateMap<MenuItemModel, MenuItem>().ReverseMap();
            CreateMap<MenuItemModel, UpdateMenuItemRequestModel>().ReverseMap();
            CreateMap<MenuItem, UpdateMenuItemRequestModel>().ReverseMap();
        }
    }
}
