using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using Ubq2.Logistics.Packing.DataObjects;
using Ubq2.Logistics.Packing.Entities;

namespace Ubq2.Logistics.Packing.Writers
{
    public record MemoryDbWriter(
        ConcurrentDictionary<string, PackageHeaderDataObject> PackageHeaders,
        ConcurrentDictionary<string, PackageItemDataObject> PackageItems)
        : IWriter
    {
        public async Task Write(PackageInsertOneCommand commandObject, CancellationToken cancellationToken)
        {
            await Task.Yield();

            var dataObject = new PackageHeaderDataObject(
                SiteId: commandObject.SiteId,
                PackageId: string.IsNullOrEmpty(commandObject.PackageId) ? $"{Guid.NewGuid()}" : commandObject.PackageId,
                PackageStatus: PackageStatusDataObject.Open,
                CreatedTime: commandObject.CreatedTime,
                UpdatedTime: DateTimeOffset.MinValue);

            var key = $"{dataObject.SiteId}, {dataObject.PackageId}";

            PackageHeaders.TryAdd(key, dataObject);
        }

        public async Task Write(PackageUpdateOneCommand commandObject, CancellationToken cancellationToken)
        {
            await Task.Yield();

            var key = $"{commandObject.SiteId}, {commandObject.PackageId}";

            PackageHeaders.AddOrUpdate(
                key: key,
                addValueFactory: key => new PackageHeaderDataObject(
                    SiteId: commandObject.SiteId,
                    PackageId: string.IsNullOrEmpty(commandObject.PackageId) ? $"{Guid.NewGuid()}" : commandObject.PackageId,
                    PackageStatus: PackageStatusDataObject.Open,
                    CreatedTime: commandObject.UpdatedTime,
                    UpdatedTime: DateTimeOffset.MinValue),
                updateValueFactory: (key, value) => value with { UpdatedTime = commandObject.UpdatedTime });
        }

        public async Task Write(PackageItemInsertManyCommand commandObject, CancellationToken cancellationToken)
        {
            await Task.Yield();

            foreach (var itemId in commandObject.ItemIds)
            {
                var key = $"{commandObject.SiteId}, {commandObject.PackageId}, {itemId}";

                var entity = new PackageItemDataObject(
                    SiteId: commandObject.SiteId,
                    PackageId: commandObject.PackageId,
                    CreatedTime: commandObject.CreatedTime,
                    ItemId: itemId);

                PackageItems.TryAdd(key, entity);
            }
        }

        public async Task Write(PackageItemDeleteManyCommand commandObject, CancellationToken cancellationToken)
        {
            await Task.Yield();

            foreach (var itemId in commandObject.ItemIds)
            {
                var key = $"{commandObject.SiteId}, {commandObject.PackageId}, {itemId}";

                PackageItems.TryRemove(key, out var _);
            }
        }
    }
}
