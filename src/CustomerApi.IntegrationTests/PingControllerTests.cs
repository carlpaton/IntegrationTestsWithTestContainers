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
            using var response = await HttpClient.GetAsync("ping");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}
