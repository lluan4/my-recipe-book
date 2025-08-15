using FluentMigrator;
using MyRecipeBook.Domain.Entities;

namespace MyRecipeBook.Infrastructure.Migrations.Versions;

[Migration(DatabaseVersions.IMAGES_FOR_RECIPES, "Add column on recipe table to save images")]
public class Version0000004:VersionBase
{
	public override void Up()
	{
		Alter.Table(TableName<Recipe>())
			.AddColumn(ColumnName<Recipe>(r => r.ImageIdentifier)).AsString().Nullable();
	}

}

