/*
  Copyright (C) 2007 Volker Berlin

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
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using java.awt.peer;
using java.awt.@event;

namespace ikvm.awt
{

    internal class WindowsRobot : RobotPeer
    {
        internal WindowsRobot(java.awt.GraphicsDevice device)
        {
        }

		public void dispose()
		{
		}

        public int getRGBPixel(int x, int y)
        {
            Bitmap bitmap = new Bitmap(1, 1);
            Graphics g = Graphics.FromImage(bitmap);
            g.CopyFromScreen( x, y, 0, 0, new Size(1,1));
            g.Dispose();
            Color color = bitmap.GetPixel(0,0);
            bitmap.Dispose();
            return color.ToArgb();
        }

        public int[] getRGBPixels(java.awt.Rectangle r)
        {
            int width = r.width;
            int height = r.height;
            Bitmap bitmap = new Bitmap(width, height);
            Graphics g = Graphics.FromImage(bitmap);
            g.CopyFromScreen(r.x, r.y, 0, 0, new Size(width, height));
            g.Dispose();
            int[] pixels = new int[width * height];
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    pixels[i+j*width] = bitmap.GetPixel(i, j).ToArgb();
                }
            }
            bitmap.Dispose();
            return pixels;
        }

        private byte MapKeyCode(int keyCode)
        {
            //TODO there need a keymap for some special chars
            //http://msdn.microsoft.com/library/default.asp?url=/library/en-us/winui/WinUI/WindowsUserInterface/UserInput/VirtualKeyCodes.asp
            switch (keyCode)
            {
                case KeyEvent.VK_DELETE:
                    return VK_DELETE;
                default:
                    return (byte)keyCode;
            }
        }

        public void keyPress(int keycode)
        {
            keybd_event(MapKeyCode(keycode), 0, KEYEVENTF_KEYDOWN, IntPtr.Zero);
        }

        public void keyRelease(int keycode)
        {
            keybd_event(MapKeyCode(keycode), 0, KEYEVENTF_KEYUP, IntPtr.Zero);
        }

        public void mouseMove(int x, int y)
        {
            Cursor.Position = new Point(x,y);
        }

        public void mousePress(int button)
        {
            int dwFlags = 0;
            switch (button)
            {
                case InputEvent.BUTTON1_MASK:
                    dwFlags |= MOUSEEVENTF_LEFTDOWN;
                    break;
                case InputEvent.BUTTON2_MASK:
                    dwFlags |= MOUSEEVENTF_MIDDLEDOWN;
                    break;
                case InputEvent.BUTTON3_MASK:
                    dwFlags |= MOUSEEVENTF_RIGHTDOWN;
                    break;
            }
            mouse_event(dwFlags, 0, 0, 0, IntPtr.Zero);
        }

        public void mouseRelease(int button)
        {
            int dwFlags = 0;
            switch (button)
            {
                case InputEvent.BUTTON1_MASK:
                    dwFlags |= MOUSEEVENTF_LEFTUP;
                    break;
                case InputEvent.BUTTON2_MASK:
                    dwFlags |= MOUSEEVENTF_MIDDLEUP;
                    break;
                case InputEvent.BUTTON3_MASK:
                    dwFlags |= MOUSEEVENTF_RIGHTUP;
                    break;
            }
            mouse_event(dwFlags, 0, 0, 0, IntPtr.Zero);
        }

        public void mouseWheel(int wheel)
        {
            mouse_event(0, 0, 0, wheel, IntPtr.Zero);
        }

        [DllImport("user32.dll")]
        private static extern void keybd_event(byte vk, byte scan, int flags, IntPtr extrainfo);

        private const int KEYEVENTF_KEYDOWN = 0x0000;
        private const int KEYEVENTF_KEYUP = 0x0002;

        [DllImport("user32.dll")]
        private static extern void mouse_event(
            int dwFlags, // motion and click options
            int dx, // horizontal position or change
            int dy, // vertical position or change
            int dwData, // wheel movement
            IntPtr dwExtraInfo // application-defined information
        );

        private const int MOUSEEVENTF_LEFTDOWN = 0x0002;
        private const int MOUSEEVENTF_LEFTUP = 0x0004;
        private const int MOUSEEVENTF_RIGHTDOWN = 0x0008;
        private const int MOUSEEVENTF_RIGHTUP = 0x0010;
        private const int MOUSEEVENTF_MIDDLEDOWN = 0x0020;
        private const int MOUSEEVENTF_MIDDLEUP = 0x0040;



        private const int VK_BACK          = 0x08;
        private const int VK_TAB           = 0x09;

/*
 * 0x0A - 0x0B : reserved
 */

        private const int VK_CLEAR         = 0x0C;
        private const int VK_RETURN        = 0x0D;

        private const int VK_SHIFT         = 0x10;
        private const int VK_CONTROL       = 0x11;
        private const int VK_MENU          = 0x12;
        private const int VK_PAUSE         = 0x13;
        private const int VK_CAPITAL       = 0x14;

        private const int VK_KANA          = 0x15;
        private const int VK_HANGEUL       = 0x15;  /* old name - should be here for compatibility */
        private const int VK_HANGUL        = 0x15;
        private const int VK_JUNJA         = 0x17;
        private const int VK_FINAL         = 0x18;
        private const int VK_HANJA         = 0x19;
        private const int VK_KANJI         = 0x19;

        private const int VK_ESCAPE        = 0x1B;

        private const int VK_CONVERT       = 0x1C;
        private const int VK_NONCONVERT    = 0x1D;
        private const int VK_ACCEPT        = 0x1E;
        private const int VK_MODECHANGE    = 0x1F;

        private const int VK_SPACE         = 0x20;
        private const int VK_PRIOR         = 0x21;
        private const int VK_NEXT          = 0x22;
        private const int VK_END           = 0x23;
        private const int VK_HOME          = 0x24;
        private const int VK_LEFT          = 0x25;
        private const int VK_UP            = 0x26;
        private const int VK_RIGHT         = 0x27;
        private const int VK_DOWN          = 0x28;
        private const int VK_SELECT        = 0x29;
        private const int VK_PRINT         = 0x2A;
        private const int VK_EXECUTE       = 0x2B;
        private const int VK_SNAPSHOT      = 0x2C;
        private const int VK_INSERT        = 0x2D;
        private const int VK_DELETE        = 0x2E;
        private const int VK_HELP          = 0x2F;

