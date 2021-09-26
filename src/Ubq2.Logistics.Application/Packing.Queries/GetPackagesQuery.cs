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
    public record GetPackagesQuery(string SiteId, PackageStatus PackageStatus) 
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
            var queryObject = new PackageSelectManyQuery(
                SiteId: request.SiteId, 
                PackageStatus: request.PackageStatus);

            var queryResult = await Reader.Read(queryObject, cancellationToken);

            return Result.Create(Value: queryResult, Status: GetPackagesQueryStatus.Ok);
        }
    }
}
