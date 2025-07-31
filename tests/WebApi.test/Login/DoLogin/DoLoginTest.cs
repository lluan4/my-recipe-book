using CommonTestUtilities.Requests;
using MyRecipeBook.Communication.Request;
using MyRecipeBook.Exceptions;
using Shouldly;
using System.Globalization;
using System.Net;
using System.Text.Json;
using WebApi.test.InlineData;

namespace WebApi.test.Login.DoLogin
{
    public class DoLoginTest : MyRecipeBookClassFixture
    {

        private readonly string METHOD = "login";

        private readonly string _email;
        private readonly string _password;
        private readonly string _name;

        public DoLoginTest(CustomWebApplicationFactory factory) : base(factory)
        {
            _email = factory.GetEmail();
            _password = factory.GetPassword();
            _name = factory.GetName();
        }

        [Fact]
        public async Task Success()
        {
            var request = new RequestLoginJson
            {
                Email = _email,
                Password = _password
            };

            var response = await DoPost(METHOD, request);

            response.StatusCode.ShouldBe(HttpStatusCode.OK);

            await using var responseBody = await response.Content.ReadAsStreamAsync();

            var responseData = await JsonDocument.ParseAsync(responseBody);

            var value = responseData.RootElement.GetProperty("name").GetString();
            var tokens = responseData.RootElement.GetProperty("tokens").GetProperty("accessToken").GetString();

            value.ShouldSatisfyAllConditions(
                e => e.ShouldNotBeNullOrWhiteSpace(),
                e => e.ShouldBe(_name)
            );

            tokens.ShouldNotBeNullOrEmpty();
        }

        [Theory]
        [ClassData(typeof(CultureInlineDataTest))]
        public async Task Error_Login_Invalid(string culture)
        {
            var request = RequestLoginJsonBuilder.Build();

            var response = await DoPost(METHOD, request, culture);

            response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);

            await using var responseBody = await response.Content.ReadAsStreamAsync();

            var responseData = await JsonDocument.ParseAsync(responseBody);

            var errors = responseData.RootElement.GetProperty("errors").EnumerateArray();

            var expectedMessage = ResourceMessagesException.ResourceManager.GetString("EMAIL_OR_PASSWORD_INVALID", new CultureInfo(culture));

            errors.ShouldSatisfyAllConditions(
                e => e.ShouldHaveSingleItem(),
                e => e.Single().GetString()!.Equals(expectedMessage)
            );
        }

    }
}
