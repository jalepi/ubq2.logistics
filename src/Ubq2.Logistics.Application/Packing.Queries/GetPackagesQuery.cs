using MediatR;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ubq2.Logistics.Common.Entities;
using Ubq2.Logistics.Packing.DataObjects;
using Ubq2.Logistics.Packing.Entities;
using Ubq2.Logistics.Packing.Readers;

namespace Ubq2.Logistics.Packing.Queries
{
    public record GetPackagesQuery(string SiteId, Entities.PackageStatus PackageStatus)
        : IRequest<IResult<IReadOnlyCollection<PackageHeader>, GetPackagesQueryStatus>>
    {
    }

    public enum GetPackagesQueryStatus { Ok }

    public record GetPackagesQueryHandler(
        ILogger<GetPackagesQueryHandler> Logger,
        IReader Reader)
        : IRequestHandler<GetPackagesQuery, IResult<IReadOnlyCollection<PackageHeader>, GetPackagesQueryStatus>>
    {
        public async Task<IResult<IReadOnlyCollection<PackageHeader>, GetPackagesQueryStatus>> Handle(GetPackagesQuery request, CancellationToken cancellationToken)
        {
            var queryObject = new PackageHeaderSelectManyQuery(
                SiteId: request.SiteId,
                PackageStatus: request.PackageStatus);

            var queryResult = await Reader.Read(queryObject, cancellationToken);

            var packageHeaders = queryResult.Select(Convert).ToList();

            return Result.Create(Value: packageHeaders, Status: GetPackagesQueryStatus.Ok);
        }

        private static PackageHeader Convert(IPackageHeaderDataObject q)
        {
            return new PackageHeader(
                SiteId: q.SiteId,
                PackageId: q.PackageId,
                PackageStatus: q.PackageStatus switch
                {
                    DataObjects.PackageStatusDataObject.Open => Entities.PackageStatus.Open,
                    _ => Entities.PackageStatus.Open,
                },
                CreatedTime: q.CreatedTime,
                UpdatedTime: q.UpdatedTime);
        }
    }
}
