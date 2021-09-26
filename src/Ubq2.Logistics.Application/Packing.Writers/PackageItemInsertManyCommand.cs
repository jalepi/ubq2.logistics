using System;
using System.Collections.Generic;

namespace Ubq2.Logistics.Packing.Writers
{
    public record PackageItemInsertManyCommand(string SiteId, string PackageId, DateTimeOffset CreatedTime, IReadOnlyCollection<string> ItemIds);
}