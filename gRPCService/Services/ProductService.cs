using Grpc.Core;
using gRPCService.Business;
using gRPCService.Protos;
using Microsoft.Extensions.Logging;
using System.Text;
using System.Threading.Tasks;

namespace gRPCService.Services
{
    public class ProductService : ProductSvc.ProductSvcBase
    {
        private readonly ILogger<ProductService> _logger;
        private readonly ProductBusiness _productBusiness;

        public ProductService(ILogger<ProductService> logger, ProductBusiness productBusiness)
        {
            _logger = logger;
            _productBusiness = productBusiness;
        }

        public override async Task GetProducts(GetProductsRequest request, IServerStreamWriter<GetProductsReply> responseStream, ServerCallContext context)
        {
            _logger.LogInformation("Get Products...");
            var products = _productBusiness.GetProducts();

            foreach (var product in products)
            {
                await responseStream.WriteAsync(new GetProductsReply { Product = product });
            }
        }

        public override Task<ProductReply> AddProduct(ProductData request, ServerCallContext context)
        {
            _logger.LogInformation("Add Product...");

            var success = _productBusiness.Add(request, out StringBuilder inconsistencies);
            return Task.FromResult(new ProductReply
            {
                Success = success,
                Message = success ? "Product added with success" : "Inconsistent datas",
                Inconsistencies = inconsistencies.ToString()
            });
        }

        public override Task<ProductReply> UpdateProduct(ProductData request, ServerCallContext context)
        {
            _logger.LogInformation("Update Product...");
            var success = _productBusiness.Update(request, out StringBuilder inconsistencies);

            return Task.FromResult(new ProductReply
            {
                Success = success,
                Message = success ? "Product updated with success" : "Inconsistent datas",
                Inconsistencies = inconsistencies.ToString()
            });
        }
    }
}
