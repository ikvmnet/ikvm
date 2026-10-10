/*
  Copyright (C) 2009-2013 Jeroen Frijters

  This software is provided 'as-is', without any express or implied
  warranty.  In no event will the authors be held liable for any damages
  arising from the use of this software.

  Permission is granted to anyone to use this software for any purpose,
  including commercial applications, and to alter it and redistribute it
  freely, subject to the following restrictions:

  1. The origin of this software must not be misrepresented; you must not
     claim that you wrote the original software. If you use this software
     in a product, an acknowledgment in the product documentation would be
     appreciated but is not required.
  2. Altered source versions must be plainly marked as such, and must not be
     misrepresented as being the original software.
  3. This notice may not be removed or altered from any source distribution.

  Jeroen Frijters
  jeroen@frijters.net
  
*/
using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;

using IKVM.Reflection.Reader;

namespace IKVM.Reflection
{

    /// <summary>
    /// Performs reflection on a module.
    /// </summary>
    abstract class Module : ICustomAttributeProvider
    {

        readonly Universe _universe;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="universe">The universe of types this module belongs to.</param>
        internal Module(Universe universe)
        {
            _universe = universe;
        }

        /// <summary>
        /// Gets the universe of types this module belongs to.
        /// </summary>
        public Universe Universe => _universe;

        /// <summary>
        /// Returns a field having the specified name.
        /// </summary>
        /// <param name="name">The field name.</param>
        /// <returns>A <see cref="FieldInfo"/> object having the specified name, or <see langword="null"/> if the field does not exist.</returns>
        public FieldInfo GetField(string name) =>
            GetField(name, BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance | BindingFlags.DeclaredOnly);

        /// <summary>
        /// Returns a field having the specified name and binding attributes.
        /// </summary>
        /// <param name="name">The field name.</param>
        /// <param name="bindingFlags">One of the <see cref="BindingFlags"/> bit flags used to control the search.</param>
        /// <returns>A <see cref="FieldInfo"/> object having the specified name and binding attributes, or <see langword="null"/> if the field does not exist.</returns>
        public FieldInfo GetField(string name, BindingFlags bindingFlags) =>
            IsResource() ? null : GetModuleType().GetField(name, bindingFlags | BindingFlags.DeclaredOnly);

        /// <summary>
        /// Returns the global fields defined on the module.
        /// </summary>
        /// <returns>An array of <see cref="FieldInfo"/> objects representing the global fields defined on the module; if there are no global fields, an empty array is returned.</returns>
        public FieldInfo[] GetFields() => GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance | BindingFlags.DeclaredOnly);

        /// <summary>
        /// Returns the global fields defined on the module that match the specified binding flags.
        /// </summary>
        /// <param name="bindingFlags">A bitwise combination of the enumeration values that limit the search.</param>
        /// <returns>An array of type <see cref="FieldInfo"/> representing the global fields defined on the module that match the specified binding flags; if no global fields match the binding flags, an empty array is returned.</returns>
        public FieldInfo[] GetFields(BindingFlags bindingFlags) =>
            IsResource() ? Array.Empty<FieldInfo>() : GetModuleType().GetFields(bindingFlags | BindingFlags.DeclaredOnly);

        /// <summary>
        /// Returns a method having the specified name.
        /// </summary>
        /// <param name="name">The method name.</param>
        /// <returns>A <see cref="MethodInfo"/> object having the specified name, or <see langword="null"/> if the method does not exist.</returns>
        public MethodInfo GetMethod(string name) =>
            IsResource() ? null : GetModuleType().GetMethod(name, BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance | BindingFlags.DeclaredOnly);

        /// <summary>
        /// Returns a method having the specified name and parameter types.
        /// </summary>
        /// <param name="name">The method name.</param>
        /// <param name="types">The parameter types to search for.</param>
        /// <returns>A <see cref="MethodInfo"/> object in accordance with the specified criteria, or <see langword="null"/> if the method does not exist.</returns>
        public MethodInfo GetMethod(string name, Type[] types) =>
            IsResource() ? null : GetModuleType().GetMethod(name, BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance | BindingFlags.DeclaredOnly, null, types, null);

