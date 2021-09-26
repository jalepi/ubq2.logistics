using System;

namespace Ubq2.Logistics.Packing.DataObjects
{
    public record PackageItemDataObject(
        string SiteId,
        string PackageId,
        string ItemId,
         DateTimeOffset CreatedTime)
        : IPackageItemDataObject
    {
        public string PartitionKey => SiteId;
        public string PrimaryKey => $"{PackageId}, {ItemId}";
    }
}
