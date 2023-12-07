// ReSharper disable All
#pragma warning disable CA1018 //Specify attribute usage

namespace System.Runtime.CompilerServices
{
    internal class IsExternalInit : Attribute {}

    internal class CompilerFeatureRequiredAttribute : Attribute
    {
        public CompilerFeatureRequiredAttribute(string name) { }
    }

    internal class RequiredMemberAttribute : Attribute {}
}

namespace System.Diagnostics.CodeAnalysis
{
    internal class SetsRequiredMembersAttribute : Attribute {}
}
