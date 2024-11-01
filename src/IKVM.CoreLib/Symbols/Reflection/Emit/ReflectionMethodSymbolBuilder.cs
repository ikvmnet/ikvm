using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

using IKVM.CoreLib.Reflection;
using IKVM.CoreLib.Symbols.Emit;

namespace IKVM.CoreLib.Symbols.Reflection.Emit
{

    class ReflectionMethodSymbolBuilder : ReflectionMethodSymbol, IReflectionMethodSymbolBuilder
    {

        readonly MethodBuilder _builder;
        MethodInfo? _method;

        ReflectionILGenerator? _il;
        List<CustomAttribute>? _incompleteCustomAttributes;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="resolvingModule"></param>
        /// <param name="resolvingType"></param>
        /// <param name="builder"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public ReflectionMethodSymbolBuilder(ReflectionSymbolContext context, IReflectionModuleSymbolBuilder resolvingModule, IReflectionTypeSymbolBuilder? resolvingType, MethodBuilder builder) :
            base(context, resolvingModule, resolvingType, builder)
        {
            _builder = builder ?? throw new ArgumentNullException(nameof(builder));
        }

        /// <inheritdoc />
        public MethodBuilder UnderlyingMethodBuilder => _builder;

        /// <inheritdoc />
        public override MethodInfo UnderlyingRuntimeMethod => _method ?? throw new InvalidOperationException();

        #region IMethodBaseSymbolBuilder

        /// <inheritdoc />
        public void SetImplementationFlags(MethodImplAttributes attributes)
        {
            UnderlyingMethodBuilder.SetImplementationFlags(attributes);
        }

        /// <inheritdoc />
        public IParameterSymbolBuilder DefineParameter(int position, ParameterAttributes attributes, string? strParamName)
        {
            return ResolveParameterSymbol(this, UnderlyingMethodBuilder.DefineParameter(position, attributes, strParamName));
        }

        /// <inheritdoc />
        public override IParameterSymbol[] GetParameters()
        {
            if (IsComplete)
                return ResolveParameterSymbols(UnderlyingRuntimeMethod.GetParameters());
            else
                return base.GetParameters();
        }

        /// <inheritdoc />
        public IILGenerator GetILGenerator()
        {
            return _il ??= new ReflectionILGenerator(Context, UnderlyingMethodBuilder.GetILGenerator());
        }

        /// <inheritdoc />
        public IILGenerator GetILGenerator(int streamSize)
        {
            return _il ??= new ReflectionILGenerator(Context, UnderlyingMethodBuilder.GetILGenerator(streamSize));
        }

        #endregion

        #region IMethodSymbolBuilder

        /// <inheritdoc />
        public void SetParameters(params ITypeSymbol[] parameterTypes)
        {
            UnderlyingMethodBuilder.SetParameters(parameterTypes.Unpack());
        }

        /// <inheritdoc />
        public void SetReturnType(ITypeSymbol? returnType)
        {
            UnderlyingMethodBuilder.SetReturnType(returnType?.Unpack());
        }

        /// <inheritdoc />
        public void SetSignature(ITypeSymbol? returnType, ITypeSymbol[]? returnTypeRequiredCustomModifiers, ITypeSymbol[]? returnTypeOptionalCustomModifiers, ITypeSymbol[]? parameterTypes, ITypeSymbol[][]? parameterTypeRequiredCustomModifiers, ITypeSymbol[][]? parameterTypeOptionalCustomModifiers)
        {
            UnderlyingMethodBuilder.SetSignature(returnType?.Unpack(), returnTypeRequiredCustomModifiers?.Unpack(), returnTypeOptionalCustomModifiers?.Unpack(), parameterTypes?.Unpack(), parameterTypeRequiredCustomModifiers?.Unpack(), parameterTypeOptionalCustomModifiers?.Unpack());
        }

        /// <inheritdoc />
        public IGenericTypeParameterSymbolBuilder[] DefineGenericParameters(params string[] names)
        {
            var l = UnderlyingMethodBuilder.DefineGenericParameters(names);
            var a = new IGenericTypeParameterSymbolBuilder[l.Length];
            for (int i = 0; i < l.Length; i++)
                a[i] = ResolveGenericTypeParameterSymbol(l[i]);

            return a;
        }

        #endregion

        #region IMethodSymbol

        /// <inheritdoc />
        public override bool IsComplete => _method != null;

        public IReflectionModuleSymbolBuilder ResolvingModuleBuilder => throw new NotImplementedException();

        #endregion

        #region ICustomAttributeProviderBuilder

        /// <inheritdoc />
        public void SetCustomAttribute(CustomAttribute attribute)
        {
            UnderlyingMethodBuilder.SetCustomAttribute(attribute.Unpack());
            _incompleteCustomAttributes ??= [];
            _incompleteCustomAttributes.Add(attribute);
        }

        #endregion

        #region ICustomAttributeProvider

        /// <inheritdoc />
        public override CustomAttribute[] GetCustomAttributes(bool inherit = false)
        {
            if (IsComplete)
                return ResolveCustomAttributes(UnderlyingRuntimeMethod.GetCustomAttributesData(inherit).ToArray());
            else if (inherit == false || GetBaseDefinition() == null)
                return _incompleteCustomAttributes?.ToArray() ?? [];
            else
                return Enumerable.Concat(_incompleteCustomAttributes?.ToArray() ?? [], ResolveCustomAttributes(GetBaseDefinition().Unpack().GetInheritedCustomAttributesData())).ToArray();
        }

        /// <inheritdoc />
        public override CustomAttribute[] GetCustomAttributes(ITypeSymbol attributeType, bool inherit = false)
        {
            if (IsComplete)
            {
                var _attributeType = attributeType.Unpack();
                return ResolveCustomAttributes(UnderlyingRuntimeMethod.GetCustomAttributesData().Where(i => i.AttributeType == _attributeType).ToArray());
            }
            else
                return GetCustomAttributes(inherit).Where(i => i.AttributeType == attributeType).ToArray();
        }

        /// <inheritdoc />
        public override CustomAttribute? GetCustomAttribute(ITypeSymbol attributeType, bool inherit = false)
        {
            if (IsComplete)
            {
                var _attributeType = attributeType.Unpack();
                return ResolveCustomAttribute(UnderlyingRuntimeMethod.GetCustomAttributesData().FirstOrDefault(i => i.AttributeType == _attributeType));
            }
            else
                return GetCustomAttributes(inherit).FirstOrDefault(i => i.AttributeType == attributeType);
        }

        #endregion

        /// <inheritdoc />
        public void OnComplete()
        {
            _method = (MethodInfo?)ResolvingModule.UnderlyingModule.ResolveMethod(MetadataToken) ?? throw new InvalidOperationException();
            _incompleteCustomAttributes = null;

            // apply the runtime generic type parameters
            var genericTypeParameters = ResolveTypeSymbols(UnderlyingRuntimeMethod.GetGenericArguments()) ?? [];
            for (int i = 0; i < genericTypeParameters.Length; i++)
                if (genericTypeParameters[i] is IReflectionGenericTypeParameterSymbolBuilder b)
                    b.OnComplete(genericTypeParameters[i].UnderlyingRuntimeType);
        }

    }

}
