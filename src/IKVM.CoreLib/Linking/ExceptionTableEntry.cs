/*
  Copyright (C) 2002-2015 Jeroen Frijters

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

using IKVM.ByteCode;

namespace IKVM.CoreLib.Linking
{

    internal readonly struct ExceptionTableEntry
    {

        readonly int _startIndex;
        readonly int _endIndex;
        readonly int _handlerIndex;
        readonly ClassConstantHandle _catchType;
        readonly int _ordinal;
        readonly bool _isFinally;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="startIndex"></param>
        /// <param name="endIndex"></param>
        /// <param name="handlerIndex"></param>
        /// <param name="catchType"></param>
        /// <param name="ordinal"></param>
        internal ExceptionTableEntry(int startIndex, int endIndex, int handlerIndex, ClassConstantHandle catchType, int ordinal) :
            this(startIndex, endIndex, handlerIndex, catchType, ordinal, false)
        {

        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="startIndex"></param>
        /// <param name="endIndex"></param>
        /// <param name="handlerIndex"></param>
        /// <param name="catchType"></param>
        /// <param name="ordinal"></param>
        /// <param name="isFinally"></param>
        internal ExceptionTableEntry(int startIndex, int endIndex, int handlerIndex, ClassConstantHandle catchType, int ordinal, bool isFinally)
        {
            _startIndex = startIndex;
            _endIndex = endIndex;
            _handlerIndex = handlerIndex;
            _catchType = catchType;
            _ordinal = ordinal;
            _isFinally = isFinally;
        }

        public readonly int StartIndex => _startIndex;

        public readonly int EndIndex => _endIndex;

        public readonly int HandlerIndex => _handlerIndex;

        public readonly ClassConstantHandle CatchType => _catchType;

        public readonly int Ordinal => _ordinal;

        public readonly bool IsFinally => _isFinally;

    }

}
