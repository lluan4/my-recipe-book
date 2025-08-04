using CommonTestUtilities.IdEncryption;
using CommonTestUtilities.Requests;
using CommonTestUtilities.Tokens;
using MyRecipeBook.Exceptions.ExceptionsBase;
using Shouldly;
using System.Net;
using System.Text.Json;
using WebApi.test.InlineData;

namespace WebApi.test.Recipe.GetById
{
    public class GetRecipeByIdTest : MyRecipeBookClassFixture
    {
        private const string METHOD = "recipe";
        
        private readonly Guid _userIdentifer;
        private readonly string _recipeId;
        private readonly string _recipeTitle;

        private readonly string _token;

        public GetRecipeByIdTest(CustomWebApplicationFactory factory) : base(factory)
        {
            _userIdentifer = factory.GetUserIdentifier();
            _recipeId = factory.GetRecipeId();
            _recipeTitle = factory.GetRecipeTitle();
            _token = JwtTokensGeneratorBuilder.Build().Generate(_userIdentifer);
        }

        [Fact]
        public async Task Success()
        {
            var response = await DoGet(method: $"{METHOD}/{_recipeId}", token: _token);

            response.StatusCode.ShouldBe(HttpStatusCode.OK);

            await using var responseBody = await response.Content.ReadAsStreamAsync();

            var responseData = await JsonDocument.ParseAsync(responseBody);

            responseData.RootElement.GetProperty("id").GetString().ShouldBe(_recipeId);
            responseData.RootElement.GetProperty("title").GetString().ShouldBe(_recipeTitle);
        }

        [Theory]
        [ClassData(typeof(CultureInlineDataTest))]
        public async Task Error_Recipe_Not_Found(string culture)
        {
            var id = IdEncripterBuilder.Build().Encode(1000);

            var response = await DoGet(method: $"{METHOD}/{id}", token: _token, culture: culture);

            response.StatusCode.ShouldBe(HttpStatusCode.NotFound);

            await using var responseBody = await response.Content.ReadAsStreamAsync();

            var responseData = await JsonDocument.ParseAsync(responseBody);

            var errors = responseData.RootElement.GetProperty("errors").EnumerateArray();

            var expectedMessage = ResourceMessageHelper.FieldNotFound(fieldName: "Recipe");

            errors.ShouldSatisfyAllConditions(
               e => e.ShouldHaveSingleItem(),
               e => e.Single().GetString()!.Equals(expectedMessage)
           );
        }

    }
}
