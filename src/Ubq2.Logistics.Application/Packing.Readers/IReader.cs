using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Ubq2.Logistics.Packing.DataObjects;

namespace Ubq2.Logistics.Packing.Readers
{
    public interface IReader
    {
        Task<IReadOnlyCollection<IPackageHeaderDataObject>> Read(PackageHeaderSelectManyQuery queryObject, CancellationToken cancellationToken);
        Task<IReadOnlyCollection<IPackageItemDataObject>> Read(PackageItemSelectManyQuery queryObject, CancellationToken cancellationToken);
    }
}
