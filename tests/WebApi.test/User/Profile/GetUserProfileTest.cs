using CommonTestUtilities.Tokens;
using Shouldly;
using System.Text.Json;

namespace WebApi.test.User.Profile
{
    public class GetUserProfileTest : MyRecipeBookClassFixture
    {
        private readonly string METHOD = "user";

        private readonly string _name;
        private readonly string _email;
        private readonly Guid _userIdentifier;
        public GetUserProfileTest(CustomWebApplicationFactory factory) : base(factory)
        {
            _name = factory.GetName();
            _email = factory.GetEmail();
            _userIdentifier = factory.GetUserIdentifier();
        }

        [Fact]
        public async Task Success()
        {
            var token = JwtTokensGeneratorBuilder.Build().Generate(_userIdentifier);

            var response = await DoGet(METHOD, token: token);

            response.StatusCode.ShouldBe(System.Net.HttpStatusCode.OK);

            await using var responseBody = await response.Content.ReadAsStreamAsync();

            var responseData = await JsonDocument.ParseAsync(responseBody);

            responseData.RootElement.GetProperty("name").GetString().ShouldNotBeNullOrWhiteSpace();
            responseData.RootElement.GetProperty("name").GetString().ShouldBe(_name);
            responseData.RootElement.GetProperty("email").GetString().ShouldNotBeNullOrWhiteSpace();
            responseData.RootElement.GetProperty("email").GetString().ShouldBe(_email);
        }
    }
}
