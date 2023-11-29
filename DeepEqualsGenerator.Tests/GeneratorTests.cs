
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using DeepEqualsGenerator.SourceGenerator;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Shouldly;
using VerifyXunit;

// ReSharper disable ConvertNullableToShortForm

namespace DeepEqualsGenerator.Tests;

[UsesVerify]
public class GeneratorTests
{
    [Fact]
    public Task Generate()
    {
        var syntaxTree = CSharpSyntaxTree.ParseText(File.ReadAllText(Source));
        
        var compilation = CSharpCompilation.Create(
            assemblyName: "DeepEquals.GeneratorTests",
            references: GetReferences(typeof(GenerateDeepEqualsAttribute), typeof(List<int>)),
            syntaxTrees: new[] {syntaxTree},
            options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

        compilation.GetDiagnostics().ShouldBeEmpty();

        var generator = new DeepEqualsIncrementalGenerator();

        GeneratorDriver driver = CSharpGeneratorDriver.Create(generator);

        driver = driver.RunGenerators(compilation);

        return Verifier.Verify(driver);
    }
    
    private static readonly string Source = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Models/Customer.cs");
    
    private static IEnumerable<MetadataReference> GetReferences(params Type[] types)
    {
        var standard = new[]
        {
            Assembly.Load("netstandard, Version=2.0.0.0"),
            Assembly.Load("System.Runtime"),
            Assembly.Load("System.ObjectModel")
        };
        var custom = types.Select(t => t.Assembly);
            
        return standard.Concat(custom)
            .Distinct()
            .Select(a => MetadataReference.CreateFromFile(a.Location));
    }
}