        /// <summary>
        /// Returns a method having the specified name, binding information, calling convention, and parameter types and modifiers.
        /// </summary>
        /// <param name="name">The method name.</param>
        /// <param name="bindingAttr">One of the <see cref="BindingFlags"/> bit flags used to control the search.</param>
        /// <param name="binder">An object that implements <see cref="Binder"/>, containing properties related to this method.</param>
        /// <param name="callConv">The calling convention for the method.</param>
        /// <param name="types">The parameter types to search for.</param>
        /// <param name="modifiers">An array of parameter modifiers used to make binding work with parameter signatures in which the types have been modified.</param>
        /// <returns>A <see cref="MethodInfo"/> object in accordance with the specified criteria, or <see langword="null"/> if the method does not exist.</returns>
        public MethodInfo GetMethod(string name, BindingFlags bindingAttr, Binder binder, CallingConventions callConv, Type[] types, ParameterModifier[] modifiers) =>
            IsResource() ? null : GetModuleType().GetMethod(name, bindingAttr | BindingFlags.DeclaredOnly, binder, callConv, types, modifiers);

        /// <summary>
        /// Returns the global methods defined on the module.
        /// </summary>
        /// <returns>An array of <see cref="MethodInfo"/> objects representing all the global methods defined on the module; if there are no global methods, an empty array is returned.</returns>
        public MethodInfo[] GetMethods() => GetMethods(BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance | BindingFlags.DeclaredOnly);

        /// <summary>
        /// Returns the global methods defined on the module that match the specified binding flags.
        /// </summary>
        /// <param name="bindingFlags">A bitwise combination of the enumeration values that limit the search.</param>
        /// <returns>An array of type <see cref="MethodInfo"/> representing the global methods defined on the module that match the specified binding flags; if no global methods match the binding flags, an empty array is returned.</returns>
        public MethodInfo[] GetMethods(BindingFlags bindingFlags) =>
            IsResource() ? Array.Empty<MethodInfo>() : GetModuleType().GetMethods(bindingFlags | BindingFlags.DeclaredOnly);

        /// <summary>
        /// Returns the signature blob identified by a metadata token.
        /// </summary>
        /// <param name="metadataToken">A metadata token that identifies a signature in the module.</param>
        /// <returns>An array of bytes representing the signature blob.</returns>
        /// <exception cref="NotSupportedException">The module does not support resolving signatures.</exception>
        public virtual byte[] ResolveSignature(int metadataToken) => throw new NotSupportedException();

        /// <summary>
        /// Gets a token that identifies the module in metadata.
        /// </summary>
        public int MetadataToken => IsResource() ? 0 : 1;

        /// <summary>
        /// Gets the metadata stream version.
        /// </summary>
        public abstract int MDStreamVersion { get; }

        /// <summary>
        /// Gets the appropriate <see cref="IKVM.Reflection.Assembly"/> for this instance of <see cref="Module"/>.
        /// </summary>
        public abstract Assembly Assembly { get; }

        /// <summary>
        /// Gets a string representing the fully qualified name and path to this module.
        /// </summary>
        public abstract string FullyQualifiedName { get; }

        /// <summary>
        /// Gets a string representing the name of the module with the path removed.
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// Gets a universally unique identifier (UUID) that can be used to distinguish between two versions of a module.
        /// </summary>
        public abstract Guid ModuleVersionId { get; }

        /// <summary>
        /// Returns the method or constructor identified by the specified metadata token, in the context defined by the specified generic type parameters.
        /// </summary>
        /// <param name="metadataToken">A metadata token that identifies a method or constructor in the module.</param>
        /// <param name="genericTypeArguments">An array of <see cref="Type"/> objects representing the generic type arguments of the type where the token is in scope, or <see langword="null"/> if that type is not generic.</param>
        /// <param name="genericMethodArguments">An array of <see cref="Type"/> objects representing the generic type arguments of the method where the token is in scope, or <see langword="null"/> if that method is not generic.</param>
        /// <returns>A <see cref="MethodBase"/> object representing the method that is identified by the specified metadata token.</returns>
        public abstract MethodBase ResolveMethod(int metadataToken, Type[] genericTypeArguments, Type[] genericMethodArguments);

