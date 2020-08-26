using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Garage.Polly.Extensions.Dapper.Sample.Entities;
using Microsoft.Data.SqlClient;

namespace Garage.Polly.Extensions.Dapper.Sample.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly IConnectionStringProvider _connectionStringProvider;

        public ProductRepository(IConnectionStringProvider connectionStringProvider)
        {
            _connectionStringProvider = connectionStringProvider;
        }

        public async Task<IEnumerable<ProductEntity>> GetAsync(CancellationToken cancellationToken = default)
        {
            const string query = @"
select Id, Name
from Product
";
            await using var connection = new SqlConnection(_connectionStringProvider.Get());
            var products = await connection.QueryAsyncWithRetry<ProductEntity>(query);
            return products;
        }

        public async Task<ProductEntity> GetAsync(int id, CancellationToken cancellationToken = default)
        {
            const string query = @"
select Id, Name
from Product
where Id = @Id
";
            await using var connection = new SqlConnection(_connectionStringProvider.Get());
            var products = await connection.QueryFirstOrDefaultAsyncWithRetry<ProductEntity>(query, new { id });
            return products;
        }

        public async Task<int> CreateAsync(ProductEntity entity, CancellationToken cancellationToken = default)
        {
            const string query = @"
insert into Product
(Name, WhenCreated, WhenUpdated)
values
(@Name, getdate(), getdate());

select cast(scope_identity() as int)
";
            await using var connection = new SqlConnection(_connectionStringProvider.Get());
            var id = await connection.ExecuteScalarAsync<int>(query, new { entity.Name });
            return id;
        }
    }
}
