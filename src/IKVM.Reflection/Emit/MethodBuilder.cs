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
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using IKVM.Reflection.Metadata;

namespace IKVM.Reflection.Emit
{

    public sealed class MethodBuilder : MethodInfo
    {

        readonly TypeBuilder type;
        readonly string name;
        readonly int pseudoToken;
        StringHandle nameIndex;
        BlobHandle signature;
        Type returnType;
        Type[] parameterTypes;
        PackedCustomModifiers customModifiers;
        MethodAttributes attributes;
        MethodImplAttributes implFlags;
        ILGenerator ilgen;
        ILGenerator ilgenBaked;
        int rva = -1;
        CallingConventions callingConvention;
        List<ParameterBuilder> parameters;
        GenericTypeParameterBuilder[] gtpb;
        List<CustomAttributeBuilder> declarativeSecurity;
        MethodSignature methodSignature;
        bool initLocals = true;

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

            return ilgen ??= new ILGenerator(type.ModuleBuilder, streamSize);
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
            var pb = new ParameterBuilder(ModuleBuilder, position, attributes, strParamName);
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
            this.returnType = returnType ?? this.Module.universe.System_Void;
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
            this.returnType = returnType ?? this.Module.universe.System_Void;
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

            internal override Module Module
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

        public void __AddUnmanagedExport(string name, int ordinal)
        {
            ModuleBuilder.AddUnmanagedExport(name, ordinal, this, new RelativeVirtualAddress(0xFFFFFFFF));
        }

        public void CreateMethodBody(byte[] il, int count)
        {
            if (il == null)
                throw new NotSupportedException();
            if (il.Length != count)
                Array.Resize(ref il, count);

            SetMethodBody(il, 16, null, null, null);
        }

        public void SetMethodBody(byte[] il, int maxStack, byte[] localSignature, IEnumerable<ExceptionHandler> exceptionHandlers, IEnumerable<int> tokenFixups)
        {
            var bdy = new BlobBuilder();
            var enc = new MethodBodyStreamEncoder(bdy);

            // calculate whether any large exception regions exist
            var exceptionHandlersList = exceptionHandlers?.ToArray() ?? Array.Empty<ExceptionHandler>();
            var hasSmallExceptions = true;
            foreach (var exceptionHandler in exceptionHandlersList)
                if (exceptionHandler.TryOffset > 65535 || exceptionHandler.TryLength > 255 || exceptionHandler.HandlerOffset > 65535 || exceptionHandler.HandlerLength > 255)
                    hasSmallExceptions = false;

            // allocate new method body
            var body = enc.AddMethodBody(
                il.Length,
                maxStack,
                exceptionHandlersList.Length,
                hasSmallExceptions,
                (StandaloneSignatureHandle)MetadataTokens.EntityHandle(localSignature == null ? 0 : ModuleBuilder.GetSignatureToken(localSignature, localSignature.Length).Token),
                MethodBodyAttributes.InitLocals,
                false);

            // copy IL directly into body
            var bodyBytes = body.Instructions.GetBytes();
            il.CopyTo(bodyBytes.Array, bodyBytes.Offset);

            // apply exception handlers
            foreach (var handler in exceptionHandlersList)
                body.ExceptionRegions.Add((ExceptionRegionKind)handler.Kind, handler.TryOffset, handler.TryLength, handler.HandlerOffset, handler.HandlerLength, MetadataTokens.EntityHandle(handler.ExceptionTypeToken), handler.FilterOffset);

            // ensure our output is aligned
            ModuleBuilder.methodBodies.Align(4);
            rva = ModuleBuilder.methodBodies.Position;

            // add fixups for offsets of created method body
            if (tokenFixups != null)
                ILGenerator.AddTokenFixups(rva, ModuleBuilder.tokenFixupOffsets, tokenFixups);

            // dump blob builder contents into module method body
            ModuleBuilder.methodBodies.Write(bdy);
        }

        internal void Bake()
        {
            nameIndex = ModuleBuilder.GetOrAddString(name);
            signature = ModuleBuilder.GetSignatureBlobIndex(MethodSignature);

            // write method body to module
            if (ilgen != null)
            {
                rva = ilgen.WriteBody(initLocals);
                ilgenBaked = ilgen;
                ilgen = null;
            }

            if (declarativeSecurity != null)
                ModuleBuilder.AddDeclarativeSecurity(pseudoToken, declarativeSecurity);
        }

        internal ModuleBuilder ModuleBuilder
        {
            get { return type.ModuleBuilder; }
        }

        internal void WriteMetadata(MetadataBuilder metadata, ref int paramList)
        {
            if (ilgen != null)
                throw new InvalidOperationException();

            // handle we expect to be allocated
            var t = (MethodDefinitionHandle)MetadataTokens.EntityHandle(ModuleBuilder.ResolvePseudoToken(pseudoToken));

            // write metadata, allocating real handle
            var h = metadata.AddMethodDefinition(
                (System.Reflection.MethodAttributes)attributes,
                (System.Reflection.MethodImplAttributes)implFlags,
                nameIndex,
                signature,
                rva,
                MetadataTokens.ParameterHandle(paramList));
            Debug.Assert(h == t);

            // ilgen code was already written, but now we can fill in the debug tables
            var sequencePointsHandle = default(BlobHandle);
            if (ilgenBaked != null)
            {
                ilgenBaked.WriteDebugInformation(h, out sequencePointsHandle);
                ilgenBaked = null;
            }

            // add required method debug information record
            var rec = new MethodDebugInformationTable.Record();
            rec.SequencePoints = sequencePointsHandle;
            var debugHandle = MetadataTokens.MethodDebugInformationHandle(type.ModuleBuilder.MethodDebugInformationTable.AddRecord(rec));
            Debug.Assert(MetadataTokens.GetRowNumber(debugHandle) == MetadataTokens.GetRowNumber(h));

            if (parameters != null)
                paramList += parameters.Count;
        }

        internal void WriteParamRecords(MetadataBuilder metadata)
        {
            if (parameters != null)
                foreach (var pb in parameters)
                    pb.WriteMetadata(metadata);
        }

        internal void FixupToken(int token, ref int parameterToken)
        {
            type.ModuleBuilder.RegisterTokenFixup(pseudoToken, token);
            if (parameters != null)
                foreach (var pb in parameters)
                    pb.FixupToken(parameterToken++);
        }

        internal override MethodSignature MethodSignature
        {
            get
            {
                methodSignature ??= MethodSignature.MakeFromBuilder(returnType ?? type.Universe.System_Void, parameterTypes ?? Type.EmptyTypes, customModifiers, callingConvention, gtpb == null ? 0 : gtpb.Length);
                return methodSignature;
            }
        }

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

        internal override bool IsBaked => type.IsBaked;

    }

}
