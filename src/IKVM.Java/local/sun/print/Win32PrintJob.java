/*
 * Copyright (c) 2000, 2013, Oracle and/or its affiliates. All rights reserved.
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

import java.awt.Color;
import java.awt.Graphics;
import java.awt.Graphics2D;
import java.awt.geom.AffineTransform;
import java.awt.image.BufferedImage;
import java.awt.print.PageFormat;
import java.awt.print.Pageable;
import java.awt.print.Paper;
import java.awt.print.Printable;
import java.awt.print.PrinterException;
import java.awt.print.PrinterJob;
import java.io.File;
import java.io.FileNotFoundException;
import java.io.FileOutputStream;
import java.io.IOException;
import java.io.InputStream;
import java.net.URI;
import java.net.URL;
import java.util.ArrayList;
import java.util.List;
import java.util.TreeSet;

import javax.print.CancelablePrintJob;
import javax.print.Doc;
import javax.print.DocFlavor;
import javax.print.PrintException;
import javax.print.PrintService;
import javax.print.attribute.Attribute;
import javax.print.attribute.AttributeSetUtilities;
import javax.print.attribute.DocAttributeSet;
import javax.print.attribute.HashPrintJobAttributeSet;
import javax.print.attribute.HashPrintRequestAttributeSet;
import javax.print.attribute.PrintJobAttribute;
import javax.print.attribute.PrintJobAttributeSet;
import javax.print.attribute.PrintRequestAttribute;
import javax.print.attribute.PrintRequestAttributeSet;
import javax.print.attribute.standard.Chromaticity;
import javax.print.attribute.standard.Copies;
import javax.print.attribute.standard.Destination;
import javax.print.attribute.standard.DocumentName;
import javax.print.attribute.standard.Fidelity;
import javax.print.attribute.standard.JobName;
import javax.print.attribute.standard.JobOriginatingUserName;
import javax.print.attribute.standard.Media;
import javax.print.attribute.standard.MediaPrintableArea;
import javax.print.attribute.standard.MediaSize;
import javax.print.attribute.standard.MediaSizeName;
import javax.print.attribute.standard.MediaTray;
import javax.print.attribute.standard.OrientationRequested;
import javax.print.attribute.standard.PageRanges;
import javax.print.attribute.standard.PrinterIsAcceptingJobs;
import javax.print.attribute.standard.PrinterState;
import javax.print.attribute.standard.PrinterStateReason;
import javax.print.attribute.standard.PrinterStateReasons;
import javax.print.attribute.standard.RequestingUserName;
import javax.print.attribute.standard.SheetCollate;
import javax.print.event.PrintJobAttributeListener;
import javax.print.event.PrintJobEvent;
import javax.print.event.PrintJobListener;

import sun.awt.windows.WPrinterJob;
import sun.print.Win32PrintService.NetMediaTray;

import cli.System.Drawing.Printing.*;

/**
 * @author Volker Berlin
 */
public class Win32PrintJob implements CancelablePrintJob{

    private ArrayList<PrintJobListener> jobListeners;

    private ArrayList<PrintJobAttributeListener> attrListeners;

    private ArrayList<PrintJobAttributeSet> listenedAttributeSets;

    private final Win32PrintService service;

    
    private boolean printing;
    private boolean printReturned;

    private PrintRequestAttributeSet reqAttrSet;

    private PrintJobAttributeSet jobAttrSet;

    private PrinterJob job = new WPrinterJob();
    private Doc doc;
    private String mDestination;

    /* these variables used globally to store reference to the print
     * data retrieved as a stream. On completion these are always closed
     * if non-null.
     */
    private InputStream instream;

    /* default values overridden by those extracted from the attributes */
    private boolean fidelity;
    private boolean printColor;
    private String jobName = "Java Printing";
    private int copies;
    private MediaSizeName mediaName;
    private MediaSize     mediaSize;
    private OrientationRequested orient;
    
    private final PrintPeer peer;
    private PrinterException printerException;

	private PageNumberConverter pageRanges;

	private MediaTray mediaTray;

    
    Win32PrintJob(Win32PrintService service, PrintPeer peer){
        this.service = service;
        this.peer = peer;
    }


    @Override
    public PrintService getPrintService(){
        return service;
    }


    @Override
    public PrintJobAttributeSet getAttributes(){
        synchronized(this){
            if(jobAttrSet == null){
                /* just return an empty set until the job is submitted */
                PrintJobAttributeSet jobSet = new HashPrintJobAttributeSet();
                return AttributeSetUtilities.unmodifiableView(jobSet);
            }else{
                return jobAttrSet;
            }
        }
    }


    @Override
    public void addPrintJobListener(PrintJobListener listener){
        synchronized(this){
            if(listener == null){
                return;
            }
            if(jobListeners == null){
                jobListeners = new ArrayList<PrintJobListener>();
            }
            jobListeners.add(listener);
        }
    }


