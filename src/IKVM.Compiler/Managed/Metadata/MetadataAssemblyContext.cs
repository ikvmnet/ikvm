using System;
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
            return new ManagedAssembly(this, assemblyDef.GetAssemblyName(), LoadCustomAttributes(primary, assemblyDef.GetCustomAttributes(), MetadataGenericContext.Empty));
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
        internal ManagedType ResolveType(MetadataReader reader, TypeDefinitionHandle handle) => typeDefsCache.GetOrAdd(handle, _ => new ManagedType(new MetadataTypeContext(this, reader, _)));

        /// <summary>
        /// Resolves a type reference from the given entity.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="handle"></param>
        /// <param name="genericContext"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        internal ManagedTypeRef ResolveTypeReference(MetadataReader reader, EntityHandle handle, MetadataGenericContext genericContext) => handle.Kind switch
        {
            HandleKind.TypeReference => ResolveTypeReference(reader, (TypeReferenceHandle)handle, genericContext),
            HandleKind.TypeDefinition => ResolveTypeReference(reader, (TypeDefinitionHandle)handle, genericContext),
            HandleKind.TypeSpecification => ResolveTypeReference(reader, (TypeSpecificationHandle)handle, genericContext),
            _ => throw new InvalidOperationException(),
        };

        /// <summary>
        /// Resolves a type reference from the given definition.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="handle"></param>
        /// <param name="genericContext"></param>
        /// <returns></returns>
        internal ManagedTypeRef ResolveTypeReference(MetadataReader reader, TypeDefinitionHandle handle, MetadataGenericContext genericContext)
        {
            var type = reader.GetTypeDefinition(handle);
            return new ManagedTypeRef(this, assembly.Name, GetTypeName(reader, type), ResolveType(reader, handle));
        }

        /// <summary>
        /// Resolves a type reference from the given reference.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="handle"></param>
        /// <param name="genericContext"></param>
        /// <returns></returns>
        internal ManagedTypeRef ResolveTypeReference(MetadataReader reader, TypeReferenceHandle handle, MetadataGenericContext genericContext)
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
        /// Resolves a type reference from the given specification.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="handle"></param>
        /// <param name="genericContext"></param>
        /// <returns></returns>
        /// <exception cref="ManagedTypeException"></exception>
        internal ManagedTypeRef ResolveTypeReference(MetadataReader reader, TypeSpecificationHandle handle, MetadataGenericContext genericContext)
        {
            var signature = ResolveTypeSignature(reader, handle, genericContext);
            if (signature is ManagedTypeRefSignature s)
                return s.Reference;

            throw new ManagedTypeException("Could not decode a type reference from type specification.");
        }

        /// <summary>
        /// Resolves a type signature from the given entity handle.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="handle"></param>
        /// <param name="genericContext"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        internal ManagedTypeSignature ResolveTypeSignature(MetadataReader reader, EntityHandle handle, MetadataGenericContext genericContext) => handle.Kind switch
        {
            HandleKind.TypeReference => ResolveTypeSignature(reader, (TypeReferenceHandle)handle, genericContext),
            HandleKind.TypeDefinition => ResolveTypeSignature(reader, (TypeDefinitionHandle)handle, genericContext),
            HandleKind.TypeSpecification => ResolveTypeSignature(reader, (TypeSpecificationHandle)handle, genericContext),
            _ => throw new InvalidOperationException(),
        };

        /// <summary>
        /// Resolves a type signature from the given type definition.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="handle"></param>
        /// <param name="genericContext"></param>
        /// <returns></returns>
        internal ManagedTypeSignature ResolveTypeSignature(MetadataReader reader, TypeDefinitionHandle handle, MetadataGenericContext genericContext)
        {
            return new ManagedTypeRefSignature(ResolveTypeReference(reader, handle, genericContext));
        }

        /// <summary>
        /// Resolves a type signature from the given type reference.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="handle"></param>
        /// <param name="genericContext"></param>
        /// <returns></returns>
        internal ManagedTypeSignature ResolveTypeSignature(MetadataReader reader, TypeReferenceHandle handle, MetadataGenericContext genericContext)
        {
            return new ManagedTypeRefSignature(ResolveTypeReference(reader, handle, genericContext));
        }

        /// <summary>
        /// Resolves a type signature from the given type specification.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="handle"></param>
        /// <param name="genericContext"></param>
        /// <returns></returns>
        internal ManagedTypeSignature ResolveTypeSignature(MetadataReader reader, TypeSpecificationHandle handle, MetadataGenericContext genericContext)
        {
            var spec = reader.GetTypeSpecification(handle);
            return spec.DecodeSignature(signatureTypeProvider, genericContext);
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

            var i = 0;
            foreach (var handle in handles)
                l[i++] = ResolveType(reader, handle);

            return l.AsReadOnly();
        }

        /// <summary>
        /// Loads the custom attributes from the given collection.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="handles"></param>
        /// <returns></returns>
        ReadOnlyFixedValueList<ManagedCustomAttribute> LoadCustomAttributes(MetadataReader reader, CustomAttributeHandleCollection handles, MetadataGenericContext genericContext)
        {
            var l = new FixedValueList<ManagedCustomAttribute>(handles.Count);

            var i = 0;
            foreach (var handle in handles)
                l[i++] = LoadCustomAttribute(reader, handle);

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
        internal ManagedTypeData LoadType(MetadataReader reader, TypeDefinitionHandle handle)
        {
            var type = reader.GetTypeDefinition(handle);

            // build generic context for type
            var genericTypeParametersRef = new FixedValueList<ManagedGenericTypeParameterRef>(type.GetGenericParameters().Count);
            for (int i = 0; i < genericTypeParametersRef.Count; i++)
                genericTypeParametersRef[i] = new ManagedGenericTypeParameterRef(i);
            var genericContext = new MetadataGenericContext(genericTypeParametersRef.AsReadOnly(), null);

            // load type data
            var declaringType = ResolveDeclaringType(reader, handle);
            var typeName = GetTypeName(reader, type);
            var genericParameters = LoadGenericParameters(reader, type.GetGenericParameters(), genericContext);
            var customAttributes = LoadCustomAttributes(reader, type.GetCustomAttributes(), genericContext);
            var baseType = type.BaseType.IsNil == false ? ResolveTypeSignature(reader, type.BaseType, genericContext) : null;
            var interfaces = LoadInterfaces(reader, type.GetInterfaceImplementations(), genericContext);
            var fields = LoadFields(reader, type.GetFields(), genericContext);
            var methods = LoadMethods(reader, type.GetMethods(), genericContext);
            var properties = LoadProperties(reader, type.GetProperties(), genericContext);
            var events = LoadEvents(reader, type.GetEvents(), genericContext);
            var nestedTypes = LoadNestedTypes(reader, type.GetNestedTypes());

            // return type data
            return new ManagedTypeData(assembly, declaringType, typeName, type.Attributes, genericParameters, customAttributes, baseType, interfaces, fields, methods, properties, events, nestedTypes);
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
        ReadOnlyFixedValueList<ManagedGenericParameter> LoadGenericParameters(MetadataReader reader, GenericParameterHandleCollection handles, MetadataGenericContext genericContext)
        {
            var l = new FixedValueList<ManagedGenericParameter>(handles.Count);

            var i = 0;
            foreach (var handle in handles)
                l[i++] = LoadGenericParameter(reader, handle, genericContext);

            return l.AsReadOnly();
        }

        /// <summary>
        /// Loads the generic parameter.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="handle"></param>
        /// <returns></returns>
        ManagedGenericParameter LoadGenericParameter(MetadataReader reader, GenericParameterHandle handle, MetadataGenericContext genericContext)
        {
            var parameter = reader.GetGenericParameter(handle);

            return new ManagedGenericParameter(
                reader.GetString(parameter.Name),
                LoadGenericParameterConstraints(reader, parameter.GetConstraints(), genericContext));
        }

        /// <summary>
        /// Loads the generic parameter constraints from the given collection.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="handles"></param>
        /// <returns></returns>
        ReadOnlyFixedValueList<ManagedGenericParameterConstraint> LoadGenericParameterConstraints(MetadataReader reader, GenericParameterConstraintHandleCollection handles, MetadataGenericContext genericContext)
        {
            var l = new FixedValueList<ManagedGenericParameterConstraint>(handles.Count);

            var i = 0;
            foreach (var handle in handles)
                l[i++] = LoadGenericParameterConstraint(reader, handle, genericContext);

            return l.AsReadOnly();
        }

        /// <summary>
        /// Loads the generic parameter constraint.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="handle"></param>
        /// <returns></returns>
        ManagedGenericParameterConstraint LoadGenericParameterConstraint(MetadataReader reader, GenericParameterConstraintHandle handle, MetadataGenericContext genericContext)
        {
            var constraint = reader.GetGenericParameterConstraint(handle);

            return new ManagedGenericParameterConstraint(
                ResolveTypeSignature(reader, constraint.Type, genericContext),
                LoadCustomAttributes(reader, constraint.GetCustomAttributes(), genericContext));
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

            var i = 0;
            foreach (var handle in handles)
                l[i++] = LoadInterface(reader, handle, genericContext);

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
                LoadCustomAttributes(reader, iface.GetCustomAttributes(), genericContext),
                ResolveTypeSignature(reader, iface.Interface, genericContext));
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
                LoadCustomAttributes(reader, field.GetCustomAttributes(), genericContext),
                field.Attributes,
                signature);
        }

        /// <summary>
        /// Loads the methods from the given collection.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="handles"></param>
        /// <returns></returns>
        ReadOnlyFixedValueList<ManagedMethod> LoadMethods(MetadataReader reader, MethodDefinitionHandleCollection handles, MetadataGenericContext genericContext)
        {
            var l = new FixedValueList<ManagedMethod>(handles.Count);

            var i = 0;
            foreach (var handle in handles)
                l[i++] = LoadMethod(reader, handle, i, genericContext);

            return l.AsReadOnly();
        }

        /// <summary>
        /// Loads the given method.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="handle"></param>
        /// <param name="index"></param>
        /// <param name="genericContext"></param>
        /// <returns></returns>
        ManagedMethod LoadMethod(MetadataReader reader, MethodDefinitionHandle handle, int index, MetadataGenericContext genericContext)
        {
            var method = reader.GetMethodDefinition(handle);

            // create a generic context including the method parameters
            var genericMethodParametersRef = new FixedValueList<ManagedGenericMethodParameterRef>(method.GetGenericParameters().Count);
            for (int i = 0; i < genericMethodParametersRef.Count; i++)
                genericMethodParametersRef[i] = new ManagedGenericMethodParameterRef(index, i);
            genericContext = new MetadataGenericContext(genericContext.TypeParameters, genericMethodParametersRef.AsReadOnly());

            // decode method data
            var name = reader.GetString(method.Name);
            var signature = method.DecodeSignature(signatureTypeProvider, genericContext);
            var genericParameters = LoadGenericParameters(reader, method.GetGenericParameters(), genericContext);
            var customAttributes = LoadCustomAttributes(reader, method.GetCustomAttributes(), genericContext);
            var parameters = LoadParameters(reader, signature.ParameterTypes, method.GetParameters(), genericContext);

            // return method data
            return new ManagedMethod(name, method.Attributes, method.ImplAttributes, genericParameters, customAttributes, signature.ReturnType, parameters);
        }

        /// <summary>
        /// Loads the properties from the given collection.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="handles"></param>
        /// <returns></returns>
        ReadOnlyFixedValueList<ManagedProperty> LoadProperties(MetadataReader reader, PropertyDefinitionHandleCollection handles, MetadataGenericContext genericContext)
        {
            var l = new FixedValueList<ManagedProperty>(handles.Count);

            var i = 0;
            foreach (var handle in handles)
                l[i++] = LoadProperty(reader, handle, i, genericContext);

            return l.AsReadOnly();
        }

        /// <summary>
        /// Loads the given property.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="handle"></param>
        /// <param name="index"></param>
        /// <param name="genericContext"></param>
        /// <returns></returns>
        ManagedProperty LoadProperty(MetadataReader reader, PropertyDefinitionHandle handle, int index, MetadataGenericContext genericContext)
        {
            var property = reader.GetPropertyDefinition(handle);
            var name = reader.GetString(property.Name);
            var signature = property.DecodeSignature(signatureTypeProvider, genericContext);
            var customAttributes = LoadCustomAttributes(reader, property.GetCustomAttributes(), genericContext);
            return new ManagedProperty(name, property.Attributes, customAttributes, signature.ReturnType);
        }

        /// <summary>
        /// Loads the properties from the given collection.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="handles"></param>
        /// <returns></returns>
        ReadOnlyFixedValueList<ManagedEvent> LoadEvents(MetadataReader reader, EventDefinitionHandleCollection handles, MetadataGenericContext genericContext)
        {
            var l = new FixedValueList<ManagedEvent>(handles.Count);

            var i = 0;
            foreach (var handle in handles)
                l[i++] = LoadEvent(reader, handle, i, genericContext);

            return l.AsReadOnly();
        }

        /// <summary>
        /// Loads the given property.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="handle"></param>
        /// <param name="index"></param>
        /// <param name="genericContext"></param>
        /// <returns></returns>
        ManagedEvent LoadEvent(MetadataReader reader, EventDefinitionHandle handle, int index, MetadataGenericContext genericContext)
        {
            var evt = reader.GetEventDefinition(handle);
            var name = reader.GetString(evt.Name);
            var type = ResolveTypeSignature(reader, evt.Type, genericContext);
            var customAttributes = LoadCustomAttributes(reader, evt.GetCustomAttributes(), genericContext);
            return new ManagedEvent(name, evt.Attributes, customAttributes, type);
        }

        /// <summary>
        /// Loads the parameters from the given collection.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="handles"></param>
        /// <returns></returns>
        ReadOnlyFixedValueList<ManagedParameter> LoadParameters(MetadataReader reader, IReadOnlyList<ManagedTypeSignature> parameterTypes, ParameterHandleCollection handles, MetadataGenericContext genericContext)
        {
            var l = new FixedValueList<ManagedParameter>(parameterTypes.Count);
            var e = handles.GetEnumerator();

            for (int i = 0; i < parameterTypes.Count; i++)
                l[i] = LoadParameter(reader, parameterTypes[i], e.MoveNext() ? e.Current : new ParameterHandle(), genericContext);

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
            // handle might be null, but signature might specify
            if (handle.IsNil == false)
            {
                var parameter = reader.GetParameter(handle);
                return new ManagedParameter(
                    reader.GetString(parameter.Name),
                    parameter.Attributes,
                    LoadCustomAttributes(reader, parameter.GetCustomAttributes(), genericContext),
                    parameterType);
            }
            else
                return new ManagedParameter(
                    null,
                    ParameterAttributes.None,
                    new ReadOnlyFixedValueList<ManagedCustomAttribute>(),
                    parameterType);
        }

        /// <summary>
        /// Loads the nested types from the given collection.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="handles"></param>
        /// <returns></returns>
        ReadOnlyFixedValueList<ManagedType> LoadNestedTypes(MetadataReader reader, IList<TypeDefinitionHandle> handles)
        {
            var l = new FixedValueList<ManagedType>(handles.Count);

            var i = 0;
            foreach (var handle in handles)
                l[i] = LoadNestedType(reader, handle);

            return l.AsReadOnly();
        }

        /// <summary>
        /// Loads the given nested type.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="handle"></param>
        /// <returns></returns>
        ManagedType LoadNestedType(MetadataReader reader, TypeDefinitionHandle handle)
        {
            return ResolveType(reader, handle);
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
