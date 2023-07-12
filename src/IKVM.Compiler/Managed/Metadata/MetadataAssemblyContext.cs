using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Text;

using IKVM.Compiler.Collections;

namespace IKVM.Compiler.Managed.Metadata
{

    /// <summary>
    /// Represents an assembly loaded from System.Reflection.Metadata.
    /// </summary>
    internal class MetadataAssemblyContext : IManagedAssemblyContext, IManagedAssemblyResolver
    {

        record class CacheValue<T>(T Value);

        /// <summary>
        /// Gets or creates a new value associated with the incoming value cache.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cache"></param>
        /// <param name="create"></param>
        /// <returns></returns>
        static T GetOrCreate<T>(WeakReference<T> cache, Func<T> create) where T : class
        {
            if (cache.TryGetTarget(out var result) == false)
                cache.SetTarget(result = create());

            return result;
        }

        readonly IMetadataReaderResolver resolver;
        readonly string coreAssemblyName;
        readonly MetadataSignatureTypeProvider signatureTypeProvider;
        readonly ConcurrentDictionary<string, WeakReference<MetadataReader>?> assemblyNameCache = new();
        readonly ConditionalWeakTable<MetadataReader, WeakReference<ManagedAssembly>> assemblyCache = new();
        readonly ConditionalWeakTable<MetadataReader, ConcurrentDictionary<string, WeakReference<CacheValue<TypeDefinition>>?>> typeNameCache = new();
        readonly ConditionalWeakTable<MetadataReader, ConcurrentDictionary<TypeDefinition, WeakReference<ManagedType>>> typeCache = new();
        readonly ConditionalWeakTable<MetadataReader, ConcurrentDictionary<string, WeakReference<CacheValue<ExportedType>>?>> exportedTypeNameCache = new();

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="resolver"></param>
        /// <param name="coreAssemblyName"></param>
        public MetadataAssemblyContext(IMetadataReaderResolver resolver, string? coreAssemblyName = null)
        {
            this.resolver = resolver;
            this.coreAssemblyName = coreAssemblyName ?? typeof(object).Assembly.GetName().Name;
            this.signatureTypeProvider = new MetadataSignatureTypeProvider(this, this.coreAssemblyName);
        }

        /// <summary>
        /// Attempts to resolve a new weak reference to the reader for the given assembly name.
        /// </summary>
        /// <param name="assemblyName"></param>
        /// <returns></returns>
        WeakReference<MetadataReader>? ResolveMetadataReaderWeakReference(string assemblyName)
        {
            return resolver.Resolve(assemblyName) is MetadataReader reader ? new WeakReference<MetadataReader>(reader) : null;
        }

        /// <summary>
        /// Implements the <see cref="IManagedAssemblyContext.LoadAssembly(ManagedAssembly,  out ManagedAssemblyData)" /> method.
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        void IManagedAssemblyContext.LoadAssembly(ManagedAssembly assembly, out ManagedAssemblyData data) => LoadAssembly(assembly, out data);

        /// <summary>
        /// Implements the <see cref="IManagedAssemblyContext.ResolveTypes(ManagedAssembly)"/> method.
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        IEnumerable<ManagedType> IManagedAssemblyContext.ResolveTypes(ManagedAssembly assembly) => ResolveTypes(assembly);

        /// <summary>
        /// Implements the <see cref="IManagedAssemblyContext.ResolveType(ManagedAssembly, string)"/> method.
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="typeName"></param>
        /// <returns></returns>
        ManagedType? IManagedAssemblyContext.ResolveType(ManagedAssembly assembly, string typeName) => ResolveType(assembly, typeName);

        /// <summary>
        /// Implements the <see cref="IManagedAssemblyContext.ResolveExportedTypes(ManagedAssembly)"/> method.
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        IEnumerable<ManagedExportedType> IManagedAssemblyContext.ResolveExportedTypes(ManagedAssembly assembly) => ResolveExportedTypes(assembly);

        /// <summary>
        /// Implements the <see cref="IManagedAssemblyContext.ResolveExportedTypes(ManagedAssembly, string)"/> method.
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        ManagedExportedType? IManagedAssemblyContext.ResolveExportedType(ManagedAssembly assembly, string typeName) => ResolveExportedType(assembly, typeName);

        /// <summary>
        /// Implements the <see cref="IManagedAssemblyContext.LoadType(ManagedType, out ManagedTypeData)"/> method.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="result"></param>
        void IManagedAssemblyContext.LoadType(ManagedType type, out ManagedTypeData result) => LoadType(type, out result);

        /// <summary>
        /// Implements the <see cref="IManagedAssemblyContext.ResolveNestedTypes(ManagedType)"/> method.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        IEnumerable<ManagedType> IManagedAssemblyContext.ResolveNestedTypes(ManagedType type) => ResolveNestedTypes(type);

        /// <summary>
        /// Implements the <see cref="IManagedAssemblyContext.ResolveNestedType(ManagedType, string)"/> method.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="typeName"></param>
        /// <returns></returns>
        ManagedType? IManagedAssemblyContext.ResolveNestedType(ManagedType type, string typeName) => ResolveNestedType(type, typeName);

        /// <summary>
        /// Implements the <see cref="IManagedAssemblyResolver.ResolveAssembly(string)"/> method.
        /// </summary>
        /// <param name="assemblyName"></param>
        /// <returns></returns>
        ManagedAssembly? IManagedAssemblyResolver.ResolveAssembly(string assemblyName) => ResolveAssembly(assemblyName);

        /// <summary>
        /// Resolves the <see cref="ManagedAssembly"/> with the given name.
        /// </summary>
        /// <param name="assemblyName"></param>
        /// <returns></returns>
        public ManagedAssembly? ResolveAssembly(string assemblyName)
        {
            var readerCache = assemblyNameCache.GetOrAdd(assemblyName, ResolveMetadataReaderWeakReference);
            var reader = readerCache != null ? GetOrCreate(readerCache, () => resolver.Resolve(assemblyName)!) : null;
            return reader != null ? ResolveAssembly(reader) : null;
        }

