using System;

namespace Ubq2.Logistics.Packing.Writers
{
    public record PackageInsertOneCommand(string SiteId, string PackageId, DateTimeOffset CreatedTime)
    {
    }
}