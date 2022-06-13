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

namespace IKVM.Internal
{

    sealed partial class ClassFile
	{
        internal sealed class ConstantPoolItemClass : ConstantPoolItem, IEquatable<ConstantPoolItemClass>
		{
			private ushort name_index;
			private string name;
			private TypeWrapper typeWrapper;
			private static char[] invalidJava15Characters = { '.', ';', '[', ']' };

			internal ConstantPoolItemClass(BigEndianBinaryReader br)
			{
				name_index = br.ReadUInt16();
			}

			internal ConstantPoolItemClass(string name, TypeWrapper typeWrapper)
			{
				this.name = name;
				this.typeWrapper = typeWrapper;
			}

			internal override void Resolve(ClassFile classFile, string[] utf8_cp, ClassFileParseOptions options)
			{
				// if the item was patched, we already have a name
				if(name != null)
				{
					return;
				}
				name = classFile.GetConstantPoolUtf8String(utf8_cp, name_index);
				if(name.Length > 0)
				{
					// We don't enforce the strict class name rules in the static compiler, since HotSpot doesn't enforce *any* rules on
					// class names for the system (and boot) class loader. We still need to enforce the 1.5 restrictions, because we
					// rely on those invariants.
#if !STATIC_COMPILER
					if(classFile.MajorVersion < 49 && (options & ClassFileParseOptions.RelaxedClassNameValidation) == 0)
					{
						char prev = name[0];
						if(Char.IsLetter(prev) || prev == '$' || prev == '_' || prev == '[' || prev == '/')
						{
							int skip = 1;
							int end = name.Length;
							if(prev == '[')
							{
								if(!IsValidFieldSig(name))
								{
									goto barf;
								}
								while(name[skip] == '[')
								{
									skip++;
								}
								if(name.EndsWith(";"))
								{
									end--;
								}
							}
							for(int i = skip; i < end; i++)
							{
								char c = name[i];
								if(!Char.IsLetterOrDigit(c) && c != '$' && c != '_' && (c != '/' || prev == '/'))
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
						if(name[0] == '[')
						{
							if(!IsValidFieldSig(name))
							{
								goto barf;
							}
							// the semicolon is only allowed at the end and IsValidFieldSig enforces this,
							// but since invalidJava15Characters contains the semicolon, we decrement end
							// to make the following check against invalidJava15Characters ignore the
							// trailing semicolon.
							if(name[end - 1] == ';')
							{
								end--;
							}
							while(name[start] == '[')
							{
								start++;
							}
						}
						if(name.IndexOfAny(invalidJava15Characters, start, end - start) >= 0)
						{
							goto barf;
						}
						name = String.Intern(name.Replace('/', '.'));
						return;
					}
				}
				barf:
					throw new ClassFormatError("Invalid class name \"{0}\"", name);
			}

			internal override void MarkLinkRequired()
			{
				if(typeWrapper == null)
				{
					typeWrapper = VerifierTypeWrapper.Null;
				}
			}

			internal void LinkSelf(TypeWrapper thisType)
			{
				this.typeWrapper = thisType;
			}

			internal override void Link(TypeWrapper thisType, LoadMode mode)
			{
				if(typeWrapper == VerifierTypeWrapper.Null)
				{
					TypeWrapper tw = thisType.GetClassLoader().LoadClass(name, mode | LoadMode.WarnClassNotFound);
#if !STATIC_COMPILER && !FIRST_PASS
					if(!tw.IsUnloadable)
					{
						try
						{
							thisType.GetClassLoader().CheckPackageAccess(tw, thisType.ClassObject.pd);
						}
						catch(java.lang.SecurityException)
						{
							tw = new UnloadableTypeWrapper(name);
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

			internal TypeWrapper GetClassType()
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
