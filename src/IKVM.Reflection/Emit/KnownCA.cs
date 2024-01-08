/*
  Copyright (C) 2008-2013 Jeroen Frijters

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
    // These are the pseudo-custom attributes that are recognized by name by the runtime (i.e. the type identity is not considered).
    // The corresponding list in the runtime is at https://github.com/dotnet/coreclr/blob/1afe5ce4f45045d724a4e129df4b816655d486fb/src/md/compiler/custattr_emit.cpp#L38
    // Note that we only need to handle a subset of the types, since we don't need the ones that are only used for validation by the runtime.
    enum KnownCA
    {
        Unknown,
        DllImportAttribute,
        ComImportAttribute,
        SerializableAttribute,
        NonSerializedAttribute,
        MethodImplAttribute,
        MarshalAsAttribute,
        PreserveSigAttribute,
        InAttribute,
        OutAttribute,
        OptionalAttribute,
        StructLayoutAttribute,
        FieldOffsetAttribute,
        SpecialNameAttribute,
        // the following is not part of the runtime known custom attributes, but we handle it here for efficiency and convenience
        SuppressUnmanagedCodeSecurityAttribute,
    }
}
