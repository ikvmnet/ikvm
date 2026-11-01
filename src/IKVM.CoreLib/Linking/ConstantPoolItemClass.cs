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
using IKVM.CoreLib.Runtime;
using IKVM.CoreLib.Jvm;

namespace IKVM.CoreLib.Linking
{

    /// <summary>
    /// Type-model representation of a class constant.
    /// </summary>
    internal sealed class ConstantPoolItemClass<TTypeSystem, TType, TMember, TField, TMethod> :
        ConstantPoolItem<TTypeSystem, TType, TMember, TField, TMethod>,
        IEquatable<ConstantPoolItemClass<TTypeSystem, TType, TMember, TField, TMethod>>
        where TTypeSystem : struct, IJvmTypeSystem<TType, TMember, TField, TMethod>
        where TType : class
        where TMember : class
        where TField : class, TMember
        where TMethod : class, TMember
    {

        static readonly char[] InvalidJava15Characters = ['.', ';', '[', ']'];

        readonly ClassConstantData _data;
        string? _name;
        TType? _javaType;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="data"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public ConstantPoolItemClass(ClassFile<TTypeSystem, TType, TMember, TField, TMethod> context, ClassConstantData data) :
            base(context)
        {
            _data = data;
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="classFile"></param>
        /// <param name="name"></param>
        /// <param name="javaType"></param>
        public ConstantPoolItemClass(ClassFile<TTypeSystem, TType, TMember, TField, TMethod> classFile, string name, TType? javaType) :
            base(classFile)
        {
            _name = name;
            _javaType = javaType;
        }

        /// <inheritdoc />
        public override void Resolve()
        {
            // if the item was patched, we already have a name
            if (_name != null)
                return;

            _name = ClassFile.GetConstantPoolUtf8String(_data.Name);
            if (_name.Length > 0)
            {
                // We don't enforce the strict class name rules in the static compiler, since HotSpot doesn't enforce *any* rules on
                // class names for the system (and boot) class loader. We still need to enforce the 1.5 restrictions, because we
                // rely on those invariants.
                if (ClassFile.TypeSystem.IsImporter == false && ClassFile.MajorVersion < 49 && (ClassFile.Options & ClassFileParseOptions.RelaxedClassNameValidation) == 0)
                {
                    var prev = _name[0];
                    if (char.IsLetter(prev) || prev == '$' || prev == '_' || prev == '[' || prev == '/')
                    {
                        int skip = 1;
                        int end = _name.Length;
                        if (prev == '[')
                        {
                            if (!ClassFile<TTypeSystem, TType, TMember, TField, TMethod>.IsValidFieldDescriptor(_name))
                                throw new ClassFormatException("Invalid class name \"{0}\"", _name);

                            while (_name[skip] == '[')
                                skip++;

                            if (_name.EndsWith(";"))
                                end--;
                        }

                        for (int i = skip; i < end; i++)
                        {
                            var c = _name[i];
                            if (!char.IsLetterOrDigit(c) && c != '$' && c != '_' && (c != '/' || prev == '/'))
                                throw new ClassFormatException("Invalid class name \"{0}\"", _name);

                            prev = c;
                        }

                        _name = string.Intern(_name.Replace('/', '.'));
                        return;
                    }
                }
                else
                {
                    // since 1.5 the restrictions on class names have been greatly reduced
                    int start = 0;
                    int end = _name.Length;
                    if (_name[0] == '[')
                    {
                        if (!ClassFile<TTypeSystem, TType, TMember, TField, TMethod>.IsValidFieldDescriptor(_name))
                            throw new ClassFormatException("Invalid class name \"{0}\"", _name);

                        // the semicolon is only allowed at the end and IsValidFieldSig enforces this,
                        // but since invalidJava15Characters contains the semicolon, we decrement end
                        // to make the following check against invalidJava15Characters ignore the
                        // trailing semicolon.
                        if (_name[end - 1] == ';')
                            end--;

                        while (_name[start] == '[')
                            start++;
                    }

                    if (_name.IndexOfAny(InvalidJava15Characters, start, end - start) >= 0)
                        throw new ClassFormatException("Invalid class name \"{0}\"", _name);

                    _name = string.Intern(_name.Replace('/', '.'));
                    return;
                }
            }
        }

        /// <inheritdoc />
        public override void MarkLinkRequired()
        {
            _javaType ??= ClassFile.TypeSystem.TypeOfVerifierNull;
        }

        internal void LinkSelf(TType thisType)
        {
            _javaType = thisType;
        }

        /// <inheritdoc />
        public override void Link(TType thisType, LoadMode mode)
        {
            if (ReferenceEquals(_javaType, ClassFile.TypeSystem.TypeOfVerifierNull))
            {
                ref readonly var ts = ref ClassFile.TypeSystem;
                var tw = ts.LoadType(thisType, _name, mode | LoadMode.WarnClassNotFound);

                if (ts.IsImporter == false && ts.IsUnloadable(tw) == false)
                    if (ts.CheckPackageAccess(thisType, tw) == false)
                        tw = ts.CreateUnloadableType(_name);

                _javaType = tw;
            }
        }

        /// <summary>
        /// Gets the name of the class.
        /// </summary>
        public string Name => _name;

        /// <summary>
        /// Gets the resulved runtime type of the class constant.
        /// </summary>
        /// <returns></returns>
        public TType? GetClassType()
        {
            return _javaType;
        }

        /// <inheritdoc />
        public override ConstantType ConstantType => ConstantType.Class;

        /// <summary>
        /// Returns <c>true</c> if this constant is equal to the other constant.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(ConstantPoolItemClass<TTypeSystem, TType, TMember, TField, TMethod>? other)
        {
            return other != null && ReferenceEquals(_name, other._name);
        }

        /// <inheritdoc />
        public sealed override int GetHashCode()
        {
            return _name.GetHashCode();
        }

    }

}
