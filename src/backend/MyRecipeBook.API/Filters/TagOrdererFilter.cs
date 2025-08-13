using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace MyRecipeBook.API.Filters
{
	public sealed class TagOrdererFilter:IDocumentFilter
	{
		public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
		{

			swaggerDoc.Tags = new List<OpenApiTag>
				{
					 new() { Name = "Authentication"},
					 new() { Name = "Users"},
					 new() { Name = "Recipes"},
					 new() { Name = "Dashboard"}
				};
		}
	}
}