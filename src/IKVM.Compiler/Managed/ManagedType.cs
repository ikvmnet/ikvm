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

        bool load = true;
        ManagedTypeData data;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        public ManagedType(IManagedTypeContext context)
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
        public ReadOnlyFixedValueList<ManagedCustomAttribute> CustomAttributes
        {
            get
            {
                LazyLoad();
                return data.CustomAttributes;
            }
        }

        /// <summary>
        /// Gets the generic parameters on the managed type.
        /// </summary>
        public ReadOnlyFixedValueList<ManagedGenericParameter> GenericParameters
        {
            get
            {
                LazyLoad();
                return data.GenericParameters;
            }
        }

        /// <summary>
        /// Gets a reference to the base type.
        /// </summary>
        public ManagedTypeSignature? BaseType
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
        public ReadOnlyFixedValueList<ManagedInterface> Interfaces
        {
            get
            {
                LazyLoad();
                return data.Interfaces;
            }
        }

        /// <summary>
        /// Gets the set of fields declared on the managed type.
        /// </summary>
        public ReadOnlyFixedValueList<ManagedField> Fields
        {
            get
            {
                LazyLoad();
                return data.Fields;
            }
        }

        /// <summary>
        /// Gets the set of methods declared on the managed type.
        /// </summary>
        public ReadOnlyFixedValueList<ManagedMethod> Methods
        {
            get
            {
                LazyLoad();
                return data.Methods;
            }
        }

        /// <summary>
        /// Gets the set of nested types within the managed type.
        /// </summary>
        public ReadOnlyFixedValueList<ManagedType> NestedTypes
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
