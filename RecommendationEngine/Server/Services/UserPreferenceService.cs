using AutoMapper;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.Extensions.Logging;
using RecommendationEngine.Common.Exceptions;
using RecommendationEngine.Data.Entities;
using RecommendationEngine.Data.Interface;
using RecommendationEngine.Data.Repositories;
using Server.Interface;
using Server.Models.DTO;
using Server.Models.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http.Headers;
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

        public override async Task<int> Add<TModel>(TModel model)
        {
            UserPreference userPreference = _mapper.Map<UserPreference>(model);

            Expression<Func<UserPreference, bool>> predicate = up => up.UserId == userPreference.UserId && up.CharacteristicId == userPreference.CharacteristicId;
            var existingPreference = (await GetList<UserPreference>(predicate: predicate)).FirstOrDefault();
            if(existingPreference != null)
            {
                throw new AppException("Can not insert the same characteristic for same user.");
            };

            return await base.Add(model);
        }

        public async Task<List<UserPreferenceModel>> GetPreferencesByUserId(int userId)
        {
            Expression<Func<UserPreference, bool>> predicate = up => up.UserId == userId;
            var preferences = await GetList<UserPreferenceModel>(include : $"{nameof(UserPreference.Characteristic)}", predicate: predicate);

            return preferences;
        }

        public async Task<int> RemoveUserPreference(RemoveUserPreferenceRequest removeUserPreferenceRequest)
        {
            if (removeUserPreferenceRequest == null)
            {
                throw new AppException(ErrorResponse.ErrorEnum.BadRequest, "request model is null");
            }

            var userPreference = await GetByIdAsNoTracking<UserPreferenceModel>(removeUserPreferenceRequest.UserPreferenceId) ?? throw new AppException(ErrorResponse.ErrorEnum.BadRequest, "user preference not found");

            if (userPreference.UserId != removeUserPreferenceRequest.UserId)
            {
                throw new AppException(ErrorResponse.ErrorEnum.BadRequest, "Cannot remove preference of another user");
            }

            return await Delete(removeUserPreferenceRequest.UserPreferenceId);
        }
    }
}
