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
    public static class MenuItemFeedbackCommandHandlers
    {
        public async static Task<CustomProtocolResponse> GiveFeedBackOnMenuItem(IServiceProvider serviceProvider, string body)
        {
            try
            {
                var request = JsonConvert.DeserializeObject<FeedbackModel>(body);

                using (var scope = serviceProvider.CreateScope())
                {
                    var feedbackService = scope.ServiceProvider.GetRequiredService<IFeedbackService>();

                    await feedbackService.AddFeedBackForMenuItem(request);
                }

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
