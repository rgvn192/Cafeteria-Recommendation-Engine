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
    public class Notification : BaseEntity
    {
        [Key]
        public int NotificationId { get; set; }

        [Column(TypeName = "varchar(500)")]
        public string Message { get; set; }
        public int UserId { get; set; }
        public int NotificationTypeId { get; set; }
        public User User { get; set; }
        public NotificationType NotificationType { get; set; }

    }
}
