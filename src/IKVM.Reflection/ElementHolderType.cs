/*
  Copyright (C) 2009-2015 Jeroen Frijters

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
using IKVM.Reflection.Emit;

namespace IKVM.Reflection
{

    abstract class ElementHolderType : TypeInfo
	{

		protected readonly Type elementType;
		private int token;
		private readonly CustomModifiers mods;

		protected ElementHolderType(Type elementType, CustomModifiers mods, byte sigElementType)
			: base(sigElementType)
		{
			this.elementType = elementType;
			this.mods = mods;
		}

		protected bool EqualsHelper(ElementHolderType other)
		{
			return other != null
				&& other.elementType.Equals(elementType)
				&& other.mods.Equals(mods);
		}

		public override CustomModifiers __GetCustomModifiers()
		{
			return mods;
		}

		public sealed override string Name
		{
			get { return elementType.Name + GetSuffix(); }
		}

		public sealed override string Namespace
		{
			get { return elementType.Namespace; }
		}

		public sealed override string FullName
		{
			get { return elementType.FullName + GetSuffix(); }
		}

		public sealed override string ToString()
		{
			return elementType.ToString() + GetSuffix();
		}

		public sealed override Type GetElementType()
		{
			return elementType;
		}

		public sealed override Module Module
		{
			get { return elementType.Module; }
		}

		internal sealed override int GetModuleBuilderToken()
		{
			if (token == 0)
			{
				token = ((ModuleBuilder)elementType.Module).ImportType(this);
			}
			return token;
		}

		public sealed override bool ContainsGenericParameters
		{
			get
			{
				Type type = elementType;
				while (type.HasElementType)
				{
					type = type.GetElementType();
				}
				return type.ContainsGenericParameters;
			}
		}

		protected sealed override bool ContainsMissingTypeImpl
		{
			get
			{
				Type type = elementType;
				while (type.HasElementType)
				{
					type = type.GetElementType();
				}
				return type.__ContainsMissingType
					|| mods.ContainsMissingType;
			}
		}

		protected sealed override bool IsValueTypeImpl
		{
			get { return false; }
		}

		internal sealed override Type BindTypeParameters(IGenericBinder binder)
		{
			Type type = elementType.BindTypeParameters(binder);
			CustomModifiers mods = this.mods.Bind(binder);
			if (ReferenceEquals(type, elementType)
				&& mods.Equals(this.mods))
			{
				return this;
			}
			return Wrap(type, mods);
		}

		internal override void CheckBaked()
		{
			elementType.CheckBaked();
		}

		internal sealed override Universe Universe
		{
			get { return elementType.Universe; }
		}

		internal sealed override bool IsBaked
		{
			get { return elementType.IsBaked; }
		}

		internal sealed override int GetCurrentToken()
		{
			// we don't have a token, so we return 0 (which is never a valid token)
			return 0;
		}

		internal abstract string GetSuffix();

		protected abstract Type Wrap(Type type, CustomModifiers mods);
	}

}
