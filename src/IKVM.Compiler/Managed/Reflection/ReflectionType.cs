using System;
using System.Collections.Generic;
using System.Reflection;

using IKVM.Compiler.Collections;

namespace IKVM.Compiler.Managed.Reflection
{

    /// <summary>
    /// Implements <see cref="IManagedType"/> by reflecting against an existing .NET type.
    /// </summary>
    internal sealed class ReflectionType : ReflectionBase, IManagedType
    {

        readonly ReflectionAssembly assembly;
        readonly Type type;
        readonly ReadOnlyListMap<ReflectionCustomAttribute, CustomAttributeData> customAttributes;
        readonly ReadOnlyListMap<ReflectionField, FieldInfo> fields;
        readonly ReadOnlyListMap<ReflectionMethod, MethodInfo> methods;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="type"></param>
        /// <exception cref="ArgumentNullException"></exception>
        internal ReflectionType(ReflectionAssembly assembly, Type type) :
            base(assembly.Context)
        {
            this.assembly = assembly;
            this.type = type;

            customAttributes = new ReadOnlyListMap<ReflectionCustomAttribute, CustomAttributeData>( type.GetCustomAttributesData().AsReadOnly(), (a, i) => new ReflectionCustomAttribute(this, a));
            fields = new ReadOnlyListMap<ReflectionField, FieldInfo>(type.GetFields(), (f, i) => new ReflectionField(this, f));
            methods = new ReadOnlyListMap<ReflectionMethod, MethodInfo>(type.GetMethods(), (m, i) => new ReflectionMethod(this, m));
        }

        public string Name => type.FullName!;

        /// <inheritdoc />
        public TypeAttributes Attributes => type.Attributes;

        public IReadOnlyList<IManagedCustomAttribute> CustomAttributes => customAttributes;

        public IReadOnlyList<IManagedField> Fields => fields;

        public IReadOnlyList<IManagedMethod> Methods => methods;

        /// <inheritdoc />
        public IReadOnlyList<IManagedType> NestedTypes => nestedTypes;

    }

}
