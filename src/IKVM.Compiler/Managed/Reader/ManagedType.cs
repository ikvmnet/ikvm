using System.Collections.Generic;
using System.Reflection;

namespace IKVM.Compiler.Managed.Reader
{

    /// <summary>
    /// Describes a managed type.
    /// </summary>
    /// <remarks>
    /// The backing data of this class is loaded on demand upon first access from the associated type context.
    /// </remarks>
    internal sealed class ManagedType
    {

        readonly ManagedAssembly assembly;
        readonly object handle;
        readonly string name;
        readonly TypeAttributes attributes;

        internal ManagedTypeData data;
        bool load = true;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="handle"></param>
        /// <param name="name"></param>
        /// <param name="attributes"></param>
        internal ManagedType(ManagedAssembly assembly, object handle, string name, TypeAttributes attributes)
        {
            this.assembly = assembly;
            this.handle = handle;
            this.name = name;
            this.attributes = attributes;
        }

        /// <summary>
        /// If necessary, loads the type.
        /// </summary>
        void LazyLoad()
        {
            // multiple threads may enter load at the same time, but this should be safe
            if (load)
            {
                assembly.Context.LoadType(this, out data);
                load = false;
            }
        }

        /// <summary>
        /// Gets the internal handle to the underlying type source.
        /// </summary>
        public object Handle => handle;

        /// <summary>
        /// Gets the parent assembly of this type.
        /// </summary>
        public ManagedAssembly Assembly => assembly;

        /// <summary>
        /// Gets the name of the managed type.
        /// </summary>
        public string Name => name;

        /// <summary>
        /// Gets the attributes for the type.
        /// </summary>
        public TypeAttributes Attributes => attributes;

        /// <summary>
        /// Gets the parent type of this type.
        /// </summary>
        public ManagedType? DeclaringType
        {
            get
            {
                LazyLoad();
                return data.DeclaringType;
            }
        }

        /// <summary>
        /// Gets the set of custom attributes applied to the type.
        /// </summary>
        public ManagedTypeCustomAttributeList CustomAttributes
        {
            get
            {
                LazyLoad();
                return new ManagedTypeCustomAttributeList(this);
            }
        }

        /// <summary>
        /// Gets the generic parameters on the managed type.
        /// </summary>
        public ManagedTypeGenericParameterList GenericParameters
        {
            get
            {
                LazyLoad();
                return new ManagedTypeGenericParameterList(this);
            }
        }

        /// <summary>
        /// Gets a reference to the base type.
        /// </summary>
        public ManagedSignature? BaseType
        {
            get
            {
                LazyLoad();
                return data.BaseType;
            }
        }

        /// <summary>
        /// Gets the set of interfaces implemented on the managed type.
        /// </summary>
        public ManagedTypeInterfaceList Interfaces
        {
            get
            {
                LazyLoad();
                return new ManagedTypeInterfaceList(this);
            }
        }

        /// <summary>
        /// Gets the set of fields declared on the managed type.
        /// </summary>
        public ManagedTypeFieldList Fields
        {
            get
            {
                LazyLoad();
                return new ManagedTypeFieldList(this);
            }
        }

        /// <summary>
        /// Gets the set of methods declared on the managed type.
        /// </summary>
        public ManagedTypeMethodList Methods
        {
            get
            {
                LazyLoad();
                return new ManagedTypeMethodList(this);
            }
        }

        /// <summary>
        /// Gets the set of properties declared on the managed type.
        /// </summary>
        public ManagedTypePropertyList Properties
        {
            get
            {
                LazyLoad();
                return new ManagedTypePropertyList(this);
            }
        }

        /// <summary>
        /// Gets the set of properties declared on the managed type.
        /// </summary>
        public ManagedTypeEventList Events
        {
            get
            {
                LazyLoad();
                return new ManagedTypeEventList(this);
            }
        }

        /// <summary>
        /// Gets the set of nested types within the managed type.
        /// </summary>
        public IEnumerable<ManagedType> ResolveNestedTypes() => assembly.Context.ResolveNestedTypes(this);

        /// <summary>
        /// Gets the nested type with the specified name.
        /// </summary>
        public ManagedType? ResolveNestedType(string name) => assembly.Context.ResolveNestedType(this, name);

        /// <inhericdoc />
        public override string ToString() => Name;

    }

}
