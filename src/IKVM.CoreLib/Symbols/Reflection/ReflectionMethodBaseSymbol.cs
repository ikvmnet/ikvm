using System;
using System.Collections.Immutable;
using System.Reflection;

namespace IKVM.CoreLib.Symbols.Reflection
{

	abstract class ReflectionMethodBaseSymbol : IMethodBaseSymbol
	{

		readonly ReflectionSymbolContext _context;
		readonly ReflectionTypeSymbol _type;
		readonly MethodBase _method;

		/// <summary>
		/// Initializes a new instance.
		/// </summary>
		/// <param name="context"></param>
		/// <param name="type"></param>
		/// <param name="method"></param>
		public ReflectionMethodBaseSymbol(ReflectionSymbolContext context, ReflectionTypeSymbol type, MethodBase method)
		{
			_context = context ?? throw new ArgumentNullException(nameof(context));
			_type = type ?? throw new ArgumentNullException(nameof(type));
			_method = method ?? throw new ArgumentNullException(nameof(method));
		}

		public MethodAttributes Attributes => _method.Attributes;

		public CallingConventions CallingConvention => _method.CallingConvention;

		public bool ContainsGenericParameters => _method.ContainsGenericParameters;

		public bool IsAbstract => _method.IsAbstract;

		public bool IsAssembly => _method.IsAssembly;

		public bool IsConstructor => _method.IsConstructor;

		public bool IsFamily => _method.IsFamily;

		public bool IsFamilyAndAssembly => _method.IsFamilyAndAssembly;

		public bool IsFamilyOrAssembly => _method.IsFamilyOrAssembly;

		public bool IsFinal => _method.IsFinal;

		public bool IsGenericMethod => _method.IsGenericMethod;

		public bool IsGenericMethodDefinition => _method.IsGenericMethodDefinition;

		public bool IsHideBySig => _method.IsHideBySig;

		public bool IsPrivate => _method.IsPrivate;

		public bool IsPublic => _method.IsPublic;

		public bool IsSecurityCritical => _method.IsSecurityCritical;

		public bool IsSecuritySafeCritical => _method.IsSecuritySafeCritical;

		public bool IsSecurityTransparent => _method.IsSecurityTransparent;

		public bool IsSpecialName => _method.IsSpecialName;

		public bool IsStatic => _method.IsStatic;

		public bool IsVirtual => _method.IsVirtual;

		public MethodImplAttributes MethodImplementationFlags => _method.MethodImplementationFlags;

		public ImmutableArray<ICustomAttributeSymbol> CustomAttributes => throw new NotImplementedException();

		public ITypeSymbol? DeclaringType => _method.DeclaringType != null ? _context.GetOrCreateTypeSymbol(_method.DeclaringType) : null;

		public MemberTypes MemberType => _method.MemberType;

		public int MetadataToken => _method.MetadataToken;

		public IModuleSymbol Module => _type.Module;

		public string Name => _method.Name;

		public bool IsMissing => throw new NotImplementedException();

		public ImmutableArray<ICustomAttributeSymbol> GetCustomAttributes(bool inherit)
		{
			throw new NotImplementedException();
		}

		public ImmutableArray<ICustomAttributeSymbol> GetCustomAttributes(ITypeSymbol attributeType, bool inherit)
		{
			throw new NotImplementedException();
		}

		public ITypeSymbol[] GetGenericArguments()
		{
			throw new NotImplementedException();
		}

		public MethodImplAttributes GetMethodImplementationFlags()
		{
			return _method.GetMethodImplementationFlags();
		}

		public IParameterSymbol[] GetParameters()
		{
			throw new NotImplementedException();
		}

		public bool IsDefined(ITypeSymbol attributeType, bool inherit)
		{
			return _method.IsDefined(((ReflectionTypeSymbol)attributeType).ReflectionType, inherit);
		}

	}

}