    @Override
    public void removePrintJobListener(PrintJobListener listener){
        synchronized(this){
            if(listener == null || jobListeners == null){
                return;
            }
            jobListeners.remove(listener);
            if(jobListeners.isEmpty()){
                jobListeners = null;
            }
        }
    }


    /* Closes any stream already retrieved for the data.
     * We want to avoid unnecessarily asking the Doc to create a stream only
     * to get a reference in order to close it because the job failed.
     * If the representation class is itself a "stream", this
     * closes that stream too.
     */
    private void closeDataStreams() {
        // TODO
    }

    private void notifyEvent(int reason) {

        /* since this method should always get called, here's where
         * we will perform the clean up of any data stream supplied.
         */
        switch (reason) {
            case PrintJobEvent.DATA_TRANSFER_COMPLETE:
            case PrintJobEvent.JOB_CANCELED :
            case PrintJobEvent.JOB_FAILED :
            case PrintJobEvent.NO_MORE_EVENTS :
            case PrintJobEvent.JOB_COMPLETE :
                closeDataStreams();
        }

        synchronized (this) {
            if (jobListeners != null) {
                PrintJobListener listener;
                PrintJobEvent event = new PrintJobEvent(this, reason);
                for (int i = 0; i < jobListeners.size(); i++) {
                    listener = jobListeners.get(i);
                    switch (reason) {

                        case PrintJobEvent.JOB_COMPLETE :
                            listener.printJobCompleted(event);
                            break;

                        case PrintJobEvent.JOB_CANCELED :
                            listener.printJobCanceled(event);
                            break;

                        case PrintJobEvent.JOB_FAILED :
                            listener.printJobFailed(event);
                            break;

                        case PrintJobEvent.DATA_TRANSFER_COMPLETE :
                            listener.printDataTransferCompleted(event);
                            break;

                        case PrintJobEvent.NO_MORE_EVENTS :
                            listener.printJobNoMoreEvents(event);
                            break;

                        default:
                            break;
                    }
                }
            }
       }
    }

    @Override
    public void addPrintJobAttributeListener(PrintJobAttributeListener listener, PrintJobAttributeSet attributes){
        synchronized(this){
            if(listener == null){
                return;
            }
            if(attrListeners == null){
                attrListeners = new ArrayList<PrintJobAttributeListener>();
                listenedAttributeSets = new ArrayList<PrintJobAttributeSet>();
            }
            attrListeners.add(listener);
            if(attributes == null){
                attributes = new HashPrintJobAttributeSet();
            }
            listenedAttributeSets.add(attributes);
        }
    }


    @Override
    public void removePrintJobAttributeListener(PrintJobAttributeListener listener){
        synchronized(this){
            if(listener == null || attrListeners == null){
                return;
            }
            int index = attrListeners.indexOf(listener);
            if(index == -1){
                return;
            }else{
                attrListeners.remove(index);
                listenedAttributeSets.remove(index);
                if(attrListeners.isEmpty()){
                    attrListeners = null;
                    listenedAttributeSets = null;
                }
            }
        }
    }


