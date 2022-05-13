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

import java.awt.Font;
import java.awt.font.FontRenderContext;
import java.lang.reflect.Constructor;
import java.lang.reflect.Method;
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
    
    private String familyName; // family name for logical fonts

    private final FontStyle style;

    private static final FontStyle REGULAR = FontStyle.wrap(FontStyle.Regular);

    private static final FontStyle BOLD = FontStyle.wrap(FontStyle.Bold);

    private static final FontStyle ITALIC = FontStyle.wrap(FontStyle.Italic);

    private static final FontStyle BOLD_ITALIC = FontStyle.wrap(FontStyle.Bold + FontStyle.Italic);

    private static final cli.System.Drawing.GraphicsUnit PIXEL = cli.System.Drawing.GraphicsUnit
            .wrap(cli.System.Drawing.GraphicsUnit.Pixel);

    //for method getStyleMetrics
    //for Reflection to the .NET 3.0 API in namespace System.Windows.Media of PresentationCore
    //If we switch to .NET 3.0 then we can access it directly
    private static boolean isMediaLoaded;
    private static Class classMediaFontFamily;
    private static Constructor ctorMediaFontFamily;
    private static Method getFamilyTypefaces;
    private static Method getItem;
    private static Method getStrikethroughPosition;
    private static Method getStrikethroughThickness;
    private static Method getUnderlinePosition;
    private static Method getUnderlineThickness;
    private float strikethroughPosition;
    private float strikethroughThickness;
    private float underlinePosition;
    private float underlineThickness;
    
    
    PhysicalFont(String name, int style){
        this.family = createFontFamily(name);
        this.style = createFontStyle(family, style);
    }

    PhysicalFont(FontFamily family, int style){
        this.family = family;
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

    /**
     * Search for .NET FamilyName. For logical fonts it set the variable familyName
     * @param name the name of the font.
     * @return ever a FontFamily
     */
    private FontFamily createFontFamily(String name){
        if(Font.MONOSPACED.equalsIgnoreCase(name)){
        	familyName = Font.MONOSPACED;
            return FontFamily.get_GenericMonospace();
        }
        if(Font.SERIF.equalsIgnoreCase(name)){
        	familyName = Font.SERIF;
            return FontFamily.get_GenericSerif();
        }
        if(name == null || Font.SANS_SERIF.equalsIgnoreCase(name)){
        	familyName = Font.SANS_SERIF;
            return FontFamily.get_GenericSansSerif();
        }
        if(Font.DIALOG.equalsIgnoreCase(name)){
        	familyName = Font.DIALOG;
            return FontFamily.get_GenericSansSerif();
        }
        if(Font.DIALOG_INPUT.equalsIgnoreCase(name)){
        	familyName = Font.DIALOG_INPUT;
            return FontFamily.get_GenericSansSerif();
        }
        try{
            if(false) throw new cli.System.ArgumentException();
            return new FontFamily(name);
        }catch(cli.System.ArgumentException ex){ 
        	// continue
        }
        
        //now we want map specific Name to a shorter Family Name like "Arial Bold" --> "Arial"
        String shortName = name;
        int spaceIdx = shortName.lastIndexOf(' ');
        while(spaceIdx > 0){
        	shortName = shortName.substring(0, spaceIdx).trim();
            try{
                if(false) throw new cli.System.ArgumentException();
                return new FontFamily(shortName);
            }catch(cli.System.ArgumentException ex){ 
            	// continue
            }
        	spaceIdx = shortName.lastIndexOf(' ');
        }
        
        //now we want map generic names to specific families like "courier" --> "Courier New"
        FontFamily[] fontFanilies = FontFamily.get_Families();
        name = name.toLowerCase();
        for (int i = 0; i < fontFanilies.length; i++) {
			FontFamily fontFamily = fontFanilies[i];
			if(fontFamily.get_Name().toLowerCase().startsWith(name)){
				return fontFamily;
			}
		}
        
        //we have not find a valid font, we use the default font
        familyName = Font.DIALOG;
        return FontFamily.get_GenericSansSerif();
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
        return new PhysicalStrike(font, family, style, frc);
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
    	if(familyName != null){
    		return familyName;
    	}
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
    
    
    /**
     * {@inheritDoc}
     */
    @Override
    public void getStyleMetrics(float pointSize, float[] metrics, int offset) {
        try{
            try{
                if(!isMediaLoaded){
                    classMediaFontFamily = Class.forName("System.Windows.Media.FontFamily, PresentationCore, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35");
                    ctorMediaFontFamily = classMediaFontFamily.getConstructor(String.class);
                    getFamilyTypefaces = classMediaFontFamily.getMethod("get_FamilyTypefaces");
                }
                if(classMediaFontFamily != null){
                    if(strikethroughPosition == 0.0){
                        Object mediaFontFamily = ctorMediaFontFamily.newInstance(family.get_Name());
                        Object familyTypefaces = getFamilyTypefaces.invoke(mediaFontFamily);
                        if(getItem == null){
                            getItem = familyTypefaces.getClass().getMethod("get_Item", Integer.TYPE);
                        }
                        Object familyTypeface = getItem.invoke(familyTypefaces, 0);
                        if(getStrikethroughPosition == null){
                            getStrikethroughPosition = familyTypeface.getClass().getMethod("get_StrikethroughPosition");
                            getStrikethroughThickness = familyTypeface.getClass().getMethod(
                                    "get_StrikethroughThickness");
                            getUnderlinePosition = familyTypeface.getClass().getMethod("get_UnderlinePosition");
                            getUnderlineThickness = familyTypeface.getClass().getMethod("get_UnderlineThickness");
                        }
                        strikethroughPosition = ((Number)getStrikethroughPosition.invoke(familyTypeface)).floatValue();
                        strikethroughThickness = ((Number)getStrikethroughThickness.invoke(familyTypeface))
                                .floatValue();
                        underlinePosition = ((Number)getUnderlinePosition.invoke(familyTypeface)).floatValue();
                        underlineThickness = ((Number)getUnderlineThickness.invoke(familyTypeface)).floatValue();
                    }
                }
                metrics[offset++] = -strikethroughPosition * pointSize;  
                metrics[offset++] = strikethroughThickness * pointSize;  
                metrics[offset++] = -underlinePosition * pointSize;  
                metrics[offset] = underlineThickness * pointSize;  
            }catch(Throwable ex){
                // ignore it, NET 3.0 is not available, use the default implementation
                super.getStyleMetrics(pointSize, metrics, offset);
            }
        }finally{
            isMediaLoaded = true;
        }
    }
}
