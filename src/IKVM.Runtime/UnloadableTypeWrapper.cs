/*
  Copyright (C) 2002-2015 Jeroen Frijters

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

#if IMPORTER || EXPORTER
using IKVM.Reflection;
using IKVM.Reflection.Emit;

using Type = IKVM.Reflection.Type;
#else
#endif

#if IMPORTER
using IKVM.Tools.Importer;
#endif

namespace IKVM.Internal
{
    sealed class UnloadableTypeWrapper : TypeWrapper
    {

        internal const string ContainerTypeName = "__<Unloadable>";
        readonly Type missingType;
        Type customModifier;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="name"></param>
        internal UnloadableTypeWrapper(string name) :
            base(TypeFlags.None, TypeWrapper.UnloadableModifiersHack, name)
        {

        }

        internal UnloadableTypeWrapper(Type missingType) :
            this(missingType.FullName) // TODO demangle and re-mangle appropriately
        {
            this.missingType = missingType;
        }

        internal UnloadableTypeWrapper(string name, Type customModifier) :
            this(name)
        {
            this.customModifier = customModifier;
        }

        internal override TypeWrapper BaseTypeWrapper
        {
            get { return null; }
        }

        internal override ClassLoaderWrapper GetClassLoader()
        {
            return null;
        }

        internal override TypeWrapper EnsureLoadable(ClassLoaderWrapper loader)
        {
            var tw = loader.LoadClassByDottedNameFast(this.Name);
            if (tw == null)
                throw new NoClassDefFoundError(this.Name);

            return tw;
        }

        internal override string SigName
        {
            get
            {
                var name = Name;
                if (name.StartsWith("["))
                    return name;

                return "L" + name + ";";
            }
        }

        protected override void LazyPublishMembers()
        {
            throw new InvalidOperationException("LazyPublishMembers called on UnloadableTypeWrapper: " + Name);
        }

        internal override Type TypeAsTBD
        {
            get
            {
                throw new InvalidOperationException("get_Type called on UnloadableTypeWrapper: " + Name);
            }
        }

        internal override TypeWrapper[] Interfaces
        {
            get
            {
#if IMPORTER
                if (missingType != null)
                {
                    StaticCompiler.IssueMissingTypeMessage(missingType);
                    return TypeWrapper.EmptyArray;
                }
#endif

                throw new InvalidOperationException("get_Interfaces called on UnloadableTypeWrapper: " + Name);
            }
        }

        internal override TypeWrapper[] InnerClasses
        {
            get
            {
                throw new InvalidOperationException("get_InnerClasses called on UnloadableTypeWrapper: " + Name);
            }
        }

        internal override TypeWrapper DeclaringTypeWrapper
        {
            get
            {
                throw new InvalidOperationException("get_DeclaringTypeWrapper called on UnloadableTypeWrapper: " + Name);
            }
        }

        internal override void Finish()
        {
            throw new InvalidOperationException("Finish called on UnloadableTypeWrapper: " + Name);
        }

        internal Type MissingType
        {
            get { return missingType; }
        }

        internal Type CustomModifier
        {
            get { return customModifier; }
        }

        internal void SetCustomModifier(Type type)
        {
            this.customModifier = type;
        }

#if EMITTERS

        internal Type GetCustomModifier(TypeWrapperFactory context)
        {
            // we don't need to lock, because we're only supposed to be called while holding the finish lock
            return customModifier ?? (customModifier = context.DefineUnloadable(this.Name));
        }

        internal override void EmitCheckcast(CodeEmitter ilgen)
        {
            throw new InvalidOperationException("EmitCheckcast called on UnloadableTypeWrapper: " + Name);
        }

        internal override void EmitInstanceOf(CodeEmitter ilgen)
        {
            throw new InvalidOperationException("EmitInstanceOf called on UnloadableTypeWrapper: " + Name);
        }

#endif // EMITTERS

    }

}
