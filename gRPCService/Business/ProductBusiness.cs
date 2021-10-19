using gRPCService.Data;
using gRPCService.Models;
using gRPCService.Protos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace gRPCService.Business
{
    public class ProductBusiness
    {
        private readonly AppDbContext _context;

        public ProductBusiness(AppDbContext context)
        {
            _context = context;
        }

        public List<ProductData> GetProducts()
        {
            var products = _context.Products
                .Select(p => new ProductData()
                {
                    Id = p.Id.ToString(),
                    Name = p.Name,
                    Price = p.Price
                })
                .ToList();

            return products;
        }

        public bool Add(ProductData productData, out StringBuilder inconsistencies)
        {
            inconsistencies = ValidateProduct(productData);
            if (inconsistencies.Length == 0)
            {
                var product = _context.Products.FirstOrDefault(p => p.Id == Guid.Parse(productData.Id));

                if (product == null)
                {
                    product = new Product
                    {
                        Id = Guid.Parse(productData.Id),
                        Name = productData.Name,
                        Price = productData.Price
                    };

                    _context.Products.Add(product);
                    _context.SaveChanges();
                }
                else
                {
                    inconsistencies.Append("Id already exists |");
                }
            }

            if (inconsistencies.Length > 0)
                inconsistencies.Insert(0, "| ");

            return inconsistencies.Length == 0;
        }

        public bool Update(ProductData productData, out StringBuilder inconsistencies)
        {
            inconsistencies = ValidateProduct(productData);
            if (inconsistencies.Length == 0)
            {
                var product = _context.Products.FirstOrDefault(p => p.Id == Guid.Parse(productData.Id));

                if (product == null)
                {
                    inconsistencies.Append("Product not found |");
                }
                else
                {
                    product.Name = productData.Name;
                    product.Price = productData.Price;
                    _context.SaveChanges();
                }
            }

            if (inconsistencies.Length > 0)
                inconsistencies.Insert(0, "| ");

            return inconsistencies.Length == 0;
        }

        private static StringBuilder ValidateProduct(ProductData product)
        {
            var result = new StringBuilder();
            if (product == null)
            {
                result.Append(" Inform the product data |");
            }
            else
            {
                ValidateId(product, result);
                ValidateName(product, result);
                ValidatePrice(product, result);
            }

            return result;
        }

        private static void ValidateId(ProductData product, StringBuilder result)
        {
            if (string.IsNullOrWhiteSpace(product.Id))
            {
                result.Append(" Inform the product Id |");
            }
        }

        private static void ValidateName(ProductData product, StringBuilder result)
        {
            if (string.IsNullOrWhiteSpace(product.Name))
            {
                result.Append(" Inform the product Name |");
            }
        }

        private static void ValidatePrice(ProductData product, StringBuilder result)
        {
            if (product.Price <= 0)
            {
                result.Append(" The product price must be greater than zero |");
            }
        }
    }
}
