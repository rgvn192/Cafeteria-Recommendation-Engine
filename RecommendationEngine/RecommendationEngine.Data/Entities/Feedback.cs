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
    public class Feedback : BaseEntity
    {
        [Key]
        public int FeedbackId { get; set; }
        public int Rating { get; set; }

        [Column(TypeName = "varchar(70)")]
        public string Comment { get; set; }
        public int UserId { get; set; }
        public int MenuItemId { get; set; }

        public User User { get; set; }
        public MenuItem MenuItem { get; set; }
    }
}
