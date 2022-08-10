using CustomerApi.IntegrationTests.Setup;
using FluentAssertions;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace CustomerApi.IntegrationTests
{
    public class PingControllerTests : TestBase
    {
        [Fact]
        public async Task Ping_ReturnsOk()
        {
            // Act
            var pingResponse = await HttpClient.GetAsync("ping");

            // Assert
            pingResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}
