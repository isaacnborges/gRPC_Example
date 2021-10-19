using System;

namespace gRPCClient.Dtos
{
    public record ProductResponse(Guid Id, string Name, double Price);
}
