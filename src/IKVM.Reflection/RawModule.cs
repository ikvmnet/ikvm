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

using IKVM.Reflection.Reader;

namespace IKVM.Reflection
{

    public sealed class RawModule : IDisposable
    {

        readonly ModuleReader module;
        readonly bool isManifestModule;
        bool imported;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="module"></param>
        internal RawModule(ModuleReader module)
        {
            this.module = module;
            this.isManifestModule = module.Assembly != null;
        }

        public string Location
        {
            get { return module.FullyQualifiedName; }
        }

        public bool IsManifestModule
        {
            get { return isManifestModule; }
        }

        public Guid ModuleVersionId
        {
            get { return module.ModuleVersionId; }
        }

        public string ImageRuntimeVersion
        {
            get { return module.__ImageRuntimeVersion; }
        }

        public int MDStreamVersion
        {
            get { return module.MDStreamVersion; }
        }

        void CheckManifestModule()
        {
            if (!IsManifestModule)
                throw new BadImageFormatException("Module does not contain a manifest");
        }

        public AssemblyName GetAssemblyName()
        {
            CheckManifestModule();
            return module.Assembly.GetName();
        }

        public AssemblyName[] GetReferencedAssemblies()
        {
            return module.__GetReferencedAssemblies();
        }

        public void Dispose()
        {
            if (!imported)
                module.Dispose();
        }

        internal AssemblyReader ToAssembly()
        {
            if (imported)
                throw new InvalidOperationException();

            imported = true;
            return (AssemblyReader)module.Assembly;
        }

        internal Module ToModule(Assembly assembly)
        {
            if (module.Assembly != null)
                throw new InvalidOperationException();

            imported = true;
            module.SetAssembly(assembly);
            return module;
        }

    }

}
