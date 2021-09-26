using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using Ubq2.Logistics.Packing.DataObjects;
using Ubq2.Logistics.Packing.Entities;
using Ubq2.Logistics.Packing.Readers;
using Xunit;

namespace Ubq2.Logistics.Packing.Queries
{
    public class GetPackagesQueryTests
    {
        [Fact]
        public async Task Handle_WhenDataAvailable_ReturnsAvailableData()
        {
            // arrange
            var packageHeaders = new ConcurrentDictionary<string, PackageHeaderDataObject>
            {
                ["site:123, package:a"] = new(SiteId: "site:123", PackageId: "package:a", PackageStatus: PackageStatusDataObject.Open, CreatedTime: DateTimeOffset.Now, UpdatedTime: DateTimeOffset.MinValue),
                ["site:123, package:b"] = new(SiteId: "site:123", PackageId: "package:b", PackageStatus: PackageStatusDataObject.Open, CreatedTime: DateTimeOffset.Now, UpdatedTime: DateTimeOffset.MinValue),
                ["site:123, package:c"] = new(SiteId: "site:123", PackageId: "package:c", PackageStatus: PackageStatusDataObject.Open, CreatedTime: DateTimeOffset.Now, UpdatedTime: DateTimeOffset.MinValue),
            };
            var packageItems = new ConcurrentDictionary<string, PackageItemDataObject>();
            var logger = new NullLogger<GetPackagesQueryHandler>();
            var reader = new MemoryDbReader(packageHeaders, packageItems);
            var handler = new GetPackagesQueryHandler(logger, reader);

            var request = new GetPackagesQuery(SiteId: "site:123", PackageStatus: PackageStatus.Open);

            // act
            var response = await handler.Handle(request, default);

            // assert
            Assert.NotNull(response);
            Assert.Equal(expected: GetPackagesQueryStatus.Ok, actual: response.Status);
            Assert.NotNull(response.Value);
            Assert.Equal(expected: 3, actual: response.Value.Count);
            Assert.Contains(collection: response.Value, filter: ph => ph.PackageId == "package:a");
            Assert.Contains(collection: response.Value, filter: ph => ph.PackageId == "package:b");
            Assert.Contains(collection: response.Value, filter: ph => ph.PackageId == "package:c");
        }
    }
}
