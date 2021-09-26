using System;

namespace Ubq2.Logistics.Packing.DataObjects
{
    public interface IPackageItemDataObject : IDataObject
    {
        string SiteId { get; }
        string PackageId { get; }
        string ItemId { get; }
        DateTimeOffset CreatedTime { get; }
    }
}
