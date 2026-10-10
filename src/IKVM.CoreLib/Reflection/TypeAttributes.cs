/*
  Copyright (C) 2009-2012 Jeroen Frijters

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

    /// <summary>
    /// Specifies type attributes.
    /// </summary>
    [Flags]
    enum TypeAttributes
    {

        /// <summary>
        /// LPTSTR is interpreted as ANSI.
        /// </summary>
        AnsiClass = 0,

        /// <summary>
        /// Specifies that the type is a class.
        /// </summary>
        Class = 0,

        /// <summary>
        /// Specifies that class fields are automatically laid out by the common language runtime.
        /// </summary>
        AutoLayout = 0,

        /// <summary>
        /// Specifies that the class is not public.
        /// </summary>
        NotPublic = 0,

        /// <summary>
        /// Specifies that the class is public.
        /// </summary>
        Public = 1,

        /// <summary>
        /// Specifies that the class is nested with public visibility.
        /// </summary>
        NestedPublic = 2,

        /// <summary>
        /// Specifies that the class is nested with private visibility.
        /// </summary>
        NestedPrivate = 3,

        /// <summary>
        /// Specifies that the class is nested with family visibility, and is thus accessible only by methods within its
        /// own type and any derived types.
        /// </summary>
        NestedFamily = 4,

        /// <summary>
        /// Specifies that the class is nested with assembly visibility, and is thus accessible only by methods within its
        /// assembly.
        /// </summary>
        NestedAssembly = 5,

        /// <summary>
        /// Specifies that the class is nested with assembly and family visibility, and is thus accessible only by methods
        /// lying in the intersection of its family and assembly.
        /// </summary>
        NestedFamANDAssem = 6,

        /// <summary>
        /// Specifies type visibility information.
        /// </summary>
        VisibilityMask = 7,

        /// <summary>
        /// Specifies that the class is nested with family or assembly visibility, and is thus accessible only by methods
        /// lying in the union of its family and assembly.
        /// </summary>
        NestedFamORAssem = 7,

        /// <summary>
        /// Specifies that class fields are laid out sequentially, in the order that the fields were emitted to the
        /// metadata.
        /// </summary>
        SequentialLayout = 8,

        /// <summary>
        /// Specifies that class fields are laid out at the specified offsets.
        /// </summary>
        ExplicitLayout = 16,

        /// <summary>
        /// Specifies class layout information.
        /// </summary>
        LayoutMask = 24,

        /// <summary>
        /// Specifies class semantics information; the current class is contextful (else agile).
        /// </summary>
        ClassSemanticsMask = 32,

        /// <summary>
        /// Specifies that the type is an interface.
        /// </summary>
        Interface = 32,

        /// <summary>
        /// Specifies that the type is abstract.
        /// </summary>
        Abstract = 128,

        /// <summary>
        /// Specifies that the class is concrete and cannot be extended.
        /// </summary>
        Sealed = 256,

        /// <summary>
        /// Specifies that the class is special in a way denoted by the name.
        /// </summary>
        SpecialName = 1024,

        /// <summary>
        /// The runtime should check the name encoding.
        /// </summary>
        RTSpecialName = 2048,

        /// <summary>
        /// Specifies that the class or interface is imported from another module.
        /// </summary>
        Import = 4096,

        /// <summary>
        /// Specifies that the class can be serialized.
        /// </summary>
        Serializable = 8192,

        /// <summary>
        /// Specifies a Windows Runtime type.
        /// </summary>
        WindowsRuntime = 16384,

        /// <summary>
        /// LPTSTR is interpreted as UNICODE.
        /// </summary>
        UnicodeClass = 65536,

        /// <summary>
        /// LPTSTR is interpreted automatically.
        /// </summary>
        AutoClass = 131072,

        /// <summary>
        /// LPSTR is interpreted by some implementation-specific means.
        /// </summary>
        CustomFormatClass = 196608,

        /// <summary>
        /// Used to retrieve string information for native interoperability.
        /// </summary>
        StringFormatMask = 196608,

        /// <summary>
        /// Type has security associated with it.
        /// </summary>
        HasSecurity = 262144,

        /// <summary>
        /// Attributes reserved for runtime use.
        /// </summary>
        ReservedMask = 264192,

        /// <summary>
        /// Specifies that calling static methods of the type does not force the system to initialize the type.
        /// </summary>
        BeforeFieldInit = 1048576,

        /// <summary>
        /// Used to retrieve non-standard encoding information for native interop.
        /// </summary>
        CustomFormatMask = 12582912,

    }

}
