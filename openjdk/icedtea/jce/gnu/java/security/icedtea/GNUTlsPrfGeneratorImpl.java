/* GNUTlsPrfGeneratorImpl.java -- TLS PRF.
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

import java.io.UnsupportedEncodingException;
import java.security.InvalidAlgorithmParameterException;
import java.security.InvalidKeyException;
import java.security.NoSuchAlgorithmException;
import java.security.SecureRandom;
import java.security.spec.AlgorithmParameterSpec;

import javax.crypto.KeyGeneratorSpi;
import javax.crypto.Mac;
import javax.crypto.SecretKey;
import javax.crypto.spec.SecretKeySpec;

import sun.security.internal.spec.TlsPrfParameterSpec;

/**
 * @author csm
 *
 */
public class GNUTlsPrfGeneratorImpl extends KeyGeneratorSpi
{
  private TlsPrfParameterSpec params;
  private final Mac hmac_md5;
  private byte[] md5_A;
  private final Mac hmac_sha;
  private byte[] sha_A;
  private byte[] labelBytes;

  public GNUTlsPrfGeneratorImpl() throws NoSuchAlgorithmException
  {
    hmac_md5 = Mac.getInstance("HmacMD5");
    hmac_sha = Mac.getInstance("HMacSHA1");
  }
  
  /* (non-Javadoc)
   * @see javax.crypto.KeyGeneratorSpi#engineGenerateKey()
   */
  @Override
  protected SecretKey engineGenerateKey()
  {
    if (params == null)
      throw new IllegalStateException("not initialized");
    
    final byte[] buf = new byte[params.size];

    for (int i = 0; i < buf.length; i += hmac_sha.getMacLength())
      {
        hmac_sha.update(sha_A);
        hmac_sha.update(labelBytes);
        hmac_sha.update(params.seed);
        byte[] x = hmac_sha.doFinal();
        hmac_sha.reset();
        System.arraycopy(x, 0, buf, i,
                         Math.min(x.length, buf.length - i));
        hmac_sha.update(sha_A);
        sha_A = hmac_sha.doFinal();
        hmac_sha.reset();
      }

    for (int i = 0; i < buf.length; i += hmac_md5.getMacLength())
      {
        hmac_md5.update(md5_A);
        hmac_md5.update(labelBytes);
        hmac_md5.update(params.seed);
        byte[] x = hmac_md5.doFinal();
        hmac_md5.reset();
        for (int j = 0; j < x.length && i + j < buf.length; j++)
          buf[i+j] ^= x[j];
        hmac_md5.update(md5_A);
        md5_A = hmac_md5.doFinal();
        hmac_md5.reset();
      }
    return new SecretKeySpec(buf, "TLS");
  }

  /* (non-Javadoc)
   * @see javax.crypto.KeyGeneratorSpi#engineInit(java.security.spec.AlgorithmParameterSpec, java.security.SecureRandom)
   */
  @Override
  protected void engineInit(AlgorithmParameterSpec params, SecureRandom random)
      throws InvalidAlgorithmParameterException
  {
    if (!(params instanceof TlsPrfParameterSpec))
      throw new InvalidAlgorithmParameterException("expecting TlsPrfParameterSpec");
    this.params = (TlsPrfParameterSpec) params;

    byte[] keyb = this.params.key.getEncoded();
    int l = (keyb.length >>> 1) + (keyb.length & 1);
    try
      {
        hmac_md5.init(new SecretKeySpec(keyb, 0, l, "HMacMD5"));
      }
    catch (InvalidKeyException ike)
      {
        throw new InvalidAlgorithmParameterException(ike);
      }
    try
      {
        labelBytes = this.params.label.getBytes("ASCII");
      }
    catch (UnsupportedEncodingException uee)
      {
        throw new InvalidAlgorithmParameterException(uee);
      }
    hmac_md5.update(labelBytes);
    hmac_md5.update(this.params.seed);
    md5_A = hmac_md5.doFinal();
    hmac_md5.reset();

    try
      {
        hmac_sha.init(new SecretKeySpec(keyb, keyb.length - l, l, "HMacSHA1"));
      }
    catch (InvalidKeyException ike)
      {
        throw new InvalidAlgorithmParameterException(ike);
      }
    hmac_sha.update(labelBytes);
    hmac_sha.update(this.params.seed);
    sha_A = hmac_sha.doFinal();
    hmac_sha.reset();

    // SecureRandom is ignored.
  }

  /* (non-Javadoc)
   * @see javax.crypto.KeyGeneratorSpi#engineInit(int, java.security.SecureRandom)
   */
  @Override
  protected void engineInit(int keySize, SecureRandom random)
  {
    throw new IllegalArgumentException("need TlsPrfParameterSpec argument");
  }

  /* (non-Javadoc)
   * @see javax.crypto.KeyGeneratorSpi#engineInit(java.security.SecureRandom)
   */
  @Override
  protected void engineInit(SecureRandom random)
  {
  }
}
