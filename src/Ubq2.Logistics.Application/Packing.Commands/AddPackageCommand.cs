using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using Ubq2.Logistics.Common.Entities;
using Ubq2.Logistics.Packing.Writers;

namespace Ubq2.Logistics.Packing.Commands
{
    public record AddPackageCommand(string SiteId, string PackageId, DateTimeOffset CreatedTime)
        : IRequest<VoidResult<AddPackageCommandStatus>>
    {
    }

    public enum AddPackageCommandStatus { Ok }

    public record AddPackageCommandHandler(
        ILogger<AddPackageCommandHandler> Logger,
        IWriter Writer)
        : IRequestHandler<AddPackageCommand, VoidResult<AddPackageCommandStatus>>
    {
        public async Task<VoidResult<AddPackageCommandStatus>> Handle(AddPackageCommand request, CancellationToken cancellationToken)
        {
            var commandObject = new PackageInsertOneCommand(
                SiteId: request.SiteId,
                PackageId: request.PackageId,
                CreatedTime: request.CreatedTime);

            await Writer.Write(commandObject, cancellationToken);

            return VoidResult.Create(AddPackageCommandStatus.Ok);
        }
    }
}
