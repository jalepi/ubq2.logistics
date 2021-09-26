namespace Ubq2.Logistics.Packing.DataObjects
{
    public interface IDataObject
    {
        string PartitionKey { get; }
        string PrimaryKey { get; }
    }
}