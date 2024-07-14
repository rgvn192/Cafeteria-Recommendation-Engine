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
    public class NotificationCommandHandlers
    {
        private readonly INotificationService _notificationService;

        public NotificationCommandHandlers(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        public async Task<CustomProtocolResponse> GetNotificationsForUser(string body)
        {
            try
            {
                var userId = JsonConvert.DeserializeObject<int>(body);
                List<NotificationModel> notifications;

                notifications = await _notificationService.GetNotificationsForUser(userId);

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

        public async Task<CustomProtocolResponse> IssueNotificationForFinalizedMenu(string body)
        {
            try
            {
                var tomorrow = DateTime.UtcNow.Date.AddDays(1);
                string tomorrowDate = tomorrow.ToShortDateString();
                List<int> roles = new()
                {
                    (int)Roles.User
                };
                await _notificationService.IssueNotifications(NotificationTypes.FinalizeMenu, $"The Menu for {tomorrowDate} has been finalized.", roles);

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
