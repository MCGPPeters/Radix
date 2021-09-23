using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Radix.Generators;

internal class SyntaxReceiver : ISyntaxReceiver
{
    internal IList<TypeDeclarationSyntax> CandidateTypes { get; } =
        new List<TypeDeclarationSyntax>();

    public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
    {
        if (syntaxNode is ClassDeclarationSyntax classDeclarationSyntax
            && classDeclarationSyntax.AttributeLists.Count > 0)
            CandidateTypes.Add(classDeclarationSyntax);

        if (syntaxNode is StructDeclarationSyntax structDeclarationSyntax
            && structDeclarationSyntax.AttributeLists.Count > 0)
            CandidateTypes.Add(structDeclarationSyntax);

        if (syntaxNode is RecordDeclarationSyntax recordDeclarationSyntax
            && recordDeclarationSyntax.AttributeLists.Count > 0)
            CandidateTypes.Add(recordDeclarationSyntax);
    }
}
