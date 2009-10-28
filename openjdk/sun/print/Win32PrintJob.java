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

import java.awt.print.PrinterJob;
import java.io.IOException;
import java.net.URL;
import java.util.ArrayList;

import javax.print.CancelablePrintJob;
import javax.print.Doc;
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
import javax.print.attribute.standard.DocumentName;
import javax.print.attribute.standard.JobName;
import javax.print.attribute.standard.JobOriginatingUserName;
import javax.print.attribute.standard.RequestingUserName;
import javax.print.event.PrintJobAttributeListener;
import javax.print.event.PrintJobEvent;
import javax.print.event.PrintJobListener;

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

    private PrinterJob job;


    Win32PrintJob(Win32PrintService service){
        this.service = service;
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
        // TODO Auto-generated method stub
        try{
            initializeAttributeSets(doc, attributes);
            System.err.println("Win32PrintJob.print:" + attributes);
        }finally{
            printReturned = true;
        }
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

}
