using System.Threading;
using System.Threading.Tasks;

namespace Ubq2.Logistics.Packing.Writers
{
    public interface IWriter
    {
        Task Write(PackageInsertOneCommand commandObject, CancellationToken cancellationToken);
        Task Write(PackageUpdateOneCommand commandObject, CancellationToken cancellationToken);
    }
}
