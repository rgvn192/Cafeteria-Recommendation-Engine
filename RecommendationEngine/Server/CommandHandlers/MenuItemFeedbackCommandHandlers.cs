using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Server.Interface;
using Server.Models.DTO;
using Server.Models.Response;
using Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.CommandHandlers
{
    public class MenuItemFeedbackCommandHandlers
    {
        private readonly IFeedbackService _feedbackService;

        public MenuItemFeedbackCommandHandlers(IFeedbackService feedbackService)
        {
            _feedbackService = feedbackService;
        }

        public async Task<CustomProtocolResponse> GiveFeedBackOnMenuItem(string body)
        {
            try
            {
                var request = JsonConvert.DeserializeObject<FeedbackModel>(body);

                await _feedbackService.AddFeedBackForMenuItem(request);
                
                var response = new CommonServerResponse
                {
                    Status = "Success",
                    Message = "Successfully Submitted Feedback."
                };

                return new CustomProtocolResponse
                {
                    Status = "Success",
                    Body = JsonConvert.SerializeObject(response)
                };
            }
            catch (Exception ex)
            {
                return new CustomProtocolResponse
                {
                    Status = "Failure",
                    Body = JsonConvert.SerializeObject(ex.Message)
                };
            }
        }
    }
}
