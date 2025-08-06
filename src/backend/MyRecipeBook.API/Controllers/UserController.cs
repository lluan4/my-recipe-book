using Microsoft.AspNetCore.Mvc;
using MyRecipeBook.API.Attributes;
using MyRecipeBook.Application.UseCases.User.ChangePassword;
using MyRecipeBook.Application.UseCases.User.Profile;
using MyRecipeBook.Application.UseCases.User.Register;
using MyRecipeBook.Application.UseCases.User.Update;
using MyRecipeBook.Communication.Request;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Communication.Response;
using MyRecipeBook.Communication.Responses;
using Swashbuckle.AspNetCore.Annotations;

namespace MyRecipeBook.API.Controllers
{
    [Tags("Users")]
    public class UserController : MyRecipeBookBaseController
    {
        /// <summary>
        /// Registra um novo usuário no sistema
        /// </summary>
        /// <param name="useCase">Use case para registro de usuário</param>
        /// <param name="request">Dados do usuário a ser registrado</param>
        /// <returns>Dados do usuário criado e token de acesso</returns>
        /// <response code="201">Usuário criado com sucesso</response>
        /// <response code="400">Dados inválidos ou email já cadastrado</response>
        [HttpPost]
        [SwaggerOperation(
            Summary = "Criar usuário",
            Description = "Cria uma nova conta de usuário no sistema",
            OperationId = "RegisterUser"
        )]
        [ProducesResponseType(typeof(ResponseRegisteredUserJson), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register(
            [FromServices] IRegisterUserUseCase useCase,
            [FromBody] RequestRegisterUserJson request)
        {
            var result = await useCase.Execute(request);
            return Created(string.Empty, result);
        }

        /// <summary>
        /// Obtém o perfil do usuário autenticado
        /// </summary>
        /// <param name="useCase">Use case para obter perfil</param>
        /// <returns>Dados do perfil do usuário</returns>
        /// <response code="200">Perfil do usuário obtido com sucesso</response>
        /// <response code="401">Token de autenticação inválido ou ausente</response>
        [HttpGet]
        [SwaggerOperation(
            Summary = "Obter perfil do usuário",
            Description = "Retorna as informações do perfil do usuário autenticado",
            OperationId = "GetUserProfile"
        )]
        [ProducesResponseType(typeof(ResponseUserProfileJson), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status401Unauthorized)]
        [AuthenticatedUser]
        public async Task<IActionResult> GetUserProfile([FromServices] IGetUserProfileUseCase useCase)
        {
            var result = await useCase.Execute();
            return Ok(result);
        }

        /// <summary>
        /// Atualiza os dados do usuário autenticado
        /// </summary>
        /// <param name="useCase">Use case para atualizar usuário</param>
        /// <param name="request">Dados atualizados do usuário</param>
        /// <returns>Confirmação da atualização</returns>
        /// <response code="204">Usuário atualizado com sucesso</response>
        /// <response code="400">Dados inválidos fornecidos</response>
        /// <response code="401">Token de autenticação inválido ou ausente</response>
        [HttpPut]
        [SwaggerOperation(
            Summary = "Atualizar perfil do usuário",
            Description = "Atualiza as informações do perfil do usuário autenticado",
            OperationId = "UpdateUser"
        )]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status401Unauthorized)]
        [AuthenticatedUser]
        public async Task<IActionResult> Update(
            [FromServices] IUpdateUserUseCase useCase,
            [FromBody] RequestUpdateUserJson request)
        {
            await useCase.Execute(request);
            return NoContent();
        }

        /// <summary>
        /// Altera a senha do usuário autenticado
        /// </summary>
        /// <param name="useCase">Use case para alterar senha</param>
        /// <param name="request">Dados para alteração de senha (senha atual e nova senha)</param>
        /// <returns>Confirmação da alteração</returns>
        /// <response code="204">Senha alterada com sucesso</response>
        /// <response code="400">Dados inválidos ou senha atual incorreta</response>
        /// <response code="401">Token de autenticação inválido ou ausente</response>
        [HttpPut("change-password")]
        [SwaggerOperation(
            Summary = "Alterar senha",
            Description = "Altera a senha do usuário autenticado",
            OperationId = "ChangePassword"
        )]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status401Unauthorized)]
        [AuthenticatedUser]
        public async Task<IActionResult> ChangePassword(
            [FromServices] IChangePasswordUseCase useCase,
            [FromBody] RequestChangePasswordJson request)
        {
            await useCase.Execute(request);
            return NoContent();
        }
    }
}