using System;

namespace Ubq2.Logistics.Packing.DataObjects
{
    public record PackageHeaderDataObject(
        string SiteId,
        string PackageId,
        PackageStatusDataObject PackageStatus,
        DateTimeOffset CreatedTime,
        DateTimeOffset UpdatedTime)
        : IPackageHeaderDataObject
    {
        public string PartitionKey => SiteId;
        public string PrimaryKey => PackageId;
    }
}
