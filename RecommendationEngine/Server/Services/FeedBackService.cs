using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RecommendationEngine.Data.Entities;
using RecommendationEngine.Data.Interface;
using Server.Interface;
using Server.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Services
{
    public class FeedBackService : CrudBaseService<Feedback>, IFeedbackService
    {

        private readonly ISentimentAnalyzer _sentimentAnalyzer;
        private readonly IKeywordExtractor _keywordExtractor;
        private readonly IMenuItemService _menuItemService;

        public FeedBackService(IMenuItemService menuItemService, ISentimentAnalyzer sentimentAnalyzer, IKeywordExtractor keywordExtractor, IFeedbackRepository feedBackRepository, IMapper mapper, ILogger<FeedBackService> logger) :
            base(feedBackRepository, mapper, logger)
        {
            _sentimentAnalyzer = sentimentAnalyzer;
            _keywordExtractor = keywordExtractor;
            _menuItemService = menuItemService;
        }

        protected override List<string> ModifiableProperties => new()
        {
            nameof(Feedback.Comment),
            nameof(Feedback.Rating)
        };

        public async Task AddFeedBackForMenuItem(FeedbackModel feedback)
        {
            await Add(feedback);
            await UpdateMenuItem(feedback);
        }

        private async Task UpdateMenuItem(FeedbackModel feedback)
        {
            decimal newSentimentScore = _sentimentAnalyzer.AnalyzeSentiment(feedback.Comment);
            var newKeywords = _keywordExtractor.ExtractKeywords(feedback.Comment);

            var menuItem = await GetMenuItemWithFeedbacks(feedback.MenuItemId);
            if (menuItem != null)
            {
                UpdateAverageRating(menuItem, feedback.Rating);
                UpdateUserLikeability(menuItem, newSentimentScore);
                UpdateComments(menuItem, newKeywords);

                await _menuItemService.Update(menuItem.MenuItemId, menuItem);
            }
        }

        private async Task<MenuItemModel> GetMenuItemWithFeedbacks(int menuItemId)
        {
            return await _menuItemService.GetById<MenuItemModel>(menuItemId, $"{nameof(MenuItem.Feedbacks)}");
        }

        private void UpdateAverageRating(MenuItemModel menuItem, int newRating)
        {
            int totalFeedbackCount = menuItem.Feedbacks.Count;
            decimal previousAverageRating = menuItem.AverageRating;
            menuItem.AverageRating = ((previousAverageRating * totalFeedbackCount) + newRating) / (totalFeedbackCount + 1);
        }

        private void UpdateUserLikeability(MenuItemModel menuItem, decimal newSentimentScore)
        {
            int totalFeedbackCount = menuItem.Feedbacks.Count + 1;
            decimal previousTotalSentimentScore = menuItem.UserLikeability * menuItem.Feedbacks.Count;
            decimal newTotalSentimentScore = previousTotalSentimentScore + newSentimentScore;
            menuItem.UserLikeability = newTotalSentimentScore / totalFeedbackCount;
        }

        private void UpdateComments(MenuItemModel menuItem, List<string> newKeywords)
        {
            var allKeywords = menuItem.Feedbacks
                .SelectMany(f => _keywordExtractor.ExtractKeywords(f.Comment))
                .Concat(newKeywords)
                .GroupBy(word => word)
                .OrderByDescending(group => group.Count())
                .Take(10)
                .Select(group => group.Key)
                .ToList();

            menuItem.Comments = string.Join(", ", allKeywords);
        }



    }
}
