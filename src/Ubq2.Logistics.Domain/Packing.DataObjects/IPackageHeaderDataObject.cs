using System;

namespace Ubq2.Logistics.Packing.DataObjects
{
    public interface IPackageHeaderDataObject : IDataObject
    {
        string SiteId { get; }
        string PackageId { get; }
        DateTimeOffset CreatedTime { get; }
        DateTimeOffset UpdatedTime { get; }
        PackageStatusDataObject PackageStatus { get; }
    }
}
