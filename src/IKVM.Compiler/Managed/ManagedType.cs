using System.Reflection;

using IKVM.Compiler.Collections;

namespace IKVM.Compiler.Managed
{

    /// <summary>
    /// Describes a managed type.
    /// </summary>
    public sealed class ManagedType
    {

        readonly IManagedTypeContext context;

        internal ManagedTypeData data;
        bool load = true;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        internal ManagedType(IManagedTypeContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// If necessary, loads the type.
        /// </summary>
        void LazyLoad()
        {
            // multiple threads may enter load at the same time, but this should be safe
            if (load)
            {
                data = context.LoadType(this);
                load = false;
            }
        }

        /// <summary>
        /// Gets the parent assembly of this type.
        /// </summary>
        public ManagedAssembly Assembly
        {
            get
            {
                LazyLoad();
                return data.Assembly;
            }
        }

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
        /// Gets the name of the managed type.
        /// </summary>
        public string Name
        {
            get
            {
                LazyLoad();
                return data.Name;
            }
        }

        /// <summary>
        /// Gets the attributes for the type.
        /// </summary>
        public TypeAttributes Attributes
        {
            get
            {
                LazyLoad();
                return data.Attributes;
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
        public ManagedInterfaceList Interfaces
        {
            get
            {
                LazyLoad();
                return new ManagedInterfaceList(this);
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
        public ReadOnlyFixedValueList1<ManagedType> NestedTypes
        {
            get
            {
                LazyLoad();
                return data.NestedTypes;
            }
        }

        /// <inhericdoc />
        public override string ToString() => Name;

    }

}
