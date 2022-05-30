/*
  Copyright (C) 2002-2014 Jeroen Frijters

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

namespace IKVM.Internal
{

    /// <summary>
    /// Describes a fatal compiler error.
    /// </summary>
    sealed class FatalCompilerErrorException : Exception
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="args"></param>
        internal FatalCompilerErrorException(Message id, params object[] args) :
            base($"fatal error IKVMC{(int)id}: {(args.Length == 0 ? GetMessage(id) : string.Format(GetMessage(id), args))}")
        {

        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="innerException"></param>
        /// <param name="args"></param>
        internal FatalCompilerErrorException(Message id, Exception innerException, params object[] args) :
            base($"fatal error IKVMC{(int)id}: {(args.Length == 0 ? GetMessage(id) : string.Format(GetMessage(id), args))}", innerException)
        {

        }

        static string GetMessage(Message id) => id switch
        {
            IKVM.Internal.Message.ResponseFileDepthExceeded => "Response file nesting depth exceeded",
            IKVM.Internal.Message.ErrorReadingFile => "Unable to read file: {0}\n\t({1})",
            IKVM.Internal.Message.NoTargetsFound => "No targets found",
            IKVM.Internal.Message.FileFormatLimitationExceeded => "File format limitation exceeded: {0}",
            IKVM.Internal.Message.CannotSpecifyBothKeyFileAndContainer => "You cannot specify both a key file and container",
            IKVM.Internal.Message.DelaySignRequiresKey => "You cannot delay sign without a key file or container",
            IKVM.Internal.Message.InvalidStrongNameKeyPair => "Invalid key {0} specified.\n\t(\"{1}\")",
            IKVM.Internal.Message.ReferenceNotFound => "Reference not found: {0}",
            IKVM.Internal.Message.OptionsMustPreceedChildLevels => "You can only specify options before any child levels",
            IKVM.Internal.Message.UnrecognizedTargetType => "Invalid value '{0}' for -target option",
            IKVM.Internal.Message.UnrecognizedPlatform => "Invalid value '{0}' for -platform option",
            IKVM.Internal.Message.UnrecognizedApartment => "Invalid value '{0}' for -apartment option",
            IKVM.Internal.Message.MissingFileSpecification => "Missing file specification for '{0}' option",
            IKVM.Internal.Message.PathTooLong => "Path too long: {0}",
            IKVM.Internal.Message.PathNotFound => "Path not found: {0}",
            IKVM.Internal.Message.InvalidPath => "Invalid path: {0}",
            IKVM.Internal.Message.InvalidOptionSyntax => "Invalid option: {0}",
            IKVM.Internal.Message.ExternalResourceNotFound => "External resource file does not exist: {0}",
            IKVM.Internal.Message.ExternalResourceNameInvalid => "External resource file may not include path specification: {0}",
            IKVM.Internal.Message.InvalidVersionFormat => "Invalid version specified: {0}",
            IKVM.Internal.Message.InvalidFileAlignment => "Invalid value '{0}' for -filealign option",
            IKVM.Internal.Message.ErrorWritingFile => "Unable to write file: {0}\n\t({1})",
            IKVM.Internal.Message.UnrecognizedOption => "Unrecognized option: {0}",
            IKVM.Internal.Message.NoOutputFileSpecified => "No output file specified",
            IKVM.Internal.Message.SharedClassLoaderCannotBeUsedOnModuleTarget => "Incompatible options: -target:module and -sharedclassloader cannot be combined",
            IKVM.Internal.Message.RuntimeNotFound => "Unable to load runtime assembly",
            IKVM.Internal.Message.MainClassRequiresExe => "Main class cannot be specified for library or module",
            IKVM.Internal.Message.ExeRequiresMainClass => "No main method found",
            IKVM.Internal.Message.PropertiesRequireExe => "Properties cannot be specified for library or module",
            IKVM.Internal.Message.ModuleCannotHaveClassLoader => "Cannot specify assembly class loader for modules",
            IKVM.Internal.Message.ErrorParsingMapFile => "Unable to parse remap file: {0}\n\t({1})",
            IKVM.Internal.Message.BootstrapClassesMissing => "Bootstrap classes missing and core assembly not found",
            IKVM.Internal.Message.StrongNameRequiresStrongNamedRefs => "All referenced assemblies must be strong named, to be able to sign the output assembly",
            IKVM.Internal.Message.MainClassNotFound => "Main class not found",
            IKVM.Internal.Message.MainMethodNotFound => "Main method not found",
            IKVM.Internal.Message.UnsupportedMainMethod => "Redirected main method not supported",
            IKVM.Internal.Message.ExternalMainNotAccessible => "External main method must be public and in a public class",
            IKVM.Internal.Message.ClassLoaderNotFound => "Custom assembly class loader class not found",
            IKVM.Internal.Message.ClassLoaderNotAccessible => "Custom assembly class loader class is not accessible",
            IKVM.Internal.Message.ClassLoaderIsAbstract => "Custom assembly class loader class is abstract",
            IKVM.Internal.Message.ClassLoaderNotClassLoader => "Custom assembly class loader class does not extend java.lang.ClassLoader",
            IKVM.Internal.Message.ClassLoaderConstructorMissing => "Custom assembly class loader constructor is missing",
            IKVM.Internal.Message.MapFileTypeNotFound => "Type '{0}' referenced in remap file was not found",
            IKVM.Internal.Message.MapFileClassNotFound => "Class '{0}' referenced in remap file was not found",
            IKVM.Internal.Message.MaximumErrorCountReached => "Maximum error count reached",
            IKVM.Internal.Message.LinkageError => "Link error: {0}",
            IKVM.Internal.Message.RuntimeMismatch => "Referenced assembly {0} was compiled with an incompatible IKVM.Runtime version\n" + "\tCurrent runtime: {1}\n" + "\tReferenced assembly runtime: {2}",
            IKVM.Internal.Message.CoreClassesMissing => "Failed to find core classes in core library",
            IKVM.Internal.Message.CriticalClassNotFound => "Unable to load critical class '{0}'",
            IKVM.Internal.Message.AssemblyContainsDuplicateClassNames => "Type '{0}' and '{1}' both map to the same name '{2}'\n" + "\t({3})",
            IKVM.Internal.Message.CallerIDRequiresHasCallerIDAnnotation => "CallerID.getCallerID() requires a HasCallerID annotation",
            IKVM.Internal.Message.UnableToResolveInterface => "Unable to resolve interface '{0}' on type '{1}'",
            IKVM.Internal.Message.MissingBaseType => "The base class or interface '{0}' in assembly '{1}' referenced by type '{2}' in '{3}' could not be resolved",
            IKVM.Internal.Message.MissingBaseTypeReference => "The type '{0}' is defined in an assembly that is not referenced. You must add a reference to assembly '{1}'",
            IKVM.Internal.Message.FileNotFound => "File not found: {0}",
            IKVM.Internal.Message.RuntimeMethodMissing => "Runtime method '{0}' not found",
            IKVM.Internal.Message.MapFileFieldNotFound => "Field '{0}' referenced in remap file was not found in class '{1}'",
            IKVM.Internal.Message.GhostInterfaceMethodMissing => "Remapped class '{0}' does not implement ghost interface method\n" + "\t({1}.{2}{3})",
            _ => "Missing Error Message. Please file a bug.",
        };

    }

}