using CommonTestUtilities.Requests;
using CommonTestUtilities.Tokens;
using Microsoft.VisualStudio.TestPlatform.Utilities;
using MyRecipeBook.Exceptions.ExceptionsBase;
using Shouldly;
using System.Net;
using System.Text.Json;
using WebApi.test.InlineData;

namespace WebApi.test.Recipe.Register
{

    public class RegisterRecipeTest : MyRecipeBookClassFixture
    {
        private const string METHOD = "recipe";
        private readonly Guid _userIdentifier;
        public RegisterRecipeTest(CustomWebApplicationFactory factory) : base(factory)
        {
            _userIdentifier = factory.GetUserIdentifier();
        }

        [Fact]
        public async Task Success() 
        {
           
                var request = RequestRecipeJsonBuilder.Build();
                var token = JwtTokensGeneratorBuilder.Build().Generate(_userIdentifier);

                var response = await DoPost(method: METHOD, request: request, token: token);

                response.StatusCode.ShouldBe(HttpStatusCode.Created);

                await using var responseBody = await response.Content.ReadAsStreamAsync();

                var responseData = await JsonDocument.ParseAsync(responseBody);

                var id = responseData.RootElement.GetProperty("id").GetString();
                var title = responseData.RootElement.GetProperty("title").GetString();

                id.ShouldNotBeNullOrEmpty();
                title.ShouldNotBeNullOrEmpty();

        }

        [Theory]
        [ClassData(typeof(CultureInlineDataTest))]
        public async Task Error_Invalid_Token(string culture)
        {
            var request = RequestRecipeJsonBuilder.Build();
            var token = "teste123";

            var response = await DoPost(method: METHOD, request: request, token: token, culture: culture);

            response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
        }

        [Theory]
        [ClassData(typeof(CultureInlineDataTest))]
        public async Task Error_Without_Token(string culture)
        {
            var request = RequestRecipeJsonBuilder.Build();

            var response = await DoPost(method: METHOD, request: request, culture: culture);

            response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
        }

        [Theory]
        [ClassData(typeof(CultureInlineDataTest))]
        public async Task Error_Title_Empty(string culture)
        {
            var request = RequestRecipeJsonBuilder.Build();
            request.Title = string.Empty;

            var token = JwtTokensGeneratorBuilder.Build().Generate(_userIdentifier);

            var response = await DoPost(method: METHOD, request: request, token: token, culture: culture);

            response.StatusCode.ShouldBe(System.Net.HttpStatusCode.BadRequest);

            await using var responseBody = await response.Content.ReadAsStreamAsync();

            var responseData = await JsonDocument.ParseAsync(responseBody);

            var errors = responseData.RootElement.GetProperty("errors").EnumerateArray();

            var expectedMessage = ResourceMessageHelper.FieldEmpty("Title");

            errors.ShouldSatisfyAllConditions(
                e => e.ShouldHaveSingleItem(),
                e => e.Single().GetString()!.Equals(expectedMessage)
            );

        }
        
    }
}