        /// <summary>
        /// Resolves the managed assembly associated with the given primary metadata reader.
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        ManagedAssembly ResolveAssembly(MetadataReader reader)
        {
            return GetOrCreate(assemblyCache.GetValue(reader, _ => new WeakReference<ManagedAssembly>(CreateAssembly(_))), () => CreateAssembly(reader));
        }

        /// <summary>
        /// Resolves the managed assembly associated with the given primary metadata reader.
        /// </summary>
        /// <returns></returns>
        ManagedAssembly CreateAssembly(MetadataReader reader)
        {
            var assemblyDef = reader.GetAssemblyDefinition();
            return new ManagedAssembly(this, reader, assemblyDef.GetAssemblyName());
        }

        /// <summary>
        /// Resolves the type with the specified name from the given assembly.
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="typeName"></param>
        /// <returns></returns>
        ManagedType? ResolveType(ManagedAssembly assembly, string typeName)
        {
            return ResolveType(assembly, (MetadataReader)assembly.Handle, typeName);
        }

        /// <summary>
        /// Resolves the type with the specified name from the given assembly.
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="reader"></param>
        /// <param name="typeName"></param>
        /// <returns></returns>
        ManagedType? ResolveType(ManagedAssembly assembly, MetadataReader reader, string typeName)
        {
            var typeNameCacheEntry = typeNameCache.GetValue(reader, _ => new ConcurrentDictionary<string, WeakReference<CacheValue<TypeDefinition>>?>());
            var typeRef = typeNameCacheEntry.GetOrAdd(typeName, _ => FindTypeDefWeakReference(typeNameCacheEntry, reader, _));
            var typeDef = typeRef != null ? GetOrCreate(typeRef, () => FindTypeDefCacheValue(typeNameCacheEntry, reader, typeName)!) : null;
            return typeDef != null ? ResolveType(assembly, reader, typeDef.Value) : null;
        }

        /// <summary>
        /// Searches for a matching type name in the given reader, and returns the weak reference to the cache value to the type definition.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="typeName"></param>
        /// <returns></returns>
        WeakReference<CacheValue<TypeDefinition>>? FindTypeDefWeakReference(ConcurrentDictionary<string, WeakReference<CacheValue<TypeDefinition>>?> typeNameCache, MetadataReader reader, string typeName)
        {
            var typeDefCacheValue = FindTypeDefCacheValue(typeNameCache, reader, typeName);
            return typeDefCacheValue != null ? new WeakReference<CacheValue<TypeDefinition>>(typeDefCacheValue) : null;
        }

        /// <summary>
        /// Searches for a matching type name in the given reader, and returns a cache value to the type definition.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="typeName"></param>
        /// <returns></returns>
        CacheValue<TypeDefinition>? FindTypeDefCacheValue(ConcurrentDictionary<string, WeakReference<CacheValue<TypeDefinition>>?> typeNameCache, MetadataReader reader, string typeName)
        {
            var typeDef = FindTypeDefinition(typeNameCache, reader, typeName);
            return typeDef != null ? new CacheValue<TypeDefinition>(typeDef.Value) : null;
        }

        /// <summary>
        /// Searches for a matching type name in the given reader and returns the type definition. Preemptively caches encountered entries.
        /// </summary>
        /// <param name="typeName"></param>
        /// <returns></returns>
        TypeDefinition? FindTypeDefinition(ConcurrentDictionary<string, WeakReference<CacheValue<TypeDefinition>>?> typeNameCache, MetadataReader reader, string typeName)
        {
            var l = reader.TypeDefinitions;
            var e = l.GetEnumerator();

            for (int i = 0; i < l.Count; i++)
            {
                e.MoveNext();
                var t = reader.GetTypeDefinition(e.Current);
                var n = GetTypeName(reader, t);
                if (n != typeName)
                    typeNameCache.GetOrAdd(n, _ => new WeakReference<CacheValue<TypeDefinition>>(new CacheValue<TypeDefinition>(t)));
                else
                    return t;
            }

            return null;
        }

        /// <summary>
        /// Resolves the specified type from this assembly.
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="reader"></param>
        /// <param name="typeDef"></param>
        /// <returns></returns>
        ManagedType ResolveType(ManagedAssembly assembly, MetadataReader reader, TypeDefinition typeDef)
        {
            var typeCacheEntry = typeCache.GetValue(reader, _ => new ConcurrentDictionary<TypeDefinition, WeakReference<ManagedType>>());
            return GetOrCreate(typeCacheEntry.GetOrAdd(typeDef, _ => new WeakReference<ManagedType>(CreateType(assembly, reader, _))), () => CreateType(assembly, reader, typeDef));
        }

        /// <summary>
        /// Creates a new type instance.
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="reader"></param>
        /// <param name="typeDef"></param>
        /// <returns></returns>
        ManagedType CreateType(ManagedAssembly assembly, MetadataReader reader, TypeDefinition typeDef)
        {
            return new ManagedType(assembly, (reader, typeDef), GetTypeName(reader, typeDef), typeDef.Attributes);
        }

        /// <summary>
        /// Resolves the type for the assembly by the handle.
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        IEnumerable<ManagedType> ResolveTypes(ManagedAssembly assembly)
        {
            return ResolveTypes(assembly, (MetadataReader)assembly.Handle);
        }

        /// <summary>
        /// Resolves the type for the assembly by the handle.
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="reader"></param>
        /// <returns></returns>
        IEnumerable<ManagedType> ResolveTypes(ManagedAssembly assembly, MetadataReader reader)
        {
            return ResolveTypes(assembly, reader, reader.TypeDefinitions);
        }

