using System.Threading;
using System.Threading.Tasks;

namespace Ubq2.Logistics.Packing.Writers
{
    public interface IWriter
    {
        Task Write(PackageHeaderInsertOneCommand commandObject, CancellationToken cancellationToken);
        Task Write(PackageHeaderUpdateOneCommand commandObject, CancellationToken cancellationToken);
        Task Write(PackageItemInsertManyCommand commandObject, CancellationToken cancellationToken);
        Task Write(PackageItemDeleteManyCommand commandObject, CancellationToken cancellationToken);
    }
}
