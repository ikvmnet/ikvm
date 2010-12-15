/*
  Copyright (C) 2009 Volker Berlin (i-net software)
  Copyright (C) 2010 Karsten Heinrich (i-net software)

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

import java.awt.Graphics;
import java.awt.Graphics2D;
import java.awt.GraphicsConfiguration;
import java.awt.HeadlessException;
import java.awt.image.BufferedImage;
import java.awt.print.PageFormat;
import java.awt.print.Pageable;
import java.awt.print.Printable;
import java.awt.print.PrinterException;
import java.awt.print.PrinterJob;
import java.io.BufferedInputStream;
import java.io.File;
import java.io.FileNotFoundException;
import java.io.FileOutputStream;
import java.io.IOException;
import java.io.InputStream;
import java.net.URI;
import java.net.URL;
import java.util.ArrayList;

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
import javax.print.attribute.standard.Copies;
import javax.print.attribute.standard.Destination;
import javax.print.attribute.standard.DocumentName;
import javax.print.attribute.standard.Fidelity;
import javax.print.attribute.standard.JobName;
import javax.print.attribute.standard.JobOriginatingUserName;
import javax.print.attribute.standard.Media;
import javax.print.attribute.standard.MediaSize;
import javax.print.attribute.standard.MediaSizeName;
import javax.print.attribute.standard.OrientationRequested;
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
import sun.reflect.generics.reflectiveObjects.NotImplementedException;

import cli.System.Drawing.Printing.*;

/**
 * @author Volker Berlin
 */
public class Win32PrintJob implements CancelablePrintJob{

    private ArrayList<PrintJobListener> jobListeners;

    private ArrayList<PrintJobAttributeListener> attrListeners;

    private ArrayList<PrintJobAttributeSet> listenedAttributeSets;

    private final Win32PrintService service;

    private boolean fidelity;
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
    private String jobName = "Java Printing";
    private int copies;
    private MediaSizeName mediaName;
    private MediaSize     mediaSize;
    private OrientationRequested orient;
    
    private final PrintPeer peer;
    private PrinterException printerException;
    
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
        try {
            PrintDocument printDocument = createPrintDocument();
    
            //TODO Attribute set in printDocument
            printDocument.add_QueryPageSettings(new QueryPageSettingsEventHandler(new QueryPage( pageable ) ) );
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
    
    
    private PrintDocument createPrintDocument(){
        PrintDocument printDocument = new PrintDocument();
        PrinterSettings settings = printDocument.get_PrinterSettings();

        Attribute destination = reqAttrSet.get(Destination.class);
        if(destination instanceof Destination){
        	File destFile = new File(((Destination)destination).getURI());
            settings.set_PrintFileName(destFile.getAbsolutePath());
        }

        settings.set_Copies((short)copies);
        if(copies > 1){
            Attribute collate = reqAttrSet.get(SheetCollate.class);
            settings.set_Collate(collate == SheetCollate.COLLATED);
        }

        return printDocument;
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
                    if (!service.isAttributeValueSupported(attr, null, null)) {
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

    
    private class PrintPage implements PrintPageEventHandler.Method{

        private final Pageable pageable;
        private int pageIndex;


        PrintPage(Pageable pageable){
            this.pageable = pageable;
            //TODO firstPage
            //TODO lastPage
            //TODO PageRange
            //TODO Num Copies
            //TODO collatedCopies
        }


        @Override
        public void Invoke(Object paramObject, PrintPageEventArgs ev){

            try{
                System.err.println("Invoke:"+paramObject);
                Printable printable = pageable.getPrintable(pageIndex);
                PageFormat pageFormat = pageable.getPageFormat(pageIndex);

                
                BufferedImage pBand = new BufferedImage(1, 1, BufferedImage.TYPE_3BYTE_BGR);
                Graphics2D imageGraphics = pBand.createGraphics();
                ((RasterPrinterJob)job).setGraphicsConfigInfo(imageGraphics.getTransform(), pageFormat.getWidth(), pageFormat.getHeight());
				PeekGraphics peekGraphics = new PeekGraphics(imageGraphics, job );

                /* 
                 * Because Sun is calling first with a PeekGraphics that we do it else for compatibility
                 */
                if(pageIndex == 0){
                    int pageResult = printable.print(peekGraphics, pageFormat, pageIndex);
                    if(pageResult != Printable.PAGE_EXISTS){
                        ev.set_HasMorePages(false);
                        ev.set_Cancel(true);
                        return;
                    }
                }
                Graphics printGraphics = peer.createGraphics(ev.get_Graphics());
                printable.print(printGraphics, pageFormat, pageIndex++);

                printable = pageable.getPrintable(pageIndex);
                pageFormat = pageable.getPageFormat(pageIndex);
                int pageResult = printable.print(peekGraphics, pageFormat, pageIndex);
                ev.set_HasMorePages(pageResult == Printable.PAGE_EXISTS);
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


        QueryPage(Pageable pageable){
            this.pageable = pageable;
            //TODO firstPage
            //TODO lastPage
            //TODO PageRange
            //TODO Num Copies
            //TODO collatedCopies
        }
        
		@Override
		public void Invoke(Object source, QueryPageSettingsEventArgs e) {
			// apply page settings to the current page
			PageFormat format = pageable.getPageFormat(pageIndex);
			PageSettings pageSettings = e.get_PageSettings();
			pageSettings.set_Landscape(format.getOrientation() == PageFormat.LANDSCAPE);
			
			PaperSize ps = new PaperSize();
			ps.set_Height( (int)Math.round( format.getHeight() ) );
			ps.set_Width( (int)Math.round( format.getWidth() ) );
			pageSettings.set_PaperSize( ps );
			
			Margins margins = new Margins();
			margins.set_Left( (int) format.getImageableX() );
			margins.set_Top( (int)format.getImageableY() );
			margins.set_Right( (int) (format.getWidth() - format.getImageableX() - format.getImageableWidth() ) );
			margins.set_Bottom( (int)(format.getHeight() - format.getImageableY() - format.getImageableHeight() ) );			
			pageSettings.set_Margins( margins );
			pageIndex++;
		}
    	
    }
}