        /// <summary>
        /// Returns the field identified by the specified metadata token, in the context defined by the specified generic type parameters.
        /// </summary>
        /// <param name="metadataToken">A metadata token that identifies a field in the module.</param>
        /// <param name="genericTypeArguments">An array of <see cref="Type"/> objects representing the generic type arguments of the type where the token is in scope, or <see langword="null"/> if that type is not generic.</param>
        /// <param name="genericMethodArguments">An array of <see cref="Type"/> objects representing the generic type arguments of the method where the token is in scope, or <see langword="null"/> if that method is not generic.</param>
        /// <returns>A <see cref="FieldInfo"/> object representing the field that is identified by the specified metadata token.</returns>
        public abstract FieldInfo ResolveField(int metadataToken, Type[] genericTypeArguments, Type[] genericMethodArguments);

        /// <summary>
        /// Returns the type or member identified by the specified metadata token, in the context defined by the specified generic type parameters.
        /// </summary>
        /// <param name="metadataToken">A metadata token that identifies a type or member in the module.</param>
        /// <param name="genericTypeArguments">An array of <see cref="Type"/> objects representing the generic type arguments of the type where the token is in scope, or <see langword="null"/> if that type is not generic.</param>
        /// <param name="genericMethodArguments">An array of <see cref="Type"/> objects representing the generic type arguments of the method where the token is in scope, or <see langword="null"/> if that method is not generic.</param>
        /// <returns>A <see cref="MemberInfo"/> object representing the type or member that is identified by the specified metadata token.</returns>
        public abstract MemberInfo ResolveMember(int metadataToken, Type[] genericTypeArguments, Type[] genericMethodArguments);

        /// <summary>
        /// Returns the string identified by the specified metadata token.
        /// </summary>
        /// <param name="metadataToken">A metadata token that identifies a string in the string heap of the module.</param>
        /// <returns>A <see cref="string"/> containing a string value from the metadata string heap.</returns>
        public abstract string ResolveString(int metadataToken);

        /// <summary>
        /// Gets a string representing the name of the module.
        /// </summary>
        public abstract string ScopeName { get; }

        /// <summary>
        /// Adds the types defined in this module to <paramref name="list"/>.
        /// </summary>
        /// <param name="list">The list to add the types to.</param>
        internal abstract void GetTypesImpl(List<Type> list);

        /// <summary>
        /// Finds a type defined in this module by its name.
        /// </summary>
        /// <param name="name">The namespace and name of the type.</param>
        /// <returns>The type, or <see langword="null"/> if this module does not define it.</returns>
        internal abstract Type FindType(TypeName name);

        /// <summary>
        /// Finds a type defined in this module by its name, ignoring case.
        /// </summary>
        /// <param name="lowerCaseName">The namespace and name of the type, converted to lower case.</param>
        /// <returns>The type, or <see langword="null"/> if this module does not define it.</returns>
        internal abstract Type FindTypeIgnoreCase(TypeName lowerCaseName);

        /// <summary>
        /// Returns the specified type, performing a case-sensitive search.
        /// </summary>
        /// <param name="className">The name of the type to locate. The name must be fully qualified with the namespace.</param>
        /// <returns>A <see cref="Type"/> object representing the given type, if the type is in this module; otherwise, <see langword="null"/>.</returns>
        public Type GetType(string className) => GetType(className, false, false);

        /// <summary>
        /// Returns the specified type, searching the module with the specified case sensitivity.
        /// </summary>
        /// <param name="className">The name of the type to locate. The name must be fully qualified with the namespace.</param>
        /// <param name="ignoreCase"><see langword="true"/> for case-insensitive search; otherwise, <see langword="false"/>.</param>
        /// <returns>A <see cref="Type"/> object representing the given type, if the type is in this module; otherwise, <see langword="null"/>.</returns>
        public Type GetType(string className, bool ignoreCase) => GetType(className, false, ignoreCase);

