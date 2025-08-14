using Microsoft.AspNetCore.Mvc;
using MyRecipeBook.API.Attributes;
using MyRecipeBook.Application.UseCases.User.ChangePassword;
using MyRecipeBook.Application.UseCases.User.Delete.Request;
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

        [HttpDelete]
        [SwaggerOperation(
            Summary = "Deletar Conta",
            Description = "Deleta conta do usuário",
            OperationId = "DeleteAccount"
        )]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status401Unauthorized)]
        [AuthenticatedUser]
        public async Task<IActionResult> Delete(
            [FromServices] IDeleteUserUseCase useCase)
        {
            await useCase.Execute();

            return NoContent();
        }
    }
}