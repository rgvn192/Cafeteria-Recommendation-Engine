using RecommendationEngine.Data.Entities;
using Server.Interface;
using Server.Models.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Services
{
    internal class AuthorisationService : IAuthorisationService
    {
        private readonly IUserService _userService;

        public AuthorisationService(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<User> AuthenticateUser(LoginRequestModel loginRequest)
        {
            if (!int.TryParse(loginRequest.UserId, out int userId))
            {
                throw new ArgumentException("Invalid UserId format. UserId must be a valid integer.");
            }

            var user = await _userService.GetById<User>(userId, $"{nameof(User.Role)}");

            if (user != null && user.Name.ToLower() == loginRequest.Name.ToLower())
            {
                return user;
            }

            return null;
        }

    }
}
