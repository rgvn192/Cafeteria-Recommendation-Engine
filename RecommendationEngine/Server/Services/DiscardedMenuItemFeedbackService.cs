using AutoMapper;
using Microsoft.Extensions.Logging;
using RecommendationEngine.Data.Entities;
using RecommendationEngine.Data.Interface;
using Server.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Services
{
    public class DiscardedMenuItemFeedbackService : CrudBaseService<DiscardedMenuItemFeedback>, IDiscardedMenuItemFeedbackService
    {
        public DiscardedMenuItemFeedbackService(IDiscardedMenuItemFeedbackRepository discardedMenuItemFeedbackRepository, IMapper mapper, ILogger<DiscardedMenuItemFeedbackService> logger) :
            base(discardedMenuItemFeedbackRepository, mapper, logger)
        {
        }

        protected override List<string> ModifiableProperties => new()
        {
        };

    }
}
