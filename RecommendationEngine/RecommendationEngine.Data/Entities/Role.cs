using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecommendationEngine.Data.Entities
{
    public class Role : BaseEntity
    {
        [Key]
        public int RoleId { get; set; }

        [Column(TypeName = ("nvarchar(1024)"))]
        public Roles Name { get; set; }
        public List<User> Users { get; set; } = new();
    }

    public enum Roles
    {
        Chef,
        Admin,
        User
    }
}
