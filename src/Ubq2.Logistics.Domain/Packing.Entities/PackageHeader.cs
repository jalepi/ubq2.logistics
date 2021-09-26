using System;

namespace Ubq2.Logistics.Packing.Entities
{
    public record PackageHeader(string SiteId, string PackageId, PackageStatus PackageStatus, DateTimeOffset CreatedTime, DateTimeOffset UpdatedTime)
    {
    }
}