        /// <summary>
        /// Returns the specified type, specifying whether to make a case-sensitive search of the module and whether to throw an exception if the type cannot be found.
        /// </summary>
        /// <param name="className">The name of the type to locate. The name must be fully qualified with the namespace.</param>
        /// <param name="throwOnError"><see langword="true"/> to throw an exception if an error occurs while loading the type; <see langword="false"/> to return <see langword="null"/>.</param>
        /// <param name="ignoreCase"><see langword="true"/> for case-insensitive search; otherwise, <see langword="false"/>.</param>
        /// <returns>A <see cref="Type"/> object representing the specified type, if the type is declared in this module; otherwise, <see langword="null"/>.</returns>
        /// <exception cref="ArgumentException"><paramref name="className"/> specifies an assembly name and <paramref name="throwOnError"/> is <see langword="true"/>.</exception>
        /// <exception cref="MissingModuleException">The type is not found and this module is missing.</exception>
        public Type GetType(string className, bool throwOnError, bool ignoreCase)
        {
            // the importer asks every referenced assembly about each class name, so most lookups are of plain names that fail
            if (throwOnError == false && ignoreCase == false && TryFindPlainType(className, out var plain))
                return plain;

            var parser = TypeNameParser.Parse(className, throwOnError);
            if (parser.Error)
                return null;

            if (parser.AssemblyName != null)
            {
                if (throwOnError)
                    throw new ArgumentException("Type names passed to Module.GetType() must not specify an assembly.");
                else
                    return null;
            }

            var typeName = TypeName.Split(TypeNameParser.Unescape(parser.FirstNamePart));
            var type = ignoreCase ? FindTypeIgnoreCase(typeName.ToLowerInvariant()) : FindType(typeName);
            if (type == null && __IsMissing)
                throw new MissingModuleException((MissingModule)this);

            return parser.Expand(type, this, throwOnError, className, false, ignoreCase);
        }

        /// <summary>
        /// Looks up a name without escapes, assembly name, generic arguments, modifiers or white space, giving the same
        /// answer as the parser would. Returns <see langword="false"/> for any other name, and for a found type with
        /// nested parts, to leave those to the parser.
        /// </summary>
        /// <param name="className">The name of the type to locate.</param>
        /// <param name="type">When this method returns <see langword="true"/>, the type found, or <see langword="null"/> if this module does not define it.</param>
        /// <returns><see langword="true"/> if the lookup gave a definitive answer; <see langword="false"/> if the name must be handled by the parser.</returns>
        /// <exception cref="MissingModuleException">The type is not found and this module is missing.</exception>
        bool TryFindPlainType(string className, out Type type)
        {
            type = null;

            var end = -1;
            for (var i = 0; i < className.Length; i++)
            {
                var c = className[i];
                if (c == '+')
                {
                    // an empty part is a parse error, which returns null without the missing module check below
                    if (i == 0 || i == className.Length - 1 || className[i - 1] == '+')
                        return false;

                    if (end == -1)
                        end = i;
                }
                else if (c is '\\' or ',' or '[' or ']' or '*' or '&' || char.IsWhiteSpace(c))
                {
                    return false;
                }
            }

            if (className.Length == 0)
                return false;

            type = FindType(TypeName.Split(end == -1 ? className : className[..end]));
            if (type == null)
            {
                if (__IsMissing)
                    throw new MissingModuleException((MissingModule)this);

                return true;
            }

            if (end == -1)
                return true;

            type = null;
            return false;
        }

        /// <summary>
        /// Returns all the types defined within this module.
        /// </summary>
        /// <returns>An array of type <see cref="Type"/> containing types defined within the module that is reflected by this instance.</returns>
        public Type[] GetTypes()
        {
            var list = new List<Type>();
            GetTypesImpl(list);
            return list.ToArray();
        }

        /// <summary>
        /// Gets a value indicating whether the object is a resource.
        /// </summary>
        /// <returns><see langword="true"/> if the object is a resource; otherwise, <see langword="false"/>.</returns>
        public virtual bool IsResource() => false;

        /// <summary>
        /// Returns the type identified by the specified metadata token.
        /// </summary>
        /// <param name="metadataToken">A metadata token that identifies a type in the module.</param>
        /// <returns>A <see cref="Type"/> object representing the type that is identified by the specified token.</returns>
        public Type ResolveType(int metadataToken) => ResolveType(metadataToken, null, null);

        /// <summary>
        /// A generic context backed by arrays of generic type and method arguments, used to resolve TypeSpec tokens.
        /// </summary>
        internal sealed class GenericContext : IGenericContext
        {

            readonly Type[] _genericTypeArguments;
            readonly Type[] _genericMethodArguments;

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="genericTypeArguments">The generic type arguments, or <see langword="null"/>.</param>
            /// <param name="genericMethodArguments">The generic method arguments, or <see langword="null"/>.</param>
            internal GenericContext(Type[] genericTypeArguments, Type[] genericMethodArguments)
            {
                _genericTypeArguments = genericTypeArguments;
                _genericMethodArguments = genericMethodArguments;
            }

