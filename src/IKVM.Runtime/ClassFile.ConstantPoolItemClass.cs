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

using IKVM.ByteCode.Decoding;

namespace IKVM.Runtime
{

    sealed partial class ClassFile
    {

        internal sealed class ConstantPoolItemClass : ConstantPoolItem, IEquatable<ConstantPoolItemClass>
        {

            static readonly char[] invalidJava15Characters = { '.', ';', '[', ']' };

            readonly ClassConstantData data;

            string name;
            RuntimeJavaType typeWrapper;

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="context"></param>
            /// <param name="data"></param>
            /// <exception cref="ArgumentNullException"></exception>
            internal ConstantPoolItemClass(RuntimeContext context, ClassConstantData data) :
                base(context)
            {
                this.data = data;
            }

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="context"></param>
            /// <param name="name"></param>
            /// <param name="typeWrapper"></param>
            internal ConstantPoolItemClass(RuntimeContext context, string name, RuntimeJavaType typeWrapper) :
                base(context)
            {
                this.name = name;
                this.typeWrapper = typeWrapper;
            }

            internal override void Resolve(ClassFile classFile, string[] utf8_cp, ClassFileParseOptions options)
            {
                // if the item was patched, we already have a name
                if (name != null)
                    return;

                name = classFile.GetConstantPoolUtf8String(utf8_cp, data.Name);
                if (name.Length > 0)
                {
                    // We don't enforce the strict class name rules in the static compiler, since HotSpot doesn't enforce *any* rules on
                    // class names for the system (and boot) class loader. We still need to enforce the 1.5 restrictions, because we
                    // rely on those invariants.
#if !IMPORTER
                    if (classFile.MajorVersion < 49 && (options & ClassFileParseOptions.RelaxedClassNameValidation) == 0)
                    {
                        char prev = name[0];
                        if (Char.IsLetter(prev) || prev == '$' || prev == '_' || prev == '[' || prev == '/')
                        {
                            int skip = 1;
                            int end = name.Length;
                            if (prev == '[')
                            {
                                if (!IsValidFieldSig(name))
                                {
                                    goto barf;
                                }
                                while (name[skip] == '[')
                                {
                                    skip++;
                                }
                                if (name.EndsWith(";"))
                                {
                                    end--;
                                }
                            }
                            for (int i = skip; i < end; i++)
                            {
                                char c = name[i];
                                if (!Char.IsLetterOrDigit(c) && c != '$' && c != '_' && (c != '/' || prev == '/'))
                                {
                                    goto barf;
                                }
                                prev = c;
                            }
                            name = String.Intern(name.Replace('/', '.'));
                            return;
                        }
                    }
                    else
#endif
                    {
                        // since 1.5 the restrictions on class names have been greatly reduced
                        int start = 0;
                        int end = name.Length;
                        if (name[0] == '[')
                        {
                            if (!IsValidFieldSig(name))
                            {
                                goto barf;
                            }
                            // the semicolon is only allowed at the end and IsValidFieldSig enforces this,
                            // but since invalidJava15Characters contains the semicolon, we decrement end
                            // to make the following check against invalidJava15Characters ignore the
                            // trailing semicolon.
                            if (name[end - 1] == ';')
                            {
                                end--;
                            }
                            while (name[start] == '[')
                            {
                                start++;
                            }
                        }

                        if (name.IndexOfAny(invalidJava15Characters, start, end - start) >= 0)
                        {
                            goto barf;
                        }

                        name = string.Intern(name.Replace('/', '.'));
                        return;
                    }
                }
            barf:
                throw new ClassFormatError("Invalid class name \"{0}\"", name);
            }

            internal override void MarkLinkRequired()
            {
                typeWrapper ??= Context.VerifierJavaTypeFactory.Null;
            }

            internal void LinkSelf(RuntimeJavaType thisType)
            {
                this.typeWrapper = thisType;
            }

            internal override void Link(RuntimeJavaType thisType, LoadMode mode)
            {
                if (typeWrapper == Context.VerifierJavaTypeFactory.Null)
                {
                    var tw = thisType.ClassLoader.LoadClass(name, mode | LoadMode.WarnClassNotFound);
#if !IMPORTER && !FIRST_PASS
                    if (!tw.IsUnloadable)
                    {
                        try
                        {
                            thisType.ClassLoader.CheckPackageAccess(tw, thisType.ClassObject.pd);
                        }
                        catch (java.lang.SecurityException)
                        {
                            tw = new RuntimeUnloadableJavaType(Context, name);
                        }
                    }
#endif
                    typeWrapper = tw;
                }
            }

            internal string Name
            {
                get
                {
                    return name;
                }
            }

            internal RuntimeJavaType GetClassType()
            {
                return typeWrapper;
            }

            internal override ConstantType GetConstantType()
            {
                return ConstantType.Class;
            }

            public sealed override int GetHashCode()
            {
                return name.GetHashCode();
            }

            public bool Equals(ConstantPoolItemClass other)
            {
                return ReferenceEquals(name, other.name);
            }

        }

    }

}
