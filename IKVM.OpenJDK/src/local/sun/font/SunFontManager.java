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

import ikvm.internal.NotYetImplementedError;

import java.awt.Font;
import java.awt.FontFormatException;
import java.io.BufferedReader;
import java.io.File;
import java.io.FileInputStream;
import java.io.FilenameFilter;
import java.io.IOException;
import java.io.InputStreamReader;
import java.security.AccessController;
import java.security.PrivilegedAction;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.HashSet;
import java.util.Hashtable;
import java.util.Iterator;
import java.util.Locale;
import java.util.Map;
import java.util.NoSuchElementException;
import java.util.StringTokenizer;
import java.util.TreeMap;
import java.util.Vector;
import java.util.concurrent.ConcurrentHashMap;

import javax.swing.plaf.FontUIResource;
import sun.awt.AppContext;
import sun.awt.FontConfiguration;
import sun.awt.SunToolkit;
import sun.java2d.FontSupport;
import sun.util.logging.PlatformLogger;

/**
 * The base implementation of the {@link FontManager} interface. It implements
 * the platform independent, shared parts of OpenJDK's FontManager
 * implementations. The platform specific parts are declared as abstract
 * methods that have to be implemented by specific implementations.
 */
public class SunFontManager implements FontManager {

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

    @Override
    public boolean usingPerAppContextComposites() {
        return _usingPerAppContextComposites;
    }

    public Font2DHandle getNewComposite(String family, int style,
            Font2DHandle handle) {

        if (!(handle.font2D instanceof CompositeFont)) {
            return handle;
        }

        CompositeFont oldComp = (CompositeFont)handle.font2D;
        PhysicalFont oldFont = oldComp.getSlotFont(0);

        if (family == null) {
            family = oldFont.getFamilyName(null);
        }
        if (style == -1) {
            style = oldComp.getStyle();
        }

        Font2D newFont = findFont2D(family, style, NO_FALLBACK);
        if (!(newFont instanceof PhysicalFont)) {
            newFont = oldFont;
        }
        PhysicalFont physicalFont = (PhysicalFont)newFont;
        CompositeFont dialog2D =
            (CompositeFont)findFont2D("dialog", style, NO_FALLBACK);
        if (dialog2D == null) { /* shouldn't happen */
            return handle;
        }
        CompositeFont compFont = new CompositeFont(physicalFont, dialog2D);
        Font2DHandle newHandle = compFont.handle;
        return newHandle;
	}
    
    /*
     * return String representation of style prepended with "."
     * This is useful for performance to avoid unnecessary string operations.
     */
    private static String dotStyleStr(int num) {
        switch(num){
          case Font.BOLD:
            return ".bold";
          case Font.ITALIC:
            return ".italic";
          case Font.ITALIC | Font.BOLD:
            return ".bolditalic";
          default:
            return ".plain";
        }
    }
    
    private ConcurrentHashMap<String, Font2D> fontNameCache =
            new ConcurrentHashMap<String, Font2D>();
    /*
     * The client supplies a name and a style.
     * The name could be a family name, or a full name.
     * A font may exist with the specified style, or it may
     * exist only in some other style. For non-native fonts the scaler
     * may be able to emulate the required style.
     */
    public Font2D findFont2D(String name, int style, int fallback) {
        String lowerCaseName = name.toLowerCase(Locale.ENGLISH);
        String mapName = lowerCaseName + dotStyleStr(style);
        Font2D font;

        /* If preferLocaleFonts() or preferProportionalFonts() has been
         * called we may be using an alternate set of composite fonts in this
         * app context. The presence of a pre-built name map indicates whether
         * this is so, and gives access to the alternate composite for the
         * name.
         */
        if (_usingPerAppContextComposites) {
            ConcurrentHashMap<String, Font2D> altNameCache =
                (ConcurrentHashMap<String, Font2D>)
                AppContext.getAppContext().get(CompositeFont.class);
            if (altNameCache != null) {
                font = (Font2D)altNameCache.get(mapName);
            } else {
                font = null;
            }
        } else {
            font = fontNameCache.get(mapName);
        }
        if (font != null) {
            return font;
        }

        if (FontUtilities.isLogging()) {
            FontUtilities.getLogger().info("Search for font: " + name);
        }

        // The check below is just so that the bitmap fonts being set by
        // AWT and Swing thru the desktop properties do not trigger the
        // the load fonts case. The two bitmap fonts are now mapped to
        // appropriate equivalents for serif and sansserif.
        // Note that the cost of this comparison is only for the first
        // call until the map is filled.
        if (FontUtilities.isWindows) {
            if (lowerCaseName.equals("ms sans serif")) {
                name = "sansserif";
            } else if (lowerCaseName.equals("ms serif")) {
                name = "serif";
            }
        }

        /* This isn't intended to support a client passing in the
         * string default, but if a client passes in null for the name
         * the java.awt.Font class internally substitutes this name.
         * So we need to recognise it here to prevent a loadFonts
         * on the unrecognised name. The only potential problem with
         * this is it would hide any real font called "default"!
         * But that seems like a potential problem we can ignore for now.
         */
        if (lowerCaseName.equals("default")) {
        	lowerCaseName = name = "dialog";
        }

        font = new PhysicalFont(name,style);
        
        switch (lowerCaseName){
        case "dialog":
        	font = new CompositeFont(font); //dialog must a CompositeFont, else there are ClassCastExceptions
        	break;
        }
        fontNameCache.put(mapName, font);
        return font;
    }

