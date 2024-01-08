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

namespace IKVM.Reflection.Emit
{
    public struct SignatureToken
	{
		public static readonly SignatureToken Empty;
		private readonly int token;

		internal SignatureToken(int token)
		{
			this.token = token;
		}

		public int Token
		{
			get { return token; }
		}

		public override bool Equals(object obj)
		{
			return obj as SignatureToken? == this;
		}

		public override int GetHashCode()
		{
			return token;
		}

		public bool Equals(SignatureToken other)
		{
			return this == other;
		}

		public static bool operator ==(SignatureToken st1, SignatureToken st2)
		{
			return st1.token == st2.token;
		}

		public static bool operator !=(SignatureToken st1, SignatureToken st2)
		{
			return st1.token != st2.token;
		}
	}
}
