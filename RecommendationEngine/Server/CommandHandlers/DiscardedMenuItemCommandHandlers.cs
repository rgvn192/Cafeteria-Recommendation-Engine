using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using RecommendationEngine.Common.Utils;
using Server.Interface;
using Server.Models.DTO;
using Server.Models.Request;
using Server.Models.Response;
using Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RecommendationEngine.Common.Exceptions;
using RecommendationEngine.Data.Entities;

namespace Server.CommandHandlers
{
    public static class DiscardedMenuItemCommandHandlers
    {
        public async static Task<CustomProtocolResponse> GenerateDiscardedMenuItems(IServiceProvider serviceProvider, string body)
        {
            try
            {
                using (var scope = serviceProvider.CreateScope())
                {
                    var discardedMenuItemService = scope.ServiceProvider.GetRequiredService<IDiscardedMenuItemService>();

                    await discardedMenuItemService.GenerateDiscardedMenuItemsForThisMonth();
                }

                var response = new CommonServerResponse
                {
                    Status = "Success",
                    Message = "Successfully Generated Discarded Menu."
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

        public async static Task<CustomProtocolResponse> GetDiscardedMenuItemsForCurrentMonth(IServiceProvider serviceProvider, string body)
        {
            try
            {
                List<DiscardedMenuItemsResponse> discardedMenuItemsResponse = new();

                using (var scope = serviceProvider.CreateScope())
                {
                    var mapper = scope.ServiceProvider.GetRequiredService<IMapper>();
                    var discardedMenuItemService = scope.ServiceProvider.GetRequiredService<IDiscardedMenuItemService>();

                    var discardedMenuItems = await discardedMenuItemService.GetDiscardedMenuItemsForCurrentMonth();

                    discardedMenuItemsResponse = mapper.Map<List<DiscardedMenuItem>, List<DiscardedMenuItemsResponse>>(discardedMenuItems);
                }

                return new CustomProtocolResponse
                {
                    Status = "Success",
                    Body = JsonHelper.SerializeObjectIgnoringCycles(discardedMenuItemsResponse)
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

        public async static Task<CustomProtocolResponse> HandleDiscardedMenuItem(IServiceProvider serviceProvider, string body)
        {
            try
            {
                var request = JsonConvert.DeserializeObject<HandleDiscardedMenuItemRequest>(body) ?? throw new AppException("request body is null");
                using (var scope = serviceProvider.CreateScope())
                {
                    var discardedMenuItemService = scope.ServiceProvider.GetRequiredService<IDiscardedMenuItemService>();

                    await discardedMenuItemService.HandleDiscardedMenuItem(request.DiscardedMenuItemId, request.MakeAvailable);
                }

                var response = new CommonServerResponse
                {
                    Status = "Success",
                    Message = request.MakeAvailable ? "Successfully made the discarded Item available" : "Successfully removed the discarded item."
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

        public async static Task<CustomProtocolResponse> GiveFeedBackOnDiscardedMenuItem(IServiceProvider serviceProvider, string body)
        {
            try
            {
                var request = JsonConvert.DeserializeObject<DiscardedMenuItemFeedbackModel>(body);

                using (var scope = serviceProvider.CreateScope())
                {
                    var feedbackService = scope.ServiceProvider.GetRequiredService<IDiscardedMenuItemFeedbackService>();

                    await feedbackService.Add(request);
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
