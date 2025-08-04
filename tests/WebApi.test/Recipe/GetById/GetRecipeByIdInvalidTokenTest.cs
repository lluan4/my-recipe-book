using CommonTestUtilities.IdEncryption;
using CommonTestUtilities.Requests;
using CommonTestUtilities.Tokens;
using Shouldly;
using System.Net;

namespace WebApi.test.Recipe.GetById
{
    public class GetRecipeByIdInvalidTokenTest : MyRecipeBookClassFixture
    {
        private const string METHOD = "recipe";
        private const string INVALID_TOKEN = "123";
        public GetRecipeByIdInvalidTokenTest(CustomWebApplicationFactory factory) : base(factory)
        {
        }

        [Fact]
        public async Task Error_Invalid_Token()
        {
            var id = IdEncripterBuilder.Build().Encode(1);

            var response = await DoGet(method: $"{METHOD}/{id}", token: INVALID_TOKEN);

            response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task Error_Without_Token()
        {
            var id = IdEncripterBuilder.Build().Encode(1);

            var response = await DoGet(method: $"{METHOD}/{id}", token: string.Empty);

            response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
        }
        [Fact]
        public async Task Error_NotFound_User_Token()
        {
            var id = IdEncripterBuilder.Build().Encode(1);

            var token = JwtTokensGeneratorBuilder.Build().Generate(Guid.NewGuid());

            var response = await DoGet(method: $"{METHOD}/{id}", token: token);

            response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
        }
    }
}
