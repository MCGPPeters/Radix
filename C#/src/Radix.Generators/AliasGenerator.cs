﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace Radix.Generators;

[Generator]
public class AliasGenerator : ISourceGenerator
{
    public void Execute(GeneratorExecutionContext context)
    {
        if (!(context.SyntaxReceiver is SyntaxReceiver receiver)) return;

        foreach (var candidate in receiver.CandidateTypes)
        {
            var model = context.Compilation.GetSemanticModel(candidate.typeDeclarationSyntax.SyntaxTree);
            var typeSymbol = ModelExtensions.GetDeclaredSymbol(model, candidate.typeDeclarationSyntax);
            var attributeSymbol = context.Compilation.GetTypeByMetadataName("Radix.AliasAttribute`1");
            var attributes = typeSymbol.GetAttributes().Where(attribute => attribute.AttributeClass.Name.Equals(attributeSymbol.Name));
            foreach (var attribute in attributes)
            {
                var classSource = ProcessType(attribute.AttributeClass.TypeArguments.First().Name, typeSymbol, attributeSymbol, candidate.isStruct);
                context.AddSource(
                    $"{typeSymbol.ContainingNamespace.ToDisplayString()}_{typeSymbol.Name}_alias",
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

    internal static string ProcessType(string valueType, ISymbol typeSymbol, ISymbol attributeSymbol,
            bool isStruct)
    {
        if (!typeSymbol.ContainingSymbol.Equals(typeSymbol.ContainingNamespace, SymbolEqualityComparer.Default))
            return null;

        var defaultPropertyName = "Value";
        var namespaceName = typeSymbol.ContainingNamespace.ToDisplayString();
        var attributeData = typeSymbol.GetAttributes().Single(attribute => attribute.AttributeClass.Name.Equals(attributeSymbol.Name));
        var overridenNameOpt =
            attributeData.NamedArguments.SingleOrDefault(kvp => kvp.Key == "PropertyName").Value;
        var propertyName = overridenNameOpt.IsNull ? defaultPropertyName : overridenNameOpt.Value.ToString();
        var source = new StringBuilder($@"
namespace {namespaceName}
{{
    public {(isStruct ? "" : "sealed")} partial {(isStruct ? "struct" : "class")} {typeSymbol.Name} : System.IEquatable<{typeSymbol.Name}>
    {{
        public {valueType} {propertyName} {{ get; }}
        public {typeSymbol.Name}({valueType} value)
        {{
            {propertyName} = value;
        }}
        public override bool Equals(object obj) => ReferenceEquals(this, obj) || obj is {typeSymbol.Name} other && Equals(other);
        public override int GetHashCode() => {propertyName}.GetHashCode();
        public override string ToString() => {propertyName}.ToString();
        public static bool operator ==({typeSymbol.Name} left, {typeSymbol.Name} right) => Equals(left, right);
        public static bool operator !=({typeSymbol.Name} left, {typeSymbol.Name} right) => !Equals(left, right);
        public bool Equals({typeSymbol.Name} other)
        {{
            {(isStruct ? "" : "if (ReferenceEquals(null, other)) return false;")}
            {(isStruct ? "" : "if (ReferenceEquals(this, other)) return true;")}
            return EqualityComparer<{valueType}>.Default.Equals({propertyName}, other.{propertyName});
        }}
        public static explicit operator {typeSymbol.Name}({valueType} value) => new {typeSymbol.Name}(value);
        public static implicit operator {valueType}({typeSymbol.Name} value) => value.{propertyName};
    }}
}}");
        return source.ToString();
    }
}

internal class SyntaxReceiver : ISyntaxReceiver
{
    internal IList<(TypeDeclarationSyntax typeDeclarationSyntax, bool isStruct)> CandidateTypes { get; } =
        new List<(TypeDeclarationSyntax, bool)>();

    public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
    {
        if (syntaxNode is ClassDeclarationSyntax classDeclarationSyntax
            && classDeclarationSyntax.AttributeLists.Count > 0)
            CandidateTypes.Add((classDeclarationSyntax, false));

        if (syntaxNode is StructDeclarationSyntax structDeclarationSyntax
            && structDeclarationSyntax.AttributeLists.Count > 0)
            CandidateTypes.Add((structDeclarationSyntax, true));

        if (syntaxNode is RecordDeclarationSyntax recordDeclarationSyntax
            && recordDeclarationSyntax.AttributeLists.Count > 0)
            CandidateTypes.Add((recordDeclarationSyntax, true));
    }
}
