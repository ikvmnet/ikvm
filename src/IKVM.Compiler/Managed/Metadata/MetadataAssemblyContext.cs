using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Reflection;
using System.Reflection.Metadata;
using System.Text;

using IKVM.Compiler.Collections;

namespace IKVM.Compiler.Managed.Metadata
{

    /// <summary>
    /// Represents an assembly loaded from System.Reflection.Metadata.
    /// </summary>
    internal class MetadataAssemblyContext : IManagedAssemblyContext
    {

        readonly IManagedAssemblyResolver resolver;
        readonly IMetadataAssemblyFileResolver? files;
        readonly MetadataReader primary;
        readonly ManagedAssembly assembly;
        readonly MetadataSignatureTypeProvider signatureTypeProvider;

        readonly ConcurrentDictionary<string, TypeDefinitionHandle?> typeNameCache = new ConcurrentDictionary<string, TypeDefinitionHandle?>();
        readonly ConcurrentDictionary<TypeDefinitionHandle, ManagedType> typeDefsCache = new ConcurrentDictionary<TypeDefinitionHandle, ManagedType>();

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="primary"></param>
        /// <param name="resolver"></param>
        /// <param name="files"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public MetadataAssemblyContext(IManagedAssemblyResolver resolver, MetadataReader primary, IMetadataAssemblyFileResolver? files = null)
        {
            this.resolver = resolver;
            this.primary = primary;
            this.files = files;

            // initialize instance by loading the primary reader
            signatureTypeProvider = new MetadataSignatureTypeProvider(this);
            assembly = LoadAssembly();
        }

        /// <summary>
        /// Loads a managed assembly from the specified reader.
        /// </summary>
        /// <param name="primary"></param>
        /// <returns></returns>
        ManagedAssembly LoadAssembly()
        {
            var d = primary.GetAssemblyDefinition();

            return new ManagedAssembly(this,
                primary.GetString(d.Name),
                LoadCustomAttributes(primary, d.GetCustomAttributes()),
                primary.GetBlobBytes(d.PublicKey),
                d.Version,
                primary.GetString(d.Culture),
                this);
        }

        /// <summary>
        /// Gets the <see cref="MetadataReader"/> associated with the context.
        /// </summary>
        public MetadataReader Primary => primary;

        /// <summary>
        /// Gets the assembly that defines this context.
        /// </summary>
        public ManagedAssembly Assembly => assembly;

        /// <summary>
        /// Gets the <see cref="ISignatureTypeProvider{TType, TGenericContext}"/> for the current context.
        /// </summary>
        public MetadataSignatureTypeProvider SignatureTypeProvider => signatureTypeProvider;

        /// <summary>
        /// Resolves the type from the specified assembly.
        /// </summary>
        /// <param name="typeName"></param>
        /// <returns></returns>
        ManagedType? IManagedAssemblyTypeResolver.Resolve(AssemblyName assemblyName, string typeName) => primary.GetAssemblyDefinition().GetAssemblyName() == assemblyName ? ResolveType(typeName) : null;

        /// <summary>
        /// Gets the set of all types from the loaded assembly.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        IEnumerator<ManagedType> IEnumerable<ManagedType>.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the set of all types from the loaded assembly.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Resolves the named type from this assembly.
        /// </summary>
        /// <param name="typeName"></param>
        /// <returns></returns>
        ManagedType? ResolveType(string typeName)
        {
            var handle = typeNameCache.GetOrAdd(typeName, FindTypeHandle);
            if (handle == null)
                return null;

            return ResolveType(primary, handle.Value);
        }

        /// <summary>
        /// Resolves the specified type from this assembly.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="handle"></param>
        /// <returns></returns>
        ManagedType? ResolveType(MetadataReader reader, TypeDefinitionHandle handle)
        {
            var type = typeDefsCache.GetOrAdd(handle, _ => LoadType(reader, _));
            if (type == null)
                return null;

            return type;
        }

        /// <summary>
        /// Resolves the type definition with the specified name.
        /// </summary>
        /// <param name="typeName"></param>
        /// <returns></returns>
        TypeDefinitionHandle? FindTypeHandle(string typeName)
        {
            var l = primary.TypeDefinitions;
            var e = l.GetEnumerator();

            for (int i = 0; i < l.Count; i++)
            {
                e.MoveNext();
                var t = primary.GetTypeDefinition(e.Current);
                var n = GetTypeName(t);
                typeNameCache.TryAdd(n, e.Current); // preemptively fill cache

                if (n == typeName)
                    return e.Current;
            }

            return null;
        }

        /// <summary>
        /// Loads the types from the given collection.
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        ReadOnlyValueList<ManagedType> ResolveTypes(MetadataReader reader, TypeDefinitionHandleCollection handles)
        {
            var l = new ManagedType[handles.Count];
            var e = handles.GetEnumerator();

            for (int i = 0; i < handles.Count; i++)
            {
                e.MoveNext();
                l[i] = ResolveType(reader, e.Current) ?? throw new InvalidOperationException();
            }

            return new ReadOnlyValueList<ManagedType>(l);
        }

