using JetBrains.Annotations;

namespace DeepEqualsGenerator;

/// <summary>
/// Specify that a deep equals method should be generated for the target type.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = true, Inherited = false)]
[PublicAPI, MeansImplicitUse(ImplicitUseTargetFlags.WithMembers)]
public sealed class GenerateDeepEqualsAttribute : Attribute
{
    /// <summary>
    /// The type to generate deep equals method for, or null if the target type should be used.
    /// </summary>
    public Type Type { get; }

    /// <summary>
    /// Specify that a deep equals method should be generated for <paramref name="type"/>
    /// </summary>
    /// <param name="type">The type to generate deep equals method for</param>
    public GenerateDeepEqualsAttribute(Type type)
    {
        Type = type;
    }
}