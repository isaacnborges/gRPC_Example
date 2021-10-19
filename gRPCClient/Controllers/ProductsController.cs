using Grpc.Net.Client;
using gRPCClient.Dtos;
using gRPCClient.Protos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace gRPCClient.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly ProductSvc.ProductSvcClient _client;

        public ProductsController(IConfiguration configuration)
        {
            var channel = GrpcChannel.ForAddress(configuration["gRPCClient"]);
            _client = new ProductSvc.ProductSvcClient(channel);
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var products = new List<ProductResponse>();
            using (var call = _client.GetProducts(new GetProductsRequest()))
            {
                var responseStream = call.ResponseStream;

                var cancellationTokenSource = new CancellationTokenSource();
                var token = cancellationTokenSource.Token;

                while (await responseStream.MoveNext(token))
                {
                    var productData = responseStream.Current.Product;
                    products.Add(new ProductResponse(Guid.Parse(productData.Id), productData.Name, productData.Price));
                }
            }

            return Ok(products);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ProductCommand productDto)
        {
            var result = await _client.AddProductAsync(
                new ProductData()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = productDto.Name,
                    Price = productDto.Price
                });

            return ResponseAction(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromRoute] Guid id, [FromBody] ProductCommand productDto)
        {
            var result = await _client.UpdateProductAsync(
                new ProductData()
                {
                    Id = id.ToString(),
                    Name = productDto.Name,
                    Price = productDto.Price
                });

            return ResponseAction(result);
        }

        private IActionResult ResponseAction(ProductReply result)
        {
            if (result.Success)
            {
                return Ok();
            }

            return BadRequest(new
            {
                result.Message,
                result.Inconsistencies
            });
        }
    }
}
