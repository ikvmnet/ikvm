/*
  Copyright (C) 2009, 2010 Volker Berlin (i-net software)

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

import ikvm.awt.IkvmToolkit;

import java.awt.Font;
import java.awt.Toolkit;
import java.lang.reflect.InvocationTargetException;
import java.lang.reflect.Method;
import java.util.Locale;
import java.util.concurrent.ConcurrentHashMap;

import javax.swing.plaf.FontUIResource;

import cli.System.Drawing.FontFamily;

import sun.reflect.generics.reflectiveObjects.NotImplementedException;



/**
 * 
 */
public class FontManager{
    
    public static final int NO_FALLBACK         = 0;
    public static final int PHYSICAL_FALLBACK   = 1;
    public static final int LOGICAL_FALLBACK    = 2;
    
    private static ConcurrentHashMap<String, Font2D> fontNameCache =  new ConcurrentHashMap<String, Font2D>();

    private static final Method getFont2D;
    static{
        try{
            getFont2D = Font.class.getDeclaredMethod("getFont2D");
            getFont2D.setAccessible(true);
        }catch(NoSuchMethodException ex){
            NoClassDefFoundError error = new NoClassDefFoundError(ex.toString());
            error.initCause(ex);
            throw error;
        }
    }

    /**
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

    /**
     * The client supplies a name and a style.
     * The name could be a family name, or a full name.
     * A font may exist with the specified style, or it may
     * exist only in some other style. For non-native fonts the scaler
     * may be able to emulate the required style.
     */
    public static Font2D findFont2D(String name, int style, int fallback){
        String lowerCaseName = name.toLowerCase(Locale.ENGLISH);
        String mapName = lowerCaseName + dotStyleStr(style);
        Font2D font2D = fontNameCache.get(mapName);

        if(font2D != null){
            return font2D;
        }
        font2D = new PhysicalFont(name,style);
        fontNameCache.put(mapName, font2D);
        return font2D;
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
    
    /** This method can be more efficient as it will only need to
     * do the lookup once, and subsequent calls on the java.awt.Font
     * instance can utilize the cached Font2D on that object.
     * Its unfortunate it needs to be a native method, but the font2D
     * variable has to be private.
     */
    public static Font2D getFont2D(Font font){
        try{
            return (Font2D)getFont2D.invoke(font);
        }catch(Exception ex){
            throw new RuntimeException(ex);
        }
    }

    public static boolean isComplexText(char[] text, int start, int limit){
        // TODO Auto-generated method stub
        return false;
    }

    public static boolean maybeUsingAlternateCompositeFonts(){
        // TODO Auto-generated method stub
        return false;
    }

    public static boolean isNonSimpleChar(char ch){
        // TODO Auto-generated method stub
        return false;
    }

    public static boolean fontSupportsDefaultEncoding(Font f){
        // In Java the font must be a instanceof CompositeFont
        // because .NET fonts are all already Composite Fonts (I think) that we can return true
        // and does not need to implements CompositeFont
        return true;
    }

    public static Font getCompositeFontUIResource(Font f){
        throw new NotImplementedException();
    }

    public static boolean registerFont(Font font){
        // TODO Auto-generated method stub
        return false;
    }

    public static void preferLocaleFonts(){
        // TODO Auto-generated method stub
        
    }

    public static void preferProportionalFonts(){
        // TODO Auto-generated method stub
        
    }

    public static boolean usePlatformFontMetrics(){
        // TODO Auto-generated method stub
        return false;
    }

    public static Font2D getNewComposite(Object object, int style, Font2D font2D){
        throw new NotImplementedException();
    }

    /* This is called by Swing passing in a fontconfig family name
     * such as "sans". In return Swing gets a FontUIResource instance
     * that has queried fontconfig to resolve the font(s) used for this.
     * Fontconfig will if asked return a list of fonts to give the largest
     * possible code point coverage.
     * For now we use only the first font returned by fontconfig, and
     * back it up with the most closely matching JDK logical font.
     * Essentially this means pre-pending what we return now with fontconfig's
     * preferred physical font. This could lead to some duplication in cases,
     * if we already included that font later. We probably should remove such
     * duplicates, but it is not a significant problem. It can be addressed
     * later as part of creating a Composite which uses more of the
     * same fonts as fontconfig. At that time we also should pay more
     * attention to the special rendering instructions fontconfig returns,
     * such as whether we should prefer embedded bitmaps over antialiasing.
     * There's no way to express that via a Font at present.
     */
    public static FontUIResource getFontConfigFUIR( String fcFamily, int style, int size ) {
        return new FontUIResource( fcFamily, style, size );
    }

}
