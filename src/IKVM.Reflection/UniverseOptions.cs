/*
  Copyright (C) 2009-2013 Jeroen Frijters

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

    /*
	 * UniverseOptions:
	 *
	 *   None
	 *		Default behavior, most compatible with System.Reflection[.Emit]
	 *
	 *   EnableFunctionPointers
	 *		Normally function pointers in signatures are replaced by System.IntPtr
	 *		(for compatibility with System.Reflection), when this option is enabled
	 *		they are represented as first class types (Type.__IsFunctionPointer will
	 *		return true for them).
	 *
	 *   DisableFusion
	 *      Don't use native Fusion API to resolve assembly names.
	 *
	 *   DisablePseudoCustomAttributeRetrieval
	 *      Set this option to disable the generaton of pseudo-custom attributes
	 *      when querying custom attributes.
	 *
	 *   DontProvideAutomaticDefaultConstructor
	 *      Normally TypeBuilder, like System.Reflection.Emit, will provide a default
	 *      constructor for types that meet the requirements. By enabling this
	 *      option this behavior is disabled.
	 *
	 *   MetadataOnly
	 *      By default, when a module is read in, the stream is kept open to satisfy
	 *      subsequent lazy loading. In MetadataOnly mode only the metadata is read in
	 *      and after that the stream is closed immediately. Subsequent lazy loading
	 *      attempts will fail with an InvalidOperationException.
	 *      APIs that are not available is MetadataOnly mode are:
	 *      - Module.ResolveString()
	 *      - Module.GetSignerCertificate()
	 *      - Module.GetManifestResourceStream()
	 *      - Module.__ReadDataFromRVA()
	 *      - MethodBase.GetMethodBody()
	 *      - FieldInfo.__GetDataFromRVA()
	 *
	 *   DeterministicOutput
	 *      The generated output file will depend only on the input. In other words,
	 *      the PE file header time stamp will be set to zero and the module version
	 *      id will be based on a SHA1 of the contents, instead of a random guid.
	 *      This option can not be used in combination with PDB file generation.
	 */

    [Flags]
    public enum UniverseOptions
    {

        None = 0,
        EnableFunctionPointers = 1,
        DisableFusion = 2,
        DisablePseudoCustomAttributeRetrieval = 4,
        DontProvideAutomaticDefaultConstructor = 8,
        MetadataOnly = 16,
        ResolveMissingMembers = 32,
        DisableWindowsRuntimeProjection = 64,
        DecodeVersionInfoAttributeBlobs = 128,
        DeterministicOutput = 256,
        DisableDefaultAssembliesLookup = 512,
    }

}
