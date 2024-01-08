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
namespace IKVM.Reflection.Emit
{

    public sealed class LocalBuilder : LocalVariableInfo
    {

        internal string name;
        internal int startOffset;
        internal int endOffset;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="localType"></param>
        /// <param name="index"></param>
        /// <param name="pinned"></param>
        internal LocalBuilder(Type localType, int index, bool pinned) :
            base(index, localType, pinned)
        {

        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="localType"></param>
        /// <param name="index"></param>
        /// <param name="pinned"></param>
        /// <param name="customModifiers"></param>
        internal LocalBuilder(Type localType, int index, bool pinned, CustomModifiers customModifiers) :
            base(index, localType, pinned, customModifiers)
        {

        }

        public void SetLocalSymInfo(string name)
        {
            this.name = name;
        }

        public void SetLocalSymInfo(string name, int startOffset, int endOffset)
        {
            this.name = name;
            this.startOffset = startOffset;
            this.endOffset = endOffset;
        }

    }

}