    @Override
    public void print(Doc doc, PrintRequestAttributeSet attributes) throws PrintException{

        synchronized(this){
            if(printing){
                throw new PrintException("already printing");
            }else{
                printing = true;
            }
        }

        PrinterState prnState = (PrinterState)service.getAttribute(PrinterState.class);
        if(prnState == PrinterState.STOPPED){
            PrinterStateReasons prnStateReasons = (PrinterStateReasons)service.getAttribute(PrinterStateReasons.class);
            if((prnStateReasons != null) && (prnStateReasons.containsKey(PrinterStateReason.SHUTDOWN))){
                throw new PrintException("PrintService is no longer available.");
            }
        }

        if((PrinterIsAcceptingJobs)(service.getAttribute(PrinterIsAcceptingJobs.class)) == PrinterIsAcceptingJobs.NOT_ACCEPTING_JOBS){
            throw new PrintException("Printer is not accepting job.");
        }

        this.doc = doc;
        /* check if the parameters are valid before doing much processing */
        DocFlavor flavor = doc.getDocFlavor();
        Object data;

        try{
            data = doc.getPrintData();
        }catch(IOException e){
            notifyEvent(PrintJobEvent.JOB_FAILED);
            throw new PrintException("can't get print data: " + e.toString());
        }

        if (data == null) {
            throw new PrintException("Null print data.");
        }

        if(flavor == null || (!service.isDocFlavorSupported(flavor))){
            notifyEvent(PrintJobEvent.JOB_FAILED);
            throw new PrintJobFlavorException("invalid flavor", flavor);
        }

        initializeAttributeSets(doc, attributes);

        getAttributeValues(flavor);

        String repClassName = flavor.getRepresentationClassName();

        if(flavor.equals(DocFlavor.INPUT_STREAM.GIF) || flavor.equals(DocFlavor.INPUT_STREAM.JPEG)
                || flavor.equals(DocFlavor.INPUT_STREAM.PNG) || flavor.equals(DocFlavor.BYTE_ARRAY.GIF)
                || flavor.equals(DocFlavor.BYTE_ARRAY.JPEG) || flavor.equals(DocFlavor.BYTE_ARRAY.PNG)){
            try{
                instream = doc.getStreamForBytes();
                if(instream == null){
                    notifyEvent(PrintJobEvent.JOB_FAILED);
                    throw new PrintException("No stream for data");
                }
                printableJob(new ImagePrinter(instream));
                service.wakeNotifier();
                return;
            }catch(ClassCastException cce){
                notifyEvent(PrintJobEvent.JOB_FAILED);
                throw new PrintException(cce);
            }catch(IOException ioe){
                notifyEvent(PrintJobEvent.JOB_FAILED);
                throw new PrintException(ioe);
            }
        }else if(flavor.equals(DocFlavor.URL.GIF) || flavor.equals(DocFlavor.URL.JPEG)
                || flavor.equals(DocFlavor.URL.PNG)){
            try{
                printableJob(new ImagePrinter((URL)data));
                service.wakeNotifier();
                return;
            }catch(ClassCastException cce){
                notifyEvent(PrintJobEvent.JOB_FAILED);
                throw new PrintException(cce);
            }
        }else if(repClassName.equals("java.awt.print.Pageable")){
            try{
                pageableJob((Pageable)doc.getPrintData());
                service.wakeNotifier();
                return;
            }catch(ClassCastException cce){
                notifyEvent(PrintJobEvent.JOB_FAILED);
                throw new PrintException(cce);
            }catch(IOException ioe){
                notifyEvent(PrintJobEvent.JOB_FAILED);
                throw new PrintException(ioe);
            }
        }else if(repClassName.equals("java.awt.print.Printable")){
            try{
                printableJob((Printable)doc.getPrintData());
                service.wakeNotifier();
                return;
            }catch(ClassCastException cce){
                notifyEvent(PrintJobEvent.JOB_FAILED);
                throw new PrintException(cce);
            }catch(IOException ioe){
                notifyEvent(PrintJobEvent.JOB_FAILED);
                throw new PrintException(ioe);
            }
        }else if(repClassName.equals("[B") || repClassName.equals("java.io.InputStream")
                || repClassName.equals("java.net.URL")){

            if(repClassName.equals("java.net.URL")){
                URL url = (URL)data;
                try{
                    instream = url.openStream();
                }catch(IOException e){
                    notifyEvent(PrintJobEvent.JOB_FAILED);
                    throw new PrintException(e.toString());
                }
            }else{
                try{
                    instream = doc.getStreamForBytes();
                }catch(IOException ioe){
                    notifyEvent(PrintJobEvent.JOB_FAILED);
                    throw new PrintException(ioe.toString());
                }
            }

            if(instream == null){
                notifyEvent(PrintJobEvent.JOB_FAILED);
                throw new PrintException("No stream for data");
            }

            if(mDestination != null){ // if destination attribute is set
                try{
                    FileOutputStream fos = new FileOutputStream(mDestination);
                    byte[] buffer = new byte[1024];
                    int cread;

                    while((cread = instream.read(buffer, 0, buffer.length)) >= 0){
                        fos.write(buffer, 0, cread);
                    }
                    fos.flush();
                    fos.close();
                }catch(FileNotFoundException fnfe){
                    notifyEvent(PrintJobEvent.JOB_FAILED);
                    throw new PrintException(fnfe.toString());
                }catch(IOException ioe){
                    notifyEvent(PrintJobEvent.JOB_FAILED);
                    throw new PrintException(ioe.toString());
                }
                notifyEvent(PrintJobEvent.DATA_TRANSFER_COMPLETE);
                notifyEvent(PrintJobEvent.JOB_COMPLETE);
                service.wakeNotifier();
                return;
            }

            notifyEvent(PrintJobEvent.JOB_FAILED);
            throw new PrintException("Print job failed. IKVM does not support raw data currently.");
        }else{
            notifyEvent(PrintJobEvent.JOB_FAILED);
            throw new PrintException("unrecognized class: " + repClassName);
        }
    }
    
    
    public void printableJob(Printable printable) throws PrintException {
        throw new PrintException("Win32PrintJob.printableJob() not implemented:");
    }

