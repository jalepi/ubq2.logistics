using System;

namespace Ubq2.Logistics.Common.Entities
{
    public interface IResult<out TValue, out TStatus> where TStatus : struct, Enum
    {
        TStatus Status { get; }
        TValue Value { get; }
    }
}