using AutoMapper;
using MyRecipeBook.Communication.Response;
using MyRecipeBook.Domain.Entities;
using MyRecipeBook.Domain.Extension;
using MyRecipeBook.Domain.Services.Storage;

namespace MyRecipeBook.Application.Extension
{
	public static class RecipeExtension
	{
		public static async Task<IList<ResponseShortRecipeJson>> MapToShortRecipeJson(
			this IList<Recipe> recipes,
			User user,
			IBlobStorageService blobStorageService,
			IMapper mapper)
		{
			var result = recipes.Select(async recipe =>
			{
				var response = mapper.Map<ResponseShortRecipeJson>(recipe);

				if(recipe.ImageIdentifier.NotEmpty()) response.ImageUrl = await blobStorageService.GetFileUrl(user, recipe.ImageIdentifier);

				return response;
			});

			var response = await Task.WhenAll(result);

			return response;
		}
	}
}
