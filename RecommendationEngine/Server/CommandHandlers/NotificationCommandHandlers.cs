using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using RecommendationEngine.Common.Utils;
using Server.Interface;
using Server.Models.DTO;
using Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RecommendationEngine.Data.Entities;
using Server.Models.Response;

namespace Server.CommandHandlers
{
    public static class NotificationCommandHandlers
    {
        public async static Task<CustomProtocolResponse> GetNotificationsForUser(IServiceProvider serviceProvider, string body)
        {
            try
            {
                var userId = JsonConvert.DeserializeObject<int>(body);
                List<NotificationModel> notifications;
                using (var scope = serviceProvider.CreateScope())
                {
                    var notificationService = scope.ServiceProvider.GetRequiredService<INotificationService>();

                    notifications = await notificationService.GetNotificationsForUser(userId);
                }

                return new CustomProtocolResponse
                {
                    Status = "Success",
                    Body = JsonHelper.SerializeObjectIgnoringCycles(notifications)
                };
            }
            catch (Exception ex)
            {
                return new CustomProtocolResponse
                {
                    Status = "Failure",
                    Body = JsonConvert.SerializeObject(ex.Message)
                };
            }
        }

        public async static Task<CustomProtocolResponse> IssueNotificationForFinalizedMenu(IServiceProvider serviceProvider, string body)
        {
            try
            {
                using (var scope = serviceProvider.CreateScope())
                {
                    var notificationService = scope.ServiceProvider.GetRequiredService<INotificationService>();

                    var tomorrow = DateTime.UtcNow.Date.AddDays(1);
                    string tomorrowDate = tomorrow.ToShortDateString();
                    List<int> roles = new List<int>()
                    {
                        (int)Roles.User
                    };
                    await notificationService.IssueNotifications(NotificationTypes.FinalizeMenu, $"The Menu for {tomorrowDate} has been finalized.", roles);
                }

                var response = new CommonServerResponse
                {
                    Status = "Success",
                    Message = "Successfully Issued Notification for Finalized Menu"
                };

                return new CustomProtocolResponse
                {
                    Status = "Success",
                    Body = JsonConvert.SerializeObject(response)
                };
            }
            catch (Exception ex)
            {
                return new CustomProtocolResponse
                {
                    Status = "Failure",
                    Body = JsonConvert.SerializeObject(ex.Message)
                };
            }
        }
    }
}
