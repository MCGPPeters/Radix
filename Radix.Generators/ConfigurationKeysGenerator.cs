﻿
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using System.Text;
using System.Diagnostics;

namespace Radix.Generators;


[Generator]
public class ConfigurationKeysGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        //Debugger.Launch();
        var declarations = context.SyntaxProvider
            .CreateSyntaxProvider(
                predicate: (syntaxNode, _) => syntaxNode is TypeDeclarationSyntax typeDecl && typeDecl.AttributeLists.Count > 0,
                transform: (syntaxContext, _) => (typeDecl: (TypeDeclarationSyntax)syntaxContext.Node, model: syntaxContext.SemanticModel))
            .Where(pair => HasConfigurationAttribute(pair.typeDecl, pair.model));

        var compilationAndDeclarations = context.CompilationProvider.Combine(declarations.Collect());

        context.RegisterSourceOutput(compilationAndDeclarations, static (sourceProductionContext, source) =>
        {
            var (compilation, declarations) = source;
            foreach (var (typeSyntax, model) in declarations.Distinct())
            {
                var className = typeSyntax.Identifier.Text;
                var typeSymbol = model.GetDeclaredSymbol(typeSyntax);
                var namespaceName = typeSymbol?.ContainingNamespace.ToDisplayString() ?? "global";
                var configurationKeysClassName = className + "ConfigurationKeys";
                var hintName = $"{configurationKeysClassName}_{Guid.NewGuid()}.g.cs";
                var sourceCode = GenerateConfigurationKeysClass(compilation, namespaceName, className, configurationKeysClassName, typeSyntax, model);
                sourceProductionContext.AddSource(hintName, SourceText.From(sourceCode, Encoding.UTF8));
            }
        });
    }

    private static bool HasConfigurationAttribute(TypeDeclarationSyntax typeSyntax, SemanticModel model)
    {
        var typeSymbol = model.GetDeclaredSymbol(typeSyntax);
        if (typeSymbol is not null)
        {
            return typeSymbol.GetAttributes().Any(ad => ad.AttributeClass is not null && ad.AttributeClass.ToDisplayString() == "Radix.Generators.Attributes.ConfigurationAttribute");
        }
        return false;
    }

    private static string GenerateConfigurationKeysClass(Compilation compilation, string namespaceName, string className, string configurationKeysClassName, TypeDeclarationSyntax classSyntax, SemanticModel model)
    {
        var stringBuilder = new StringBuilder();
        // Header comment indicating the code is auto-generated
        stringBuilder.AppendLine("// <auto-generated>");
        stringBuilder.AppendLine("// This code was generated by a tool.");
        stringBuilder.AppendLine("// Changes to this file may cause incorrect behavior and will be lost if");
        stringBuilder.AppendLine("// the code is regenerated.");
        stringBuilder.AppendLine("// </auto-generated>");
        stringBuilder.AppendLine();
        stringBuilder.AppendLine("using System;");
        stringBuilder.AppendLine("using System.Diagnostics;");
        stringBuilder.AppendLine("using System.CodeDom.Compiler;"); // Include namespace for GeneratedCodeAttribute
        stringBuilder.AppendLine();
        stringBuilder.AppendLine($"namespace {namespaceName}");
        stringBuilder.AppendLine("{");
        // Apply DebuggerNonUserCode and GeneratedCodeAttribute to the class
        stringBuilder.AppendLine("    [DebuggerNonUserCode]");
        stringBuilder.AppendLine("    [GeneratedCode(\"Radix.Generators.ConfigurationKeysGenerator\", \"1.0\")]"); // Adjust version as appropriate
        stringBuilder.AppendLine($"    /// <summary>");
        stringBuilder.AppendLine($"    /// Provides configuration key paths for the {className} class.");
        stringBuilder.AppendLine($"    /// </summary>");
        stringBuilder.AppendLine($"    public class {configurationKeysClassName}");
        stringBuilder.AppendLine("    {");

        var classSymbol = model.GetDeclaredSymbol(classSyntax);

        GenerateConfigurationKeysForType(stringBuilder, classSymbol!, className, []);

        stringBuilder.AppendLine("    }");
        stringBuilder.AppendLine("}");
        string v = stringBuilder.ToString();
        return v;
    }


    private static void GenerateConfigurationKeysForType(StringBuilder stringBuilder, INamedTypeSymbol classSymbol, string parentPath, HashSet<INamedTypeSymbol> processedTypes)
    {
        if (!processedTypes.Add(classSymbol))
        {
            // This type is already being processed, skip to avoid recursion
            return;
        }

        foreach (var property in classSymbol.GetMembers().OfType<IPropertySymbol>())
        {
            // Skip compiler-generated properties, like for records
            if (property.IsImplicitlyDeclared)
                continue;

            var propertyName = property.Name;
            var propertyPath = $"{parentPath}:{propertyName}";

            // Add property comment
            stringBuilder.AppendLine($"        /// <summary>");
            stringBuilder.AppendLine($"        /// Configuration key for {propertyName}.");
            stringBuilder.AppendLine($"        /// </summary>");
            stringBuilder.AppendLine($"        public static string {propertyPath.Replace(':', '_')} => \"{propertyPath}\";");

            if (property.Type.TypeKind == TypeKind.Class && property.Type.SpecialType != SpecialType.System_String)
            {
                GenerateConfigurationKeysForType(stringBuilder, (INamedTypeSymbol)property.Type, propertyPath, processedTypes);
            }
        }

        processedTypes.Remove(classSymbol);
    }

}
