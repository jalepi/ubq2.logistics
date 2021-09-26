using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ubq2.Logistics.Packing.DataObjects;
using Ubq2.Logistics.Packing.Writers;
using Xunit;

namespace Ubq2.Logistics.Packing.Commands
{
    public class RemovePackageItemsCommandTests
    {
        [Fact]
        public async Task Handle_WhenDataProvided_DataPersisted()
        {
            // arrange
            var packageHeaders = new ConcurrentDictionary<string, PackageHeaderDataObject>();
            var packageItems = new ConcurrentDictionary<string, PackageItemDataObject>
            {
                ["site:123, package:a, item:abc.001"] = new(SiteId: "site:123", PackageId: "package:a", ItemId: "item:abc.001", CreatedTime: DateTimeOffset.Now),
                ["site:123, package:a, item:abc.002"] = new(SiteId: "site:123", PackageId: "package:a", ItemId: "item:abc.002", CreatedTime: DateTimeOffset.Now),
                ["site:123, package:a, item:abc.003"] = new(SiteId: "site:123", PackageId: "package:a", ItemId: "item:abc.003", CreatedTime: DateTimeOffset.Now),
            };

            var logger = new NullLogger<RemovePackageItemsCommandHandler>();
            var writer = new MemoryDbWriter(packageHeaders, packageItems);
            var handler = new RemovePackageItemsCommandHandler(logger, writer);

            var itemIds = new HashSet<string> {
                "item:abc.001",
                "item:abc.003",
            };

            var request = new RemovePackageItemsCommand(
                SiteId: "site:123",
                PackageId: "package:a",
                RequestTime: DateTimeOffset.Now,
                ItemIds: itemIds);

            // act
            var response = await handler.Handle(request, default);

            // assert
            Assert.NotNull(response);
            Assert.Equal(expected: 1, actual: packageItems.Values.Count);

            foreach (var itemId in new[] { "item:abc.002" })
            {
                var key = $"site:123, package:a, {itemId}";

                Assert.True(packageItems.ContainsKey(key));
                Assert.Equal(packageItems[key].SiteId, request.SiteId);
                Assert.Equal(packageItems[key].PackageId, request.PackageId);
                Assert.Equal(packageItems[key].ItemId, itemId);
            }

            foreach (var itemId in itemIds)
            {
                var key = $"site:123, package:a, {itemId}";

                Assert.False(packageItems.ContainsKey(key));
            }
        }
    }
}
