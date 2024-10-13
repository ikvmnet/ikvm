using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

using IKVM.CoreLib.Collections;
using IKVM.CoreLib.Reflection;
using IKVM.CoreLib.Symbols.Emit;

namespace IKVM.CoreLib.Symbols.Reflection.Emit
{

    class ReflectionConstructorSymbolBuilder : ReflectionConstructorSymbol, IReflectionConstructorSymbolBuilder
    {

        readonly ConstructorBuilder _builder;
        ConstructorInfo? _ctor;

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
        public ReflectionConstructorSymbolBuilder(ReflectionSymbolContext context, IReflectionModuleSymbolBuilder resolvingModule, IReflectionTypeSymbolBuilder resolvingType, ConstructorBuilder builder) :
            base(context, resolvingModule, resolvingType, builder)
        {
            _builder = builder ?? throw new ArgumentNullException(nameof(builder));
        }

        /// <inheritdoc />
        public ConstructorBuilder UnderlyingConstructorBuilder => _builder;

        /// <inheritdoc />
        public override ConstructorInfo UnderlyingRuntimeConstructor => _ctor ?? throw new InvalidOperationException();

        /// <inheritdoc />
        public IReflectionModuleSymbolBuilder ResolvingModuleBuilder => (IReflectionModuleSymbolBuilder)ResolvingModule;

        #region IConstructorSymbolBuilder

        /// <inheritdoc />
        public void SetImplementationFlags(MethodImplAttributes attributes)
        {
            UnderlyingConstructorBuilder.SetImplementationFlags(attributes);
        }

        /// <inheritdoc />
        public IParameterSymbolBuilder DefineParameter(int iSequence, ParameterAttributes attributes, string? strParamName)
        {
            if (iSequence <= 0)
                throw new ArgumentOutOfRangeException(nameof(iSequence));

            return ResolveParameterSymbol(this, UnderlyingConstructorBuilder.DefineParameter(iSequence, attributes, strParamName));
        }

        /// <inheritdoc />
        public IILGenerator GetILGenerator()
        {
            return _il ??= new ReflectionILGenerator(Context, UnderlyingConstructorBuilder.GetILGenerator());
        }

        /// <inheritdoc />
        public IILGenerator GetILGenerator(int streamSize)
        {
            return _il ??= new ReflectionILGenerator(Context, UnderlyingConstructorBuilder.GetILGenerator(streamSize));
        }

        #endregion

        #region IConstructorSymbol

        /// <inheritdoc />
        public override bool IsComplete => _ctor != null;

        #endregion

        #region ICustomAttributeProviderBuilder

        /// <inheritdoc />
        public void SetCustomAttribute(CustomAttribute attribute)
        {
            UnderlyingConstructorBuilder.SetCustomAttribute(attribute.Unpack());
            _incompleteCustomAttributes ??= [];
            _incompleteCustomAttributes.Add(attribute);
        }

        #endregion

        #region ICustomAttributeProvider

        /// <inheritdoc />
        public override CustomAttribute[] GetCustomAttributes(bool inherit = false)
        {
            if (IsComplete)
                return ResolveCustomAttributes(UnderlyingRuntimeConstructor.GetCustomAttributesData(inherit).ToArray());
            else
                return _incompleteCustomAttributes?.ToArray() ?? [];
        }

        /// <inheritdoc />
        public override CustomAttribute[] GetCustomAttributes(ITypeSymbol attributeType, bool inherit = false)
        {
            if (IsComplete)
            {
                var _attributeType = attributeType.Unpack();
                return ResolveCustomAttributes(UnderlyingRuntimeConstructor.GetCustomAttributesData().Where(i => i.AttributeType == _attributeType).ToArray());
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
                return ResolveCustomAttribute(UnderlyingRuntimeConstructor.GetCustomAttributesData().FirstOrDefault(i => i.AttributeType == _attributeType));
            }
            else
                return GetCustomAttributes(inherit).FirstOrDefault(i => i.AttributeType == attributeType);
        }

        #endregion

        /// <inheritdoc />
        public void OnComplete()
        {
            _ctor = (ConstructorInfo?)ResolvingModule.UnderlyingModule.ResolveMethod(MetadataToken) ?? throw new InvalidOperationException();
            _incompleteCustomAttributes = null;
        }

    }

}
