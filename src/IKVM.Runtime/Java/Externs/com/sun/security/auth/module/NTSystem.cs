/*
  Copyright (C) 2007-2015 Jeroen Frijters
  Copyright (C) 2009 Volker Berlin (i-net software)

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
using System.Security.Principal;

namespace IKVM.Java.Externs.com.sun.security.auth.module
{

    static class NTSystem
    {

        public static void getCurrent(object thisObj, bool debug, ref string userName, ref string domain, ref string domainSID, ref string userSID, ref string[] groupIDs, ref string primaryGroupID)
        {
            WindowsIdentity id = WindowsIdentity.GetCurrent();
            string[] name = id.Name.Split('\\');
            userName = name[1];
            domain = name[0];
            domainSID = id.User.AccountDomainSid.Value;
            userSID = id.User.Value;
            string[] groups = new string[id.Groups.Count];
            for (int i = 0; i < groups.Length; i++)
            {
                groups[i] = id.Groups[i].Value;
            }
            groupIDs = groups;
            // HACK it turns out that Groups[0] is the primary group, but AFAIK this is not documented anywhere
            primaryGroupID = groups[0];
        }

        public static long getImpersonationToken0(object thisObj)
        {
            return WindowsIdentity.GetCurrent().Token.ToInt64();
        }

    }

}
