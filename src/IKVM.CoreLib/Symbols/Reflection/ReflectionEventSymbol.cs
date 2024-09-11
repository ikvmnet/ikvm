using System;
using System.Collections.Immutable;
using System.Reflection;

namespace IKVM.CoreLib.Symbols.Reflection
{

	class ReflectionEventSymbol : IEventSymbol
	{

		readonly ReflectionSymbolContext _context;
		readonly ReflectionTypeSymbol _type;
		readonly EventInfo _event;

		/// <summary>
		/// Initializes a new instance.
		/// </summary>
		/// <param name="context"></param>
		/// <param name="type"></param>
		/// <param name="event"></param>
		public ReflectionEventSymbol(ReflectionSymbolContext context, ReflectionTypeSymbol type, EventInfo @event)
		{
			_context = context ?? throw new ArgumentNullException(nameof(context));
			_type = type ?? throw new ArgumentNullException(nameof(type));
			_event = @event ?? throw new ArgumentNullException(nameof(@event));
		}

		public IMethodSymbol? AddMethod => throw new NotImplementedException();

		public EventAttributes Attributes => throw new NotImplementedException();

		public ITypeSymbol? EventHandlerType => throw new NotImplementedException();

		public bool IsMulticast => throw new NotImplementedException();

		public bool IsSpecialName => throw new NotImplementedException();

		public IMethodSymbol? RaiseMethod => throw new NotImplementedException();

		public IMethodSymbol? RemoveMethod => throw new NotImplementedException();

		public ITypeSymbol? DeclaringType => throw new NotImplementedException();

		public MemberTypes MemberType => throw new NotImplementedException();

		public int MetadataToken => throw new NotImplementedException();

		public IModuleSymbol Module => throw new NotImplementedException();

		public string Name => throw new NotImplementedException();

		public bool IsMissing => throw new NotImplementedException();

		public IMethodSymbol? GetAddMethod()
		{
			throw new NotImplementedException();
		}

		public IMethodSymbol? GetAddMethod(bool nonPublic)
		{
			throw new NotImplementedException();
		}

		public ImmutableArray<ICustomAttributeSymbol> GetCustomAttributes(bool inherit)
		{
			throw new NotImplementedException();
		}

		public ImmutableArray<ICustomAttributeSymbol> GetCustomAttributes(ITypeSymbol attributeType, bool inherit)
		{
			throw new NotImplementedException();
		}

		public IMethodSymbol[] GetOtherMethods()
		{
			throw new NotImplementedException();
		}

		public IMethodSymbol[] GetOtherMethods(bool nonPublic)
		{
			throw new NotImplementedException();
		}

		public IMethodSymbol? GetRaiseMethod()
		{
			throw new NotImplementedException();
		}

		public IMethodSymbol? GetRaiseMethod(bool nonPublic)
		{
			throw new NotImplementedException();
		}

		public IMethodSymbol? GetRemoveMethod(bool nonPublic)
		{
			throw new NotImplementedException();
		}

		public IMethodSymbol? GetRemoveMethod()
		{
			throw new NotImplementedException();
		}

		public bool IsDefined(ITypeSymbol attributeType, bool inherit)
		{
			throw new NotImplementedException();
		}

	}

}