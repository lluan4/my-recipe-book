using CommonTestUtilities.Requests;
using CommonTestUtilities.Tokens;
using MyRecipeBook.Communication.Request;
using MyRecipeBook.Exceptions;
using Shouldly;
using System.Text.Json;
using WebApi.test.InlineData;

namespace WebApi.test.User.ChangePassword;



public class ChangePasswordTest : MyRecipeBookClassFixture
{
    private readonly string METHOD = "user/change-password";

    private readonly string _password;
    private readonly string _email;
    private readonly Guid _userIdentifier;
    public ChangePasswordTest(CustomWebApplicationFactory factory) : base(factory)
    {
        _password = factory.GetPassword();
        _email = factory.GetEmail();
        _userIdentifier = factory.GetUserIdentifier();
    }

    [Fact]
    public async Task Success()
    {
        var request = RequestChangePasswordJsonBuilder.Build();
        request.Password = _password;

        var token = JwtTokensGeneratorBuilder.Build().Generate(_userIdentifier);

        var response = await DoPut(METHOD, request, token: token);

        response.StatusCode.ShouldBe(System.Net.HttpStatusCode.NoContent);

        var loginRequest = new RequestLoginJson
        {
            Email = _email,
            Password = _password
        };

        response = await DoPost("login", loginRequest);
        response.StatusCode.ShouldBe(System.Net.HttpStatusCode.Unauthorized);

        loginRequest.Password = request.NewPassword;

        response = await DoPost("login", loginRequest);
        response.StatusCode.ShouldBe(System.Net.HttpStatusCode.OK);
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_NewPassword_Empty(string culture)
    {
        var request = new RequestChangePasswordJson
        {
            Password = _password,
            NewPassword = string.Empty
        };

        var token = JwtTokensGeneratorBuilder.Build().Generate(_userIdentifier);

        var response = await DoPut(METHOD, request, token, culture);

        response.StatusCode.ShouldBe(System.Net.HttpStatusCode.BadRequest);

        var responseBody = await response.Content.ReadAsStringAsync();

        using var responseData =  JsonDocument.Parse(responseBody);

        var errors = responseData.RootElement.GetProperty("errors").EnumerateArray();

        var expectedMessage = ResourceMessagesException.ResourceManager.GetString("PASSWORD_EMPTY", new System.Globalization.CultureInfo(culture));

        errors.ShouldHaveSingleItem();
        errors.First().GetString().ShouldBe(expectedMessage);

    }
}

