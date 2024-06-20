using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Radix.Generators.Attributes;

namespace Radix.Web.Css.Data.Keywords;

[Literal]
public partial struct auto : Value { } 

[Literal(StringRepresentation = "max-content")]
public partial struct max_content : Value { }

[Literal(StringRepresentation = "min-content")]
public partial struct min_content : Value { }

[Literal]
public partial struct inherit : Value { }

[Literal]
public partial struct initial : Value { }

[Literal]
public partial struct revert : Value { }

[Literal]
public partial struct unset : Value { }
