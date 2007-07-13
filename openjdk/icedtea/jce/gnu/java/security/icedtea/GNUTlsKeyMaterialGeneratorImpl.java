/* GNUTlsKeyMaterialGeneratorImpl.java -- 
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
import javax.crypto.spec.IvParameterSpec;
import javax.crypto.spec.SecretKeySpec;

import sun.security.internal.spec.TlsKeyMaterialParameterSpec;
import sun.security.internal.spec.TlsKeyMaterialSpec;
import sun.security.internal.spec.TlsPrfParameterSpec;

/**
 * @author Casey Marshall (csm@gnu.org)
 */
public class GNUTlsKeyMaterialGeneratorImpl extends KeyGeneratorSpi
{
  static final String PRF_LABEL = "key expansion";
  static final String PRF_EXPORT_CLIENT_LABEL = "client write key";
  static final String PRF_EXPORT_SERVER_LABEL = "server write key";
  static final String PRF_EXPORT_IV_LABEL = "IV block";
  private final KeyGenerator kg;
  private TlsKeyMaterialParameterSpec params;

  public GNUTlsKeyMaterialGeneratorImpl() throws NoSuchAlgorithmException
  {
    super();
    this.kg = KeyGenerator.getInstance("SunTlsPrf");
  }

  /* (non-Javadoc)
   * @see javax.crypto.KeyGeneratorSpi#engineGenerateKey()
   */
  @Override
  protected SecretKey engineGenerateKey()
  {
    if (params == null)
      throw new IllegalStateException("not initialized");

    byte[] seed = new byte[params.client_random.length
                           + params.server_random.length];
    System.arraycopy(params.server_random, 0, seed, 0, params.server_random.length);
    System.arraycopy(params.client_random, 0, seed, params.server_random.length,
                     params.client_random.length);
    int kmlen = (2 * params.keySize) + (2 * params.ivSize) + (2 * params.hashSize);
    TlsPrfParameterSpec prfParams = new TlsPrfParameterSpec(params.masterSecret,
                                                            PRF_LABEL, seed,
                                                            kmlen);
    try
      {
	kg.init(prfParams);
      }
    catch (InvalidAlgorithmParameterException iape)
      {
	throw new IllegalArgumentException(iape);
      }
    SecretKey keyMaterial = kg.generateKey();
    byte[] keyMBytes = keyMaterial.getEncoded();
    
    SecretKey clientMacKey = new SecretKeySpec(keyMBytes, 0,
                                               params.hashSize, "HMac");
    SecretKey serverMacKey = new SecretKeySpec(keyMBytes, params.hashSize,
                                               params.hashSize, "HMac");
    SecretKey clientWriteKey = new SecretKeySpec(keyMBytes, 2 * params.hashSize,
                                                 params.keySize,
                                                 params.algorithm);
    SecretKey serverWriteKey = new SecretKeySpec(keyMBytes,
                                                 2 * params.hashSize + params.keySize,
                                                 params.keySize,
                                                 params.algorithm);
    IvParameterSpec clientIv = new IvParameterSpec(keyMBytes,
                                                   2 * (params.keySize + params.hashSize),
                                                   params.ivSize);
    IvParameterSpec serverIv = new IvParameterSpec(keyMBytes,
                                                   2 * (params.hashSize + params.keySize) + params.ivSize,
                                                   params.ivSize);
    
    // This is set for exportable ciphers; need to transform these
    // keys a little more.
    if (params.expandedKeySize > 0)
      {
        prfParams = new TlsPrfParameterSpec(clientWriteKey,
                                            PRF_EXPORT_CLIENT_LABEL, seed,
                                            params.expandedKeySize);
	try
	  {
	    kg.init(prfParams);
	  }
	catch (InvalidAlgorithmParameterException iape)
	  {
	    throw new IllegalArgumentException(iape);
	  }
        clientWriteKey = new SecretKeySpec(kg.generateKey().getEncoded(),
                                           params.algorithm);
        prfParams = new TlsPrfParameterSpec(serverWriteKey,
                                            PRF_EXPORT_SERVER_LABEL, seed,
                                            params.expandedKeySize);
	try
	  {
	    kg.init(prfParams);
	  }
	catch (InvalidAlgorithmParameterException iape)
	  {
	    throw new IllegalArgumentException(iape);
	  }
        serverWriteKey = new SecretKeySpec(kg.generateKey().getEncoded(),
                                           params.algorithm);
        prfParams = new TlsPrfParameterSpec(new SecretKeySpec(new byte[0], ""),
                                            PRF_EXPORT_IV_LABEL, seed,
                                            2 *params.ivSize);
	try
	  {
	    kg.init(prfParams);
	  }
	catch (InvalidAlgorithmParameterException iape)
	  {
	    throw new IllegalArgumentException(iape);
	  }
        byte[] newIv = kg.generateKey().getEncoded();
        clientIv = new IvParameterSpec(newIv, 0, params.ivSize);
        serverIv = new IvParameterSpec(newIv, params.ivSize, params.ivSize);
      }
    
    return new TlsKeyMaterialSpec(clientWriteKey, serverWriteKey,
                                  clientIv, serverIv, clientMacKey,
                                  serverMacKey);
  }

  /* (non-Javadoc)
   * @see javax.crypto.KeyGeneratorSpi#engineInit(java.security.spec.AlgorithmParameterSpec, java.security.SecureRandom)
   */
  @Override
  protected void engineInit(final AlgorithmParameterSpec params, SecureRandom random)
      throws InvalidAlgorithmParameterException
  {
    this.params = null;
    if (!(params instanceof TlsKeyMaterialParameterSpec))
      throw new InvalidAlgorithmParameterException("not a TlsKeyMaterialParameterSpec");
    this.params = (TlsKeyMaterialParameterSpec) params;
  }

  /* (non-Javadoc)
   * @see javax.crypto.KeyGeneratorSpi#engineInit(int, java.security.SecureRandom)
   */
  @Override
  protected void engineInit(int keySize, SecureRandom random)
  {
    throw new IllegalArgumentException("need a TlsKeyMaterialParameterSpec");
  }

  /* (non-Javadoc)
   * @see javax.crypto.KeyGeneratorSpi#engineInit(java.security.SecureRandom)
   */
  @Override
  protected void engineInit(SecureRandom random)
  {
  }

}
