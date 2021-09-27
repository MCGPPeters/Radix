﻿using System;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace Radix.Generators;

[Generator]
public class ConstrainedAliasGenerator : ISourceGenerator
{
    public void Execute(GeneratorExecutionContext context)
    {
        if (context.SyntaxReceiver is not SyntaxReceiver receiver) return;

        foreach (var candidate in receiver.CandidateTypes)
        {
            var model = context.Compilation.GetSemanticModel(candidate.SyntaxTree);
            var typeSymbol = ModelExtensions.GetDeclaredSymbol(model, candidate);
            var constrainedAttributeSymbol = context.Compilation.GetTypeByMetadataName("Radix.ConstrainedAliasAttribute`2");
            var constrainedAttributes = typeSymbol.GetAttributes().Where(attribute => attribute.AttributeClass.Name.Equals(constrainedAttributeSymbol.Name));
            foreach (var attribute in constrainedAttributes)
            {
                var classSource = ProcessConstrainedType(attribute.AttributeClass.TypeArguments[0].Name, attribute.AttributeClass.TypeArguments[1].Name, typeSymbol, constrainedAttributeSymbol, candidate);
                context.AddSource(
                    $"{typeSymbol.ContainingNamespace.ToDisplayString()}_{typeSymbol.Name}_constrained_alias",
                    SourceText.From(classSource, Encoding.UTF8));
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
        Debug.WriteLine("Initalize code generator");
    }

    internal static string ProcessConstrainedType(string valueType, string validatorType, ISymbol typeSymbol, ISymbol attributeSymbol, TypeDeclarationSyntax typeDeclarationSyntax)
    {
        if (!typeSymbol.ContainingSymbol.Equals(typeSymbol.ContainingNamespace, SymbolEqualityComparer.Default))
            return null;

        var propertyName = "Value";
        var namespaceName = typeSymbol.ContainingNamespace.ToDisplayString();
        string kindSource = NewMethod(typeSymbol, typeDeclarationSyntax);

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
            _ => throw new NotSupportedException("Unsupported type kind for generating ConstainedAlias code")
        };

        var source = new StringBuilder($@"
namespace {namespaceName}
{{
    {kindSource}
    {{
        public {valueType} {propertyName} {{ get; }}
        private {typeSymbol.Name}({valueType} value)
        {{
            {propertyName} = value;
        }}

        public static Validated<{typeSymbol.Name}>Create({valueType} value) => {validatorType}.Validate(value).Map<{valueType}, {typeSymbol.Name}>(v => new {typeSymbol.Name}(v)) ;

        public override string ToString() => {propertyName}.ToString();
        {equalsSource}

        public static implicit operator {valueType}({typeSymbol.Name} value) => value.{propertyName};
    }}
}}");
        return source.ToString();
    }

    private static string NewMethod(ISymbol typeSymbol, TypeDeclarationSyntax typeDeclarationSyntax) =>
            typeDeclarationSyntax.Kind() switch
            {
                SyntaxKind.ClassDeclaration => $"public sealed partial class {typeSymbol.Name}  : System.IEquatable<{typeSymbol.Name}>",
                SyntaxKind.RecordDeclaration => $"public sealed partial record {typeSymbol.Name}",
                SyntaxKind.StructDeclaration => $"public partial struct {typeSymbol.Name}  : System.IEquatable<{typeSymbol.Name}>",
                SyntaxKind.RecordStructDeclaration => $"public partial record struct {typeSymbol.Name}",
                _ => throw new NotSupportedException("Unsupported type kind for generating Alias code")
            };
}