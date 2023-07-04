using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
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

        readonly IMetadataReaderAssemblyFileLoader? files;
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
        public MetadataAssemblyContext(MetadataReader primary, IMetadataReaderAssemblyFileLoader? files = null)
        {
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
            var assemblyDef = primary.GetAssemblyDefinition();
            return new ManagedAssembly(this, assemblyDef.GetAssemblyName(), LoadCustomAttributes(primary, assemblyDef.GetCustomAttributes()));
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
        /// Implements the IManagedAssemblyContext.ResolveTypes method.
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="typeName"></param>
        /// <returns></returns>
        ManagedType? IManagedAssemblyContext.ResolveType(ManagedAssembly assembly, string typeName) => ResolveType(typeName);

        /// <summary>
        /// Implements the IManagedAssemblyContext.ResolveTypes method.
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        IEnumerable<ManagedType> IManagedAssemblyContext.ResolveTypes(ManagedAssembly assembly) => ResolveTypes();

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
        internal ManagedType ResolveType(MetadataReader reader, TypeDefinitionHandle handle) => typeDefsCache.GetOrAdd(handle, _ => LoadType(reader, _));

        /// <summary>
        /// Resolves the types from the assembly assembly.
        /// </summary>
        /// <param name="handle"></param>
        /// <returns></returns>
        internal IEnumerable<ManagedType> ResolveNestedTypes(MetadataReader reader, TypeDefinitionHandle handle) => ResolveTypes(reader, reader.GetTypeDefinition(handle).GetNestedTypes());

        /// <summary>
        /// Resolves the type reference from teh assembly context.
        /// </summary>
        /// <param name="handle"></param>
        /// <returns></returns>
        internal ManagedTypeRef ResolveTypeReference(MetadataReader reader, EntityHandle handle) => handle.Kind switch
        {
            HandleKind.TypeReference => ResolveTypeReference(reader, (TypeReferenceHandle)handle),
            HandleKind.TypeDefinition => ResolveTypeReference(reader, (TypeDefinitionHandle)handle),
            HandleKind.TypeSpecification => ResolveTypeReference(reader, (TypeSpecificationHandle)handle),
            _ => throw new InvalidOperationException(),
        };

        /// <summary>
        /// Resolves the type reference from teh assembly context.
        /// </summary>
        /// <param name="handle"></param>
        /// <returns></returns>
        internal ManagedTypeRef ResolveTypeReference(MetadataReader reader, TypeDefinitionHandle handle)
        {
            var type = reader.GetTypeDefinition(handle);
            return new ManagedTypeRef(this, assembly.Name, GetTypeName(reader, type), null);
        }

        /// <summary>
        /// Resolves the type reference from teh assembly context.
        /// </summary>
        /// <param name="handle"></param>
        /// <returns></returns>
        internal ManagedTypeRef ResolveTypeReference(MetadataReader reader, TypeSpecificationHandle handle)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Resolves the specified type from this assembly.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="handle"></param>
        /// <returns></returns>
        ManagedTypeRef ResolveTypeReference(MetadataReader reader, TypeReferenceHandle handle)
        {
            var typeReference = reader.GetTypeReference(handle);
            var typeName = GetTypeName(reader, typeReference);

            // type reference is a type nested under a parent type
            if (typeReference.ResolutionScope.Kind == HandleKind.TypeReference)
            {
                var declaringTypeHandle = (TypeReferenceHandle)typeReference.ResolutionScope;

                return new ManagedTypeRef(this, ResolveAssemblyName(reader, declaringTypeHandle), typeName);
            }

            // type reference is nested under a type in another assembly
            if (typeReference.ResolutionScope.Kind == HandleKind.AssemblyReference)
            {
                var assemblyReferenceHandle = (AssemblyReferenceHandle)typeReference.ResolutionScope;
                var assemblyReference = reader.GetAssemblyReference(assemblyReferenceHandle);
                var assemblyName = assemblyReference.GetAssemblyName();

                return new ManagedTypeRef(this, assemblyName, typeName);
            }

            // type lives in the same assembly, resolve by string
            return new ManagedTypeRef(this, Assembly.Name, typeName, ResolveType(typeName));
        }

        /// <summary>
        /// Resolves the assembly name of the given type reference.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="handle"></param>
        /// <returns></returns>
        AssemblyName ResolveAssemblyName(MetadataReader reader, TypeReferenceHandle handle)
        {
            var typeReference = reader.GetTypeReference(handle);

            if (typeReference.ResolutionScope.Kind == HandleKind.TypeReference)
                return ResolveAssemblyName(reader, (TypeReferenceHandle)typeReference.ResolutionScope);

            if (typeReference.ResolutionScope.Kind == HandleKind.AssemblyReference)
                return reader.GetAssemblyReference(((AssemblyReferenceHandle)typeReference.ResolutionScope)).GetAssemblyName();

            return assembly.Name;
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
                var n = GetTypeName(primary, t);
                typeNameCache.TryAdd(n, e.Current); // preemptively fill cache

                if (n == typeName)
                    return e.Current;
            }

            return null;
        }

        /// <summary>
        /// Lodas the types from the specified assembly.
        /// </summary>
        /// <returns></returns>
        ReadOnlyFixedValueList<ManagedType> ResolveTypes()
        {
            return ResolveTypes(primary, primary.TypeDefinitions);
        }

        /// <summary>
        /// Loads the types from the given collection.
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        ReadOnlyFixedValueList<ManagedType> ResolveTypes(MetadataReader reader, TypeDefinitionHandleCollection handles)
        {
            var l = new FixedValueList<ManagedType>(handles.Count);
            var e = handles.GetEnumerator();

            for (int i = 0; i < handles.Count; i++)
            {
                e.MoveNext();
                l[i] = ResolveType(reader, e.Current) ?? throw new InvalidOperationException();
            }

            return l.AsReadOnly();
        }

        /// <summary>
        /// Loads the types from the given collection.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="handles"></param>
        /// <returns></returns>
        ReadOnlyFixedValueList<ManagedType> ResolveTypes(MetadataReader reader, ImmutableArray<TypeDefinitionHandle> handles)
        {
            var l = new FixedValueList<ManagedType>(handles.Length);
            var e = handles.GetEnumerator();

            for (int i = 0; i < handles.Length; i++)
            {
                e.MoveNext();
                l[i] = ResolveType(reader, e.Current) ?? throw new InvalidOperationException();
            }

            return l.AsReadOnly();
        }

        /// <summary>
        /// Loads the custom attributes from the given collection.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="handles"></param>
        /// <returns></returns>
        ReadOnlyFixedValueList<ManagedCustomAttribute> LoadCustomAttributes(MetadataReader reader, CustomAttributeHandleCollection handles)
        {
            var l = new FixedValueList<ManagedCustomAttribute>(handles.Count);
            var e = handles.GetEnumerator();

            for (int i = 0; i < handles.Count; i++)
            {
                e.MoveNext();
                l[i] = LoadCustomAttribute(reader, e.Current);
            }

            return l.AsReadOnly();
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
        /// Loads a <see cref="ManagedType"/> from the given <see cref="TypeDefinitionHandle"/>.
        /// </summary>
        /// <param name="handle"></param>
        /// <returns></returns>
        ManagedType LoadType(MetadataReader reader, TypeDefinitionHandle handle)
        {
            var type = reader.GetTypeDefinition(handle);
            var declaringType = ResolveDeclaringType(reader, handle);
            var typeName = GetTypeName(reader, type);
            var customAttributes = LoadCustomAttributes(reader, type.GetCustomAttributes());

            var genericParameters = LoadGenericParameters(reader, type.GetGenericParameters());
            var genericTypeParametersRef = new FixedValueList<ManagedGenericTypeParameterRef>(genericParameters.Count);
            var typeRef = ResolveTypeReference(reader, handle);
            for (int i = 0; i < genericParameters.Count; i++)
                genericTypeParametersRef[i] = new ManagedGenericTypeParameterRef(typeRef, i);
            var genericContext = new MetadataGenericContext(genericTypeParametersRef.AsReadOnly(), null);

            var interfaces = LoadInterfaces(reader, type.GetInterfaceImplementations(), genericContext);
            var fields = LoadFields(reader, type.GetFields(), genericContext);
            var methods = LoadMethods(reader, type.GetMethods(), genericContext);

            return new ManagedType(
                new MetadataTypeContext(this, reader, handle),
                assembly,
                declaringType,
                typeName,
                type.Attributes,
                customAttributes,
                genericParameters,
                type.BaseType.IsNil == false ? ResolveTypeReference(reader, type.BaseType) : null,
                interfaces,
                fields,
                methods);
        }

        /// <summary>
        /// Gets a type reference to the declaring type of a type definition.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="handle"></param>
        /// <returns></returns>
        ManagedType? ResolveDeclaringType(MetadataReader reader, TypeDefinitionHandle handle)
        {
            var declaringTypeHandle = reader.GetTypeDefinition(handle).GetDeclaringType();
            if (declaringTypeHandle.IsNil)
                return null;

            return ResolveType(reader, declaringTypeHandle);
        }

        /// <summary>
        /// Loads the generic parameters from the given collection.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="handles"></param>
        /// <returns></returns>
        ReadOnlyFixedValueList<ManagedGenericParameter> LoadGenericParameters(MetadataReader reader, GenericParameterHandleCollection handles)
        {
            var l = new FixedValueList<ManagedGenericParameter>(handles.Count);
            var e = handles.GetEnumerator();

            var i = 0;
            foreach (var handle in handles)
                l[i++] = LoadGenericParameter(reader, handle);

            return l.AsReadOnly();
        }

        /// <summary>
        /// Loads the generic parameter.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="handle"></param>
        /// <returns></returns>
        ManagedGenericParameter LoadGenericParameter(MetadataReader reader, GenericParameterHandle handle)
        {
            var parameter = reader.GetGenericParameter(handle);

            return new ManagedGenericParameter(
                reader.GetString(parameter.Name),
                LoadGenericParameterConstraints(reader, parameter.GetConstraints()));
        }

        /// <summary>
        /// Loads the generic parameter constraints from the given collection.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="handles"></param>
        /// <returns></returns>
        ReadOnlyFixedValueList<ManagedGenericParameterConstraint> LoadGenericParameterConstraints(MetadataReader reader, GenericParameterConstraintHandleCollection handles)
        {
            var l = new FixedValueList<ManagedGenericParameterConstraint>(handles.Count);

            var i = 0;
            foreach (var handle in handles)
                l[i++] = LoadGenericParameterConstraint(reader, handle);

            return l.AsReadOnly();
        }

        /// <summary>
        /// Loads the generic parameter constraint.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="handle"></param>
        /// <returns></returns>
        ManagedGenericParameterConstraint LoadGenericParameterConstraint(MetadataReader reader, GenericParameterConstraintHandle handle)
        {
            var constraint = reader.GetGenericParameterConstraint(handle);

            return new ManagedGenericParameterConstraint(
                ResolveTypeReference(reader, constraint.Type),
                LoadCustomAttributes(reader, constraint.GetCustomAttributes()));
        }

        /// <summary>
        /// Loads the interface implementations from the given collection.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="handles"></param>
        /// <returns></returns>
        ReadOnlyFixedValueList<ManagedInterface> LoadInterfaces(MetadataReader reader, InterfaceImplementationHandleCollection handles, MetadataGenericContext genericContext)
        {
            var l = new FixedValueList<ManagedInterface>(handles.Count);
            var e = handles.GetEnumerator();

            for (int i = 0; i < handles.Count; i++)
            {
                e.MoveNext();
                l[i] = LoadInterface(reader, e.Current, genericContext);
            }

            return l.AsReadOnly();
        }

        /// <summary>
        /// Loads the given interface.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="handle"></param>
        /// <returns></returns>
        ManagedInterface LoadInterface(MetadataReader reader, InterfaceImplementationHandle handle, MetadataGenericContext genericContext)
        {
            var iface = reader.GetInterfaceImplementation(handle);

            return new ManagedInterface(
                LoadCustomAttributes(reader, iface.GetCustomAttributes()),
                ResolveTypeReference(reader, iface.Interface));
        }

        /// <summary>
        /// Loads the fields from the given collection.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="handles"></param>
        /// <param name="genericContext"></param>
        /// <returns></returns>
        ReadOnlyFixedValueList<ManagedField> LoadFields(MetadataReader reader, FieldDefinitionHandleCollection handles, MetadataGenericContext genericContext)
        {
            var l = new FixedValueList<ManagedField>(handles.Count);
            var e = handles.GetEnumerator();

            for (int i = 0; i < handles.Count; i++)
            {
                e.MoveNext();
                l[i] = LoadField(reader, e.Current, genericContext);
            }

            return l.AsReadOnly();
        }

        /// <summary>
        /// Loads the given field.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="handle"></param>
        /// <param name="genericContext"></param>
        /// <returns></returns>
        ManagedField LoadField(MetadataReader reader, FieldDefinitionHandle handle, MetadataGenericContext genericContext)
        {
            var field = reader.GetFieldDefinition(handle);
            var fieldName = reader.GetString(field.Name);
            var signature = field.DecodeSignature(signatureTypeProvider, genericContext);

            return new ManagedField(
                fieldName,
                LoadCustomAttributes(reader, field.GetCustomAttributes()),
                field.Attributes,
                signature);
        }

        /// <summary>
        /// Loads the fields from the given collection.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="handles"></param>
        /// <returns></returns>
        ReadOnlyFixedValueList<ManagedMethod> LoadMethods(MetadataReader reader, MethodDefinitionHandleCollection handles, MetadataGenericContext genericContext)
        {
            var l = new FixedValueList<ManagedMethod>(handles.Count);
            var e = handles.GetEnumerator();

            for (int i = 0; i < handles.Count; i++)
            {
                e.MoveNext();
                l[i] = LoadMethod(reader, e.Current, genericContext);
            }

            return l.AsReadOnly();
        }

        /// <summary>
        /// Loads the given method.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="typeRef"></param>
        /// <param name="handle"></param>
        /// <param name="genericContext"></param>
        /// <returns></returns>
        ManagedMethod LoadMethod(MetadataReader reader, ManagedTypeRef typeRef, MethodDefinitionHandle handle, MetadataGenericContext genericContext)
        {
            var method = reader.GetMethodDefinition(handle);
            var signature = method.DecodeSignature(signatureTypeProvider, genericContext);

            var genericParameters = LoadGenericParameters(reader, method.GetGenericParameters());
            var genericMethodParametersRef = new FixedValueList<ManagedGenericMethodParameterRef>(genericParameters.Count);
            for (int i = 0; i < genericParameters.Count; i++)
                genericMethodParametersRef[i] = new ManagedGenericMethodParameterRef(typeRef, i);
            genericContext = new MetadataGenericContext(genericContext.TypeParameters, genericMethodParametersRef.AsReadOnly());

            var customAttributes = LoadCustomAttributes(reader, method.GetCustomAttributes());
            var parameters = LoadParameters(reader, signature.ParameterTypes, method.GetParameters(), genericContext);

            return new ManagedMethod(
                reader.GetString(method.Name),
                method.Attributes,
                method.ImplAttributes,
                customAttributes,
                genericParameters,
                signature.ReturnType,
                parameters);
        }

        /// <summary>
        /// Loads the parameters from the given collection.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="handles"></param>
        /// <returns></returns>
        ReadOnlyFixedValueList<ManagedParameter> LoadParameters(MetadataReader reader, IReadOnlyList<ManagedTypeSignature> parameterTypes, ParameterHandleCollection handles, MetadataGenericContext genericContext)
        {
            var l = new FixedValueList<ManagedParameter>(handles.Count);
            var e = handles.GetEnumerator();

            for (int i = 0; i < handles.Count; i++)
            {
                e.MoveNext();
                l[i] = LoadParameter(reader, parameterTypes[i], e.Current, genericContext);
            }

            return l.AsReadOnly();
        }

        /// <summary>
        /// Loads the given custom attribute.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="parameterType"></param>
        /// <param name="handle"></param>
        /// <returns></returns>
        ManagedParameter LoadParameter(MetadataReader reader, ManagedTypeSignature parameterType, ParameterHandle handle, MetadataGenericContext genericContext)
        {
            var parameter = reader.GetParameter(handle);

            return new ManagedParameter(
                reader.GetString(parameter.Name),
                parameter.Attributes,
                LoadCustomAttributes(reader, parameter.GetCustomAttributes()),
                parameterType);
        }

        /// <summary>
        /// Generates a full type name string for the given type reference.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="reference"></param>
        /// <returns></returns>
        string GetTypeName(MetadataReader reader, TypeReference reference)
        {
            var b = new StringBuilder();

            // may be a nested type
            if (reference.ResolutionScope.Kind == HandleKind.TypeReference)
            {
                var declaringTypeReferenceHandle = (TypeReferenceHandle)reference.ResolutionScope;
                var declaringTypeReference = reader.GetTypeReference(declaringTypeReferenceHandle);
                var declaringTypeName = GetTypeName(reader, declaringTypeReference);
                b.Append(declaringTypeName).Append('+');
            }

            // may have namespace
            if (reference.Namespace.IsNil == false)
            {
                var ns = reader.GetString(reference.Namespace);
                b.Append(ns).Append('.');
            }

            // append simple type name
            b.Append(reader.GetString(reference.Name));

            return b.ToString();
        }

        /// <summary>
        /// Generates a full type name string for the given type definition.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="definition"></param>
        /// <returns></returns>
        string GetTypeName(MetadataReader reader, TypeDefinition definition)
        {
            var b = new StringBuilder();

            // may be a nested type
            var declaringTypeHandle = definition.GetDeclaringType();
            if (declaringTypeHandle.IsNil == false)
            {
                var declaringType = reader.GetTypeDefinition(declaringTypeHandle);
                var declaringTypeName = GetTypeName(reader, declaringType);
                b.Append(declaringTypeName).Append('+');
            }

            // may have namespace
            if (definition.Namespace.IsNil == false)
            {
                var ns = reader.GetString(definition.Namespace);
                b.Append(ns).Append('.');
            }

            // append simple type name
            b.Append(reader.GetString(definition.Name));

            return b.ToString();
        }

    }

}
