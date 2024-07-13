﻿using Microsoft.Extensions.Logging;
using RecommendationEngine.Data.Entities;
using RecommendationEngine.Data.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecommendationEngine.Data.Repositories
{
    public class CharacteristicRepository : CrudBaseRepository<Characteristic>, ICharacteristicRepository
    {
        public CharacteristicRepository(AppDbContext appDbContext, ILogger<CharacteristicRepository> logger) :
            base(appDbContext, logger)
        {

        }

    }
}
