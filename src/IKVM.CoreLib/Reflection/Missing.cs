/*
  Copyright (C) 2008-2011 Jeroen Frijters

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
    /// Represents a missing <see cref="object"/>. This class cannot be inherited.
    /// </summary>
    [Serializable]
    sealed class Missing :
        System.Runtime.Serialization.ISerializable
    {

        /// <summary>
        /// Represents the sole instance of the <see cref="Missing"/> class.
        /// </summary>
        public static readonly Missing Value = new();

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        Missing()
        {

        }

        /// <summary>
        /// Sets up serialization so that the sole instance is serialized as a reference to
        /// <see cref="SingletonSerializationHelper"/>, which deserializes back to <see cref="Value"/>.
        /// </summary>
        /// <param name="info">The object to populate with serialization information.</param>
        /// <param name="context">The destination of the serialization.</param>
        void System.Runtime.Serialization.ISerializable.GetObjectData(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) =>
            info.SetType(typeof(SingletonSerializationHelper));

        /// <summary>
        /// Stands in for <see cref="Missing"/> during serialization, so that deserialization yields the sole instance.
        /// </summary>
        [Serializable]
        sealed class SingletonSerializationHelper : System.Runtime.Serialization.IObjectReference
        {

            /// <summary>
            /// Returns the object that this helper stands for.
            /// </summary>
            /// <param name="context">The source of the serialized stream.</param>
            /// <returns>The sole instance, <see cref="Value"/>.</returns>
            public object GetRealObject(System.Runtime.Serialization.StreamingContext context) => Value;

        }

    }

}
