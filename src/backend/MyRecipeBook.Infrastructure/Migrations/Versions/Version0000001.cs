using FluentMigrator;
using MyRecipeBook.Domain.Entities;

namespace MyRecipeBook.Infrastructure.Migrations.Versions
{
    [Migration(DatabaseVersions.TABLE_USER, "Create table to save the user's information")]
    public class Version0000001 : VersionBase
    {
        public override void Up()
        {
            CreateTable(TableName<User>())
                 .WithColumn(ColumnName<User>(r => r.Name)).AsString(255).NotNullable()
                 .WithColumn(ColumnName<User>(r => r.Email)).AsString(255).NotNullable()
                 .WithColumn(ColumnName<User>(r => r.Password)).AsString(2000).NotNullable()
                 .WithColumn(ColumnName<User>(r => r.UserIdentifier)).AsGuid().NotNullable();
        }
    }
}
