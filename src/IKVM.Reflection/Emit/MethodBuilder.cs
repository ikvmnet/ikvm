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
using IKVM.Reflection.Metadata;

namespace IKVM.Reflection.Emit
{

    public sealed class MethodBuilder : MethodInfo
    {

        readonly TypeBuilder type;
        readonly string name;
        readonly int pseudoToken;

        // user configurable values
        Type returnType;
        Type[] parameterTypes;
        PackedCustomModifiers customModifiers;
        MethodAttributes attributes;
        MethodImplAttributes implFlags;

        List<ParameterBuilder> parameters;
        ILGenerator m_ilGenerator;
        GenericTypeParameterBuilder[] gtpb;
        List<CustomAttributeBuilder> declarativeSecurity;
        MethodSignature methodSignature;
        CallingConventions callingConvention;
        bool initLocals = true;

        StringHandle nameIndex;
        BlobHandle signature;
        int rva = -1;

        byte[] m_ubBody;                                // The IL for the method
        int m_maxStack;                                 // Maximum stack size calculated 
        byte[] m_localSignature;                        // Local signature if set explicitly via DefineBody. Null otherwise.
        ExceptionHandler[] m_exceptions;                // Exception handlers or null if there are none.
        int[] m_mdMethodFixups;                         // The location of all of the token fixups. Null means no fixups.
        internal LocalSymInfo m_localSymInfo;           // keep track debugging local information

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="typeBuilder"></param>
        /// <param name="name"></param>
        /// <param name="attributes"></param>
        /// <param name="callingConvention"></param>
        internal MethodBuilder(TypeBuilder typeBuilder, string name, MethodAttributes attributes, CallingConventions callingConvention)
        {
            this.type = typeBuilder;
            this.name = name;
            this.pseudoToken = typeBuilder.ModuleBuilder.AllocPseudoToken();
            this.attributes = attributes;
            if ((attributes & MethodAttributes.Static) == 0)
                callingConvention |= CallingConventions.HasThis;
            this.callingConvention = callingConvention;
        }

        public ILGenerator GetILGenerator()
        {
            return GetILGenerator(16);
        }

        public ILGenerator GetILGenerator(int streamSize)
        {
            if (rva != -1)
                throw new InvalidOperationException();

            return m_ilGenerator ??= new ILGenerator(this, streamSize);
        }

        public void SetCustomAttribute(ConstructorInfo con, byte[] binaryAttribute)
        {
            SetCustomAttribute(new CustomAttributeBuilder(con, binaryAttribute));
        }

        private void SetDllImportPseudoCustomAttribute(CustomAttributeBuilder customBuilder)
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
                implFlags |= MethodImplAttributes.PreserveSig;

            if (setLastError.HasValue && setLastError.Value)
                flags |= SupportsLastError;

            var rec = new ImplMapTable.Record();
            rec.MappingFlags = flags;
            rec.MemberForwarded = pseudoToken;
            rec.ImportName = ModuleBuilder.GetOrAddString(entryName ?? name);
            rec.ImportScope = MetadataTokens.GetToken(MetadataTokens.ModuleReferenceHandle(ModuleBuilder.ModuleRefTable.FindOrAddRecord(dllName == null ? default : ModuleBuilder.GetOrAddString(dllName))));
            ModuleBuilder.ImplMapTable.AddRecord(rec);
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
            implFlags = (MethodImplAttributes)opt;
            var type = customBuilder.GetFieldValue<MethodCodeType>("MethodCodeType");
            if (type.HasValue)
                implFlags |= (MethodImplAttributes)type;
        }

        public void SetCustomAttribute(CustomAttributeBuilder customBuilder)
        {
            switch (customBuilder.KnownCA)
            {
                case KnownCA.DllImportAttribute:
                    SetDllImportPseudoCustomAttribute(customBuilder.DecodeBlob(this.Module.Assembly));
                    attributes |= MethodAttributes.PinvokeImpl;
                    break;
                case KnownCA.MethodImplAttribute:
                    SetMethodImplAttribute(customBuilder.DecodeBlob(this.Module.Assembly));
                    break;
                case KnownCA.PreserveSigAttribute:
                    implFlags |= MethodImplAttributes.PreserveSig;
                    break;
                case KnownCA.SpecialNameAttribute:
                    attributes |= MethodAttributes.SpecialName;
                    break;
                case KnownCA.SuppressUnmanagedCodeSecurityAttribute:
                    attributes |= MethodAttributes.HasSecurity;
                    goto default;
                default:
                    ModuleBuilder.SetCustomAttribute(pseudoToken, customBuilder);
                    break;
            }
        }

