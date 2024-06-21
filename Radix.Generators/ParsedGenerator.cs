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
public class ParsedGenerator : ISourceGenerator
{
    public void Execute(GeneratorExecutionContext context)
    {
        if (context.SyntaxReceiver is not SyntaxReceiver receiver) return;

        foreach (var candidate in receiver.CandidateTypes)
        {
            var model = context.Compilation.GetSemanticModel(candidate.SyntaxTree);
            var typeSymbol = ModelExtensions.GetDeclaredSymbol(model, candidate);
            var attributeSymbol = context.Compilation.GetTypeByMetadataName("Radix.ParsedAttribute`2");
            if (typeSymbol == null) continue; // Check for null

            var attributes = typeSymbol.GetAttributes().Where(attribute => attribute.AttributeClass?.Name.Equals(attributeSymbol?.Name) == true);
            foreach (var attribute in attributes)
            {
                if (attribute.AttributeClass is not null && attribute.AttributeClass.TypeArguments.Length >= 2)
                {
                    var classSource = ProcessType(attribute.AttributeClass.TypeArguments[0].Name, $"{attribute.AttributeClass.TypeArguments[1].ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)}", typeSymbol, candidate);
                    var normalizedSourceCodeText = CSharpSyntaxTree.ParseText(classSource).GetRoot().NormalizeWhitespace().GetText(Encoding.UTF8);
                    context.AddSource(
                        $"Parsed{typeSymbol.ContainingNamespace.ToDisplayString()}_{typeSymbol.Name}",
                        normalizedSourceCodeText);
                }
            }
        }
    }



    public void Initialize(GeneratorInitializationContext context)
    {
        context.RegisterForSyntaxNotifications(() => new SyntaxReceiver());

        Debug.WriteLine("Initalize code generator");
    }

    internal static string ProcessType(string valueType, string validityType, ISymbol typeSymbol, TypeDeclarationSyntax typeDeclarationSyntax)
    {
        if (!typeSymbol.ContainingSymbol.Equals(typeSymbol.ContainingNamespace, SymbolEqualityComparer.Default))
            return "";

        var propertyName = "Value";
        var namespaceName = typeSymbol.ContainingNamespace.ToDisplayString();

        var kindSource = typeDeclarationSyntax.Kind() switch
        {
            SyntaxKind.ClassDeclaration => $"public sealed partial class {typeSymbol.Name}  : System.IEquatable<{typeSymbol.Name}>",
            SyntaxKind.RecordDeclaration => $"public sealed partial record {typeSymbol.Name}",
            SyntaxKind.StructDeclaration => $"public partial struct {typeSymbol.Name}  : System.IEquatable<{typeSymbol.Name}>",
            SyntaxKind.RecordStructDeclaration => $"public partial record struct {typeSymbol.Name} ",
            _ => throw new NotSupportedException("Unsupported type kind for generating Alias code")
        };

        var equalsOperatorsSource = $@"
                public static bool operator ==({typeSymbol.Name} left, {typeSymbol.Name} right) => Equals(left, right);
                public static bool operator !=({typeSymbol.Name} left, {typeSymbol.Name} right) => !Equals(left, right);
            ";

        var equalsSource = typeDeclarationSyntax.Kind() switch
        {
            SyntaxKind.ClassDeclaration => $@"
                {equalsOperatorsSource}
                public override bool Equals(object obj) => ReferenceEquals(this, obj) || obj is {typeSymbol.Name} other && Equals(other);
                public override int GetHashCode() => {propertyName}.GetHashCode();
                public bool Equals({typeSymbol.Name} other){{ return {propertyName} == other.{propertyName}; }}",
            SyntaxKind.RecordDeclaration => "",
            SyntaxKind.StructDeclaration => $@"
                {equalsOperatorsSource}
                public override bool Equals(object obj) => ReferenceEquals(this, obj) || obj is {typeSymbol.Name} other && Equals(other);
                public override int GetHashCode() => {propertyName}.GetHashCode();
                public bool Equals({typeSymbol.Name} other)
                {{
                    if (ReferenceEquals(null, other)) return false;
                    if (ReferenceEquals(this, other)) return true;
                    return {propertyName} == other.{propertyName};
                }}",
            SyntaxKind.RecordStructDeclaration => "",
            _ => throw new NotSupportedException("Unsupported type kind for generating Validated code")
        };

        var source = new StringBuilder($@"
namespace {namespaceName}
{{
    using static Radix.Control.Validated.Extensions;
    using Radix.Data;

    {kindSource}
    {{
        public static Validated<{typeSymbol.Name}> Create(string? value)
        {{
            if (value is null) return new Invalid<{{typeSymbol.Name}}>(""Value cannot be null."");
            var result = from validated in {validityType}.Parse(value, $""The value provided for {typeSymbol.Name}, '{{value}}',  is not valid"")
                         select new {typeSymbol.Name}(validated);
            return result;
        }}
        

        public {valueType} {propertyName} {{ get; }}
        private {typeSymbol.Name}({valueType} value)
        {{
            {propertyName} = value ?? throw new ArgumentNullException(nameof(value));
        }}

        public override string ToString() => {propertyName}.ToString();
        {equalsSource}
        public static implicit operator {valueType}({typeSymbol.Name} value) => value.{propertyName};
    }}
}}");
        return source.ToString();
    }
}
