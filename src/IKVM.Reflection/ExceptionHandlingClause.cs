/*
  Copyright (C) 2009 Jeroen Frijters

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
using IKVM.Reflection.Reader;

namespace IKVM.Reflection
{

    public sealed class ExceptionHandlingClause
    {

        readonly int flags;
        readonly int tryOffset;
        readonly int tryLength;
        readonly int handlerOffset;
        readonly int handlerLength;
        readonly Type catchType;
        readonly int filterOffset;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="module"></param>
        /// <param name="flags"></param>
        /// <param name="tryOffset"></param>
        /// <param name="tryLength"></param>
        /// <param name="handlerOffset"></param>
        /// <param name="handlerLength"></param>
        /// <param name="classTokenOrfilterOffset"></param>
        /// <param name="context"></param>
        internal ExceptionHandlingClause(ModuleReader module, int flags, int tryOffset, int tryLength, int handlerOffset, int handlerLength, int classTokenOrfilterOffset, IGenericContext context)
        {
            this.flags = flags;
            this.tryOffset = tryOffset;
            this.tryLength = tryLength;
            this.handlerOffset = handlerOffset;
            this.handlerLength = handlerLength;
            this.catchType = flags == (int)ExceptionHandlingClauseOptions.Clause && classTokenOrfilterOffset != 0 ? module.ResolveType(classTokenOrfilterOffset, context) : null;
            this.filterOffset = flags == (int)ExceptionHandlingClauseOptions.Filter ? classTokenOrfilterOffset : 0;
        }

        public Type CatchType
        {
            get { return catchType; }
        }

        public int FilterOffset
        {
            get { return filterOffset; }
        }

        public ExceptionHandlingClauseOptions Flags
        {
            get { return (ExceptionHandlingClauseOptions)flags; }
        }

        public int HandlerLength
        {
            get { return handlerLength; }
        }

        public int HandlerOffset
        {
            get { return handlerOffset; }
        }

        public int TryLength
        {
            get { return tryLength; }
        }

        public int TryOffset
        {
            get { return tryOffset; }
        }

    }

}