    public void pageableJob(Pageable pageable) throws PrintException {
    	// a general hint for this code here: The PrintJob implementation (WPrinterJob) in the Oracle
    	// JRE implements SunPrinterJobService - which we don't here. Unfortunately this is used as a hint
    	// to distinguish between the build-in printer service and custom printer services. So for the RasterPrinterJob
    	// this implementation of the Win32PrintJob is a custom print job! That's why several methods of
    	// RasterPrintJob(which only apply to the SunPrinterJobService otherwise) had to be moved here.
    	// This includes the attribute resolving and the media patch for instance.
        try {
            PrintDocument printDocument = createPrintDocument( );
            pageable = patchMedia( pageable );
    
            printDocument.add_QueryPageSettings(new QueryPageSettingsEventHandler(new QueryPage( pageable, printColor, mediaTray ) ) );
            printDocument.add_PrintPage(new PrintPageEventHandler(new PrintPage(pageable)));
            printDocument.Print();
            if(printerException != null){
                throw printerException;
            }
            //TODO throw exception on Cancel
            notifyEvent(PrintJobEvent.DATA_TRANSFER_COMPLETE);
            return;
        } catch (PrinterException pe) {
            notifyEvent(PrintJobEvent.JOB_FAILED);
            throw new PrintException(pe);
        } finally {
            printReturned = true;
            notifyEvent(PrintJobEvent.NO_MORE_EVENTS);
        }
    }
    
    
    private PrintDocument createPrintDocument() throws PrintException{
        PrintDocument printDocument = new PrintDocument();
        PrinterSettings settings = printDocument.get_PrinterSettings();
        settings.set_PrinterName( service.getName() );
        if( !settings.get_IsValid() ){
        	throw new PrintException("Printer name ''" + service.getName() + "' is invalid.");
        }
        
        if( jobName != null ){
        	printDocument.set_DocumentName( jobName );
        }
        printDocument.get_DefaultPageSettings().set_Color(printColor);
        
        Attribute destination = reqAttrSet.get(Destination.class);
        if(destination instanceof Destination){
        	File destFile = new File(((Destination)destination).getURI());
            settings.set_PrintFileName(destFile.getAbsolutePath());
            settings.set_PrintToFile(true);
        }
        
        settings.set_Copies((short)copies);
        boolean collated = false;
        if(copies > 1){
            Object collate = reqAttrSet.get(SheetCollate.class);
            if( collate == null ){
            	collate = service.getDefaultAttributeValue(SheetCollate.class);
            }
            collated = collate == SheetCollate.COLLATED;
            settings.set_Collate( collated );
        }
        Attribute pageRangeObj = reqAttrSet.get(PageRanges.class);
        if( pageRangeObj != null ){
        	int[][] ranges = ((PageRanges)pageRangeObj).getMembers();
        	if( ranges.length > 1 ){
    			settings.set_PrintRange( PrintRange.wrap( PrintRange.Selection ) );
        	} else {
        		if( ranges.length > 0 ){
        			settings.set_FromPage(ranges[0][0]);
        			settings.set_ToPage(ranges[0][1]);
        			settings.set_PrintRange( PrintRange.wrap( PrintRange.SomePages ) );
        		} // else allPages???
        	}
        } else {
        	settings.set_PrintRange( PrintRange.wrap( PrintRange.AllPages ) );
        }
        pageRanges = new PageNumberConverter( (PageRanges)pageRangeObj, copies, collated );
        return printDocument;
    }
    
