﻿using RecommendationEngine.Data.Entities;
using Server.Models.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Interface
{
    internal interface IAuthorisationService
    {
        Task<User> AuthenticateUser(LoginRequestModel loginRequest);
    }
}
