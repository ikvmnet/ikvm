/*
 * Copyright (c) 2000, 2006, Oracle and/or its affiliates. All rights reserved.
 * Copyright (C) 2009, 2012 Volker Berlin (i-net software)
 * Copyright (C) 2010, 2011 Karsten Heinrich (i-net software)
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
package sun.print;

import java.io.File;
import java.net.URI;
import java.net.URISyntaxException;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.HashSet;
import java.util.Hashtable;
import java.util.List;
import java.util.Set;
import java.util.concurrent.atomic.AtomicInteger;

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
import javax.print.attribute.standard.CopiesSupported;
import javax.print.attribute.standard.Destination;
import javax.print.attribute.standard.Fidelity;
import javax.print.attribute.standard.JobName;
import javax.print.attribute.standard.Media;
import javax.print.attribute.standard.MediaPrintableArea;
import javax.print.attribute.standard.MediaSize;
import javax.print.attribute.standard.MediaSizeName;
import javax.print.attribute.standard.MediaTray;
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

import cli.System.NewsStyleUriParser;
import cli.System.Type;
import cli.System.Collections.IEnumerator;
import cli.System.Drawing.RectangleF;
import cli.System.Drawing.Printing.Duplex;
import cli.System.Drawing.Printing.PaperKind;
import cli.System.Drawing.Printing.PaperSize;
import cli.System.Drawing.Printing.PaperSource;
import cli.System.Drawing.Printing.PrintDocument;
import cli.System.Drawing.Printing.PrinterSettings;
import cli.System.Drawing.Printing.PrinterSettings.PaperSizeCollection;
import cli.System.Drawing.Printing.PrinterSettings.PaperSourceCollection;
import cli.System.Drawing.Printing.PrinterSettings.PrinterResolutionCollection;
import cli.System.Net.Mime.MediaTypeNames;

/**
 * @author Volker Berlin
 */
public class Win32PrintService implements PrintService {
	// note: the Win32PrintService is implemented as foreign service (doesn't implement SunPrinterJobService)
	// to avoid implementing the WPrinterJob

    private static final DocFlavor[] supportedFlavors = {
        DocFlavor.SERVICE_FORMATTED.PAGEABLE,
        DocFlavor.SERVICE_FORMATTED.PRINTABLE,
    };
    
    /** Mapping for PageSize.RawKind to predefined MediaSizeName */ 
    private static final MediaSizeName[] MEDIA_NAMES = new MediaSizeName[70];
    
    private static final Hashtable<String, MediaSizeName> CUSTOM_MEDIA_NAME = new Hashtable<>();
    
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

    // conversion from 1/100 Inch (.NET) to um (Java)
    private static final int INCH100_TO_MYM = 254;
    private static final int  MATCH_DIFF = 500; // 0.5 mm
    