        /// <summary>
        /// Resolves all the types for the assembly.
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="reader"></param>
        /// <param name="handles"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        IEnumerable<ManagedType> ResolveTypes(ManagedAssembly assembly, MetadataReader reader, TypeDefinitionHandleCollection handles)
        {
            foreach (var handle in handles)
                yield return ResolveType(assembly, reader, reader.GetTypeDefinition(handle)) ?? throw new InvalidOperationException();
        }

        /// <summary>
        /// Loads the managed assembly.
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        void LoadAssembly(ManagedAssembly assembly, out ManagedAssemblyData result)
        {
            var reader = (MetadataReader)assembly.Handle;
            var assemblyDef = reader.GetAssemblyDefinition();
            result = new ManagedAssemblyData();
            LoadAssemblyCustomAttributes(assembly, reader, assemblyDef.GetCustomAttributes(), out result.CustomAttributes);
            LoadAssemblyReferences(assembly, reader, reader.AssemblyReferences, out result.References);
        }

        /// <summary>
        /// Resolves a type reference from the given entity.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="handle"></param>
        /// <param name="genericContext"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        internal ManagedType? ResolveType(MetadataReader reader, EntityHandle handle, MetadataGenericContext genericContext) => handle.Kind switch
        {
            HandleKind.TypeReference => ResolveType(reader, (TypeReferenceHandle)handle, genericContext),
            HandleKind.TypeDefinition => ResolveType(reader, (TypeDefinitionHandle)handle, genericContext),
            HandleKind.TypeSpecification => ResolveType(reader, (TypeSpecificationHandle)handle, genericContext),
            _ => throw new InvalidOperationException(),
        };

        /// <summary>
        /// Resolves a type reference from the given definition.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="handle"></param>
        /// <param name="genericContext"></param>
        /// <returns></returns>
        ManagedType? ResolveType(MetadataReader reader, TypeDefinitionHandle handle, MetadataGenericContext genericContext)
        {
            var assemblyName = reader.GetString(reader.GetAssemblyDefinition().Name);
            var assembly = ResolveAssembly(assemblyName);
            if (assembly == null)
                throw new ManagedResolveException($"Could not resolve assembly '{assemblyName}'.");

            var typeDef = reader.GetTypeDefinition(handle);
            var typeName = GetTypeName(reader, typeDef);

            // assembly contains a type export: follow
            var exportedType = assembly.ResolveExportedType(typeName);
            if (exportedType != null)
                return ResolveType(reader, exportedType.Value);

            // fallback to scanning for regular types
            // TODO check friend access
            return assembly.ResolveType(typeName);
        }

        /// <summary>
        /// Resolves a type reference from the given reference.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="handle"></param>
        /// <param name="genericContext"></param>
        /// <returns></returns>
        ManagedType? ResolveType(MetadataReader reader, TypeReferenceHandle handle, MetadataGenericContext genericContext)
        {
            var typeReference = reader.GetTypeReference(handle);
            var typeName = GetTypeName(reader, typeReference);

            // type reference is a type nested under a parent type
            if (typeReference.ResolutionScope.Kind == HandleKind.TypeReference)
            {
                var declaringTypeHandle = (TypeReferenceHandle)typeReference.ResolutionScope;
                var declaringType = ResolveType(reader, declaringTypeHandle, genericContext);
                if (declaringType == null)
                    throw new ManagedResolveException($"Could not find declaring type of {typeName}.");

                return declaringType.ResolveNestedType(typeName);
            }

            // type reference is nested under a type in another assembly
            if (typeReference.ResolutionScope.Kind == HandleKind.AssemblyReference)
            {
                var assemblyReferenceHandle = (AssemblyReferenceHandle)typeReference.ResolutionScope;
                var assemblyReference = reader.GetAssemblyReference(assemblyReferenceHandle);
                var assemblyName = reader.GetString(assemblyReference.Name);

                var assembly = ResolveAssembly(assemblyName);
                if (assembly == null)
                    return null;

                // assembly contains a type export: follow
                var exportedType = assembly.ResolveExportedType(typeName);
                if (exportedType != null)
                    return ResolveType(reader, exportedType.Value);

                // fallback to scanning for regular types
                // TODO check friend access (InternalsVisibleTo)
                return assembly.ResolveType(typeName);
            }

            throw new ManagedResolveException("Invalid type reference pointing to same assembly.");
        }

        /// <summary>
        /// Resolves a type reference from the given specification.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="handle"></param>
        /// <param name="genericContext"></param>
        /// <returns></returns>
        /// <exception cref="ManagedException"></exception>
        ManagedType? ResolveType(MetadataReader reader, TypeSpecificationHandle handle, MetadataGenericContext genericContext)
        {
            var signature = ResolveTypeSignature(reader, handle, genericContext);
            if (signature.Kind == ManagedSignatureKind.Type)
                return ((ManagedTypeSignature)signature).Type;

            throw new ManagedResolveException("Could not decode a type reference from type specification.");
        }

        /// <summary>
        /// Resolves an exported type to its type implementation.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="exportedType"></param>
        /// <returns></returns>
        ManagedType? ResolveType(MetadataReader reader, ManagedExportedType exportedType)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Resolves a type signature from the given entity handle.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="handle"></param>
        /// <param name="genericContext"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        ManagedSignature ResolveTypeSignature(MetadataReader reader, EntityHandle handle, MetadataGenericContext genericContext) => handle.Kind switch
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
        ManagedSignature ResolveTypeSignature(MetadataReader reader, TypeDefinitionHandle handle, MetadataGenericContext genericContext)
        {
            var type = ResolveType(reader, handle, genericContext);
            if (type != null)
                return ManagedSignature.Type(type);

            var typeRef = reader.GetTypeDefinition(handle);
            var typeName = GetTypeName(reader, typeRef);
            throw new ManagedResolveException("Coult not resolve type definition '{typeName}' for signature.");
        }

