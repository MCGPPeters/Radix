using System;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace Radix.Generators;

[Generator]
public class LiteralGenerator : ISourceGenerator
{
    public void Execute(GeneratorExecutionContext context)
    {
        if (context.SyntaxReceiver is not SyntaxReceiver receiver) return;

        foreach (var candidate in receiver.CandidateTypes)
        {
            var model = context.Compilation.GetSemanticModel(candidate.SyntaxTree);
            var typeSymbol = ModelExtensions.GetDeclaredSymbol(model, candidate);
            var attributeSymbol = context.Compilation.GetTypeByMetadataName("Radix.Generators.LiteralAttribute");
            if (typeSymbol is not null && attributeSymbol is not null)
            {
                var attributes = typeSymbol.GetAttributes().Where(attribute =>
                {
                    return attribute.AttributeClass is not null && attribute.AttributeClass.Name.Equals(attributeSymbol.Name);
                });

                foreach (var attribute in attributes)
                {
                    var stringRepresentation = attribute.NamedArguments.Any()
                        ? attribute.NamedArguments[0].Value.Value?.ToString() ?? ""
                        : "";

                    var classSource = ProcessType(typeSymbol, candidate, stringRepresentation);
                    var normalizedSourceCodeText = CSharpSyntaxTree.ParseText(classSource)
                        .GetRoot()
                        .NormalizeWhitespace()
                        .GetText(Encoding.UTF8);

                    context.AddSource($"{typeSymbol.ContainingNamespace.ToDisplayString()}_{typeSymbol.Name}_literal", normalizedSourceCodeText);
                }
            }
        }
    }

    public void Initialize(GeneratorInitializationContext context)
    {
        context.RegisterForSyntaxNotifications(() => new SyntaxReceiver());
        Debug.WriteLine("Initialize code generator");
    }