    static {
    	MEDIA_NAMES[0] = MediaSizeName.NA_LETTER;
    	MEDIA_NAMES[1] =  MediaSizeName.NA_LETTER ;
    	MEDIA_NAMES[2] =  MediaSizeName.TABLOID ;
    	MEDIA_NAMES[3] =  MediaSizeName.LEDGER ;
    	MEDIA_NAMES[4] =  MediaSizeName.NA_LEGAL ;
    	MEDIA_NAMES[5] =  MediaSizeName.INVOICE ; // Statement
    	MEDIA_NAMES[6] =  MediaSizeName.EXECUTIVE ;
    	MEDIA_NAMES[7] =  MediaSizeName.ISO_A3 ;
    	MEDIA_NAMES[8] =  MediaSizeName.ISO_A4 ;
    	MEDIA_NAMES[9] =  MediaSizeName.ISO_A4 ; // A4Small, 10
    	MEDIA_NAMES[10] =  MediaSizeName.ISO_A5 ;
    	MEDIA_NAMES[11] =  MediaSizeName.JIS_B4 ;
    	MEDIA_NAMES[12] =  MediaSizeName.JIS_B5 ;
    	MEDIA_NAMES[13] =  MediaSizeName.FOLIO ;
    	MEDIA_NAMES[14] =  MediaSizeName.QUARTO ;
    	MEDIA_NAMES[15] =  MediaSizeName.NA_10X14_ENVELOPE ;
    	MEDIA_NAMES[16] =  MediaSizeName.B ; // 10x17 Envelope
    	MEDIA_NAMES[17] =  MediaSizeName.NA_LETTER ; // Note
    	MEDIA_NAMES[18] =  MediaSizeName.NA_NUMBER_9_ENVELOPE ;
    	MEDIA_NAMES[19] =  MediaSizeName.NA_NUMBER_10_ENVELOPE ; // 20
    	MEDIA_NAMES[20] =  MediaSizeName.NA_NUMBER_11_ENVELOPE ;
    	MEDIA_NAMES[21] =  MediaSizeName.NA_NUMBER_12_ENVELOPE ;
    	MEDIA_NAMES[22] =  MediaSizeName.NA_NUMBER_14_ENVELOPE ;
    	MEDIA_NAMES[23] =  MediaSizeName.C ;
    	MEDIA_NAMES[24] =  MediaSizeName.D ;
    	MEDIA_NAMES[25] =  MediaSizeName.E ;
    	MEDIA_NAMES[26] =  MediaSizeName.ISO_DESIGNATED_LONG ;
    	MEDIA_NAMES[27] =  MediaSizeName.ISO_C5 ;
    	MEDIA_NAMES[28] =  MediaSizeName.ISO_C3 ;
    	MEDIA_NAMES[29] =  MediaSizeName.ISO_C4 ; // 30
    	MEDIA_NAMES[30] =  MediaSizeName.ISO_C6 ;
    	MEDIA_NAMES[31] =  MediaSizeName.ITALY_ENVELOPE ;
    	MEDIA_NAMES[32] =  MediaSizeName.ISO_B4 ;
    	MEDIA_NAMES[33] =  MediaSizeName.ISO_B5 ;
    	MEDIA_NAMES[34] =  MediaSizeName.ISO_B6 ;
    	MEDIA_NAMES[35] =  MediaSizeName.ITALY_ENVELOPE ;
    	MEDIA_NAMES[36] =  MediaSizeName.MONARCH_ENVELOPE ;
    	MEDIA_NAMES[37] =  MediaSizeName.PERSONAL_ENVELOPE ;
    	MEDIA_NAMES[38] =  MediaSizeName.NA_10X15_ENVELOPE ; // USStandardFanfold
    	MEDIA_NAMES[39] =  MediaSizeName.NA_9X12_ENVELOPE ; // GermanStandardFanfold, 40
    	MEDIA_NAMES[40] =  MediaSizeName.FOLIO ; // GermanLegalFanfold
    	MEDIA_NAMES[41] =  MediaSizeName.ISO_B4 ;
    	MEDIA_NAMES[42] =  MediaSizeName.JAPANESE_POSTCARD ;
    	MEDIA_NAMES[43] =  MediaSizeName.NA_9X11_ENVELOPE ;
    	
    	MEDIA_NAMES[65] =  MediaSizeName.ISO_A2 ;
    	
    	MEDIA_NAMES[69] =  MediaSizeName.ISO_A6 ;

//    	// augment the media size with the .NET default sizes available on the printer 
//    	PrinterSettings ps = new PrinterSettings();
//    	IEnumerator printers = PrinterSettings.get_InstalledPrinters().GetEnumerator();
//    	printers.Reset();
//    	while( printers.MoveNext() ){
//    		ps.set_PrinterName( (String) printers.get_Current() );
//    		IEnumerator sizes = ps.get_PaperSizes().GetEnumerator();
//    		sizes.Reset();
//    		while( sizes.MoveNext() ){
//    			PaperSize size = (PaperSize) sizes.get_Current();
//    			int kind = size.get_RawKind();
//				if( kind >= 0  && kind < MEDIA_NAMES.length && MEDIA_NAMES[kind] == null ){
//					MEDIA_NAMES[kind] = new CustomMediaSizeName( size.get_PaperName() );					
//					int x = size.get_Width();
//					int y = size.get_Height();
//					if( x > y ){ // not allowed by MediaSize
//						int tmp = x;
//						x = y;
//						y = tmp;
//					}
//					new MediaSize(x, y, INCH100_TO_MYM, MEDIA_NAMES[kind]); // cache entry in map
//    			}
//    		}
//    	}
    }
    
