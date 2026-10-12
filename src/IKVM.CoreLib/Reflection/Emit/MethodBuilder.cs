/*
  Copyright (C) 2008-2012 Jeroen Frijters

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
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.SymbolStore;
using System.Linq;
using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using IKVM.Reflection.Diagnostics;

namespace IKVM.Reflection.Emit
{

    /// <summary>
    /// Defines and represents a method (or constructor) on a dynamic class.
    /// </summary>
    sealed class MethodBuilder : MethodInfo
    {

        readonly TypeBuilder _type;
        readonly string _name;
        readonly int _pseudoToken;

        // user configurable values
        Type _returnType;
        Type[] _parameterTypes = Array.Empty<Type>();
        PackedCustomModifiers _customModifiers;
        MethodAttributes _attributes;
        MethodImplAttributes _implFlags;

        List<ParameterBuilder> _parameters;
        ILGenerator _ilGenerator;
        GenericTypeParameterBuilder[] _gtpb;
        MethodSignature _methodSignature;
        readonly CallingConventions _callingConvention;
        readonly bool _initLocals = true;

        StringHandle _nameIndex;
        BlobHandle _signature;
        int _rva = -1;

        // the IL for the method
        byte[] _body;

        // maximum stack size calculated
        int _maxStack;

        // local variable signature taken from the ILGenerator, or null if the method has no locals
        byte[] _localSignature;

        // exception handlers, or null if there are none
        ExceptionHandler[] _exceptions;

        // the offsets in the IL of all of the token fixups, or null if there are none
        int[] _mdMethodFixups;

        /// <summary>
        /// Debugging information about the local variables of the method, written to the symbol writer along with the
        /// method.
        /// </summary>
        internal LocalSymInfo LocalSymInfo;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="typeBuilder">The type that defines the method.</param>
        /// <param name="name">The name of the method.</param>
        /// <param name="attributes">The attributes of the method.</param>
        /// <param name="callingConvention">The calling convention of the method. <see cref="CallingConventions.HasThis"/> is added for an instance method.</param>
        internal MethodBuilder(TypeBuilder typeBuilder, string name, MethodAttributes attributes, CallingConventions callingConvention)
        {
            _type = typeBuilder;
            _name = name;
            _pseudoToken = typeBuilder.ModuleBuilder.AllocPseudoToken();
            _attributes = attributes;
            if ((attributes & MethodAttributes.Static) == 0)
                callingConvention |= CallingConventions.HasThis;
            _callingConvention = callingConvention;
            _returnType = Module.Universe.System_Void;
        }

        /// <summary>
        /// Returns an <see cref="ILGenerator"/> for this method with a default Microsoft intermediate language (MSIL)
        /// stream size of 16 bytes.
        /// </summary>
        /// <returns>An <see cref="ILGenerator"/> object for this method.</returns>
        /// <exception cref="InvalidOperationException">The method body has already been written.</exception>
        public ILGenerator GetILGenerator() => GetILGenerator(16);

        /// <summary>
        /// Returns an <see cref="ILGenerator"/> for this method with the specified Microsoft intermediate language
        /// (MSIL) stream size. The same generator is returned on every call.
        /// </summary>
        /// <param name="streamSize">The size of the MSIL stream, in bytes.</param>
        /// <returns>An <see cref="ILGenerator"/> object for this method.</returns>
        /// <exception cref="InvalidOperationException">The method body has already been written.</exception>
        public ILGenerator GetILGenerator(int streamSize)
        {
            if (_rva != -1)
                throw new InvalidOperationException();

            return _ilGenerator ??= new ILGenerator(this, streamSize);
        }

        /// <summary>
        /// Records a <c>DllImportAttribute</c> as an ImplMap row instead of a custom attribute.
        /// </summary>
        /// <param name="customBuilder">The decoded <c>DllImportAttribute</c>.</param>
        void SetDllImportPseudoCustomAttribute(CustomAttributeBuilder customBuilder)
        {
            var callingConvention = customBuilder.GetFieldValue<CallingConvention>("CallingConvention");
            var charSet = customBuilder.GetFieldValue<CharSet>("CharSet");
            SetDllImportPseudoCustomAttribute((string)customBuilder.GetConstructorArgument(0),
                (string)customBuilder.GetFieldValue("EntryPoint"),
                callingConvention,
                charSet,
                (bool?)customBuilder.GetFieldValue("BestFitMapping"),
                (bool?)customBuilder.GetFieldValue("ThrowOnUnmappableChar"),
                (bool?)customBuilder.GetFieldValue("SetLastError"),
                (bool?)customBuilder.GetFieldValue("PreserveSig"),
                (bool?)customBuilder.GetFieldValue("ExactSpelling"));
        }

        /// <summary>
        /// Records the platform invoke information of the method as an ImplMap row, encoding the options of
        /// <c>DllImportAttribute</c> as P/Invoke flags. Unless <paramref name="preserveSig"/> is
        /// <see langword="false"/>, <see cref="MethodImplAttributes.PreserveSig"/> is also set on the method.
        /// </summary>
        /// <param name="dllName">The name of the DLL that contains the unmanaged method.</param>
        /// <param name="entryName">The name of the unmanaged entry point, or <see langword="null"/> to use the name of the method.</param>
        /// <param name="nativeCallConv">The unmanaged calling convention, or <see langword="null"/> for the platform default.</param>
        /// <param name="nativeCharSet">The character set used to marshal strings, or <see langword="null"/> if not specified.</param>
        /// <param name="bestFitMapping">Whether best-fit mapping is enabled, or <see langword="null"/> if not specified.</param>
        /// <param name="throwOnUnmappableChar">Whether an unmappable character throws, or <see langword="null"/> if not specified.</param>
        /// <param name="setLastError">Whether the callee calls <c>SetLastError</c> before returning, or <see langword="null"/> if not specified.</param>
        /// <param name="preserveSig">Whether the signature is preserved exactly, or <see langword="null"/> for the default of <see langword="true"/>.</param>
        /// <param name="exactSpelling">Whether the entry point name is used without character set suffixes, or <see langword="null"/> if not specified.</param>
        internal void SetDllImportPseudoCustomAttribute(string dllName, string entryName, CallingConvention? nativeCallConv, CharSet? nativeCharSet, bool? bestFitMapping, bool? throwOnUnmappableChar, bool? setLastError, bool? preserveSig, bool? exactSpelling)
        {
            const short NoMangle = 0x0001;
            const short CharSetMask = 0x0006;
            const short CharSetNotSpec = 0x0000;
            const short CharSetAnsi = 0x0002;
            const short CharSetUnicode = 0x0004;
            const short CharSetAuto = 0x0006;
            const short SupportsLastError = 0x0040;
            const short CallConvMask = 0x0700;
            const short CallConvWinapi = 0x0100;
            const short CallConvCdecl = 0x0200;
            const short CallConvStdcall = 0x0300;
            const short CallConvThiscall = 0x0400;
            const short CallConvFastcall = 0x0500;
            // non-standard flags
            const short BestFitOn = 0x0010;
            const short BestFitOff = 0x0020;
            const short CharMapErrorOn = 0x1000;
            const short CharMapErrorOff = 0x2000;
            short flags = CharSetNotSpec | CallConvWinapi;

            if (bestFitMapping.HasValue)
                flags |= bestFitMapping.Value ? BestFitOn : BestFitOff;

            if (throwOnUnmappableChar.HasValue)
                flags |= throwOnUnmappableChar.Value ? CharMapErrorOn : CharMapErrorOff;

            if (nativeCallConv.HasValue)
            {
                flags &= ~CallConvMask;
                switch (nativeCallConv.Value)
                {
                    case System.Runtime.InteropServices.CallingConvention.Cdecl:
                        flags |= CallConvCdecl;
                        break;
                    case System.Runtime.InteropServices.CallingConvention.FastCall:
                        flags |= CallConvFastcall;
                        break;
                    case System.Runtime.InteropServices.CallingConvention.StdCall:
                        flags |= CallConvStdcall;
                        break;
                    case System.Runtime.InteropServices.CallingConvention.ThisCall:
                        flags |= CallConvThiscall;
                        break;
                    case System.Runtime.InteropServices.CallingConvention.Winapi:
                        flags |= CallConvWinapi;
                        break;
                }
            }

            if (nativeCharSet.HasValue)
            {
                flags &= ~CharSetMask;
                switch (nativeCharSet.Value)
                {
                    case CharSet.Ansi:
                    case CharSet.None:
                        flags |= CharSetAnsi;
                        break;
                    case CharSet.Auto:
                        flags |= CharSetAuto;
                        break;
                    case CharSet.Unicode:
                        flags |= CharSetUnicode;
                        break;
                }
            }

            if (exactSpelling.HasValue && exactSpelling.Value)
                flags |= NoMangle;

            if (!preserveSig.HasValue || preserveSig.Value)
                _implFlags |= MethodImplAttributes.PreserveSig;

            if (setLastError.HasValue && setLastError.Value)
                flags |= SupportsLastError;

            ModuleBuilder.AddImplMap(_pseudoToken, (ImplMapFlags)(ushort)flags, entryName ?? _name, dllName);
        }

        /// <summary>
        /// Applies a <c>MethodImplAttribute</c> to the implementation flags of the method instead of emitting it as a
        /// custom attribute. This replaces any implementation flags set earlier.
        /// </summary>
        /// <param name="customBuilder">The decoded <c>MethodImplAttribute</c>.</param>
        /// <exception cref="NotSupportedException">The attribute constructor has more than one parameter.</exception>
        void SetMethodImplAttribute(CustomAttributeBuilder customBuilder)
        {
            MethodImplOptions opt;
            switch (customBuilder.Constructor.ParameterCount)
            {
                case 0:
                    opt = 0;
                    break;
                case 1:
                    {
                        var val = customBuilder.GetConstructorArgument(0);
                        if (val is short s)
                            opt = (MethodImplOptions)s;
                        else if (val is int)
                            opt = (MethodImplOptions)(int)val;
                        else
                            opt = (MethodImplOptions)val;
                        break;
                    }
                default:
                    throw new NotSupportedException();
            }
            _implFlags = (MethodImplAttributes)opt;
            var type = customBuilder.GetFieldValue<MethodCodeType>("MethodCodeType");
            if (type.HasValue)
                _implFlags |= (MethodImplAttributes)type;
        }

        /// <summary>
        /// Sets a custom attribute using a custom attribute builder. Pseudo custom attributes, such as
        /// <c>DllImportAttribute</c> and <c>MethodImplAttribute</c>, are applied to the method metadata instead.
        /// </summary>
        /// <param name="customBuilder">An instance of a helper class to describe the custom attribute.</param>
        public void SetCustomAttribute(CustomAttributeBuilder customBuilder)
        {
            switch (customBuilder.KnownCA)
            {
                case KnownCA.DllImportAttribute:
                    SetDllImportPseudoCustomAttribute(customBuilder.DecodeBlob(Module.Assembly));
                    _attributes |= MethodAttributes.PinvokeImpl;
                    break;
                case KnownCA.MethodImplAttribute:
                    SetMethodImplAttribute(customBuilder.DecodeBlob(Module.Assembly));
                    break;
                case KnownCA.PreserveSigAttribute:
                    _implFlags |= MethodImplAttributes.PreserveSig;
                    break;
                case KnownCA.SpecialNameAttribute:
                    _attributes |= MethodAttributes.SpecialName;
                    break;
                case KnownCA.SuppressUnmanagedCodeSecurityAttribute:
                    _attributes |= MethodAttributes.HasSecurity;
                    goto default;
                default:
                    ModuleBuilder.SetCustomAttribute(_pseudoToken, customBuilder);
                    break;
            }
        }

        /// <summary>
        /// Sets the implementation flags for this method.
        /// </summary>
        /// <param name="attributes">The implementation flags to set.</param>
        public void SetImplementationFlags(MethodImplAttributes attributes) => _implFlags = attributes;

        /// <summary>
        /// Sets the parameter attributes and the name of a parameter of this method, or of the return value of this
        /// method. Returns a <see cref="ParameterBuilder"/> that can be used to apply custom attributes.
        /// </summary>
        /// <param name="position">The position of the parameter in the parameter list. Parameters are indexed beginning with the number 1 for the first parameter; the number 0 represents the return value of the method.</param>
        /// <param name="attributes">The parameter attributes of the parameter.</param>
        /// <param name="strParamName">The name of the parameter. The name can be the null string.</param>
        /// <returns>An object that represents a parameter of this method or the return value of this method.</returns>
        public ParameterBuilder DefineParameter(int position, ParameterAttributes attributes, string strParamName)
        {
            _parameters ??= [];

            var pb = new ParameterBuilder(this, position, attributes, strParamName);
            if (_parameters.Count == 0 || position >= _parameters[^1].Position)
            {
                _parameters.Add(pb);
            }
            else
            {
                for (var i = 0; i < _parameters.Count; i++)
                {
                    if (_parameters[i].Position > position)
                    {
                        _parameters.Insert(i, pb);
                        break;
                    }
                }
            }

            return pb;
        }

        /// <summary>
        /// Throws if the signature of the method has already been computed, after which it can no longer change.
        /// </summary>
        /// <exception cref="InvalidOperationException">The signature has already been used.</exception>
        void CheckSig()
        {
            if (_methodSignature != null)
                throw new InvalidOperationException("The method signature can not be modified after it has been used.");
        }

        /// <summary>
        /// Sets the method signature, including the return type, the parameter types, and the required and optional
        /// custom modifiers of the return type and parameter types.
        /// </summary>
        /// <param name="returnType">The return type of the method.</param>
        /// <param name="returnTypeRequiredCustomModifiers">An array of types representing the required custom modifiers, such as <c>IsConst</c>, for the return type of the method. If the return type has no required custom modifiers, specify <see langword="null"/>.</param>
        /// <param name="returnTypeOptionalCustomModifiers">An array of types representing the optional custom modifiers, such as <c>IsConst</c>, for the return type of the method. If the return type has no optional custom modifiers, specify <see langword="null"/>.</param>
        /// <param name="parameterTypes">The types of the parameters of the method.</param>
        /// <param name="parameterTypeRequiredCustomModifiers">An array of arrays of types. Each array of types represents the required custom modifiers for the corresponding parameter. If no parameter has required custom modifiers, specify <see langword="null"/>.</param>
        /// <param name="parameterTypeOptionalCustomModifiers">An array of arrays of types. Each array of types represents the optional custom modifiers for the corresponding parameter. If no parameter has optional custom modifiers, specify <see langword="null"/>.</param>
        /// <exception cref="InvalidOperationException">The signature has already been used.</exception>
        public void SetSignature(Type returnType, Type[] returnTypeRequiredCustomModifiers, Type[] returnTypeOptionalCustomModifiers, Type[] parameterTypes, Type[][] parameterTypeRequiredCustomModifiers, Type[][] parameterTypeOptionalCustomModifiers) =>
            SetSignature(returnType, parameterTypes, PackedCustomModifiers.CreateFromExternal(returnTypeOptionalCustomModifiers, returnTypeRequiredCustomModifiers, parameterTypeOptionalCustomModifiers, parameterTypeRequiredCustomModifiers, Util.NullSafeLength(parameterTypes)));

        /// <summary>
        /// Sets the return type, parameter types and custom modifiers of the method.
        /// </summary>
        /// <param name="returnType">The return type, or <see langword="null"/> for <see cref="void"/>.</param>
        /// <param name="parameterTypes">The parameter types. The array is copied.</param>
        /// <param name="customModifiers">The custom modifiers of the return type and parameters.</param>
        /// <exception cref="InvalidOperationException">The signature has already been used.</exception>
        void SetSignature(Type returnType, Type[] parameterTypes, PackedCustomModifiers customModifiers)
        {
            CheckSig();
            _returnType = returnType ?? Module.Universe.System_Void;
            _parameterTypes = Util.Copy(parameterTypes);
            _customModifiers = customModifiers;
        }

        /// <summary>
        /// Gets the generic parameters defined on the method, or <see langword="null"/> if none have been defined.
        /// </summary>
        internal GenericTypeParameterBuilder[] GenericParameters => _gtpb;

        /// <summary>
        /// Sets the number of generic type parameters for the current method, specifies their names, and returns an
        /// array of <see cref="GenericTypeParameterBuilder"/> objects that can be used to define their constraints.
        /// </summary>
        /// <param name="names">An array of strings that represent the names of the generic type parameters.</param>
        /// <returns>An array of <see cref="GenericTypeParameterBuilder"/> objects representing the type parameters of the generic method.</returns>
        /// <exception cref="InvalidOperationException">Generic type parameters have already been defined for this method, or the signature has already been used.</exception>
        public GenericTypeParameterBuilder[] DefineGenericParameters(params string[] names)
        {
            CheckSig();
            if (_gtpb != null)
                throw new InvalidOperationException("Generic parameters already defined.");

            _gtpb = new GenericTypeParameterBuilder[names.Length];
            for (var i = 0; i < names.Length; i++)
                _gtpb[i] = new GenericTypeParameterBuilder(names[i], this, i);

            return (GenericTypeParameterBuilder[])_gtpb.Clone();
        }

        /// <inheritdoc />
        public override MethodInfo MakeGenericMethod(params Type[] typeArguments) => new GenericMethodInstance(_type, this, typeArguments);

        /// <summary>
        /// Returns this method, which is its own generic method definition.
        /// </summary>
        /// <returns>The current instance.</returns>
        /// <exception cref="InvalidOperationException">The current method is not generic.</exception>
        public override MethodInfo GetGenericMethodDefinition()
        {
            if (_gtpb == null)
                throw new InvalidOperationException();

            return this;
        }

        /// <inheritdoc />
        public override Type[] GetGenericArguments() => Util.Copy(_gtpb);

        /// <inheritdoc />
        internal override Type GetGenericMethodArgument(int index) => _gtpb[index];

        /// <inheritdoc />
        public override Type ReturnType => _returnType;

        /// <inheritdoc />
        public override ParameterInfo ReturnParameter => new ParameterInfoImpl(this, -1);

        /// <inheritdoc />
        public override MethodAttributes Attributes => _attributes;

        /// <inheritdoc />
        public override MethodImplAttributes GetMethodImplementationFlags() => _implFlags;

        /// <summary>
        /// Represents a parameter, or the return parameter, of a <see cref="MethodBuilder"/>. The type comes from the
        /// method signature; the name, attributes and default value come from the <see cref="Emit.ParameterBuilder"/>
        /// defined for the position, if there is one.
        /// </summary>
        sealed class ParameterInfoImpl : ParameterInfo
        {

            readonly MethodBuilder _method;
            readonly int _parameter;

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="method">The method that declares the parameter.</param>
            /// <param name="parameter">The zero-based position of the parameter, or -1 for the return parameter.</param>
            internal ParameterInfoImpl(MethodBuilder method, int parameter)
            {
                _method = method;
                _parameter = parameter;
            }

            /// <summary>
            /// Gets the parameter builder defined for this position, or <see langword="null"/> if none was defined.
            /// </summary>
            ParameterBuilder ParameterBuilder
            {
                get
                {
                    if (_method._parameters != null)
                        foreach (var pb in _method._parameters)
                            if (pb.Position - 1 == _parameter)
                                return pb;

                    return null;
                }
            }

            /// <inheritdoc />
            public override string Name
            {
                get
                {
                    var pb = ParameterBuilder;
                    return pb?.Name;
                }
            }

            /// <inheritdoc />
            public override Type ParameterType => _parameter == -1 ? _method._returnType : _method._parameterTypes[_parameter];

            /// <inheritdoc />
            public override ParameterAttributes Attributes
            {
                get
                {
                    var pb = ParameterBuilder;
                    return pb != null ? (ParameterAttributes)pb.Attributes : ParameterAttributes.None;
                }
            }

            /// <inheritdoc />
            public override int Position => _parameter;

            /// <inheritdoc />
            public override object RawDefaultValue
            {
                get
                {
                    var pb = ParameterBuilder;
                    if (pb != null && (pb.Attributes & (int)ParameterAttributes.HasDefault) != 0)
                        return _method.ModuleBuilder.GetConstant(pb.PseudoToken);

                    // decimal and DateTime defaults are recorded as custom attributes, as the reader reports them
                    if (pb != null && TryGetCustomConstant(out var value))
                        return value;

                    if (pb != null && (pb.Attributes & (int)ParameterAttributes.Optional) != 0)
                        return Missing.Value;

                    return null;
                }
            }

            /// <inheritdoc />
            public override CustomModifiers __GetCustomModifiers() => _method._customModifiers.GetParameterCustomModifiers(_parameter);

            /// <summary>
            /// Attempts to read the marshalling information of the parameter. Marshalling information is not available
            /// for the parameters of a method being built, so this always returns <see langword="false"/>.
            /// </summary>
            /// <param name="fieldMarshal">When this method returns, contains the default value.</param>
            /// <returns>Always <see langword="false"/>.</returns>
            public override bool __TryGetFieldMarshal(out FieldMarshal fieldMarshal)
            {
                fieldMarshal = new FieldMarshal();
                return false;
            }

            /// <inheritdoc />
            public override MemberInfo Member => _method;

            /// <summary>
            /// Gets the pseudo token of the parameter builder, or 0x08000000 for a parameter without one.
            /// </summary>
            public override int MetadataToken
            {
                get
                {
                    var pb = ParameterBuilder;
                    return pb != null ? pb.PseudoToken : 0x08000000;
                }
            }

            /// <inheritdoc />
            public override Module Module => _method.Module;

        }

        /// <inheritdoc />
        public override ParameterInfo[] GetParameters()
        {
            var parameters = new ParameterInfo[_parameterTypes.Length];
            for (var i = 0; i < parameters.Length; i++)
                parameters[i] = new ParameterInfoImpl(this, i);

            return parameters;
        }

        /// <inheritdoc />
        internal override Type[] GetParameterTypes() => _parameterTypes;

        /// <inheritdoc />
        internal override int ParameterCount => _parameterTypes.Length;

        /// <summary>
        /// Gets the type that declares this method, or <see langword="null"/> for a global method defined on the
        /// &lt;Module&gt; pseudo type.
        /// </summary>
        public override Type DeclaringType => _type.IsModulePseudoType ? null : _type;

        /// <inheritdoc />
        public override string Name => _name;

        /// <inheritdoc />
        public override CallingConventions CallingConvention => _callingConvention;

        /// <summary>
        /// Gets the pseudo token of the method, which is resolved to its real token when the module is saved.
        /// </summary>
        public override int MetadataToken => _pseudoToken;

        /// <inheritdoc />
        public override bool IsGenericMethod => _gtpb != null;

        /// <inheritdoc />
        public override bool IsGenericMethodDefinition => _gtpb != null;

        /// <inheritdoc />
        public override Module Module => _type.Module;

        /// <summary>
        /// Sets the method body to the specified IL and exception information. The body is copied to the module IL
        /// stream when the metadata is written, after the tokens have been resolved.
        /// </summary>
        /// <param name="il">The IL of the method body.</param>
        /// <param name="maxStack">The maximum stack depth of the method body.</param>
        /// <param name="localSignature">The local variable signature, or <see langword="null"/> if the method has no locals.</param>
        /// <param name="exceptionHandlers">The exception handlers, or <see langword="null"/> if there are none.</param>
        /// <param name="tokenFixups">The offsets in <paramref name="il"/> of the tokens to resolve, or <see langword="null"/> if there are none.</param>
        void SetMethodBody(byte[] il, int maxStack, byte[] localSignature, ExceptionHandler[] exceptionHandlers, int[] tokenFixups)
        {
            _body = il;
            _maxStack = maxStack;
            _localSignature = localSignature;
            _exceptions = exceptionHandlers;
            _mdMethodFixups = tokenFixups;
        }

        /// <summary>
        /// Completes the method: adds its name and signature to the metadata, and takes the IL, local signature,
        /// exception handlers and token fixups from its <see cref="ILGenerator"/>, if it has one.
        /// </summary>
        /// <exception cref="InvalidOperationException">A local variable scope is still open.</exception>
        internal void Bake()
        {
            _nameIndex = ModuleBuilder.GetOrAddString(_name);
            _signature = ModuleBuilder.GetSignatureBlobIndex(MethodSignature);

            // extract method body information from IL generator
            if (_ilGenerator != null)
            {
                if (_ilGenerator.ScopeTree.OpenScopeCount != 0)
                    throw new InvalidOperationException("Local variable scope was not properly closed.");

                // save information from the ILGenerator; a local signature must have at least one local, and without one
                // the body can use a tiny header
                var localSignature = _ilGenerator.LocalCount > 0 ? _ilGenerator.LocalSignature.GetSignature() : null;
                SetMethodBody(_ilGenerator.BakeByteArray(), _ilGenerator.GetMaxStackSize(), localSignature, GetExceptions(_ilGenerator.GetExceptions()), _ilGenerator.GetTokenFixups());
            }
        }

        /// <summary>
        /// Transforms the ILGenerator exception information into a list of <see cref="ExceptionHandler"/> structures,
        /// one for each catch, filter, fault or finally clause.
        /// </summary>
        /// <param name="excp">The exception blocks recorded by the <see cref="ILGenerator"/>, or <see langword="null"/> if there are none.</param>
        /// <returns>The exception handlers, or <see langword="null"/> if <paramref name="excp"/> is <see langword="null"/>.</returns>
        ExceptionHandler[] GetExceptions(__ExceptionInfo[] excp)
        {
            // no exceptions required
            if (excp == null)
                return null;

            var numExceptions = CalculateNumberOfExceptions(excp);
            if (numExceptions > 0)
            {
                var counter = 0;
                var m_exceptions = new ExceptionHandler[numExceptions];

                for (var i = 0; i < excp.Length; i++)
                {
                    var numCatch = excp[i].GetNumberOfCatches();
                    var start = excp[i].GetStartAddress();
                    var end = excp[i].GetEndAddress();
                    var type = excp[i].GetExceptionTypes();

                    var filterAddrs = excp[i].GetFilterAddresses();
                    var catchAddrs = excp[i].GetCatchAddresses();
                    var catchEndAddrs = excp[i].GetCatchEndAddresses();
                    var catchClass = excp[i].GetCatchClass();

                    // for each clause of the exception block
                    for (var j = 0; j < numCatch; j++)
                    {
                        var tkExceptionClass = 0;
                        if (catchClass[j] != null)
                        {
                            tkExceptionClass = ModuleBuilder.GetTypeTokenForMemberRef(catchClass[j]);
                            Debug.Assert(ModuleBuilder.IsPseudoToken(tkExceptionClass) == false);
                        }

                        switch (type[j])
                        {
                            case __ExceptionInfo.None:
                            case __ExceptionInfo.Fault:
                            case __ExceptionInfo.Filter:
                                m_exceptions[counter++] = new ExceptionHandler(start, end, filterAddrs[j], catchAddrs[j], catchEndAddrs[j], type[j], tkExceptionClass);
                                break;

                            case __ExceptionInfo.Finally:
                                m_exceptions[counter++] = new ExceptionHandler(start, excp[i].GetFinallyEndAddress(), filterAddrs[j], catchAddrs[j], catchEndAddrs[j], type[j], tkExceptionClass);
                                break;
                        }
                    }
                }

                return m_exceptions;
            }

            return Array.Empty<ExceptionHandler>();
        }

        /// <summary>
        /// Calculates the number of exception regions to be emitted from the exceptions information returned by the
        /// <see cref="ILGenerator"/>.
        /// </summary>
        /// <remarks>
        /// Copied from .NET 8.0 RuntimeMethodBuilder.
        /// </remarks>
        /// <param name="excp">The exception blocks recorded by the <see cref="ILGenerator"/>, or <see langword="null"/>.</param>
        /// <returns>The total number of clauses in all exception blocks.</returns>
        static int CalculateNumberOfExceptions(__ExceptionInfo[] excp)
        {
            var num = 0;

            if (excp != null)
                for (var i = 0; i < excp.Length; i++)
                    num += excp[i].GetNumberOfCatches();

            return num;
        }

        /// <summary>
        /// Gets the module builder that defines the method.
        /// </summary>
        internal ModuleBuilder ModuleBuilder => _type.ModuleBuilder;

        /// <summary>
        /// Writes the method body, the MethodDef row and the debugging information of the method to the metadata, and
        /// then releases the IL information.
        /// </summary>
        /// <param name="paramList">The row number of the first Param row of this method. On return, advanced past the parameters of this method.</param>
        internal void WriteMetadata(ref int paramList)
        {
            Debug.Assert(IsBaked);

            // encode local signature into metadata
            var localSignatureHandle = default(StandaloneSignatureHandle);
            if (_localSignature != null)
            {
                var buf = new BlobBuilder();
                buf.WriteBytes(_localSignature);
                localSignatureHandle = ModuleBuilder.GetStandAloneSignature(ModuleBuilder.GetOrAddBlob(buf));
            }

            // write the body to the metadata
            WriteBody(localSignatureHandle);

            // handle we expect to be allocated
            var t = (MethodDefinitionHandle)MetadataTokens.EntityHandle(ModuleBuilder.ResolvePseudoToken(_pseudoToken));

            // write metadata, allocating real handle
            var h = ModuleBuilder.Metadata.AddMethodDefinition(
                (System.Reflection.MethodAttributes)_attributes,
                (System.Reflection.MethodImplAttributes)_implFlags,
                _nameIndex,
                _signature,
                _rva,
                MetadataTokens.ParameterHandle(paramList));
            Debug.Assert(h == t);

            // ilgen code was already written, but now we can fill in the debug tables
            WriteSymbols(h, localSignatureHandle);

            if (_parameters != null)
                paramList += _parameters.Count;

            // release IL information
            _ilGenerator = null;
            _body = null;
            _exceptions = null;
            LocalSymInfo = null;
        }

        /// <summary>
        /// Writes the final method body, with its tokens resolved, to the module IL stream and records its RVA. Does
        /// nothing if the method has no body.
        /// </summary>
        /// <param name="localSignatureHandle">The handle of the local variable signature, or a nil handle if the method has no locals.</param>
        void WriteBody(StandaloneSignatureHandle localSignatureHandle)
        {
            // might not have a body
            if (_body == null)
                return;

            // calculate whether any large exception regions exist
            var hasSmallExceptions = HasSmallExceptionRegions(_exceptions);
            var methodBody = ModuleBuilder.MethodBodyEncoder.AddMethodBody(_body.Length, _maxStack, _exceptions != null ? _exceptions.Length : 0, hasSmallExceptions, localSignatureHandle, _initLocals ? MethodBodyAttributes.InitLocals : MethodBodyAttributes.None, false);
            var ilBytes = methodBody.Instructions.GetBytes();

            // if we've been provided with any token fixups, let's patch up the method body directly before copying to IL stream
            if (_mdMethodFixups != null && _mdMethodFixups.Length > 0)
            {
                var span = _body.AsSpan();
                foreach (var offset in _mdMethodFixups)
                {
                    var ilp = span.Slice(offset, sizeof(int));
                    BinaryPrimitives.WriteInt32LittleEndian(ilp, ModuleBuilder.ResolvePseudoToken(BinaryPrimitives.ReadInt32LittleEndian(ilp)));
                }
            }

            // copy il stream to instruction space
            _body.CopyTo(ilBytes.AsSpan());

            // add exception regions
            if (_exceptions != null)
                foreach (var e in _exceptions)
                    methodBody.ExceptionRegions.Add((ExceptionRegionKind)(int)e.Kind, e.TryOffset, e.TryLength, e.HandlerOffset, e.HandlerLength, MetadataTokens.EntityHandle(e.ExceptionTypeToken), e.FilterOffset);

            // capture offset as RVA of method
            _rva = methodBody.Offset;
        }

        /// <summary>
        /// Determines whether the exception handlers can be encoded in the small exception region format.
        /// </summary>
        /// <param name="exceptions">The exception handlers, or <see langword="null"/> if there are none.</param>
        /// <returns><see langword="true"/> if the handlers fit the small format; otherwise, <see langword="false"/>.</returns>
        bool HasSmallExceptionRegions(ExceptionHandler[] exceptions)
        {
            if (exceptions == null)
                return true;

            if (ExceptionRegionEncoder.IsSmallRegionCount(exceptions.Length) == false)
                return false;

            foreach (var e in exceptions)
                if (ExceptionRegionEncoder.IsSmallExceptionRegion(e.TryOffset, e.TryLength) == false || ExceptionRegionEncoder.IsSmallExceptionRegion(e.HandlerOffset, e.HandlerLength) == false)
                    return false;

            return true;
        }

        /// <summary>
        /// Writes the final debugging information of the method, its scopes, locals and line numbers, to the module's
        /// symbol writer, if it has one.
        /// </summary>
        /// <param name="methodHandle">The handle of the method's MethodDef row.</param>
        /// <param name="localSignatureHandle">The handle of the local variable signature, or a nil handle if the method has no locals.</param>
        void WriteSymbols(MethodDefinitionHandle methodHandle, StandaloneSignatureHandle localSignatureHandle)
        {
            if (ModuleBuilder.GetSymWriter() != null)
            {
                // set the debugging information such as scope and line number if it is in a debug module
                SymbolToken tk = new SymbolToken(MetadataTokens.GetToken(methodHandle));
                ISymbolWriter symWriter = ModuleBuilder.GetSymWriter();

                // call OpenMethod to make this method the current method
                if (symWriter is IMetadataSymbolWriter metadataSymWriter)
                    metadataSymWriter.OpenMethod(tk, localSignatureHandle);
                else
                    symWriter.OpenMethod(tk);

                // we do write method (since the method exists), but not the contents
                if (_ilGenerator != null)
                {
                    // call OpenScope because OpenMethod no longer implicitly creates the top-level method scope
                    symWriter.OpenScope(0);

                    // emit debug information for method
                    LocalSymInfo?.EmitLocalSymInfo(symWriter);
                    _ilGenerator.ScopeTree.EmitScopeTree(symWriter);
                    _ilGenerator.LineNumberInfo.EmitLineNumberInfo(symWriter);

                    symWriter.CloseScope(_ilGenerator.ILOffset);
                }

                // exit the method
                symWriter.CloseMethod();
            }
        }

        /// <summary>
        /// Writes the Param rows of the parameters defined on the method.
        /// </summary>
        internal void WriteParamRecords()
        {
            if (_parameters != null)
                foreach (var pb in _parameters)
                    pb.WriteMetadata();
        }

        /// <summary>
        /// Registers the real tokens of the method and of its parameters, replacing their pseudo tokens.
        /// </summary>
        /// <param name="token">The real MethodDef token of the method.</param>
        /// <param name="parameterToken">The real Param token of the first parameter. On return, advanced past the parameters of this method.</param>
        internal void FixupToken(int token, ref int parameterToken)
        {
            _type.ModuleBuilder.RegisterTokenFixup(_pseudoToken, token);
            if (_parameters != null)
                foreach (var pb in _parameters)
                    pb.FixupToken(parameterToken++);
        }

        /// <summary>
        /// Gets the signature of the method, computing it on first use. After that the signature can no longer be
        /// changed.
        /// </summary>
        internal override MethodSignature MethodSignature =>
            _methodSignature ??= MethodSignature.MakeFromBuilder(_returnType ?? _type.Universe.System_Void, _parameterTypes ?? Type.EmptyTypes, _customModifiers, _callingConvention, _gtpb == null ? 0 : _gtpb.Length);

        /// <inheritdoc />
        internal override int ImportTo(ModuleBuilder other) => other.ImportMethodOrField(_type, _name, MethodSignature);

        /// <summary>
        /// Throws if the type that declares the method has not been created.
        /// </summary>
        internal void CheckBaked() => _type.CheckBaked();

        /// <summary>
        /// Gets the real token of the method once the module has been saved, and its pseudo token before that.
        /// </summary>
        /// <returns>The current token of the method.</returns>
        internal override int GetCurrentToken()
        {
            if (_type.ModuleBuilder.IsSaved)
                return _type.ModuleBuilder.ResolvePseudoToken(_pseudoToken);
            else
                return _pseudoToken;
        }

        /// <summary>
        /// Determines whether the type that declares the method has been created.
        /// </summary>
        /// <returns><see langword="true"/> if the declaring type has been created; otherwise, <see langword="false"/>.</returns>
        internal bool IsTypeCreated() => _type.IsCreated();

        /// <inheritdoc />
        internal override bool IsBaked => _type.IsBaked;

    }

}
