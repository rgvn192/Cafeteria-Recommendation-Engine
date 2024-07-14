using AutoMapper;
using Microsoft.Extensions.Logging;
using RecommendationEngine.Common.Exceptions;
using RecommendationEngine.Data.Entities;
using RecommendationEngine.Data.Interface;
using Server.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Server.Services
{

    public class DailyRolledOutMenuItemVoteService : CrudBaseService<DailyRolledOutMenuItemVote>, IDailyRolledOutMenuItemVoteService
    {
        public DailyRolledOutMenuItemVoteService(IDailyRolledOutMenuItemVoteRepository rolledOutMenuItemVoteRepository, IMapper mapper, ILogger<DailyRolledOutMenuItemService> logger) :
            base(rolledOutMenuItemVoteRepository, mapper, logger)
        {
        }

        protected override List<string> ModifiableProperties => new()
        {

        };

        public async Task VoteForMenuItem(int dailyRolledOutMenuItemId, int userId)
        {

            if(dailyRolledOutMenuItemId <= 0 || userId <= 0)
            {
                throw new AppException(ErrorResponse.ErrorEnum.Validation, "dailyRolledOutMenuItemId or userId is invalid");
            }

            Expression<Func<DailyRolledOutMenuItemVote, bool>> predicate = v => v.UserId == userId && v.DailyRolledOutMenuItemId == dailyRolledOutMenuItemId;
            var duplicateVote = await GetList<DailyRolledOutMenuItemVote>(predicate: predicate);

            if (duplicateVote != null && duplicateVote.Count != 0)
            {
                throw new AppException(ErrorResponse.ErrorEnum.Validation, "Cannot Vote on already voted item");
            }

            var dailyMenuItemVote = new DailyRolledOutMenuItemVote()
            {
                DailyRolledOutMenuItemId = dailyRolledOutMenuItemId,
                UserId = userId
            };
            await Add(dailyMenuItemVote);
        }
    }
}
