/*
  Copyright (C) 2007 Jeroen Frijters

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

package sun.security.jgss.wrapper;

// this is a compilation stub only
public abstract class NativeGSSFactory implements sun.security.jgss.spi.MechanismFactory
{
    private NativeGSSFactory() { }
    /*
    public Oid getMechanismOid()
    {
        throw new UnsupportedOperationException();
    }

    public Provider getProvider()
    {
        throw new UnsupportedOperationException();
    }

    public Oid[] getNameTypes()
    {
        throw new UnsupportedOperationException();
    }

    public GSSCredentialSpi getCredentialElement(GSSNameSpi name, int initLifetime, int acceptLifetime, int usage)
    {
        throw new UnsupportedOperationException();
    }

    public GSSNameSpi getNameElement(String nameStr, Oid nameType)
    {
        throw new UnsupportedOperationException();
    }

    public GSSNameSpi getNameElement(byte[] name, Oid nameType)
    {
        throw new UnsupportedOperationException();
    }

    public GSSContextSpi getMechanismContext(GSSNameSpi peer, GSSCredentialSpi myInitiatorCred, int lifetime)
    {
    }

    public GSSContextSpi getMechanismContext(GSSCredentialSpi myAcceptorCred)
    {
    }

    public GSSContextSpi getMechanismContext(byte[] exportedContext)
    {
    }
*/
    public abstract void setMech(org.ietf.jgss.Oid mech);
}
