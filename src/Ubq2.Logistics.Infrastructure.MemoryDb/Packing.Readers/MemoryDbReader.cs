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
    public record MemoryDbReader(ConcurrentDictionary<string, PackageHeader> PackageHeaders)
        : IReader
    {
        public async Task<IReadOnlyCollection<PackageHeader>> Read(PackageSelectManyQuery queryObject, CancellationToken cancellationToken)
        {
            await Task.Yield();

            return PackageHeaders.Values.Where(o => o.SiteId == queryObject.SiteId).ToArray();
        }
    }
}
