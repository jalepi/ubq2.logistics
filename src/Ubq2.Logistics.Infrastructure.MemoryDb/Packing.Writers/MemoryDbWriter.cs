using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Ubq2.Logistics.Packing.Entities;

namespace Ubq2.Logistics.Packing.Writers
{
    public record MemoryDbWriter(ConcurrentDictionary<string, PackageHeader> PackageHeaders)
        : IWriter
    {
        public async Task Write(PackageInsertOneCommand commandObject, CancellationToken cancellationToken)
        {
            await Task.Yield();

            var entity = new PackageHeader(
                SiteId: commandObject.SiteId,
                PackageId: string.IsNullOrEmpty(commandObject.PackageId) ? $"{Guid.NewGuid()}" : commandObject.PackageId,
                PackageStatus: PackageStatus.Open,
                CreatedTime: commandObject.CreatedTime,
                UpdatedTime: DateTimeOffset.MinValue);

            var key = $"{commandObject.SiteId}, {commandObject.PackageId}";
            
            PackageHeaders.TryAdd(key, entity);
        }

        public async Task Write(PackageUpdateOneCommand commandObject, CancellationToken cancellationToken)
        {
            await Task.Yield();

            var key = $"{commandObject.SiteId}, {commandObject.PackageId}";

            PackageHeaders.AddOrUpdate(
                key: key, 
                addValueFactory: key => new PackageHeader(
                    SiteId: commandObject.SiteId,
                    PackageId: string.IsNullOrEmpty(commandObject.PackageId) ? $"{Guid.NewGuid()}" : commandObject.PackageId,
                    PackageStatus: PackageStatus.Open,
                    CreatedTime: commandObject.UpdatedTime,
                    UpdatedTime: DateTimeOffset.MinValue),
                updateValueFactory: (key, value) => value with { UpdatedTime = commandObject.UpdatedTime });
        }
    }
}
