using System;

namespace Ubq2.Logistics.Packing.DataObjects
{
    public interface IPackageProductDataObject : IDataObject
    {
        string SiteId { get; }
        string PackageId { get; }
        string ProductId { get; }
        int Count { get; }
        DateTimeOffset UpdatedTime { get; }
    }
}
