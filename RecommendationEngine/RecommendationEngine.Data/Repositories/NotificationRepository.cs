using Microsoft.Extensions.Logging;
using RecommendationEngine.Data.Entities;
using RecommendationEngine.Data.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecommendationEngine.Data.Repositories
{
    internal class NotificationRepository : CrudBaseRepository<Notification>, INotificationRepository
    {
        public NotificationRepository(AppDbContext appDbContext, ILogger<NotificationRepository> logger) :
            base(appDbContext, logger)
        {

        }

    }
}