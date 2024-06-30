using AutoMapper;
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
        public FeedBackService(IFeedbackRepository feedBackRepository, IMapper mapper, ILogger<FeedBackService> logger) :
            base(feedBackRepository, mapper, logger)
        {
        }

        protected override List<string> ModifiableProperties => new()
        {
            nameof(Feedback.Comment),
            nameof(Feedback.Rating)
        };

        public async Task AddFeedBackForMenuItem(FeedbackModel feedback)
        {
            await Add(feedback);
            //Update Menu Item on the basis of Comment and Rating.
        }
    }
}
