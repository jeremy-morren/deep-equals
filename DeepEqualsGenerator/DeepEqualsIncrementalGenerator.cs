using DeepEqualsGenerator.Framework;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace DeepEqualsGenerator;

public class DeepEqualsIncrementalGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        //We run on all changes of 'Type' or 'Property' or 'Field'

        var symbols = context.SyntaxProvider.CreateSyntaxProvider(
            static (n, _) => n is TypeDeclarationSyntax or PropertyDeclarationSyntax or FieldDeclarationSyntax,
            static (n, _) =>
            {
                return n.Node switch
                {
                    TypeDeclarationSyntax t => (ITypeSymbol?)n.SemanticModel.GetDeclaredSymbol(t),
                    PropertyDeclarationSyntax p => n.SemanticModel.GetDeclaredSymbol(p)?.ContainingType,
                    FieldDeclarationSyntax f => n.SemanticModel.GetDeclaredSymbol(f)?.ContainingType,
                    _ => throw new NotImplementedException()
                };
            });

        symbols = symbols.Where(t => t != null && t.HasDeepEqualsAttribute(out _));
        
        context.RegisterSourceOutput(symbols.Collect(), static (context, source) =>
        {
            var log = new CompilationLogProvider(context);
            try
            {
                var types = new HashSet<ITypeSymbol>(SymbolEqualityComparer.Default);
                foreach (var t in source)
                {
                    if (t == null || !t.HasDeepEqualsAttribute(out var args))
                        continue;
                    foreach (var a in args)
                        types.Add(a);
                }
                
                var csharp = new DeepEqualsWriter().Generate(types.ToList());

                context.AddSource("DeepEqualsGenerated.g.cs", csharp);
            }
            catch (Exception e)
            {
                log.WriteError(Location.None, 
                    "DEG1000", 
                    "DeepEqualsGraphGenerator fatal error", 
                    "Deep equals graph generator encountered a fatal error: {0}", 
                    e);
            }
        });
    }
}