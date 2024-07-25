using RecommendationEngine.Data.Entities;
using Server.Models.DTO;
using Server.Models.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Interface
{
    public interface IUserPreferenceService : ICrudBaseService<UserPreference>
    {
        Task<List<UserPreferenceModel>> GetPreferencesByUserId(int userId);
        Task<int> RemoveUserPreference(RemoveUserPreferenceRequest removeUserPreferenceRequest);
    }
}
