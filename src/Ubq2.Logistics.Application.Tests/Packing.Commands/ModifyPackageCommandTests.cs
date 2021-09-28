using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using Ubq2.Logistics.Packing.DataObjects;
using Ubq2.Logistics.Packing.Writers;
using Xunit;

namespace Ubq2.Logistics.Packing.Commands
{
    public class ModifyPackageCommandTests
    {
        [Fact]
        public async Task Handle_WhenDataProvided_DataPersisted()
        {
            // arrange
            var packageHeaders = new ConcurrentDictionary<string, PackageHeaderDataObject>
            {
                ["site:123, package:a"] = new(SiteId: "site:123", PackageId: "package:a", PackageStatus: PackageStatusDataObject.Open, DateTimeOffset.Now, DateTimeOffset.MinValue),
                ["site:123, package:b"] = new(SiteId: "site:123", PackageId: "package:b", PackageStatus: PackageStatusDataObject.Open, DateTimeOffset.Now, DateTimeOffset.MinValue),
                ["site:123, package:c"] = new(SiteId: "site:123", PackageId: "package:c", PackageStatus: PackageStatusDataObject.Open, DateTimeOffset.Now, DateTimeOffset.MinValue),
            };
            var packageItems = new ConcurrentDictionary<string, PackageItemDataObject>();
            var packageProducts = new ConcurrentDictionary<string, PackageProductDataObject>();

            var logger = new NullLogger<ModifyPackageCommandHandler>();
            var writer = new MemoryDbWriter(packageHeaders, packageItems, packageProducts);
            var handler = new ModifyPackageCommandHandler(logger, writer);

            var request = new ModifyPackageCommand(SiteId: "site:123", PackageId: "package:a", RequestTime: DateTimeOffset.Now);

            // act
            var response = await handler.Handle(request, default);

            // assert
            Assert.NotNull(response);
            Assert.Contains(collection: packageHeaders, ph => ph.Value.PackageId == "package:a");

            Assert.NotEqual(expected: packageHeaders["site:123, package:a"].CreatedTime, actual: request.RequestTime);
            Assert.Equal(expected: packageHeaders["site:123, package:a"].UpdatedTime, actual: request.RequestTime);
        }
    }
}
