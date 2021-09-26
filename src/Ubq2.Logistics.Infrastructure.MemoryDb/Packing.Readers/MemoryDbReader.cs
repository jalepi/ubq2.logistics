using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ubq2.Logistics.Packing.DataObjects;

namespace Ubq2.Logistics.Packing.Readers
{
    public record MemoryDbReader(
        ConcurrentDictionary<string, PackageHeaderDataObject> PackageHeaders,
        ConcurrentDictionary<string, PackageItemDataObject> PackageItems)
        : IReader
    {
        public async Task<IReadOnlyCollection<IPackageHeaderDataObject>> Read(PackageHeaderSelectManyQuery queryObject, CancellationToken cancellationToken)
        {
            await Task.Yield();

            return PackageHeaders.Values.Where(o => o.SiteId == queryObject.SiteId).ToArray();
        }

        public async Task<IReadOnlyCollection<IPackageItemDataObject>> Read(PackageItemSelectManyQuery queryObject, CancellationToken cancellationToken)
        {
            await Task.Yield();

            return PackageItems.Values.Where(o => o.SiteId == queryObject.SiteId && o.PackageId == queryObject.PackageId).ToArray();
        }
    }
}
