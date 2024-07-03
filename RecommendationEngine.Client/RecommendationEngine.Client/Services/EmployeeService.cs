﻿using Microsoft.Extensions.Logging;
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
    public class EmployeeService : IEmployeeService
    {
        private readonly ILogger<EmployeeService> _logger;

        public EmployeeService(ILogger<EmployeeService> logger)
        {
            _logger = logger;
        }

        public async Task ShowMenuAsync(NetworkStream stream, string role, int? userId) // Include the role parameter
        {
            while (true)
            {
                Console.WriteLine("Employee Menu:");
                Console.WriteLine("1. Check Notifications");
                Console.WriteLine("2. Check for Rolled out menu for next day");
                Console.WriteLine("3. Vote for Menu Item for next day");
                Console.WriteLine("4. View Finalized Menu Items for next day");
                Console.WriteLine("5. Give Feedback for Menu Item");
                Console.WriteLine("6. View Monthly Discarded Menu List");
                Console.WriteLine("7. Give FeedBack For Discarded Menu Item");

                Console.WriteLine("x. Logout");
                Console.Write("Select an option: ");
                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        await CheckNotifications(stream, role, userId); // Pass the role parameter
                        break;
                    case "2":
                        await CheckForRolledOutMenuForNextDay(stream, role, userId); // Pass the role parameter
                        break;
                    case "3":
                        await VoteForMenuItemForNextDay(stream, role, userId);
                        break;
                    case "4":
                        await ViewFinalizedMenuItems(stream, role);
                        break;
                    case "5":
                        await GiveFeedBackForMenuItem(stream, role, userId);
                        break;
                    case "6":
                        await ViewMonthlyDiscardedMenuList(stream, role);
                        break;
                    case "7":
                        await GiveFeedBackForDiscardedMenuItem(stream, role, userId);
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
            }

        }

        private async Task CheckForRolledOutMenuForNextDay(NetworkStream stream, string role, int? userId) // Include the role parameter
        {
            try
            {

                if (userId == null)
                {
                    throw new ArgumentNullException(nameof(userId));
                }

                var customRequest = new CustomProtocolRequest
                {
                    Command = "GetRolledOutMenuItemsOfTodayForUser",
                    Role = role, // Include the role in the request
                    Body = JsonConvert.SerializeObject(userId)
                };

                var response = await SendRequestAsync(stream, customRequest);

                if (response != null && response.Status == "Success")
                {
                    var rolledOutMenuItemResponse = JsonConvert.DeserializeObject<List<RolledOutMenuItem>>(response.Body);
                    if (rolledOutMenuItemResponse != null)
                    {
                        Console.WriteLine($"Rolled Out Menu Item Id\tName\tPrice\tAverageRating\tMealType\n");

                        foreach (var item in rolledOutMenuItemResponse)
                        {
                            Console.WriteLine($"{item.DailyRolledOutMenuItemId}\t\t{item.MenuItem.Name}\t{item.MenuItem.Price}\t{item.MenuItem.AverageRating}\t{item.MenuItem.Comments}\t{item.MealType.Name}\n");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending request to server");
                Console.WriteLine($"{ex.Message}");
            }

        }

        private async Task VoteForMenuItemForNextDay(NetworkStream stream, string role, int? userId) // Include the role parameter
        {
            try
            {
                if (userId == null)
                {
                    throw new ArgumentNullException(nameof(userId));
                }

                Console.Write("Enter Rolled Out Menu Item Id :");
                if (!int.TryParse(Console.ReadLine(), out var rolledOutItemId))
                {
                    Console.WriteLine("Invalid. Please try again.");
                    return;
                }

                var voteForDailyMenuItemRequest = new VoteForDailyMenuItemRequest()
                {
                    DailyRolledOutMenuItemId = rolledOutItemId,
                    UserId = userId.Value
                };

                var customRequest = new CustomProtocolRequest
                {
                    Command = "VoteForDailyMenuItem",
                    Role = role, // Include the role in the request
                    Body = JsonConvert.SerializeObject(voteForDailyMenuItemRequest)
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
            }

        }

        private async Task ViewFinalizedMenuItems(NetworkStream stream, string role) // Include the role parameter
        {
            try
            {
                var customRequest = new CustomProtocolRequest
                {
                    Command = "ViewFinalizedRolledOutMenuItems",
                    Role = role, // Include the role in the request
                };

                var response = await SendRequestAsync(stream, customRequest);

                if (response != null && response.Status == "Success")
                {
                    var rolledOutMenuItemResponse = JsonConvert.DeserializeObject<List<ViewFinalizedRolledOutMenuItemsResponse>>(response.Body);
                    if (rolledOutMenuItemResponse != null)
                    {
                        Console.WriteLine($"Name\tPrice\tMeal Type\n");

                        foreach (var item in rolledOutMenuItemResponse)
                        {
                            Console.WriteLine($"{item.MenuItem.Name}\t{item.MenuItem.Price}\t{item.Mealtype}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending request to server");
                Console.WriteLine($"{ex.Message}");
            }

        }

        private async Task GiveFeedBackForMenuItem(NetworkStream stream, string role, int? userId) // Include the role parameter
        {
            try
            {
                if (userId == null)
                {
                    throw new ArgumentNullException(nameof(userId));
                }

                Console.Write("Enter Menu Item Id :");
                if (!int.TryParse(Console.ReadLine(), out var menuItemId))
                {
                    Console.WriteLine("Invalid. Please try again.");
                    return;
                }

                int rating;
                while (true)
                {
                    Console.Write("Enter Rating (1-5): ");
                    if (int.TryParse(Console.ReadLine(), out rating) && rating >= 1 && rating <= 5)
                    {
                        break;
                    }
                    Console.WriteLine("Invalid rating. Please enter a number between 1 and 5.");
                }

                string comment;
                while (true)
                {
                    Console.Write("Enter Comment (max 200 characters): ");
                    comment = Console.ReadLine();
                    if (!string.IsNullOrEmpty(comment) && comment.Length <= 200)
                    {
                        break;
                    }
                    Console.WriteLine("Invalid comment. Please enter a comment with a maximum length of 200 characters.");
                }

                var feedback = new FeedbackModel
                {
                    UserId = userId.Value,
                    MenuItemId = menuItemId,
                    Rating = rating,
                    Comment = comment
                };

                var customRequest = new CustomProtocolRequest
                {
                    Command = "GiveFeedBackOnMenuItem",
                    Role = role, // Include the role in the request
                    Body = JsonConvert.SerializeObject(feedback)
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
            }

        }

        private async Task GiveFeedBackForDiscardedMenuItem(NetworkStream stream, string role, int? userId) // Include the role parameter
        {
            try
            {
                if (userId == null)
                {
                    throw new ArgumentNullException(nameof(userId));
                }

                Console.Write("Enter Discarded Menu Item Id :");
                if (!int.TryParse(Console.ReadLine(), out var discardedMenuItemId))
                {
                    Console.WriteLine("Invalid. Please try again.");
                    return;
                }

                StringBuilder answers = new StringBuilder();
                string[] questions = {
                    "What didn't you like about the food item?",
                    "How would you like the food item to taste?",
                    "Share your recipe:"
                };

                foreach (string question in questions)
                {
                    Console.WriteLine(question);
                    answers.AppendLine(Console.ReadLine());
                }

                var feedback = new DiscardedMenuItemFeedback
                {
                    UserId = userId.Value,
                    DiscardedMenuItemId = discardedMenuItemId,
                    Feedback = answers.ToString(),
                };

                var customRequest = new CustomProtocolRequest
                {
                    Command = "GiveFeedBackOnDiscardedMenuItem",
                    Role = role, // Include the role in the request
                    Body = JsonConvert.SerializeObject(feedback)
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
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending request to server");
                Console.WriteLine($"{ex.Message}");
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
            var buffer = new byte[32768];
            var bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
            var responseJson = Encoding.UTF8.GetString(buffer, 0, bytesRead);
            return JsonConvert.DeserializeObject<CustomProtocolResponse>(responseJson);
        }
    }
}