    // copied from RasterPrintJob 
    // Since we don't implement SunPrinterJobService the hack in RasterPrintService which applies
    // the media format to the auto-generated OpenBook doesn't work here. So we have to modify
    // the page format of the OpenBook here - equal to the code in RasterPrintJob
    private Pageable patchMedia( Pageable pageable ){
    	/* OpenBook is used internally only when app uses Printable.
         * This is the case when we use the values from the attribute set.
         */
        Media media = (Media)reqAttrSet.get(Media.class);
        OrientationRequested orientReq = (OrientationRequested)reqAttrSet.get(OrientationRequested.class);
        MediaPrintableArea mpa = (MediaPrintableArea)reqAttrSet.get(MediaPrintableArea.class);

        if ((orientReq != null || media != null || mpa != null) && pageable instanceof OpenBook) {

            /* We could almost(!) use PrinterJob.getPageFormat() except
             * here we need to start with the PageFormat from the OpenBook :
             */
            Printable printable = pageable.getPrintable(0);
            PageFormat pf = (PageFormat)pageable.getPageFormat(0).clone();
            Paper paper = pf.getPaper();

            /* If there's a media but no media printable area, we can try
             * to retrieve the default value for mpa and use that.
             */
            if (mpa == null && media != null && service.isAttributeCategorySupported(MediaPrintableArea.class)) {
                Object mpaVals = service. getSupportedAttributeValues(MediaPrintableArea.class, null, reqAttrSet);
                if (mpaVals instanceof MediaPrintableArea[] && ((MediaPrintableArea[])mpaVals).length > 0) {
                    mpa = ((MediaPrintableArea[])mpaVals)[0];
                }
            }

            if (isSupportedValue(orientReq, reqAttrSet) || (!fidelity && orientReq != null)) {
                int orient;
                if (orientReq.equals(OrientationRequested.REVERSE_LANDSCAPE)) {
                    orient = PageFormat.REVERSE_LANDSCAPE;
                } else if (orientReq.equals(OrientationRequested.LANDSCAPE)) {
                    orient = PageFormat.LANDSCAPE;
                } else {
                    orient = PageFormat.PORTRAIT;
                }
                pf.setOrientation(orient);
            }

            if (isSupportedValue(media, reqAttrSet) || (!fidelity && media != null)) {
                if (media instanceof MediaSizeName) {
                    MediaSizeName msn = (MediaSizeName)media;
                    MediaSize msz = MediaSize.getMediaSizeForName(msn);
                    if (msz != null) {
                        float paperWid =  msz.getX(MediaSize.INCH) * 72.0f;
                        float paperHgt =  msz.getY(MediaSize.INCH) * 72.0f;
                        paper.setSize(paperWid, paperHgt);
                        if (mpa == null) {
                            paper.setImageableArea(72.0, 72.0, paperWid-144.0, paperHgt-144.0);
                        }
                    }
                }
            }

            if (isSupportedValue(mpa, reqAttrSet) || (!fidelity && mpa != null)) {
                float [] printableArea = mpa.getPrintableArea(MediaPrintableArea.INCH);
                for (int i=0; i < printableArea.length; i++) {
                    printableArea[i] = printableArea[i]*72.0f;
                }
                paper.setImageableArea(printableArea[0], printableArea[1], printableArea[2], printableArea[3]);
            }

            pf.setPaper(paper);
            pf = validatePage(pf);
            return new OpenBook(pf, printable);
        }
        return pageable;
    }
    
    // copied from RasterPrintJob to since we don't implement SunPrinterJobService
    /**
     * The passed in PageFormat is cloned and altered to be usable on
     * the PrinterJob's current printer.
     */
    private PageFormat validatePage(PageFormat page) {
        PageFormat newPage = (PageFormat)page.clone();
        Paper newPaper = new Paper();
        validatePaper(newPage.getPaper(), newPaper);
        newPage.setPaper(newPaper);

        return newPage;
    }
    
    // copied from RasterPrintJob to since we don't implement SunPrinterJobService
    /**
     * updates a Paper object to reflect the current printer's selected
     * paper size and imageable area for that paper size.
     * Default implementation copies settings from the original, applies
     * applies some validity checks, changes them only if they are
     * clearly unreasonable, then sets them into the new Paper.
     * Subclasses are expected to override this method to make more
     * informed decisons.
     */
    protected void validatePaper(Paper origPaper, Paper newPaper) {
        if (origPaper == null || newPaper == null) {
            return;
        } else {
            double wid = origPaper.getWidth();
            double hgt = origPaper.getHeight();
            double ix = origPaper.getImageableX();
            double iy = origPaper.getImageableY();
            double iw = origPaper.getImageableWidth();
            double ih = origPaper.getImageableHeight();

            /* Assume any +ve values are legal. Overall paper dimensions
             * take precedence. Make sure imageable area fits on the paper.
             */
            Paper defaultPaper = new Paper();
            wid = ((wid > 0.0) ? wid : defaultPaper.getWidth());
            hgt = ((hgt > 0.0) ? hgt : defaultPaper.getHeight());
            ix = ((ix > 0.0) ? ix : defaultPaper.getImageableX());
            iy = ((iy > 0.0) ? iy : defaultPaper.getImageableY());
            iw = ((iw > 0.0) ? iw : defaultPaper.getImageableWidth());
            ih = ((ih > 0.0) ? ih : defaultPaper.getImageableHeight());
            /* full width/height is not likely to be imageable, but since we
             * don't know the limits we have to allow it
             */
            if (iw > wid) {
                iw = wid;
            }
            if (ih > hgt) {
                ih = hgt;
            }
            if ((ix + iw) > wid) {
                ix = wid - iw;
            }
            if ((iy + ih) > hgt) {
                iy = hgt - ih;
            }
            newPaper.setSize(wid, hgt);
            newPaper.setImageableArea(ix, iy, iw, ih);
        }
    }
    
    // copied from RasterPrintJob
    /**
     * Checks whether a certain attribute value is valid for the current print service
     * @param attrval the attribute value 
     * @param attrset Set of printing attributes for a supposed job (both job-level attributes and document-level attributes), or null.
     * @return true if valid
     */
    protected boolean isSupportedValue(Attribute attrval, PrintRequestAttributeSet attrset) {
    	return (attrval != null && service != null && service.isAttributeValueSupported(attrval, DocFlavor.SERVICE_FORMATTED.PAGEABLE, attrset));
    }
    

