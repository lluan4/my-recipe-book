using FluentMigrator;
using FluentMigrator.Builders.Create.Table;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq.Expressions;
using System.Reflection;

namespace MyRecipeBook.Infrastructure.Migrations.Versions
{
    public abstract class VersionBase : ForwardOnlyMigration
    {
        protected ICreateTableColumnOptionOrWithColumnSyntax CreateTable(string table)
        {
            return Create.Table(table)
                .WithColumn("Id").AsInt64().PrimaryKey().Identity()
                .WithColumn("CreatedOn").AsDateTime().NotNullable()
                .WithColumn("Active").AsBoolean().NotNullable();
        }

        protected ICreateTableColumnOptionOrWithColumnSyntax CreateReferenceTable(string table)
        {
            return Create.Table(table)
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("Description").AsString().NotNullable();
        }

        protected ICreateTableWithColumnSyntax CreateJunctionTable(string table)
        {
            return Create.Table(table)
                .WithColumn("CreatedOn").AsDateTime().NotNullable().WithDefault(SystemMethods.CurrentDateTime);
        }

        protected static string TableName<T>()
        {
            return typeof(T).GetCustomAttribute<TableAttribute>()?.Name ?? typeof(T).Name;
        }
        protected static string ColumnNameByText<T>(string prop) =>
            typeof(T).GetProperty(prop)?
            .GetCustomAttribute<ColumnAttribute>()?
            .Name?.ToLower() ?? prop.ToLower();


        protected static string ColumnName<T>(Expression<Func<T, object?>> selector)
        {
            static MemberExpression GetMember(Expression expr) => expr switch
            {
                MemberExpression m => m,
                UnaryExpression { Operand: var op } => GetMember(op),
                _ => throw new ArgumentException(
                         "Selector deve ser uma property.", nameof(selector))
            };

            var member = GetMember(selector.Body);
            return ColumnNameByText<T>(member.Member.Name);
        }

        protected static string BuildForeignKeyName<TFrom, TTo>()
        {
            return $"FK_{TableName<TFrom>()}_{TableName<TTo>()}";
        }

        protected static string BuildPrimaryKeyName<TName>()
        {
            return $"PK_{TableName<TName>()}";
        }

        protected static string BuildUniqueConstraintName<TName>(params string[] columnsName)
        {
            if (columnsName is null || columnsName.Length == 0)
                throw new ArgumentException("Informe ao menos uma coluna.", nameof(columnsName));

            var cols = string.Join("_", columnsName);
            return $"UC_{TableName<TName>()}_{cols}";
        }
        protected static string BuildIndexName<TName>(string columnName)
        {

            return $"IX_{TableName<TName>()}_{columnName}";
        }

    }
}
