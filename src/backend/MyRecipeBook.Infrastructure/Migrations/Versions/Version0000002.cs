using FluentMigrator;
using MyRecipeBook.Domain.Entities;
using System.Data;


namespace MyRecipeBook.Infrastructure.Migrations.Versions;

[Migration(DatabaseVersions.TABLE_RECIPES, "Create table to save the recipes' information")]
public class Version0000002 : VersionBase
{
    public override void Up()
    {
        /*--------------- Tabelas de referência ---------------*/
        CreateReferenceTable(TableName<CookingTime>());
        Insert.IntoTable(TableName<CookingTime>())
              .Row(new { Description = "< 10 mins" })
              .Row(new { Description = "10-30 mins" })
              .Row(new { Description = "30-60 mins" })
              .Row(new { Description = "> 60 mins" });

        CreateReferenceTable(TableName<Difficulty>());
        Insert.IntoTable(TableName<Difficulty>())
              .Row(new { Description = "Low" })
              .Row(new { Description = "Medium" })
              .Row(new { Description = "High" });

        CreateReferenceTable(TableName<DishType>());
        Insert.IntoTable(TableName<DishType>())
              .Row(new { Description = "Breakfast" })
              .Row(new { Description = "Lunch" })
              .Row(new { Description = "Appetizers" })
              .Row(new { Description = "Snack" })
              .Row(new { Description = "Dessert" })
              .Row(new { Description = "Dinner" })
              .Row(new { Description = "Drinks" });

        /*--------------- Tabela principal ---------------*/
        CreateTable(TableName<Recipe>())
            .WithColumn(ColumnName<Recipe>(r => r.Title)).AsString(200).NotNullable()

            .WithColumn(ColumnName<Recipe>(r => r.CookingTimeId)).AsInt32().Nullable()
                .ForeignKey(
                    BuildForeignKeyName<Recipe, CookingTime>(),
                    TableName<CookingTime>(),
                    ColumnName<CookingTime>(c => c.Id))

            .WithColumn(ColumnName<Recipe>(r => r.DifficultyId)).AsInt32().Nullable()
                .ForeignKey(
                    BuildForeignKeyName<Recipe, Difficulty>(),
                    TableName<Difficulty>(),
                    ColumnName<Difficulty>(d => d.Id))

            .WithColumn(ColumnName<Recipe>(r => r.UserId)).AsInt64().NotNullable()
                .ForeignKey(
                    BuildForeignKeyName<Recipe, User>(),
                    TableName<User>(),
                    ColumnName<User>(u => u.Id));

        /*--------------- Dependentes ---------------*/
        CreateTable(TableName<Ingredient>())
            .WithColumn(ColumnName<Ingredient>(i => i.Item)).AsString(100).NotNullable()
            .WithColumn(ColumnName<Ingredient>(i => i.RecipeId)).AsInt64().NotNullable()
                .ForeignKey(
                    BuildForeignKeyName<Ingredient, Recipe>(),
                    TableName<Recipe>(),
                    ColumnName<Recipe>(r => r.Id))
                .OnDelete(Rule.Cascade);

        CreateTable(TableName<Instruction>())
            .WithColumn(ColumnName<Instruction>(i => i.Step)).AsInt32().NotNullable()
            .WithColumn(ColumnName<Instruction>(i => i.Description)).AsString(2000).NotNullable()
            .WithColumn(ColumnName<Instruction>(i => i.RecipeId)).AsInt64().NotNullable()
                .ForeignKey(
                    BuildForeignKeyName<Instruction, Recipe>(),
                    TableName<Recipe>(),
                    ColumnName<Recipe>(r => r.Id))
                .OnDelete(Rule.Cascade);

        /*--------------- Junction ---------------*/
        CreateJunctionTable(TableName<RecipeDishType>())
            .WithColumn(ColumnName<RecipeDishType>(r => r.RecipeId)).AsInt64().NotNullable()
                .ForeignKey(
                    BuildForeignKeyName<RecipeDishType, Recipe>(),
                    TableName<Recipe>(),
                    ColumnName<Recipe>(r => r.Id))
                .OnDelete(Rule.Cascade)

            .WithColumn(ColumnName<RecipeDishType>(r => r.DishTypeId)).AsInt32().NotNullable()
                .ForeignKey(
                    BuildForeignKeyName<RecipeDishType, DishType>(),
                    TableName<DishType>(),
                    ColumnName<DishType>(d => d.Id));

        Create.PrimaryKey(BuildPrimaryKeyName<RecipeDishType>())
              .OnTable(TableName<RecipeDishType>())
              .Columns(
                  ColumnName<RecipeDishType>(r => r.RecipeId),
                  ColumnName<RecipeDishType>(r => r.DishTypeId));

        Create.UniqueConstraint(
            BuildUniqueConstraintName<Instruction>(
                ColumnName<Instruction>(i => i.RecipeId),
                ColumnName<Instruction>(i => i.Step))
            )
              .OnTable(TableName<Instruction>())
              .Columns(
                  ColumnName<Instruction>(i => i.RecipeId),
                  ColumnName<Instruction>(i => i.Step));

        /*--------------- Índices ---------------*/
        Create.Index(
            BuildIndexName<Recipe>(
                ColumnName<Recipe>(r => r.UserId))
            )
              .OnTable(TableName<Recipe>())
              .OnColumn(ColumnName<Recipe>(r => r.UserId));

        Create.Index(
            BuildIndexName<Ingredient>(
                ColumnName<Ingredient>(r => r.RecipeId))
            )
              .OnTable(TableName<Ingredient>())
              .OnColumn(ColumnName<Ingredient>(i => i.RecipeId));

        Create.Index(
            BuildIndexName<Instruction>(
                ColumnName<Instruction>(r => r.RecipeId))
            )
              .OnTable(TableName<Instruction>())
              .OnColumn(ColumnName<Instruction>(i => i.RecipeId));
    }
}
