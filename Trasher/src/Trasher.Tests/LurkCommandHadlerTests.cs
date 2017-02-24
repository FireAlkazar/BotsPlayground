using Xunit;
using Trasher.CommandHandlers;

namespace Trasher.Tests
{
    public class LurkCommandHadlerTests
    {
         [Fact]
         public void GetInfo_NotNull()
         {
             var handler = new LurkCommandHandler();

             string info = handler.GetInfo(string.Empty);

            Assert.NotNull(info);
         }
    }
}