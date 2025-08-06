using CommonTestUtilities.Requests;
using CommonTestUtilities.Tokens;
using Shouldly;
using System.Net;

namespace WebApi.test.Dashboard
{
    public class GetDashboardInvalidTokenTest : MyRecipeBookClassFixture
    {
        private const string METHOD = "dashboard";
        private const string INVALID_TOKEN = "123";

        public GetDashboardInvalidTokenTest(CustomWebApplicationFactory factory) : base(factory)
        {
        }

        [Fact]
        public async Task Error_Invalid_Token()
        {
            var response = await DoGet(method: METHOD, token: INVALID_TOKEN);

            response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task Error_Without_Token()
        {
            var response = await DoGet(method: METHOD,  token: string.Empty);

            response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task Error_Token_With_User_NotFound()
        {
            var token = JwtTokensGeneratorBuilder.Build().Generate(Guid.NewGuid());

            var response = await DoGet(method: METHOD, token: token);

            response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
        }
    }
}
