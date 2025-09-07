using System.Collections.Immutable;

namespace IKVM.CoreLib.Symbols
{

    /// <summary>
    /// Description of the API available on a type to calculate type names.
    /// </summary>
    /// <typeparam name="TName"></typeparam>
    interface ITypeSymbolNameProvider<TName> where TName : ITypeSymbolNameProvider<TName>
    {

        string? Name { get; }

        string Namespace { get; }

        string AssemblyFullName { get; }

        TName DeclaringType { get; }

        bool IsGenericType { get; }

        bool IsGenericTypeDefinition { get; }

        bool IsGenericParameter { get; }

        bool ContainsGenericParameters { get; }

        ImmutableArray<TName> GenericArguments { get; }

        bool HasElementType { get; }

        TName GetElementType();

        bool IsPointer { get; }

        bool IsByRef { get; }

        bool IsSZArray { get; }

        bool IsArray { get; }

        int GetArrayRank();

    }

}