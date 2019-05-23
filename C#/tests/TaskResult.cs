using System;
using System.Threading.Tasks;
using Radix.Tests;
using Radix.Tests.Future;

namespace Radix.Tests
{
    public class TaskResult<T, TError> : Task<Result<T>>
    {
        public TaskResult(Func<object, Result<T>> function, object state) : base(function, state)
        {
        }

    }
}