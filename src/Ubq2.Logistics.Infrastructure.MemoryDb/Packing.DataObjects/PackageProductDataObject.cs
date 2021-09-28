using System;

namespace Ubq2.Logistics.Packing.DataObjects
{
    public record PackageProductDataObject(
        string SiteId,
        string PackageId,
        string ProductId,
        int Count,
         DateTimeOffset UpdatedTime)
        : IPackageProductDataObject
    {
        public string PartitionKey => SiteId;
        public string PrimaryKey => $"{PackageId}, {ProductId}";
    }
}
