/*
 * Copyright (c) 2003, 2010, Oracle and/or its affiliates. All rights reserved.
 * Copyright (C) 2009 - 2011 Volker Berlin (i-net software)
 * DO NOT ALTER OR REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 *
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.  Oracle designates this
 * particular file as subject to the "Classpath" exception as provided
 * by Oracle in the LICENSE file that accompanied this code.
 *
 * This code is distributed in the hope that it will be useful, but WITHOUT
 * ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or
 * FITNESS FOR A PARTICULAR PURPOSE.  See the GNU General Public License
 * version 2 for more details (a copy is included in the LICENSE file that
 * accompanied this code).
 *
 * You should have received a copy of the GNU General Public License version
 * 2 along with this work; if not, write to the Free Software Foundation,
 * Inc., 51 Franklin St, Fifth Floor, Boston, MA 02110-1301 USA.
 *
 * Please contact Oracle, 500 Oracle Parkway, Redwood Shores, CA 94065 USA
 * or visit www.oracle.com if you need additional information or have any
 * questions.
 */

package sun.font;

import java.awt.Font;
import java.lang.reflect.Method;
import java.util.Locale;
import java.util.concurrent.ConcurrentHashMap;

import javax.swing.plaf.FontUIResource;

import cli.System.Drawing.FontFamily;
import ikvm.internal.NotYetImplementedError;

/*
 * Interface between Java Fonts (java.awt.Font) and the underlying
 * font files/native font resources and the Java and native font scalers.
 */
public final class FontManager {

    public static final int NO_FALLBACK         = 0;
    public static final int PHYSICAL_FALLBACK   = 1;
    public static final int LOGICAL_FALLBACK    = 2;
    
    /* deprecated, unsupported hack - actually invokes a bug! */
    private static boolean usePlatformFontMetrics = false;

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

    /* Revise the implementation to in fact mean "font is a composite font.
     * This ensures that Swing components will always benefit from the
     * fall back fonts
     */
    public static boolean fontSupportsDefaultEncoding(Font font) {
        // In Java the font must be a instanceof CompositeFont
        // because .NET fonts are all already Composite Fonts (I think) that we can return true
        // and does not need to implements CompositeFont
        return true;
    }

    /**
     * This method is provided for internal and exclusive use by Swing.
     *
     * It may be used in conjunction with fontSupportsDefaultEncoding(Font)
     * In the event that a desktop properties font doesn't directly
     * support the default encoding, (ie because the host OS supports
     * adding support for the current locale automatically for native apps),
     * then Swing calls this method to get a font which  uses the specified
     * font for the code points it covers, but also supports this locale
     * just as the standard composite fonts do.
     * Note: this will over-ride any setting where an application
     * specifies it prefers locale specific composite fonts.
     * The logic for this, is that this method is used only where the user or
     * application has specified that the native L&F be used, and that
     * we should honour that request to use the same font as native apps use.
     *
     * The behaviour of this method is to construct a new composite
     * Font object that uses the specified physical font as its first
     * component, and adds all the components of "dialog" as fall back
     * components.
     * The method currently assumes that only the size and style attributes
     * are set on the specified font. It doesn't copy the font transform or
     * other attributes because they aren't set on a font created from
     * the desktop. This will need to be fixed if use is broadened.
     *
     * Operations such as Font.deriveFont will work properly on the
     * font returned by this method for deriving a different point size.
     * Additionally it tries to support a different style by calling
     * getNewComposite() below. That also supports replacing slot zero
     * with a different physical font but that is expected to be "rare".
     * Deriving with a different style is needed because its been shown
     * that some applications try to do this for Swing FontUIResources.
     * Also operations such as new Font(font.getFontName(..), Font.PLAIN, 14);
     * will NOT yield the same result, as the new underlying CompositeFont
     * cannot be "looked up" in the font registry.
     * This returns a FontUIResource as that is the Font sub-class needed
     * by Swing.
     * Suggested usage is something like :
     * FontUIResource fuir;
     * Font desktopFont = getDesktopFont(..);
     * // NOTE even if fontSupportsDefaultEncoding returns true because
     * // you get Tahoma and are running in an English locale, you may
     * // still want to just call getCompositeFontUIResource() anyway
     * // as only then will you get fallback fonts - eg for CJK.
     * if (FontManager.fontSupportsDefaultEncoding(desktopFont)) {
     *   fuir = new FontUIResource(..);
     * } else {
     *   fuir = FontManager.getCompositeFontUIResource(desktopFont);
     * }
     * return fuir;
     */
    public static FontUIResource getCompositeFontUIResource(Font font) {
        throw new NotYetImplementedError();
    }

