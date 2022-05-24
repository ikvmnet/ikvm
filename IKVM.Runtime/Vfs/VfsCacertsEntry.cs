/*
  Copyright (C) 2007-2011 Jeroen Frijters

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
using System.IO;
using System.Security.Cryptography.X509Certificates;

namespace IKVM.Runtime.Vfs
{

    /// <summary>
    /// Represents a fake cacerts file.
    /// </summary>
    sealed class VfsCacertsEntry : VfsFile
    {

        readonly Lazy<byte[]> buff;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="type"></param>
        internal VfsCacertsEntry(VfsContext context) :
            base(context)
        {
            this.buff = new Lazy<byte[]>(GenerateCacertsFile, true);
        }

        /// <summary>
        /// Gets the class file.
        /// </summary>
        /// <returns></returns>
        byte[] GenerateCacertsFile()
        {
#if FIRST_PASS
            return null;
#else
            var jstore = global::java.security.KeyStore.getInstance("jks");
            jstore.load(null);
            var cf = global::java.security.cert.CertificateFactory.getInstance("X509");

            using var store = new X509Store(StoreName.Root, StoreLocation.LocalMachine);
            store.Open(OpenFlags.ReadOnly | OpenFlags.OpenExistingOnly);

            var aliases = new Dictionary<string, string>();
            foreach (X509Certificate2 cert in store.Certificates)
            {
                if (cert.HasPrivateKey)
                    continue;

                // the alias must be unique, otherwise we overwrite the previous certificate with that alias
                var alias = cert.Subject;
                var unique = 0;
                while (aliases.ContainsKey(alias))
                    alias = cert.Subject + " #" + (++unique);

                aliases.Add(alias, null);
                jstore.setCertificateEntry(alias, cf.generateCertificate(new global::java.io.ByteArrayInputStream(cert.RawData)));
            }

            global::java.io.ByteArrayOutputStream baos = new global::java.io.ByteArrayOutputStream();
            jstore.store(baos, new char[0]);
            return baos.toByteArray();
#endif
        }

        /// <summary>
        /// Opens the class file.
        /// </summary>
        /// <returns></returns>
        protected override Stream OpenRead() => new MemoryStream(buff.Value);

        /// <summary>
        /// Gets the length of the class file.
        /// </summary>
        public override long Size => buff.Value.Length;

    }

}
