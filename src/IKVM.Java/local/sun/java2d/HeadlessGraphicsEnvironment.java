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
package sun.java2d;

import java.awt.Font;
import java.awt.Graphics2D;
import java.awt.GraphicsDevice;
import java.awt.GraphicsEnvironment;
import java.awt.HeadlessException;
import java.awt.Point;
import java.awt.Rectangle;
import java.awt.image.BufferedImage;
import java.util.Locale;


/**
 * Placeholder for not supported headless environment
 */
public class HeadlessGraphicsEnvironment extends GraphicsEnvironment{

    private final GraphicsEnvironment env;
    

    public HeadlessGraphicsEnvironment(GraphicsEnvironment env){
        this.env = env;
    }
    
    /**
     * {@inheritDoc}
     */
    @Override
    public Graphics2D createGraphics(BufferedImage img){
        return env.createGraphics(img);
    }

    /**
     * {@inheritDoc}
     */
    @Override
    public boolean equals(Object obj){
        return env.equals(obj);
    }

    /**
     * {@inheritDoc}
     */
    @Override
    public Font[] getAllFonts(){
        return env.getAllFonts();
    }

    /**
     * {@inheritDoc}
     */
    @Override
    public String[] getAvailableFontFamilyNames(){
        return env.getAvailableFontFamilyNames();
    }

    /**
     * {@inheritDoc}
     */
    @Override
    public String[] getAvailableFontFamilyNames(Locale l){
        return env.getAvailableFontFamilyNames(l);
    }

    /**
     * {@inheritDoc}
     */
    @Override
    public Point getCenterPoint() throws HeadlessException{
        return env.getCenterPoint();
    }

    /**
     * {@inheritDoc}
     */
    @Override
    public GraphicsDevice getDefaultScreenDevice() throws HeadlessException{
        return env.getDefaultScreenDevice();
    }

    /**
     * {@inheritDoc}
     */
    @Override
    public Rectangle getMaximumWindowBounds() throws HeadlessException{
        return env.getMaximumWindowBounds();
    }

    /**
     * {@inheritDoc}
     */
    @Override
    public GraphicsDevice[] getScreenDevices() throws HeadlessException{
        return env.getScreenDevices();
    }

    /**
     * {@inheritDoc}
     */
    @Override
    public int hashCode(){
        return env.hashCode();
    }

    /**
     * {@inheritDoc}
     */
    @Override
    public boolean isHeadlessInstance(){
        return env.isHeadlessInstance();
    }

    /**
     * {@inheritDoc}
     */
    @Override
    public void preferLocaleFonts(){
        env.preferLocaleFonts();
    }

    /**
     * {@inheritDoc}
     */
    @Override
    public void preferProportionalFonts(){
        env.preferProportionalFonts();
    }

    /**
     * {@inheritDoc}
     */
    @Override
    public boolean registerFont(Font font){
        return env.registerFont(font);
    }

    /**
     * {@inheritDoc}
     */
    @Override
    public String toString(){
        return env.toString();
    }    
}
