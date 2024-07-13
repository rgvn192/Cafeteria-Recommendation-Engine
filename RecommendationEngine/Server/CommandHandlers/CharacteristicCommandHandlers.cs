using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using RecommendationEngine.Common.Utils;
using Server.Interface;
using Server.Models.DTO;
using Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.CommandHandlers
{
    public static class CharacteristicCommandHandlers
    {
        public async static Task<CustomProtocolResponse> GetCharacteristics(IServiceProvider serviceProvider, string body)
        {
            try
            {
                List<CharacteristicModel> characteristics = new();

                using (var scope = serviceProvider.CreateScope())
                {
                    var mapper = scope.ServiceProvider.GetRequiredService<IMapper>();
                    var characteristicService = scope.ServiceProvider.GetRequiredService<ICharacteristicService>();

                    characteristics = await characteristicService.GetList<CharacteristicModel>();
                }

                return new CustomProtocolResponse
                {
                    Status = "Success",
                    Body = JsonHelper.SerializeObjectIgnoringCycles(characteristics)
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
