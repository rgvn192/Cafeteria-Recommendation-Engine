using System;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using RecommendationEngine.Client.Interfaces;
using RecommendationEngine.Client.Models;

namespace RecommendationEngine.Client
{

    public class Client
    {
        private readonly TcpClient _tcpClient;
        private NetworkStream _stream;
        private readonly ILogger<Client> _logger;
        private readonly IServiceProvider _serviceProvider;
        private CancellationTokenSource _cancellationTokenSource;
        private string _userRole;
        private int? _userId;

        private const string _serverIP = "127.0.0.1";
        private const int _port = 5000;


        public Client(ILogger<Client> logger, IServiceProvider serviceProvider)
        {
            _tcpClient = new TcpClient();
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        public async Task StartClientAsync()
        {
            _cancellationTokenSource = new CancellationTokenSource();

            try
            {
                await ConnectToServerAsync();

                var authService = _serviceProvider.GetRequiredService<IAuthService>();

                if (await authService.SignInAsync(_stream, _logger))
                {
                    _userRole = authService.GetUserRole();
                    _userId = authService.GetUserId();

                    // Display menu based on user role
                    await DisplayMenuAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the client");
            }
            finally
            {
                _tcpClient.Close();
                _cancellationTokenSource.Cancel();
            }
        }

        private async Task ConnectToServerAsync()
        {
            try
            {
                await _tcpClient.ConnectAsync(_serverIP, _port);
                _logger.LogInformation("Connected to server...");
                _stream = _tcpClient.GetStream();
            }
            catch (SocketException ex)
            {
                _logger.LogError(ex, "Failed to connect to server");
                throw;
            }
        }

        private async Task DisplayMenuAsync()
        {
            switch (_userRole)
            {
                case "Admin":
                    var adminService = _serviceProvider.GetRequiredService<IAdminService>();
                    await adminService.ShowMenuAsync(_stream, _userRole); // Pass the user role to the admin service
                    break;
                case "Chef":
                    var chefService = _serviceProvider.GetRequiredService<IChefService>();
                    await chefService.ShowMenuAsync(_stream, _userRole, _userId); // Pass the user role to the chef service
                    break;
                case "User":
                    var employeeService = _serviceProvider.GetRequiredService<IEmployeeService>();
                    await employeeService.ShowMenuAsync(_stream, _userRole, _userId); // Pass the user role to the employee service
                    break;
                default:
                    _logger.LogWarning("Unknown role.");
                    break;
            }
        }

    }

}
