using Microsoft.AspNetCore.Http;

namespace MyRecipeBook.Communication.Request;
public class RequestRegisterRecipeFormData : RequestRecipeJson
{
    public IFormFile? Image { get; set; }
}