        public void __AddDeclarativeSecurity(CustomAttributeBuilder customBuilder)
        {
            attributes |= MethodAttributes.HasSecurity;
            declarativeSecurity ??= new List<CustomAttributeBuilder>();
            declarativeSecurity.Add(customBuilder);
        }

        public void AddDeclarativeSecurity(System.Security.Permissions.SecurityAction securityAction, System.Security.PermissionSet permissionSet)
        {
            this.ModuleBuilder.AddDeclarativeSecurity(pseudoToken, securityAction, permissionSet);
            this.attributes |= MethodAttributes.HasSecurity;
        }

        public void SetImplementationFlags(MethodImplAttributes attributes)
        {
            implFlags = attributes;
        }

        public ParameterBuilder DefineParameter(int position, ParameterAttributes attributes, string strParamName)
        {
            parameters ??= new List<ParameterBuilder>();

            ModuleBuilder.ParamTable.AddVirtualRecord();
            var pb = new ParameterBuilder(this, position, attributes, strParamName);
            if (parameters.Count == 0 || position >= parameters[parameters.Count - 1].Position)
            {
                parameters.Add(pb);
            }
            else
            {
                for (var i = 0; i < parameters.Count; i++)
                {
                    if (parameters[i].Position > position)
                    {
                        parameters.Insert(i, pb);
                        break;
                    }
                }
            }

            return pb;
        }

        void CheckSig()
        {
            if (methodSignature != null)
                throw new InvalidOperationException("The method signature can not be modified after it has been used.");
        }

        public void SetParameters(params Type[] parameterTypes)
        {
            CheckSig();
            this.parameterTypes = Util.Copy(parameterTypes);
        }

        public void SetReturnType(Type returnType)
        {
            CheckSig();
            this.returnType = returnType ?? this.Module.Universe.System_Void;
        }

        public void SetSignature(Type returnType, Type[] returnTypeRequiredCustomModifiers, Type[] returnTypeOptionalCustomModifiers, Type[] parameterTypes, Type[][] parameterTypeRequiredCustomModifiers, Type[][] parameterTypeOptionalCustomModifiers)
        {
            SetSignature(returnType, parameterTypes, PackedCustomModifiers.CreateFromExternal(returnTypeOptionalCustomModifiers, returnTypeRequiredCustomModifiers, parameterTypeOptionalCustomModifiers, parameterTypeRequiredCustomModifiers, Util.NullSafeLength(parameterTypes)));
        }

        public void __SetSignature(Type returnType, CustomModifiers returnTypeCustomModifiers, Type[] parameterTypes, CustomModifiers[] parameterTypeCustomModifiers)
        {
            SetSignature(returnType, parameterTypes, PackedCustomModifiers.CreateFromExternal(returnTypeCustomModifiers, parameterTypeCustomModifiers, Util.NullSafeLength(parameterTypes)));
        }

        private void SetSignature(Type returnType, Type[] parameterTypes, PackedCustomModifiers customModifiers)
        {
            CheckSig();
            this.returnType = returnType ?? this.Module.Universe.System_Void;
            this.parameterTypes = Util.Copy(parameterTypes);
            this.customModifiers = customModifiers;
        }

        public GenericTypeParameterBuilder[] DefineGenericParameters(params string[] names)
        {
            CheckSig();
            if (gtpb != null)
                throw new InvalidOperationException("Generic parameters already defined.");

            gtpb = new GenericTypeParameterBuilder[names.Length];
            for (int i = 0; i < names.Length; i++)
                gtpb[i] = new GenericTypeParameterBuilder(names[i], this, i);

            return (GenericTypeParameterBuilder[])gtpb.Clone();
        }

        public override MethodInfo MakeGenericMethod(params Type[] typeArguments)
        {
            return new GenericMethodInstance(type, this, typeArguments);
        }

        public override MethodInfo GetGenericMethodDefinition()
        {
            if (gtpb == null)
                throw new InvalidOperationException();

            return this;
        }

