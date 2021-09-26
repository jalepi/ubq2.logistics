using System;

namespace Ubq2.Logistics.Packing.Entities
{
    public record PackageItem(string SiteId, string PackageId, string ItemId, DateTimeOffset CreatedTime)
    {
    }
}