/*
 * VK_0 - VK_9 are the same as ASCII '0' - '9' (0x30 - 0x39)
 * 0x40 : unassigned
 * VK_A - VK_Z are the same as ASCII 'A' - 'Z' (0x41 - 0x5A)
 */

        private const int VK_LWIN          = 0x5B;
        private const int VK_RWIN          = 0x5C;
        private const int VK_APPS          = 0x5D;

/*
 * 0x5E : reserved
 */

        private const int VK_SLEEP         = 0x5F;

        private const int VK_NUMPAD0       = 0x60;
        private const int VK_NUMPAD1       = 0x61;
        private const int VK_NUMPAD2       = 0x62;
        private const int VK_NUMPAD3       = 0x63;
        private const int VK_NUMPAD4       = 0x64;
        private const int VK_NUMPAD5       = 0x65;
        private const int VK_NUMPAD6       = 0x66;
        private const int VK_NUMPAD7       = 0x67;
        private const int VK_NUMPAD8       = 0x68;
        private const int VK_NUMPAD9       = 0x69;
        private const int VK_MULTIPLY      = 0x6A;
        private const int VK_ADD           = 0x6B;
        private const int VK_SEPARATOR     = 0x6C;
        private const int VK_SUBTRACT      = 0x6D;
        private const int VK_DECIMAL       = 0x6E;
        private const int VK_DIVIDE        = 0x6F;
        private const int VK_F1            = 0x70;
        private const int VK_F2            = 0x71;
        private const int VK_F3            = 0x72;
        private const int VK_F4            = 0x73;
        private const int VK_F5            = 0x74;
        private const int VK_F6            = 0x75;
        private const int VK_F7            = 0x76;
        private const int VK_F8            = 0x77;
        private const int VK_F9            = 0x78;
        private const int VK_F10           = 0x79;
        private const int VK_F11           = 0x7A;
        private const int VK_F12           = 0x7B;
        private const int VK_F13           = 0x7C;
        private const int VK_F14           = 0x7D;
        private const int VK_F15           = 0x7E;
        private const int VK_F16           = 0x7F;
        private const int VK_F17           = 0x80;
        private const int VK_F18           = 0x81;
        private const int VK_F19           = 0x82;
        private const int VK_F20           = 0x83;
        private const int VK_F21           = 0x84;
        private const int VK_F22           = 0x85;
        private const int VK_F23           = 0x86;
        private const int VK_F24           = 0x87;

/*
 * 0x88 - 0x8F : unassigned
 */

        private const int VK_NUMLOCK       = 0x90;
        private const int VK_SCROLL        = 0x91;

/*
 * NEC PC-9800 kbd definitions
 */
        private const int VK_OEM_NEC_EQUAL = 0x92;   // '=' key on numpad

/*
 * Fujitsu/OASYS kbd definitions
 */
        private const int VK_OEM_FJ_JISHO  = 0x92;   // 'Dictionary' key
        private const int VK_OEM_FJ_MASSHOU= 0x93;   // 'Unregister word' key
        private const int VK_OEM_FJ_TOUROKU= 0x94;   // 'Register word' key
        private const int VK_OEM_FJ_LOYA   = 0x95;   // 'Left OYAYUBI' key
        private const int VK_OEM_FJ_ROYA   = 0x96;   // 'Right OYAYUBI' key

