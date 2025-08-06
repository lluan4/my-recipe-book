using CommonTestUtilities.Tokens;
using Shouldly;
using System.Net;
using System.Text.Json;

namespace WebApi.test.Dashboard
{
    public class GetDashboardTest : MyRecipeBookClassFixture
    {
        private const string METHOD = "dashboard";

        private readonly Guid _userIdentifier;

        public GetDashboardTest(CustomWebApplicationFactory factory) : base(factory)
        {
            _userIdentifier = factory.GetUserIdentifier();
        }

        [Fact]
        public async Task Success()
        {
            var token = JwtTokensGeneratorBuilder.Build().Generate(_userIdentifier);

            var response = await DoGet(method: METHOD, token: token);

            response.StatusCode.ShouldBe(HttpStatusCode.OK);

            await using var responseBody = await response.Content.ReadAsStreamAsync();

            var responseData = await JsonDocument.ParseAsync(responseBody);

            responseData.RootElement.GetProperty("recipes").GetArrayLength().ShouldBeGreaterThan(0);
        }
    }
}
