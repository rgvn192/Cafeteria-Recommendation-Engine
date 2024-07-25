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
    public class CharacteristicCommandHandlers
    {
        private readonly ICharacteristicService _characteristicService;

        public CharacteristicCommandHandlers(ICharacteristicService characteristicService)
        {
            _characteristicService = characteristicService;
        }

        public async Task<CustomProtocolResponse> GetCharacteristics(string body)
        {
            try
            {
                List<CharacteristicModel> characteristics = new();
                characteristics = await _characteristicService.GetList<CharacteristicModel>();
                
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
