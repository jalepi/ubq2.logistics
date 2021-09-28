using System.Collections.Generic;
using Ubq2.Logistics.Packing.Writers;

namespace Ubq2.Logistics.Packing.Providers
{
    public interface IProductIdentifierProvider
    {
        string GetProductId(string itemId);
        IReadOnlyCollection<ProductIncrement> GetProductIds(IReadOnlyCollection<string> itemIds);
    }
}
