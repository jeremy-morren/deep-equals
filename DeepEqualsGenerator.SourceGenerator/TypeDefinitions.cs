

// ReSharper disable StringLiteralTypo

// ReSharper disable TailRecursiveCall

namespace DeepEqualsGenerator.SourceGenerator;

internal static class TypeDefinitions
{
    public static string CSharpName(this ITypeSymbol type) =>
        type.ToDisplayString(NullableFlowState.None, SymbolDisplayFormat.FullyQualifiedFormat);
    
    public static bool IsNullable(this ITypeSymbol type, out ITypeSymbol underlyingType)
    {
        var name = type.ToDisplayString(NullableFlowState.None, SymbolDisplayFormat.MinimallyQualifiedFormat);
        if (name.EndsWith("?") && type is INamedTypeSymbol named)
        {
            underlyingType = named.TypeArguments[0];
            return true;
        }
        underlyingType = null!;
        return false;
    }

    public static bool IsPublic(this ISymbol symbol) =>
        symbol.DeclaredAccessibility is Accessibility.Public or Accessibility.ProtectedOrInternal;

    /// <summary>
    /// Checks if a type is a primitive type (including nullable)
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public static bool IsPrimitive(this ITypeSymbol type)
    {
        //NB: Special case for nullable primitives: we want to treat them as primitives, not complex types

        if (type.IsNullable(out var u)) return IsPrimitive(u);

        switch(type.SpecialType)
        {
            case SpecialType.System_Boolean 
                or SpecialType.System_Byte 
                or SpecialType.System_SByte 
                or SpecialType.System_Char
                or SpecialType.System_Decimal 
                or SpecialType.System_Double 
                or SpecialType.System_Single 
                or SpecialType.System_Int32
                or SpecialType.System_UInt32
                or SpecialType.System_Int64 
                or SpecialType.System_UInt64 
                or SpecialType.System_Int16
                or SpecialType.System_UInt16
                or SpecialType.System_String
                or SpecialType.System_DateTime:
                return true;
                
            default:
                    
                //Treat all enums as primitives
                if (type is INamedTypeSymbol { EnumUnderlyingType: not null })
                    return true;

                var name = type.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat);
                
                return name switch
                {
                    nameof(DateTime)
                        or nameof(DateTimeOffset) 
                        or nameof(TimeSpan) 
                        or "DateOnly" 
                        or "TimeOnly" => true,
                    
                    _ => false
                };
        }
    }
}