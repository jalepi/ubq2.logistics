using System;

namespace Ubq2.Logistics.Common.Entities
{
    public static class VoidResult
    {
        public static VoidResult<TStatus> Create<TStatus>(TStatus Status) where TStatus : struct, Enum
        {
            return new VoidResult<TStatus>(Status: Status);
        }
    }

    public record VoidResult<TStatus>(TStatus Status) : IResult<Void, TStatus> where TStatus : struct, Enum
    {
        private static readonly Void @void = new();

        public Void Value => @void;
    }
}
