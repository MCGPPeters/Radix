using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radix.Tests;

public record ExecuteTask<T>(Task<T> Task);

public interface TaskResult { }

public record TaskSucceeded<TResult>(TResult Result) : TaskResult;

public record TaskFailed<TError>(TError Result) : TaskResult;