    /*
     * There's some inefficiency here as the job set is created even though it may never be requested.
     */
    private synchronized void initializeAttributeSets(Doc doc, PrintRequestAttributeSet reqSet){

        reqAttrSet = new HashPrintRequestAttributeSet();
        jobAttrSet = new HashPrintJobAttributeSet();

        Attribute[] attrs;
        if(reqSet != null){
            reqAttrSet.addAll(reqSet);
            attrs = reqSet.toArray();
            for(int i = 0; i < attrs.length; i++){
                if(attrs[i] instanceof PrintJobAttribute){
                    jobAttrSet.add(attrs[i]);
                }
            }
        }

        DocAttributeSet docSet = doc.getAttributes();
        if(docSet != null){
            attrs = docSet.toArray();
            for(int i = 0; i < attrs.length; i++){
                if(attrs[i] instanceof PrintRequestAttribute){
                    reqAttrSet.add(attrs[i]);
                }
                if(attrs[i] instanceof PrintJobAttribute){
                    jobAttrSet.add(attrs[i]);
                }
            }
        }

        /* add the user name to the job */
        String userName = "";
        try{
            userName = System.getProperty("user.name");
        }catch(SecurityException se){
        }

        if(userName == null || userName.equals("")){
            RequestingUserName ruName = (RequestingUserName)reqSet.get(RequestingUserName.class);
            if(ruName != null){
                jobAttrSet.add(new JobOriginatingUserName(ruName.getValue(), ruName.getLocale()));
            }else{
                jobAttrSet.add(new JobOriginatingUserName("", null));
            }
        }else{
            jobAttrSet.add(new JobOriginatingUserName(userName, null));
        }

        /*
         * if no job name supplied use doc name (if supplied), if none and its a URL use that, else finally anything ..
         */
        if(jobAttrSet.get(JobName.class) == null){
            JobName jobName;
            if(docSet != null && docSet.get(DocumentName.class) != null){
                DocumentName docName = (DocumentName)docSet.get(DocumentName.class);
                jobName = new JobName(docName.getValue(), docName.getLocale());
                jobAttrSet.add(jobName);
            }else{
                String str = "JPS Job:" + doc;
                try{
                    Object printData = doc.getPrintData();
                    if(printData instanceof URL){
                        str = ((URL)(doc.getPrintData())).toString();
                    }
                }catch(IOException e){
                }
                jobName = new JobName(str, null);
                jobAttrSet.add(jobName);
            }
        }

        jobAttrSet = AttributeSetUtilities.unmodifiableView(jobAttrSet);
    }

    private void getAttributeValues(DocFlavor flavor) throws PrintException {

        if (reqAttrSet.get(Fidelity.class) == Fidelity.FIDELITY_TRUE) {
            fidelity = true;
        } else {
            fidelity = false;
        }
        
        Attribute chroma = reqAttrSet.get( Chromaticity.class );
        // TODO check whether supported by the print service
        printColor = chroma == Chromaticity.COLOR;

        Attribute newTray = reqAttrSet.get( Media.class );
        if( newTray instanceof MediaTray ){
        	mediaTray = (MediaTray)newTray;
        }
        // TODO check whether supported by the print service
        
        Class category;
        Attribute [] attrs = reqAttrSet.toArray();
        for (int i=0; i<attrs.length; i++) {
            Attribute attr = attrs[i];
            category = attr.getCategory();
            if (fidelity == true) {
                if (!service.isAttributeCategorySupported(category)) {
                    notifyEvent(PrintJobEvent.JOB_FAILED);
                    throw new PrintJobAttributeException(
                        "unsupported category: " + category, category, null);
                } else if
                    (!service.isAttributeValueSupported(attr, flavor, null)) {
                    notifyEvent(PrintJobEvent.JOB_FAILED);
                    throw new PrintJobAttributeException(
                        "unsupported attribute: " + attr, null, attr);
                }
            }
            if (category == Destination.class) {
              URI uri = ((Destination)attr).getURI();
              if (!"file".equals(uri.getScheme())) {
                notifyEvent(PrintJobEvent.JOB_FAILED);
                throw new PrintException("Not a file: URI");
              } else {
                try {
                  mDestination = (new File(uri)).getPath();
                } catch (Exception e) {
                  throw new PrintException(e);
                }
                // check write access
                SecurityManager security = System.getSecurityManager();
                if (security != null) {
                  try {
                    security.checkWrite(mDestination);
                  } catch (SecurityException se) {
                    notifyEvent(PrintJobEvent.JOB_FAILED);
                    throw new PrintException(se);
                  }
                }
              }
            } else if (category == JobName.class) {
                jobName = ((JobName)attr).getValue();
            } else if (category == Copies.class) {
                copies = ((Copies)attr).getValue();
            } else if (category == Media.class) {
              if (attr instanceof MediaSizeName) {
                    mediaName = (MediaSizeName)attr;
                    // If requested MediaSizeName is not supported,
                    // get the corresponding media size - this will
                    // be used to create a new PageFormat.
                    if (!service.isAttributeValueSupported(attr, flavor, null)) {
                        mediaSize = MediaSize.getMediaSizeForName(mediaName);
                    }
                }
            } else if (category == OrientationRequested.class) {
                orient = (OrientationRequested)attr;
            }
        }
    }


