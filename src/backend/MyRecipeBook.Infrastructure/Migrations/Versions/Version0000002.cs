using FluentMigrator;
using MyRecipeBook.Infrastructure.Persistence;
using System.Data;

namespace MyRecipeBook.Infrastructure.Migrations.Versions
{
    [Migration(DatabaseVersions.TABLE_RECIPES, "Create table to save the recipes' information")]
    public class Version0000002 : VersionBase
    {
        public override void Up()
        {
            // Tabelas de referência
            CreateReferenceTable(DbObjects.CookingTime.TableName);
            Insert.IntoTable(DbObjects.CookingTime.TableName)
                .Row(new { Description = "< 10 mins" })
                .Row(new { Description = "10-30 mins" })
                .Row(new { Description = "30-60 mins" })
                .Row(new { Description = "> 60 mins" });

            CreateReferenceTable(DbObjects.Difficulty.TableName);
            Insert.IntoTable(DbObjects.Difficulty.TableName)
                .Row(new { Description = "Low" })
                .Row(new { Description = "Medium" })
                .Row(new { Description = "High" });

            CreateReferenceTable(DbObjects.DishTypes.TableName);
            Insert.IntoTable(DbObjects.DishTypes.TableName)
                .Row(new { Description = "Breakfast" })
                .Row(new { Description = "Lunch" })
                .Row(new { Description = "Appetizers" })
                .Row(new { Description = "Snack" })
                .Row(new { Description = "Dessert" })
                .Row(new { Description = "Dinner" })
                .Row(new { Description = "Drinks" });

            // Tabela principal
            CreateTable(DbObjects.Recipes.TableName)
                .WithColumn(DbObjects.Recipes.Title).AsString(200).NotNullable()
                .WithColumn(DbObjects.Recipes.CookingTimeId).AsInt32().Nullable()
                    .ForeignKey(DbObjects.Naming.FK(DbObjects.Recipes.TableName, DbObjects.CookingTime.TableName), DbObjects.CookingTime.TableName, DbObjects.CookingTime.Id)
                .WithColumn(DbObjects.Recipes.DifficultyId).AsInt32().Nullable()
                    .ForeignKey(DbObjects.Naming.FK(DbObjects.Recipes.TableName, DbObjects.Difficulty.TableName), DbObjects.Difficulty.TableName, DbObjects.Difficulty.Id)
                .WithColumn(DbObjects.Recipes.UserId).AsInt64().NotNullable()
                    .ForeignKey(DbObjects.Naming.FK(DbObjects.Recipes.TableName, DbObjects.Users.TableName), DbObjects.Users.TableName, DbObjects.Users.Id);

            // Tabelas dependentes
            CreateTable(DbObjects.Ingredients.TableName)
                .WithColumn(DbObjects.Ingredients.Item).AsString(100).NotNullable()
                .WithColumn(DbObjects.Ingredients.RecipeId).AsInt64().NotNullable()
                    .ForeignKey(DbObjects.Naming.FK(DbObjects.Ingredients.TableName, DbObjects.Recipes.TableName), DbObjects.Recipes.TableName, DbObjects.Recipes.Id)
                        .OnDelete(Rule.Cascade);

            CreateTable(DbObjects.Instructions.TableName)
                .WithColumn(DbObjects.Instructions.Step).AsInt32().NotNullable()
                .WithColumn(DbObjects.Instructions.Description).AsString(2000).NotNullable()
                .WithColumn(DbObjects.Instructions.RecipeId).AsInt64().NotNullable()
                    .ForeignKey(DbObjects.Naming.FK(DbObjects.Instructions.TableName, DbObjects.Recipes.TableName), DbObjects.Recipes.TableName, DbObjects.Recipes.Id)
                        .OnDelete(Rule.Cascade);

            // Tabela de ligação
            CreateJunctionTable(DbObjects.RecipesDishTypes.TableName)
                .WithColumn(DbObjects.RecipesDishTypes.RecipeId).AsInt64().NotNullable()
                    .ForeignKey(DbObjects.Naming.FK(DbObjects.RecipesDishTypes.TableName, DbObjects.Recipes.TableName), DbObjects.Recipes.TableName, DbObjects.Recipes.Id)
                        .OnDelete(Rule.Cascade)
                .WithColumn(DbObjects.RecipesDishTypes.DishTypeId).AsInt32().NotNullable()
                    .ForeignKey(DbObjects.Naming.FK(DbObjects.RecipesDishTypes.TableName, DbObjects.DishTypes.TableName), DbObjects.DishTypes.TableName, DbObjects.DishTypes.Id);

            // Constraints e índices
            Create.PrimaryKey(DbObjects.Naming.PK(DbObjects.RecipesDishTypes.TableName))
                .OnTable(DbObjects.RecipesDishTypes.TableName)
                .Columns(DbObjects.RecipesDishTypes.RecipeId, DbObjects.RecipesDishTypes.DishTypeId);

            Create.UniqueConstraint(DbObjects.Naming.UC(DbObjects.Instructions.TableName, DbObjects.Instructions.RecipeId, DbObjects.Instructions.Step))
                .OnTable(DbObjects.Instructions.TableName)
                .Columns(DbObjects.Instructions.RecipeId, DbObjects.Instructions.Step);

            // Índices para performance
            Create.Index(DbObjects.Naming.IX(DbObjects.Recipes.TableName, DbObjects.Recipes.UserId)).OnTable(DbObjects.Recipes.TableName).OnColumn(DbObjects.Recipes.UserId);
            Create.Index(DbObjects.Naming.IX(DbObjects.Ingredients.TableName, DbObjects.Ingredients.RecipeId)).OnTable(DbObjects.Ingredients.TableName).OnColumn(DbObjects.Ingredients.RecipeId);
            Create.Index(DbObjects.Naming.IX(DbObjects.Instructions.TableName, DbObjects.Instructions.RecipeId)).OnTable(DbObjects.Instructions.TableName).OnColumn(DbObjects.Instructions.RecipeId);
        }
    }
}
