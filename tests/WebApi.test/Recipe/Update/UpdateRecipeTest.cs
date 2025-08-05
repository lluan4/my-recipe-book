using CommonTestUtilities.IdEncryption;
using CommonTestUtilities.Requests;
using CommonTestUtilities.Tokens;
using MyRecipeBook.Exceptions.ExceptionsBase;
using Shouldly;
using System.Net;
using System.Text.Json;
using WebApi.test.InlineData;

namespace WebApi.test.Recipe.Update
{
    public class UpdateRecipeTest : MyRecipeBookClassFixture
    {
        private const string METHOD = "recipe";

        private readonly Guid _userIdentifer;
        private readonly string _recipeId;
        private readonly string _recipeTitle;

        private readonly string _token;

        public UpdateRecipeTest(CustomWebApplicationFactory factory) : base(factory)
        {
            _userIdentifer = factory.GetUserIdentifier();
            _recipeId = factory.GetRecipeId();
            _recipeTitle = factory.GetRecipeTitle();
            _token = JwtTokensGeneratorBuilder.Build().Generate(_userIdentifer);
        }

        [Fact]
        public async Task Success()
        {
            var request = RequestRecipeJsonBuilder.Build();

            var response = await DoPut(method: $"{METHOD}/{_recipeId}", token: _token, request: request);

            response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
        }

        [Theory]
        [ClassData(typeof(CultureInlineDataTest))]
        public async Task Error_Recipe_Title_Empty(string culture)
        {
            var request = RequestRecipeJsonBuilder.Build();
            request.Title = string.Empty;

            var response = await DoPut(method: $"{METHOD}/{_recipeId}", token: _token, culture: culture, request: request);

            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

            await using var responseBody = await response.Content.ReadAsStreamAsync();

            var responseData = await JsonDocument.ParseAsync(responseBody);

            var errors = responseData.RootElement.GetProperty("errors").EnumerateArray();

            var expectedMessage = ResourceMessageHelper.FieldEmpty(fieldName: "Title");

            errors.ShouldSatisfyAllConditions(
               e => e.ShouldHaveSingleItem(),
               e => e.Single().GetString()!.Equals(expectedMessage)
           );
        }
    }
}
