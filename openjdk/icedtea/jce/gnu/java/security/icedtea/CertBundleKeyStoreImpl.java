/* CertBundleKeyStoreImpl.java
   Copyright (C) 2007  Casey Marshall <csm@gnu.org>

This file is part of IcedTea.

IcedTea is free software; you can redistribute it and/or 
modify it under the terms of the GNU General Public License as
published by the Free Software Foundation, version 2.

IcedTea is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
General Public License for more details.

You should have received a copy of the GNU General Public License
along with IcedTea; see the file COPYING.  If not, write to
the Free Software Foundation, Inc., 51 Franklin Street, Fifth Floor,
Boston, MA 02110-1301 USA.

Linking this library statically or dynamically with other modules is
making a combined work based on this library.  Thus, the terms and
conditions of the GNU General Public License cover the whole
combination.

As a special exception, the copyright holders of this library give you
permission to link this library with independent modules to produce an
executable, regardless of the license terms of these independent
modules, and to copy and distribute the resulting executable under
terms of your choice, provided that you also meet, for each linked
independent module, the terms and conditions of the license of that
module.  An independent module is a module which is not derived from
or based on this library.  If you modify this library, you may extend
this exception to your version of the library, but you are not
obligated to do so.  If you do not wish to do so, delete this
exception statement from your version.  */


package gnu.java.security.icedtea;

import java.io.BufferedReader;
import java.io.ByteArrayInputStream;
import java.io.ByteArrayOutputStream;
import java.io.IOException;
import java.io.InputStream;
import java.io.InputStreamReader;
import java.io.OutputStream;
import java.io.OutputStreamWriter;
import java.io.PrintWriter;
import java.security.Key;
import java.security.KeyStoreException;
import java.security.KeyStoreSpi;
import java.security.NoSuchAlgorithmException;
import java.security.UnrecoverableKeyException;
import java.security.cert.Certificate;
import java.security.cert.CertificateException;
import java.security.cert.CertificateFactory;
import java.util.Date;
import java.util.Enumeration;
import java.util.HashMap;
import java.util.Map;
import java.util.Vector;

/**
 * A key store implementation for "certificate bundle" files, commonly used
 * on many free operating systems. Certificate bundles are plain text files
 * containing one or more "PEM" encoded X.509 certificates, which comprise
 * a list of trusted root certificates.
 * 
 * This class implements a read-only key store that reads in one or more
 * certificate bundles, storing all certificates successfully read. Calling
 * load multiple times will add certificates to the store.
 * 
 * @author Casey Marshall (csm@gnu.org)
 */
public class CertBundleKeyStoreImpl extends KeyStoreSpi
{
  private int x = 0;
  private Map<String, Certificate> certs = new HashMap<String, Certificate>();

  @Override public Enumeration<String> engineAliases()
  {
    return new Vector<String>(certs.keySet()).elements();
  }

  @Override public boolean engineContainsAlias(String alias)
  {
    return certs.containsKey(alias);
  }

  @Override public void engineDeleteEntry(String alias) throws KeyStoreException
  {
    certs.remove(alias);
  }

  @Override public Certificate engineGetCertificate(String alias)
  {
    return certs.get(alias);
  }

  @Override public String engineGetCertificateAlias(Certificate cert)
  {
    for (Map.Entry<String, Certificate> e : certs.entrySet())
      {
        if (e.getValue().equals(cert))
          return e.getKey();
      }
    return null;
  }

  @Override public Certificate[] engineGetCertificateChain(String arg0)
  {
    return null;
  }

  @Override public Date engineGetCreationDate(String alias)
  {
    return new Date(0);
  }

  @Override public Key engineGetKey(String arg0, char[] arg1)
    throws NoSuchAlgorithmException, UnrecoverableKeyException
  {
    return null;
  }

  @Override public boolean engineIsCertificateEntry(String alias)
  {
    return certs.containsKey(alias);
  }

  @Override public boolean engineIsKeyEntry(String arg0)
  {
    return false;
  }

  @Override public void engineLoad(InputStream in, char[] arg1)
    throws IOException, NoSuchAlgorithmException, CertificateException
  {
    CertificateFactory cf = CertificateFactory.getInstance("X.509");
    ByteArrayOutputStream bout = new ByteArrayOutputStream();
    PrintWriter out = new PrintWriter(new OutputStreamWriter(bout));
    BufferedReader rin = new BufferedReader(new InputStreamReader(in));
    String line;
    boolean push = false;
    while ((line = rin.readLine()) != null)
      {
        if (line.equals("-----BEGIN CERTIFICATE-----"))
          {
            push = true;
            out.println(line);
          }
        else if (push)
          {
            out.println(line);
            if (line.equals("-----END CERTIFICATE-----"))
              {
                push = false;
		out.flush();
		byte[] bytes = bout.toByteArray();
                Certificate cert = cf.generateCertificate(new ByteArrayInputStream(bytes));
                bout.reset();
                String alias = "cert-" + (x++);
                certs.put(alias, cert);
              }
          }
      }
  }

  @Override public void engineSetCertificateEntry(String alias, Certificate cert)
    throws KeyStoreException
  {
    certs.put(alias, cert);
  }

  @Override public void engineSetKeyEntry(String arg0, byte[] arg1,
                                          Certificate[] arg2)
    throws KeyStoreException
  {
    throw new KeyStoreException("not supported");
  }

  @Override public void engineSetKeyEntry(String arg0, Key arg1, char[] arg2,
                                          Certificate[] arg3)
    throws KeyStoreException
  {
    throw new KeyStoreException("not supported");
  }

  @Override public int engineSize()
  {
    return certs.size();
  }

  @Override public void engineStore(OutputStream arg0, char[] arg1)
    throws IOException, NoSuchAlgorithmException, CertificateException
  {
    throw new UnsupportedOperationException("read-only key stores");
  }
}