    public static Font2D getNewComposite(String family, int style, Font2D handle) {
        throw new NotYetImplementedError();
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

    /*
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
    
    /* This method can be more efficient as it will only need to
     * do the lookup once, and subsequent calls on the java.awt.Font
     * instance can utilise the cached Font2D on that object.
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

    /* Stuff below was in NativeFontWrapper and needed a new home */

    /*
     * Workaround for apps which are dependent on a font metrics bug
     * in JDK 1.1. This is an unsupported win32 private setting.
     */
    public static boolean usePlatformFontMetrics() {
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
    static boolean maybeUsingAlternateCompositeFonts() {
        // TODO Auto-generated method stub
        return false;
    }

    public static synchronized void preferLocaleFonts() {
        // TODO Auto-generated method stub
        
    }

    public static synchronized void preferProportionalFonts() {
        // TODO Auto-generated method stub
    }

    public static boolean registerFont(Font font) {
        /* This method should not be called with "null".
         * It is the caller's responsibility to ensure that.
         */
        // TODO Auto-generated method stub
        return false;
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

    /* The following fields and methods which relate to layout
     * perhaps belong in some other class but FontManager is already
     * widely used as an entry point for other JDK code that needs
     * access to the font system internals.
     */

    /**
     * Referenced by code in the JDK which wants to test for the
     * minimum char code for which layout may be required.
     * Note that even basic latin text can benefit from ligatures,
     * eg "ffi" but we presently apply those only if explicitly
     * requested with TextAttribute.LIGATURES_ON.
     * The value here indicates the lowest char code for which failing
     * to invoke layout would prevent acceptable rendering.
     */
    public static final int MIN_LAYOUT_CHARCODE = 0x0300;

    /**
     * Referenced by code in the JDK which wants to test for the
     * maximum char code for which layout may be required.
     * Note this does not account for supplementary characters
     * where the caller interprets 'layout' to mean any case where
     * one 'char' (ie the java type char) does not map to one glyph
     */
    public static final int MAX_LAYOUT_CHARCODE = 0x206F;

    /* If the character code falls into any of a number of unicode ranges
     * where we know that simple left->right layout mapping chars to glyphs
     * 1:1 and accumulating advances is going to produce incorrect results,
     * we want to know this so the caller can use a more intelligent layout
     * approach. A caller who cares about optimum performance may want to
     * check the first case and skip the method call if its in that range.
     * Although there's a lot of tests in here, knowing you can skip
     * CTL saves a great deal more. The rest of the checks are ordered
     * so that rather than checking explicitly if (>= start & <= end)
     * which would mean all ranges would need to be checked so be sure
     * CTL is not needed, the method returns as soon as it recognises
     * the code point is outside of a CTL ranges.
     * NOTE: Since this method accepts an 'int' it is asssumed to properly
     * represent a CHARACTER. ie it assumes the caller has already
     * converted surrogate pairs into supplementary characters, and so
     * can handle this case and doesn't need to be told such a case is
     * 'complex'.
     */
    static boolean isComplexCharCode(int code) {

        if (code < MIN_LAYOUT_CHARCODE || code > MAX_LAYOUT_CHARCODE) {
            return false;
        }
        else if (code <= 0x036f) {
            // Trigger layout for combining diacriticals 0x0300->0x036f
            return true;
        }
        else if (code < 0x0590) {
            // No automatic layout for Greek, Cyrillic, Armenian.
             return false;
        }
        else if (code <= 0x06ff) {
            // Hebrew 0590 - 05ff
            // Arabic 0600 - 06ff
            return true;
        }
        else if (code < 0x0900) {
            return false; // Syriac and Thaana
        }
        else if (code <= 0x0e7f) {
            // if Indic, assume shaping for conjuncts, reordering:
            // 0900 - 097F Devanagari
            // 0980 - 09FF Bengali
            // 0A00 - 0A7F Gurmukhi
            // 0A80 - 0AFF Gujarati
            // 0B00 - 0B7F Oriya
            // 0B80 - 0BFF Tamil
            // 0C00 - 0C7F Telugu
            // 0C80 - 0CFF Kannada
            // 0D00 - 0D7F Malayalam
            // 0D80 - 0DFF Sinhala
            // 0E00 - 0E7F if Thai, assume shaping for vowel, tone marks
            return true;
        }
        else if (code < 0x1780) {
            return false;
        }
        else if (code <= 0x17ff) { // 1780 - 17FF Khmer
            return true;
        }
        else if (code < 0x200c) {
            return false;
        }
        else if (code <= 0x200d) { //  zwj or zwnj
            return true;
        }
        else if (code >= 0x202a && code <= 0x202e) { // directional control
            return true;
        }
        else if (code >= 0x206a && code <= 0x206f) { // directional control
            return true;
        }
        return false;
    }

    /* This is almost the same as the method above, except it takes a
     * char which means it may include undecoded surrogate pairs.
     * The distinction is made so that code which needs to identify all
     * cases in which we do not have a simple mapping from
     * char->unicode character->glyph can be be identified.
     * For example measurement cannot simply sum advances of 'chars',
     * the caret in editable text cannot advance one 'char' at a time, etc.
     * These callers really are asking for more than whether 'layout'
     * needs to be run, they need to know if they can assume 1->1
     * char->glyph mapping.
     */
    static boolean isNonSimpleChar(char ch) {
        return
            isComplexCharCode(ch) ||
            (ch >= CharToGlyphMapper.HI_SURROGATE_START &&
             ch <= CharToGlyphMapper.LO_SURROGATE_END);
    }

    /**
     * If there is anything in the text which triggers a case
     * where char->glyph does not map 1:1 in straightforward
     * left->right ordering, then this method returns true.
     * Scripts which might require it but are not treated as such
     * due to JDK implementations will not return true.
     * ie a 'true' return is an indication of the treatment by
     * the implementation.
     * Whether supplementary characters should be considered is dependent
     * on the needs of the caller. Since this method accepts the 'char' type
     * then such chars are always represented by a pair. From a rendering
     * perspective these will all (in the cases I know of) still be one
     * unicode character -> one glyph. But if a caller is using this to
     * discover any case where it cannot make naive assumptions about
     * the number of chars, and how to index through them, then it may
     * need the option to have a 'true' return in such a case.
     */
    public static boolean isComplexText(char [] chs, int start, int limit) {

        for (int i = start; i < limit; i++) {
            if (chs[i] < MIN_LAYOUT_CHARCODE) {
                continue;
            }
            else if (isNonSimpleChar(chs[i])) {
                return true;
            }
        }
        return false;
    }

}