    @Override
    public void cancel() throws PrintException{
        synchronized(this){
            if(!printing){
                throw new PrintException("Job is not yet submitted.");
            }else if(job != null && !printReturned){
                job.cancel();
                notifyEvent(PrintJobEvent.JOB_CANCELED);
                return;
            }else{
                throw new PrintException("Job could not be cancelled.");
            }
        }
    }
    
    /**
     * Converts the Java 1/72 inch to .NET 1/100 inch
     * @param javaLength the java length in 1/72 inch
     * @return the .NET length in 1/100 inch
     */
    private static int java2netLength( int javaLength ){
    	return (int) Math.round( (double)(javaLength * 100) / 72d );
    }
    
    /**
     * Converts the Java 1/72 inch to .NET 1/100 inch
     * @param javaLength the java length in 1/72 inch
     * @return the .NET length in 1/100 inch
     */
    private static int java2netLength( double javaLength ){
    	return (int) Math.round( (javaLength * 100) / 72d );
    }

    
    private class PrintPage implements PrintPageEventHandler.Method{

        private final Pageable pageable;
        private int pageIndex;


        PrintPage(Pageable pageable){
            this.pageable = pageable;
        }


        @Override
        public void Invoke(Object paramObject, PrintPageEventArgs ev){

            try{
            	int realPage = pageRanges.getPageForIndex(pageIndex);
            	
                Printable printable = pageable.getPrintable(realPage);
                PageFormat pageFormat = pageable.getPageFormat(realPage);

                
                BufferedImage pBand = new BufferedImage(1, 1, BufferedImage.TYPE_3BYTE_BGR);
                Graphics2D imageGraphics = pBand.createGraphics();
                ((RasterPrinterJob)job).setGraphicsConfigInfo(imageGraphics.getTransform(), pageFormat.getImageableWidth(), pageFormat.getImageableHeight());
				PeekGraphics peekGraphics = new PeekGraphics(imageGraphics, job );

                /* 
                 * Because Sun is calling first with a PeekGraphics that we do it else for compatibility
                 */
                if(realPage == 0){
                    int pageResult = printable.print(peekGraphics, pageFormat, realPage);
                    if(pageResult != Printable.PAGE_EXISTS){
                        ev.set_HasMorePages(false);
                        ev.set_Cancel(true);
                        return;
                    }
                }
                Graphics2D printGraphics = peer.createGraphics(ev.get_Graphics());
            	Graphics2D g2d = ((Graphics2D)printGraphics);
            	int tX = (int) pageFormat.getWidth(); 
            	int tY = (int) pageFormat.getHeight();
            	// apply Java to .NET scaling (1/72 inch to 1/100 inch)
            	g2d.scale(100d/72d, 100d/72d);
            	// NOTE on Landscape printing:
            	// Setting landscape to true on the printer settings
            	// of a page already rotates the page! The orig. java code rotates the page itself,
            	// for .NET this is not required.
            	if( orient == OrientationRequested.REVERSE_LANDSCAPE){
            		g2d.translate( tX, tY );
            		g2d.rotate( Math.PI );
            	}

                printable.print(printGraphics, pageFormat, realPage);
                
                realPage = pageRanges.getPageForIndex(++pageIndex);
                if( realPage >= 0 ){
	                printable = pageable.getPrintable(realPage);
	                pageFormat = pageable.getPageFormat(realPage);
	                int pageResult = printable.print(peekGraphics, pageFormat, realPage);
                	ev.set_HasMorePages( pageResult == Printable.PAGE_EXISTS );
                } else {
                	ev.set_HasMorePages( false );
                }
            }catch(PrinterException ex){
                printerException = ex;
                ex.printStackTrace();
                ev.set_HasMorePages(false);
            }

        }

    }
    
    private class QueryPage implements QueryPageSettingsEventHandler.Method{

    	private final Pageable pageable;
        private int pageIndex;
		private final boolean printColor;
		private final MediaTray tray;


        QueryPage(Pageable pageable, boolean printColor, MediaTray tray  ){
            this.printColor = printColor;
			this.pageable = pageable;
			this.tray = tray;
        }
        
