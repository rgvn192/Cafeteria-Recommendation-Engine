using AutoMapper;
using RecommendationEngine.Data.Entities;
using Server.Models.Request;
using Server.Models.Response;
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
            CreateMap<DailyRolledOutMenuItem, ViewVotesOnRolledOutMenuItemsResponse>().ForMember(dest => dest.Votes, opt => opt.MapFrom(src => src.DailyRolledOutMenuItemVotes.Count));
            CreateMap<DailyRolledOutMenuItem, RolledOutMenuItem>();
        }
    }
}
