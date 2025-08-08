using Moq;
using MyRecipeBook.Domain.Dtos.Recipes;
using MyRecipeBook.Domain.Services.GeminiApi;

namespace CommonTestUtilities.GeminiAI
{
    public class GenerateRecipeAIBuilder
    {
        public static IGenerateRecipeAI Build(GeneratedRecipeDto dto)
        {
            var mock = new Mock<IGenerateRecipeAI>();

            mock.Setup(service => service.Generate(It.IsAny<List<string>>()))
                .ReturnsAsync(dto);

            return mock.Object;
        }
    }
}