        public override Type[] GetGenericArguments()
        {
            return Util.Copy(gtpb);
        }

        internal override Type GetGenericMethodArgument(int index)
        {
            return gtpb[index];
        }

        internal override int GetGenericMethodArgumentCount()
        {
            return gtpb == null ? 0 : gtpb.Length;
        }

        public override Type ReturnType
        {
            get { return returnType; }
        }

        public override ParameterInfo ReturnParameter
        {
            get { return new ParameterInfoImpl(this, -1); }
        }

        public override MethodAttributes Attributes
        {
            get { return attributes; }
        }

        public void __SetAttributes(MethodAttributes attributes)
        {
            this.attributes = attributes;
        }

        public void __SetCallingConvention(CallingConventions callingConvention)
        {
            this.callingConvention = callingConvention;
            this.methodSignature = null;
        }

        public override MethodImplAttributes GetMethodImplementationFlags()
        {
            return implFlags;
        }

        sealed class ParameterInfoImpl : ParameterInfo
        {

            readonly MethodBuilder method;
            readonly int parameter;

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="method"></param>
            /// <param name="parameter"></param>
            internal ParameterInfoImpl(MethodBuilder method, int parameter)
            {
                this.method = method;
                this.parameter = parameter;
            }

            ParameterBuilder ParameterBuilder
            {
                get
                {
                    if (method.parameters != null)
                        foreach (var pb in method.parameters)
                            if (pb.Position - 1 == parameter)
                                return pb;

                    return null;
                }
            }

            public override string Name
            {
                get
                {
                    var pb = ParameterBuilder;
                    return pb != null ? pb.Name : null;
                }
            }

            public override Type ParameterType
            {
                get { return parameter == -1 ? method.returnType : method.parameterTypes[parameter]; }
            }

            public override ParameterAttributes Attributes
            {
                get
                {
                    var pb = ParameterBuilder;
                    return pb != null ? (ParameterAttributes)pb.Attributes : ParameterAttributes.None;
                }
            }

            public override int Position
            {
                get { return parameter; }
            }

            public override object RawDefaultValue
            {
                get
                {
                    var pb = ParameterBuilder;
                    if (pb != null && (pb.Attributes & (int)ParameterAttributes.HasDefault) != 0)
                        return method.ModuleBuilder.ConstantTable.GetRawConstantValue(method.ModuleBuilder, pb.PseudoToken);
                    if (pb != null && (pb.Attributes & (int)ParameterAttributes.Optional) != 0)
                        return Missing.Value;

                    return null;
                }
            }

            public override CustomModifiers __GetCustomModifiers()
            {
                return method.customModifiers.GetParameterCustomModifiers(parameter);
            }

            public override bool __TryGetFieldMarshal(out FieldMarshal fieldMarshal)
            {
                fieldMarshal = new FieldMarshal();
                return false;
            }

            public override MemberInfo Member
            {
                get { return method; }
            }

            public override int MetadataToken
            {
                get
                {
                    var pb = ParameterBuilder;
                    return pb != null ? pb.PseudoToken : 0x08000000;
                }
            }

            public override Module Module
            {
                get { return method.Module; }
            }

        }

        public override ParameterInfo[] GetParameters()
        {
            var parameters = new ParameterInfo[parameterTypes.Length];
            for (int i = 0; i < parameters.Length; i++)
                parameters[i] = new ParameterInfoImpl(this, i);

            return parameters;
        }

        internal override Type[] GetParameterTypes()
        {
            return parameterTypes;
        }

        internal override int ParameterCount
        {
            get { return parameterTypes.Length; }
        }

        public override Type DeclaringType
        {
            get { return type.IsModulePseudoType ? null : type; }
        }

        public override string Name
        {
            get { return name; }
        }

        public override CallingConventions CallingConvention
        {
            get { return callingConvention; }
        }

        public override int MetadataToken
        {
            get { return pseudoToken; }
        }

        public override bool IsGenericMethod
        {
            get { return gtpb != null; }
        }

        public override bool IsGenericMethodDefinition
        {
            get { return gtpb != null; }
        }

        public override Module Module
        {
            get { return type.Module; }
        }

        public Module GetModule()
        {
            return type.Module;
        }

        public MethodToken GetToken()
        {
            return new MethodToken(pseudoToken);
        }

        public override MethodBody GetMethodBody()
        {
            throw new NotSupportedException();
        }

