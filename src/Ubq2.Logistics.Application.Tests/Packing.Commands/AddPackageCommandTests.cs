using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ubq2.Logistics.Packing.Entities;
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
            var packageHeaders = new ConcurrentDictionary<string, PackageHeader>();

            var logger = new NullLogger<AddPackageCommandHandler>();
            var writer = new MemoryDbWriter(packageHeaders);
            var handler = new AddPackageCommandHandler(logger, writer);

            var request = new AddPackageCommand(SiteId: "site:123", PackageId: "package:a", CreatedTime: DateTimeOffset.Now);

            // act
            var response = await handler.Handle(request, default);

            // assert
            Assert.NotNull(response);
            Assert.Single(collection: packageHeaders);
            Assert.Contains(collection: packageHeaders, ph => ph.Value.PackageId == "package:a");
        }
    }
}
