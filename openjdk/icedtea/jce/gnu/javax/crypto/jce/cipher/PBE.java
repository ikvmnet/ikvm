/* PBE.java -- 
 Copyright (C) 2007  Free Software Foundation, Inc.

 This file is a part of GNU Classpath.

 GNU Classpath is free software; you can redistribute it and/or modify
 it under the terms of the GNU General Public License as published by
 the Free Software Foundation; either version 2 of the License, or (at
 your option) any later version.

 GNU Classpath is distributed in the hope that it will be useful, but
 WITHOUT ANY WARRANTY; without even the implied warranty of
 MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
 General Public License for more details.

 You should have received a copy of the GNU General Public License
 along with GNU Classpath; if not, write to the Free Software
 Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA 02110-1301
 USA

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


package gnu.javax.crypto.jce.cipher;

import gnu.java.security.Registry;
import gnu.javax.crypto.jce.spec.BlockCipherParameterSpec;
import gnu.javax.crypto.key.GnuPBEKey;
import gnu.javax.crypto.mode.BaseMode;

import java.io.UnsupportedEncodingException;
import java.security.AlgorithmParameters;
import java.security.InvalidAlgorithmParameterException;
import java.security.InvalidKeyException;
import java.security.Key;
import java.security.MessageDigest;
import java.security.NoSuchAlgorithmException;
import java.security.SecureRandom;
import java.security.spec.AlgorithmParameterSpec;
import java.util.Arrays;

import javax.crypto.NoSuchPaddingException;
import javax.crypto.spec.PBEParameterSpec;
import javax.crypto.spec.SecretKeySpec;

/**
 */
