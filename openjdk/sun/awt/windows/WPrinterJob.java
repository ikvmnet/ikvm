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

import java.awt.HeadlessException;
import java.awt.print.PageFormat;
import java.awt.print.Pageable;
import java.awt.print.Printable;
import java.awt.print.PrinterException;
import java.awt.print.PrinterJob;



/**
 * @author Volker Berlin
 */
public class WPrinterJob extends PrinterJob{

    @Override
    public void cancel(){
        // TODO Auto-generated method stub

    }


    @Override
    public PageFormat defaultPage(PageFormat page){
        // TODO Auto-generated method stub
        return null;
    }


    @Override
    public int getCopies(){
        // TODO Auto-generated method stub
        return 0;
    }


    @Override
    public String getJobName(){
        // TODO Auto-generated method stub
        return null;
    }


    @Override
    public String getUserName(){
        // TODO Auto-generated method stub
        return null;
    }


    @Override
    public boolean isCancelled(){
        // TODO Auto-generated method stub
        return false;
    }


    @Override
    public PageFormat pageDialog(PageFormat page) throws HeadlessException{
        // TODO Auto-generated method stub
        return null;
    }


    @Override
    public void print() throws PrinterException{
        // TODO Auto-generated method stub

    }


    @Override
    public boolean printDialog() throws HeadlessException{
        // TODO Auto-generated method stub
        return false;
    }


    @Override
    public void setCopies(int copies){
        // TODO Auto-generated method stub

    }


    @Override
    public void setJobName(String jobName){
        // TODO Auto-generated method stub

    }


    @Override
    public void setPageable(Pageable document) throws NullPointerException{
        // TODO Auto-generated method stub

    }


    @Override
    public void setPrintable(Printable painter){
        // TODO Auto-generated method stub

    }


    @Override
    public void setPrintable(Printable painter, PageFormat format){
        // TODO Auto-generated method stub

    }


    @Override
    public PageFormat validatePage(PageFormat page){
        // TODO Auto-generated method stub
        return null;
    }

}
