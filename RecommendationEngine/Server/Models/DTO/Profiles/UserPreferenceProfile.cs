using AutoMapper;
using RecommendationEngine.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Models.DTO.Profiles
{
    public class UserPreferenceProfile : Profile
    {
        public UserPreferenceProfile()
        {
            CreateMap<UserPreference, UserPreferenceModel>().ReverseMap();
        }
    }
}
