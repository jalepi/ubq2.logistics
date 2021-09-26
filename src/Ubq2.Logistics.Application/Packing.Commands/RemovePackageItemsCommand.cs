using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Ubq2.Logistics.Common.Entities;
using Ubq2.Logistics.Packing.Writers;

namespace Ubq2.Logistics.Packing.Commands
{
    public record RemovePackageItemsCommand(string SiteId, string PackageId, DateTimeOffset RequestTime, IReadOnlyCollection<string> ItemIds)
        : IRequest<VoidResult<RemovePackageItemsCommandStatus>>;

    public enum RemovePackageItemsCommandStatus { Ok }

    public record RemovePackageItemsCommandHandler(
        ILogger<RemovePackageItemsCommandHandler> Logger,
        IWriter Writer)
        : IRequestHandler<RemovePackageItemsCommand, VoidResult<RemovePackageItemsCommandStatus>>
    {
        public async Task<VoidResult<RemovePackageItemsCommandStatus>> Handle(RemovePackageItemsCommand request, CancellationToken cancellationToken)
        {
            var commandObject = new PackageItemDeleteManyCommand(
                SiteId: request.SiteId,
                PackageId: request.PackageId,
                ItemIds: request.ItemIds);

            await Writer.Write(commandObject, cancellationToken);

            return VoidResult.Create(RemovePackageItemsCommandStatus.Ok);
        }
    }
}
