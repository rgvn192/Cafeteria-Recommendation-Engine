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
    public class DiscardedMenuItemProfile : Profile
    {
        public DiscardedMenuItemProfile()
        {
            CreateMap<DiscardedMenuItem, DiscardedMenuItemsResponse>().ReverseMap();
        }
    }
}
