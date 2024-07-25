using AutoMapper;
using RecommendationEngine.Data.Entities;
using Server.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Models.DTO.Profiles
{
    
    public class DiscardedMenuItemFeedbackProfile : Profile
    {
        public DiscardedMenuItemFeedbackProfile()
        {
            CreateMap<DiscardedMenuItemFeedback, DiscardedMenuItemFeedbackModel>().ReverseMap();
        }
    }
}
