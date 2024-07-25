﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Interface
{
    public interface ISentimentAnalyzer
    {
        decimal AnalyzeSentiment(string comment);
    }
}
