using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RecommendationEngine.Client.Common;
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
    public class ChefService : IChefService
    {
        private readonly ILogger<ChefService> _logger;

        public ChefService(ILogger<ChefService> logger)
        {
            _logger = logger;
        }

        public async Task ShowMenuAsync(NetworkStream stream, string role, int? userId) // Include the role parameter
        {
            while (true)
            {
                Console.WriteLine("Chef Menu:");
                Console.WriteLine("1. Get Recommendations for Menu Items");
                Console.WriteLine("2. Roll Out Menu For next day for Voting");
                Console.WriteLine("3. View Votes on Rolled Out Menu Items");
                Console.WriteLine("4. Get Menu Items");
                Console.WriteLine("5. Finalize Menu Item For next day");
                Console.WriteLine("6. Send Notification for Finalized Menu");
                Console.WriteLine("7. View Notifications");
                Console.WriteLine("8. Generate Discarded Menu List");
                Console.WriteLine("9. View Discarded Menu List");
                Console.WriteLine("10. Handle Discarded Menu");
                Console.WriteLine("x. Logout");
                Console.Write("Select an option: ");
                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        await GetRecommendation(stream, role); // Pass the role parameter
                        break;
                    case "2":
                        await RollOutMenuForNextDayForVoting(stream, role);
                        break;
                    case "3":
                        await ViewVotesOnRolledOutMenuItems(stream, role);
                        break;
                    case "4":
                        await GetMenuItemAsync(stream, role);
                        break;
                    case "5":
                        await FinalizeMenuItemForNextDay(stream, role);
                        break;
                    case "6":
                        await SendNotificationForFinalizedMenu(stream, role);
                        break;
                    case "7":
                        await CheckNotifications(stream, role, userId);
                        break;
                    case "8":
                        await GenerateMonthlyDiscardedMenuList(stream, role);
                        break;
                    case "9":
                        await ViewMonthlyDiscardedMenuList(stream, role);
                        break;
                    case "10":
                        await HandleDiscardedMenuItem(stream, role);
                        break;
                    case "x":
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Invalid option. Please try again.");
                        break;
                }
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

        private async Task CheckNotifications(NetworkStream stream, string role, int? userId) // Include the role parameter
        {
            try
            {
                if (userId == null)
                {
                    throw new ArgumentNullException(nameof(userId));
                }

                var customRequest = new CustomProtocolRequest
                {
                    Command = "GetNotificationsForUser",
                    Role = role, // Include the role in the request
                    Body = JsonConvert.SerializeObject(userId)
                };

                var response = await SendRequestAsync(stream, customRequest);

                if (response != null && response.Status == "Success")
                {
                    var notifications = JsonConvert.DeserializeObject<List<NotificationModel>>(response.Body);
                    if (notifications != null && notifications.Count != 0)
                    {
                        foreach (var notification in notifications)
                        {
                            Console.WriteLine($"{notification.Message}\n");
                        }
                    }
                    else
                    {
                        Console.WriteLine("No new notifications");
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

        private async Task ViewVotesOnRolledOutMenuItems(NetworkStream stream, string role) // Include the role parameter
        {
            try
            {
                var customRequest = new CustomProtocolRequest
                {
                    Command = "ViewVotesOnRolledOutMenuItems",
                    Role = role, // Include the role in the request
                    Body = null
                };

                var response = await SendRequestAsync(stream, customRequest);

                if (response != null && response.Status == "Success")
                {
                    var rolledOutMenuItems = JsonConvert.DeserializeObject<List<ViewVotesOnRolledOutMenuItemsResponse>>(response.Body);
                    if (rolledOutMenuItems != null)
                    {
                        Console.WriteLine($"DailyRolledOutMenuItemId\tMenuItemId\tVotes\tMealTypeId\tIsShortListed\n");
                        foreach (var item in rolledOutMenuItems)
                        {
                            Console.WriteLine($"\t\t{item.DailyRolledOutMenuItemId}\t\t\t{item.MenuItemId}\t{item.Votes}\t{item.MealTypeId}\t{item.IsShortListed}\n");
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

        private async Task FinalizeMenuItemForNextDay(NetworkStream stream, string role) // Include the role parameter
        {
            try
            {
                Console.Write("Enter Rolled Out Menu Item Id:");
                if (!int.TryParse(Console.ReadLine(), out var rolledOutMenuItemId))
                {
                    Console.WriteLine("Invalid. Please try again.");
                    return;
                }

                var customRequest = new CustomProtocolRequest
                {
                    Command = "ShortListDailyMenuItem",
                    Role = role, // Include the role in the request
                    Body = JsonConvert.SerializeObject(rolledOutMenuItemId)
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

        private async Task RollOutMenuForNextDayForVoting(NetworkStream stream, string role) // Include the role parameter
        {
            try
            {
                List<int> menuItemsForBreakfast = GetMenuItems(MealTypes.Breakfast);
                if (menuItemsForBreakfast == null) return;

                List<int> menuItemsForLunch = GetMenuItems(MealTypes.Lunch);
                if (menuItemsForLunch == null) return;

                List<int> menuItemsForDinner = GetMenuItems(MealTypes.Dinner);
                if (menuItemsForDinner == null) return;

                var menuItemsToRollOut = new List<DailyRolledOutMenuItem>();

                foreach (var item in menuItemsForBreakfast)
                {
                    var breakFastMenuItemToRollOut = new DailyRolledOutMenuItem()
                    {
                        MealTypeId = (int)MealTypes.Breakfast,
                        MenuItemId = item
                    };
                    menuItemsToRollOut.Add(breakFastMenuItemToRollOut);
                }

                foreach (var item in menuItemsForLunch)
                {
                    var lunchMenuItemToRollOut = new DailyRolledOutMenuItem()
                    {
                        MealTypeId = (int)MealTypes.Lunch,
                        MenuItemId = item
                    };
                    menuItemsToRollOut.Add(lunchMenuItemToRollOut);
                }

                foreach (var item in menuItemsForDinner)
                {
                    var dinnerMenuItemToRollOut = new DailyRolledOutMenuItem()
                    {
                        MealTypeId = (int)MealTypes.Dinner,
                        MenuItemId = item
                    };
                    menuItemsToRollOut.Add(dinnerMenuItemToRollOut);
                }

                var customRequest = new CustomProtocolRequest
                {
                    Command = "RollOutMenuForNextDayForVoting",
                    Role = role, // Include the role in the request
                    Body = JsonConvert.SerializeObject(menuItemsToRollOut)
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

        private async Task GetRecommendation(NetworkStream stream, string role) // Include the role parameter
        {
            try
            {
                Console.Write("Enter Menu Item Category Id :");
                if (!int.TryParse(Console.ReadLine(), out var menuItemCategoryId))
                {
                    Console.WriteLine("Invalid. Please try again.");
                    return;
                }

                Console.Write("Enter number of recommendations you need :");
                if (!int.TryParse(Console.ReadLine(), out var limit))
                {
                    Console.WriteLine("Invalid. Please try again.");
                    return;
                }

                var getRecommendationRequestModel = new GetRecommendationRequestModel()
                {
                    Limit = limit,
                    MenuItemCategoryId = menuItemCategoryId
                };

                var customRequest = new CustomProtocolRequest
                {
                    Command = "GetRecommendation",
                    Role = role, // Include the role in the request
                    Body = JsonConvert.SerializeObject(getRecommendationRequestModel)
                };

                var response = await SendRequestAsync(stream, customRequest);

                if (response != null && response.Status == "Success")
                {
                    var getRecommendationResponseModel = JsonConvert.DeserializeObject<List<GetRecommendationMenuItemResponse>>(response.Body);
                    if (getRecommendationResponseModel != null)
                    {
                        Console.WriteLine($"MenuItemId\tName\tPrice\titem.MenuItemCategoryId\tUserLikeability\tAverageRating\tComments");
                        foreach (var item in getRecommendationResponseModel)
                        {
                            Console.WriteLine($"{item.MenuItemId}\t{item.Name}\t{item.Price}\t{item.MenuItemCategoryId}\t{item.UserLikeability}\t{item.AverageRating}\t{item.Comments}");
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

        private async Task SendNotificationForFinalizedMenu(NetworkStream stream, string role) // Include the role parameter
        {
            try
            {
                var customRequest = new CustomProtocolRequest
                {
                    Command = "IssueNotificationForFinalizedMenu",
                    Role = role, // Include the role in the request
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

        private async Task GenerateMonthlyDiscardedMenuList(NetworkStream stream, string role) // Include the role parameter
        {
            try
            {
                var customRequest = new CustomProtocolRequest
                {
                    Command = "GenerateDiscardedMenuItems",
                    Role = role, // Include the role in the request
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

        private async Task ViewMonthlyDiscardedMenuList(NetworkStream stream, string role) // Include the role parameter
        {
            try
            {
                var customRequest = new CustomProtocolRequest
                {
                    Command = "GetDiscardedMenuItemsForCurrentMonth",
                    Role = role, // Include the role in the request
                    Body = null
                };

                var response = await SendRequestAsync(stream, customRequest);

                if (response != null && response.Status == "Success")
                {
                    var discardedMenuItemsResponse = JsonConvert.DeserializeObject<List<DiscardedMenuItemsResponse>>(response.Body);
                    if (discardedMenuItemsResponse != null)
                    {
                        Console.WriteLine($"DailyRolledOutMenuItemId\tName\tAverageRating\n");
                        foreach (var item in discardedMenuItemsResponse)
                        {
                            Console.WriteLine($"\t\t{item.DiscardedMenuItemId}\t\t{item.MenuItem.Name}\t{item.MenuItem.AverageRating}\n");
                        }
                    }
                }
                else { Console.WriteLine(response.Body); }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending request to server");
                Console.WriteLine($"{ex.Message}");
                return;
            }
        }

        private async Task HandleDiscardedMenuItem(NetworkStream stream, string role) // Include the role parameter
        {
            try
            {
                Console.Write("Enter Discarded Menu Item Id: ");
                if (!int.TryParse(Console.ReadLine(), out var discardMenuItemId))
                {
                    Console.WriteLine("Invalid ID. Please try again.");
                    return;
                }

                Console.Write("Want to make this available again? (Y/N): ");
                string input = Console.ReadLine()?.Trim().ToUpper(); // Read input and normalize to upper case

                bool makeAvailable;
                switch (input)
                {
                    case "Y":
                        makeAvailable = true;
                        break;
                    case "N":
                        makeAvailable = false;
                        break;
                    default:
                        Console.WriteLine("Invalid input. Please enter 'Y' or 'N'.");
                        return;
                }

                var handleDiscardedMenuItemRequest = new HandleDiscardedMenuItemRequest()
                {
                    DiscardedMenuItemId = discardMenuItemId,
                    MakeAvailable = makeAvailable
                };

                var customRequest = new CustomProtocolRequest
                {
                    Command = "HandleDiscardedMenuItem",
                    Role = role, // Include the role in the request
                    Body = JsonConvert.SerializeObject(handleDiscardedMenuItemRequest)
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

        private static List<int> GetMenuItems(MealTypes mealType)
        {
            Console.Write($"Enter Number of Menu Items to Roll-Out for {mealType}: ");
            if (!int.TryParse(Console.ReadLine(), out var numberOfMenuItems))
            {
                Console.WriteLine("Invalid. Please try again.");
                return null;
            }

            List<int> menuItems = new List<int>();
            Console.WriteLine($"Please Give {mealType} Menu details");
            for (int i = 1; i <= numberOfMenuItems; i++)
            {
                Console.Write($"Give MenuItem Id for {mealType} Item number {i}: ");
                if (!int.TryParse(Console.ReadLine(), out var menuItemId))
                {
                    Console.WriteLine("Invalid. Please try again.");
                    return null;
                }
                menuItems.Add(menuItemId);
            }

            return menuItems;
        }
    }
}
