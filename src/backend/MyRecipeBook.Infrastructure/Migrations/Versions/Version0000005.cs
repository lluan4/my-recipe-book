using FluentMigrator;
using MyRecipeBook.Domain.Entities;

namespace MyRecipeBook.Infrastructure.Migrations.Versions;

[Migration(DatabaseVersions.TABLE_REFRESH_TOKEN, "Add table Refresh token")]
public class Version0000005:VersionBase
{
	public override void Up()
	{
		CreateTable(TableName<RefreshToken>())
			.WithColumn(ColumnName<RefreshToken>(rt => rt.Value)).AsString().NotNullable()
			.WithColumn(ColumnName<Recipe>(rt => rt.UserId)).AsInt64().NotNullable()
				.ForeignKey(
					BuildForeignKeyName<RefreshToken, User>(),
					TableName<User>(),
					ColumnName<User>(u => u.Id)
				);
	}

}