    private final PrintPeer peer;
    
    private final String printer;
    private final PrinterSettings settings;
    private PrinterName name;

    private MediaTray[] mediaTrays;
    
    transient private ServiceNotifier notifier = null;

    public Win32PrintService(String name, PrintPeer peer){
        if(name == null){
            throw new IllegalArgumentException("null printer name");
        }
        this.peer = peer;
        printer = name;
        settings = new PrintDocument().get_PrinterSettings();
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
            throw new IllegalArgumentException("The categhory '" + category + "' is not a valid PrintServiceAttribute");
        }
        if(category == ColorSupported.class){
        	// works better than settings.get_SupportsColor();
            if(settings.get_DefaultPageSettings().get_Color()){
                return (T)ColorSupported.SUPPORTED;
            }else{
                return (T)ColorSupported.NOT_SUPPORTED;
            }
        }else if(category == PrinterName.class){
            return (T)getPrinterName();
        } else {
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
        // TODO: Seems to be more accurate than settings.get_SupportsColor(), which doesn't work for CutePDF
        if(settings.get_DefaultPageSettings().get_Color()){
            attrs.add(ColorSupported.SUPPORTED);
        }else{
            attrs.add(ColorSupported.NOT_SUPPORTED);
        }

        return AttributeSetUtilities.unmodifiableView(attrs);
    }