		@Override
		public void Invoke(Object source, QueryPageSettingsEventArgs e) {
			int realPage = pageRanges.getPageForIndex(pageIndex);
			// apply page settings to the current page
			PageFormat format = pageable.getPageFormat(realPage);
			PageSettings pageSettings = e.get_PageSettings();
			pageSettings.set_Color( printColor );
			PaperSource paperSource = service.getPaperSourceForTray( tray );
			if( paperSource != null ){
				pageSettings.set_PaperSource( paperSource );
			}
			
			PaperSize ps = new PaperSize();
			ps.set_Height( java2netLength( format.getHeight() ) );
			ps.set_Width( java2netLength( format.getWidth() ) );
			pageSettings.set_PaperSize( ps );
			
			Margins margins = new Margins();
			margins.set_Left( java2netLength( format.getImageableX() ) );
			margins.set_Top( java2netLength( format.getImageableY() ) );
			margins.set_Right( java2netLength(format.getWidth() - format.getImageableX() - format.getImageableWidth() ) );
			margins.set_Bottom( java2netLength(format.getHeight() - format.getImageableY() - format.getImageableHeight() ) );			
			pageSettings.set_Margins( margins );			
			pageIndex++;
		}
    }

    /**
     * Determines which logical page to print for a certain physical page 
     */
    public static class PageNumberConverter{        
        
        private List<Range> ranges;
        private int totalPages = -1;
        private final int copies;
        
        public PageNumberConverter( PageRanges pages, int copies, boolean collated ) {
            // NOTE: uncollated is handled by the printer driver! If we would handle that here, 
            // we would get copies^2 copies! 
            this.copies = collated ? copies : 1;
            if( pages != null && pages.getMembers() != null && pages.getMembers().length > 0 ){
                TreeSet<Range> rangesSort = new TreeSet<Range>();
                OUTER:
                for( int[] range : pages.getMembers() ){
                    Range r = new Range( range[0], range[1] + 1 ); // +1 to inlucde the uppre end
                    for( Range recent : rangesSort ){
                        if( recent.canMerge(r) ){
                            recent.merge(r);
                            continue OUTER;
                        }
                    }
                    rangesSort.add( r );
                }
                // finally merge
                Range recent = null;
                ranges = new ArrayList<Range>();
                for( Range r : rangesSort ){
                    if( recent != null && recent.canMerge( r ) ){
                        recent.merge( r );
                    } else {
                        ranges.add( r );
                        recent = r;
                    }
                }
                // calculate total pages, required for collated copies
                totalPages = 0;
                for( Range r : rangesSort ){
                    int diff = r.end - r.start;
                    totalPages += diff;
                }
            }
        }
        
        /**
         * Must be called in case the printable returns no-more-pages. Will return whether the print job 
         * will continue due to pending copies
         * @param index the current page index
         * @return if false, the print job will continue with copies, if true terminate the job
         */
        public boolean checkJobComplete( int index ){
            if( ranges != null ){
                return true;
            }
            if( totalPages < 0 ){
                // this is the first time, this was called for 'all-pages' so it's the total number
                totalPages = index;
            }
            return index > copies * totalPages;
        }
        
        /**
         * Returns which page to be printed for a certain page index
         * @param index the inex to be printed
         * @return the page number or -1, if there is no page for this index
         */
        public int getPageForIndex( int index ){
            if( index < 0 || ( totalPages >=0 && index >= copies * totalPages ) ){
                return -1;
            }
            if( ranges == null ){
                return totalPages >=0 ? index % totalPages : index;
            }
            int counter = 0;
            if( copies > 1 ){
                counter += (index / totalPages) * totalPages;
            }
            for( Range r : ranges ){
                int upper = counter + (r.end - r.start);
                if( index < upper ){
                    // so we're in the correct range
                    return r.start + ( index - counter ) - 1;
                } else {
                    counter = upper;
                }
            }
            return -1;
        }
        
        /**
         * A singular page range
         */
        private static class Range implements Comparable<Range> {

            public int start;
            public int end;
            
            public Range(int start, int end) {
                this.start = start;
                this.end = end;
            }
            
            /**
             * Checks whether the ranges intersect of have no gap in between
             * @param otherRange the range to be checked
             * @return true, if the ranges can be merged
             */
            public boolean canMerge( Range otherRange ){
                if( otherRange.end >= start && otherRange.end <= end ){
                    return true;
                }
                if( otherRange.start >= start && otherRange.start <= end ){
                    return true;
                }
                return false;
            }
            
            /**
             * Merges the other range into this range. Ignores the gap between the ranges if there is any
             * @param otherRange the range to be merged
             */
            public void merge( Range otherRange ){
                start = Math.min(start, otherRange.start);
                end = Math.max(end, otherRange.end);
            }
            
            public int compareTo(Range o) {             
                return start - o.start;
            }
        }
    }
}
