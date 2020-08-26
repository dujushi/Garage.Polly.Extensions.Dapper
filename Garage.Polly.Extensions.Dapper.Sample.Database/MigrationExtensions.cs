using FluentMigrator.Builders.Create.Table;

namespace Garage.Polly.Extensions.Dapper.Sample.Database
{
    internal static class MigrationExtensions
    {
        public static ICreateTableColumnOptionOrWithColumnSyntax WithIdColumn(this ICreateTableWithColumnSyntax tableWithColumnSyntax)
        {
            return tableWithColumnSyntax
                .WithColumn("Id")
                .AsInt32()
                .NotNullable()
                .PrimaryKey()
                .Identity();
        }

        public static ICreateTableColumnOptionOrWithColumnSyntax WithTimeStamps(this ICreateTableWithColumnSyntax tableWithColumnSyntax)
        {
            return tableWithColumnSyntax
                .WithColumn("WhenCreated").AsDateTime().NotNullable()
                .WithColumn("WhenUpdated").AsDateTime().NotNullable();
        }
    }
}
