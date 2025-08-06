using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MyRecipeBook.API.Converters;
using MyRecipeBook.API.Filters;
using MyRecipeBook.API.Middleware;
using MyRecipeBook.API.OpenApi;
using MyRecipeBook.API.Token;
using MyRecipeBook.Application;
using MyRecipeBook.Domain.Security.Tokens;
using MyRecipeBook.Infrastructure;
using MyRecipeBook.Infrastructure.Extensions;
using MyRecipeBook.Infrastructure.Migrations;
using Scalar.AspNetCore;
using System.Reflection;


var builder = WebApplication.CreateBuilder(args);

const string _bearer = "Bearer";

const string _gitHubUrl = "https://github.com/lluan4/my-recipe-book";
const string _mitLicense = "https://opensource.org/licenses/MIT";


builder.Services.AddControllers().AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new StringConverter()));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SupportNonNullableReferenceTypes();
    c.UseAllOfToExtendReferenceSchemas();

    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "My Recipe Book API",
        Version = "v1.0.0",
        Description = "API completa para gerenciamento de receitas culinárias. " +
                     "Permite criar, editar, listar e excluir receitas, além de gerenciar usuários e autenticação.",
        Contact = new OpenApiContact
        {
            Name = "Luan Lima",
            Url = new Uri(_gitHubUrl)
        },
        License = new OpenApiLicense
        {
            Name = "MIT License",
            Url = new Uri(_mitLicense)
        },
    });

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        c.IncludeXmlComments(xmlPath);
    }

    c.EnableAnnotations();
    c.DocumentFilter<TagOrdererFilter>();

    c.OperationFilter<IdsFilter>();

    c.AddSecurityDefinition(_bearer, new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter your JWT token in the text input below." +
        "Example: 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...'",

    });

    c.OrderActionsBy(api =>
    {
        var controllerOrder = new Dictionary<string, int>
        {
            ["User"] = 1,
            ["Login"] = 2,
            ["Dashboard"] = 3,
            ["Recipes"] = 4
        };

        var ctrl = api.ActionDescriptor.RouteValues["controller"] ?? "";
        var grp = controllerOrder.TryGetValue(ctrl, out var g) ? g : 99;

        var method = api.HttpMethod?.ToUpperInvariant() switch
        {
            "GET" => 1,
            "POST" => 2,
            "PUT" => 3,
            "DELETE" => 4,
            _ => 9
        };

        // 01_01_/user/{id}
        return $"{grp:D2}_{method:D2}_{api.RelativePath}";
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                          new OpenApiSecurityScheme
                          {
                              Reference = new OpenApiReference
                              {
                                  Type = ReferenceType.SecurityScheme,
                                  Id = _bearer
                              },
                              Scheme = "bearer",
                              Name = _bearer,
                              In = ParameterLocation.Header
                          },
                         new List<string>()
                    }
                });
});

builder.Services.AddMvc(options => options.Filters.Add<ExceptionFilter>());


builder.Services.AddApplication(builder.Configuration);
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddScoped<ITokenProvider, HttpContextTokenValue>();
builder.Services.AddHttpContextAccessor();

builder.Services.AddRouting(options => options.LowercaseUrls = true);

builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer<BearerSecuritySchemeTransformer>();
    options.AddDocumentTransformer<ScalarIdsDocumentTransformer>();
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters { };
    });


var app = builder.Build();


if (app.Environment.IsDevelopment())
{

    app.MapOpenApi();

    app.MapScalarApiReference(options =>
    {
        options.WithTheme(ScalarTheme.BluePlanet)
            .WithDarkModeToggle(true)
            .WithSidebar(true)     
            .WithModels(true)
            .WithTitle("My Recipe Book")
            .AddPreferredSecuritySchemes(_bearer)
         
            .WithTagSorter(TagSorter.Alpha)
            .WithOperationSorter(OperationSorter.Method);
    });

    app.UseSwagger();      
    app.UseSwaggerUI();
}

app.UseMiddleware<CultureMiddleware>();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

MigrateDatabase();

await app.RunAsync();

void MigrateDatabase()
{
    if (builder.Configuration.IsUnitTestEnviroment()) return;

    var connectionString  = builder.Configuration.ConnectionString();

    var serviceScope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();
    DatabaseMigration.Migrate(connectionString, serviceScope.ServiceProvider);
}


public partial class Program
{
    protected Program() { }
}
