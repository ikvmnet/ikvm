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

            static readonly char[] invalidJava15Characters = ['.', ';', '[', ']'];

            readonly ClassConstantData _data;
            string _name;
            RuntimeJavaType _javaType;

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="context"></param>
            /// <param name="data"></param>
            /// <exception cref="ArgumentNullException"></exception>
            internal ConstantPoolItemClass(RuntimeContext context, ClassConstantData data) :
                base(context)
            {
                this._data = data;
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
                this._name = name;
                this._javaType = typeWrapper;
            }

            internal override void Resolve(ClassFile classFile, string[] utf8_cp, ClassFileParseOptions options)
            {
                // if the item was patched, we already have a name
                if (_name != null)
                    return;

                _name = classFile.GetConstantPoolUtf8String(utf8_cp, _data.Name);
                if (_name.Length > 0)
                {
                    // We don't enforce the strict class name rules in the static compiler, since HotSpot doesn't enforce *any* rules on
                    // class names for the system (and boot) class loader. We still need to enforce the 1.5 restrictions, because we
                    // rely on those invariants.
#if !IMPORTER
                    if (classFile.MajorVersion < 49 && (options & ClassFileParseOptions.RelaxedClassNameValidation) == 0)
                    {
                        var prev = _name[0];
                        if (char.IsLetter(prev) || prev == '$' || prev == '_' || prev == '[' || prev == '/')
                        {
                            int skip = 1;
                            int end = _name.Length;
                            if (prev == '[')
                            {
                                if (!IsValidFieldSig(_name))
                                    throw new ClassFormatError("Invalid class name \"{0}\"", _name);

                                while (_name[skip] == '[')
                                    skip++;

                                if (_name.EndsWith(";"))
                                    end--;
                            }

                            for (int i = skip; i < end; i++)
                            {
                                var c = _name[i];
                                if (!char.IsLetterOrDigit(c) && c != '$' && c != '_' && (c != '/' || prev == '/'))
                                    throw new ClassFormatError("Invalid class name \"{0}\"", _name);

                                prev = c;
                            }

                            _name = string.Intern(_name.Replace('/', '.'));
                            return;
                        }
                    }
                    else
#endif
                    {
                        // since 1.5 the restrictions on class names have been greatly reduced
                        int start = 0;
                        int end = _name.Length;
                        if (_name[0] == '[')
                        {
                            if (!IsValidFieldSig(_name))
                                throw new ClassFormatError("Invalid class name \"{0}\"", _name);

                            // the semicolon is only allowed at the end and IsValidFieldSig enforces this,
                            // but since invalidJava15Characters contains the semicolon, we decrement end
                            // to make the following check against invalidJava15Characters ignore the
                            // trailing semicolon.
                            if (_name[end - 1] == ';')
                                end--;

                            while (_name[start] == '[')
                                start++;
                        }

                        if (_name.IndexOfAny(invalidJava15Characters, start, end - start) >= 0)
                            throw new ClassFormatError("Invalid class name \"{0}\"", _name);

                        _name = string.Intern(_name.Replace('/', '.'));
                        return;
                    }
                }
            }

            internal override void MarkLinkRequired()
            {
                _javaType ??= Context.VerifierJavaTypeFactory.Null;
            }

            internal void LinkSelf(RuntimeJavaType thisType)
            {
                this._javaType = thisType;
            }

            internal override void Link(RuntimeJavaType thisType, LoadMode mode)
            {
                if (_javaType == Context.VerifierJavaTypeFactory.Null)
                {
                    var tw = thisType.ClassLoader.LoadClass(_name, mode | LoadMode.WarnClassNotFound);

#if IMPORTER == false && FIRST_PASS == false
                    if (!tw.IsUnloadable)
                    {
                        try
                        {
                            thisType.ClassLoader.CheckPackageAccess(tw, thisType.ClassObject.pd);
                        }
                        catch (java.lang.SecurityException)
                        {
                            tw = new RuntimeUnloadableJavaType(Context, _name);
                        }
                    }
#endif

                    _javaType = tw;
                }
            }

            internal string Name => _name;

            internal RuntimeJavaType GetClassType()
            {
                return _javaType;
            }

            internal override ConstantType GetConstantType()
            {
                return ConstantType.Class;
            }

            public sealed override int GetHashCode()
            {
                return _name.GetHashCode();
            }

            public bool Equals(ConstantPoolItemClass other)
            {
                return ReferenceEquals(_name, other._name);
            }

        }

    }

}
