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

        if(context.SyntaxReceiver is not SyntaxReceiver receiver) return;

        foreach (var candidate in receiver.CandidateTypes)
        {
            var model = context.Compilation.GetSemanticModel(candidate.SyntaxTree);
            var typeSymbol = ModelExtensions.GetDeclaredSymbol(model, candidate);
            var attributeSymbol = context.Compilation.GetTypeByMetadataName("Radix.LiteralAttribute");
            var attributes = typeSymbol!.GetAttributes().Where(attribute => attribute.AttributeClass!.Name.Equals(attributeSymbol!.Name));
            foreach (var normalizedSourceCodeText in from attribute in attributes
                                                     let stringRepresentation = attribute.NamedArguments.Any() ? attribute.NamedArguments[0].Value.Value!.ToString() : ""
                                                     let classSource = ProcessType(typeSymbol, candidate, stringRepresentation)// fix text formating according to default ruleset
                                                     let normalizedSourceCodeText
                                                                         = CSharpSyntaxTree.ParseText(classSource).GetRoot().NormalizeWhitespace().GetText(Encoding.UTF8)
                                                     select normalizedSourceCodeText)
            {
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

    internal static string ProcessType(ISymbol typeSymbol, TypeDeclarationSyntax typeDeclarationSyntax, string stringRepresention)
    {
        if (!typeSymbol.ContainingSymbol.Equals(typeSymbol.ContainingNamespace, SymbolEqualityComparer.Default))
            return "";

        var namespaceName = typeSymbol.ContainingNamespace.ToDisplayString();

        var typeSymbolName = typeSymbol.ToDisplayParts().Last().ToString();

        var kindSource = typeDeclarationSyntax.Kind() switch
        {
            SyntaxKind.ClassDeclaration => $"public sealed partial class {typeSymbolName} : Literal<{typeSymbolName}>, System.IEquatable<{typeSymbolName}>",
            SyntaxKind.RecordDeclaration => $"public sealed partial record {typeSymbolName} : Literal<{typeSymbolName}>",
            SyntaxKind.StructDeclaration => $"public partial struct {typeSymbolName}  : Literal<{typeSymbolName}>, System.IEquatable<{typeSymbolName}>",
            SyntaxKind.RecordStructDeclaration => $"public partial record struct {typeSymbolName} : Literal<{typeSymbolName}>",
            _ => throw new NotSupportedException("Unsupported type kind for generating Literal code")
        };

        var equalsOperatorsSource = $@"
                public static bool operator ==({typeSymbol.ToDisplayString()} left, {typeSymbol.ToDisplayString()} right) => Equals(left, right);
                public static bool operator !=({typeSymbol.ToDisplayString()} left, {typeSymbol.ToDisplayString()} right) => !Equals(left, right);
            ";

        var equalsSource = typeDeclarationSyntax.Kind() switch
        {
            SyntaxKind.ClassDeclaration => $@"
                {equalsOperatorsSource}
                public override bool Equals(object obj) => obj is {typeSymbol.ToDisplayString()} other;
                public override int GetHashCode() => ""{typeSymbolName}"".GetHashCode();
                public bool Equals({typeSymbolName} other) => true;",
            SyntaxKind.RecordDeclaration => "",
            SyntaxKind.StructDeclaration => $@"
                {equalsOperatorsSource}
                public override bool Equals(object? obj) => obj is {typeSymbol.ToDisplayString()} other;
                public override int GetHashCode() => ""{typeSymbolName}"".GetHashCode();
                public bool Equals({typeSymbolName} other) => true;",
            SyntaxKind.RecordStructDeclaration => "",
            _ => throw new NotSupportedException("Unsupported type kind for generating Literal code")
        };

        var toString = string.IsNullOrEmpty(stringRepresention) ? typeSymbol.Name : stringRepresention;

        var source = new StringBuilder($@"
namespace {namespaceName}
{{
    {kindSource}
    {{
        public override string ToString() => ""{toString}"";
        {equalsSource}
        public static implicit operator string({typeSymbolName} value) => ""{toString}"";
        public static implicit operator {typeSymbolName}(string value) => value  == ""{toString}"" ? new() : throw new ArgumentException(""'value' is not assignable to '{typeSymbol.Name}'"");
        public static string Format() => ""{toString}"";
    }}
}}");
        return source.ToString();
    }
}
