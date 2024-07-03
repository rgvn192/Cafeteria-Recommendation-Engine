using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RecommendationEngine.Client.Interfaces;
using RecommendationEngine.Client.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace RecommendationEngine.Client.Services
{
    public class AuthService : IAuthService
    {
        private string _userRole;
        private int? _userId;

        public int? GetUserId()
        {
            return _userId;
        }

        public string GetUserRole()
        {
            return _userRole;
        }

        public async Task<bool> SignInAsync(NetworkStream stream, ILogger logger)
        {
            Console.Write("Enter User ID: ");
            var userId = Console.ReadLine();
            Console.Write("Enter Name: ");
            var userName = Console.ReadLine();

            var loginRequest = new LoginRequestModel
            {
                UserId = userId,
                Name = userName
            };

            var customRequest = new CustomProtocolRequest
            {
                Command = "Login",
                Body = JsonConvert.SerializeObject(loginRequest)
            };

            var response = await SendRequestAsync(stream, customRequest, logger);

            if (response != null && response.Status == "Success")
            {
                var loginResponse = JsonConvert.DeserializeObject<LoginResponseModel>(response.Body);
                _userRole = loginResponse.Role;
                _userId = loginResponse.UserId;
                logger.LogInformation($"Login successful. Role: {_userRole}, UserId : {_userId}");
                return true;
            }

            logger.LogWarning("Login failed.");
            return false;
        }

        public async Task<bool> SignInToThirdPartyServerAsync(NetworkStream stream, ILogger logger)
        {
            Console.Write("Enter Username : ");
            var username = Console.ReadLine();
            Console.Write("Enter Password : ");
            var password = Console.ReadLine();

            var loginRequest = new LoginRequest
            {
                username = username,
                password = password,
                endpoint = "/login"

            };

            var response = await SendRequestToThirdPartyServerAsync(stream, loginRequest, logger);

            if (response != null && response.status == "success")
            {
                
                _userRole = response.User.RoleName;
                _userId = response.User.ID;
                logger.LogInformation($"Login successful. Role: {_userRole}, UserId : {_userId}");
                return true;
            }

            logger.LogWarning("Login failed.");
            return false;
        }

        private async Task<CustomProtocolResponse> SendRequestAsync(NetworkStream stream, CustomProtocolRequest request, ILogger logger)
        {
            try
            {
                var requestData = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(request));
                await stream.WriteAsync(requestData, 0, requestData.Length);

                var buffer = new byte[1024];
                var bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                var responseJson = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                return JsonConvert.DeserializeObject<CustomProtocolResponse>(responseJson);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error sending request to server");
                return null;
            }
        }

        private async Task<LoginResponse> SendRequestToThirdPartyServerAsync(NetworkStream stream, LoginRequest request, ILogger logger)
        {
            try
            {
                var requestData = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(request));
                await stream.WriteAsync(requestData, 0, requestData.Length);

                var buffer = new byte[1024];
                var bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                var responseJson = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                return JsonConvert.DeserializeObject<LoginResponse>(responseJson);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error sending request to server");
                return null;
            }
        }
    }

}
