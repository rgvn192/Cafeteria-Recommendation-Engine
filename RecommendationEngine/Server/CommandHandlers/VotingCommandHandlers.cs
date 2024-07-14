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
    public class VotingCommandHandlers
    {
        private readonly IDailyRolledOutMenuItemVoteService _dailyRolledOutMenuItemVoteService;
        public VotingCommandHandlers(IDailyRolledOutMenuItemVoteService dailyRolledOutMenuItemVoteService)
        {
            _dailyRolledOutMenuItemVoteService = dailyRolledOutMenuItemVoteService;
        }

        public async Task<CustomProtocolResponse> VoteForDailyMenuItem(string body)
        {
            try
            {
                var request = JsonConvert.DeserializeObject<VoteForDailyMenuItemRequest>(body);

                await _dailyRolledOutMenuItemVoteService.VoteForMenuItem(request.DailyRolledOutMenuItemId, request.UserId);

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
