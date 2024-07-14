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
    public class DiscardedMenuItemCommandHandlers
    {
        private readonly IDiscardedMenuItemService _discardedMenuItemService;
        private readonly IDiscardedMenuItemFeedbackService _discardedMenuItemFeedbackService;
        private readonly IMapper _mapper;

        public DiscardedMenuItemCommandHandlers(IDiscardedMenuItemService discardedMenuItemService, IDiscardedMenuItemFeedbackService discardedMenuItemFeedbackService, IMapper mapper)
        {
            _discardedMenuItemFeedbackService = discardedMenuItemFeedbackService;
            _discardedMenuItemService = discardedMenuItemService;
            _mapper = mapper;

        }

        public async Task<CustomProtocolResponse> GenerateDiscardedMenuItems(string body)
        {
            try
            {
                await _discardedMenuItemService.GenerateDiscardedMenuItemsForThisMonth();

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

        public async Task<CustomProtocolResponse> GetDiscardedMenuItemsForCurrentMonth(string body)
        {
            try
            {
                List<DiscardedMenuItemsResponse> discardedMenuItemsResponse = new();

                var discardedMenuItems = await _discardedMenuItemService.GetDiscardedMenuItemsForCurrentMonth();

                discardedMenuItemsResponse = _mapper.Map<List<DiscardedMenuItem>, List<DiscardedMenuItemsResponse>>(discardedMenuItems);

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

        public async Task<CustomProtocolResponse> HandleDiscardedMenuItem(string body)
        {
            try
            {
                var request = JsonConvert.DeserializeObject<HandleDiscardedMenuItemRequest>(body) ?? throw new AppException("request body is null");

                await _discardedMenuItemService.HandleDiscardedMenuItem(request.DiscardedMenuItemId, request.MakeAvailable);

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

        public async Task<CustomProtocolResponse> GiveFeedBackOnDiscardedMenuItem(string body)
        {
            try
            {
                var request = JsonConvert.DeserializeObject<DiscardedMenuItemFeedbackModel>(body);

                await _discardedMenuItemFeedbackService.Add(request);
                
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
