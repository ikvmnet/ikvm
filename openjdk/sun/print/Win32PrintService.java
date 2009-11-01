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

import java.util.ArrayList;

import javax.print.DocFlavor;
import javax.print.DocPrintJob;
import javax.print.PrintService;
import javax.print.ServiceUIFactory;
import javax.print.attribute.Attribute;
import javax.print.attribute.AttributeSet;
import javax.print.attribute.AttributeSetUtilities;
import javax.print.attribute.HashPrintServiceAttributeSet;
import javax.print.attribute.PrintServiceAttribute;
import javax.print.attribute.PrintServiceAttributeSet;
import javax.print.attribute.standard.Chromaticity;
import javax.print.attribute.standard.ColorSupported;
import javax.print.attribute.standard.Copies;
import javax.print.attribute.standard.Destination;
import javax.print.attribute.standard.Fidelity;
import javax.print.attribute.standard.JobName;
import javax.print.attribute.standard.Media;
import javax.print.attribute.standard.MediaPrintableArea;
import javax.print.attribute.standard.OrientationRequested;
import javax.print.attribute.standard.PageRanges;
import javax.print.attribute.standard.PrintQuality;
import javax.print.attribute.standard.PrinterIsAcceptingJobs;
import javax.print.attribute.standard.PrinterName;
import javax.print.attribute.standard.PrinterResolution;
import javax.print.attribute.standard.PrinterState;
import javax.print.attribute.standard.PrinterStateReasons;
import javax.print.attribute.standard.QueuedJobCount;
import javax.print.attribute.standard.RequestingUserName;
import javax.print.attribute.standard.SheetCollate;
import javax.print.attribute.standard.Sides;
import javax.print.event.PrintServiceAttributeListener;

import cli.System.Drawing.Printing.PrinterSettings;

/**
 * @author Volker Berlin
 */
public class Win32PrintService implements PrintService{

    private static final DocFlavor[] supportedFlavors = {
        DocFlavor.SERVICE_FORMATTED.PAGEABLE,
        DocFlavor.SERVICE_FORMATTED.PRINTABLE,
    };
    
    /*  it turns out to be inconvenient to store the other categories
     *  separately because many attributes are in multiple categories.
     */
    private static Class[] otherAttrCats = {
        JobName.class,
        RequestingUserName.class,
        Copies.class,
        Destination.class,
        OrientationRequested.class,
        PageRanges.class,
        Media.class,
        MediaPrintableArea.class,
        Fidelity.class,
        // We support collation on 2D printer jobs, even if the driver can't.
        SheetCollate.class,
        SunAlternateMedia.class,
        Chromaticity.class
    };
    
    private final PrintPeer peer;
    
    private final String printer;
    private final PrinterSettings settings;
    private PrinterName name;

    transient private ServiceNotifier notifier = null;

    public Win32PrintService(String name, PrintPeer peer){
        if(name == null){
            throw new IllegalArgumentException("null printer name");
        }
        this.peer = peer;
        printer = name;
        settings = new PrinterSettings();
        settings.set_PrinterName(printer);
    }


    @Override
    public String getName(){
        return printer;
    }


    private PrinterName getPrinterName(){
        if(name == null){
            name = new PrinterName(printer, null);
        }
        return name;
    }


    public void wakeNotifier() {
        synchronized (this) {
            if (notifier != null) {
                notifier.wake();
            }
        }
    }
    

    @Override
    public void addPrintServiceAttributeListener(PrintServiceAttributeListener listener){
        synchronized (this) {
            if (listener == null) {
                return;
            }
            if (notifier == null) {
                notifier = new ServiceNotifier(this);
            }
            notifier.addListener(listener);
        }
    }


    @Override
    public void removePrintServiceAttributeListener(PrintServiceAttributeListener listener){
        synchronized (this) {
            if (listener == null || notifier == null ) {
                return;
            }
            notifier.removeListener(listener);
            if (notifier.isEmpty()) {
                notifier.stopNotifier();
                notifier = null;
            }
        }
    }


