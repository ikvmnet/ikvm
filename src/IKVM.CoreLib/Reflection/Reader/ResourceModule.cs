/*
  Copyright (C) 2009-2011 Jeroen Frijters

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
using System.Collections.Generic;

namespace IKVM.Reflection.Reader
{

    /// <summary>
    /// Represents a file of a multi-file assembly that the manifest's File table marks as containing no metadata. The
    /// module defines no types; its contents are only reachable as a resource.
    /// </summary>
    sealed class ResourceModule : NonPEModule
    {

        readonly ModuleReader _manifest;

        /// <summary>
        /// The index of the file in the manifest's File table.
        /// </summary>
        readonly int _index;

        readonly string _location;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="manifest">The manifest module of the assembly that lists the file.</param>
        /// <param name="index">The index of the file in the manifest's File table.</param>
        /// <param name="location">The full path of the file, or <see langword="null"/> if it is not known.</param>
        internal ResourceModule(ModuleReader manifest, int index, string location) :
            base(manifest.Universe)
        {
            _manifest = manifest;
            _index = index;
            _location = location;
        }

        /// <summary>
        /// Not supported for a resource module.
        /// </summary>
        /// <exception cref="NotSupportedException">Always thrown.</exception>
        public override int MDStreamVersion => throw new NotSupportedException();

        /// <summary>
        /// Gets a value indicating whether the object is a resource, which is always <see langword="true"/>.
        /// </summary>
        /// <returns><see langword="true"/>.</returns>
        public override bool IsResource() => true;

        /// <inheritdoc />
        public override Assembly Assembly => _manifest.Assembly;

        /// <summary>
        /// Gets the full path of the file, or "&lt;Unknown&gt;" if it is not known.
        /// </summary>
        public override string FullyQualifiedName => _location ?? "<Unknown>";

        /// <summary>
        /// Gets the file name of the file, without its path, or "&lt;Unknown&gt;" if it is not known.
        /// </summary>
        public override string Name => _location == null ? "<Unknown>" : System.IO.Path.GetFileName(_location);

        /// <summary>
        /// Gets the name of the file as listed in the manifest's File table.
        /// </summary>
        public override string ScopeName => _manifest.GetFileName(_index);

        /// <summary>
        /// Not supported for a resource module.
        /// </summary>
        /// <exception cref="NotSupportedException">Always thrown.</exception>
        public override Guid ModuleVersionId => throw new NotSupportedException();

        /// <summary>
        /// Finds no type, because a resource module defines none.
        /// </summary>
        /// <param name="typeName">The name of the type to find.</param>
        /// <returns>Always <see langword="null"/>.</returns>
        internal override Type FindType(TypeName typeName) => null;

        /// <summary>
        /// Finds no type, because a resource module defines none.
        /// </summary>
        /// <param name="lowerCaseName">The lower-case name of the type to find.</param>
        /// <returns>Always <see langword="null"/>.</returns>
        internal override Type FindTypeIgnoreCase(TypeName lowerCaseName) => null;

        /// <summary>
        /// Adds nothing, because a resource module defines no types.
        /// </summary>
        /// <param name="list">The list the types would be added to.</param>
        internal override void GetTypesImpl(List<Type> list)
        {
        }

        /// <summary>
        /// Creates the exception thrown where the base class would throw
        /// <see cref="System.ArgumentOutOfRangeException"/>.
        /// </summary>
        /// <returns>A <see cref="NotSupportedException"/>.</returns>
        protected override Exception ArgumentOutOfRangeException() => new NotSupportedException();

    }

}