    internal static string ProcessType(ISymbol typeSymbol, TypeDeclarationSyntax typeDeclarationSyntax, string stringRepresention)
    {
        if (!typeSymbol.ContainingSymbol.Equals(typeSymbol.ContainingNamespace, SymbolEqualityComparer.Default))
            return "";

        var namespaceName = typeSymbol.ContainingNamespace.ToDisplayString();

        var typeSymbolName = typeSymbol.ToDisplayParts().Last().ToString();

        var kindSource = typeDeclarationSyntax.Kind() switch
        {
            SyntaxKind.ClassDeclaration => $"public sealed partial class {typeSymbolName} : Radix.Literal<{typeSymbolName}>, System.IEquatable<{typeSymbolName}>",
            SyntaxKind.RecordDeclaration => $"public sealed partial record {typeSymbolName} : Radix.Literal<{typeSymbolName}>",
            SyntaxKind.StructDeclaration => $"public partial struct {typeSymbolName}  : Radix.Literal<{typeSymbolName}>, System.IEquatable<{typeSymbolName}>",
            SyntaxKind.RecordStructDeclaration => $"public partial record struct {typeSymbolName} : Radix.Literal<{typeSymbolName}>",
            _ => throw new NotSupportedException("Unsupported type kind for generating Literal code")
        };

        var equalsOperatorsSource = $@"
            /// <summary>
            /// Determines whether two specified instances are equal.
            /// </summary>
            /// <param name=""left"">The first instance to compare.</param>
            /// <param name=""right"">The second instance to compare.</param>
            /// <returns>true if left and right are equal; otherwise, false.</returns>
            public static bool operator ==({typeSymbol.ToDisplayString()} left, {typeSymbol.ToDisplayString()} right) => Equals(left, right);
            /// <summary>
            /// Determines whether two specified instances are not equal.
            /// </summary>
            /// <param name=""left"">The first instance to compare.</param>
            /// <param name=""right"">The second instance to compare.</param>
            /// <returns>true if left and right are not equal; otherwise, false.</returns>
            public static bool operator !=({typeSymbol.ToDisplayString()} left, {typeSymbol.ToDisplayString()} right) => !Equals(left, right);
        ";

        var equalsSource = typeDeclarationSyntax.Kind() switch
        {
            SyntaxKind.ClassDeclaration => $@"
            {equalsOperatorsSource}
            /// <summary>
            /// Determines whether the specified object is equal to the current object.
            /// </summary>
            /// <param name=""obj"">The object to compare with the current object.</param>
            /// <returns>true if the specified object is equal to the current object; otherwise, false.</returns>
            public override bool Equals(object obj) => obj is {typeSymbol.ToDisplayString()} other;
            /// <summary>
            /// Serves as the default hash function.
            /// </summary>
            /// <returns>A hash code for the current object.</returns>
            public override int GetHashCode() => ""{typeSymbolName}"".GetHashCode();
            /// <summary>
            /// Indicates whether the current object is equal to another object of the same type.
            /// </summary>
            /// <param name=""other"">An object to compare with this object.</param>
            /// <returns>true if the current object is equal to the other parameter; otherwise, false.</returns>
            public bool Equals({typeSymbolName} other) => true;",
            SyntaxKind.RecordDeclaration => "",
            SyntaxKind.StructDeclaration => $@"
            {equalsOperatorsSource}
            /// <summary>
            /// Determines whether the specified object is equal to the current object.
            /// </summary>
            /// <param name=""obj"">The object to compare with the current object.</param>
            /// <returns>true if the specified object is equal to the current object; otherwise, false.</returns>
            public override bool Equals(object? obj) => obj is {typeSymbol.ToDisplayString()} other;
            /// <summary>
            /// Serves as the default hash function.
            /// </summary>
            /// <returns>A hash code for the current object.</returns>
            public override int GetHashCode() => ""{typeSymbolName}"".GetHashCode();
            /// <summary>
            /// Indicates whether the current object is equal to another object of the same type.
            /// </summary>
            /// <param name=""other"">An object to compare with this object.</param>
            /// <returns>true if the current object is equal to the other parameter; otherwise, false.</returns>
            public bool Equals({typeSymbolName} other) => true;",
            SyntaxKind.RecordStructDeclaration => "",
            _ => throw new NotSupportedException("Unsupported type kind for generating Literal code")
        };

        var toString = string.IsNullOrEmpty(stringRepresention) ? typeSymbol.Name : stringRepresention;

        var source = new StringBuilder($@"
namespace {namespaceName}
{{
    {kindSource}, System.IParsable<{typeSymbolName}>
    {{
        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString() => ""{toString}"";
        {equalsSource}

        /// <summary>
        /// Defines an implicit conversion of a {typeSymbolName} to a string.
        /// </summary>
        /// <param name=""value"">The {typeSymbolName} to convert.</param>
        /// <returns>A string representation of the value.</returns>
        public static implicit operator string({typeSymbolName} value) => ""{toString}"";

        /// <summary>
        /// Defines an implicit conversion of a string to a {typeSymbolName}.
        /// </summary>
        /// <param name=""value"">The string to convert.</param>
        /// <returns>A {typeSymbolName} representation of the string.</returns>
        public static implicit operator {typeSymbolName}(string value) => value == ""{toString}"" ? new() : throw new ArgumentException(""'value' is not assignable to '{typeSymbol.Name}'"");

        /// <summary>
        /// Formats the value of the current instance using the specified format.
        /// </summary>
        /// <returns>The value of the current instance in string format.</returns>
        public static string Format() => ""{toString}"";

        public string ToString(string? format, IFormatProvider? formatProvider) => ""{toString}"";

        /// <summary>
        /// Converts the string representation of a number to its {typeSymbolName} equivalent.
        /// </summary>
        /// <param name=""s"">A string containing a number to convert.</param>
        /// <param name=""provider"">An object that supplies culture-specific formatting information.</param>
        /// <returns>A {typeSymbolName} equivalent to the number contained in s.</returns>
        public static {typeSymbolName} Parse(string s, IFormatProvider? provider)
        {{
            if (""{toString}"" == s) return new {typeSymbolName}();
            throw new ArgumentException(""'value' is not assignable to '{typeSymbol.Name}'"");
        }}

        /// <summary>
        /// Tries to convert the string representation of a number to its {typeSymbolName} equivalent, and returns a value that indicates whether the conversion succeeded.
        /// </summary>
        /// <param name=""s"">The string representation of a number.</param>
        /// <param name=""provider"">An object that supplies culture-specific formatting information.</param>
        /// <param name=""result"">When this method returns, contains the {typeSymbolName} value equivalent to the number contained in s, if the conversion succeeded, or null if the conversion failed. The conversion fails if the s parameter is null or String.Empty, is not of the correct format, or represents a number less than MinValue or greater than MaxValue. This parameter is passed uninitialized; any value originally supplied in result will be overwritten.</param>
        /// <returns>true if s was converted successfully; otherwise, false.</returns>
        public static bool TryParse(string? s, IFormatProvider? provider, [System.Diagnostics.CodeAnalysis.MaybeNullWhen(returnValue: false)] out {typeSymbolName} result)
        {{
            result = default;
            bool success = ""{toString}"" == s;
            if(success) result = new {typeSymbolName}();
            return success;
        }}
    }}
}}");
        return source.ToString();
    }
}
