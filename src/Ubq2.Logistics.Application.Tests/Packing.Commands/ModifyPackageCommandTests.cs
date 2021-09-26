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
    public class ModifyPackageCommandTests
    {
        [Fact]
        public async Task Handle_WhenDataProvided_DataPersisted()
        {
            // arrange
            var packageHeaders = new ConcurrentDictionary<string, PackageHeader>
            {
                ["site:123, package:a"] = new(SiteId: "site:123", PackageId: "package:a", PackageStatus: PackageStatus.Open, DateTimeOffset.Now, DateTimeOffset.MinValue),
                ["site:123, package:b"] = new(SiteId: "site:123", PackageId: "package:b", PackageStatus: PackageStatus.Open, DateTimeOffset.Now, DateTimeOffset.MinValue),
                ["site:123, package:c"] = new(SiteId: "site:123", PackageId: "package:c", PackageStatus: PackageStatus.Open, DateTimeOffset.Now, DateTimeOffset.MinValue),
            };

            var logger = new NullLogger<ModifyPackageCommandHandler>();
            var writer = new MemoryDbWriter(packageHeaders);
            var handler = new ModifyPackageCommandHandler(logger, writer);

            var request = new ModifyPackageCommand(SiteId: "site:123", PackageId: "package:a", UpdatedTime: DateTimeOffset.Now);

            // act
            var response = await handler.Handle(request, default);

            // assert
            Assert.NotNull(response);
            Assert.Contains(collection: packageHeaders, ph => ph.Value.PackageId == "package:a");

            Assert.NotEqual(expected: packageHeaders["site:123, package:a"].CreatedTime, actual: request.UpdatedTime);
            Assert.Equal(expected: packageHeaders["site:123, package:a"].UpdatedTime, actual: request.UpdatedTime);
        }
    }
}