        /// <summary>
        /// Resolves a type signature from the given type reference.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="handle"></param>
        /// <param name="genericContext"></param>
        /// <returns></returns>
        ManagedSignature ResolveTypeSignature(MetadataReader reader, TypeReferenceHandle handle, MetadataGenericContext genericContext)
        {
            var type = ResolveType(reader, handle, genericContext);
            if (type != null)
                return ManagedSignature.Type(type);

            var typeRef = reader.GetTypeReference(handle);
            var typeName = GetTypeName(reader, typeRef);
            throw new ManagedResolveException("Coult not resolve type reference '{typeName}' for signature.");
        }

        /// <summary>
        /// Resolves a type signature from the given type specification.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="handle"></param>
        /// <param name="genericContext"></param>
        /// <returns></returns>
        ManagedSignature ResolveTypeSignature(MetadataReader reader, TypeSpecificationHandle handle, MetadataGenericContext genericContext)
        {
            var spec = reader.GetTypeSpecification(handle);
            return spec.DecodeSignature(signatureTypeProvider, genericContext);
        }

        /// <summary>
        /// Resolves the assembly name of the given type reference.
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="reader"></param>
        /// <param name="handle"></param>
        /// <returns></returns>
        AssemblyName ResolveAssemblyName(ManagedAssembly assembly, MetadataReader reader, TypeReferenceHandle handle)
        {
            var typeReference = reader.GetTypeReference(handle);

            if (typeReference.ResolutionScope.Kind == HandleKind.TypeReference)
                return ResolveAssemblyName(assembly, reader, (TypeReferenceHandle)typeReference.ResolutionScope);

            if (typeReference.ResolutionScope.Kind == HandleKind.AssemblyReference)
                return reader.GetAssemblyReference((AssemblyReferenceHandle)typeReference.ResolutionScope).GetAssemblyName();

            return assembly.Name;
        }

        /// <summary>
        /// Loads the exported types from this assembly.
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        IEnumerable<ManagedExportedType> ResolveExportedTypes(ManagedAssembly assembly)
        {
            var reader = (MetadataReader)assembly.Handle;
            return ResolveExportedTypes(assembly, reader, reader.ExportedTypes);
        }

        /// <summary>
        /// Loads the exported types from this assembly.
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="reader"></param>
        /// <param name="handles"></param>
        /// <returns></returns>
        IEnumerable<ManagedExportedType> ResolveExportedTypes(ManagedAssembly assembly, MetadataReader reader, ExportedTypeHandleCollection handles)
        {
            foreach (var handle in handles)
                yield return ResolveExportedType(assembly, reader, handle);
        }

        /// <summary>
        /// Resolves the specified exported type from this assembly.
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="typeName"></param>
        /// <returns></returns>
        ManagedExportedType? ResolveExportedType(ManagedAssembly assembly, string typeName)
        {
            var reader = (MetadataReader)assembly.Handle;
            return ResolveExportedType(assembly, reader, typeName);
        }

        /// <summary>
        /// Resolves the specified exported type from this assembly.
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="reader"></param>
        /// <param name="typeName"></param>
        /// <returns></returns>
        ManagedExportedType? ResolveExportedType(ManagedAssembly assembly, MetadataReader reader, string typeName)
        {
            var exportedTypeNameCacheEntry = exportedTypeNameCache.GetValue(reader, _ => new ConcurrentDictionary<string, WeakReference<CacheValue<ExportedType>>?>());
            var typeRef = exportedTypeNameCacheEntry.GetOrAdd(typeName, _ => FindExportedTypeWeakReference(exportedTypeNameCacheEntry, reader, _));
            var typeDef = typeRef != null ? GetOrCreate(typeRef, () => FindExportedTypeDefCacheValue(exportedTypeNameCacheEntry, reader, typeName)!) : null;
            return typeDef != null ? ResolveExportedType(assembly, reader, typeDef.Value) : null;
        }

        /// <summary>
        /// Searches for a matching exported type name in the given reader, and returns the weak reference to the cache value to the type definition.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="typeName"></param>
        /// <returns></returns>
        WeakReference<CacheValue<ExportedType>>? FindExportedTypeWeakReference(ConcurrentDictionary<string, WeakReference<CacheValue<ExportedType>>?> typeNameCache, MetadataReader reader, string typeName)
        {
            var typeDefCacheValue = FindExportedTypeDefCacheValue(typeNameCache, reader, typeName);
            return typeDefCacheValue != null ? new WeakReference<CacheValue<ExportedType>>(typeDefCacheValue) : null;
        }

        /// <summary>
        /// Searches for a matching exported type name in the given reader, and returns a cache value to the type definition.
        /// </summary>
        /// <param name="typeNameCache"></param>
        /// <param name="reader"></param>
        /// <param name="typeName"></param>
        /// <returns></returns>
        CacheValue<ExportedType>? FindExportedTypeDefCacheValue(ConcurrentDictionary<string, WeakReference<CacheValue<ExportedType>>?> typeNameCache, MetadataReader reader, string typeName)
        {
            var typeDef = FindExportedTypeDefinition(typeNameCache, reader, typeName);
            return typeDef != null ? new CacheValue<ExportedType>(typeDef.Value) : null;
        }

