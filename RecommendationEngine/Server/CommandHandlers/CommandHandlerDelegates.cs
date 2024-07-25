using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using RecommendationEngine.Data;
using Server.Interface;
using Server.Models.Request;
using Server.Models.Response;
using Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.Models.DTO;
using AutoMapper;
using RecommendationEngine.Data.Entities;
using System.Linq.Expressions;
using RecommendationEngine.Common.Exceptions;
using RecommendationEngine.Common.Utils;

namespace Server.CommandHandlers
{
    public static class CommandHandlerDelegates
    {
        public delegate Task<CustomProtocolResponse> CommandHandler(string body);
        public delegate bool AuthorizationCheck(string role);
    }
}
