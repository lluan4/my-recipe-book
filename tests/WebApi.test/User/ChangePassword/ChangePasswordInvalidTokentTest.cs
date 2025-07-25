using CommonTestUtilities.Tokens;
using MyRecipeBook.Communication.Request;
using Shouldly;
using System.Net;

namespace WebApi.test.User.ChangePassword;

public class ChangePasswordInvalidTokentTest : MyRecipeBookClassFixture
{
    private readonly string METHOD = "user/change-password";
    private readonly RequestChangePasswordJson _request = new RequestChangePasswordJson();

    public ChangePasswordInvalidTokentTest(CustomWebApplicationFactory webApplication) : base(webApplication) { }

    [Fact]
    public async Task Error_Token_Invalid()
    {
 
        var response = await DoPut(METHOD, _request, token: "tokenInvalid");

        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Error_Without_Token()
    {
        var response = await DoPut(METHOD, _request, token: string.Empty);

        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Error_Token_With_NotFound()
    {
        var token = JwtTokensGeneratorBuilder.Build().Generate(Guid.NewGuid());

        var response = await DoPut(METHOD, _request, token);

        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }

}


