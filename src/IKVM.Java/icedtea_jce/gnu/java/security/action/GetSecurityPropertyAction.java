/* GetSecurityPropertyAction.java
   Copyright (C) 2004 Free Software Foundation, Inc.

This file is part of GNU Classpath.

GNU Classpath is free software; you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation; either version 2, or (at your option)
any later version.

GNU Classpath is distributed in the hope that it will be useful, but
WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
General Public License for more details.

You should have received a copy of the GNU General Public License
along with GNU Classpath; see the file COPYING.  If not, write to the
Free Software Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA
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

package gnu.java.security.action;

import java.security.PrivilegedAction;
import java.security.Security;

/**
 * PrivilegedAction implementation that calls Security.getProperty()
 * with the property name passed to its constructor.
 *
 * Example of use:
 * <code>
 * GetSecurityPropertyAction action = new GetSecurityPropertyAction("javax.net.ssl.trustStorePassword");
 * String passwd = AccessController.doPrivileged(action);
 * </code>
 */
public class GetSecurityPropertyAction implements PrivilegedAction<String>
{
  private String name;
  private String value;

  public GetSecurityPropertyAction()
  {
  }

  public GetSecurityPropertyAction(String propName)
  {
    setParameters(propName);
  }

  public GetSecurityPropertyAction(String propName, String defaultValue)
  {
    setParameters(propName, defaultValue);
  }

  public GetSecurityPropertyAction setParameters(String propName)
  {
    this.name = propName;
    this.value = null;
    return this;
  }

  public GetSecurityPropertyAction setParameters(String propName, String defaultValue)
  {
    this.name = propName;
    this.value = defaultValue;
    return this;
  }

  public String run()
  {
    String val = Security.getProperty(name);
    if (val == null)
      val = value;
    return val;
  }
}
