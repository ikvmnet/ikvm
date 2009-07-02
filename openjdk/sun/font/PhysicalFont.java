/*
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
package sun.font;

import java.awt.Font;
import java.awt.font.FontRenderContext;
import java.util.Locale;

import cli.System.Drawing.FontFamily;
import cli.System.Drawing.FontStyle;
import cli.System.Globalization.CultureInfo;

/**
 * A Font2D implementation that based on .NET fonts. It replace the equals naming Sun class.
 * A Font2D is define with the font name and the font style but it is independent of the size;
 */
class PhysicalFont extends Font2D{

    private final FontFamily family;

    private final FontStyle style;

    private static final FontStyle REGULAR = FontStyle.wrap(FontStyle.Regular);

    private static final FontStyle BOLD = FontStyle.wrap(FontStyle.Bold);

    private static final FontStyle ITALIC = FontStyle.wrap(FontStyle.Italic);

    private static final FontStyle BOLD_ITALIC = FontStyle.wrap(FontStyle.Bold + FontStyle.Italic);

    private static final cli.System.Drawing.GraphicsUnit PIXEL = cli.System.Drawing.GraphicsUnit
            .wrap(cli.System.Drawing.GraphicsUnit.Pixel);

    private FontStrike strike;


    PhysicalFont(String name, int style){
        this.family = createFontFamily(name);
        this.style = createFontStyle(family, style);
    }


    /**
     * {@inheritDoc}
     */
    @Override
    public cli.System.Drawing.Font createNetFont(Font font){
        float size2D = font.getSize2D();
        if(size2D <= 0){
            size2D = 1;
        }
        return new cli.System.Drawing.Font(family, size2D, style, PIXEL);
    }


    private static FontFamily createFontFamily(String name){
        if("monospaced".equalsIgnoreCase(name) || "courier".equalsIgnoreCase(name)){
            return FontFamily.get_GenericMonospace();
        }
        if("serif".equalsIgnoreCase(name)){
            return FontFamily.get_GenericSerif();
        }
        if(name == null || "sansserif".equalsIgnoreCase(name) || "dialog".equalsIgnoreCase(name)
                || "dialoginput".equalsIgnoreCase(name) || "default".equalsIgnoreCase(name)){
            return FontFamily.get_GenericSansSerif();
        }
        try{
            if(false) throw new cli.System.ArgumentException();
            return new FontFamily(name);
        }catch(cli.System.ArgumentException ex) // cli.System.ArgumentException
        {
            return FontFamily.get_GenericSansSerif();
        }
    }


    private static FontStyle createFontStyle(FontFamily family, int style){
        int fs = FontStyle.Regular;
        if((style & java.awt.Font.BOLD) != 0){
            fs |= FontStyle.Bold;
        }
        if((style & java.awt.Font.ITALIC) != 0){
            fs |= FontStyle.Italic;
        }
        FontStyle fontStyle = FontStyle.wrap(fs);
        if(!family.IsStyleAvailable(fontStyle)){
            // Some Fonts (for example Aharoni) does not support Regular style. This throw an exception else it is not
            // documented.
            if(family.IsStyleAvailable(REGULAR)){
                fontStyle = REGULAR;
            }else if(family.IsStyleAvailable(BOLD)){
                fontStyle = BOLD;
            }else if(family.IsStyleAvailable(ITALIC)){
                fontStyle = ITALIC;
            }else if(family.IsStyleAvailable(BOLD_ITALIC)){
                fontStyle = BOLD_ITALIC;
            }
        }
        return fontStyle;
    }


    /**
     * {@inheritDoc}
     */
    @Override
    public FontStrike getStrike(Font font, FontRenderContext frc){
        if(strike == null){
            strike = new PhysicalStrike(font.getSize2D(), family, style);
        }
        return strike;
    }


    /**
     * {@inheritDoc}
     */
    @Override
    public int getStyle(){
        return style.Value;
    }


    /**
     * {@inheritDoc}
     */
    @Override
    public String getPostscriptName(){
        return family.get_Name();
    }


    /**
     * {@inheritDoc}
     */
    @Override
    public String getFontName(Locale locale){
        return family.GetName(getLanguage(locale));
    }


    /**
     * {@inheritDoc}
     */
    @Override
    public String getFamilyName(Locale locale){
        return family.GetName(getLanguage(locale));
    }
    
    /**
     * Convert the Java locale to a language ID
     */
    private int getLanguage(Locale locale){
        int language = 0;
        try{
            language = CultureInfo.GetCultureInfo(locale.toString().replace("_", "-")).get_LCID();
        }catch(Throwable th){
            try{
                language = CultureInfo.GetCultureInfo(locale.getLanguage()).get_LCID();
            }catch(Throwable th2){}
        }
        return language;
    }
}
