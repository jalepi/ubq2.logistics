using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using Ubq2.Logistics.Packing.DataObjects;
using Ubq2.Logistics.Packing.Writers;
using Xunit;

namespace Ubq2.Logistics.Packing.Commands
{
    public class AddPackageCommandTests
    {
        [Fact]
        public async Task Handle_WhenDataProvided_DataPersisted()
        {
            // arrange
            var packageHeaders = new ConcurrentDictionary<string, PackageHeaderDataObject>();
            var packageItems = new ConcurrentDictionary<string, PackageItemDataObject>();
            var packageProducts = new ConcurrentDictionary<string, PackageProductDataObject>();

            var logger = new NullLogger<AddPackageCommandHandler>();
            var writer = new MemoryDbWriter(packageHeaders, packageItems, packageProducts);
            var handler = new AddPackageCommandHandler(logger, writer);

            var request = new AddPackageCommand(SiteId: "site:123", PackageId: "package:a", RequestTime: DateTimeOffset.Now);

            // act
            var response = await handler.Handle(request, default);

            // assert
            Assert.NotNull(response);
            Assert.Single(collection: packageHeaders);
            Assert.Contains(collection: packageHeaders, ph => ph.Value.PackageId == "package:a");
        }
    }
}
