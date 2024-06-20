using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecommendationEngine.Data.Entities
{
    public class NotificationType : BaseEntity
    {
        [Key]
        public int NotificationTypeId { get; set; }

        [Column(TypeName = ("nvarchar(30)"))]
        public NotificationTypes Name { get; set; }

        public List<Notification> Notifications { get; set; }
    }

    public enum NotificationTypes
    {
        NewMenuItemAdded,
        MenuItemAvailabilityUpdated,
        MenuItemVoting,
        FinalizeMenu
    }
}
