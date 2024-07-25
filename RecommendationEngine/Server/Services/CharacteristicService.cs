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
    public class CharacteristicService : CrudBaseService<Characteristic>, ICharacteristicService
    {
        public CharacteristicService(ICharacteristicRepository characteristicRepository, IMapper mapper, ILogger<CharacteristicService> logger) :
            base(characteristicRepository, mapper, logger)
        {
        }

        protected override List<string> ModifiableProperties => new()
        {
            nameof(Characteristic.Name)
        };
    }
}
