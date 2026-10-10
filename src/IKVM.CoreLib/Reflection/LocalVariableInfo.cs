/*
  Copyright (C) 2009-2011 Jeroen Frijters

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

namespace IKVM.Reflection
{

    internal class LocalVariableInfo
    {

        readonly int _index;
        readonly Type _type;
        readonly bool _pinned;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="type"></param>
        /// <param name="pinned"></param>
        internal LocalVariableInfo(int index, Type type, bool pinned)
        {
            this._index = index;
            this._type = type;
            this._pinned = pinned;
        }

        public int LocalIndex
        {
            get { return _index; }
        }

        public Type LocalType
        {
            get { return _type; }
        }

        public override string ToString()
        {
            return String.Format(_pinned ? "{0} ({1}) (pinned)" : "{0} ({1})", _type, _index);
        }

    }

}