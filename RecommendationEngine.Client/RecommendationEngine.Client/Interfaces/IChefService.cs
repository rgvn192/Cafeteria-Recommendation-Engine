using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace RecommendationEngine.Client.Interfaces
{
    public interface IChefService
    {
        Task ShowMenuAsync(NetworkStream stream, string role, int? userId); // Include the role parameter
    }
}
