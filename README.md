# gRPC_Example

This example contains two [gRPC](https://docs.microsoft.com/en-us/aspnet/core/grpc/?view=aspnetcore-5.0) projects developed with [.NET 5](https://dotnet.microsoft.com/download/dotnet/5.0)


## gRPCService
Simple project that provide a server with three actions in `products.proto` and save the products using [UseInMemoryDatabase](https://docs.microsoft.com/en-us/dotnet/api/microsoft.entityframeworkcore.inmemorydbcontextoptionsextensions.useinmemorydatabase?view=efcore-5.0)
- GetProducts
- AddProduct
- UpdateProduct

## gRPCClient
gRPC client and api project with ProductsController that connect on server using gRPC and response the correct model.