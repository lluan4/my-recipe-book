using CommonTestUtilities.Requests;
using CommonTestUtilities.Tokens;
using Shouldly;
using System.Net;

namespace WebApi.test.Recipe.Generate
{
    public class GenerateRecipeInvalidTokenTest : MyRecipeBookClassFixture
    {
        private const string METHOD = "recipe/generate";
        private const string INVALID_TOKEN = "123";

        public GenerateRecipeInvalidTokenTest(CustomWebApplicationFactory factory) : base(factory)
        {
        }

        [Fact]
        public async Task Error_Invalid_Token()
        {
            var request = RequestFilterRecipeJsonBuilder.Build();

            var response = await DoPost(method: METHOD, request: request, token: INVALID_TOKEN);

            response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task Error_Without_Token()
        {
            var request = RequestFilterRecipeJsonBuilder.Build();

            var response = await DoPost(method: METHOD, request: request, token: string.Empty);

            response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task Error_Token_With_User_NotFound()
        {
            var request = RequestFilterRecipeJsonBuilder.Build();

            var token = JwtTokensGeneratorBuilder.Build().Generate(Guid.NewGuid());

            var response = await DoPost(method: METHOD, request: request, token: token);

            response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
        }
    }
}
