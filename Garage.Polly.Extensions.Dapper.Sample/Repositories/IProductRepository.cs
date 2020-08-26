using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Garage.Polly.Extensions.Dapper.Sample.Entities;

namespace Garage.Polly.Extensions.Dapper.Sample.Repositories
{
    public interface IProductRepository
    {
        Task<IEnumerable<ProductEntity>> GetAsync(CancellationToken cancellationToken = default);
        Task<ProductEntity> GetAsync(int id, CancellationToken cancellationToken = default);
        Task<int> CreateAsync(ProductEntity entity, CancellationToken cancellationToken = default);
    }
}