public abstract class PBE
    extends CipherAdapter
{
  MessageDigest hash;

  protected PBE(String cipherName, String hashName)
  {
    super(cipherName);
    try
      {
        this.hash = MessageDigest.getInstance(hashName);
      }
    catch (NoSuchAlgorithmException ignored)
      {
      }
  }

  protected void engineInit(int opmode, Key key, SecureRandom random)
      throws InvalidKeyException
  {
    if (! (key instanceof GnuPBEKey))
      throw new InvalidKeyException("not a GNU PBE key");
    GnuPBEKey k = (GnuPBEKey) key;
    int c1 = k.getIterationCount();
    byte[] s1 = k.getSalt();
    PBEPKCS5_V1Params pkcs5 = genParams(c1, s1, k.getPassword());
    initInternal(opmode, pkcs5.skSpec, pkcs5.ivSpec, random);
  }

  protected void engineInit(int opmode, Key key, AlgorithmParameterSpec params,
                            SecureRandom random)
      throws InvalidKeyException, InvalidAlgorithmParameterException
  {
    if (! (key instanceof GnuPBEKey))
      throw new InvalidKeyException("not a GNU PBE key");
    GnuPBEKey k = (GnuPBEKey) key;
    int c1 = k.getIterationCount();
    byte[] s1 = k.getSalt();
    if (params != null)
      {
        if (! (params instanceof PBEParameterSpec))
          throw new InvalidAlgorithmParameterException(
              "The algorithm-parameter-spec, when not null,  MUST be of type "
              + "PBEParameterSpec or one of its subclasses");
        PBEParameterSpec ps = (PBEParameterSpec) params;
        // it must share the same salt and iteration count as the secret key
        int c2 = ps.getIterationCount();
	if (c1 == 0)
          c1 = c2;
        else if (c1 != c2)
          throw new InvalidAlgorithmParameterException(
              "The algorithm-parameter-spec and the key MUST share the same "
              + "iteration count");
        byte[] s2 = ps.getSalt();
	// salt may be unspecified
	if (s1 == null)
	  s1 = s2;
        else if ((s1 != null && s1.length > 0 && s2 == null)
            || (s1 == null && s2 != null && s2.length > 0)
            || (s1 != null && s2 != null && !Arrays.equals(s1, s2)))
          throw new InvalidAlgorithmParameterException(
              "The algorithm-parameter-spec and the key MUST share the same salt");
      }
    PBEPKCS5_V1Params pkcs5 = genParams(c1, s1, k.getPassword());
    initInternal(opmode, pkcs5.skSpec, pkcs5.ivSpec, random);
  }

  protected void engineInit(int opmode, Key key, AlgorithmParameters params,
                            SecureRandom random) throws InvalidKeyException,
      InvalidAlgorithmParameterException
  {
    if (! (key instanceof GnuPBEKey))
      throw new InvalidKeyException("not a GNU PBE key");
    GnuPBEKey k = (GnuPBEKey) key;
    int c1 = k.getIterationCount();
    byte[] s1 = k.getSalt();
    if (params != null)
      throw new InvalidAlgorithmParameterException(
          "We [MUST] supply our own algorithm-parameter");
    PBEPKCS5_V1Params pkcs5 = genParams(c1, s1, k.getPassword());
    initInternal(opmode, pkcs5.skSpec, pkcs5.ivSpec, random);
  }

  private void initInternal(int opmode, SecretKeySpec key,
		                    BlockCipherParameterSpec params,
		                    SecureRandom random) throws InvalidKeyException
  {
    try {
		super.engineInit(opmode, key, params, random);
	} catch (InvalidAlgorithmParameterException x) {
		// this should not happen since 'params' is generated by us with
		// the genParams() method.  if it does re-throw as IKE
		throw new InvalidKeyException(x);
	}
  }

  private PBEPKCS5_V1Params genParams(int c, byte[] s, char[] password)
  throws InvalidKeyException
  {
    // transform the password's chars into bytes assuming UTF-8
    byte[] p;
    try {
		p = new String(password).getBytes("UTF-8");
	} catch (UnsupportedEncodingException x) {
		throw new InvalidKeyException(x);
	}
    
    String name = cipher.name();
    int blockSize = cipher.defaultBlockSize();
    int keySize = cipher.defaultKeySize();
    int hashSize = this.hash.getDigestLength();
    Integer att_ivSize = (Integer) attributes.get(mode.MODE_BLOCK_SIZE);
    int ivSize = (att_ivSize == null ? blockSize : att_ivSize.intValue());
    
    // digest once
    this.hash.update(p);
    byte[] buffer = s == null ? this.hash.digest() : this.hash.digest(s);

    // and now complete the remaining iterations
    for (int i = 1; i < c; i++)
    	buffer = this.hash.digest(buffer);

    PBEPKCS5_V1Params result = new PBEPKCS5_V1Params();
    result.skSpec = new SecretKeySpec(buffer, 0, blockSize, name.substring(0, name.indexOf('-')));
    byte[] iv = new byte[ivSize];
    System.arraycopy(buffer, blockSize, iv, 0, hashSize - blockSize);
    result.ivSpec = new BlockCipherParameterSpec(iv, blockSize, keySize);
    return result;
  }

  static class PBEPKCS5_V1Params {
	  SecretKeySpec skSpec;
	  BlockCipherParameterSpec ivSpec;
  }

  public static class MD5
      extends PBE
  {
    public MD5(String cipher)
    {
      super(cipher, "MD5");
    }

    public static class DES
        extends MD5
    {
      public DES()
      {
    	// we really need a DES/CBC/PKCS5 combined padded block cipher
        super(Registry.DES_CIPHER);
        
	    // the superclass's field 'cipher' has a plain DES-ECB so we need to        
	    // change its mode and padding
        try {
			this.engineSetMode(Registry.CBC_MODE);
		} catch (NoSuchAlgorithmException ignored) {
			ignored.printStackTrace(System.err);
		}
        try {
			this.engineSetPadding(Registry.PKCS5_PAD);
		} catch (NoSuchPaddingException ignored) {
			ignored.printStackTrace(System.err);
		}
      }
    }
  }
}