    /*
     * Workaround for apps which are dependent on a font metrics bug
     * in JDK 1.1. This is an unsupported win32 private setting.
     * Left in for a customer - do not remove.
     */
	public boolean usePlatformFontMetrics() {
		return usePlatformFontMetrics;
	}

    public Font2D createFont2D(File fontFile, int fontFormat,
                               boolean isCopy, CreatedFontTracker tracker)
    throws FontFormatException {
    	throw new NotYetImplementedError();
    }
    /*
     * This is called when font is determined to be invalid/bad.
     * It designed to be called (for example) by the font scaler
     * when in processing a font file it is discovered to be incorrect.
     * This is different than the case where fonts are discovered to
     * be incorrect during initial verification, as such fonts are
     * never registered.
     * Handles to this font held are re-directed to a default font.
     * This default may not be an ideal substitute buts it better than
     * crashing This code assumes a PhysicalFont parameter as it doesn't
     * make sense for a Composite to be "bad".
     */
    public synchronized void deRegisterBadFont(Font2D font2D) {
        if (!(font2D instanceof PhysicalFont)) {
            /* We should never reach here, but just in case */
            return;
        } else {
            if (FontUtilities.isLogging()) {
                FontUtilities.getLogger()
                                     .severe("Deregister bad font: " + font2D);
            }
            throw new NotYetImplementedError();
        }
    }
    
    /* Supporting "alternate" composite fonts on 2D graphics objects
     * is accessed by the application by calling methods on the local
     * GraphicsEnvironment. The overall implementation is described
     * in one place, here, since otherwise the implementation is spread
     * around it may be difficult to track.
     * The methods below call into SunGraphicsEnvironment which creates a
     * new FontConfiguration instance. The FontConfiguration class,
     * and its platform sub-classes are updated to take parameters requesting
     * these behaviours. This is then used to create new composite font
     * instances. Since this calls the initCompositeFont method in
     * SunGraphicsEnvironment it performs the same initialization as is
     * performed normally. There may be some duplication of effort, but
     * that code is already written to be able to perform properly if called
     * to duplicate work. The main difference is that if we detect we are
     * running in an applet/browser/Java plugin environment these new fonts
     * are not placed in the "default" maps but into an AppContext instance.
     * The font lookup mechanism in java.awt.Font.getFont2D() is also updated
     * so that look-up for composite fonts will in that case always
     * do a lookup rather than returning a cached result.
     * This is inefficient but necessary else singleton java.awt.Font
     * instances would not retrieve the correct Font2D for the appcontext.
     * sun.font.FontManager.findFont2D is also updated to that it uses
     * a name map cache specific to that appcontext.
     *
     * Getting an AppContext is expensive, so there is a global variable
     * that records whether these methods have ever been called and can
     * avoid the expense for almost all applications. Once the correct
     * CompositeFont is associated with the Font, everything should work
     * through existing mechanisms.
     * A special case is that GraphicsEnvironment.getAllFonts() must
     * return an AppContext specific list.
     *
     * Calling the methods below is "heavyweight" but it is expected that
     * these methods will be called very rarely.
     *
     * If _usingPerAppContextComposites is true, we are in "applet"
     * (eg browser) enviroment and at least one context has selected
     * an alternate composite font behaviour.
     * If _usingAlternateComposites is true, we are not in an "applet"
     * environment and the (single) application has selected
     * an alternate composite font behaviour.
     *
     * - Printing: The implementation delegates logical fonts to an AWT
     * mechanism which cannot use these alternate configurations.
     * We can detect that alternate fonts are in use and back-off to 2D, but
     * that uses outlines. Much of this can be fixed with additional work
     * but that may have to wait. The results should be correct, just not
     * optimal.
     */
    private boolean _usingPerAppContextComposites = false;
    private boolean _usingAlternateComposites = false;

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
	
    public synchronized void preferLocaleFonts() {
        if (FontUtilities.isLogging()) {
            FontUtilities.getLogger().info("Entered preferLocaleFonts().");
        }
        /* Test if re-ordering will have any effect */
        if (!FontConfiguration.willReorderForStartupLocale()) {
            return;
        }

    }
    
    public synchronized void preferProportionalFonts() {
    	throw new NotYetImplementedError();
    }

    public boolean registerFont(Font font) {
        /* This method should not be called with "null".
         * It is the caller's responsibility to ensure that.
         */
        if (font == null) {
            return false;
        }
        throw new NotYetImplementedError();
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
    public static Font2D createFont2D( cli.System.Drawing.FontFamily family, int style ) {
        return new PhysicalFont( family, style );
    }  
}