        public override int __MethodRVA
        {
            get { throw new NotImplementedException(); }
        }

        public bool InitLocals
        {
            get { return initLocals; }
            set { initLocals = value; }
        }

        public void CreateMethodBody(byte[] il, int count)
        {
            if (il == null)
                throw new NotSupportedException();
            if (il.Length != count)
                Array.Resize(ref il, count);

            SetMethodBody(il, 16, null, null, null);
        }

        /// <summary>
        /// Throws an exception if this method should not have a method body.
        /// </summary>
        /// <exception cref="InvalidOperationException"></exception>
        void ThrowIfShouldNotHaveBody()
        {
            if ((implFlags & MethodImplAttributes.CodeTypeMask) != MethodImplAttributes.IL ||
                (implFlags & MethodImplAttributes.Unmanaged) != 0 ||
                (attributes & MethodAttributes.PinvokeImpl) != 0 ||
                /*m_isDllImport*/ false)
                throw new InvalidOperationException("Method body should not exist.");
        }

        /// <summary>
        /// Creates the body of the method by using a specified byte array of Microsoft intermediate language (MSIL) instructions.
        /// </summary>
        /// <param name="il"></param>
        /// <param name="maxStack"></param>
        /// <param name="localSignature"></param>
        /// <param name="exceptionHandlers"></param>
        /// <param name="tokenFixups"></param>
        public void SetMethodBody(byte[] il, int maxStack, byte[] localSignature, IEnumerable<ExceptionHandler> exceptionHandlers, IEnumerable<int> tokenFixups)
        {
            if (IsBaked)
                throw new InvalidOperationException("Method already has a body.");

            ThrowIfShouldNotHaveBody();
            SetMethodBody(il, maxStack, localSignature, exceptionHandlers?.ToArray(), tokenFixups?.ToArray());
        }

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
            this.m_ubBody = il;
            this.m_maxStack = maxStack;
            this.m_localSignature = localSignature;
            this.m_exceptions = exceptionHandlers;
            this.m_mdMethodFixups = tokenFixups;
        }

        internal void Bake()
        {
            nameIndex = ModuleBuilder.GetOrAddString(name);
            signature = ModuleBuilder.GetSignatureBlobIndex(MethodSignature);

            // extract method body information from IL generator
            if (m_ilGenerator != null)
            {
                if (m_ilGenerator.m_ScopeTree.m_iOpenScopeCount != 0)
                    throw new InvalidOperationException("Local variable scope was not properly closed.");

                // save information from the ILGenerator
                SetMethodBody(m_ilGenerator.BakeByteArray(), m_ilGenerator.GetMaxStackSize(), m_ilGenerator.m_localSignature.GetSignature(), GetExceptions(m_ilGenerator.GetExceptions()), m_ilGenerator.GetTokenFixups());
            }

            if (declarativeSecurity != null)
                ModuleBuilder.AddDeclarativeSecurity(pseudoToken, declarativeSecurity);
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

            int numExceptions = CalculateNumberOfExceptions(excp);
            if (numExceptions > 0)
            {
                var counter = 0;
                var m_exceptions = new ExceptionHandler[numExceptions];

                for (int i = 0; i < excp.Length; i++)
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
                    for (int j = 0; j < numCatch; j++)
                    {
                        int tkExceptionClass = 0;
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
                for (int i = 0; i < excp.Length; i++)
                    num += excp[i].GetNumberOfCatches();

            return num;
        }

        internal ModuleBuilder ModuleBuilder => type.ModuleBuilder;

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
            if (m_localSignature != null)
            {
                var buf = new BlobBuilder();
                buf.WriteBytes(m_localSignature);
                localSignatureHandle = MetadataTokens.StandaloneSignatureHandle(ModuleBuilder.StandAloneSigTable.FindOrAddRecord(ModuleBuilder.GetOrAddBlob(buf)));
            }

            // write the body to the metadata
            WriteBody(localSignatureHandle);

            // handle we expect to be allocated
            var t = (MethodDefinitionHandle)MetadataTokens.EntityHandle(ModuleBuilder.ResolvePseudoToken(pseudoToken));

            // write metadata, allocating real handle
            var h = ModuleBuilder.Metadata.AddMethodDefinition(
                (System.Reflection.MethodAttributes)attributes,
                (System.Reflection.MethodImplAttributes)implFlags,
                nameIndex,
                signature,
                rva,
                MetadataTokens.ParameterHandle(paramList));
            Debug.Assert(h == t);

            // ilgen code was already written, but now we can fill in the debug tables
            WriteSymbols(h, localSignatureHandle);

            if (parameters != null)
                paramList += parameters.Count;

            // release IL information
            m_ilGenerator = null;
            m_ubBody = null;
            m_exceptions = null;
            m_localSymInfo = null;
        }

        /// <summary>
        /// Writes the final method body to the metadata.
        /// </summary>
        /// <param name="localSignatureHandle"></param>
        void WriteBody(StandaloneSignatureHandle localSignatureHandle)
        {
            // might not have a body
            if (m_ubBody == null)
                return;

            // calculate whether any large exception regions exist
            var hasSmallExceptions = HasSmallExceptionRegions(m_exceptions);
            var methodBody = ModuleBuilder.MethodBodyEncoder.AddMethodBody(m_ubBody.Length, m_maxStack, m_exceptions != null ? m_exceptions.Length : 0, hasSmallExceptions, localSignatureHandle, initLocals ? MethodBodyAttributes.InitLocals : MethodBodyAttributes.None, false);
            var ilBytes = methodBody.Instructions.GetBytes();

            // if we've been provided with any token fixups, let's patch up the method body directly before copying to IL stream
            if (m_mdMethodFixups != null && m_mdMethodFixups.Length > 0)
            {
                var span = m_ubBody.AsSpan();
                foreach (int offset in m_mdMethodFixups)
                {
                    var ilp = span.Slice(offset, sizeof(int));
                    BinaryPrimitives.WriteInt32LittleEndian(ilp, ModuleBuilder.ResolvePseudoToken(BinaryPrimitives.ReadInt32LittleEndian(ilp)));
                }
            }

            // copy il stream to instruction space
            m_ubBody.CopyTo(ilBytes.AsSpan());

            // add exception regions
            if (m_exceptions != null)
                foreach (var e in m_exceptions)
                    methodBody.ExceptionRegions.Add((ExceptionRegionKind)(int)e.Kind, e.TryOffset, e.TryLength, e.HandlerOffset, e.HandlerLength, MetadataTokens.EntityHandle(e.ExceptionTypeToken), e.FilterOffset);

            // capture offset as RVA of method
            rva = methodBody.Offset;
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
                if (m_ilGenerator != null)
                {
                    // call OpenScope because OpenMethod no longer implicitly creating
                    // the top-level method scope
                    symWriter.OpenScope(0);

                    // emit debug information for method
                    m_localSymInfo?.EmitLocalSymInfo(symWriter);
                    m_ilGenerator.m_ScopeTree.EmitScopeTree(symWriter);
                    m_ilGenerator.m_LineNumberInfo.EmitLineNumberInfo(symWriter);

                    symWriter.CloseScope(m_ilGenerator.ILOffset);
                }

                // exit the method
                symWriter.CloseMethod();
            }
        }

        internal void WriteParamRecords()
        {
            if (parameters != null)
                foreach (var pb in parameters)
                    pb.WriteMetadata();
        }

        internal void FixupToken(int token, ref int parameterToken)
        {
            type.ModuleBuilder.RegisterTokenFixup(pseudoToken, token);
            if (parameters != null)
                foreach (var pb in parameters)
                    pb.FixupToken(parameterToken++);
        }

        internal override MethodSignature MethodSignature => methodSignature ??= MethodSignature.MakeFromBuilder(returnType ?? type.Universe.System_Void, parameterTypes ?? Type.EmptyTypes, customModifiers, callingConvention, gtpb == null ? 0 : gtpb.Length);

        internal override int ImportTo(ModuleBuilder other)
        {
            return other.ImportMethodOrField(type, name, this.MethodSignature);
        }

        internal void CheckBaked()
        {
            type.CheckBaked();
        }

        internal override int GetCurrentToken()
        {
            if (type.ModuleBuilder.IsSaved)
                return type.ModuleBuilder.ResolvePseudoToken(pseudoToken);
            else
                return pseudoToken;
        }

        internal bool IsTypeCreated()
        {
            return type.IsCreated();
        }

        internal override bool IsBaked => type.IsBaked;

    }

}