    @Override
    public DocPrintJob createPrintJob(){
        SecurityManager security = System.getSecurityManager();
        if(security != null){
            security.checkPrintJobAccess();
        }
        return new Win32PrintJob(this, peer);
    }


    @Override
    public <T extends PrintServiceAttribute>T getAttribute(Class<T> category){
        if(category == null){
            throw new NullPointerException("category");
        }
        if(!(PrintServiceAttribute.class.isAssignableFrom(category))){
            throw new IllegalArgumentException("Not a PrintServiceAttribute");
        }
        if(category == ColorSupported.class){
            if(settings.get_SupportsColor()){
                return (T)ColorSupported.SUPPORTED;
            }else{
                return (T)ColorSupported.NOT_SUPPORTED;
            }
        }else if(category == PrinterName.class){
            return (T)getPrinterName();
        }else if(category == PrinterState.class){
            return (T)PrinterState.UNKNOWN; // TODO
        }else if(category == PrinterStateReasons.class){
            return null; // TODO
        }else{
            // QueuedJobCount and PrinterIsAcceptingJobs
            return (T)peer.getPrinterStatus(printer, category);
        }
    }


    @Override
    public PrintServiceAttributeSet getAttributes(){
        PrintServiceAttributeSet attrs = new HashPrintServiceAttributeSet();
        attrs.add(getPrinterName());
        PrinterIsAcceptingJobs acptJobs = getAttribute(PrinterIsAcceptingJobs.class);
        if(acptJobs != null){
            attrs.add(acptJobs);
        }
        PrinterState prnState = getAttribute(PrinterState.class);
        if(prnState != null){
            attrs.add(prnState);
        }
        PrinterStateReasons prnStateReasons = getAttribute(PrinterStateReasons.class);
        if(prnStateReasons != null){
            attrs.add(prnStateReasons);
        }
        QueuedJobCount jobCount = getAttribute(QueuedJobCount.class);
        if(jobCount != null){
            attrs.add(jobCount);
        }
        if(settings.get_SupportsColor()){
            attrs.add(ColorSupported.SUPPORTED);
        }else{
            attrs.add(ColorSupported.NOT_SUPPORTED);
        }

        return AttributeSetUtilities.unmodifiableView(attrs);
    }


    @Override
    public Object getDefaultAttributeValue(Class<? extends Attribute> category){
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
        ArrayList<Class> categList = new ArrayList<Class>(otherAttrCats.length+3);
        for (int i=0; i < otherAttrCats.length; i++) {
            categList.add(otherAttrCats[i]);
        }

        if (settings.get_CanDuplex()) {
            categList.add(Sides.class);
        }

        if (settings.get_PrinterResolutions().get_Count() > 0) {
            categList.add(PrinterResolution.class);
        }

        return categList.toArray(new Class[categList.size()]);
    }


    @Override
    public Object getSupportedAttributeValues(Class<? extends Attribute> category, DocFlavor flavor,
            AttributeSet attributes){
        // TODO Auto-generated method stub
        return null;
    }


    @Override
    public DocFlavor[] getSupportedDocFlavors(){
        int len = supportedFlavors.length;
        DocFlavor[] result = new DocFlavor[len];
        System.arraycopy(supportedFlavors, 0, result, 0, len);
        return result;
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
        for (int f=0; f<supportedFlavors.length; f++) {
            if (flavor.equals(supportedFlavors[f])) {
                return true;
            }
        }
        return false;
    }


    @Override
    public String toString(){
        return "Win32 Printer : " + getName();
    }


    @Override
    public boolean equals(Object obj){
        return (obj == this || (obj instanceof Win32PrintService && ((Win32PrintService)obj).getName()
                .equals(getName())));
    }


    @Override
    public int hashCode(){
        return this.getClass().hashCode() + getName().hashCode();
    }
}