        /// <summary>
        /// Loads the types from the given collection.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="handles"></param>
        /// <returns></returns>
        ReadOnlyValueList<ManagedType> ResolveTypes(MetadataReader reader, ImmutableArray<TypeDefinitionHandle> handles)
        {
            var l = new ManagedType[handles.Length];
            var e = handles.GetEnumerator();

            for (int i = 0; i < handles.Length; i++)
            {
                e.MoveNext();
                l[i] = ResolveType(reader, e.Current) ?? throw new InvalidOperationException();
            }

            return new ReadOnlyValueList<ManagedType>(l);
        }

        /// <summary>
        /// Loads a <see cref="ManagedType"/> from the given <see cref="TypeDefinitionHandle"/>.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        ManagedType LoadType(MetadataReader reader, TypeDefinitionHandle type)
        {
            var t = reader.GetTypeDefinition(type);

            return new ManagedType(
                ResolveDeclaringType(reader, type),
                GetTypeName(t),
                t.Attributes,
                LoadCustomAttributes(reader, t.GetCustomAttributes()),
                LoadFields(reader, t.GetFields()),
                LoadMethods(reader, t.GetMethods()),
                () => ResolveTypes(reader, t.GetNestedTypes()));
        }

        /// <summary>
        /// Gets a type reference to the declaring type of a type definition.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        ManagedType? ResolveDeclaringType(MetadataReader reader, TypeDefinitionHandle type)
        {
            var declaringTypeHandle = reader.GetTypeDefinition(type).GetDeclaringType();
            if (declaringTypeHandle.IsNil)
                return null;

            return ResolveType(reader, declaringTypeHandle);
        }

        /// <summary>
        /// Loads the custom attributes from the given collection.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="handles"></param>
        /// <returns></returns>
        ReadOnlyValueList<ManagedCustomAttribute> LoadCustomAttributes(MetadataReader reader, CustomAttributeHandleCollection handles)
        {
            var l = new ManagedCustomAttribute[handles.Count];
            var e = handles.GetEnumerator();

            for (int i = 0; i < handles.Count; i++)
            {
                e.MoveNext();
                l[i] = LoadCustomAttribute(reader, e.Current);
            }

            return new ReadOnlyValueList<ManagedCustomAttribute>(l);
        }

        /// <summary>
        /// Loads the given custom attribute.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="handle"></param>
        /// <returns></returns>
        ManagedCustomAttribute LoadCustomAttribute(MetadataReader reader, CustomAttributeHandle handle)
        {
            var a = reader.GetCustomAttribute(handle);
            return new ManagedCustomAttribute();
        }

        /// <summary>
        /// Loads the fields from the given collection.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="handles"></param>
        /// <returns></returns>
        ReadOnlyValueList<ManagedField> LoadFields(MetadataReader reader, FieldDefinitionHandleCollection handles)
        {
            var l = new ManagedField[handles.Count];
            var e = handles.GetEnumerator();

            for (int i = 0; i < handles.Count; i++)
            {
                e.MoveNext();
                l[i] = LoadField(reader, e.Current);
            }

            return new ReadOnlyValueList<ManagedField>(l);
        }

        /// <summary>
        /// Loads the given field.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="handle"></param>
        /// <returns></returns>
        ManagedField LoadField(MetadataReader reader, FieldDefinitionHandle handle)
        {
            var a = reader.GetFieldDefinition(handle);
            return new ManagedField();
        }

        /// <summary>
        /// Loads the fields from the given collection.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="handles"></param>
        /// <returns></returns>
        ReadOnlyValueList<ManagedMethod> LoadMethods(MetadataReader reader, MethodDefinitionHandleCollection handles)
        {
            var l = new ManagedMethod[handles.Count];
            var e = handles.GetEnumerator();

            for (int i = 0; i < handles.Count; i++)
            {
                e.MoveNext();
                l[i] = LoadMethod(reader, e.Current);
            }

            return new ReadOnlyValueList<ManagedMethod>(l);
        }

        /// <summary>
        /// Loads the given method.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="handle"></param>
        /// <returns></returns>
        ManagedMethod LoadMethod(MetadataReader reader, MethodDefinitionHandle handle)
        {
            var a = reader.GetMethodDefinition(handle);
            return new ManagedMethod();
        }

        /// <summary>
        /// Generates a full type name string for the given type reference.
        /// </summary>
        /// <param name="reference"></param>
        /// <returns></returns>
        string GetTypeName(TypeReference reference)
        {
            var f = new StringBuilder(reference.Namespace.IsNil ? "" : primary.GetString(reference.Namespace), 32);
            if (f.Length > 0)
                f.Append(".");
            f.Append(primary.GetString(reference.Name));
            return f.ToString();
        }

        /// <summary>
        /// Generates a full type name string for the given type definition.
        /// </summary>
        /// <param name="definition"></param>
        /// <returns></returns>
        string GetTypeName(TypeDefinition definition)
        {
            var f = new StringBuilder(definition.Namespace.IsNil ? "" : primary.GetString(definition.Namespace), 32);
            if (f.Length > 0)
                f.Append(".");
            f.Append(primary.GetString(definition.Name));
            return f.ToString();
        }
    }

}