        /// <summary>
        /// Searches for a matching exported type name in the given reader and returns the type definition. Preemptively caches encountered entries.
        /// </summary>
        /// <param name="exportedTypeNameCache"></param>
        /// <param name="reader"></param>
        /// <param name="typeName"></param>
        /// <returns></returns>
        ExportedType? FindExportedTypeDefinition(ConcurrentDictionary<string, WeakReference<CacheValue<ExportedType>>?> exportedTypeNameCache, MetadataReader reader, string typeName)
        {
            var l = reader.ExportedTypes;
            var e = l.GetEnumerator();

            for (int i = 0; i < l.Count; i++)
            {
                e.MoveNext();
                var t = reader.GetExportedType(e.Current);
                var n = GetExportedTypeName(reader, t);
                if (n != typeName)
                    exportedTypeNameCache.GetOrAdd(n, _ => new WeakReference<CacheValue<ExportedType>>(new CacheValue<ExportedType>(t)));
                else
                    return t;
            }

            return null;
        }

        /// <summary>
        /// Resolves the specified exported type from this assembly.
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="reader"></param>
        /// <param name="exportedTypeHandle"></param>
        /// <returns></returns>
        ManagedExportedType ResolveExportedType(ManagedAssembly assembly, MetadataReader reader, ExportedTypeHandle exportedTypeHandle)
        {
            return ResolveExportedType(assembly, reader, reader.GetExportedType(exportedTypeHandle));
        }

        /// <summary>
        /// Resolves the specified exported type from this assembly.
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="reader"></param>
        /// <param name="exportedType"></param>
        /// <returns></returns>
        ManagedExportedType ResolveExportedType(ManagedAssembly assembly, MetadataReader reader, ExportedType exportedType)
        {
            return CreateExportedType(assembly, reader, exportedType);
        }

        /// <summary>
        /// Creates a new <see cref="ManagedExportedType"/> instance.
        /// </summary>
        /// <param name="exportedType"></param>
        /// <returns></returns>
        ManagedExportedType CreateExportedType(ManagedAssembly assembly, MetadataReader reader, ExportedType exportedType)
        {
            var result = new ManagedExportedType();
            result.Name = GetExportedTypeName(reader, exportedType);
            LoadExportedTypeCustomAttributes(result, reader, exportedType.GetCustomAttributes(), out result.CustomAttributes);
            return result;
        }

        /// <summary>
        /// Resolves all nested types of the specified type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        IEnumerable<ManagedType> ResolveNestedTypes(ManagedType type)
        {
            var (reader, typeDef) = ((MetadataReader, TypeDefinition))type.Handle;
            return ResolveNestedTypes(type, reader, typeDef);
        }

        /// <summary>
        /// Resolves all nested types of the specified type.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="reader"></param>
        /// <param name="typeDef"></param>
        /// <returns></returns>
        IEnumerable<ManagedType> ResolveNestedTypes(ManagedType type, MetadataReader reader, TypeDefinition typeDef)
        {
            foreach (var handle in typeDef.GetNestedTypes())
                yield return ResolveType(type.Assembly, reader, reader.GetTypeDefinition(handle));
        }

        /// <summary>
        /// Resolves the nested type of the specified type.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="typeName"></param>
        /// <returns></returns>
        ManagedType? ResolveNestedType(ManagedType type, string typeName)
        {
            var (reader, typeDef) = ((MetadataReader, TypeDefinition))type.Handle;
            return ResolveNestedType(type, reader, typeDef, typeName);
        }

        /// <summary>
        /// Resolves all nested types of the specified type.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="reader"></param>
        /// <param name="typeName"></param>
        /// <returns></returns>
        ManagedType? ResolveNestedType(ManagedType type, MetadataReader reader, TypeDefinition typeDef, string typeName)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Loads the custom attributes from the given collection.
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="reader"></param>
        /// <param name="handles"></param>
        /// <param name="result"></param>
        void LoadAssemblyCustomAttributes(ManagedAssembly assembly, MetadataReader reader, CustomAttributeHandleCollection handles, out FixedValueList8<ManagedCustomAttributeData> result)
        {
            result = new FixedValueList8<ManagedCustomAttributeData>(handles.Count);

            var i = 0;
            foreach (var handle in handles)
            {
                LoadAssemblyCustomAttribute(assembly, reader, handle, i, out result.GetItemRef(i));
                i++;
            }
        }

        /// <summary>
        /// Loads the given custom attribute.
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="reader"></param>
        /// <param name="handle"></param>
        /// <param name="index"></param>
        /// <param name="result"></param>
        void LoadAssemblyCustomAttribute(ManagedAssembly assembly, MetadataReader reader, CustomAttributeHandle handle, int index, out ManagedCustomAttributeData result)
        {
            result = new ManagedCustomAttributeData();
        }

        /// <summary>
        /// Loads the assembly references from the given collection.
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="reader"></param>
        /// <param name="handles"></param>
        /// <param name="result"></param>
        void LoadAssemblyReferences(ManagedAssembly assembly, MetadataReader reader, AssemblyReferenceHandleCollection handles, out FixedValueList8<ManagedAssemblyReferenceData> result)
        {
            result = new FixedValueList8<ManagedAssemblyReferenceData>(handles.Count);

            var i = 0;
            foreach (var handle in handles)
            {
                LoadAssemblyReference(assembly, reader, handle, i, out result.GetItemRef(i));
                i++;
            }
        }

        /// <summary>
        /// Loads the given custom attribute.
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="reader"></param>
        /// <param name="handle"></param>
        /// <param name="index"></param>
        /// <param name="result"></param>
        void LoadAssemblyReference(ManagedAssembly assembly, MetadataReader reader, AssemblyReferenceHandle handle, int index, out ManagedAssemblyReferenceData result)
        {
            var assemblyRef = reader.GetAssemblyReference(handle);

            result = new ManagedAssemblyReferenceData();
            result.AssemblyName = assemblyRef.GetAssemblyName();
        }

