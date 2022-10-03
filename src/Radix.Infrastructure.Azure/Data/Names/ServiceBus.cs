using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Radix.Data.String.Validity;

namespace Radix.Infrastructure.Azure.Data.Names.ServiceBus;

[Validated<string, StartsWithALetter>]
[Validated<string, EndsWithALetterOrNumber>]
[Validated<string, IsNotNullOrEmpty>]
[Validated<string, ContainsOnlyAlphaNumericsAndHyphens>]
public partial record struct Namespace;
