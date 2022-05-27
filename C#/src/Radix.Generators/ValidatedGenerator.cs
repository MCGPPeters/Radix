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
public class ValidatedGenerator : ISourceGenerator
{
    public void Execute(GeneratorExecutionContext context)
    {
        if (context.SyntaxReceiver is not SyntaxReceiver receiver) return;

        foreach (var candidate in receiver.CandidateTypes)
        {
            var model = context.Compilation.GetSemanticModel(candidate.SyntaxTree);
            var typeSymbol = ModelExtensions.GetDeclaredSymbol(model, candidate);
            var attributeSymbol = context.Compilation.GetTypeByMetadataName("Radix.ValidatedAttribute`2");
            var attributes = typeSymbol.GetAttributes().Where(attribute => attribute.AttributeClass.Name.Equals(attributeSymbol.Name));
            foreach (var attribute in attributes)
            {
                Console.WriteLine($"{attribute.AttributeClass.TypeArguments[1].ContainingNamespace.Name}.{attribute.AttributeClass.TypeArguments[1].Name}");
                var classSource = ProcessType(attribute.AttributeClass.TypeArguments[0].Name, $"{attribute.AttributeClass.TypeArguments[1].ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)}" , typeSymbol, candidate);
                // fix text formating according to default ruleset
                var normalizedSourceCodeText
                    = CSharpSyntaxTree.ParseText(classSource).GetRoot().NormalizeWhitespace().GetText(Encoding.UTF8);
                context.AddSource(
                    $"Validated{typeSymbol.ContainingNamespace.ToDisplayString()}_{typeSymbol.Name}",
                   normalizedSourceCodeText);
            }
        }
    }


    public void Initialize(GeneratorInitializationContext context)
    {
        context.RegisterForSyntaxNotifications(() => new SyntaxReceiver());

        //if (!Debugger.IsAttached)
        //{
        //    Debugger.Launch();
        //}
        Debug.WriteLine("Initalize code generator");
    }

    internal static string ProcessType(string valueType, string validityType, ISymbol typeSymbol, TypeDeclarationSyntax typeDeclarationSyntax)
    {
        if (!typeSymbol.ContainingSymbol.Equals(typeSymbol.ContainingNamespace, SymbolEqualityComparer.Default))
            return null;

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
        public static Validated<{typeSymbol.Name}> Create({valueType} value)
        {{
            var result = from validated in {validityType}.Validate(""{typeSymbol.Name}"", value)
                         select new {typeSymbol.Name}(validated);
            return result;
        }}
        

        public {valueType} {propertyName} {{ get; }}

        private {typeSymbol.Name}({valueType} value)
        {{
            {propertyName} = value;
        }}

        public override string ToString() => {propertyName}.ToString();

        {equalsSource}

        public static implicit operator {valueType}({typeSymbol.Name} value) => value.{propertyName};
    }}
}}");
        return source.ToString();
    }
}
