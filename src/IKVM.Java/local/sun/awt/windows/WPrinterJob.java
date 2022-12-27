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
package sun.awt.windows;

import ikvm.awt.IkvmToolkit;

import java.awt.Graphics;
import java.awt.HeadlessException;
import java.awt.Toolkit;
import java.awt.image.BufferedImage;
import java.awt.print.PageFormat;
import java.awt.print.Pageable;
import java.awt.print.Paper;
import java.awt.print.Printable;
import java.awt.print.PrinterException;
import java.awt.print.PrinterJob;

import javax.print.PrintService;
import javax.print.attribute.PrintRequestAttributeSet;

import cli.System.Drawing.Printing.*;

import sun.print.PeekGraphics;
import sun.print.PrintPeer;
import sun.print.RasterPrinterJob;
import sun.print.Win32PrintService;

import ikvm.internal.NotYetImplementedError;

/**
 * @author Volker Berlin
 */
public class WPrinterJob extends RasterPrinterJob{

    @Override
    protected void abortDoc(){
        throw new NotYetImplementedError();
    }

    @Override
    protected void endDoc() throws PrinterException{
        throw new NotYetImplementedError();
    }

    @Override
    protected void endPage(PageFormat format, Printable painter, int index) throws PrinterException{
        throw new NotYetImplementedError();
    }

    @Override
    protected double getPhysicalPageHeight(Paper p){
        throw new NotYetImplementedError();
    }

    @Override
    protected double getPhysicalPageWidth(Paper p){
        throw new NotYetImplementedError();
    }

    @Override
    protected double getPhysicalPrintableHeight(Paper p){
        throw new NotYetImplementedError();
    }

    @Override
    protected double getPhysicalPrintableWidth(Paper p){
        throw new NotYetImplementedError();
    }

    @Override
    protected double getPhysicalPrintableX(Paper p){
        throw new NotYetImplementedError();
    }

    @Override
    protected double getPhysicalPrintableY(Paper p){
        throw new NotYetImplementedError();
    }

    @Override
    protected double getXRes(){
        throw new NotYetImplementedError();
    }

    @Override
    protected double getYRes(){
        throw new NotYetImplementedError();
    }

    @Override
    protected void printBand(byte[] data, int x, int y, int width, int height) throws PrinterException{
        throw new NotYetImplementedError();
    }

    @Override
    protected void startDoc() throws PrinterException{
        throw new NotYetImplementedError();
    }

    @Override
    protected void startPage(PageFormat format, Printable painter, int index, boolean paperChanged)
            throws PrinterException{
        throw new NotYetImplementedError();
    }
}
