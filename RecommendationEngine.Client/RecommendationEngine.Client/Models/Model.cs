using RecommendationEngine.Client.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecommendationEngine.Client.Models
{
    public class CustomProtocolRequest
    {
        public string Command { get; set; }
        public string Role { get; set; } // Ensure the Role property is included
        public string Body { get; set; }
    }

    public class CustomProtocolResponse
    {
        public string Status { get; set; }
        public string Body { get; set; }
    }

    public class LoginResponseModel
    {
        public string Role { get; set; }
        public int UserId { get; set; }
    }

    public class LoginRequestModel
    {
        public string UserId { get; set; }
        public string Name { get; set; }
    }

    public class MenuItemModel
    {
        public int MenuItemId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int MenuItemCategoryId { get; set; }
    }

    public class GetMenuItemsResponseModel
    {
        public List<MenuItemModel> MenuItems { get; set; } = new List<MenuItemModel>();
    }

    public class GetMenuItemsRequestModel
    {
        public int Limit { get; set; }
        public int Offset { get; set; }

    }

    public class GetRecommendationRequestModel
    {
        public int Limit { get; set; }
        public int MenuItemCategoryId { get; set; }

    }

    public class DailyRolledOutMenuItem
    {
        public int? DailyRolledOutMenuItemId { get; set; }
        public bool? IsShortListed { get; set; }
        public int MenuItemId { get; set; }
        public int MealTypeId { get; set; }

    }

    public class GetRecommendationMenuItemResponse
    {
        public int MenuItemId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string? Comments { get; set; }
        public bool IsAvailable { get; set; }
        public int MenuItemCategoryId { get; set; }

        public decimal UserLikeability { get; set; }

        public decimal AverageRating { get; set; }
    }

    public class MealType
    {
        public int MealTypeId { get; set; }

        public MealTypes Name { get; set; }
    }

    public class RolledOutMenuItem
    {
        public int DailyRolledOutMenuItemId { get; set; }

        public GetRecommendationMenuItemResponse MenuItem { get; set; }

        public MealType MealType { get; set; }
    }

    public class ViewVotesOnRolledOutMenuItemsResponse
    {
        public int DailyRolledOutMenuItemId { get; set; }
        public bool IsShortListed { get; set; }
        public int MenuItemId { get; set; }
        public int MealTypeId { get; set; }

        public int Votes { get; set; }
    }

    public class VoteForDailyMenuItemRequest
    {
        public int DailyRolledOutMenuItemId { get; set; }
        public int UserId { get; set; }
    }

    public class ViewFinalizedRolledOutMenuItemsResponse
    {
        public string Mealtype { get; set; }
        public MenuItemModel MenuItem { get; set; }
    }

    public class FeedbackModel
    {
        public int FeedbackId { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
        public int UserId { get; set; }
        public int MenuItemId { get; set; }

    }

    public class NotificationModel
    {
        public string Message { get; set; }
    }

    public class DiscardedMenuItemsResponse
    {
        public int DiscardedMenuItemId { get; set; }

        public int MenuItemId { get; set; }

        public GetRecommendationMenuItemResponse MenuItem { get; set; }
    }

    public class HandleDiscardedMenuItemRequest
    {
        public int DiscardedMenuItemId { get; set; }
        public bool MakeAvailable { get; set; }
    }

    public class DiscardedMenuItemFeedback
    {
        public int DiscardedMenuItemFeedbackId { get; set; }

        public int DiscardedMenuItemId { get; set; }
        public int UserId { get; set; }

        public string Feedback { get; set; }

    }
}
