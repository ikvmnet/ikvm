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
    public struct Label
    {

        // 1-based here, to make sure that an uninitialized Label isn't valid
        private readonly int index1;

        internal Label(int index)
        {
            this.index1 = index + 1;
        }

        internal int Index
        {
            get { return index1 - 1; }
        }

        public bool Equals(Label other)
        {
            return other.index1 == index1;
        }

        public override bool Equals(object obj)
        {
            return this == obj as Label?;
        }

        public override int GetHashCode()
        {
            return index1;
        }

        public static bool operator ==(Label arg1, Label arg2)
        {
            return arg1.index1 == arg2.index1;
        }

        public static bool operator !=(Label arg1, Label arg2)
        {
            return !(arg1 == arg2);
        }
    }

}
