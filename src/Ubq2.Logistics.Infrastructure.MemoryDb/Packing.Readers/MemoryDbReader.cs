using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Ubq2.Logistics.Packing.Entities;

namespace Ubq2.Logistics.Packing.Readers
{
    public record MemoryDbReader(
        ConcurrentDictionary<string, PackageHeader> PackageHeaders,
        ConcurrentDictionary<string, PackageItem> PackageItems)
        : IReader
    {
        public async Task<IReadOnlyCollection<PackageHeader>> Read(PackageSelectManyQuery queryObject, CancellationToken cancellationToken)
        {
            await Task.Yield();

            return PackageHeaders.Values.Where(o => o.SiteId == queryObject.SiteId).ToArray();
        }

        public async Task<IReadOnlyCollection<PackageItem>> Read(PackageItemSelectManyQuery queryObject, CancellationToken cancellationToken)
        {
            await Task.Yield();

            return PackageItems.Values.Where(o => o.SiteId == queryObject.SiteId && o.PackageId == queryObject.PackageId).ToArray();
        }
    }
}
