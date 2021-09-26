# ubq2.logistics
Basic microservice for logistics

### Development

- part00: setup C# solution and write some readme.md
- part01: create application and domain projects and implement basic use cases for Package Headers
- part02: further implement basic use cases for Package Items
- part03: introduce Package Products

### Basic Entity definitions

Business CRUD

CREATE: add
READ: get
UPDATE: modify
DELETE: remove

Database CRUD

CREATE: insert
READ: select
UPDATE: update
DELETE: delete

#### Package Headers
- siteId: string
- packageId: string
- packageStatus: string

#### Package Items
- siteId: string
- packageId: string
- itemId: string
- productId: string

#### Package Products
- siteId: string
- packageId: string
- productId: string
- targetQty: int

### Domain use cases

#### GetPackagesQuery
- siteId: string
- packageStatus: string? 

returns package headers, operationStatus

#### AddPackageCommand
- siteId: string
- packageId: string?

returns packageId, operationStatus

#### ModifyPackageCommand
- siteId: string
- packageId: string

returns void, operationStatus

#### ModifyPackageStatusCommand
- siteId: string
- packageId: string
- packageStatus: string

returns void, operationStatus

#### GetPackageItemsQuery
- siteId: string
- packageId: string

returns package items, operationStatus

#### AddPackageItemsCommand
- siteId: string
- packageId: string
- items: string[]

returns void, operationStatus

#### RemovePackageItemsCommand
- siteId: string
- packageId: string
- items: string[]

returns void, operationStatus


### API definitions

#### Get package headers

`GET api/{{siteId}}/packages`

#### Add package header

`POST api/{{siteId}}/packages`

#### Modify package header

`PUT api/{{siteId}}/packages/{{packageId}}`

#### Modify package status

`PUT api/{{siteId}}/packages/{{packageId}}/status/{{packageStatus}}`

#### Get package items

`GET api/{{siteId}}/packages/{{packageId}}/items`

#### Add package items

`POST api/{{siteId}}/packages/{{packageId}}/add-items`

#### Remove package items

`POST api/{{siteId}}/packages/{{packageId}}/remove-items`

### C# Solution
Using clean architecture and Mediator

#### Ubq2.Logistics.Api
Asp.Net core Web application

#### Ubq2.Logistics.Application
Use cases implementation

#### Ubq2.Logistics.Domain
Enterprise entities

#### Ubq2.Logistics.Infrastructure.MemoryDb
Data readers and writers using in memory as persistence layer

#### Ubq2.Logistics.Infrastructure.MongoDb
Data readers and writers using MongoDb as persistence layer