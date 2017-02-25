using Trasher.CommandHandlers.Services;
using Xunit;

namespace Trasher.Tests
{
    public class RandomWordsServiceTests
    {
        [Fact]
        public void GetRandomWords_NoArgs_NotEmpty()
        {
            var randomWordsService = new RandomWordsService();

            string randomWord = randomWordsService.GetRandomWords();

            Assert.NotEmpty(randomWord);
        }
    }
}