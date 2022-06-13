/*
  Copyright (C) 2002-2013 Jeroen Frijters

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
using System.Collections.Generic;

namespace IKVM.StubGen
{
    sealed class RuntimeVisibleParameterAnnotationsAttribute : ClassFileAttribute
	{
		private readonly List<RuntimeVisibleAnnotationsAttribute> parameters = new List<RuntimeVisibleAnnotationsAttribute>();

		internal RuntimeVisibleParameterAnnotationsAttribute(ClassFileWriter classFile)
			: base(classFile.AddUtf8("RuntimeVisibleParameterAnnotations"))
		{
		}

		internal void Add(RuntimeVisibleAnnotationsAttribute parameter)
		{
			parameters.Add(parameter);
		}

		public override void Write(BigEndianStream bes)
		{
			base.Write(bes);
			uint length = 1;
			foreach (RuntimeVisibleAnnotationsAttribute attr in parameters)
			{
				length += attr.Length;
			}
			bes.WriteUInt32(length);
			bes.WriteByte((byte)parameters.Count);
			foreach (RuntimeVisibleAnnotationsAttribute attr in parameters)
			{
				attr.WriteImpl(bes);
			}
		}
	}
}
