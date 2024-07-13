using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Server.Interface;
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
    public static class AuthCommandHandlers
    {
        public static async Task<CustomProtocolResponse> Login(IServiceProvider serviceProvider, string body)
        {
            try
            {
                var loginRequest = JsonConvert.DeserializeObject<LoginRequestModel>(body);
                var authService = serviceProvider.GetRequiredService<IAuthorisationService>();
                var user = await authService.AuthenticateUser(loginRequest);

                var loginResponse = new LoginResponseModel
                {
                    Role = user?.Role.Name.ToString(),
                    UserId = user?.UserId
                };

                return new CustomProtocolResponse
                {
                    Status = user != null ? "Success" : "Failed",
                    Body = JsonConvert.SerializeObject(loginResponse)
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
