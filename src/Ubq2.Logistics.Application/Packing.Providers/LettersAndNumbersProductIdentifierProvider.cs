using System;
using System.Collections.Generic;
using System.Linq;
using Ubq2.Logistics.Packing.Writers;

namespace Ubq2.Logistics.Packing.Providers
{
    public record LettersAndNumbersProductIdentifierProvider(
        int ProductIdLength,
        string InvalidProductId) 
        : IProductIdentifierProvider
    {
        public string GetProductId(string itemId)
        {
            if (false == String.IsNullOrEmpty(itemId) && itemId.Length >= ProductIdLength)
            {
                return itemId.Substring(0, ProductIdLength);
            }
            else
            {
                return InvalidProductId;
            }
        }

        public IReadOnlyCollection<ProductIncrement> GetProductIds(IReadOnlyCollection<string> itemIds)
        {
            return (
                from itemId in itemIds
                let productId = GetProductId(itemId)
                group itemId by productId into @group
                let productIcrement = new ProductIncrement(
                    ProductId: @group.Key,
                    Count: @group.Count())
                select productIcrement
            ).ToArray();
        }
    }
}
