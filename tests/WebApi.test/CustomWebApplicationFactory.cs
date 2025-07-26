using CommonTestUtilities.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MyRecipeBook.Infrastructure.DataAccess;

namespace WebApi.test
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {

        private MyRecipeBook.Domain.Entities.User _user = default!;
        private string _password = string.Empty;

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Test")
                .ConfigureServices(services =>
                {
                    var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<MyRecipeBookDbContext>));

                    if (descriptor is not null)
                        services.Remove(descriptor);

                    var provider = services.AddEntityFrameworkInMemoryDatabase().BuildServiceProvider();

                    services.AddDbContext<MyRecipeBookDbContext>(options =>
                    {
                        options.UseInMemoryDatabase("InMemoryDbForTesting");
                        options.UseInternalServiceProvider(provider);
                    });

                    using var scope = services.BuildServiceProvider().CreateScope();

                    var dbContext = scope.ServiceProvider.GetRequiredService<MyRecipeBookDbContext>();

                    dbContext.Database.EnsureDeleted();

                    StartDatabase(dbContext);
                });
        }


        public string GetEmail() => _user.Email;
        public string GetPassword() => _password;
        public string GetName() => _user.Name;
        public Guid GetUserIdentifier() => _user.UserIdentifier;

        private void StartDatabase(MyRecipeBookDbContext dbContext)
        {
            (_user, _password) = UserBuilder.Build();

            dbContext.Users.Add(_user);

            dbContext.CookingTime.AddRange(
                new MyRecipeBook.Domain.Entities.CookingTime { Id = MyRecipeBook.Domain.Enums.RecipeCookingTime.Less_10_Minutes, Description = "< 10 mins" },
                new MyRecipeBook.Domain.Entities.CookingTime { Id = MyRecipeBook.Domain.Enums.RecipeCookingTime.Between_10_30_Minutes, Description = "10-30 mins" },
                new MyRecipeBook.Domain.Entities.CookingTime { Id = MyRecipeBook.Domain.Enums.RecipeCookingTime.Between_30_60_Minutes, Description = "30-60 mins" },
                new MyRecipeBook.Domain.Entities.CookingTime { Id = MyRecipeBook.Domain.Enums.RecipeCookingTime.Greater_60_Minutes, Description = "> 60 mins" }
            );

            dbContext.Difficulty.AddRange(
                new MyRecipeBook.Domain.Entities.Difficulty { Id = MyRecipeBook.Domain.Enums.RecipeDifficulty.Low, Description = "Low" },
                new MyRecipeBook.Domain.Entities.Difficulty { Id = MyRecipeBook.Domain.Enums.RecipeDifficulty.Medium, Description = "Medium" },
                new MyRecipeBook.Domain.Entities.Difficulty { Id = MyRecipeBook.Domain.Enums.RecipeDifficulty.High, Description = "High" }
            );

            dbContext.DishTypes.AddRange(
                new MyRecipeBook.Domain.Entities.DishTypes { Id = MyRecipeBook.Domain.Enums.RecipeDishType.Breakfast, Description = "Breakfast" },
                new MyRecipeBook.Domain.Entities.DishTypes { Id = MyRecipeBook.Domain.Enums.RecipeDishType.Lunch, Description = "Lunch" },
                new MyRecipeBook.Domain.Entities.DishTypes { Id = MyRecipeBook.Domain.Enums.RecipeDishType.Appertizers, Description = "Appetizers" },
                new MyRecipeBook.Domain.Entities.DishTypes { Id = MyRecipeBook.Domain.Enums.RecipeDishType.Snack, Description = "Snack" },
                new MyRecipeBook.Domain.Entities.DishTypes { Id = MyRecipeBook.Domain.Enums.RecipeDishType.Dessert, Description = "Dessert" },
                new MyRecipeBook.Domain.Entities.DishTypes { Id = MyRecipeBook.Domain.Enums.RecipeDishType.Dinner, Description = "Dinner" },
                new MyRecipeBook.Domain.Entities.DishTypes { Id = MyRecipeBook.Domain.Enums.RecipeDishType.Drinks, Description = "Drinks" }
            );

            dbContext.SaveChanges();
        }
    }
}
