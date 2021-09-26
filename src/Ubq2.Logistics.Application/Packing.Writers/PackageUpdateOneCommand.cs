using System;

namespace Ubq2.Logistics.Packing.Writers
{
    public record PackageUpdateOneCommand(string SiteId, string PackageId, DateTimeOffset UpdatedTime)
    {
    }
}