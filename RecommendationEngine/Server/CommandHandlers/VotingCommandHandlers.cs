using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using RecommendationEngine.Common.Exceptions;
using RecommendationEngine.Data.Entities;
using Server.Interface;
using Server.Models.Request;
using Server.Models.Response;
using Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Server.CommandHandlers
{
    public static class VotingCommandHandlers
    {
        public async static Task<CustomProtocolResponse> VoteForDailyMenuItem(IServiceProvider serviceProvider, string body)
        {
            try
            {
                var request = JsonConvert.DeserializeObject<VoteForDailyMenuItemRequest>(body);

                using (var scope = serviceProvider.CreateScope())
                {
                    var voteService = scope.ServiceProvider.GetRequiredService<IDailyRolledOutMenuItemVoteService>();

                    Expression<Func<DailyRolledOutMenuItemVote, bool>> predicate = v => v.UserId == request.UserId && v.DailyRolledOutMenuItemId == request.DailyRolledOutMenuItemId;
                    var duplicateVote = await voteService.GetList<DailyRolledOutMenuItemVote>(predicate: predicate);

                    if (duplicateVote != null && duplicateVote.Count != 0)
                    {
                        throw new AppException("Cannot Vote on already voted item");
                    }

                    var dailyMenuItemVote = new DailyRolledOutMenuItemVote()
                    {
                        DailyRolledOutMenuItemId = request.DailyRolledOutMenuItemId,
                        UserId = request.UserId
                    };
                    await voteService.Add(dailyMenuItemVote);
                }

                var response = new CommonServerResponse
                {
                    Status = "Success",
                    Message = "Voted successfully."
                };

                return new CustomProtocolResponse
                {
                    Status = "Success",
                    Body = JsonConvert.SerializeObject(response)
                };
            }
            catch (Exception ex)
            {
                return new CustomProtocolResponse
                {
                    Status = "Failure",
                    Body = JsonConvert.SerializeObject(ex.Message)
                };
            }

        }
    }
}
