using Server.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest.Services
{

    [TestClass]
    public class SentimentAnalyzerTests
    {
        private SentimentAnalyzer _sentimentAnalyzer;

        [TestInitialize]
        public void Setup()
        {
            _sentimentAnalyzer = new SentimentAnalyzer();
        }

        [TestMethod]
        public void AnalyzeSentiment_ShouldReturnHighPositiveScore_WhenCommentIsPositive()
        {
            string comment = "This is a delicious and amazing dish!";

            decimal score = _sentimentAnalyzer.AnalyzeSentiment(comment);

            Assert.IsTrue(score > 3.5m);
        }

        [TestMethod]
        public void AnalyzeSentiment_ShouldReturnLowNegativeScore_WhenCommentIsNegative()
        {
            string comment = "This is a terrible and disgusting dish!";

            decimal score = _sentimentAnalyzer.AnalyzeSentiment(comment);

            Assert.IsTrue(score < 1.5m);
        }

        [TestMethod]
        public void AnalyzeSentiment_ShouldReturnNeutralScore_WhenCommentIsNeutral()
        {            
            string comment = "The dish was okay.";

            decimal score = _sentimentAnalyzer.AnalyzeSentiment(comment);

            Assert.IsTrue(score >= 2.0m && score <= 3.0m);
        }

        [TestMethod]
        public void AnalyzeSentiment_ShouldHandleNegationCorrectly()
        {
            string comment = "This dish is not delicious and not terrible.";

            decimal score = _sentimentAnalyzer.AnalyzeSentiment(comment);

            Assert.IsTrue(score >= 2.0m && score <= 3.0m);
        }

        [TestMethod]
        public void AnalyzeSentiment_ShouldReturnCorrectScore_WhenCommentIsMixed()
        {
            string comment = "The food was tasty but the service was awful.";

            decimal score = _sentimentAnalyzer.AnalyzeSentiment(comment);

            Assert.IsTrue(score >= 2.0m && score <= 3.0m);
        }
    }
}


