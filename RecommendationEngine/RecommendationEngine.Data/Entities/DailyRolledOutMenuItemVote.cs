using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecommendationEngine.Data.Entities
{
    public class DailyRolledOutMenuItemVote : BaseEntity
    {
        [Key]
        public int DailyRolledOutMenuItemVoteId { get; set; }
        public int DailyRolledOutMenuItemId { get; set; }
        public int UserId { get; set; }

        public DailyRolledOutMenuItem DailyRolledOutMenuItem { get; set; }
        public User User { get; set; }

    }
}
