using Trasher.CommandHandlers;
using Xunit;

namespace Trasher.Tests
{
    public class KaktamCommandHandlerTests
    {
        [Fact]
        public void GetInfo_NoArgs_NotEmptyResult()
        {
            var handler = new KaktamCommandHandler();

            string info = handler.GetInfo(string.Empty);

            Assert.NotEmpty(info);
        }
    }
}