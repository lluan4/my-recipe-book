using CommonTestUtilities.Requests;
using MyRecipeBook.Exceptions;
using Shouldly;
using System.Globalization;
using System.Net;
using System.Text.Json;
using WebApi.test.InlineData;

namespace WebApi.test.User.Register
{
    public class RegisterUserTest : MyRecipeBookClassFixture
    {


        private readonly string METHOD = "user";
        public RegisterUserTest(CustomWebApplicationFactory factory) : base(factory) { }

        [Fact]
        public async Task Success()
        {
            var request = RequestRegisterUserJsonBuilder.Build();

            var response = await DoPost(METHOD, request);

            response.StatusCode.ShouldBe(HttpStatusCode.Created);

            await using var responseBody = await response.Content.ReadAsStreamAsync();

            var responseData = await JsonDocument.ParseAsync(responseBody);

            var value = responseData.RootElement.GetProperty("name").GetString();
            var tokens = responseData.RootElement.GetProperty("tokens").GetProperty("accessToken").GetString();

            value.ShouldSatisfyAllConditions(
                e => e.ShouldNotBeNullOrWhiteSpace(),
                e => e.ShouldBe(request.Name)
            );

            tokens.ShouldNotBeNullOrEmpty();
        }


        [Theory]
        [ClassData(typeof(CultureInlineDataTest))]
        public async Task Error_Empty_Name(string culture)
        {
            var request = RequestRegisterUserJsonBuilder.Build();
            request.Name = string.Empty;


            var response = await DoPost(METHOD, request, culture);

            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

            await using var responseBody = await response.Content.ReadAsStreamAsync();

            var responseData = await JsonDocument.ParseAsync(responseBody);

            var errors = responseData.RootElement.GetProperty("errors").EnumerateArray();

            var expectedMessage = ResourceMessagesException.ResourceManager.GetString("NAME_EMPTY", new CultureInfo(culture));

            errors.ShouldSatisfyAllConditions(
                e => e.ShouldHaveSingleItem(),
                e => e.Single().GetString()!.Equals(expectedMessage)
            );

        }
    }
}
