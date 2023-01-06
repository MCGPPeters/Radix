using System.Diagnostics;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Linq;

namespace Radix.Generators;

[Generator]
public class ValidatedGenerator : ISourceGenerator
{
    public void Execute(GeneratorExecutionContext context)
    {
        if (context.SyntaxReceiver is not SyntaxReceiver receiver) return;

        // the syntax receiver is used to find all types that have attributes
        foreach (var candidate in receiver.CandidateTypes)
        {
            var model = context.Compilation.GetSemanticModel(candidate.SyntaxTree);
            var typeSymbol = ModelExtensions.GetDeclaredSymbol(model, candidate);
            var attributeSymbol = context.Compilation.GetTypeByMetadataName("Radix.ValidatedAttribute`2");
            // check if the candidate had Validated<T, V> attributes
            // get the metadata for all attributes that are Validated attributes
            var attributes = typeSymbol?.GetAttributes().Where(attribute =>
            {
                if (attribute.AttributeClass is not null)
                {
                    if (attribute.AttributeClass.Name.Equals(attributeSymbol?.Name))
                        return true;
                }
                return false;
            });

            var derivedAttributes = typeSymbol?.GetAttributes().Where(attribute =>
            {
                if (attribute.AttributeClass is not null)
                {
                    if (attribute.AttributeClass.BaseType is not null)
                    {
                        if (attribute.AttributeClass.BaseType.Name.Equals(attributeSymbol?.Name))
                            return true;
                        if ((attribute.AttributeClass.AllInterfaces.Any(ts => ts.Name.Equals(attributeSymbol?.Name))))
                            return true;
                    }
                }
                return false;
            });



            if (attributes.Any())
            {
                // get the Validity<T> subtype which holds the validator function (which is the second type argument of the Validated<T, V> attributes)
                var validityTypes =
                    attributes
                        .Select(attribute => $"{attribute.AttributeClass?.TypeArguments[1].ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)}");

                if (derivedAttributes.Any())
                {
                    // get the Validity<T> subtype which holds the validator function (which is the second type argument of the Validated<T, V> attributes)
                    validityTypes = validityTypes.Concat(derivedAttributes
                        .Select(Selector));
                }


                if (typeSymbol is not null && validityTypes.Any())
                {
                    // get the type we are "aliasing" (which is the first type argument of the Validated<t, V> attributes)
                    string valueType = attributes.FirstOrDefault().AttributeClass!.TypeArguments[0].ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
                    // create the required source code
                    var sourceCode = ProcessType(valueType, validityTypes, typeSymbol, candidate);
                    // fix text formating according to default ruleset
                    var normalizedSourceCodeText
                        = CSharpSyntaxTree.ParseText(sourceCode).GetRoot().NormalizeWhitespace().GetText(Encoding.UTF8);
                    context.AddSource(
                        $"Validated{typeSymbol.ContainingNamespace.ToDisplayString()}_{typeSymbol.Name}",
                       normalizedSourceCodeText);
                }
            }
        }
    }

    private string Selector(AttributeData attribute)
    {
        return $"{attribute.AttributeClass?.BaseType?.TypeArguments[1].ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)}";
    }


