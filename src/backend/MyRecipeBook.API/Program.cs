using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MyRecipeBook.API.BackgroundServices;
using MyRecipeBook.API.Converters;
using MyRecipeBook.API.Filters;
using MyRecipeBook.API.Middleware;
using MyRecipeBook.API.OpenApi;
using MyRecipeBook.API.Token;
using MyRecipeBook.Application;
using MyRecipeBook.Domain.Extension;
using MyRecipeBook.Domain.Security.Tokens;
using MyRecipeBook.Infrastructure;
using MyRecipeBook.Infrastructure.Extensions;
using MyRecipeBook.Infrastructure.Migrations;
using Scalar.AspNetCore;
using System.Reflection;


var builder = WebApplication.CreateBuilder(args);

const string _bearer = "Bearer";

var configuration = builder.Configuration;

var gitHubUrl = configuration.GetValue<string>("Settings:OpenApi:GitHubUrl")!;
var mitLicenseUrl = configuration.GetValue<string>("Settings:OpenApi:MitLicenseUrl")!;
var contactName = configuration.GetValue<string>("Settings:OpenApi:ContactName")!;
var licenseName = configuration.GetValue<string>("Settings:OpenApi:LicenseName")!;



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
			Name = contactName,
			Url = new Uri(gitHubUrl)
		},
		License = new OpenApiLicense
		{
			Name = licenseName,
			Url = new Uri(mitLicenseUrl)
		},
	});

	var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
	var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
	if(File.Exists(xmlPath))
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

if(builder.Configuration.IsUnitTestEnviroment().isFalse())
{
	builder.Services.AddHostedService<DeleteUserService>();

	AddGoogleAuthentication();
}

var app = builder.Build();



if(app.Environment.IsDevelopment())
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
	if(builder.Configuration.IsUnitTestEnviroment()) return;

	var connectionString = builder.Configuration.ConnectionString();

	var serviceScope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();
	DatabaseMigration.Migrate(connectionString, serviceScope.ServiceProvider);
}

void AddGoogleAuthentication()
{
	var clientId = builder.Configuration.GetValue<string>("Settings:Google:ClientId")!;
	var clientSecret = builder.Configuration.GetValue<string>("Settings:Google:ClientSecret")!;

	builder.Services.AddAuthentication(config =>
	{
		config.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
	})
		.AddCookie()
		.AddGoogle(googleOption =>
		{
			googleOption.ClientId = clientId;
			googleOption.ClientSecret = clientSecret;
		});
}

public partial class Program
{
	protected Program() { }
}
