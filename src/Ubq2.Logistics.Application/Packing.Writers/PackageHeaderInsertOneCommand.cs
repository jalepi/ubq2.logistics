using System;

namespace Ubq2.Logistics.Packing.Writers
{
    public record PackageHeaderInsertOneCommand(string SiteId, string PackageId, DateTimeOffset CreatedTime)
    {
    }
}