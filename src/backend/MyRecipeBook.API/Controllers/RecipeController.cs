using Microsoft.AspNetCore.Mvc;
using MyRecipeBook.API.Attributes;
using MyRecipeBook.API.Binders;
using MyRecipeBook.Application.UseCases.Recipe.Delete;
using MyRecipeBook.Application.UseCases.Recipe.Filter;
using MyRecipeBook.Application.UseCases.Recipe.GetById;
using MyRecipeBook.Application.UseCases.Recipe.Register;
using MyRecipeBook.Application.UseCases.Recipe.Update;
using MyRecipeBook.Communication.Request;
using MyRecipeBook.Communication.Response;
using Swashbuckle.AspNetCore.Annotations;

namespace MyRecipeBook.API.Controllers
{
    [AuthenticatedUser]
    [Tags("Recipes")]
    public class RecipeController : MyRecipeBookBaseController
    {
        /// <summary>
        /// Registra uma nova receita
        /// </summary>
        /// <param name="useCase">Use case para registro de receita</param>
        /// <param name="request">Dados da receita a ser criada</param>
        /// <returns>Dados da receita criada</returns>
        /// <response code="201">Receita criada com sucesso</response>
        /// <response code="400">Dados inválidos fornecidos</response>
        /// <response code="401">Token de autenticação inválido ou ausente</response>
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
            [FromBody] RequestRecipeJson request
        )
        {
            var response = await useCase.Execute(request);
            return Created(string.Empty, response);
        }

        /// <summary>
        /// Filtra receitas baseado nos critérios fornecidos
        /// </summary>
        /// <param name="useCase">Use case para filtrar receitas</param>
        /// <param name="request">Critérios de filtro</param>
        /// <returns>Lista de receitas que atendem aos critérios</returns>
        /// <response code="200">Receitas encontradas</response>
        /// <response code="204">Nenhuma receita encontrada</response>
        /// <response code="401">Token de autenticação inválido ou ausente</response>
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

            if (response.Recipes.Any())
                return Ok(response);

            return NoContent();
        }

        /// <summary>
        /// Obtém uma receita específica pelo ID
        /// </summary>
        /// <param name="useCase">Use case para obter receita</param>
        /// <param name="id">ID criptografado da receita</param>
        /// <returns>Dados completos da receita</returns>
        /// <response code="200">Receita encontrada</response>
        /// <response code="404">Receita não encontrada</response>
        /// <response code="401">Token de autenticação inválido ou ausente</response>
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

        /// <summary>
        /// Atualiza uma receita existente
        /// </summary>
        /// <param name="useCase">Use case para atualizar receita</param>
        /// <param name="id">ID criptografado da receita</param>
        /// <param name="request">Dados atualizados da receita</param>
        /// <returns>Confirmação da atualização</returns>
        /// <response code="204">Receita atualizada com sucesso</response>
        /// <response code="400">Dados inválidos fornecidos</response>
        /// <response code="404">Receita não encontrada</response>
        /// <response code="401">Token de autenticação inválido ou ausente</response>
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

        /// <summary>
        /// Exclui uma receita
        /// </summary>
        /// <param name="useCase">Use case para excluir receita</param>
        /// <param name="id">ID criptografado da receita</param>
        /// <returns>Confirmação da exclusão</returns>
        /// <response code="204">Receita excluída com sucesso</response>
        /// <response code="404">Receita não encontrada</response>
        /// <response code="401">Token de autenticação inválido ou ausente</response>
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