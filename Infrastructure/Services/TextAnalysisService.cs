using Azure.AI.TextAnalytics;
using Infrastructure.Services.Interfaces;


namespace Infrastructure.Services
{
    public class TextAnalysisService : ITextAnalysisService
    {
        private readonly TextAnalyticsClient _client;

        public TextAnalysisService(TextAnalyticsClient client)
        {
            _client = client;
        }

        public async Task<string> AnalyzeSentimentAsync(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                throw new ArgumentException("Text cannot be null or empty", nameof(text));

            var response = await _client.AnalyzeSentimentAsync(text);

            return response.Value.Sentiment.ToString();
        }
    }
}