            /// <inheritdoc />
            public Type GetGenericTypeArgument(int index) => _genericTypeArguments[index];

            /// <inheritdoc />
            public Type GetGenericMethodArgument(int index) => _genericMethodArguments[index];

        }

        /// <summary>
        /// Returns the type identified by the specified metadata token, in the context defined by the specified generic type parameters.
        /// </summary>
        /// <param name="metadataToken">A metadata token that identifies a type in the module.</param>
        /// <param name="genericTypeArguments">An array of <see cref="Type"/> objects representing the generic type arguments of the type where the token is in scope, or <see langword="null"/> if that type is not generic.</param>
        /// <param name="genericMethodArguments">An array of <see cref="Type"/> objects representing the generic type arguments of the method where the token is in scope, or <see langword="null"/> if that method is not generic.</param>
        /// <returns>A <see cref="Type"/> object representing the type that is identified by the specified token.</returns>
        public Type ResolveType(int metadataToken, Type[] genericTypeArguments, Type[] genericMethodArguments)
        {
            if ((metadataToken >> 24) == (int)TableIndex.TypeSpec)
                return ResolveType(metadataToken, new GenericContext(genericTypeArguments, genericMethodArguments));
            else
                return ResolveType(metadataToken, null);
        }

        /// <summary>
        /// Returns the type identified by the specified metadata token, resolving generic parameters through <paramref name="context"/>.
        /// </summary>
        /// <param name="metadataToken">A metadata token that identifies a type in the module.</param>
        /// <param name="context">The generic context, or <see langword="null"/>.</param>
        /// <returns>The type that is identified by the specified token.</returns>
        internal abstract Type ResolveType(int metadataToken, IGenericContext context);

        /// <summary>
        /// Returns the method or constructor identified by the specified metadata token.
        /// </summary>
        /// <param name="metadataToken">A metadata token that identifies a method or constructor in the module.</param>
        /// <returns>A <see cref="MethodBase"/> object representing the method or constructor that is identified by the specified metadata token.</returns>
        public MethodBase ResolveMethod(int metadataToken) => ResolveMethod(metadataToken, null, null);

        /// <summary>
        /// Returns the field identified by the specified metadata token.
        /// </summary>
        /// <param name="metadataToken">A metadata token that identifies a field in the module.</param>
        /// <returns>A <see cref="FieldInfo"/> object representing the field that is identified by the specified metadata token.</returns>
        public FieldInfo ResolveField(int metadataToken) => ResolveField(metadataToken, null, null);

        /// <summary>
        /// Returns the type or member identified by the specified metadata token.
        /// </summary>
        /// <param name="metadataToken">A metadata token that identifies a type or member in the module.</param>
        /// <returns>A <see cref="MemberInfo"/> object representing the type or member that is identified by the specified metadata token.</returns>
        public MemberInfo ResolveMember(int metadataToken) => ResolveMember(metadataToken, null, null);

        /// <summary>
        /// Returns a value that indicates whether the specified attribute type has been applied to this module.
        /// </summary>
        /// <param name="attributeType">The type of custom attribute to test for.</param>
        /// <param name="inherit">This argument is ignored for objects of this type.</param>
        /// <returns><see langword="true"/> if one or more instances of <paramref name="attributeType"/> have been applied to this module; otherwise, <see langword="false"/>.</returns>
        public bool IsDefined(Type attributeType, bool inherit) => CustomAttributeData.__GetCustomAttributes(this, attributeType, inherit).Count != 0;

        /// <summary>
        /// Returns the custom attributes of the specified type that have been applied to this module.
        /// </summary>
        /// <param name="attributeType">The type of custom attribute to return, or <see langword="null"/> for all custom attributes.</param>
        /// <param name="inherit">This argument is ignored for modules.</param>
        /// <returns>A list of <see cref="CustomAttributeData"/> objects for the matching custom attributes.</returns>
        public IList<CustomAttributeData> __GetCustomAttributes(Type attributeType, bool inherit) =>
            CustomAttributeData.__GetCustomAttributes(this, attributeType, inherit);

        /// <summary>
        /// Returns a list of <see cref="CustomAttributeData"/> objects for the current module, which can be used in the reflection-only context.
        /// </summary>
        /// <returns>A generic list of <see cref="CustomAttributeData"/> objects representing data about the attributes that have been applied to the current module.</returns>
        public IList<CustomAttributeData> GetCustomAttributesData() => CustomAttributeData.GetCustomAttributes(this);

