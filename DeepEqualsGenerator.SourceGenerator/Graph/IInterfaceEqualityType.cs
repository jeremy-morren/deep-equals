﻿namespace DeepEqualsGenerator.SourceGenerator.Graph;

internal interface IInterfaceEqualityType : IEqualityType
{
    INamedTypeSymbol InterfaceType { get; }

    object ElementType { get; }
}