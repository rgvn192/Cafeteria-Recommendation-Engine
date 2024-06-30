using Microsoft.EntityFrameworkCore.Metadata.Internal;
using RecommendationEngine.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Models.DTO
{
    public class NotificationModel
    {
        public int NotificationId { get; set; }
        public string Message { get; set; }
        public int UserId { get; set; }
        public int NotificationTypeId { get; set; }
        public User User { get; set; }
        public NotificationType NotificationType { get; set; }

    }
}
