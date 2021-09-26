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
    public class AddPackageItemsCommandTests
    {
        [Fact]
        public async Task Handle_WhenDataProvided_DataPersisted()
        {
            // arrange
            var packageHeaders = new ConcurrentDictionary<string, PackageHeaderDataObject>();
            var packageItems = new ConcurrentDictionary<string, PackageItemDataObject>();

            var logger = new NullLogger<AddPackageItemsCommandHandler>();
            var writer = new MemoryDbWriter(packageHeaders, packageItems);
            var handler = new AddPackageItemsCommandHandler(logger, writer);

            var itemIds = new HashSet<string> {
                "itemId:abc.001",
                "itemId:abc.002",
                "itemId:abc.003",
            };

            var request = new AddPackageItemsCommand(
                SiteId: "site:123",
                PackageId: "package:a",
                RequestTime: DateTimeOffset.Now,
                ItemIds: itemIds);

            // act
            var response = await handler.Handle(request, default);

            // assert
            Assert.NotNull(response);
            Assert.Equal(expected: 3, actual: packageItems.Values.Count);

            foreach (var itemId in itemIds)
            {
                var key = $"site:123, package:a, {itemId}";

                Assert.True(packageItems.ContainsKey(key));
                Assert.Equal(packageItems[key].CreatedTime, request.RequestTime);
                Assert.Equal(packageItems[key].SiteId, request.SiteId);
                Assert.Equal(packageItems[key].PackageId, request.PackageId);
                Assert.Equal(packageItems[key].ItemId, itemId);
            }
        }
    }
}
