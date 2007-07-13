/* GNUTlsMasterSecretGenerator.java -- 
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
exception statement from your version. */


package gnu.java.security.icedtea;

import java.security.InvalidAlgorithmParameterException;
import java.security.NoSuchAlgorithmException;
import java.security.SecureRandom;
import java.security.spec.AlgorithmParameterSpec;

import javax.crypto.KeyGenerator;
import javax.crypto.KeyGeneratorSpi;
import javax.crypto.SecretKey;
import javax.crypto.spec.SecretKeySpec;

import sun.security.internal.spec.TlsMasterSecretParameterSpec;
import sun.security.internal.spec.TlsPrfParameterSpec;

/**
 * @author Casey Marshall (csm@gnu.org)
 */
public class GNUTlsMasterSecretGenerator extends KeyGeneratorSpi
{
  static final String PRF_LABEL = "master secret";
  static final int MASTER_SECRET_LEN = 48;
  private TlsMasterSecretParameterSpec params;
  private final KeyGenerator kg;

  public GNUTlsMasterSecretGenerator() throws NoSuchAlgorithmException
  {
    kg = KeyGenerator.getInstance("SunTlsPrf");
  }
  
  /* (non-Javadoc)
   * @see javax.crypto.KeyGeneratorSpi#engineGenerateKey()
   */
  @Override
  protected SecretKey engineGenerateKey()
  {
    if (kg == null)
      throw new IllegalStateException("not initialized");
    
    SecretKey sk = kg.generateKey();
    return new SecretKeySpec(sk.getEncoded(), "TLS");
  }

  /* (non-Javadoc)
   * @see javax.crypto.KeyGeneratorSpi#engineInit(java.security.spec.AlgorithmParameterSpec, java.security.SecureRandom)
   */
  @Override
  protected void engineInit(AlgorithmParameterSpec params, SecureRandom random)
      throws InvalidAlgorithmParameterException
  {
    this.params = null;
    if (!(params instanceof TlsMasterSecretParameterSpec))
      throw new InvalidAlgorithmParameterException("expecting a TlsMasterSecretParameterSpec");
    this.params = (TlsMasterSecretParameterSpec) params;
    byte[] seed = new byte[this.params.client_random.length
                           + this.params.server_random.length];
    System.arraycopy(this.params.client_random, 0, seed, 0,
                     this.params.client_random.length);
    System.arraycopy(this.params.server_random, 0, seed,
                     this.params.client_random.length,
                     this.params.server_random.length);
    TlsPrfParameterSpec prfSpec = new TlsPrfParameterSpec(this.params.key,
                                                          PRF_LABEL, seed,
                                                          MASTER_SECRET_LEN);
    kg.init(prfSpec);
  }

  /* (non-Javadoc)
   * @see javax.crypto.KeyGeneratorSpi#engineInit(int, java.security.SecureRandom)
   */
  @Override
  protected void engineInit(int keySize, SecureRandom random)
  {
    // TODO Auto-generated method stub

  }

  /* (non-Javadoc)
   * @see javax.crypto.KeyGeneratorSpi#engineInit(java.security.SecureRandom)
   */
  @Override
  protected void engineInit(SecureRandom random)
  {
  }
}
