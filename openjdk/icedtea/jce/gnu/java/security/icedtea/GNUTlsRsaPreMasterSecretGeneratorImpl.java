/* GNUTlsRsaPreMasterSecretGeneratorImpl.java -- TLS pre-master secrets.
   Copyright (C) 2007  Casey Marshall <csm@gnu.org>

This file is part of IcedTea.

IcedTea is free software; you can redistribute it and/or 
modify it under the terms of the GNU General Public License
as published by the Free Software Foundation, version 2.

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
exception statement from your version. */


package gnu.java.security.icedtea;

import java.security.InvalidAlgorithmParameterException;
import java.security.SecureRandom;
import java.security.spec.AlgorithmParameterSpec;

import javax.crypto.KeyGeneratorSpi;
import javax.crypto.SecretKey;
import javax.crypto.spec.SecretKeySpec;

import sun.security.internal.spec.TlsRsaPremasterSecretParameterSpec;

/**
 * Implementation of a TLS pre-master secret generator.
 * 
 * This is used in the client-side handshake for RSA cipher suites. It
 * basically generates a 48 byte random string, where the first two
 * bytes are a protocol version.
 * 
 * @author csm
 *
 */
public class GNUTlsRsaPreMasterSecretGeneratorImpl extends KeyGeneratorSpi
{
  private TlsRsaPremasterSecretParameterSpec params;
  private SecureRandom random;

  public GNUTlsRsaPreMasterSecretGeneratorImpl()
  {
    params = null;
    random = null;
  }

  @Override
  protected SecretKey engineGenerateKey()
  {
    if (params == null || random == null)
      throw new IllegalStateException("not ready to generate keys");
    final byte[] key = new byte[48];
    random.nextBytes(key);
    key[0] = (byte) params.getMajorVersion();
    key[1] = (byte) params.getMinorVersion();
    return new SecretKeySpec(key, "TLS");
  };

  @Override
  protected void engineInit(AlgorithmParameterSpec params, SecureRandom random)
      throws InvalidAlgorithmParameterException
  {
    if (!(params instanceof TlsRsaPremasterSecretParameterSpec))
      throw new InvalidAlgorithmParameterException("not a TlsRsaPremasterSecretParameterSpec");
    this.params = (TlsRsaPremasterSecretParameterSpec) params;
    if (random == null)
      {
        if (this.random == null)
          this.random = new SecureRandom();
      }
    else
      this.random = random;
  }

  @Override
  protected void engineInit(int keySize, SecureRandom random)
  {
    throw new IllegalArgumentException("needs to be passed a TlsRsaPremasterSecretParameterSpec");
  }

  @Override
  protected void engineInit(SecureRandom random)
  {
    // XXX
    params = new TlsRsaPremasterSecretParameterSpec(0, 0);
    if (random != null)
      {
        this.random = random;
      }
    else
      {
        if (this.random == null)
          this.random = new SecureRandom();
      }
  }
}
