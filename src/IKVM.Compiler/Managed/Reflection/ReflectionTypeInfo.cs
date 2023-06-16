using System;
using System.Collections.Generic;
using System.Reflection;

using IKVM.Compiler.Collections;

namespace IKVM.Compiler.Managed.Reflection
{

    /// <summary>
    /// Implements <see cref="IManagedTypeDefinition"/> by reflecting against an existing .NET type.
    /// </summary>
    internal sealed class ReflectionTypeInfo : ReflectionEntityInfo, IManagedTypeDefinition
    {

        readonly ReflectionModuleInfo module;
        readonly Type type;
        readonly ReadOnlyListMap<ReflectionFieldInfo, FieldInfo> fields;
        readonly ReadOnlyListMap<ReflectionMethodInfo, MethodInfo> methods;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="module"></param>
        /// <param name="type"></param>
        /// <exception cref="ArgumentNullException"></exception>
        internal ReflectionTypeInfo(ReflectionModuleInfo module, Type type) :
            base(module.Context)
        {
            this.module = module ?? throw new ArgumentNullException(nameof(module));
            this.type = type ?? throw new ArgumentNullException(nameof(type));

            fields = new ReadOnlyListMap<ReflectionFieldInfo, FieldInfo>(type.GetFields(), (f, i) => new ReflectionFieldInfo(this, f));
            methods = new ReadOnlyListMap<ReflectionMethodInfo, MethodInfo>(type.GetMethods(), (m, i) => new ReflectionMethodInfo(this, m));
        }

        public IManagedModuleDefinition Module => module;

        public string Name => type.FullName;

        public IReadOnlyList<IManagedFieldDefinition> Fields => fields;

        public IReadOnlyList<IManagedMethodDefinition> Methods => methods;

    }

}
