/* TlsKeyMaterialSpec.java -- TLS session keys.
   Copyright (C) 2007 Red Hat, Inc.
   Copyright (C) 2007  Casey Marshall <csm@gnu.org>

This file is part of IcedTea.

IcedTea is free software; you can redistribute it and/or 
modify it under the terms of the GNU General Public License as published by
the Free Software Foundation, version 2.

IcedTea is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
General Public License for more details.

You should have received a copy of the GNU General Public License
along with IcedTea; see the file COPYING.  If not, write to
the Free Software Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA
02110-1301 USA.

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


package sun.security.internal.spec;

import java.security.spec.KeySpec;

import javax.crypto.SecretKey;
import javax.crypto.spec.IvParameterSpec;

public class TlsKeyMaterialSpec implements KeySpec, SecretKey
{
  private static final long serialVersionUID = 0L;

  private final SecretKey clientCipherKey;
  private final SecretKey serverCipherKey;
  private final IvParameterSpec clientIv;
  private final IvParameterSpec serverIv;
  private final SecretKey clientMacKey;
  private final SecretKey serverMacKey;
  
  public TlsKeyMaterialSpec(SecretKey clientCipherKey,
                            SecretKey serverCipherKey,
                            IvParameterSpec clientIv,
                            IvParameterSpec serverIv,
                            SecretKey clientMacKey,
                            SecretKey serverMacKey)
  {
    super();
    this.clientCipherKey = clientCipherKey;
    this.serverCipherKey = serverCipherKey;
    this.clientIv = clientIv;
    this.serverIv = serverIv;
    this.clientMacKey = clientMacKey;
    this.serverMacKey = serverMacKey;
  }

  public SecretKey getClientCipherKey()
  {
    return clientCipherKey;
  }
  
  public SecretKey getServerCipherKey()
  {
    return serverCipherKey;
  }
  
  public IvParameterSpec getClientIv()
  {
    return clientIv;
  }
  
  public IvParameterSpec getServerIv()
  {
    return serverIv;
  }
  
  public SecretKey getClientMacKey()
  {
    return clientMacKey;
  }
  
  public SecretKey getServerMacKey()
  {
    return serverMacKey;
  }

  public String getAlgorithm()
  {
    return "TLS";
  }

  public byte[] getEncoded()
  {
    // TODO Auto-generated method stub
    return null;
  }

  public String getFormat()
  {
    return "RAW";
  }
}
