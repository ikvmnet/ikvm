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

using IKVM.Runtime;

namespace IKVM.Tools.Importer
{

    sealed class FatalCompilerErrorException : Exception
    {

        internal FatalCompilerErrorException(Message id, params object[] args) :
            base($"fatal error IKVMC{(int)id}: {(args.Length == 0 ? GetMessage(id) : string.Format(GetMessage(id), args))}")
        {

        }

        private static string GetMessage(Message id)
        {
            switch (id)
            {
                case IKVM.Tools.Importer.Message.ResponseFileDepthExceeded:
                    return "Response file nesting depth exceeded";
                case IKVM.Tools.Importer.Message.ErrorReadingFile:
                    return "Unable to read file: {0}\n\t({1})";
                case IKVM.Tools.Importer.Message.NoTargetsFound:
                    return "No targets found";
                case IKVM.Tools.Importer.Message.FileFormatLimitationExceeded:
                    return "File format limitation exceeded: {0}";
                case IKVM.Tools.Importer.Message.CannotSpecifyBothKeyFileAndContainer:
                    return "You cannot specify both a key file and container";
                case IKVM.Tools.Importer.Message.DelaySignRequiresKey:
                    return "You cannot delay sign without a key file or container";
                case IKVM.Tools.Importer.Message.InvalidStrongNameKeyPair:
                    return "Invalid key {0} specified.\n\t(\"{1}\")";
                case IKVM.Tools.Importer.Message.ReferenceNotFound:
                    return "Reference not found: {0}";
                case IKVM.Tools.Importer.Message.OptionsMustPreceedChildLevels:
                    return "You can only specify options before any child levels";
                case IKVM.Tools.Importer.Message.UnrecognizedTargetType:
                    return "Invalid value '{0}' for -target option";
                case IKVM.Tools.Importer.Message.UnrecognizedPlatform:
                    return "Invalid value '{0}' for -platform option";
                case IKVM.Tools.Importer.Message.UnrecognizedApartment:
                    return "Invalid value '{0}' for -apartment option";
                case IKVM.Tools.Importer.Message.MissingFileSpecification:
                    return "Missing file specification for '{0}' option";
                case IKVM.Tools.Importer.Message.PathTooLong:
                    return "Path too long: {0}";
                case IKVM.Tools.Importer.Message.PathNotFound:
                    return "Path not found: {0}";
                case IKVM.Tools.Importer.Message.InvalidPath:
                    return "Invalid path: {0}";
                case IKVM.Tools.Importer.Message.InvalidOptionSyntax:
                    return "Invalid option: {0}";
                case IKVM.Tools.Importer.Message.ExternalResourceNotFound:
                    return "External resource file does not exist: {0}";
                case IKVM.Tools.Importer.Message.ExternalResourceNameInvalid:
                    return "External resource file may not include path specification: {0}";
                case IKVM.Tools.Importer.Message.InvalidVersionFormat:
                    return "Invalid version specified: {0}";
                case IKVM.Tools.Importer.Message.InvalidFileAlignment:
                    return "Invalid value '{0}' for -filealign option";
                case IKVM.Tools.Importer.Message.ErrorWritingFile:
                    return "Unable to write file: {0}\n\t({1})";
                case IKVM.Tools.Importer.Message.UnrecognizedOption:
                    return "Unrecognized option: {0}";
                case IKVM.Tools.Importer.Message.NoOutputFileSpecified:
                    return "No output file specified";
                case IKVM.Tools.Importer.Message.SharedClassLoaderCannotBeUsedOnModuleTarget:
                    return "Incompatible options: -target:module and -sharedclassloader cannot be combined";
                case IKVM.Tools.Importer.Message.RuntimeNotFound:
                    return "Unable to load runtime assembly";
                case IKVM.Tools.Importer.Message.MainClassRequiresExe:
                    return "Main class cannot be specified for library or module";
                case IKVM.Tools.Importer.Message.ExeRequiresMainClass:
                    return "No main method found";
                case IKVM.Tools.Importer.Message.PropertiesRequireExe:
                    return "Properties cannot be specified for library or module";
                case IKVM.Tools.Importer.Message.ModuleCannotHaveClassLoader:
                    return "Cannot specify assembly class loader for modules";
                case IKVM.Tools.Importer.Message.ErrorParsingMapFile:
                    return "Unable to parse remap file: {0}\n\t({1})";
                case IKVM.Tools.Importer.Message.BootstrapClassesMissing:
                    return "Bootstrap classes missing and core assembly not found";
                case IKVM.Tools.Importer.Message.StrongNameRequiresStrongNamedRefs:
                    return "All referenced assemblies must be strong named, to be able to sign the output assembly";
                case IKVM.Tools.Importer.Message.MainClassNotFound:
                    return "Main class not found";
                case IKVM.Tools.Importer.Message.MainMethodNotFound:
                    return "Main method not found";
                case IKVM.Tools.Importer.Message.UnsupportedMainMethod:
                    return "Redirected main method not supported";
                case IKVM.Tools.Importer.Message.ExternalMainNotAccessible:
                    return "External main method must be public and in a public class";
                case IKVM.Tools.Importer.Message.ClassLoaderNotFound:
                    return "Custom assembly class loader class not found";
                case IKVM.Tools.Importer.Message.ClassLoaderNotAccessible:
                    return "Custom assembly class loader class is not accessible";
                case IKVM.Tools.Importer.Message.ClassLoaderIsAbstract:
                    return "Custom assembly class loader class is abstract";
                case IKVM.Tools.Importer.Message.ClassLoaderNotClassLoader:
                    return "Custom assembly class loader class does not extend java.lang.ClassLoader";
                case IKVM.Tools.Importer.Message.ClassLoaderConstructorMissing:
                    return "Custom assembly class loader constructor is missing";
                case IKVM.Tools.Importer.Message.MapFileTypeNotFound:
                    return "Type '{0}' referenced in remap file was not found";
                case IKVM.Tools.Importer.Message.MapFileClassNotFound:
                    return "Class '{0}' referenced in remap file was not found";
                case IKVM.Tools.Importer.Message.MaximumErrorCountReached:
                    return "Maximum error count reached";
                case IKVM.Tools.Importer.Message.LinkageError:
                    return "Link error: {0}";
                case IKVM.Tools.Importer.Message.RuntimeMismatch:
                    return "Referenced assembly {0} was compiled with an incompatible IKVM.Runtime version\n" +
                        "\tCurrent runtime: {1}\n" +
                        "\tReferenced assembly runtime: {2}";
                case IKVM.Tools.Importer.Message.CoreClassesMissing:
                    return "Failed to find core classes in core library";
                case IKVM.Tools.Importer.Message.CriticalClassNotFound:
                    return "Unable to load critical class '{0}'";
                case IKVM.Tools.Importer.Message.AssemblyContainsDuplicateClassNames:
                    return "Type '{0}' and '{1}' both map to the same name '{2}'\n\t({3})";
                case IKVM.Tools.Importer.Message.CallerIDRequiresHasCallerIDAnnotation:
                    return "CallerID.getCallerID() requires a HasCallerID annotation";
                case IKVM.Tools.Importer.Message.UnableToResolveInterface:
                    return "Unable to resolve interface '{0}' on type '{1}'";
                case IKVM.Tools.Importer.Message.MissingBaseType:
                    return "The base class or interface '{0}' in assembly '{1}' referenced by type '{2}' in '{3}' could not be resolved";
                case IKVM.Tools.Importer.Message.MissingBaseTypeReference:
                    return "The type '{0}' is defined in an assembly that is not referenced. You must add a reference to assembly '{1}'";
                case IKVM.Tools.Importer.Message.FileNotFound:
                    return "File not found: {0}";
                case IKVM.Tools.Importer.Message.RuntimeMethodMissing:
                    return "Runtime method '{0}' not found";
                case IKVM.Tools.Importer.Message.MapFileFieldNotFound:
                    return "Field '{0}' referenced in remap file was not found in class '{1}'";
                case IKVM.Tools.Importer.Message.GhostInterfaceMethodMissing:
                    return "Remapped class '{0}' does not implement ghost interface method\n\t({1}.{2}{3})";
                case Importer.Message.ModuleInitializerMethodRequirements:
                    return "Method '{1}.{2}{3}' does not meet the requirements of a module initializer.";
                default:
                    return "Missing Error IKVM.Tools.Importer.Message. Please file a bug.";
            }
        }
    }

}
