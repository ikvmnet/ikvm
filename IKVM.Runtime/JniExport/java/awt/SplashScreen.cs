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
using System;


namespace IKVM.Runtime.JniExport.java.awt
{

    static class SplashScreen
    {
        public static void _update(long splashPtr, int[] data, int x, int y, int width, int height, int scanlineStride) { }
        public static bool _isVisible(long splashPtr) { return false; }
        public static object _getBounds(long splashPtr) { return null; }
        public static long _getInstance() { return 0; }
        public static void _close(long splashPtr) { }
        public static String _getImageFileName(long splashPtr) { return null; }
        public static String _getImageJarName(long splashPtr) { return null; }
        public static bool _setImageData(long splashPtr, byte[] data) { return false; }
        public static float _getScaleFactor(long SplashPtr) { return 1; }

    }

}
