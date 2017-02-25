using System;
using Trasher.CommandHandlers;
using Xunit;

namespace Trasher.Tests
{
    public class RandomImageCommandHandlerTests
    {
        [Fact]
        public void GetInfo_DiagramUserQuery_NotEmpty()
        {
            var handler = new RandomImageCommandHandler();

            string url = handler.GetInfo("@Trasher img Diagram");

            Assert.NotEmpty(url);
            Assert.True(Uri.IsWellFormedUriString(url, UriKind.Absolute));
        }

        [Fact]
        public void GetInfo_EmptyUserQuery_NotEmpty()
        {
            var handler = new RandomImageCommandHandler();

            string info = handler.GetInfo("@Trasher img");

            Assert.NotEmpty(info);
        }
    }
}