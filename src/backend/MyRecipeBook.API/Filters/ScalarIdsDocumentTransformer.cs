using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi.Models;
using MyRecipeBook.API.Binders;
using Microsoft.AspNetCore.Mvc.ApiExplorer;

namespace MyRecipeBook.API.Filters
{
    internal sealed class ScalarIdsDocumentTransformer : IOpenApiDocumentTransformer
    {
        public Task TransformAsync(
            OpenApiDocument document,
            OpenApiDocumentTransformerContext context,
            CancellationToken cancellationToken)
        {
           
            foreach (var path in document.Paths)
            {
                foreach (var operation in path.Value.Operations)
                {
                    if (operation.Value.Parameters != null)
                    {
                        foreach (var parameter in operation.Value.Parameters)
                        {
                            
                            if (parameter.Name == "id" && parameter.Schema?.Type == "integer")
                            {
                                parameter.Schema.Type = "string";
                                parameter.Schema.Format = null;
                                parameter.Description = "Encrypted ID (string)";
                            }
                        }
                    }
                }
            }

            return Task.CompletedTask;
        }
    }
}