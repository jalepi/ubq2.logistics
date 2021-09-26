using System;

namespace Ubq2.Logistics.Common.Entities
{
    public static class Result
    {
        public static IResult<TValue, TStatus> Create<TValue, TStatus>(TValue Value, TStatus Status) where TStatus : struct, Enum
        {
            return new Result<TValue, TStatus>(Value: Value, Status: Status);
        }
    }

    public record Result<TValue, TStatus>(TValue Value, TStatus Status) : IResult<TValue, TStatus> where TStatus : struct, Enum
    {
    }
}
