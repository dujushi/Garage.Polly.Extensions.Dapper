using FluentMigrator;

namespace Garage.Polly.Extensions.Dapper.Sample.Database.Migrations._2020._08
{
    [Migration(202008251541)]
    public class M202008251541CreateProductTable : Migration
    {
        private const string ProductTableName = "Product";
        public override void Up()
        {
            Create.Table(ProductTableName)
                .WithIdColumn()
                .WithTimeStamps()
                .WithColumn("Name").AsString(100).Nullable();
        }

        public override void Down()
        {
            Delete.Table(ProductTableName);
        }
    }
}
