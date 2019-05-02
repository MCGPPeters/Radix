using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Radix.Tests;
using Radix.Tests.Result;
using Task;
using static Radix.Tests.Result.Extensions;

namespace TaskResult
{
    public static class Extensions
    {
     
        public static Task<Result<TResult>> Map<T, TResult>
            (this Task<Result<T>> task, Func<T, TResult> f)
            => task.Select(result => result.Map(f));

        public static Task<Result<TResult>> Bind<T, TResult>(this Task<Result<T>> task, Func<T, Task<Result<TResult>>> function) 
            => task.SelectMany(tr => tr.Bind(async x => await function(x)))
    }
}