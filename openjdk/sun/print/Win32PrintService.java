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
package sun.print;

import javax.print.DocFlavor;
import javax.print.DocPrintJob;
import javax.print.PrintService;
import javax.print.ServiceUIFactory;
import javax.print.attribute.Attribute;
import javax.print.attribute.AttributeSet;
import javax.print.attribute.PrintServiceAttribute;
import javax.print.attribute.PrintServiceAttributeSet;
import javax.print.event.PrintServiceAttributeListener;



/**
 * @author Volker Berlin
 */
public class Win32PrintService implements PrintService{

    public Win32PrintService(String string){
        // TODO Auto-generated constructor stub
    }


    @Override
    public void addPrintServiceAttributeListener(PrintServiceAttributeListener listener){
        // TODO Auto-generated method stub

    }


    @Override
    public DocPrintJob createPrintJob(){
        // TODO Auto-generated method stub
        return null;
    }


    @Override
    public <T extends PrintServiceAttribute>T getAttribute(Class<T> category){
        // TODO Auto-generated method stub
        return null;
    }


    @Override
    public PrintServiceAttributeSet getAttributes(){
        // TODO Auto-generated method stub
        return null;
    }


    @Override
    public Object getDefaultAttributeValue(Class<? extends Attribute> category){
        // TODO Auto-generated method stub
        return null;
    }


    @Override
    public String getName(){
        // TODO Auto-generated method stub
        return null;
    }


    @Override
    public ServiceUIFactory getServiceUIFactory(){
        // TODO Auto-generated method stub
        return null;
    }


    @Override
    public Class<?>[] getSupportedAttributeCategories(){
        // TODO Auto-generated method stub
        return null;
    }


    @Override
    public Object getSupportedAttributeValues(Class<? extends Attribute> category, DocFlavor flavor,
            AttributeSet attributes){
        // TODO Auto-generated method stub
        return null;
    }


    @Override
    public DocFlavor[] getSupportedDocFlavors(){
        // TODO Auto-generated method stub
        return null;
    }


    @Override
    public AttributeSet getUnsupportedAttributes(DocFlavor flavor, AttributeSet attributes){
        // TODO Auto-generated method stub
        return null;
    }


    @Override
    public boolean isAttributeCategorySupported(Class<? extends Attribute> category){
        // TODO Auto-generated method stub
        return false;
    }


    @Override
    public boolean isAttributeValueSupported(Attribute attrval, DocFlavor flavor, AttributeSet attributes){
        // TODO Auto-generated method stub
        return false;
    }


    @Override
    public boolean isDocFlavorSupported(DocFlavor flavor){
        // TODO Auto-generated method stub
        return false;
    }


    @Override
    public void removePrintServiceAttributeListener(PrintServiceAttributeListener listener){
        // TODO Auto-generated method stub

    }

}
