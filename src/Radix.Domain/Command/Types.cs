using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Radix.Data;

namespace Radix.Domain.Command;

/// <summary>
/// Type of a function that take a command and returns a validated version
/// </summary>
/// <typeparam name="TCommand">The type of the command</typeparam>
/// <param name="command">The command to validate</param>
/// <returns>A validated command</returns>
public delegate Validated<TCommand> Validate<TCommand>(TCommand command);
