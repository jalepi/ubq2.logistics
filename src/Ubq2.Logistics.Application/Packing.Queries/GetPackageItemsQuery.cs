using MediatR;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Ubq2.Logistics.Common.Entities;
using Ubq2.Logistics.Packing.Entities;
using Ubq2.Logistics.Packing.Readers;

namespace Ubq2.Logistics.Packing.Queries
{
    public record GetPackageItemsQuery(string SiteId, string PackageId)
        : IRequest<IResult<IReadOnlyCollection<PackageItem>, GetPackageItemsQueryStatus>>
    {
    }

    public enum GetPackageItemsQueryStatus { Ok }

    public record GetPackageItemsQueryHandler(
        ILogger<GetPackageItemsQueryHandler> Logger,
        IReader Reader)
        : IRequestHandler<GetPackageItemsQuery, IResult<IReadOnlyCollection<PackageItem>, GetPackageItemsQueryStatus>>
    {
        public async Task<IResult<IReadOnlyCollection<PackageItem>, GetPackageItemsQueryStatus>> Handle(GetPackageItemsQuery request, CancellationToken cancellationToken)
        {
            var queryObject = new PackageItemSelectManyQuery(
                SiteId: request.SiteId,
                PackageId: request.PackageId);

            var queryResult = await Reader.Read(queryObject, cancellationToken);

            return Result.Create(queryResult, GetPackageItemsQueryStatus.Ok);
        }
    }
}
