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
    public static class UserPreferenceCommandHandlers
    {
        public async static Task<CustomProtocolResponse> AddUserPreference(IServiceProvider serviceProvider, string body)
        {
            try
            {
                var request = JsonConvert.DeserializeObject<UserPreferenceModel>(body);

                using (var scope = serviceProvider.CreateScope())
                {
                    var userPreferenceService = scope.ServiceProvider.GetRequiredService<IUserPreferenceService>();

                    await userPreferenceService.Add(request);
                }

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

        public async static Task<CustomProtocolResponse> DeleteUserPreference(IServiceProvider serviceProvider, string body)
        {
            try
            {
                var request = JsonConvert.DeserializeObject<RemoveUserPreferenceRequest>(body);

                using (var scope = serviceProvider.CreateScope())
                {
                    var userPreferenceService = scope.ServiceProvider.GetRequiredService<IUserPreferenceService>();

                    await userPreferenceService.RemoveUserPreference(request);
                }

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

        public async static Task<CustomProtocolResponse> GetUserPreferences(IServiceProvider serviceProvider, string body)
        {
            try
            {
                var userId = JsonConvert.DeserializeObject<int>(body);
                List<UserPreferenceModel> userPreferences = new();

                using (var scope = serviceProvider.CreateScope())
                {
                    var mapper = scope.ServiceProvider.GetRequiredService<IMapper>();
                    var userPreferenceService = scope.ServiceProvider.GetRequiredService<IUserPreferenceService>();

                    var preferences = await userPreferenceService.GetPreferencesByUserId(userId);

                    userPreferences = preferences;
                }

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
