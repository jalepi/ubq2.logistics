using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ubq2.Logistics.Common.Entities;
using Ubq2.Logistics.Packing.Providers;
using Ubq2.Logistics.Packing.Writers;

namespace Ubq2.Logistics.Packing.Commands
{
    public record RemovePackageItemsCommand(string SiteId, string PackageId, DateTimeOffset RequestTime, IReadOnlyCollection<string> ItemIds)
        : IRequest<VoidResult<RemovePackageItemsCommandStatus>>;

    public enum RemovePackageItemsCommandStatus { Ok }

    public record RemovePackageItemsCommandHandler(
        ILogger<RemovePackageItemsCommandHandler> Logger,
        IWriter Writer,
        IProductIdentifierProvider ProductIdentifierProvider)
        : IRequestHandler<RemovePackageItemsCommand, VoidResult<RemovePackageItemsCommandStatus>>
    {
        public async Task<VoidResult<RemovePackageItemsCommandStatus>> Handle(RemovePackageItemsCommand request, CancellationToken cancellationToken)
        {
            var productIncrements = (
                from pi in ProductIdentifierProvider.GetProductIds(request.ItemIds)
                select pi with { Count = -pi.Count }
            ).ToArray();

            var packageProductIncrementMany = new PackageProductIncrementManyCommand(
                SiteId: request.SiteId,
                PackageId: request.PackageId,
                UpdatedTime: request.RequestTime,
                ProductIncrements: productIncrements);

            await Writer.Write(packageProductIncrementMany, cancellationToken);

            var packageItemDeleteMany = new PackageItemDeleteManyCommand(
                SiteId: request.SiteId,
                PackageId: request.PackageId,
                ItemIds: request.ItemIds);

            await Writer.Write(packageItemDeleteMany, cancellationToken);

            return VoidResult.Create(RemovePackageItemsCommandStatus.Ok);
        }
    }
}
