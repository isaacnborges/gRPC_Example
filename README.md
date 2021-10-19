# gRPC_Example

This example contains two gRPC projects developed with [.NET 5](https://dotnet.microsoft.com/download/dotnet/5.0)


### gRPCService
Simple project that provide three actions using [UseInMemoryDatabase](https://docs.microsoft.com/en-us/dotnet/api/microsoft.entityframeworkcore.inmemorydbcontextoptionsextensions.useinmemorydatabase?view=efcore-5.0)
- GetProducts
- AddProduct
- UpdateProduct

### gRPCClient
Simple Api project with ProductsController that connect on server using gRPC and response the correct model.