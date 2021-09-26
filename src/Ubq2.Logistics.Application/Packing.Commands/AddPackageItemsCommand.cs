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
    public record AddPackageItemsCommand(string SiteId, string PackageId, DateTimeOffset RequestTime, IReadOnlyCollection<string> ItemIds)
        : IRequest<VoidResult<AddPackageItemsCommandStatus>>;

    public enum AddPackageItemsCommandStatus { Ok }

    public record AddPackageItemsCommandHandler(
        ILogger<AddPackageItemsCommandHandler> Logger,
        IWriter Writer)
        : IRequestHandler<AddPackageItemsCommand, VoidResult<AddPackageItemsCommandStatus>>
    {
        public async Task<VoidResult<AddPackageItemsCommandStatus>> Handle(AddPackageItemsCommand request, CancellationToken cancellationToken)
        {
            var commandObject = new PackageItemInsertManyCommand(
                SiteId: request.SiteId,
                PackageId: request.PackageId,
                CreatedTime: request.RequestTime,
                ItemIds: request.ItemIds);

            await Writer.Write(commandObject, cancellationToken);

            return VoidResult.Create(AddPackageItemsCommandStatus.Ok);
        }
    }
}
