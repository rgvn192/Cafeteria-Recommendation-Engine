using RecommendationEngine.Data.Entities;
using Server.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Interface
{
    public interface IFeedbackService : ICrudBaseService<Feedback>
    {
        Task AddFeedBackForMenuItem(FeedbackModel feedback);
    }
}
