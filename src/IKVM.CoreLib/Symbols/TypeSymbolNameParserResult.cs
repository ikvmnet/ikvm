// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Immutable;
using System.Diagnostics;

namespace IKVM.CoreLib.Symbols
{

    [DebuggerDisplay("{AssemblyQualifiedName}")]
    struct TypeSymbolNameParserResult
    {

        readonly ReadOnlyMemory<char> _namespaceName;
        readonly ReadOnlyMemory<char> _name;
        readonly AssemblyIdentity? _assembly;
        readonly ImmutableArray<TypeSymbolNameParserResult> _genericTypeArguments;
        readonly int _rankOrModifier;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="namespaceName"></param>
        /// <param name="name"></param>
        /// <param name="genericTypeArguments"></param>
        /// <param name="rankOrModifier"></param>
        internal TypeSymbolNameParserResult(ReadOnlyMemory<char> namespaceName, ReadOnlyMemory<char> name, AssemblyIdentity? assembly, ImmutableArray<TypeSymbolNameParserResult> genericTypeArguments, int rankOrModifier = default)
        {
            _namespaceName = namespaceName;
            _name = name;
            _assembly = assembly;
            _genericTypeArguments = genericTypeArguments;
            _rankOrModifier = rankOrModifier;
        }

        public string AssemblyQualifiedName { get; }

    }

}