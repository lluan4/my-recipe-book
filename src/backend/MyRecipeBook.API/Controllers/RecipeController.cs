using Microsoft.AspNetCore.Mvc;
using MyRecipeBook.API.Attributes;
using MyRecipeBook.API.Binders;
using MyRecipeBook.Application.UseCases.Recipe.Delete;
using MyRecipeBook.Application.UseCases.Recipe.Filter;
using MyRecipeBook.Application.UseCases.Recipe.Generate;
using MyRecipeBook.Application.UseCases.Recipe.GetById;
using MyRecipeBook.Application.UseCases.Recipe.Image;
using MyRecipeBook.Application.UseCases.Recipe.Register;
using MyRecipeBook.Application.UseCases.Recipe.Update;
using MyRecipeBook.Communication.Request;
using MyRecipeBook.Communication.Response;
using Swashbuckle.AspNetCore.Annotations;

namespace MyRecipeBook.API.Controllers
{
	[AuthenticatedUser]
	[Tags("Recipes")]
	public class RecipeController:MyRecipeBookBaseController
	{
		[HttpGet("{id}")]
		[SwaggerOperation(
		  Summary = "Obter receita por ID",
		  Description = "Retorna os detalhes completos de uma receita específica",
		  OperationId = "GetRecipeById"
		)]
		[ProducesResponseType(typeof(ResponseRecipeJson), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
		[ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status401Unauthorized)]
		public async Task<IActionResult> GetById(
		  [FromServices] IGetRecipeByIdUseCase useCase,
		  [FromRoute][ModelBinder(typeof(MyRecipeBookIdBinder))] long id
		)
		{
			var response = await useCase.Execute(id);
			return Ok(response);
		}

		[HttpPost]
		[SwaggerOperation(
		  Summary = "Criar receitas",
		  Description = "Cria uma nova receita para o usuário autenticado",
		  OperationId = "CreateRecipe"
		)]
		[ProducesResponseType(typeof(ResponseRegisteredRecipeJson), StatusCodes.Status201Created)]
		[ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
		[ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status401Unauthorized)]
		public async Task<IActionResult> Register(
		  [FromServices] IRegisterRecipeUseCase useCase,
		  [FromForm] RequestRegisterRecipeFormData request
		)
		{
			var response = await useCase.Execute(request);
			return Created(string.Empty, response);
		}


		[HttpPost("filter")]
		[SwaggerOperation(
		  Summary = "Filtrar receitas",
		  Description = "Filtra receitas do usuário baseado em critérios como dificuldade, tempo de preparo, etc.",
		  OperationId = "FilterRecipes"
		)]
		[ProducesResponseType(typeof(ResponseRecipesJson), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status401Unauthorized)]
		public async Task<IActionResult> Filter(
		  [FromServices] IFilterRecipeUseCase useCase,
		  [FromBody] RequestFilterRecipeJson request
		)
		{
			var response = await useCase.Execute(request);

			if(response.Recipes.Any())
				return Ok(response);

			return NoContent();
		}


		[HttpPost("generate")]
		[SwaggerOperation(
		  Summary = "Gerar receitas",
		  Description = "Gerar receitas para o  usuário baseado em critérios como dificuldade, tempo de preparo, etc.",
		  OperationId = "GenerateRecipes"
		)]
		[ProducesResponseType(typeof(ResponseGeneratedRecipeJson), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status401Unauthorized)]
		[ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> Generate(
		  [FromServices] IGeneratedRecipeUseCase useCase,
		  [FromBody] RequestGenerateRecipeJson request
		)
		{
			var response = await useCase.Execute(request);

			return Ok(response);
		}

		[HttpPut("{id}")]
		[SwaggerOperation(
		  Summary = "Atualizar receita",
		  Description = "Atualiza todos os dados de uma receita existente",
		  OperationId = "UpdateRecipe"
		)]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
		[ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
		[ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status401Unauthorized)]
		public async Task<IActionResult> Update(
		  [FromServices] IUpdateRecipeUseCase useCase,
		  [FromRoute][ModelBinder(typeof(MyRecipeBookIdBinder))] long id,
		  [FromBody] RequestRecipeJson request
		)
		{
			await useCase.Execute(id, request);
			return NoContent();
		}

		[HttpPut]
		[Route("image/{id}")]
		[SwaggerOperation(
		  Summary = "Editar imagem receita",
		  Description = "Edita imagem de uma receita já criada",
		  OperationId = "EditImageRecipe"
		)]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
		[ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status401Unauthorized)]
		[ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> UpdateImage(
		  [FromServices] IAddUpdateImageCoverUseCase useCase,
		  [FromRoute][ModelBinder(typeof(MyRecipeBookIdBinder))] long id,
		  IFormFile file
		)
		{
			await useCase.Execute(id, file);

			return NoContent();
		}


		[HttpDelete("{id}")]
		[SwaggerOperation(
		  Summary = "Excluir receita",
		  Description = "Remove permanentemente uma receita do sistema",
		  OperationId = "DeleteRecipe"
		)]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
		[ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status401Unauthorized)]
		public async Task<IActionResult> Delete(
		  [FromServices] IDeleteRecipeUseCase useCase,
		  [FromRoute][ModelBinder(typeof(MyRecipeBookIdBinder))] long id
		)
		{
			await useCase.Execute(id);
			return NoContent();
		}


	}
}