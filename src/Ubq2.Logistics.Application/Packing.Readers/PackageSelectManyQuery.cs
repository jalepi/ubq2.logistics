using Ubq2.Logistics.Packing.Entities;

namespace Ubq2.Logistics.Packing.Readers
{
    public record PackageSelectManyQuery(string SiteId, PackageStatus PackageStatus)
    {
    }
}