/*
 * 0x97 - 0x9F : unassigned
 */

/*
 * VK_L* & VK_R* - left and right Alt, Ctrl and Shift virtual keys.
 * Used only as parameters to GetAsyncKeyState() and GetKeyState().
 * No other API or message will distinguish left and right keys in this way.
 */
        private const int VK_LSHIFT        = 0xA0;
        private const int VK_RSHIFT        = 0xA1;
        private const int VK_LCONTROL      = 0xA2;
        private const int VK_RCONTROL      = 0xA3;
        private const int VK_LMENU         = 0xA4;
        private const int VK_RMENU         = 0xA5;

        private const int VK_BROWSER_BACK       = 0xA6;
        private const int VK_BROWSER_FORWARD    = 0xA7;
        private const int VK_BROWSER_REFRESH    = 0xA8;
        private const int VK_BROWSER_STOP       = 0xA9;
        private const int VK_BROWSER_SEARCH     = 0xAA;
        private const int VK_BROWSER_FAVORITES  = 0xAB;
        private const int VK_BROWSER_HOME       = 0xAC;

        private const int VK_VOLUME_MUTE        = 0xAD;
        private const int VK_VOLUME_DOWN        = 0xAE;
        private const int VK_VOLUME_UP          = 0xAF;
        private const int VK_MEDIA_NEXT_TRACK   = 0xB0;
        private const int VK_MEDIA_PREV_TRACK   = 0xB1;
        private const int VK_MEDIA_STOP         = 0xB2;
        private const int VK_MEDIA_PLAY_PAUSE   = 0xB3;
        private const int VK_LAUNCH_MAIL        = 0xB4;
        private const int VK_LAUNCH_MEDIA_SELECT= 0xB5;
        private const int VK_LAUNCH_APP1        = 0xB6;
        private const int VK_LAUNCH_APP2        = 0xB7;


/*
 * 0xB8 - 0xB9 : reserved
 */

        private const int VK_OEM_1         = 0xBA;   // ';:' for US
        private const int VK_OEM_PLUS      = 0xBB;   // '+' any country
        private const int VK_OEM_COMMA     = 0xBC;   // ',' any country
        private const int VK_OEM_MINUS     = 0xBD;   // '-' any country
        private const int VK_OEM_PERIOD    = 0xBE;   // '.' any country
        private const int VK_OEM_2         = 0xBF;   // '/?' for US
        private const int VK_OEM_3         = 0xC0;   // '`~' for US

/*
 * 0xC1 - 0xD7 : reserved
 */

/*
 * 0xD8 - 0xDA : unassigned
 */

        private const int VK_OEM_4         = 0xDB;  //  '[{' for US
        private const int VK_OEM_5         = 0xDC;  //  '\|' for US
        private const int VK_OEM_6         = 0xDD;  //  ']}' for US
        private const int VK_OEM_7         = 0xDE;  //  ''"' for US
        private const int VK_OEM_8         = 0xDF;

/*
 * 0xE0 : reserved
 */

/*
 * Various extended or enhanced keyboards
 */
        private const int VK_OEM_AX        = 0xE1;  //  'AX' key on Japanese AX kbd
        private const int VK_OEM_102       = 0xE2;  //  "<>" or "\|" on RT 102-key kbd.
        private const int VK_ICO_HELP      = 0xE3;  //  Help key on ICO
        private const int VK_ICO_00        = 0xE4;  //  00 key on ICO


/*
 * 0xE8 : unassigned
 */

/*
 * Nokia/Ericsson definitions
 */
        private const int VK_OEM_RESET     = 0xE9;
        private const int VK_OEM_JUMP      = 0xEA;
        private const int VK_OEM_PA1       = 0xEB;
        private const int VK_OEM_PA2       = 0xEC;
        private const int VK_OEM_PA3       = 0xED;
        private const int VK_OEM_WSCTRL    = 0xEE;
        private const int VK_OEM_CUSEL     = 0xEF;
        private const int VK_OEM_ATTN      = 0xF0;
        private const int VK_OEM_FINISH    = 0xF1;
        private const int VK_OEM_COPY      = 0xF2;
        private const int VK_OEM_AUTO      = 0xF3;
        private const int VK_OEM_ENLW      = 0xF4;
        private const int VK_OEM_BACKTAB   = 0xF5;

        private const int VK_ATTN          = 0xF6;
        private const int VK_CRSEL         = 0xF7;
        private const int VK_EXSEL         = 0xF8;
        private const int VK_EREOF         = 0xF9;
        private const int VK_PLAY          = 0xFA;
        private const int VK_ZOOM          = 0xFB;
        private const int VK_NONAME        = 0xFC;
        private const int VK_PA1           = 0xFD;
        private const int VK_OEM_CLEAR     = 0xFE;


    }
}