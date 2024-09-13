using DeepEqualsGenerator.SourceGenerator.Framework;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace DeepEqualsGenerator.SourceGenerator;

[Generator(LanguageNames.CSharp)]
public class DeepEqualsIncrementalGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        //We run on all changes of 'Type' or 'Property' or 'Field'

        var symbols = context.SyntaxProvider.CreateSyntaxProvider(
            static (n, _) => n is TypeDeclarationSyntax,
            static (context, _) =>
            {
                var t = (TypeDeclarationSyntax)context.Node;
                return (ITypeSymbol?)context.SemanticModel.GetDeclaredSymbol(t);
            });
        
        context.RegisterSourceOutput(symbols.Collect(), static (context, source) =>
        {
            var log = new CompilationLogProvider(context);
            try
            {
                const string @interface = "global::DeepEqualsGenerator.IDeepEqualsContext";
                var contexts =
                    from t in source
                    where t != null
                          && t.Interfaces.Any(i => i.CSharpName() == @interface)
                    select t;
                
                var csharp = new DeepEqualsWriter().Generate(contexts.ToList());
                
                context.AddSource("DeepEqualsGenerated.g.cs", csharp);
            }
            catch (Exception e)
            {
                log.WriteError(Location.None, 
                    "DEG1000", 
                    "DeepEqualsGraphGenerator fatal error", 
                    "Deep equals generator encountered a fatal error: {0}", 
                    e);
            }
        });
    }
}