        /// <summary>
        /// Loads the custom attributes from the given collection.
        /// </summary>
        /// <param name="exportedType"></param>
        /// <param name="reader"></param>
        /// <param name="handles"></param>
        /// <param name="result"></param>
        void LoadExportedTypeCustomAttributes(ManagedExportedType exportedType, MetadataReader reader, CustomAttributeHandleCollection handles, out FixedValueList1<ManagedCustomAttributeData> result)
        {
            result = new FixedValueList1<ManagedCustomAttributeData>(handles.Count);

            var i = 0;
            foreach (var handle in handles)
            {
                LoadExportedTypeCustomAttribute(exportedType, reader, handle, i, out result.GetItemRef(i));
                i++;
            }
        }

        /// <summary>
        /// Loads the given custom attribute..
        /// </summary>
        /// <param name="exportedType"></param>
        /// <param name="reader"></param>
        /// <param name="handle"></param>
        /// <param name="index"></param>
        /// <param name="result"></param>
        void LoadExportedTypeCustomAttribute(ManagedExportedType exportedType, MetadataReader reader, CustomAttributeHandle handle, int index, out ManagedCustomAttributeData result)
        {
            result = new ManagedCustomAttributeData();
        }

        /// <summary>
        /// Loads the managed type.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="result"></param>
        void LoadType(ManagedType type, out ManagedTypeData result)
        {
            var (reader, typeDef) = ((MetadataReader, TypeDefinition))type.Handle;

            // build generic context for type
            var genericTypeParametersRef = new FixedValueList1<ManagedGenericTypeParameterRef>(typeDef.GetGenericParameters().Count);
            for (int i = 0; i < genericTypeParametersRef.Count; i++)
                genericTypeParametersRef[i] = new ManagedGenericTypeParameterRef(i);
            var genericContext = new MetadataGenericContext(genericTypeParametersRef, null);

            // load type data
            result = new ManagedTypeData();
            result.DeclaringType = ResolveDeclaringType(type, reader, typeDef);
            LoadTypeCustomAttributes(type, reader, typeDef.GetCustomAttributes(), genericContext, out result.CustomAttributes);
            LoadTypeGenericParameters(type, reader, typeDef.GetGenericParameters(), genericContext, out result.GenericParameters);
            result.BaseType = typeDef.BaseType.IsNil == false ? ResolveTypeSignature(reader, typeDef.BaseType, genericContext) : null;
            LoadTypeInterfaces(type, reader, typeDef.GetInterfaceImplementations(), genericContext, out result.Interfaces);
            LoadTypeFields(type, reader, typeDef.GetFields(), genericContext, out result.Fields);
            LoadTypeMethods(type, reader, typeDef.GetMethods(), genericContext, out result.Methods);
            LoadTypeProperties(type, reader, typeDef.GetProperties(), genericContext, out result.Properties);
            LoadTypeEvents(type, reader, typeDef.GetEvents(), genericContext, out result.Events);
        }

        /// <summary>
        /// Gets a type reference to the declaring type of a type definition.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="reader"></param>
        /// <param name="typeDef"></param>
        /// <returns></returns>
        ManagedType? ResolveDeclaringType(ManagedType type, MetadataReader reader, TypeDefinition typeDef)
        {
            var declaringTypeHandle = typeDef.GetDeclaringType();
            return declaringTypeHandle.IsNil == false ? ResolveType(type.Assembly, reader, reader.GetTypeDefinition(declaringTypeHandle)) : null;
        }

        /// <summary>
        /// Loads the custom attributes from the given collection.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="reader"></param>
        /// <param name="handles"></param>
        /// <param name="genericContext"></param>
        /// <param name="result"></param>
        void LoadTypeCustomAttributes(ManagedType type, MetadataReader reader, CustomAttributeHandleCollection handles, MetadataGenericContext genericContext, out FixedValueList1<ManagedCustomAttributeData> result)
        {
            result = new FixedValueList1<ManagedCustomAttributeData>(handles.Count);

            var i = 0;
            foreach (var handle in handles)
            {
                LoadTypeCustomAttribute(type, reader, handle, i, genericContext, out result.GetItemRef(i));
                i++;
            }
        }

        /// <summary>
        /// Loads the given custom attribute.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="reader"></param>
        /// <param name="handle"></param>
        /// <param name="index"></param>
        /// <param name="genericContext"></param>
        /// <param name="result"></param>
        void LoadTypeCustomAttribute(ManagedType type, MetadataReader reader, CustomAttributeHandle handle, int index, MetadataGenericContext genericContext, out ManagedCustomAttributeData result)
        {
            result = new ManagedCustomAttributeData();
        }

        /// <summary>
        /// Loads the generic parameter constraints from the given collection.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="reader"></param>
        /// <param name="handles"></param>
        /// <param name="genericContext"></param>
        /// <param name="result"></param>
        void LoadTypeGenericParameters(ManagedType type, MetadataReader reader, GenericParameterHandleCollection handles, MetadataGenericContext genericContext, out FixedValueList1<ManagedGenericParameter> result)
        {
            result = new FixedValueList1<ManagedGenericParameter>(handles.Count);

            var i = 0;
            foreach (var handle in handles)
            {
                LoadTypeGenericParameter(type, reader, handle, i, genericContext, out result.GetItemRef(i));
                i++;
            }
        }

        /// <summary>
        /// Loads the generic parameter.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="reader"></param>
        /// <param name="handle"></param>
        /// <param name="index"></param>
        /// <param name="genericContext"></param>
        /// <param name="result"></param>
        void LoadTypeGenericParameter(ManagedType type, MetadataReader reader, GenericParameterHandle handle, int index, MetadataGenericContext genericContext, out ManagedGenericParameter result)
        {
            var parameter = reader.GetGenericParameter(handle);

            result = new ManagedGenericParameter();
            result.Name = reader.GetString(parameter.Name);
            LoadTypeGenericParameterConstraints(type, reader, parameter.GetConstraints(), genericContext, out result.Constraints);
        }