        /// <summary>
        /// Returns the types listed in the ExportedType table of this module.
        /// </summary>
        /// <returns>The exported types.</returns>
        public abstract Type[] __GetExportedTypes();

        /// <summary>
        /// Gets a value indicating whether this module is a placeholder for a module that could not be found.
        /// </summary>
        public virtual bool __IsMissing => false;

        /// <summary>
        /// Gets the P/Invoke mapping (ImplMap) information for a method defined in this module.
        /// </summary>
        /// <param name="token">The metadata token of the method.</param>
        /// <param name="mappingFlags">When this method returns, the P/Invoke attributes of the mapping.</param>
        /// <param name="importName">When this method returns, the name of the imported entry point.</param>
        /// <param name="importScope">When this method returns, the name of the module the entry point is imported from.</param>
        /// <returns><see langword="true"/> if the method has P/Invoke mapping information; otherwise, <see langword="false"/>.</returns>
        public bool __TryGetImplMap(int token, out ImplMapFlags mappingFlags, out string importName, out string importScope)
        {
            if (this is Emit.ModuleBuilder builder)
                return builder.TryGetImplMap(token, out mappingFlags, out importName, out importScope);

            // only methods are imported in practice; System.Reflection.Metadata has no API for imported fields
            if (this is ModuleReader reader && (token >> 24) == (int)TableIndex.MethodDef && (token & 0xFFFFFF) != 0)
            {
                var import = reader.Metadata.GetMethodDefinition(System.Reflection.Metadata.Ecma335.MetadataTokens.MethodDefinitionHandle(token & 0xFFFFFF)).GetImport();
                if (import.Module.IsNil == false)
                {
                    mappingFlags = (ImplMapFlags)(ushort)import.Attributes;
                    importName = GetString(import.Name);
                    importScope = GetString(reader.Metadata.GetModuleReference(import.Module).Name);
                    return true;
                }
            }

            mappingFlags = 0;
            importName = null;
            importScope = null;
            return false;
        }

        /// <summary>
        /// Gets the <c>&lt;Module&gt;</c> type that holds the global fields and methods of this module.
        /// </summary>
        /// <returns>The module type.</returns>
        internal abstract Type GetModuleType();

        /// <summary>
        /// Releases the resources, such as the file or stream, held by this module.
        /// </summary>
        internal virtual void Dispose()
        {
        }

        /// <summary>
        /// Adds the public types of this module to the ExportedType table of <paramref name="manifestModule"/>, when this
        /// module is a non-manifest module of an assembly being built.
        /// </summary>
        /// <param name="handle">The File table handle of this module in the manifest module.</param>
        /// <param name="manifestModule">The manifest module of the assembly.</param>
        internal virtual void ExportTypes(AssemblyFileHandle handle, IKVM.Reflection.Emit.ModuleBuilder manifestModule)
        {
        }

        /// <summary>
        /// Gets a string from the string heap of this module.
        /// </summary>
        /// <param name="handle">The handle of the string.</param>
        /// <returns>The string.</returns>
        /// <exception cref="NotImplementedException">The module does not have a string heap.</exception>
        internal virtual string GetString(StringHandle handle) => throw new NotImplementedException();

        /// <summary>
        /// Gets a reader for a blob in the blob heap of this module.
        /// </summary>
        /// <param name="handle">The handle of the blob.</param>
        /// <returns>A reader positioned at the start of the blob.</returns>
        /// <exception cref="NotImplementedException">The module does not have a blob heap.</exception>
        internal virtual BlobReader GetBlobReader(BlobHandle handle) => throw new NotImplementedException();

    }

    /// <summary>
    /// Represents a delegate that is used to filter a list of members represented in an array of <see cref="MemberInfo"/> objects.
    /// </summary>
    /// <param name="m">The <see cref="MemberInfo"/> object to which the filter is applied.</param>
    /// <param name="filterCriteria">An arbitrary object used to filter the list.</param>
    /// <returns><see langword="true"/> to include the member in the filtered list; otherwise, <see langword="false"/>.</returns>
    delegate bool MemberFilter(MemberInfo m, object filterCriteria);

}
