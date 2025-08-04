using FluentMigrator;
using Microsoft.Extensions.Configuration;
using MyRecipeBook.Domain.Entities;
using MyRecipeBook.Domain.Enums;
using MyRecipeBook.Infrastructure.Security.Cryptography;
using System.Data;
using System.Security.Cryptography;
using System.Text;

namespace MyRecipeBook.Infrastructure.Migrations.Versions;

[Migration(DatabaseVersions.SEED_ADMIN_DATA, "Insert admin user and sample recipes")]
public class Version0000003 : VersionBase
{
    private readonly IConfiguration _configuration;
    public Version0000003(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public override void Up()
    {
        var adminUserIdentifier = Guid.NewGuid();
        var additionalKey = _configuration["Settings:Password:AdditionalKey"]
                            ?? throw new InvalidOperationException("Settings:Password:AdditionalKey configuration not found");

        var EncryptPassword = new Sha512Encripter(additionalKey);                            
        var adminPassword = EncryptPassword.Encrypt("12345678");

        var now = DateTime.UtcNow;

        Insert.IntoTable(TableName<User>())
            .Row(new
            {
                Name = "Administrator",
                Email = "admin@admin.com",
                Password = adminPassword,
                UserIdentifier = adminUserIdentifier,
                CreatedOn = now,
                Active = true
            });

        var adminUserId = 1;

        var recipes = GetSampleRecipes(adminUserId, now);

        foreach (var recipe in recipes)
        {
            Insert.IntoTable(TableName<Recipe>())
                .Row(recipe.RecipeData);

            foreach (var ingredient in recipe.Ingredients)
            {
                Insert.IntoTable(TableName<Ingredient>())
                    .Row(ingredient);
            }

            foreach (var instruction in recipe.Instructions)
            {
                Insert.IntoTable(TableName<Instruction>())
                    .Row(instruction);
            }

            foreach (var dishType in recipe.DishTypes)
            {
                Insert.IntoTable(TableName<Domain.Entities.RecipeDishType>())
                    .Row(dishType);
            }
        }
    }

    private static List<RecipeDataModel> GetSampleRecipes(long userId, DateTime createdOn)
    {
        return new List<RecipeDataModel>
        {
            new RecipeDataModel
            {
                RecipeData = new
                {
                    Title = "Pancakes Clássicos",
                    CookingTimeId = (int)RecipeCookingTime.Between_10_30_Minutes,
                    DifficultyId = (int)RecipeDifficulty.Low,
                    UserId = userId,
                    CreatedOn = createdOn,
                    Active = true
                },
                Ingredients = new[]
                {
                    new { Item = "2 xícaras de farinha de trigo", RecipeId = 1, CreatedOn = createdOn, Active = true },
                    new { Item = "2 colheres de sopa de açúcar", RecipeId = 1, CreatedOn = createdOn, Active = true },
                    new { Item = "2 ovos", RecipeId = 1, CreatedOn = createdOn, Active = true },
                    new { Item = "1 1/2 xícara de leite", RecipeId = 1, CreatedOn = createdOn, Active = true }
                },
                Instructions = new[]
                {
                    new { Step = 1, Description = "Misture os ingredientes secos em uma tigela", RecipeId = 1, CreatedOn = createdOn, Active = true },
                    new { Step = 2, Description = "Adicione os ovos e o leite, misture bem", RecipeId = 1, CreatedOn = createdOn, Active = true },
                    new { Step = 3, Description = "Aqueça a frigideira e despeje a massa", RecipeId = 1, CreatedOn = createdOn, Active = true }
                },
                DishTypes = new[]
                {
                    new { RecipeId = 1, DishTypeId = 1, CreatedOn = createdOn } // Breakfast
                }
            },
            new RecipeDataModel
            {
                RecipeData = new
                {
                    Title = "Salada Caesar",
                    CookingTimeId = (int)RecipeCookingTime.Less_10_Minutes,
                    DifficultyId = (int)RecipeDifficulty.Low,
                    UserId = userId,
                    CreatedOn = createdOn,
                    Active = true
                },
                Ingredients = new[]
                {
                    new { Item = "Alface romana", RecipeId = 2, CreatedOn = createdOn, Active = true },
                    new { Item = "Queijo parmesão", RecipeId = 2, CreatedOn = createdOn, Active = true },
                    new { Item = "Croutons", RecipeId = 2, CreatedOn = createdOn, Active = true },
                    new { Item = "Molho Caesar", RecipeId = 2, CreatedOn = createdOn, Active = true }
                },
                Instructions = new[]
                {
                    new { Step = 1, Description = "Lave e corte a alface", RecipeId = 2, CreatedOn = createdOn, Active = true },
                    new { Step = 2, Description = "Adicione croutons e queijo", RecipeId = 2, CreatedOn = createdOn, Active = true },
                    new { Step = 3, Description = "Regue com molho Caesar", RecipeId = 2, CreatedOn = createdOn, Active = true }
                },
                DishTypes = new[]
                {
                    new { RecipeId = 2, DishTypeId = 2, CreatedOn = createdOn } // Lunch
                }
            },
            new RecipeDataModel
            {
                RecipeData = new
                {
                    Title = "Lasanha Bolonhesa",
                    CookingTimeId = (int)RecipeCookingTime.Greater_60_Minutes,
                    DifficultyId = (int)RecipeDifficulty.High,
                    UserId = userId,
                    CreatedOn = createdOn,
                    Active = true
                },
                Ingredients = new[]
                {
                    new { Item = "Massa de lasanha", RecipeId = 3, CreatedOn = createdOn, Active = true },
                    new { Item = "Carne moída", RecipeId = 3, CreatedOn = createdOn, Active = true },
                    new { Item = "Molho de tomate", RecipeId = 3, CreatedOn = createdOn, Active = true },
                    new { Item = "Queijo mozzarella", RecipeId = 3, CreatedOn = createdOn, Active = true },
                    new { Item = "Molho bechamel", RecipeId = 3, CreatedOn = createdOn, Active = true }
                },
                Instructions = new[]
                {
                    new { Step = 1, Description = "Prepare o molho bolonhesa", RecipeId = 3, CreatedOn = createdOn, Active = true },
                    new { Step = 2, Description = "Cozinhe a massa de lasanha", RecipeId = 3, CreatedOn = createdOn, Active = true },
                    new { Step = 3, Description = "Monte as camadas alternando massa, molho e queijo", RecipeId = 3, CreatedOn = createdOn, Active = true },
                    new { Step = 4, Description = "Asse por 45 minutos a 180°C", RecipeId = 3, CreatedOn = createdOn, Active = true }
                },
                DishTypes = new[]
                {
                    new { RecipeId = 3, DishTypeId = 6, CreatedOn = createdOn } // Dinner
                }
            },
            new RecipeDataModel
            {
                RecipeData = new
                {
                    Title = "Smoothie de Frutas",
                    CookingTimeId = (int)RecipeCookingTime.Less_10_Minutes,
                    DifficultyId = (int)RecipeDifficulty.Low,
                    UserId = userId,
                    CreatedOn = createdOn,
                    Active = true
                },
                Ingredients = new[]
                {
                    new { Item = "1 banana", RecipeId = 4, CreatedOn = createdOn, Active = true },
                    new { Item = "1/2 xícara de morangos", RecipeId = 4, CreatedOn = createdOn, Active = true },
                    new { Item = "1 xícara de leite", RecipeId = 4, CreatedOn = createdOn, Active = true },
                    new { Item = "1 colher de mel", RecipeId = 4, CreatedOn = createdOn, Active = true }
                },
                Instructions = new[]
                {
                    new { Step = 1, Description = "Coloque todos os ingredientes no liquidificador", RecipeId = 4, CreatedOn = createdOn, Active = true },
                    new { Step = 2, Description = "Bata até ficar homogêneo", RecipeId = 4, CreatedOn = createdOn, Active = true },
                    new { Step = 3, Description = "Sirva gelado", RecipeId = 4, CreatedOn = createdOn, Active = true }
                },
                DishTypes = new[]
                {
                    new { RecipeId = 4, DishTypeId = 7, CreatedOn = createdOn } // Drinks
                }
            },
            new RecipeDataModel
            {
                RecipeData = new
                {
                    Title = "Bruschetta Italiana",
                    CookingTimeId = (int)RecipeCookingTime.Between_10_30_Minutes,
                    DifficultyId = (int)RecipeDifficulty.Low,
                    UserId = userId,
                    CreatedOn = createdOn,
                    Active = true
                },
                Ingredients = new[]
                {
                    new { Item = "Pão italiano fatiado", RecipeId = 5, CreatedOn = createdOn, Active = true },
                    new { Item = "Tomates frescos", RecipeId = 5, CreatedOn = createdOn, Active = true },
                    new { Item = "Manjericão fresco", RecipeId = 5, CreatedOn = createdOn, Active = true },
                    new { Item = "Alho", RecipeId = 5, CreatedOn = createdOn, Active = true },
                    new { Item = "Azeite extra virgem", RecipeId = 5, CreatedOn = createdOn, Active = true }
                },
                Instructions = new[]
                {
                    new { Step = 1, Description = "Torre o pão até dourar", RecipeId = 5, CreatedOn = createdOn, Active = true },
                    new { Step = 2, Description = "Esfregue alho no pão", RecipeId = 5, CreatedOn = createdOn, Active = true },
                    new { Step = 3, Description = "Cubra com tomate e manjericão", RecipeId = 5, CreatedOn = createdOn, Active = true },
                    new { Step = 4, Description = "Regue com azeite", RecipeId = 5, CreatedOn = createdOn, Active = true }
                },
                DishTypes = new[]
                {
                    new { RecipeId = 5, DishTypeId = 3, CreatedOn = createdOn } // Appetizers
                }
            },
            new RecipeDataModel
            {
                RecipeData = new
                {
                    Title = "Bolo de Chocolate",
                    CookingTimeId = (int)RecipeCookingTime.Between_30_60_Minutes,
                    DifficultyId = (int)RecipeDifficulty.Medium,
                    UserId = userId,
                    CreatedOn = createdOn,
                    Active = true
                },
                Ingredients = new[]
                {
                    new { Item = "2 xícaras de farinha", RecipeId = 6, CreatedOn = createdOn, Active = true },
                    new { Item = "1 xícara de chocolate em pó", RecipeId = 6, CreatedOn = createdOn, Active = true },
                    new { Item = "3 ovos", RecipeId = 6, CreatedOn = createdOn, Active = true },
                    new { Item = "1 xícara de açúcar", RecipeId = 6, CreatedOn = createdOn, Active = true },
                    new { Item = "1/2 xícara de óleo", RecipeId = 6, CreatedOn = createdOn, Active = true }
                },
                Instructions = new[]
                {
                    new { Step = 1, Description = "Misture ingredientes secos", RecipeId = 6, CreatedOn = createdOn, Active = true },
                    new { Step = 2, Description = "Adicione ovos e óleo", RecipeId = 6, CreatedOn = createdOn, Active = true },
                    new { Step = 3, Description = "Asse por 40 minutos a 180°C", RecipeId = 6, CreatedOn = createdOn, Active = true }
                },
                DishTypes = new[]
                {
                    new { RecipeId = 6, DishTypeId = 5, CreatedOn = createdOn } // Dessert
                }
            },
            new RecipeDataModel
            {
                RecipeData = new
                {
                    Title = "Sanduíche Natural",
                    CookingTimeId = (int)RecipeCookingTime.Less_10_Minutes,
                    DifficultyId = (int)RecipeDifficulty.Low,
                    UserId = userId,
                    CreatedOn = createdOn,
                    Active = true
                },
                Ingredients = new[]
                {
                    new { Item = "Pão integral", RecipeId = 7, CreatedOn = createdOn, Active = true },
                    new { Item = "Peito de peru", RecipeId = 7, CreatedOn = createdOn, Active = true },
                    new { Item = "Queijo branco", RecipeId = 7, CreatedOn = createdOn, Active = true },
                    new { Item = "Alface", RecipeId = 7, CreatedOn = createdOn, Active = true },
                    new { Item = "Tomate", RecipeId = 7, CreatedOn = createdOn, Active = true }
                },
                Instructions = new[]
                {
                    new { Step = 1, Description = "Corte o pão ao meio", RecipeId = 7, CreatedOn = createdOn, Active = true },
                    new { Step = 2, Description = "Monte o sanduíche com os ingredientes", RecipeId = 7, CreatedOn = createdOn, Active = true },
                    new { Step = 3, Description = "Sirva imediatamente", RecipeId = 7, CreatedOn = createdOn, Active = true }
                },
                DishTypes = new[]
                {
                    new { RecipeId = 7, DishTypeId = 4, CreatedOn = createdOn } // Snack
                }
            },
            new RecipeDataModel
            {
                RecipeData = new
                {
                    Title = "Risotto de Cogumelos",
                    CookingTimeId = (int)RecipeCookingTime.Between_30_60_Minutes,
                    DifficultyId = (int)RecipeDifficulty.High,
                    UserId = userId,
                    CreatedOn = createdOn,
                    Active = true
                },
                Ingredients = new[]
                {
                    new { Item = "Arroz arbóreo", RecipeId = 8, CreatedOn = createdOn, Active = true },
                    new { Item = "Cogumelos variados", RecipeId = 8, CreatedOn = createdOn, Active = true },
                    new { Item = "Caldo de legumes", RecipeId = 8, CreatedOn = createdOn, Active = true },
                    new { Item = "Vinho branco", RecipeId = 8, CreatedOn = createdOn, Active = true },
                    new { Item = "Queijo parmesão", RecipeId = 8, CreatedOn = createdOn, Active = true }
                },
                Instructions = new[]
                {
                    new { Step = 1, Description = "Refogue os cogumelos", RecipeId = 8, CreatedOn = createdOn, Active = true },
                    new { Step = 2, Description = "Adicione o arroz e o vinho", RecipeId = 8, CreatedOn = createdOn, Active = true },
                    new { Step = 3, Description = "Vá adicionando o caldo aos poucos", RecipeId = 8, CreatedOn = createdOn, Active = true },
                    new { Step = 4, Description = "Finalize com queijo parmesão", RecipeId = 8, CreatedOn = createdOn, Active = true }
                },
                DishTypes = new[]
                {
                    new { RecipeId = 8, DishTypeId = 6, CreatedOn = createdOn } // Dinner
                }
            },
            new RecipeDataModel
            {
                RecipeData = new
                {
                    Title = "Salada de Frutas",
                    CookingTimeId = (int)RecipeCookingTime.Less_10_Minutes,
                    DifficultyId = (int)RecipeDifficulty.Low,
                    UserId = userId,
                    CreatedOn = createdOn,
                    Active = true
                },
                Ingredients = new[]
                {
                    new { Item = "Maçã", RecipeId = 9, CreatedOn = createdOn, Active = true },
                    new { Item = "Banana", RecipeId = 9, CreatedOn = createdOn, Active = true },
                    new { Item = "Uvas", RecipeId = 9, CreatedOn = createdOn, Active = true },
                    new { Item = "Laranja", RecipeId = 9, CreatedOn = createdOn, Active = true },
                    new { Item = "Mel", RecipeId = 9, CreatedOn = createdOn, Active = true }
                },
                Instructions = new[]
                {
                    new { Step = 1, Description = "Corte todas as frutas em pedaços", RecipeId = 9, CreatedOn = createdOn, Active = true },
                    new { Step = 2, Description = "Misture em uma tigela", RecipeId = 9, CreatedOn = createdOn, Active = true },
                    new { Step = 3, Description = "Regue com mel", RecipeId = 9, CreatedOn = createdOn, Active = true }
                },
                DishTypes = new[]
                {
                    new { RecipeId = 9, DishTypeId = 5, CreatedOn = createdOn } // Dessert
                }
            },
            new RecipeDataModel
            {
                RecipeData = new
                {
                    Title = "Frango Grelhado",
                    CookingTimeId = (int)RecipeCookingTime.Between_30_60_Minutes,
                    DifficultyId = (int)RecipeDifficulty.Medium,
                    UserId = userId,
                    CreatedOn = createdOn,
                    Active = true
                },
                Ingredients = new[]
                {
                    new { Item = "Peito de frango", RecipeId = 10, CreatedOn = createdOn, Active = true },
                    new { Item = "Azeite", RecipeId = 10, CreatedOn = createdOn, Active = true },
                    new { Item = "Alho", RecipeId = 10, CreatedOn = createdOn, Active = true },
                    new { Item = "Ervas finas", RecipeId = 10, CreatedOn = createdOn, Active = true },
                    new { Item = "Sal e pimenta", RecipeId = 10, CreatedOn = createdOn, Active = true }
                },
                Instructions = new[]
                {
                    new { Step = 1, Description = "Tempere o frango com sal, pimenta e ervas", RecipeId = 10, CreatedOn = createdOn, Active = true },
                    new { Step = 2, Description = "Aqueça a grelha com azeite", RecipeId = 10, CreatedOn = createdOn, Active = true },
                    new { Step = 3, Description = "Grelhe por 15 minutos de cada lado", RecipeId = 10, CreatedOn = createdOn, Active = true },
                    new { Step = 4, Description = "Sirva quente", RecipeId = 10, CreatedOn = createdOn, Active = true }
                },
                DishTypes = new[]
                {
                    new { RecipeId = 10, DishTypeId = 2, CreatedOn = createdOn } // Lunch
                }
            }
        };
    }
    sealed class RecipeDataModel
    {
        public object RecipeData { get; set; } = null!; 
        public object[] Ingredients { get; set; } = [];
        public object[] Instructions { get; set; } = [];
        public object[] DishTypes { get; set; } = [];
    }

    
}