    @Override
    public Object getDefaultAttributeValue(Class<? extends Attribute> category){
    	if (category == null) {
    		throw new NullPointerException("category must not be null");
    	}
    	if ( !Attribute.class.isAssignableFrom( category ) ) { 
    		throw new IllegalArgumentException( category +" has to be an " + Attribute.class.getName() );
    	}
    	if ( !isAttributeCategorySupported( category ) ) {
    		return null;
    	}
    	if (category == Copies.class) {
    		short copies = settings.get_Copies();
    		return new Copies( copies > 0 ? copies : 1 );
    	} else if (category == Chromaticity.class) {
    		// NOTE: this works for CutePDF, settings.get_SupportsColor() does not
    		return settings.get_DefaultPageSettings().get_Color() ? Chromaticity.COLOR : Chromaticity.MONOCHROME;
    	} else if (category == JobName.class) {
    		return new JobName( "Java Printing", null ); // TODO this is Java-Default, use another one for IKVM?
    	} else if (category == OrientationRequested.class) {
    		return settings.get_DefaultPageSettings().get_Landscape() ? OrientationRequested.LANDSCAPE : OrientationRequested.PORTRAIT; 
    	} else if (category == PageRanges.class) {
    		return new PageRanges(1, Integer.MAX_VALUE );
    	} else if (category == Media.class) {
            int rawKind = settings.get_DefaultPageSettings().get_PaperSize().get_RawKind();
    		if( rawKind > MEDIA_NAMES.length || rawKind < 1 || MEDIA_NAMES[ rawKind - 1 ] == null ){ // custom page format
    			return findMatchingMedia( settings.get_DefaultPageSettings().get_PaperSize() );
    		} else {
    			return MEDIA_NAMES[ rawKind - 1 ];
    		}
    	} else if (category == MediaPrintableArea.class) {
    		RectangleF area = settings.get_DefaultPageSettings().get_PrintableArea();
    		// get_PrintableArea is in 1/100 inch, see http://msdn.microsoft.com/de-de/library/system.drawing.printing.pagesettings.printablearea(v=VS.90).aspx
    		return new MediaPrintableArea(area.get_X()/100, area.get_Y()/100, area.get_Width()/100, area.get_Height()/100, MediaPrintableArea.INCH);
    	} else if (category == Destination.class) {
    		String path = "out.prn";
    		try {
    			return new Destination( ( new File( path ) ).toURI() );
    		} catch (SecurityException se) {
    			try {
    				return new Destination( new URI( "file:" + path) );
    			} catch (URISyntaxException e) {
    				return null;
    			}
    		}
    	} else if (category == Sides.class) {
    		switch( settings.get_Duplex().Value ){
    			case cli.System.Drawing.Printing.Duplex.Default: // MSDN: 'The printer's default duplex setting.' - what ever that might be
    			case cli.System.Drawing.Printing.Duplex.Simplex:
    				return Sides.ONE_SIDED;
    			case cli.System.Drawing.Printing.Duplex.Horizontal:
    				return Sides.TWO_SIDED_LONG_EDGE;
    			case cli.System.Drawing.Printing.Duplex.Vertical:
    				return Sides.TWO_SIDED_SHORT_EDGE;
    		}
    	} else if (category == PrinterResolution.class) {
    		cli.System.Drawing.Printing.PrinterResolution pRes = settings.get_DefaultPageSettings().get_PrinterResolution();
    		int xRes = pRes.get_X();
    		int yRes = pRes.get_Y();
            if ((xRes <= 0) || (yRes <= 0)) {
                int res = (yRes > xRes) ? yRes : xRes;
                if (res > 0) {
                 return new PrinterResolution(res, res, PrinterResolution.DPI);
                }
            }
            else {
               return new PrinterResolution(xRes, yRes, PrinterResolution.DPI);
            }
    	} else if (category == ColorSupported.class) {
    		if ( settings.get_SupportsColor() ) {
    			return ColorSupported.SUPPORTED;
    		} else {
    			return ColorSupported.NOT_SUPPORTED;
    		}
    	} else if( category == PrintQuality.class ){
			cli.System.Drawing.Printing.PrinterResolutionKind kind = settings.get_DefaultPageSettings().get_PrinterResolution().get_Kind();
			switch (kind.Value) {
			case cli.System.Drawing.Printing.PrinterResolutionKind.High:
				return PrintQuality.HIGH;
			case cli.System.Drawing.Printing.PrinterResolutionKind.Medium:
			case cli.System.Drawing.Printing.PrinterResolutionKind.Low:
				return PrintQuality.NORMAL;
			case cli.System.Drawing.Printing.PrinterResolutionKind.Draft:
				return PrintQuality.DRAFT;
			}
    	} else if (category == RequestingUserName.class) {
    		try{
    			return new RequestingUserName( System.getProperty("user.name", ""), null);
    		} catch( SecurityException e ){
    			return new RequestingUserName( "", null);
    		}
    	} else if (category == SheetCollate.class){
    		return settings.get_Collate() ? SheetCollate.COLLATED : SheetCollate.UNCOLLATED;
    	} else if (category == Fidelity.class) {
    		return Fidelity.FIDELITY_FALSE;
    	}
        return null;
    }


