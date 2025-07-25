using CommonTestUtilities.Requests;
using CommonTestUtilities.Tokens;
using MyRecipeBook.Exceptions;
using Shouldly;
using System.Globalization;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using WebApi.test.InlineData;

namespace WebApi.test.User.Update
{
    public class UpdateUserUpdateUserInvalidTokenTest : MyRecipeBookClassFixture
    {

        private const string METHOD = "user";
        private const string INVALID_TOKEN = "tokenInvalid";
        public UpdateUserUpdateUserInvalidTokenTest(CustomWebApplicationFactory factory) : base(factory) 
        { 
        }

        [Fact]
        public async Task Error_Token_Invalid()
        {
            var request = RequestUpdateUserJsonBuilder.Build();

            var response = await DoPut(METHOD, request, token: INVALID_TOKEN);

            response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task Error_Without_Token()
        {
            var request = RequestUpdateUserJsonBuilder.Build();

            var response = await DoPut(METHOD, request, token: string.Empty);

            response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task Error_Token_With_User_NotFound()
        {
            var request = RequestUpdateUserJsonBuilder.Build();

            var token = JwtTokensGeneratorBuilder.Build().Generate(Guid.NewGuid());

            var response = await DoPut(METHOD, request, token);

            response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
        }
    }
}
