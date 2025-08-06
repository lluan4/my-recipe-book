using Microsoft.AspNetCore.Mvc;
using MyRecipeBook.API.Attributes;
using MyRecipeBook.Application.UseCases.Dashboard.Get;
using MyRecipeBook.Communication.Response;
using Swashbuckle.AspNetCore.Annotations;

namespace MyRecipeBook.API.Controllers
{
    [AuthenticatedUser]
    public class DashboardController : MyRecipeBookBaseController
    {
        /// <summary>
        /// Obtém os dados do dashboard do usuário
        /// </summary>
        /// <param name="useCase">Use case para obter dados do dashboard</param>
        /// <returns>Lista das receitas mais recentes do usuário</returns>
        /// <response code="200">Dados do dashboard obtidos com sucesso</response>
        /// <response code="204">Usuário não possui receitas</response>
        /// <response code="401">Token de autenticação inválido ou ausente</response>
        [HttpGet]
        [SwaggerOperation(
            Summary = "Obtêm receitas",
            Description = "Retorna as receitas mais recentes do usuário para exibição no dashboard",
            OperationId = "GetDashboard"
        )]
        [ProducesResponseType(typeof(ResponseRecipesJson), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Get(
            [FromServices] IGetDashboardUseCase useCase
        )
        {
            var response = await useCase.Execute();

            if (response.Recipes.Any())
                return Ok(response);

            return NoContent();
        }
    }
}