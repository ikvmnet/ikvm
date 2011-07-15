/*
  Copyright (C) 2011 Volker Berlin (i-net software)

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
package sun.font;

import java.io.File;
import java.io.FilenameFilter;

import javax.swing.plaf.FontUIResource;

import cli.System.Drawing.FontFamily;

public class SunFontManager {

    private static class TTFilter implements FilenameFilter {
        public boolean accept(File dir,String name) {
            /* all conveniently have the same suffix length */
            int offset = name.length()-4;
            if (offset <= 0) { /* must be at least A.ttf */
                return false;
            } else {
                return(name.startsWith(".ttf", offset) ||
                       name.startsWith(".TTF", offset) ||
                       name.startsWith(".ttc", offset) ||
                       name.startsWith(".TTC", offset) ||
                       name.startsWith(".otf", offset) ||
                       name.startsWith(".OTF", offset));
            }
        }
    }

    private static class T1Filter implements FilenameFilter {
        public boolean accept(File dir,String name) {
            if (noType1Font) {
                return false;
            }
            /* all conveniently have the same suffix length */
            int offset = name.length()-4;
            if (offset <= 0) { /* must be at least A.pfa */
                return false;
            } else {
                return(name.startsWith(".pfa", offset) ||
                       name.startsWith(".pfb", offset) ||
                       name.startsWith(".PFA", offset) ||
                       name.startsWith(".PFB", offset));
            }
        }
    }

    /* No need to keep consing up new instances - reuse a singleton.
     * The trade-off is that these objects don't get GC'd.
     */
    private static final FilenameFilter ttFilter = new TTFilter();
    private static final FilenameFilter t1Filter = new T1Filter();

    public static boolean noType1Font;

    /**
     * Deprecated, unsupported hack - actually invokes a bug!
     * Left in for a customer, don't remove.
     */
    private boolean usePlatformFontMetrics = false;

    /**
     * Returns the global SunFontManager instance. This is similar to
     * {@link FontManagerFactory#getInstance()} but it returns a
     * SunFontManager instance instead. This is only used in internal classes
     * where we can safely assume that a SunFontManager is to be used.
     *
     * @return the global SunFontManager instance
     */
    public static SunFontManager getInstance() {
        FontManager fm = FontManagerFactory.getInstance();
        return (SunFontManager) fm;
    }

    public FilenameFilter getTrueTypeFilter() {
        return ttFilter;
    }

    public FilenameFilter getType1Filter() {
        return t1Filter;
    }

    /*
     * Workaround for apps which are dependent on a font metrics bug
     * in JDK 1.1. This is an unsupported win32 private setting.
     * Left in for a customer - do not remove.
     */
	public boolean usePlatformFontMetrics() {
		return usePlatformFontMetrics;
	}

    /* This method doesn't check if alternates are selected in this app
     * context. Its used by the FontMetrics caching code which in such
     * a case cannot retrieve a cached metrics solely on the basis of
     * the Font.equals() method since it needs to also check if the Font2D
     * is the same.
     * We also use non-standard composites for Swing native L&F fonts on
     * Windows. In that case the policy is that the metrics reported are
     * based solely on the physical font in the first slot which is the
     * visible java.awt.Font. So in that case the metrics cache which tests
     * the Font does what we want. In the near future when we expand the GTK
     * logical font definitions we may need to revisit this if GTK reports
     * combined metrics instead. For now though this test can be simple.
     */
    public boolean maybeUsingAlternateCompositeFonts() {
       return false;
    }

	public boolean usingAlternateFontforJALocales() {
		return false;
	}

    /* Called to register fall back fonts */
	public void registerFontsInDir(String fallbackDirName) {
	}

    /**
     * Returns file name for default font, either absolute
     * or relative as needed by registerFontFile.
     */
    public synchronized String getDefaultFontFile() {
    	return null;
    }

    /**
     * Returns face name for default font, or null if
     * no face names are used for CompositeFontDescriptors
     * for this platform.
     */
    public synchronized String getDefaultFontFaceName() {
		return null;
	}

    protected FontUIResource getFontConfigFUIR(String family, int style,
            int size)
	{
    	return new FontUIResource(family, style, size);
	}
    
    /**
     * Create a new Font2D without caching. This is used from createFont
     * 
     * @param family
     *            .NET FontFamily
     * @param style
     *            the style
     * @return a Font2D
     */
    public static Font2D createFont2D( FontFamily family, int style ) {
        return new PhysicalFont( family, style );
    }  
}
