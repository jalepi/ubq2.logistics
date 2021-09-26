using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using Ubq2.Logistics.Common.Entities;
using Ubq2.Logistics.Packing.Writers;

namespace Ubq2.Logistics.Packing.Commands
{
    public record ModifyPackageCommand(string SiteId, string PackageId, DateTimeOffset RequestTime)
        : IRequest<VoidResult<ModifyPackageCommandStatus>>
    {
    }

    public enum ModifyPackageCommandStatus { Ok }

    public record ModifyPackageCommandHandler(
        ILogger<ModifyPackageCommandHandler> Logger,
        IWriter Writer)
        : IRequestHandler<ModifyPackageCommand, VoidResult<ModifyPackageCommandStatus>>
    {
        public async Task<VoidResult<ModifyPackageCommandStatus>> Handle(ModifyPackageCommand request, CancellationToken cancellationToken)
        {
            var commandObject = new PackageHeaderUpdateOneCommand(
                SiteId: request.SiteId,
                PackageId: request.PackageId,
                UpdatedTime: request.RequestTime);

            await Writer.Write(commandObject, cancellationToken);

            return VoidResult.Create(ModifyPackageCommandStatus.Ok);
        }
    }
}
