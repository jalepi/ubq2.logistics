using System;

namespace Ubq2.Logistics.Packing.Writers
{
    public record PackageHeaderUpdateOneCommand(string SiteId, string PackageId, DateTimeOffset UpdatedTime)
    {
    }
}