        /// <summary>
        /// Loads the generic parameter constraints from the given collection.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="reader"></param>
        /// <param name="handles"></param>
        /// <param name="genericContext"></param>
        /// <param name="result"></param>
        void LoadTypeGenericParameterConstraints(ManagedType type, MetadataReader reader, GenericParameterConstraintHandleCollection handles, MetadataGenericContext genericContext, out FixedValueList1<ManagedGenericParameterConstraint> result)
        {
            result = new FixedValueList1<ManagedGenericParameterConstraint>(handles.Count);

            var i = 0;
            foreach (var handle in handles)
            {
                LoadTypeGenericParameterConstraint(type, reader, handle, i, genericContext, out result.GetItemRef(i));
                i++;
            }
        }

        /// <summary>
        /// Loads the generic parameter constraint.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="reader"></param>
        /// <param name="handle"></param>
        /// <param name="index"></param>
        /// <param name="genericContext"></param>
        /// <param name="result"></param>
        void LoadTypeGenericParameterConstraint(ManagedType type, MetadataReader reader, GenericParameterConstraintHandle handle, int index, MetadataGenericContext genericContext, out ManagedGenericParameterConstraint result)
        {
            var constraint = reader.GetGenericParameterConstraint(handle);

            result = new ManagedGenericParameterConstraint();
            LoadTypeCustomAttributes(type, reader, constraint.GetCustomAttributes(), genericContext, out result.CustomAttributes);
            result.Type = ResolveTypeSignature(reader, constraint.Type, genericContext);
        }

        /// <summary>
        /// Loads the interface implementations from the given collection.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="reader"></param>
        /// <param name="handles"></param>
        /// <param name="genericContext"></param>
        /// <param name="result"></param>
        void LoadTypeInterfaces(ManagedType type, MetadataReader reader, InterfaceImplementationHandleCollection handles, MetadataGenericContext genericContext, out FixedValueList2<ManagedInterfaceData> result)
        {
            result = new FixedValueList2<ManagedInterfaceData>(handles.Count);

            var i = 0;
            foreach (var handle in handles)
            {
                LoadTypeInterface(type, reader, handle, i, genericContext, out result.GetItemRef(i));
                i++;
            }
        }

        /// <summary>
        /// Loads the given interface.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="handle"></param>
        /// <returns></returns>
        void LoadTypeInterface(ManagedType type, MetadataReader reader, InterfaceImplementationHandle handle, int index, MetadataGenericContext genericContext, out ManagedInterfaceData result)
        {
            var iface = reader.GetInterfaceImplementation(handle);

            result = new ManagedInterfaceData();
            LoadTypeCustomAttributes(type, reader, iface.GetCustomAttributes(), genericContext, out result.CustomAttributes);
            result.Type = ResolveTypeSignature(reader, iface.Interface, genericContext);
        }

        /// <summary>
        /// Loads the fields from the given collection.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="reader"></param>
        /// <param name="handles"></param>
        /// <param name="genericContext"></param>
        /// <param name="result"></param>
        void LoadTypeFields(ManagedType type, MetadataReader reader, FieldDefinitionHandleCollection handles, MetadataGenericContext genericContext, out FixedValueList4<ManagedFieldData> result)
        {
            result = new FixedValueList4<ManagedFieldData>(handles.Count);

            var i = 0;
            foreach (var handle in handles)
            {
                LoadTypeField(type, reader, handle, i, genericContext, out result.GetItemRef(i));
                i++;
            }
        }

        /// <summary>
        /// Loads the given field.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="reader"></param>
        /// <param name="handle"></param>
        /// <param name="index"></param>
        /// <param name="genericContext"></param>
        /// <param name="result"></param>
        void LoadTypeField(ManagedType type, MetadataReader reader, FieldDefinitionHandle handle, int index, MetadataGenericContext genericContext, out ManagedFieldData result)
        {
            var field = reader.GetFieldDefinition(handle);

            result = new ManagedFieldData();
            result.Name = (string?)reader.GetString(field.Name);
            result.Attributes = field.Attributes;
            LoadTypeCustomAttributes(type, reader, field.GetCustomAttributes(), genericContext, out result.CustomAttributes);
            result.FieldType = field.DecodeSignature(signatureTypeProvider, genericContext);
        }

        /// <summary>
        /// Loads the methods from the given collection.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="reader"></param>
        /// <param name="handles"></param>
        /// <param name="genericContext"></param>
        /// <param name="result"></param>
        void LoadTypeMethods(ManagedType type, MetadataReader reader, MethodDefinitionHandleCollection handles, MetadataGenericContext genericContext, out FixedValueList4<ManagedMethodData> result)
        {
            result = new FixedValueList4<ManagedMethodData>(handles.Count);

            var i = 0;
            foreach (var handle in handles)
            {
                LoadTypeMethod(type, reader, handle, i, genericContext, out result.GetItemRef(i));
                i++;
            }
        }

        /// <summary>
        /// Loads the given method.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="reader"></param>
        /// <param name="handle"></param>
        /// <param name="index"></param>
        /// <param name="genericContext"></param>
        /// <param name="result"></param>
        void LoadTypeMethod(ManagedType type, MetadataReader reader, MethodDefinitionHandle handle, int index, MetadataGenericContext genericContext, out ManagedMethodData result)
        {
            var method = reader.GetMethodDefinition(handle);

            // create a generic context including the method parameters
            var genericMethodParametersRef = new FixedValueList1<ManagedGenericMethodParameterRef>(method.GetGenericParameters().Count);
            for (int i = 0; i < genericMethodParametersRef.Count; i++)
                genericMethodParametersRef[i] = new ManagedGenericMethodParameterRef(index, i);
            genericContext = new MetadataGenericContext(genericContext.TypeParameters, genericMethodParametersRef);

            // decode method data
            var signature = method.DecodeSignature(signatureTypeProvider, genericContext);

            result = new ManagedMethodData(false);
            result.Name = reader.GetString(method.Name);
            result.Attributes = method.Attributes;
            result.ImplAttributes = method.ImplAttributes;
            LoadTypeCustomAttributes(type, reader, method.GetCustomAttributes(), genericContext, out result.CustomAttributes);
            LoadTypeGenericParameters(type, reader, method.GetGenericParameters(), genericContext, out result.GenericParameters);
            LoadTypeParameters(type, reader, signature, method.GetParameters(), genericContext, out result.Parameters);
            result.ReturnType = signature.ReturnType;
        }

