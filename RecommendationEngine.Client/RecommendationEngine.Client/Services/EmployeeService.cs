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

        public async Task ShowMenuAsync(NetworkStream stream, string role, int? userId)
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
                Console.WriteLine("8. Show my Food Preferences");

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
                    case "8":
                        await GetUserPreferences(stream, role, userId);
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

        private async Task GetUserPreferences(NetworkStream stream, string role, int? userId)
        {
            try
            {
                if (userId == null)
                {
                    throw new ArgumentNullException(nameof(userId));
                }

                var userPreferences = await FetchUserPreferences(stream, role, userId.Value);
                DisplayUserPreferences(userPreferences);
                await HandleUserPreferenceModification(stream, role, userId.Value);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending request to server");
                Console.WriteLine($"{ex.Message}");
            }
        }

        private async Task<List<UserPreferenceModel>> FetchUserPreferences(NetworkStream stream, string role, int userId)
        {
            var customRequest = new CustomProtocolRequest
            {
                Command = "GetUserPreferences",
                Role = role,
                Body = JsonConvert.SerializeObject(userId)
            };

            var response = await SendRequestAsync(stream, customRequest);

            if (response != null && response.Status == "Success")
            {
                return JsonConvert.DeserializeObject<List<UserPreferenceModel>>(response.Body);
            }

            return new List<UserPreferenceModel>();
        }

        private static void DisplayUserPreferences(List<UserPreferenceModel> userPreferences)
        {
            if (userPreferences != null && userPreferences.Count > 0)
            {
                const int userPreferenceIdWidth = 20;
                const int characteristicNameWidth = 30;

                Console.WriteLine(
                    $"{nameof(UserPreferenceModel.UserPreferenceId),-userPreferenceIdWidth}" +
                    $"{nameof(UserPreferenceModel.Characteristic.Name),-characteristicNameWidth}"
                );

                foreach (var userPreference in userPreferences)
                {
                    Console.WriteLine(
                        $"{userPreference.UserPreferenceId,-userPreferenceIdWidth}" +
                        $"{userPreference.Characteristic?.Name ?? "N/A",-characteristicNameWidth}"
                    );
                }
            }
            else
            {
                Console.WriteLine("No Food Preferences");
            }
        }

        private async Task HandleUserPreferenceModification(NetworkStream stream, string role, int userId)
        {
            while (true)
            {
                Console.WriteLine("Do you want to:");
                Console.WriteLine("1. Add preferences");
                Console.WriteLine("2. Remove preferences");
                Console.WriteLine("x. Return to main menu");
                Console.Write("Select an option: ");
                string choice = Console.ReadLine()?.ToLower();

                switch (choice)
                {
                    case "1":
                        await AddUserPreference(stream, role, userId);
                        break;
                    case "2":
                        await RemoveUserPreference(stream, role, userId);
                        break;
                    case "x":
                        return; // Exit the function to return to the main menu
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
        }

        private async Task AddUserPreference(NetworkStream stream, string role, int userId)
        {
            var allCharacteristics = await FetchAllCharacteristics(stream, role);
            DisplayAllCharacteristics(allCharacteristics);

            Console.WriteLine("Enter the CharacteristicId you want to add:");
            if (int.TryParse(Console.ReadLine(), out int characteristicId))
            {

                var userpreferenceModel = new UserPreferenceModel()
                {
                    UserId = userId,
                    CharacteristicId = characteristicId
                };

                var addPreferenceRequest = new CustomProtocolRequest
                {
                    Command = "AddUserPreference",
                    Role = role,
                    Body = JsonConvert.SerializeObject(userpreferenceModel)
                };

                var addResponse = await SendRequestAsync(stream, addPreferenceRequest);

                if (addResponse != null && addResponse.Status == "Success")
                {
                    Console.WriteLine("Preference added successfully.");
                }
                else
                {
                    Console.WriteLine("Failed to add preference.");
                }
            }
            else
            {
                Console.WriteLine("Invalid CharacteristicId.");
            }
        }

        private async Task<List<Characteristic>> FetchAllCharacteristics(NetworkStream stream, string role)
        {
            var allCharacteristicsRequest = new CustomProtocolRequest
            {
                Command = "GetCharacteristics",
                Role = role,
            };

            var response = await SendRequestAsync(stream, allCharacteristicsRequest);

            if (response != null && response.Status == "Success")
            {
                return JsonConvert.DeserializeObject<List<Characteristic>>(response.Body);
            }

            return new List<Characteristic>();
        }

        private static void DisplayAllCharacteristics(List<Characteristic> allCharacteristics)
        {
            if (allCharacteristics != null && allCharacteristics.Count > 0)
            {
                const int characteristicIdWidth = 20;
                const int characteristicNameWidth = 30;

                Console.WriteLine(
                    $"{nameof(Characteristic.CharacteristicId),-characteristicIdWidth}" +
                    $"{nameof(Characteristic.Name),-characteristicNameWidth}"
                );

                foreach (var characteristic in allCharacteristics)
                {
                    Console.WriteLine(
                        $"{characteristic.CharacteristicId,-characteristicIdWidth}" +
                        $"{characteristic.Name,-characteristicNameWidth}"
                    );
                }
            }
            else
            {
                Console.WriteLine("No characteristics to display.");
            }
        }

        private async Task RemoveUserPreference(NetworkStream stream, string role, int userId)
        {
            Console.WriteLine("Enter the UserPreferenceId you want to remove:");
            if (int.TryParse(Console.ReadLine(), out int userPreferenceId))
            {

                var removePreferenceRequest = new RemoveUserPreferenceRequest()
                {
                    UserId = userId,
                    UserPreferenceId = userPreferenceId
                };

                var request = new CustomProtocolRequest
                {
                    Command = "DeleteUserPreference",
                    Role = role,
                    Body = JsonConvert.SerializeObject(removePreferenceRequest)
                };

                var removeResponse = await SendRequestAsync(stream, request);

                if (removeResponse != null && removeResponse.Status == "Success")
                {
                    Console.WriteLine("Preference removed successfully.");
                }
                else
                {
                    Console.WriteLine("Failed to remove preference.");
                }
            }
            else
            {
                Console.WriteLine("Invalid UserPreferenceId.");
            }
        }

        private async Task CheckNotifications(NetworkStream stream, string role, int? userId)
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

        private async Task CheckForRolledOutMenuForNextDay(NetworkStream stream, string role, int? userId)
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
                    DisplayRolledOutMenuItems(rolledOutMenuItemResponse);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending request to server");
                Console.WriteLine($"{ex.Message}");
            }
        }

        private static void DisplayRolledOutMenuItems(List<RolledOutMenuItem> rolledOutMenuItemResponse)
        {
            if (rolledOutMenuItemResponse != null && rolledOutMenuItemResponse.Count > 0)
            {
                const int rolledOutMenuItemIdWidth = 25;
                const int nameWidth = 25;
                const int priceWidth = 10;
                const int averageRatingWidth = 15;
                const int mealTypeWidth = 15;

                Console.WriteLine(
                    $"{nameof(RolledOutMenuItem.DailyRolledOutMenuItemId),-rolledOutMenuItemIdWidth}" +
                    $"{nameof(RolledOutMenuItem.MenuItem.Name),-nameWidth}" +
                    $"{nameof(RolledOutMenuItem.MenuItem.Price),-priceWidth}" +
                    $"{nameof(RolledOutMenuItem.MenuItem.AverageRating),-averageRatingWidth}" +
                    $"{nameof(RolledOutMenuItem.MealType.Name),-mealTypeWidth}"
                );

                foreach (var item in rolledOutMenuItemResponse)
                {
                    Console.WriteLine(
                        $"{item.DailyRolledOutMenuItemId,-rolledOutMenuItemIdWidth}" +
                        $"{item.MenuItem.Name,-nameWidth}" +
                        $"{item.MenuItem.Price,-priceWidth}" +
                        $"{item.MenuItem.AverageRating,-averageRatingWidth}" +
                        $"{item.MealType.Name,-mealTypeWidth}");
                }
            }
            else
            {
                Console.WriteLine("No rolled out menu items to display.");
            }
        }

        private async Task VoteForMenuItemForNextDay(NetworkStream stream, string role, int? userId)
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

        private async Task ViewFinalizedMenuItems(NetworkStream stream, string role)
        {
            try
            {
                var customRequest = new CustomProtocolRequest
                {
                    Command = "ViewFinalizedRolledOutMenuItems",
                    Role = role, 
                };

                var response = await SendRequestAsync(stream, customRequest);

                if (response != null && response.Status == "Success")
                {
                    var rolledOutMenuItemResponse = JsonConvert.DeserializeObject<List<ViewFinalizedRolledOutMenuItemsResponse>>(response.Body);
                    DisplayFinalizedRolledOutMenuItems(rolledOutMenuItemResponse);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending request to server");
                Console.WriteLine($"{ex.Message}");
            }

        }

        private static void DisplayFinalizedRolledOutMenuItems(List<ViewFinalizedRolledOutMenuItemsResponse> rolledOutMenuItemResponse)
        {
            if (rolledOutMenuItemResponse != null && rolledOutMenuItemResponse.Count > 0)
            {
                // Define the column widths
                const int nameWidth = 25;
                const int priceWidth = 10;
                const int mealTypeWidth = 15;

                // Print the header with proper alignment
                Console.WriteLine(
                    $"{nameof(ViewFinalizedRolledOutMenuItemsResponse.MenuItem.Name),-nameWidth}" +
                    $"{nameof(ViewFinalizedRolledOutMenuItemsResponse.MenuItem.Price),-priceWidth}" +
                    $"{nameof(ViewFinalizedRolledOutMenuItemsResponse.Mealtype),-mealTypeWidth}"
                );

                // Print each item with proper alignment
                foreach (var item in rolledOutMenuItemResponse)
                {
                    Console.WriteLine(
                        $"{item.MenuItem.Name,-nameWidth}" +
                        $"{item.MenuItem.Price,-priceWidth:C2}" + // Format price as currency
                        $"{item.Mealtype,-mealTypeWidth}"
                    );
                }
            }
            else
            {
                Console.WriteLine("No rolled out menu items to display.");
            }
        }

        private async Task GiveFeedBackForMenuItem(NetworkStream stream, string role, int? userId)
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

        private async Task GiveFeedBackForDiscardedMenuItem(NetworkStream stream, string role, int? userId)
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
                    Role = role,
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

        private async Task ViewMonthlyDiscardedMenuList(NetworkStream stream, string role)
        {
            try
            {
                var customRequest = new CustomProtocolRequest
                {
                    Command = "GetDiscardedMenuItemsForCurrentMonth",
                    Role = role,
                    Body = null
                };

                var response = await SendRequestAsync(stream, customRequest);

                if (response != null && response.Status == "Success")
                {
                    var discardedMenuItemsResponse = JsonConvert.DeserializeObject<List<DiscardedMenuItemsResponse>>(response.Body);
                    DisplayDiscardedMenuItems(discardedMenuItemsResponse);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending request to server");
                Console.WriteLine($"{ex.Message}");
            }
        }

        private static void DisplayDiscardedMenuItems(List<DiscardedMenuItemsResponse> discardedMenuItemsResponse)
        {
            if (discardedMenuItemsResponse != null && discardedMenuItemsResponse.Count > 0)
            {
                const int discardedMenuItemIdWidth = 25;
                const int nameWidth = 20;
                const int averageRatingWidth = 15;

                Console.WriteLine(
                    $"{nameof(DiscardedMenuItemsResponse.DiscardedMenuItemId),-discardedMenuItemIdWidth}" +
                    $"{nameof(DiscardedMenuItemsResponse.MenuItem.Name),-nameWidth}" +
                    $"{nameof(DiscardedMenuItemsResponse.MenuItem.AverageRating),-averageRatingWidth}"
                );

                foreach (var item in discardedMenuItemsResponse)
                {
                    Console.WriteLine(
                        $"{item.DiscardedMenuItemId,-discardedMenuItemIdWidth}" +
                        $"{item.MenuItem.Name,-nameWidth}" +
                        $"{item.MenuItem.AverageRating,-averageRatingWidth}"
                    );
                }
            }
            else
            {
                Console.WriteLine("No discarded menu items to display.");
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
            using (var memoryStream = new MemoryStream())
            {
                var buffer = new byte[32768];
                int bytesRead;

                while ((bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length)) > 0)
                {
                    memoryStream.Write(buffer, 0, bytesRead);

                    if (!stream.DataAvailable)
                    {
                        break;
                    }
                }

                var responseJson = Encoding.UTF8.GetString(memoryStream.ToArray());

                return JsonConvert.DeserializeObject<CustomProtocolResponse>(responseJson);
            }
        }
    }
}
