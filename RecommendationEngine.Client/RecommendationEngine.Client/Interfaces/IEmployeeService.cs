using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace RecommendationEngine.Client.Interfaces
{
    public interface IEmployeeService
    {
        Task ShowMenuAsync(NetworkStream stream, string role, int? userId);
    }
}