        /// <summary>
        /// Loads the properties from the given collection.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="handles"></param>
        /// <returns></returns>
        void LoadTypeProperties(ManagedType type, MetadataReader reader, PropertyDefinitionHandleCollection handles, MetadataGenericContext genericContext, out FixedValueList4<ManagedPropertyData> result)
        {
            result = new FixedValueList4<ManagedPropertyData>(handles.Count);

            var i = 0;
            foreach (var handle in handles)
            {
                LoadTypeProperty(type, reader, handle, i, genericContext, out result.GetItemRef(i));
                i++;
            }
        }

        /// <summary>
        /// Loads the given property.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="reader"></param>
        /// <param name="handle"></param>
        /// <param name="index"></param>
        /// <param name="genericContext"></param>
        /// <param name="result"></param>
        void LoadTypeProperty(ManagedType type, MetadataReader reader, PropertyDefinitionHandle handle, int index, MetadataGenericContext genericContext, out ManagedPropertyData result)
        {
            var property = reader.GetPropertyDefinition(handle);

            result = new ManagedPropertyData();
            result.Name = reader.GetString(property.Name);
            result.Attributes = property.Attributes;
            result.PropertyType = property.DecodeSignature(signatureTypeProvider, genericContext).ReturnType;
            LoadTypeCustomAttributes(type, reader, property.GetCustomAttributes(), genericContext, out result.CustomAttributes);
        }

        /// <summary>
        /// Loads the events from the given collection.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="handles"></param>
        /// <returns></returns>
        void LoadTypeEvents(ManagedType type, MetadataReader reader, EventDefinitionHandleCollection handles, MetadataGenericContext genericContext, out FixedValueList1<ManagedEventData> result)
        {
            result = new FixedValueList1<ManagedEventData>(handles.Count);

            var i = 0;
            foreach (var handle in handles)
            {
                LoadTypeEvent(type, reader, handle, i, genericContext, out result.GetItemRef(i));
                i++;
            }
        }

        /// <summary>
        /// Loads the given event.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="reader"></param>
        /// <param name="handle"></param>
        /// <param name="genericContext"></param>
        /// <param name="result"></param>
        void LoadTypeEvent(ManagedType type, MetadataReader reader, EventDefinitionHandle handle, int index, MetadataGenericContext genericContext, out ManagedEventData result)
        {
            result = new ManagedEventData();
            var evt = reader.GetEventDefinition(handle);
            result.Name = reader.GetString(evt.Name);
            LoadTypeCustomAttributes(type, reader, evt.GetCustomAttributes(), genericContext, out result.CustomAttributes);
            result.EventHandlerType = ResolveTypeSignature(reader, evt.Type, genericContext);
        }

        /// <summary>
        /// Loads the parameters from the given collection.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="handles"></param>
        /// <returns></returns>
        void LoadTypeParameters(ManagedType type, MetadataReader reader, MethodSignature<ManagedSignature> signature, ParameterHandleCollection handles, MetadataGenericContext genericContext, out FixedValueList4<ManagedParameterData> result)
        {
            result = new FixedValueList4<ManagedParameterData>(signature.ParameterTypes.Length);

            var i = 0;
            var e = handles.GetEnumerator();

            while (e.MoveNext())
            {
                var p = reader.GetParameter(e.Current);
                if (p.Name.IsNil)
                    continue;

                LoadParameter(type, reader, signature.ParameterTypes[i], e.Current, i, genericContext, out result.GetItemRef(i));
                i++;
            }
        }

        /// <summary>
        /// Loads the given custom attribute.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="reader"></param>
        /// <param name="parameterType"></param>
        /// <param name="handle"></param>
        /// <param name="genericContext"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        void LoadParameter(ManagedType type, MetadataReader reader, ManagedSignature? parameterType, ParameterHandle handle, int index, MetadataGenericContext genericContext, out ManagedParameterData result)
        {

            result = new ManagedParameterData();
            if (parameterType == null)
                throw new NullReferenceException();
            result.ParameterType = parameterType ?? default;

            // handle might be null, but signature might specify
            if (handle.IsNil == false)
            {
                var parameter = reader.GetParameter(handle);
                result.Name = reader.GetString(parameter.Name);
                result.Attributes = parameter.Attributes;
                LoadTypeCustomAttributes(type, reader, parameter.GetCustomAttributes(), genericContext, out result.CustomAttributes);
            }
            else
            {
                result.Name = null;
                result.Attributes = ParameterAttributes.None;
                result.CustomAttributes = new FixedValueList1<ManagedCustomAttributeData>();
            }
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

        /// <summary>
        /// Generates a full type name string for the given exported type.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        string GetExportedTypeName(MetadataReader reader, ExportedType type)
        {
            var b = new StringBuilder();

            // may be a nested type
            if (type.Implementation.Kind == HandleKind.ExportedType)
            {
                // nested exports
                throw new NotImplementedException();
            }

            // may have namespace
            if (type.Namespace.IsNil == false)
            {
                var ns = reader.GetString(type.Namespace);
                b.Append(ns).Append('.');
            }

            // append simple type name
            b.Append(reader.GetString(type.Name));

            return b.ToString();
        }

    }

}
