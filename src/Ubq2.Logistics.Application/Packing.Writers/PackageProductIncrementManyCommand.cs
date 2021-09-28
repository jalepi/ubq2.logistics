using System;
using System.Collections.Generic;

namespace Ubq2.Logistics.Packing.Writers
{
    public record PackageProductIncrementManyCommand(string SiteId, string PackageId, DateTimeOffset UpdatedTime, IReadOnlyCollection<ProductIncrement> ProductIncrements);
    public record ProductIncrement(string ProductId, int Count);
}
