using Azure.Messaging.ServiceBus;
using Azure.Storage.Blobs;
using FluentMigrator.Runner;
using GenerativeAI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyRecipeBook.Domain.Repositories;
using MyRecipeBook.Domain.Repositories.CookingTime;
using MyRecipeBook.Domain.Repositories.Difficulty;
using MyRecipeBook.Domain.Repositories.DishType;
using MyRecipeBook.Domain.Repositories.Recipe;
using MyRecipeBook.Domain.Repositories.RecipesDishType;
using MyRecipeBook.Domain.Repositories.User;
using MyRecipeBook.Domain.Security.Cryptography;
using MyRecipeBook.Domain.Security.Tokens;
using MyRecipeBook.Domain.Services.GeminiApi;
using MyRecipeBook.Domain.Services.LoggedUser;
using MyRecipeBook.Domain.Services.ServiceBus;
using MyRecipeBook.Domain.Services.Storage;
using MyRecipeBook.Infrastructure.DataAccess;
using MyRecipeBook.Infrastructure.DataAccess.Repositories;
using MyRecipeBook.Infrastructure.Extensions;
using MyRecipeBook.Infrastructure.Security.Cryptography;
using MyRecipeBook.Infrastructure.Security.Tokens.Access.Generator;
using MyRecipeBook.Infrastructure.Security.Tokens.Access.Validator;
using MyRecipeBook.Infrastructure.Services.GeminiApi;
using MyRecipeBook.Infrastructure.Services.LoggedUser;
using MyRecipeBook.Infrastructure.Services.ServiceBus;
using MyRecipeBook.Infrastructure.Services.Storage;
using System.Reflection;

namespace MyRecipeBook.Infrastructure
{
	public static class DepedencyInjectionExtension
	{

		private const string QUEUE_NAME = "user";
		public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
		{
			AddRepositories(services);
			AddLoggedUser(services);
			AddTokens(services, configuration);
			AddPasswordEncripter(services, configuration);
			AddGeminiAI(services, configuration);
			AddAzureStorage(services, configuration);
			AddQueue(services, configuration);

			if(configuration.IsUnitTestEnviroment()) return;

			AddDbContext(services, configuration);
			AddFluentMigrator(services, configuration);
		}

		private static void AddDbContext(IServiceCollection services, IConfiguration configuration)
		{
			var connectionString = configuration.ConnectionString();
			var serverVersion = new MySqlServerVersion(new Version(8, 0, 40));

			services.AddDbContext<MyRecipeBookDbContext>(dbContextOptions =>
			{
				dbContextOptions.UseMySql(connectionString, serverVersion);
			});
		}

		private static void AddRepositories(IServiceCollection services)
		{
			services.AddScoped<IUnitOfWork, UnitOfWork>();

			services.AddScoped<IUserWriteOnlyRepository, UserRepository>();
			services.AddScoped<IUserReadOnlyRepository, UserRepository>();
			services.AddScoped<IUserUpdateOnlyRepository, UserRepository>();
			services.AddScoped<IUserDeleteOnlyRepository, UserRepository>();

			services.AddScoped<ICookingTimeReadOnlyRepository, CookingTimeRepository>();
			services.AddScoped<IDifficultyReadOnlyRepository, DifficultyRepository>();
			services.AddScoped<IDishTypeReadOnlyRepository, DishTypesRepository>();

			services.AddScoped<IRecipeWriteOnlyRepository, RecipeRepository>();
			services.AddScoped<IRecipeReadOnlyRepository, RecipeRepository>();
			services.AddScoped<IRecipeUpdateOnlyRepository, RecipeRepository>();

			services.AddScoped<IRecipesDishTypeWriteOnlyRepository, RecipesDishTypeRepository>();

		}

		private static void AddQueue(IServiceCollection services, IConfiguration configuration)
		{
			var connectionString = configuration.GetValue<string>("Settings:ServiceBus:DeleteUserAccount");

			var client = new ServiceBusClient(connectionString, new ServiceBusClientOptions
			{
				TransportType = ServiceBusTransportType.AmqpWebSockets
			});

			var deleteQueue = new DeleteUserQueue(client.CreateSender(QUEUE_NAME));

			var deleteUserProcessor = new DeleteUserProcessor(client.CreateProcessor(QUEUE_NAME, new ServiceBusProcessorOptions
			{
				MaxConcurrentCalls = 1
			}));

			services.AddSingleton(deleteUserProcessor.GetProcessor());

			services.AddScoped<IDeleteUserQueue>(o => deleteQueue);
		}

		private static void AddFluentMigrator(IServiceCollection services, IConfiguration configuration)
		{
			var connectionString = configuration.ConnectionString();

			services.AddFluentMigratorCore().ConfigureRunner(options =>
			{
				options
					.AddMySql5()
					.WithGlobalConnectionString(connectionString)
					.ScanIn(Assembly.Load("MyRecipeBook.Infrastructure"))
					.For.All();
			});
		}

		private static void AddTokens(IServiceCollection services, IConfiguration configuration)
		{
			var expirationTimeMinutes = configuration.GetValue<uint>("Settings:Jwt:ExpirationTimeMinutes");
			var signingKey = configuration.GetValue<string>("Settings:Jwt:SigningKey");

			services.AddScoped<IAccessTokenGenerator>(options => new JwtTokenGenerator(expirationTimeMinutes, signingKey!));
			services.AddScoped<IAccessTokenValidator>(options => new JwtTokenValidator(signingKey!));
		}

		private static void AddLoggedUser(IServiceCollection services) => services.AddScoped<ILoggedUser, LoggedUser>();

		private static void AddPasswordEncripter(IServiceCollection services, IConfiguration configuration)
		{
			var additionalKey = configuration.GetValue<string>("Settings:Password:AdditionalKey");

			services.AddScoped<IPasswordEncripter>(option => new Sha512Encripter(additionalKey!));
		}

		private static void AddGeminiAI(IServiceCollection services, IConfiguration configuration)
		{
			services.AddScoped<IGenerateRecipeAI, GeminiService>();

			var geminiApiKey = configuration.GetValue<string>("Settings:GeminiAI:ApiKey")!;

			services.AddScoped<IGenerativeAI>(option => new GoogleAi(geminiApiKey));
		}
		private static void AddAzureStorage(IServiceCollection services, IConfiguration configuration)
		{
			var connectionString = configuration.GetValue<string>("Settings:BlobStorage:Azure");

			if(string.IsNullOrEmpty(connectionString)) return;

			services.AddScoped<IBlobStorageService>(option => new AzureStorageService(new BlobServiceClient(connectionString)));
		}
	}
}
