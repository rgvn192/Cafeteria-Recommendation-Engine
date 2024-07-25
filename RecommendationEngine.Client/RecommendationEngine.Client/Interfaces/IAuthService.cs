using Microsoft.Extensions.Logging;
using RecommendationEngine.Client.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace RecommendationEngine.Client.Interfaces
{
    public interface IAuthService
    {
        Task<bool> SignInAsync(NetworkStream stream, ILogger logger);
        string GetUserRole();
        int? GetUserId();

        Task<bool> SignInToThirdPartyServerAsync(NetworkStream stream, ILogger logger);
       
    }
}
