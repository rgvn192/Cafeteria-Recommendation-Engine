﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Models.Request
{
    public class GetRecommendationRequestModel
    {
        public int Limit { get; set; }
        public int MenuItemCategoryId { get; set; }

    }
}
