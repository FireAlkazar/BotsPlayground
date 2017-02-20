using System;
using Trasher.CommandHandlers;
using Xunit;

namespace Trasher.Tests
{
    public class RandomImageCommandHandlerTests
    {
        [Fact]
        public void GetInfo_DiagramString_NotEmpty()
        {
            var handler = new RandomImageCommandHandler();

            string url = handler.GetInfo("@Trasher img Губернатор");

            Assert.NotEmpty(url);
            Assert.True(Uri.IsWellFormedUriString(url, UriKind.Absolute));
        }
    }
}