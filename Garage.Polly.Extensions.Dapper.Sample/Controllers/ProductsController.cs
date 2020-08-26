using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Garage.Polly.Extensions.Dapper.Sample.Entities;
using Garage.Polly.Extensions.Dapper.Sample.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Garage.Polly.Extensions.Dapper.Sample.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _productRepository;

        public ProductsController(IProductRepository productRepository)
        {
            _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
        }

        [HttpGet]
        public Task<IEnumerable<ProductEntity>> Get(CancellationToken cancellationToken = default)
        {
            return _productRepository.GetAsync(cancellationToken);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id, CancellationToken cancellationToken = default)
        {
            var product = await _productRepository.GetAsync(id, cancellationToken);
            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        [HttpPost]
        public async Task<IActionResult> Post(ProductEntity entity, CancellationToken cancellationToken = default)
        {
            var id = await _productRepository.CreateAsync(entity, cancellationToken);
            entity.Id = id;
            return CreatedAtAction(nameof(Get), new { id }, entity);
        }
    }
}
