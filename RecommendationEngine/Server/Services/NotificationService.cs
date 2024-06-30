using AutoMapper;
using Microsoft.Extensions.Logging;
using RecommendationEngine.Common.Exceptions;
using RecommendationEngine.Common.Utils;
using RecommendationEngine.Data.Entities;
using RecommendationEngine.Data.Interface;
using Server.Interface;
using Server.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Server.Services
{
    public class NotificationService : CrudBaseService<Notification>, INotificationService
    {
        private readonly IUserService _userService;

        public NotificationService(IUserService userService, INotificationRepository notificationRepository, IMapper mapper, ILogger<NotificationService> logger) :
            base(notificationRepository, mapper, logger)
        {
            _userService = userService;
        }

        protected override List<string> ModifiableProperties => new()
        {
        };

        public async Task IssueNotifications(NotificationTypes notificationType, string message, List<int> roleIds)
        {
            Expression<Func<User, bool>> predicate = u => roleIds.Contains(u.RoleId);
            var users = await _userService.GetList<User>(predicate: predicate);

            var notifications = users.Select(user => new Notification
            {
                NotificationTypeId = (int)notificationType,
                Message = message,
                UserId = user.UserId
            }).ToList();

            await AddRange(notifications);
        }

        public async Task<List<NotificationModel>> GetNotificationsForUser(int userId)
        {
            var user = await GetById<User>(userId) ?? throw new AppException(ErrorResponse.ErrorEnum.NotFound,
                    LogExtensions.GetLogMessage(nameof(GetNotificationsForUser), null, "No such user found"));

            Expression<Func<Notification, bool>> predicate = n => n.UserId == user.UserId && n.CreatedDatetime > user.LastSeenNotificationAt;
            var notifications = await GetList<NotificationModel>(predicate: predicate);

            return notifications;
        }
    }
}
