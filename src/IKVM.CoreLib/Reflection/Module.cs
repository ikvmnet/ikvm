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

    internal abstract class Module : ICustomAttributeProvider
    {

        readonly Universe universe;
        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="universe"></param>
        internal Module(Universe universe)
        {
            this.universe = universe;
        }

        /// <summary>
        /// Gets the universe of types this module belongs to.
        /// </summary>
        public Universe Universe => universe;

        public FieldInfo GetField(string name)
        {
            return GetField(name, BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance | BindingFlags.DeclaredOnly);
        }

        public FieldInfo GetField(string name, BindingFlags bindingFlags)
        {
            return IsResource() ? null : GetModuleType().GetField(name, bindingFlags | BindingFlags.DeclaredOnly);
        }

        public FieldInfo[] GetFields()
        {
            return GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance | BindingFlags.DeclaredOnly);
        }

        public FieldInfo[] GetFields(BindingFlags bindingFlags)
        {
            return IsResource() ? Array.Empty<FieldInfo>() : GetModuleType().GetFields(bindingFlags | BindingFlags.DeclaredOnly);
        }

        public MethodInfo GetMethod(string name)
        {
            return IsResource() ? null : GetModuleType().GetMethod(name, BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance | BindingFlags.DeclaredOnly);
        }

        public MethodInfo GetMethod(string name, Type[] types)
        {
            return IsResource() ? null : GetModuleType().GetMethod(name, BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance | BindingFlags.DeclaredOnly, null, types, null);
        }

        public MethodInfo GetMethod(string name, BindingFlags bindingAttr, Binder binder, CallingConventions callConv, Type[] types, ParameterModifier[] modifiers)
        {
            return IsResource() ? null : GetModuleType().GetMethod(name, bindingAttr | BindingFlags.DeclaredOnly, binder, callConv, types, modifiers);
        }

        public MethodInfo[] GetMethods()
        {
            return GetMethods(BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance | BindingFlags.DeclaredOnly);
        }

        public MethodInfo[] GetMethods(BindingFlags bindingFlags)
        {
            return IsResource() ? Array.Empty<MethodInfo>() : GetModuleType().GetMethods(bindingFlags | BindingFlags.DeclaredOnly);
        }

        public virtual byte[] ResolveSignature(int metadataToken)
        {
            throw new NotSupportedException();
        }

        public int MetadataToken
        {
            get { return IsResource() ? 0 : 1; }
        }

        public abstract int MDStreamVersion { get; }

        public abstract Assembly Assembly { get; }

        public abstract string FullyQualifiedName { get; }

        public abstract string Name { get; }

        public abstract Guid ModuleVersionId { get; }

        public abstract MethodBase ResolveMethod(int metadataToken, Type[] genericTypeArguments, Type[] genericMethodArguments);

        public abstract FieldInfo ResolveField(int metadataToken, Type[] genericTypeArguments, Type[] genericMethodArguments);

        public abstract MemberInfo ResolveMember(int metadataToken, Type[] genericTypeArguments, Type[] genericMethodArguments);

        public abstract string ResolveString(int metadataToken);

        public abstract string ScopeName { get; }

        internal abstract void GetTypesImpl(List<Type> list);

        internal abstract Type FindType(TypeName name);

        internal abstract Type FindTypeIgnoreCase(TypeName lowerCaseName);

        public Type GetType(string className)
        {
            return GetType(className, false, false);
        }

        public Type GetType(string className, bool ignoreCase)
        {
            return GetType(className, false, ignoreCase);
        }

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
        /// answer as the parser would. Returns <c>false</c> for any other name, and for a found type with nested parts,
        /// to leave those to the parser.
        /// </summary>
        /// <param name="className"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        bool TryFindPlainType(string className, out Type type)
        {
            type = null;

            var end = -1;
            for (int i = 0; i < className.Length; i++)
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

            type = FindType(TypeName.Split(end == -1 ? className : className.Substring(0, end)));
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

        public Type[] GetTypes()
        {
            var list = new List<Type>();
            GetTypesImpl(list);
            return list.ToArray();
        }

        public virtual bool IsResource()
        {
            return false;
        }

        public Type ResolveType(int metadataToken)
        {
            return ResolveType(metadataToken, null, null);
        }

        internal sealed class GenericContext : IGenericContext
        {

            readonly Type[] genericTypeArguments;
            readonly Type[] genericMethodArguments;

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="genericTypeArguments"></param>
            /// <param name="genericMethodArguments"></param>
            internal GenericContext(Type[] genericTypeArguments, Type[] genericMethodArguments)
            {
                this.genericTypeArguments = genericTypeArguments;
                this.genericMethodArguments = genericMethodArguments;
                }

                public Type GetGenericTypeArgument(int index)
                {
                    return genericTypeArguments[index];
                }

                public Type GetGenericMethodArgument(int index)
                {
                    return genericMethodArguments[index];
                }

        }

        public Type ResolveType(int metadataToken, Type[] genericTypeArguments, Type[] genericMethodArguments)
        {
            if ((metadataToken >> 24) == (int)TableIndex.TypeSpec)
                return ResolveType(metadataToken, new GenericContext(genericTypeArguments, genericMethodArguments));
            else
                return ResolveType(metadataToken, null);
        }

        internal abstract Type ResolveType(int metadataToken, IGenericContext context);

        public MethodBase ResolveMethod(int metadataToken)
        {
            return ResolveMethod(metadataToken, null, null);
        }

        public FieldInfo ResolveField(int metadataToken)
        {
            return ResolveField(metadataToken, null, null);
        }

        public MemberInfo ResolveMember(int metadataToken)
        {
            return ResolveMember(metadataToken, null, null);
        }

        public bool IsDefined(Type attributeType, bool inherit)
        {
            return CustomAttributeData.__GetCustomAttributes(this, attributeType, inherit).Count != 0;
        }

        public IList<CustomAttributeData> __GetCustomAttributes(Type attributeType, bool inherit)
        {
            return CustomAttributeData.__GetCustomAttributes(this, attributeType, inherit);
        }

        public IList<CustomAttributeData> GetCustomAttributesData()
        {
            return CustomAttributeData.GetCustomAttributes(this);
        }

        public abstract Type[] __GetExportedTypes();

        public virtual bool __IsMissing => false;

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

        internal abstract Type GetModuleType();

        internal virtual void Dispose()
        {
        }

        internal virtual void ExportTypes(AssemblyFileHandle handle, IKVM.Reflection.Emit.ModuleBuilder manifestModule)
        {
        }

        internal virtual string GetString(StringHandle handle)
        {
            throw new NotImplementedException();
        }

        internal virtual BlobReader GetBlobReader(BlobHandle handle)
        {
            throw new NotImplementedException();
        }

    }

    internal delegate bool MemberFilter(MemberInfo m, object filterCriteria);

}
