using AutoMapper;
using Microsoft.Extensions.Logging;
using RecommendationEngine.Data.Entities;
using RecommendationEngine.Data.Interface;
using RecommendationEngine.Data.Repositories;
using Server.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Services
{
    public class UserPreferenceService : CrudBaseService<UserPreference>, IUserPreferenceService
    {
        public UserPreferenceService(IUserPreferenceRepository userPreferenceRepository, IMapper mapper, ILogger<UserPreferenceService> logger) :
            base(userPreferenceRepository, mapper, logger)
        {
        }

        protected override List<string> ModifiableProperties => new()
        {            
        };
    }
}
