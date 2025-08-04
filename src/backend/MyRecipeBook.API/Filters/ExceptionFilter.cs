using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using MyRecipeBook.Communication.Response;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.ExceptionsBase;

namespace MyRecipeBook.API.Filters
{
    public class ExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            if (context.Exception is MyRecipeBookException myRecipeBookException)
                HandleProjectException(myRecipeBookException, context);
            else
                ThrowUnknowException(context);

        }

        private static void HandleProjectException(MyRecipeBookException myRecipeBookException, ExceptionContext context)
        {
            context.HttpContext.Response.StatusCode = (int)myRecipeBookException.GetStatusCode();
            context.Result = new ObjectResult(new ResponseErrorJson(myRecipeBookException.GetErrorMessages()));

            //if (context.Exception is InvalidLoginException)
            //{
            //    context.HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            //    context.Result = new UnauthorizedObjectResult(new ResponseErrorJson(context.Exception.Message));
            //    return;
            //}

            //if (context.Exception is ErrorOnValidationException exception)
            //{
            //    context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            //    context.Result = new BadRequestObjectResult(new ResponseErrorJson(exception.ErrorMessages));
            //}

            //if (context.Exception is NotFoundException)
            //{
            //    context.HttpContext.Response.StatusCode = StatusCodes.Status404NotFound;
            //    context.Result = new NotFoundObjectResult(new ResponseErrorJson(context.Exception.Message));
            //}
        }

        private static void ThrowUnknowException(ExceptionContext context)
        {
            context.HttpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Result = new ObjectResult(new ResponseErrorJson(ResourceMessagesException.UNKNOWN_ERROR));
        }
    }
}
