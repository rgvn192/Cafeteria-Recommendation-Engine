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
    public class DailyRolledOutMenuItemProfile : Profile
    {
        public DailyRolledOutMenuItemProfile()
        {
            CreateMap<DailyRolledOutMenuItem, DailyRolledOutMenuItemModel>().ReverseMap();
            CreateMap<DailyRolledOutMenuItemModel, DailyRolledOutMenuItemRequestModel>().ReverseMap();
            CreateMap<DailyRolledOutMenuItem, DailyRolledOutMenuItemRequestModel>().ReverseMap();
        }
    }
}
