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
using System.Xml.Linq;

namespace RecommendationEngine.Client.Services
{

    public class AdminService : IAdminService
    {
        private readonly ILogger<AdminService> _logger;

        public AdminService(ILogger<AdminService> logger)
        {
            _logger = logger;
        }

        public async Task ShowMenuAsync(NetworkStream stream, string role) // Include the role parameter
        {
            while (true)
            {
                Console.WriteLine("Admin Menu:");
                Console.WriteLine("1. Add Menu Item");
                Console.WriteLine("2. Update Menu Item");
                Console.WriteLine("3. Delete Menu Item");
                Console.WriteLine("4. Get Menu Items");
                Console.WriteLine("5. Toggle Menu Item Availability");
                Console.WriteLine("6. Logout");
                Console.Write("Select an option: ");
                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        await AddMenuItemAsync(stream, role); // Pass the role parameter
                        break;
                    case "2":
                        await UpdateMenuItemAsync(stream, role);
                        break;
                    case "3":
                        await DeleteMenuItemAsync(stream, role);
                        break;
                    case "4":
                        await GetMenuItemAsync(stream, role);
                        break;
                    case "5":
                        await ToggleMenuItemAvailabilityAsync(stream, role);
                        break;
                    case "6":
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Invalid option. Please try again.");
                        break;
                }
            }
        }

