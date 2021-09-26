using MediatR;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
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

            var packageItems = queryResult.Select(static q => new PackageItem(
                SiteId: q.SiteId,
                PackageId: q.PackageId,
                ItemId: q.ItemId,
                CreatedTime: q.CreatedTime)).ToList();

            return Result.Create(packageItems, GetPackageItemsQueryStatus.Ok);
        }
    }
}
