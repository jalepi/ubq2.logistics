using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using Ubq2.Logistics.Packing.DataObjects;
using Ubq2.Logistics.Packing.Readers;
using Xunit;

namespace Ubq2.Logistics.Packing.Queries
{
    public class GetPackageItemsQueryTests
    {
        [Fact]
        public async Task Handle_WhenDataAvailable_ReturnsAvailableData()
        {
            // arrange
            var packageHeaders = new ConcurrentDictionary<string, PackageHeaderDataObject>();
            var packageItems = new ConcurrentDictionary<string, PackageItemDataObject>
            {
                ["site:123, package:a, item:abc.001"] = new(SiteId: "site:123", PackageId: "package:a", ItemId: "item:abc.001", CreatedTime: DateTimeOffset.Now),
                ["site:123, package:a, item:abc.002"] = new(SiteId: "site:123", PackageId: "package:a", ItemId: "item:abc.002", CreatedTime: DateTimeOffset.Now),
                ["site:123, package:a, item:abc.003"] = new(SiteId: "site:123", PackageId: "package:a", ItemId: "item:abc.003", CreatedTime: DateTimeOffset.Now),
            };

            var logger = new NullLogger<GetPackageItemsQueryHandler>();
            var reader = new MemoryDbReader(packageHeaders, packageItems);
            var handler = new GetPackageItemsQueryHandler(logger, reader);

            var request = new GetPackageItemsQuery(SiteId: "site:123", PackageId: "package:a");

            // act
            var response = await handler.Handle(request, default);

            // assert
            Assert.NotNull(response);
            Assert.Contains(collection: response.Value, filter: ph => ph.ItemId == "item:abc.001");
            Assert.Contains(collection: response.Value, filter: ph => ph.ItemId == "item:abc.002");
            Assert.Contains(collection: response.Value, filter: ph => ph.ItemId == "item:abc.003");
        }
    }
}
