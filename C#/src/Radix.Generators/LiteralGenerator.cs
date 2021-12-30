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
            var attributeSymbol = context.Compilation.GetTypeByMetadataName("Radix.LiteralAttribute");
            var attributes = typeSymbol.GetAttributes().Where(attribute => attribute.AttributeClass.Name.Equals(attributeSymbol.Name));
            foreach (var _ in attributes)
            {
                var classSource = ProcessType(typeSymbol, candidate);
                // fix text formating according to default ruleset
                var normalizedSourceCodeText
                    = CSharpSyntaxTree.ParseText(classSource).GetRoot().NormalizeWhitespace().GetText(Encoding.UTF8);
                context.AddSource(
                    $"{typeSymbol.ContainingNamespace.ToDisplayString()}_{typeSymbol.Name}_literal",
                    normalizedSourceCodeText);
            }
        }
    }


    public void Initialize(GeneratorInitializationContext context)
    {
        context.RegisterForSyntaxNotifications(() => new SyntaxReceiver());
#if DEBUG
        if (!Debugger.IsAttached)
        {
            Debugger.Launch();
        }
#endif
        Debug.WriteLine("Initialize code generator");
    }

    internal static string ProcessType(ISymbol typeSymbol, TypeDeclarationSyntax typeDeclarationSyntax)
    {
        if (!typeSymbol.ContainingSymbol.Equals(typeSymbol.ContainingNamespace, SymbolEqualityComparer.Default))
            return null;

        var namespaceName = typeSymbol.ContainingNamespace.ToDisplayString();

        var kindSource = typeDeclarationSyntax.Kind() switch
        {
            SyntaxKind.ClassDeclaration => $"public sealed partial class {typeSymbol.Name}  : System.IEquatable<{typeSymbol.Name}>",
            SyntaxKind.RecordDeclaration => $"public sealed partial record {typeSymbol.Name}",
            SyntaxKind.StructDeclaration => $"public partial struct {typeSymbol.Name}  : System.IEquatable<{typeSymbol.Name}>",
            SyntaxKind.RecordStructDeclaration => $"public partial record struct {typeSymbol.Name} ",
            _ => throw new NotSupportedException("Unsupported type kind for generating Literal code")
        };

        var equalsOperatorsSource = $@"
                public static bool operator ==({typeSymbol.Name} left, {typeSymbol.Name} right) => Equals(left, right);
                public static bool operator !=({typeSymbol.Name} left, {typeSymbol.Name} right) => !Equals(left, right);
            ";

        var equalsSource = typeDeclarationSyntax.Kind() switch
        {
            SyntaxKind.ClassDeclaration => $@"
                {equalsOperatorsSource}
                public override bool Equals(object obj) => obj is {typeSymbol.Name} other;
                public override int GetHashCode() => ""{typeSymbol.Name}"".GetHashCode();
                public bool Equals({typeSymbol.Name} other) => true;",
            SyntaxKind.RecordDeclaration => "",
            SyntaxKind.StructDeclaration => $@"
                {equalsOperatorsSource}
                public override bool Equals(object? obj) => obj is {typeSymbol.Name} other;
                public override int GetHashCode() => ""{typeSymbol.Name}"".GetHashCode();
                public bool Equals({typeSymbol.Name} other) => true;",
            SyntaxKind.RecordStructDeclaration => "",
            _ => throw new NotSupportedException("Unsupported type kind for generating Literal code")
        };

        var source = new StringBuilder($@"
namespace {namespaceName}
{{
    {kindSource}
    {{
        public override string ToString() => ""{typeSymbol.Name}"";
        {equalsSource}
        public static implicit operator string({typeSymbol.Name} value) => ""{typeSymbol.Name}"";
        public static implicit operator {typeSymbol.Name}(string value) => value  == ""{typeSymbol.Name}"" ? new() : throw new ArgumentException(""'value' is not assignable to '{typeSymbol.Name}'"");

    }}
}}");
        return source.ToString();
    }
}
