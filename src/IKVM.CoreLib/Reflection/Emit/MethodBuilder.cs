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

        byte[] _body;                                // The IL for the method
        int _maxStack;                                 // Maximum stack size calculated 
        byte[] _localSignature;                        // Local signature if set explicitly via DefineBody. Null otherwise.
        ExceptionHandler[] _exceptions;                // Exception handlers or null if there are none.
        int[] _mdMethodFixups;                         // The location of all of the token fixups. Null means no fixups.
        internal LocalSymInfo LocalSymInfo;           // keep track debugging local information

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="typeBuilder"></param>
        /// <param name="name"></param>
        /// <param name="attributes"></param>
        /// <param name="callingConvention"></param>
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

        public ILGenerator GetILGenerator() => GetILGenerator(16);

        public ILGenerator GetILGenerator(int streamSize)
        {
            if (_rva != -1)
                throw new InvalidOperationException();

            return _ilGenerator ??= new ILGenerator(this, streamSize);
        }

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

        public void SetImplementationFlags(MethodImplAttributes attributes) => _implFlags = attributes;

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

        void CheckSig()
        {
            if (_methodSignature != null)
                throw new InvalidOperationException("The method signature can not be modified after it has been used.");
        }

        public void SetSignature(Type returnType, Type[] returnTypeRequiredCustomModifiers, Type[] returnTypeOptionalCustomModifiers, Type[] parameterTypes, Type[][] parameterTypeRequiredCustomModifiers, Type[][] parameterTypeOptionalCustomModifiers) =>
            SetSignature(returnType, parameterTypes, PackedCustomModifiers.CreateFromExternal(returnTypeOptionalCustomModifiers, returnTypeRequiredCustomModifiers, parameterTypeOptionalCustomModifiers, parameterTypeRequiredCustomModifiers, Util.NullSafeLength(parameterTypes)));

        void SetSignature(Type returnType, Type[] parameterTypes, PackedCustomModifiers customModifiers)
        {
            CheckSig();
            _returnType = returnType ?? Module.Universe.System_Void;
            _parameterTypes = Util.Copy(parameterTypes);
            _customModifiers = customModifiers;
        }

        /// <summary>
        /// Gets the generic parameters defined on the method, or <c>null</c>.
        /// </summary>
        internal GenericTypeParameterBuilder[] GenericParameters => _gtpb;

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

        public override MethodInfo MakeGenericMethod(params Type[] typeArguments) => new GenericMethodInstance(_type, this, typeArguments);

        public override MethodInfo GetGenericMethodDefinition()
        {
            if (_gtpb == null)
                throw new InvalidOperationException();

            return this;
        }

        public override Type[] GetGenericArguments() => Util.Copy(_gtpb);

        internal override Type GetGenericMethodArgument(int index) => _gtpb[index];

        public override Type ReturnType => _returnType;

        public override ParameterInfo ReturnParameter => new ParameterInfoImpl(this, -1);

        public override MethodAttributes Attributes => _attributes;

        public override MethodImplAttributes GetMethodImplementationFlags() => _implFlags;

        sealed class ParameterInfoImpl : ParameterInfo
        {

            readonly MethodBuilder _method;
            readonly int _parameter;

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="method"></param>
            /// <param name="parameter"></param>
            internal ParameterInfoImpl(MethodBuilder method, int parameter)
            {
                _method = method;
                _parameter = parameter;
            }

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

            public override string Name
            {
                get
                {
                    var pb = ParameterBuilder;
                    return pb?.Name;
                }
            }

            public override Type ParameterType => _parameter == -1 ? _method._returnType : _method._parameterTypes[_parameter];

            public override ParameterAttributes Attributes
            {
                get
                {
                    var pb = ParameterBuilder;
                    return pb != null ? (ParameterAttributes)pb.Attributes : ParameterAttributes.None;
                }
            }

            public override int Position => _parameter;

            public override object RawDefaultValue
            {
                get
                {
                    var pb = ParameterBuilder;
                    if (pb != null && (pb.Attributes & (int)ParameterAttributes.HasDefault) != 0)
                        return _method.ModuleBuilder.GetConstant(pb.PseudoToken);
                    if (pb != null && (pb.Attributes & (int)ParameterAttributes.Optional) != 0)
                        return Missing.Value;

                    return null;
                }
            }

            public override CustomModifiers __GetCustomModifiers() => _method._customModifiers.GetParameterCustomModifiers(_parameter);

            public override bool __TryGetFieldMarshal(out FieldMarshal fieldMarshal)
            {
                fieldMarshal = new FieldMarshal();
                return false;
            }

            public override MemberInfo Member => _method;

            public override int MetadataToken
            {
                get
                {
                    var pb = ParameterBuilder;
                    return pb != null ? pb.PseudoToken : 0x08000000;
                }
            }

            public override Module Module => _method.Module;

        }

        public override ParameterInfo[] GetParameters()
        {
            var parameters = new ParameterInfo[_parameterTypes.Length];
            for (var i = 0; i < parameters.Length; i++)
                parameters[i] = new ParameterInfoImpl(this, i);

            return parameters;
        }

        internal override Type[] GetParameterTypes() => _parameterTypes;

        internal override int ParameterCount => _parameterTypes.Length;

        public override Type DeclaringType => _type.IsModulePseudoType ? null : _type;

        public override string Name => _name;

        public override CallingConventions CallingConvention => _callingConvention;

        public override int MetadataToken => _pseudoToken;

        public override bool IsGenericMethod => _gtpb != null;

        public override bool IsGenericMethodDefinition => _gtpb != null;

        public override Module Module => _type.Module;

        /// <summary>
        /// Sets the method body to the specified IL and exception information. Space in the module IL blob is reserved and later filled in after tokens are resolved.
        /// </summary>
        /// <param name="il"></param>
        /// <param name="maxStack"></param>
        /// <param name="localSignature"></param>
        /// <param name="exceptionHandlers"></param>
        /// <param name="tokenFixups"></param>
        void SetMethodBody(byte[] il, int maxStack, byte[] localSignature, ExceptionHandler[] exceptionHandlers, int[] tokenFixups)
        {
            _body = il;
            _maxStack = maxStack;
            _localSignature = localSignature;
            _exceptions = exceptionHandlers;
            _mdMethodFixups = tokenFixups;
        }

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
        /// Transforms the ILGenerator exception information into a list of <see cref="ExceptionHandler"/> structures.
        /// </summary>
        /// <param name="excp"></param>
        /// <returns></returns>
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

                    // for each 
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
        /// Calculates the number of exception regions to be emitted from the exceptions information returned by the <see cref="ILGenerator"/>.
        /// </summary>
        /// <remarks>
        /// Copied from .NET 8.0 RuntimeMethodBuilder.
        /// </remarks>
        /// <param name="excp"></param>
        /// <returns></returns>
        static int CalculateNumberOfExceptions(__ExceptionInfo[] excp)
        {
            var num = 0;

            if (excp != null)
                for (var i = 0; i < excp.Length; i++)
                    num += excp[i].GetNumberOfCatches();

            return num;
        }

        internal ModuleBuilder ModuleBuilder => _type.ModuleBuilder;

        /// <summary>
        /// Writes the method information to the metadata.
        /// </summary>
        /// <param name="paramList"></param>
        /// <exception cref="InvalidOperationException"></exception>
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
        /// Writes the final method body to the metadata.
        /// </summary>
        /// <param name="localSignatureHandle"></param>
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
        /// Returns true if the exceptions can fit into a small region.
        /// </summary>
        /// <param name="exceptions"></param>
        /// <returns></returns>
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
        /// Writes the final debugging information to the metadata tables.
        /// </summary>
        /// <param name="methodHandle"></param>
        /// <param name="localSignatureHandle"></param>
        void WriteSymbols(MethodDefinitionHandle methodHandle, StandaloneSignatureHandle localSignatureHandle)
        {
            if (ModuleBuilder.GetSymWriter() != null)
            {
                // set the debugging information such as scope and line number
                // if it is in a debug module
                //
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
                    // call OpenScope because OpenMethod no longer implicitly creating
                    // the top-level method scope
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

        internal void WriteParamRecords()
        {
            if (_parameters != null)
                foreach (var pb in _parameters)
                    pb.WriteMetadata();
        }

        internal void FixupToken(int token, ref int parameterToken)
        {
            _type.ModuleBuilder.RegisterTokenFixup(_pseudoToken, token);
            if (_parameters != null)
                foreach (var pb in _parameters)
                    pb.FixupToken(parameterToken++);
        }

        internal override MethodSignature MethodSignature =>
            _methodSignature ??= MethodSignature.MakeFromBuilder(_returnType ?? _type.Universe.System_Void, _parameterTypes ?? Type.EmptyTypes, _customModifiers, _callingConvention, _gtpb == null ? 0 : _gtpb.Length);

        internal override int ImportTo(ModuleBuilder other) => other.ImportMethodOrField(_type, _name, MethodSignature);

        internal void CheckBaked() => _type.CheckBaked();

        internal override int GetCurrentToken()
        {
            if (_type.ModuleBuilder.IsSaved)
                return _type.ModuleBuilder.ResolvePseudoToken(_pseudoToken);
            else
                return _pseudoToken;
        }

        internal bool IsTypeCreated() => _type.IsCreated();

        internal override bool IsBaked => _type.IsBaked;

    }

}
