using System.Collections.Generic;

namespace Ubq2.Logistics.Packing.Writers
{
    public record PackageItemDeleteManyCommand(string SiteId, string PackageId, IReadOnlyCollection<string> ItemIds);
}