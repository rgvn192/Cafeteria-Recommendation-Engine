using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace RecommendationEngine.Data.Entities
{
    public class User : BaseEntity
    {
        [Key]
        public int UserId { get; set; }

        [Column(TypeName = ("nvarchar(1024)"))]
        public string Name { get; set; }

        [Column(TypeName = ("nvarchar(1024)"))]
        public string Email { get; set; }

        public int RoleId { get; set; }
        public DateTime LastSeenNotificationAt { get; set; }
        public Role Role { get; set; }

        public bool IsEnabled { get; set; }

        public List<Notification> Notifications { get; set; }
        public List<Feedback> Feedbacks { get; set; }
        public List<DailyRolledOutMenuItemVote> DailyRolledOutMenuItemVotes { get; set; }
        public List<UserPreference> Preferences { get; set; }
        public List<DiscardedMenuItemFeedback> DiscardedMenuItemFeedbacks { get; set; }
    }
}
