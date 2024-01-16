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
using System.Reflection.Metadata;

using IKVM.Reflection.Reader;

namespace IKVM.Reflection
{

    abstract class NonPEModule : Module
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="universe"></param>
        protected NonPEModule(Universe universe) :
            base(universe)
        {

        }

        protected virtual Exception InvalidOperationException()
        {
            return new InvalidOperationException();
        }

        protected virtual Exception NotSupportedException()
        {
            return new NotSupportedException();
        }

        protected virtual Exception ArgumentOutOfRangeException()
        {
            return new ArgumentOutOfRangeException();
        }

        internal sealed override Type GetModuleType()
        {
            throw InvalidOperationException();
        }

        internal sealed override ByteReader GetBlobReader(BlobHandle handle)
        {
            throw InvalidOperationException();
        }

        public sealed override AssemblyName[] __GetReferencedAssemblies()
        {
            throw NotSupportedException();
        }

        public sealed override string[] __GetReferencedModules()
        {
            throw NotSupportedException();
        }

        public override Type[] __GetReferencedTypes()
        {
            throw NotSupportedException();
        }

        public override Type[] __GetExportedTypes()
        {
            throw NotSupportedException();
        }

        protected sealed override ulong GetImageBaseImpl()
        {
            throw NotSupportedException();
        }

        protected sealed override ulong GetStackReserveImpl()
        {
            throw NotSupportedException();
        }

        protected sealed override uint GetFileAlignmentImpl()
        {
            throw NotSupportedException();
        }

        protected override DllCharacteristics GetDllCharacteristicsImpl()
        {
            throw NotSupportedException();
        }

        internal sealed override Type ResolveType(int metadataToken, IGenericContext context)
        {
            throw ArgumentOutOfRangeException();
        }

        public sealed override MethodBase ResolveMethod(int metadataToken, Type[] genericTypeArguments, Type[] genericMethodArguments)
        {
            throw ArgumentOutOfRangeException();
        }

        public sealed override FieldInfo ResolveField(int metadataToken, Type[] genericTypeArguments, Type[] genericMethodArguments)
        {
            throw ArgumentOutOfRangeException();
        }

        public sealed override MemberInfo ResolveMember(int metadataToken, Type[] genericTypeArguments, Type[] genericMethodArguments)
        {
            throw ArgumentOutOfRangeException();
        }

        public sealed override string ResolveString(int metadataToken)
        {
            throw ArgumentOutOfRangeException();
        }

        public sealed override Type[] __ResolveOptionalParameterTypes(int metadataToken, Type[] genericTypeArguments, Type[] genericMethodArguments, out CustomModifiers[] customModifiers)
        {
            throw ArgumentOutOfRangeException();
        }

    }

}
