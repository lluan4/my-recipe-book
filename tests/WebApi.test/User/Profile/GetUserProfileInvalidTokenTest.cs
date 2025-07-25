using CommonTestUtilities.Tokens;
using Shouldly;
using System.Net;

namespace WebApi.test.User.Profile
{
    public class GetUserProfileInvalidTokenTest : MyRecipeBookClassFixture
    {
        private readonly string METHOD = "user";

        public GetUserProfileInvalidTokenTest(CustomWebApplicationFactory factory) : base(factory) { }

        [Fact]
        public async Task Error_Token_Invalid()
        {
            var response = await DoGet(METHOD, token: "tokenInvalid");

            response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task Error_Without_Invalid()
        {
            var response = await DoGet(METHOD, token: string.Empty);

            response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task Error_Token_With_NotFound()
        {
            var token = JwtTokensGeneratorBuilder.Build().Generate(Guid.NewGuid());

            var response = await DoGet(METHOD, token);

            response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
        }
        
    }
}
