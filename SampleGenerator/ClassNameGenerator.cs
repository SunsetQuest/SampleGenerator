using System;
using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace SampleGenerator;

// This was me following along on this tutorial: https://www.youtube.com/watch?v=KNUUkD9OoOQ

[Generator]
public class ClassNameGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var provider = context.SyntaxProvider.CreateSyntaxProvider(
            predicate: (c, _) => c is ClassDeclarationSyntax,
            transform: (n, _) => (ClassDeclarationSyntax)n.Node
            ).Where(m => m is not null);

        var compilation = context.CompilationProvider.Combine(provider.Collect());

        context.RegisterSourceOutput(compilation,
            (spc, source) => Execute(spc, source.Left, source.Right));

    }

    private void Execute(SourceProductionContext context, Compilation compilation, ImmutableArray<ClassDeclarationSyntax> typeList)
    {
        var code = """
            namespace SampleSouceGenerator
            {
                public static class ClassNames
                {
                    public static string Test = "Hello World";
                }
            }
            """;

        context.AddSource("ClassNames.g.cs", code);


    }
}
