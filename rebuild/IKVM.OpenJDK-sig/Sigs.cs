/*
  Copyright (C) 2010-2014 Jeroen Frijters

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

namespace ikvm
{
	namespace @internal
	{
		public class CallerID { }
	}
}

namespace java
{
	namespace io
	{
		public class File { }
		public class FileDescriptor { }
		public class ObjectInputStream { }
		public class ObjectOutputStream { }
		public class ObjectStreamField { }
		public class PrintStream { }
		public class PrintWriter { }
	}

	namespace lang
	{
		public class Class { }
		public class ClassLoader { }
		public class IllegalArgumentException { }
		public class SecurityManager { }
		public class StackTraceElement { }

		namespace invoke
		{
			public class DirectMethodHandle { }
			public class LambdaForm { }
			public class MemberName { }
			public class MethodType { }
			public class MethodHandle { }
			public class CallSite { }
		}

		namespace reflect
		{
			public class Constructor : Executable { }
			public class Executable { }
			public class Field { }
			public class Method : Executable { }
		}
	}

	namespace net
	{
		public class URL { }
		public class InetAddress { }
	}

	namespace nio
	{
		public class ByteBuffer { }
	}

	namespace security
	{
		public class AccessControlContext { }
		public class ProtectionDomain { }
	}

	namespace util
	{
		public class Enumeration { }
		public class Vector { }
	}
}

namespace sun.reflect
{
	public interface ConstructorAccessor { }
	public interface FieldAccessor { }
	public interface MethodAccessor { }
}
