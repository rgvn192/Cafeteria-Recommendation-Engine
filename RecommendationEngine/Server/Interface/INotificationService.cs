using RecommendationEngine.Data.Entities;
using Server.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Interface
{
    public interface INotificationService : ICrudBaseService<Notification>
    {
        Task IssueNotifications(NotificationTypes notificationType, string message, List<int> roleIds);
        Task<List<NotificationModel>> GetNotificationsForUser(int userId);
    }
}
