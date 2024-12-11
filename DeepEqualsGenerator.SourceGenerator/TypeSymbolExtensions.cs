

// ReSharper disable StringLiteralTypo

// ReSharper disable TailRecursiveCall

namespace DeepEqualsGenerator.SourceGenerator;

internal static class TypeSymbolExtensions
{
    public static string CSharpName(this ITypeSymbol type) =>
        type.ToDisplayString(NullableFlowState.None, SymbolDisplayFormat.FullyQualifiedFormat);
    
    /// <summary>
    /// Returns true if the type is <see cref="Nullable{T}"/>
    /// </summary>
    public static bool IsNullable(this ITypeSymbol type, out ITypeSymbol underlyingType)
    {
        if (type is INamedTypeSymbol { IsGenericType: true, ConstructedFrom.SpecialType: SpecialType.System_Nullable_T } nullable)
        {
            underlyingType = nullable.TypeArguments[0];
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
                or SpecialType.System_DateTime
                or SpecialType.System_UIntPtr
                or SpecialType.System_IntPtr
                or SpecialType.System_Enum: //All Enums are primitives
                return true;
                
            default:

                if (type is INamedTypeSymbol { EnumUnderlyingType: not null })
                    return true;
                
                var name = type.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat);
                
                return name switch
                {
                    nameof(DateTime)
                        or nameof(DateTimeOffset) 
                        or nameof(TimeSpan)
                        or nameof(IntPtr)
                        or nameof(UIntPtr)
                        or "DateOnly" 
                        or "TimeOnly" => true,
                    
                    _ => false
                };
        }
    }
}