    @Override
    public ServiceUIFactory getServiceUIFactory(){
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
    public Object getSupportedAttributeValues(Class<? extends Attribute> category, DocFlavor flavor, AttributeSet attributes){
    	if ( category == null || !Attribute.class.isAssignableFrom( category ) ) { 
    		throw new IllegalArgumentException( "The category '" + category + "' is not an Attribute" );
    	}
        if( !isAttributeCategorySupported(category) ){
        	return null;
        }
        if (category == JobName.class || category == RequestingUserName.class || category == ColorSupported.class 
        		|| category == Destination.class ) {
        	return getDefaultAttributeValue(category);
        }
        if( category == Copies.class ){
        	return new CopiesSupported(1, settings.get_MaximumCopies() );
        }
        if( category == Media.class ){
        	PaperSizeCollection sizes = settings.get_PaperSizes();
        	List<Media> medias = new ArrayList<Media>();
        	for( int i = 0; i < sizes.get_Count(); i++ ){
        		PaperSize media = sizes.get_Item(i);				
        		MediaSizeName mediaName = findMatchingMedia( sizes.get_Item(i) );
    			if( mediaName != null 
    					&& !medias.contains( mediaName )){ // slow but better than creating a HashSet here
    				medias.add( mediaName);
        		}
        	}
        	// add media trays
        	MediaTray[] trays = getMediaTrays();
        	for( MediaTray tray : trays ){
        		medias.add( tray );
        	}
        	return medias.size() > 0 ? medias.toArray( new Media[medias.size() ] ) : null;
        }
        if (category == PageRanges.class) {
        	if (flavor == null|| flavor.equals(DocFlavor.SERVICE_FORMATTED.PAGEABLE)
        			|| flavor.equals(DocFlavor.SERVICE_FORMATTED.PRINTABLE)) {
        		PageRanges[] arr = new PageRanges[1];
        		arr[0] = new PageRanges(1, Integer.MAX_VALUE);
        		return arr;
        	} else {
        		return null;
        	}
        }
        if (category == Fidelity.class) {
        	return new Fidelity[]{ Fidelity.FIDELITY_FALSE, Fidelity.FIDELITY_TRUE};
        }
        if (category == PrintQuality.class) {
        	return new PrintQuality[]{ PrintQuality.DRAFT, PrintQuality.HIGH, PrintQuality.NORMAL };
        }
        
        boolean printPageAble = flavor == null|| flavor.equals(DocFlavor.SERVICE_FORMATTED.PAGEABLE)
									|| flavor.equals(DocFlavor.SERVICE_FORMATTED.PRINTABLE);
        if (category == Sides.class) {
        	if ( printPageAble ) {
        		return new Sides[]{ Sides.ONE_SIDED, Sides.TWO_SIDED_LONG_EDGE, Sides.TWO_SIDED_SHORT_EDGE};
        	} else {
        		return null;
        	}
        }
        if (category == SheetCollate.class) {
        	if ( printPageAble ) {
        		return new SheetCollate[]{ SheetCollate.COLLATED, SheetCollate.UNCOLLATED} ;
        	} else {
        		return null;
        	}
        }
        boolean imageBased =  printPageAble || flavor.equals(DocFlavor.INPUT_STREAM.GIF)
									        || flavor.equals(DocFlavor.INPUT_STREAM.JPEG)
									        || flavor.equals(DocFlavor.INPUT_STREAM.PNG)
									        || flavor.equals(DocFlavor.BYTE_ARRAY.GIF)
									        || flavor.equals(DocFlavor.BYTE_ARRAY.JPEG)
									        || flavor.equals(DocFlavor.BYTE_ARRAY.PNG)
									        || flavor.equals(DocFlavor.URL.GIF)
									        || flavor.equals(DocFlavor.URL.JPEG)
									        || flavor.equals(DocFlavor.URL.PNG);
        if (category == OrientationRequested.class) {
        	if( imageBased ){
        		return new OrientationRequested[]{ OrientationRequested.PORTRAIT, OrientationRequested.LANDSCAPE, OrientationRequested.REVERSE_LANDSCAPE };
        	} else {
        		return null;
        	}
        }
        if (category == Chromaticity.class) {
        	if( imageBased ){
        		if( settings.get_DefaultPageSettings().get_Color() ){
        			return new Chromaticity[]{ Chromaticity.MONOCHROME, Chromaticity.COLOR };
        		} else {
        			return new Chromaticity[]{ Chromaticity.MONOCHROME };
        		}
        	} else {
        		return null;
        	}
        }
        return null;
    }
    
    private MediaTray[] getMediaTrays(){
    	if( mediaTrays  != null ){
    		// the print service is a singleton per printer so we only do this once
    		return mediaTrays;
    	}
    	PaperSourceCollection trays = settings.get_PaperSources();
    	int count = trays.get_Count();
    	List<MediaTray> trayList = new ArrayList<MediaTray>();
    	for( int i=0; i < count; i++ ){
    		PaperSource tray = trays.get_Item(i);
    		MediaTray javaTray = getDefaultTray(tray);
    		if( javaTray != null ){
    			trayList.add( javaTray );
    		} else {
    			trayList.add( new NetMediaTray( tray ) );
    		}
    	}
    	mediaTrays = trayList.toArray( new MediaTray[trayList.size()]);
    	return mediaTrays;
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
    	if ( category == null || !Attribute.class.isAssignableFrom( category ) ) { 
    		throw new IllegalArgumentException( "The category '" + category + "' is not an Attribute" );
    	}
        Class<?>[] supported = getSupportedAttributeCategories();
        for( int i=0; i < supported.length; i++ ){
        	if( category == supported[i] ){
        		return true;
        	}
        }
        return false;
    }


    private boolean isPostScriptFlavor(DocFlavor flavor) {
        if (flavor.equals(DocFlavor.BYTE_ARRAY.POSTSCRIPT) ||
            flavor.equals(DocFlavor.INPUT_STREAM.POSTSCRIPT) ||
            flavor.equals(DocFlavor.URL.POSTSCRIPT)) {
            return true;
        }
        else {
            return false;
        }
    }

    private boolean isPSDocAttr(Class category) {
        if (category == OrientationRequested.class || category == Copies.class) {
                return true;
        }
        else {
            return false;
        }
    }

    private boolean isAutoSense(DocFlavor flavor) {
        if (flavor.equals(DocFlavor.BYTE_ARRAY.AUTOSENSE) ||
            flavor.equals(DocFlavor.INPUT_STREAM.AUTOSENSE) ||
            flavor.equals(DocFlavor.URL.AUTOSENSE)) {
            return true;
        }
        else {
            return false;
        }
    }

    @Override
    public boolean isAttributeValueSupported(Attribute attr, DocFlavor flavor, AttributeSet attributes){
        if (attr == null) {
            throw new NullPointerException("null attribute");
        }
        Class category = attr.getCategory();
        if (flavor != null) {
            if (!isDocFlavorSupported(flavor)) {
                throw new IllegalArgumentException(flavor +
                                                   " is an unsupported flavor");
                // if postscript & category is already specified within the PostScript data
                // we return false
            } else if (isAutoSense(flavor) || (isPostScriptFlavor(flavor) &&
                       (isPSDocAttr(category)))) {
                return false;
            }
        }

        if (!isAttributeCategorySupported(category)) {
            return false;
        }
        else if (category == Chromaticity.class) {
            if ((flavor == null) ||
                flavor.equals(DocFlavor.SERVICE_FORMATTED.PAGEABLE) ||
                flavor.equals(DocFlavor.SERVICE_FORMATTED.PRINTABLE) ||
                flavor.equals(DocFlavor.BYTE_ARRAY.GIF) ||
                flavor.equals(DocFlavor.INPUT_STREAM.GIF) ||
                flavor.equals(DocFlavor.URL.GIF) ||
                flavor.equals(DocFlavor.BYTE_ARRAY.JPEG) ||
                flavor.equals(DocFlavor.INPUT_STREAM.JPEG) ||
                flavor.equals(DocFlavor.URL.JPEG) ||
                flavor.equals(DocFlavor.BYTE_ARRAY.PNG) ||
                flavor.equals(DocFlavor.INPUT_STREAM.PNG) ||
                flavor.equals(DocFlavor.URL.PNG)) {
                if (settings.get_DefaultPageSettings().get_Color()) {
                    return true;
                } else {
                    return attr == Chromaticity.MONOCHROME;
                }
            } else {
                return false;
            }
        } else if (category == Copies.class) {
            return ((Copies)attr).getValue() >= 1 && ((Copies)attr).getValue() <= settings.get_MaximumCopies();

        } else if (category == Destination.class) {
            URI uri = ((Destination)attr).getURI();
            if ("file".equals(uri.getScheme()) &&
                !(uri.getSchemeSpecificPart().equals(""))) {
                return true;
            } else {
            return false;
            }

        } else if (category == Media.class) {
            Media[] medias = (Media[])getSupportedAttributeValues( Media.class, flavor, attributes );
            if( medias != null ) {
                return Arrays.asList( medias ).contains( attr );
            }

        } else if (category == MediaPrintableArea.class) {
            //TODO
            return true;

        } else if (category == SunAlternateMedia.class) {
            Media media = ((SunAlternateMedia)attr).getMedia();
            return isAttributeValueSupported(media, flavor, attributes);

        } else if (category == PageRanges.class ||
                   category == SheetCollate.class ||
                   category == Sides.class) {
            if (flavor != null &&
                !(flavor.equals(DocFlavor.SERVICE_FORMATTED.PAGEABLE) ||
                flavor.equals(DocFlavor.SERVICE_FORMATTED.PRINTABLE))) {
                return false;
            }
        } else if (category == PrinterResolution.class) {
            if (attr instanceof PrinterResolution) {
                int[] jRes = ((PrinterResolution)attr).getResolution( PrinterResolution.DPI );
                PrinterResolutionCollection resolutions = settings.get_PrinterResolutions();
                for( int i=0; i< resolutions.get_Count(); i++ ) {
                    cli.System.Drawing.Printing.PrinterResolution nRes = resolutions.get_Item( i );
                    if( nRes.get_X() == jRes[0] && nRes.get_Y() == jRes[1] ) {
                        return true;
                    }
                }
                return false;
            }
        } else if (category == OrientationRequested.class) {
            if (attr == OrientationRequested.REVERSE_PORTRAIT ||
                (flavor != null) &&
                !(flavor.equals(DocFlavor.SERVICE_FORMATTED.PAGEABLE) ||
                flavor.equals(DocFlavor.SERVICE_FORMATTED.PRINTABLE) ||
                flavor.equals(DocFlavor.INPUT_STREAM.GIF) ||
                flavor.equals(DocFlavor.INPUT_STREAM.JPEG) ||
                flavor.equals(DocFlavor.INPUT_STREAM.PNG) ||
                flavor.equals(DocFlavor.BYTE_ARRAY.GIF) ||
                flavor.equals(DocFlavor.BYTE_ARRAY.JPEG) ||
                flavor.equals(DocFlavor.BYTE_ARRAY.PNG) ||
                flavor.equals(DocFlavor.URL.GIF) ||
                flavor.equals(DocFlavor.URL.JPEG) ||
                flavor.equals(DocFlavor.URL.PNG))) {
                return false;
            }

        } else if (category == ColorSupported.class) {
            boolean isColorSup = settings.get_DefaultPageSettings().get_Color();
            if  ((!isColorSup && (attr == ColorSupported.SUPPORTED)) ||
                (isColorSup && (attr == ColorSupported.NOT_SUPPORTED))) {
                return false;
            }
        }
        return true;
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
    
    /**
     * Tries to find a matching {@link MediaSizeName} for a paper by it's size
     * @param paper
     * @return
     */
    private MediaSizeName findMatchingMedia( PaperSize paper ){
    	int rawKind = paper.get_RawKind();
		if( rawKind > 0 && rawKind <= MEDIA_NAMES.length ){
    		// match to predefined size
    		MediaSizeName media = MEDIA_NAMES[ rawKind - 1 ];
    		if( media != null ) {
    			return media;
    		}
    	}
    	int x = paper.get_Width() * INCH100_TO_MYM;
    	int y = paper.get_Height() * INCH100_TO_MYM;
    	if( x > y ){ // MediaSizes are always portrait!
    		int tmp = x;
    		x = y;
    		y = tmp;
    	}
    	for( MediaSizeName name : MEDIA_NAMES ){
    		MediaSize media = MediaSize.getMediaSizeForName(name);
    		if( media != null ){
    			if( Math.abs( x - media.getX(1) ) < MATCH_DIFF && Math.abs( y - media.getY(1) ) < MATCH_DIFF ){
    				return name;
    			}
    		}
    	}
    	MediaSizeName media = CUSTOM_MEDIA_NAME.get(paper.get_PaperName());
		if (media == null) {
			media = new CustomMediaSizeName(paper.get_PaperName());
			CUSTOM_MEDIA_NAME.put(paper.get_PaperName(), media);
			new MediaSize( x, y, MediaSize.INCH/100, media);
		}
    	return media;
    }
    
    /**
     * Returns the Java-default {@link MediaTray} for a paper source. This is required since these default
     * trays are public constants which can be used without checking for the actually present media trays 
     * @param source the .NET paper source to get the predefined source for
     * @return the media tray or null, in case there is no mapping for the paper source
     */
    private MediaTray getDefaultTray( PaperSource source ){
    	// convert from .NET kind to java's pre defined MediaTrays
    	switch( source.get_RawKind() ){
    		case 1 : return MediaTray.TOP;
    		case 2 : return MediaTray.BOTTOM;
    		case 3 : return MediaTray.MIDDLE;
    		case 4 : return MediaTray.MANUAL;
    		case 5 : return MediaTray.ENVELOPE;
    		case 6 : return Win32MediaTray.ENVELOPE_MANUAL;
    		case 7 : return Win32MediaTray.AUTO;
    		case 8 : return Win32MediaTray.TRACTOR;
    		case 9 : return Win32MediaTray.SMALL_FORMAT;
    		case 10 : return Win32MediaTray.LARGE_FORMAT;
    		case 11 : return MediaTray.LARGE_CAPACITY;
    		case 14 : return MediaTray.MAIN;
    		case 15 : return Win32MediaTray.FORMSOURCE;
    		// FIXME which PaperSourceKind is MediaTray.SIDE ???
    	}
    	return null;
    }
    
    /**
     * Returns the .NET {@link PaperSource} for a media tray. This will be done either by mapping or
     * directly in case the tray is a {@link NetMediaTray}
     * @param tray the tray to get the paper source for, must not be null
     * @return the selected {@link PaperSource} or null, in case there is no matching {@link PaperSource}
     */
    public PaperSource getPaperSourceForTray( MediaTray tray ){
    	if( tray instanceof NetMediaTray ){
			return ((NetMediaTray)tray).getPaperSource( this );
		}
    	// try to find the appropriate paper source for the Java-Defined tray
    	PaperSourceCollection trays = settings.get_PaperSources();
    	int count = trays.get_Count();
    	for( int i=0; i < count; i++ ){
    		PaperSource paperSource = trays.get_Item(i);
			if( getDefaultTray( paperSource ) == tray ){
    			return paperSource;
    		}
    	}
    	return null;
    }
    
    public static class NetMediaTray extends MediaTray{
    	
		private static final long serialVersionUID = 1L;

		/** Not really used but required by the EnumSyntax super class */
    	private static AtomicInteger idCounter = new AtomicInteger(8);
    	
    	private int rawKind;
    	private String name;
    	private transient PaperSource netSource;

		public NetMediaTray( PaperSource netSource ) {
			super( idCounter.getAndIncrement() );
			this.rawKind = netSource.get_RawKind();
			this.name = netSource.get_SourceName();
			this.netSource = netSource;
		}
    	
		public PaperSource getPaperSource( Win32PrintService service ){
			if( netSource == null ){
				PaperSourceCollection sources = service.settings.get_PaperSources();
				int count = sources.get_Count();
				for( int i=0; i < count; i++ ){
					PaperSource source = sources.get_Item(i);
					if( source.get_RawKind() == rawKind ){
						netSource = source;
						break;
					}
				}
			}
			return netSource;
		}
		
		@Override
		public String toString() {
			return netSource != null ? netSource.get_SourceName() : name;
		}
    }
}
