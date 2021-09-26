using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Ubq2.Logistics.Packing.Entities;

namespace Ubq2.Logistics.Packing.Readers
{
    public interface IReader
    {
        Task<IReadOnlyCollection<PackageHeader>> Read(PackageSelectManyQuery queryObject, CancellationToken cancellationToken);
    }
}
