/*
  Copyright (C) 2008 Jeroen Frijters

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
using IKVM.Reflection.Writer;

namespace IKVM.Reflection.Emit
{

    public sealed class ParameterBuilder
    {

        readonly ModuleBuilder moduleBuilder;
        short flags;
        readonly short sequence;
        readonly int nameIndex;
        readonly string name;
        int lazyPseudoToken;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="moduleBuilder"></param>
        /// <param name="sequence"></param>
        /// <param name="attribs"></param>
        /// <param name="name"></param>
        internal ParameterBuilder(ModuleBuilder moduleBuilder, int sequence, ParameterAttributes attribs, string name)
        {
            this.moduleBuilder = moduleBuilder;
            this.flags = (short)attribs;
            this.sequence = (short)sequence;
            this.nameIndex = name == null ? 0 : moduleBuilder.Strings.Add(name);
            this.name = name;
        }

        internal int PseudoToken
        {
            get
            {
                if (lazyPseudoToken == 0)
                    lazyPseudoToken = moduleBuilder.AllocPseudoToken();

                return lazyPseudoToken;
            }
        }

        public string Name
        {
            get { return name; }
        }

        /// <summary>
        /// One-based index of the parameter.
        /// </summary>
        public int Position
        {
            get { return sequence; }
        }

        public int Attributes
        {
            get { return flags; }
        }

        public bool IsIn
        {
            get { return (flags & (short)ParameterAttributes.In) != 0; }
        }

        public bool IsOut
        {
            get { return (flags & (short)ParameterAttributes.Out) != 0; }
        }

        public bool IsOptional
        {
            get { return (flags & (short)ParameterAttributes.Optional) != 0; }
        }

        public void SetCustomAttribute(ConstructorInfo con, byte[] binaryAttribute)
        {
            SetCustomAttribute(new CustomAttributeBuilder(con, binaryAttribute));
        }

        public void SetCustomAttribute(CustomAttributeBuilder customAttributeBuilder)
        {
            switch (customAttributeBuilder.KnownCA)
            {
                case KnownCA.InAttribute:
                    flags |= (short)ParameterAttributes.In;
                    break;
                case KnownCA.OutAttribute:
                    flags |= (short)ParameterAttributes.Out;
                    break;
                case KnownCA.OptionalAttribute:
                    flags |= (short)ParameterAttributes.Optional;
                    break;
                case KnownCA.MarshalAsAttribute:
                    FieldMarshal.SetMarshalAsAttribute(moduleBuilder, PseudoToken, customAttributeBuilder);
                    flags |= (short)ParameterAttributes.HasFieldMarshal;
                    break;
                default:
                    moduleBuilder.SetCustomAttribute(PseudoToken, customAttributeBuilder);
                    break;
            }
        }

        public void SetConstant(object defaultValue)
        {
            flags |= (short)ParameterAttributes.HasDefault;
            moduleBuilder.AddConstant(PseudoToken, defaultValue);
        }

        internal void WriteParamRecord(MetadataWriter mw)
        {
            mw.Write(flags);
            mw.Write(sequence);
            mw.WriteStringIndex(nameIndex);
        }

        internal void FixupToken(int parameterToken)
        {
            if (lazyPseudoToken != 0)
                moduleBuilder.RegisterTokenFixup(lazyPseudoToken, parameterToken);
        }

    }

}