        private async Task AddMenuItemAsync(NetworkStream stream, string role) // Include the role parameter
        {
            try
            {
                Console.Write("Enter menu item name: ");
                var name = Console.ReadLine();

                Console.Write("Enter menu item type id: ");
                if (!int.TryParse(Console.ReadLine(), out var menuItemCategoryId))
                {
                    Console.WriteLine("Invalid id. Please try again.");
                    return;
                }

                Console.Write("Enter item price: ");
                if (!decimal.TryParse(Console.ReadLine(), out var price))
                {
                    Console.WriteLine("Invalid price. Please try again.");
                    return;
                }

                var addMenuItemRequest = new MenuItemModel()
                {
                    Name = name,
                    MenuItemCategoryId = menuItemCategoryId,
                    Price = price
                };

                var customRequest = new CustomProtocolRequest
                {
                    Command = "AddMenuItem",
                    Role = role, // Include the role in the request
                    Body = JsonConvert.SerializeObject(addMenuItemRequest)
                };

                var response = await SendRequestAsync(stream, customRequest);

                if (response != null)
                {
                    _logger.LogInformation($"Received response: {response.Body}");
                    Console.WriteLine($"Response: {response.Body}");
                }
                else
                {
                    Console.WriteLine("Failed to get response from server.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending request to server");
                Console.WriteLine($"{ex.Message}");
                return;
            }
        }

        private async Task UpdateMenuItemAsync(NetworkStream stream, string role) // Include the role parameter
        {
            try
            {
                Console.Write("Enter menu item id: ");
                if (!int.TryParse(Console.ReadLine(), out var menuItemId))
                {
                    Console.WriteLine("Invalid id. Please try again.");
                    return;
                }

                Console.Write("Enter menu item name: ");
                var name = Console.ReadLine();

                Console.Write("Enter menu item type id: ");
                if (!int.TryParse(Console.ReadLine(), out var menuItemCategoryId))
                {
                    Console.WriteLine("Invalid id. Please try again.");
                    return;
                }

                Console.Write("Enter item price: ");
                if (!decimal.TryParse(Console.ReadLine(), out var price))
                {
                    Console.WriteLine("Invalid price. Please try again.");
                    return;
                }

                var updateMenuItemRequest = new MenuItemModel()
                {
                    MenuItemId = menuItemId,
                    Name = name,
                    MenuItemCategoryId = menuItemCategoryId,
                    Price = price
                };

                var customRequest = new CustomProtocolRequest
                {
                    Command = "UpdateMenuItem",
                    Role = role, // Include the role in the request
                    Body = JsonConvert.SerializeObject(updateMenuItemRequest)
                };

                var response = await SendRequestAsync(stream, customRequest);

                if (response != null)
                {
                    _logger.LogInformation($"Received response: {response.Body}");
                    Console.WriteLine($"Response: {response.Body}");
                }
                else
                {
                    Console.WriteLine("Failed to get response from server.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending request to server");
                Console.WriteLine($"{ex.Message}");
                return;
            }
        }

        private async Task DeleteMenuItemAsync(NetworkStream stream, string role) // Include the role parameter
        {
            try
            {
                Console.Write("Enter menu item id: ");
                if (!int.TryParse(Console.ReadLine(), out var menuItemId))
                {
                    Console.WriteLine("Invalid id. Please try again.");
                    return;
                }

                var customRequest = new CustomProtocolRequest
                {
                    Command = "DeleteMenuItem",
                    Role = role, // Include the role in the request
                    Body = JsonConvert.SerializeObject(menuItemId)
                };

                var response = await SendRequestAsync(stream, customRequest);

                if (response != null)
                {
                    _logger.LogInformation($"Received response: {response.Body}");
                    Console.WriteLine($"Response: {response.Body}");
                }
                else
                {
                    Console.WriteLine("Failed to get response from server.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending request to server");
                Console.WriteLine($"{ex.Message}");
                return;
            }
        }

        private async Task GetMenuItemAsync(NetworkStream stream, string role) // Include the role parameter
        {
            try
            {
                Console.Write("Enter limit :");
                if (!int.TryParse(Console.ReadLine(), out var limit))
                {
                    Console.WriteLine("Invalid. Please try again.");
                    return;
                }

                Console.Write("Enter offset :");
                if (!int.TryParse(Console.ReadLine(), out var offset))
                {
                    Console.WriteLine("Invalid. Please try again.");
                    return;
                }

                var getMenuItemsRequestModel = new GetMenuItemsRequestModel()
                {
                    Limit = limit,
                    Offset = offset
                };

                var customRequest = new CustomProtocolRequest
                {
                    Command = "GetMenuItems",
                    Role = role, // Include the role in the request
                    Body = JsonConvert.SerializeObject(getMenuItemsRequestModel)
                };

                var response = await SendRequestAsync(stream, customRequest);

                if (response != null && response.Status == "Success")
                {
                    var getMenuItemResponse = JsonConvert.DeserializeObject<GetMenuItemsResponseModel>(response.Body);
                    var menuItems = getMenuItemResponse?.MenuItems;
                    if (menuItems != null)
                    {
                        foreach (var item in menuItems)
                        {
                            Console.WriteLine($"{item.MenuItemId}\t{item.Name}\t{item.Price}\t{item.MenuItemCategoryId}\n");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending request to server");
                Console.WriteLine($"{ex.Message}");
                return;
            }
        }

        private async Task ToggleMenuItemAvailabilityAsync(NetworkStream stream, string role) // Include the role parameter
        {
            try
            {
                Console.Write("Enter menu item id: ");
                if (!int.TryParse(Console.ReadLine(), out var menuItemId))
                {
                    Console.WriteLine("Invalid id. Please try again.");
                    return;
                }

                var customRequest = new CustomProtocolRequest
                {
                    Command = "ToggleMenuItemAvailability",
                    Role = role, // Include the role in the request
                    Body = JsonConvert.SerializeObject(menuItemId)
                };

                var response = await SendRequestAsync(stream, customRequest);

                if (response != null)
                {
                    _logger.LogInformation($"Received response: {response.Body}");
                    Console.WriteLine($"Response: {response.Body}");
                }
                else
                {
                    Console.WriteLine("Failed to get response from server.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending request to server");
                Console.WriteLine($"{ex.Message}");
                return;
            }
        }

        private async Task<CustomProtocolResponse> SendRequestAsync(NetworkStream stream, CustomProtocolRequest request)
        {
            try
            {
                var requestData = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(request));
                await stream.WriteAsync(requestData, 0, requestData.Length);
                return await ReceiveResponseAsync(stream);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending request to server");
                return null;
            }
        }

        private async Task<CustomProtocolResponse> ReceiveResponseAsync(NetworkStream stream)
        {
            var buffer = new byte[8192];
            var bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
            var responseJson = Encoding.UTF8.GetString(buffer, 0, bytesRead);
            return JsonConvert.DeserializeObject<CustomProtocolResponse>(responseJson);
        }

    }

}
