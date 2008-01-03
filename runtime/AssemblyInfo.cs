/*
  Copyright (C) 2002, 2003, 2004, 2005, 2006 Jeroen Frijters

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
using System.Reflection;
using System.Runtime.CompilerServices;

//
// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
//
[assembly: AssemblyTitle("IKVM.NET Runtime")]
[assembly: AssemblyDescription("JVM for Mono and .NET")]

[assembly: System.Security.SecurityCritical]
[assembly: System.Security.AllowPartiallyTrustedCallers]

#if SIGNCODE
[assembly: InternalsVisibleTo("IKVM.Runtime.JNI, PublicKey=0024000004800000940000000602000000240000525341310004000001000100DD6B140E5209CAE3D1C710030021EF589D0F00D05ACA8771101A7E99E10EE063E66040DF96E6F842F717BFC5B62D2EC2B62CEB0282E4649790DACB424DB29B68ADC7EAEAB0356FCE04702379F84400B8427EDBB33DAB8720B9F16A42E2CDB87F885EF413DBC4229F2BD157C9B8DC2CD14866DEC5F31C764BFB9394CC3C60E6C0")]
#if OPENJDK
[assembly: InternalsVisibleTo("IKVM.OpenJDK.ClassLibrary, PublicKey=0024000004800000940000000602000000240000525341310004000001000100DD6B140E5209CAE3D1C710030021EF589D0F00D05ACA8771101A7E99E10EE063E66040DF96E6F842F717BFC5B62D2EC2B62CEB0282E4649790DACB424DB29B68ADC7EAEAB0356FCE04702379F84400B8427EDBB33DAB8720B9F16A42E2CDB87F885EF413DBC4229F2BD157C9B8DC2CD14866DEC5F31C764BFB9394CC3C60E6C0")]
#else
[assembly: InternalsVisibleTo("IKVM.GNU.Classpath, PublicKey=0024000004800000940000000602000000240000525341310004000001000100DD6B140E5209CAE3D1C710030021EF589D0F00D05ACA8771101A7E99E10EE063E66040DF96E6F842F717BFC5B62D2EC2B62CEB0282E4649790DACB424DB29B68ADC7EAEAB0356FCE04702379F84400B8427EDBB33DAB8720B9F16A42E2CDB87F885EF413DBC4229F2BD157C9B8DC2CD14866DEC5F31C764BFB9394CC3C60E6C0")]
#endif
#else
[assembly: InternalsVisibleTo("IKVM.Runtime.JNI")]
#if OPENJDK
[assembly: InternalsVisibleTo("IKVM.OpenJDK.ClassLibrary")]
#else
[assembly: InternalsVisibleTo("IKVM.GNU.Classpath")]
#endif
#endif