    public void Initialize(GeneratorInitializationContext context)
    {
        context.RegisterForSyntaxNotifications(() => new SyntaxReceiver());
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="valueTypeName">The typename of the value to validate (the aliased type)</param>
    /// <param name="validityTypeNames">The type names of the validity instances holding the validator functions</param>
    /// <param name="typeSymbol">The symbol of the type to which the Validated attributes were added
    /// <param name="typeDeclarationSyntax">The declaration syntax of the type to which the Validated attributes were added</param>
    /// <returns></returns>
    /// <exception cref="NotSupportedException"></exception>
    internal static string ProcessType(string valueTypeName, IEnumerable<string> validityTypeNames, ISymbol typeSymbol, TypeDeclarationSyntax typeDeclarationSyntax)
    {
        if (!typeSymbol.ContainingSymbol.Equals(typeSymbol.ContainingNamespace, SymbolEqualityComparer.Default))
            return "";

        const string propertyName = "Value";
        var namespaceName = typeSymbol.ContainingNamespace.ToDisplayString();

        var kindSource = typeDeclarationSyntax.Kind() switch
        {
            SyntaxKind.ClassDeclaration => $"public sealed partial class {typeSymbol.Name}  : System.IEquatable<{typeSymbol.Name}>",
            SyntaxKind.RecordDeclaration => $"public sealed partial record {typeSymbol.Name}",
            SyntaxKind.StructDeclaration => $"public partial struct {typeSymbol.Name}  : System.IEquatable<{typeSymbol.Name}>",
            SyntaxKind.RecordStructDeclaration => $"public partial record struct {typeSymbol.Name} ",
            _ => throw new NotSupportedException("Unsupported type kind for generating Validated code")
        };

        var equalsOperatorsSource = $@"
                public static bool operator ==({typeSymbol.Name} left, {typeSymbol.Name} right) => Equals(left, right);
                public static bool operator !=({typeSymbol.Name} left, {typeSymbol.Name} right) => !Equals(left, right);
            ";

        var equalsSource = typeDeclarationSyntax.Kind() switch
        {
            SyntaxKind.ClassDeclaration => $@"
                {equalsOperatorsSource}
                public override bool Equals(object? obj) => obj is {typeSymbol.Name} other && Equals(other);
                public override int GetHashCode() => {propertyName}.GetHashCode();
                public bool Equals({typeSymbol.Name} other){{ return {propertyName} == other.{propertyName}; }}",
            // records have implicit value equality
            SyntaxKind.RecordDeclaration => "",
            SyntaxKind.StructDeclaration => $@"
                {equalsOperatorsSource}
                public override bool Equals(object? obj) => obj is {typeSymbol.Name} other && Equals(other);
                public override int GetHashCode() => {propertyName}.GetHashCode();
                public bool Equals({typeSymbol.Name} other)
                {{
                    return {propertyName} == other.{propertyName};
                }}",
            // records struct have implicit value equality
            SyntaxKind.RecordStructDeclaration => "",
            _ => throw new NotSupportedException("Unsupported type kind for generating Validated code")
        };

        // create a string representation of a list of validator functions
        string validatorFunctions =
            validityTypeNames
            .Select(validityType => $"{validityType}.Validate(\"{typeSymbol.Name}\")")
            .Aggregate((current, next) => $"{current},{Environment.NewLine}{next}");

        // build an array initializer from the comma seperated list
        var validations = $"new []{{ {validatorFunctions} }}";

        var source = new StringBuilder($@"
namespace {namespaceName}
{{
    using static Radix.Control.Validated.Extensions;
    using Radix.Data;

    {kindSource}
    {{
        {((typeDeclarationSyntax.Kind() is SyntaxKind.StructDeclaration || typeDeclarationSyntax.Kind() is SyntaxKind.RecordStructDeclaration)
            ? $"#pragma warning disable CS8618 {Environment.NewLine} [System.Obsolete(\"Calling a constructor on a Validated type is not allowed.\", true)]public " + typeSymbol.Name + "(){}" + Environment.NewLine + "#pragma warning restore CS8618"
            : "")}

        public static Validated<{typeSymbol.Name}> Create({valueTypeName} value)
        {{
            return value.Validate({validations}).Map(d => new {typeSymbol.Name}(d));
        }}
        

        public {valueTypeName} {propertyName} {{ get; }}

        private {typeSymbol.Name}({valueTypeName} value)
        {{
            {propertyName} = value;
        }}

        public override string ToString() => {propertyName}.ToString();

        {equalsSource}

        public static implicit operator {valueTypeName} ({typeSymbol.Name} value) => value.{propertyName};
    }}
}}");
        return source.ToString();
    }
}
