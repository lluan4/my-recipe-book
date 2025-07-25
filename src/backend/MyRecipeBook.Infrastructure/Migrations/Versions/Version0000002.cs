using FluentMigrator;

namespace MyRecipeBook.Infrastructure.Migrations.Versions
{
    [Migration(DatabaseVersions.TABLE_RECIPES, "Create table to save the recipes' information")]
    public class Version0000002 : VersionBase
    {
        public override void Up()
        {
            // Tabelas de referência
            CreateReferenceTable("CookingTime");
            Insert.IntoTable("CookingTime")
                .Row(new { Description = "< 10 mins" })
                .Row(new { Description = "10-30 mins" })
                .Row(new { Description = "30-60 mins" })
                .Row(new { Description = "> 60 mins" });

            CreateReferenceTable("Difficulty");
            Insert.IntoTable("Difficulty")
                .Row(new { Description = "Low" })
                .Row(new { Description = "Medium" })
                .Row(new { Description = "High" });

            CreateReferenceTable("DishTypes");
            Insert.IntoTable("DishTypes")
                .Row(new { Description = "Breakfast" })
                .Row(new { Description = "Lunch" })
                .Row(new { Description = "Appetizers" })
                .Row(new { Description = "Snack" })
                .Row(new { Description = "Dessert" })
                .Row(new { Description = "Dinner" })
                .Row(new { Description = "Drinks" });

            // Tabela principal
            CreateTable("Recipes")
                .WithColumn("Title").AsString(200).NotNullable()
                .WithColumn("CookingTimeId").AsInt32().Nullable()
                    .ForeignKey("FK_Recipe_CookingTime_Id", "CookingTime", "Id")
                .WithColumn("DifficultyId").AsInt32().Nullable()
                    .ForeignKey("FK_Recipe_Difficulty_Id", "Difficulty", "Id")
                .WithColumn("UserId").AsInt64().NotNullable()
                    .ForeignKey("FK_Recipe_User_Id", "Users", "Id");

            // Tabelas dependentes
            CreateTable("Ingredients")
                .WithColumn("Item").AsString(100).NotNullable()
                .WithColumn("RecipeId").AsInt64().NotNullable()
                    .ForeignKey("FK_Ingredient_Recipe_Id", "Recipes", "Id")
                        .OnDelete(System.Data.Rule.Cascade);

            CreateTable("Instructions")
                .WithColumn("Step").AsInt32().NotNullable()
                .WithColumn("Description").AsString(2000).NotNullable()
                .WithColumn("RecipeId").AsInt64().NotNullable()
                    .ForeignKey("FK_Instruction_Recipe_Id", "Recipes", "Id")
                        .OnDelete(System.Data.Rule.Cascade);

            // Tabela de ligação
            CreateJunctionTable("RecipesDishTypes")
                .WithColumn("RecipeId").AsInt64().NotNullable()
                    .ForeignKey("FK_RecipesDishTypes_Recipe_Id", "Recipes", "Id")
                        .OnDelete(System.Data.Rule.Cascade)
                .WithColumn("DishTypeId").AsInt32().NotNullable()
                    .ForeignKey("FK_RecipesDishTypes_DishType_Id", "DishTypes", "Id");

            // Constraints e índices
            Create.PrimaryKey("PK_RecipesDishTypes")
                .OnTable("RecipesDishTypes")
                .Columns("RecipeId", "DishTypeId");

            Create.UniqueConstraint("UC_Instructions_Recipe_Step")
                .OnTable("Instructions")
                .Columns("RecipeId", "Step");

            // Índices para performance
            Create.Index("IX_Recipes_UserId").OnTable("Recipes").OnColumn("UserId");
            Create.Index("IX_Ingredients_RecipeId").OnTable("Ingredients").OnColumn("RecipeId");
            Create.Index("IX_Instructions_RecipeId").OnTable("Instructions").OnColumn("RecipeId");
        }
    }
}
