using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecommendationEngine.Client.Models
{
    public class User
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string RoleName { get; set; }

    }

    public class LoginResponse
    {
        public string status { get; set; }
        public User User { get; set; }

    }

    public class ViewMenuResponseOfThirdParty
    {
        public string status { get; set; }
        public List<MenuItemOfThirdParty> menu { get; set; }
    }

    public class MenuItemOfThirdParty
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int AvailabilityStatus { get; set; }
    }

}
