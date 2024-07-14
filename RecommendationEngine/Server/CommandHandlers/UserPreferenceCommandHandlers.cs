using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using RecommendationEngine.Common.Utils;
using Server.Interface;
using Server.Models.DTO;
using Server.Models.Request;
using Server.Models.Response;
using Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.CommandHandlers
{
    public class UserPreferenceCommandHandlers
    {
        private readonly IUserPreferenceService _userPreferenceService;

        public UserPreferenceCommandHandlers(IUserPreferenceService userPreferenceService)
        {
            _userPreferenceService = userPreferenceService;
        }

        public async Task<CustomProtocolResponse> AddUserPreference(string body)
        {
            try
            {
                var request = JsonConvert.DeserializeObject<UserPreferenceModel>(body);

                await _userPreferenceService.Add(request);

                var response = new CommonServerResponse
                {
                    Status = "Success",
                    Message = "Successfully Added User Preference."
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

        public async Task<CustomProtocolResponse> DeleteUserPreference(string body)
        {
            try
            {
                var request = JsonConvert.DeserializeObject<RemoveUserPreferenceRequest>(body);

                await _userPreferenceService.RemoveUserPreference(request);

                var response = new DeleteMenuResponseModel
                {
                    Status = "Success",
                    Message = "User Preference deleted successfully."
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

        public async Task<CustomProtocolResponse> GetUserPreferences(string body)
        {
            try
            {
                var userId = JsonConvert.DeserializeObject<int>(body);
                List<UserPreferenceModel> userPreferences = new();

                var preferences = await _userPreferenceService.GetPreferencesByUserId(userId);

                userPreferences = preferences;

                return new CustomProtocolResponse
                {
                    Status = "Success",
                    Body = JsonHelper.SerializeObjectIgnoringCycles(userPreferences)
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
