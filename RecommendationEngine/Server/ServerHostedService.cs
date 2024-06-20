using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RecommendationEngine.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Server.Models;
using Server.Models.Request;
using Server.Models.Response;
using Server.Interface;
using Server.CommandHandlers;

namespace Server
{
    public class ServerHostedService : IHostedService
    {
        private readonly ILogger<ServerHostedService> _logger;
        private readonly ICommandHandlerRegistry _commandHandlerRegistry;
        private TcpListener _listener;

        public ServerHostedService(ILogger<ServerHostedService> logger, ICommandHandlerRegistry commandHandlerRegistry)
        {
            _logger = logger;
            _commandHandlerRegistry = commandHandlerRegistry;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _listener = new TcpListener(IPAddress.Any, 5000);
            _listener.Start();
            _logger.LogInformation("Server started on port 5000...");

            Task.Run(async () =>
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    var client = await _listener.AcceptTcpClientAsync();
                    _ = Task.Run(() => HandleClientAsync(client));
                }
            }, cancellationToken);

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _listener.Stop();
            _logger.LogInformation("Server stopped.");
            return Task.CompletedTask;
        }

        private async Task HandleClientAsync(TcpClient client)
        {
            _logger.LogInformation("Client connected...");
            var stream = client.GetStream();
            var buffer = new byte[1024];
            int bytesRead;

            while ((bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length)) != 0)
            {
                var requestJson = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                var customRequest = JsonConvert.DeserializeObject<CustomProtocolRequest>(requestJson);
                _logger.LogInformation($"Received command: {customRequest.Command}");

                var response = await ProcessRequest(customRequest);
                var responseData = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(response));
                await stream.WriteAsync(responseData, 0, responseData.Length);
            }

            client.Close();
            _logger.LogInformation("Client disconnected...");
        }

        private async Task<CustomProtocolResponse> ProcessRequest(CustomProtocolRequest request)
        {
            if (_commandHandlerRegistry.CommandHandlers.TryGetValue(request.Command, out var handler))
            {
                // Check authorization
                if (!handler.Authorize(request.Role))
                {
                    return new CustomProtocolResponse { Status = "Error", Body = "Access denied" };
                }

                // Handle the command
                return await handler.Handler(request.Body);
            }
            else
            {
                return new CustomProtocolResponse { Status = "Error", Body = "Invalid command" };
            }
        }
    }

}
