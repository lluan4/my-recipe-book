using CommonTestUtilities.Requests;
using CommonTestUtilities.Tokens;
using MyRecipeBook.Communication.Request;
using MyRecipeBook.Domain.Enums;
using MyRecipeBook.Exceptions.ExceptionsBase;
using Shouldly;
using System.Net;
using System.Text.Json;
using WebApi.test.InlineData;


namespace WebApi.test.Recipe.Filter
{
    public class FilterRecipeTest : MyRecipeBookClassFixture
    {
        private const string METHOD = "recipe/filter";
        private const string INVALID_TOKEN = "123";

        private readonly Guid _userIdentifier;

        private string _recipeTitle;
        private RecipeDifficulty? _recipeDifficulty;
        private RecipeCookingTime? _recipeCookingTime;
        private IList<RecipeDishType>? _recipeDishTypes;

        public FilterRecipeTest(CustomWebApplicationFactory factory) : base(factory)
        {
            _userIdentifier = factory.GetUserIdentifier();

            _recipeTitle = factory.GetRecipeTitle();
            _recipeCookingTime = factory.GetRecipeCookingTime();
            _recipeDifficulty = factory.GetDifficulty();
            _recipeDishTypes = factory.GetRecipeDishType();
        }

        [Fact]
        public async Task Success()
        {
            IList<MyRecipeBook.Communication.Enums.RecipeCookingTime> cookingTimes = [(MyRecipeBook.Communication.Enums.RecipeCookingTime)_recipeCookingTime!];
            IList<MyRecipeBook.Communication.Enums.RecipeDifficulty> difficulties = [(MyRecipeBook.Communication.Enums.RecipeDifficulty)_recipeDifficulty!];
            IList<MyRecipeBook.Communication.Enums.RecipeDishType> dishTypes = _recipeDishTypes!.Select(dishType => (MyRecipeBook.Communication.Enums.RecipeDishType)dishType).ToList();

            var request = new RequestFilterRecipeJson
            {
                CookingTimes = cookingTimes,
                Difficulties = difficulties,
                DishTypes = dishTypes,
                RecipeTitle_Ingredient = _recipeTitle,
            };

            var token = JwtTokensGeneratorBuilder.Build().Generate(_userIdentifier);

            var response = await DoPost(method: METHOD, request: request, token: token);

            response.StatusCode.ShouldBe(HttpStatusCode.OK);

            await using var responseBody = await response.Content.ReadAsStreamAsync();

            var responseData = await JsonDocument.ParseAsync(responseBody);

            responseData.RootElement.GetProperty("recipes").EnumerateArray().ShouldNotBeEmpty();



        }

        [Fact]
        public async Task Success_NoContent()
        {
            var recipeTitleOrIngredientNotExist = "recipeDontExists";

            var request = RequestFilterRecipeJsonBuilder.Build();
            request.RecipeTitle_Ingredient = recipeTitleOrIngredientNotExist;

            var token = JwtTokensGeneratorBuilder.Build().Generate(_userIdentifier);

            var response = await DoPost(method: METHOD, request: request, token: token);

            response.StatusCode.ShouldBe(HttpStatusCode.NoContent);

        }

        [Theory]
        [ClassData(typeof(CultureInlineDataTest))]
        public async Task Error_CookingTime_Invalid(string culture)
        {
            var request = RequestFilterRecipeJsonBuilder.Build();
            request.CookingTimes.Add((MyRecipeBook.Communication.Enums.RecipeCookingTime)100);

            var token = JwtTokensGeneratorBuilder.Build().Generate(_userIdentifier);

            var response = await DoPost(method: METHOD, request: request, token: token, culture: culture);

            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

            await using var responseBody = await response.Content.ReadAsStreamAsync();

            var responseData = await JsonDocument.ParseAsync(responseBody);

            var errors = responseData.RootElement.GetProperty("errors").EnumerateArray();

            var expectedMessage = ResourceMessageHelper.FieldNotSupported(fieldName: "CookingTime");

            errors.ShouldSatisfyAllConditions(
               e => e.ShouldHaveSingleItem(),
               e => e.Single().GetString()!.Equals(expectedMessage)
           );
        }

    }
}
