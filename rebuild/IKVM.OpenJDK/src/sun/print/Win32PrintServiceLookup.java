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

import ikvm.awt.IkvmToolkit;

import java.awt.Toolkit;

import java.io.BufferedReader;
import java.io.InputStream;
import java.io.InputStreamReader;
import java.io.IOException;
import java.util.ArrayList;
import java.security.AccessController;
import java.security.PrivilegedActionException;
import java.security.PrivilegedExceptionAction;
import javax.print.DocFlavor;
import javax.print.MultiDocPrintService;
import javax.print.PrintService;
import javax.print.PrintServiceLookup;
import javax.print.attribute.Attribute;
import javax.print.attribute.AttributeSet;
import javax.print.attribute.HashPrintRequestAttributeSet;
import javax.print.attribute.HashPrintServiceAttributeSet;
import javax.print.attribute.PrintRequestAttribute;
import javax.print.attribute.PrintRequestAttributeSet;
import javax.print.attribute.PrintServiceAttribute;
import javax.print.attribute.PrintServiceAttributeSet;
import javax.print.attribute.standard.PrinterName;

public class Win32PrintServiceLookup extends PrintServiceLookup {
    
    private final PrintPeer peer = IkvmToolkit.DefaultToolkit.get().getPrintPeer();

    private String defaultPrinter;
    private PrintService defaultPrintService;
    private String[] printers; /* excludes the default printer */
    private PrintService[] printServices; /* includes the default printer */
    

    /* Want the PrintService which is default print service to have
     * equality of reference with the equivalent in list of print services
     * This isn't required by the API and there's a risk doing this will
     * lead people to assume its guaranteed.
     */
    public synchronized PrintService[] getPrintServices() {
        SecurityManager security = System.getSecurityManager();
        if (security != null) {
            security.checkPrintJobAccess();
        }
        if (printServices == null) {
            refreshServices();
        }
        return printServices;
    }

    private synchronized void refreshServices() {
        printers = peer.getAllPrinterNames();
        if (printers == null) {
            // In Windows it is safe to assume no default if printers == null so we
            // don't get the default.
            printServices = new PrintService[0];
            return;
        }

        PrintService[] newServices = new PrintService[printers.length];
        PrintService defService = getDefaultPrintService();
        for (int p = 0; p < printers.length; p++) {
            if (defService != null &&
                printers[p].equals(defService.getName())) {
                newServices[p] = defService;
            } else {
                if (printServices == null) {
                    newServices[p] = new Win32PrintService(printers[p], peer);
                } else {
                    int j;
                    for (j = 0; j < printServices.length; j++) {
                        if ((printServices[j]!= null) &&
                            (printers[p].equals(printServices[j].getName()))) {
                            newServices[p] = printServices[j];
                            printServices[j] = null;
                            break;
                        }
                    }
                    if (j == printServices.length) {
                        newServices[p] = new Win32PrintService(printers[p], peer);
                    }
                }
            }
        }

        printServices = newServices;
    }


    public synchronized PrintService getPrintServiceByName(String name) {

        if (name == null || name.equals("")) {
            return null;
        } else {
            /* getPrintServices() is now very fast. */
            PrintService[] printServices = getPrintServices();
            for (int i=0; i<printServices.length; i++) {
                if (printServices[i].getName().equals(name)) {
                    return printServices[i];
                }
            }
            return null;
        }
    }

    boolean matchingService(PrintService service,
                            PrintServiceAttributeSet serviceSet) {
        if (serviceSet != null) {
            Attribute [] attrs =  serviceSet.toArray();
            Attribute serviceAttr;
            for (int i=0; i<attrs.length; i++) {
                serviceAttr
                    = service.getAttribute((Class<PrintServiceAttribute>)attrs[i].getCategory());
                if (serviceAttr == null || !serviceAttr.equals(attrs[i])) {
                    return false;
                }
            }
        }
        return true;
    }

    public PrintService[] getPrintServices(DocFlavor flavor,
                                           AttributeSet attributes) {

        SecurityManager security = System.getSecurityManager();
        if (security != null) {
          security.checkPrintJobAccess();
        }
        PrintRequestAttributeSet requestSet = null;
        PrintServiceAttributeSet serviceSet = null;

        if (attributes != null && !attributes.isEmpty()) {

            requestSet = new HashPrintRequestAttributeSet();
            serviceSet = new HashPrintServiceAttributeSet();

            Attribute[] attrs = attributes.toArray();
            for (int i=0; i<attrs.length; i++) {
                if (attrs[i] instanceof PrintRequestAttribute) {
                    requestSet.add(attrs[i]);
                } else if (attrs[i] instanceof PrintServiceAttribute) {
                    serviceSet.add(attrs[i]);
                }
            }
        }

        /*
         * Special case: If client is asking for a particular printer
         * (by name) then we can save time by getting just that service
         * to check against the rest of the specified attributes.
         */
        PrintService[] services = null;
        if (serviceSet != null && serviceSet.get(PrinterName.class) != null) {
            PrinterName name = (PrinterName)serviceSet.get(PrinterName.class);
            PrintService service = getPrintServiceByName(name.getValue());
            if (service == null || !matchingService(service, serviceSet)) {
                services = new PrintService[0];
            } else {
                services = new PrintService[1];
                services[0] = service;
            }
        } else {
            services = getPrintServices();
        }

        if (services.length == 0) {
            return services;
        } else {
            ArrayList matchingServices = new ArrayList();
            for (int i=0; i<services.length; i++) {
                try {
                    if (services[i].
                        getUnsupportedAttributes(flavor, requestSet) == null) {
                        matchingServices.add(services[i]);
                    }
                } catch (IllegalArgumentException e) {
                }
            }
            services = new PrintService[matchingServices.size()];
            return (PrintService[])matchingServices.toArray(services);
        }
    }

    /*
     * return empty array as don't support multi docs
     */
    public MultiDocPrintService[]
        getMultiDocPrintServices(DocFlavor[] flavors,
                                 AttributeSet attributes) {
        SecurityManager security = System.getSecurityManager();
        if (security != null) {
          security.checkPrintJobAccess();
        }
        return new MultiDocPrintService[0];
    }


    public synchronized PrintService getDefaultPrintService() {
        SecurityManager security = System.getSecurityManager();
        if (security != null) {
          security.checkPrintJobAccess();
        }


        // Windows does not have notification for a change in default
        // so we always get the latest.
        defaultPrinter = peer.getDefaultPrinterName();
        if (defaultPrinter == null) {
            return null;
        }

        if ((defaultPrintService != null) &&
            defaultPrintService.getName().equals(defaultPrinter)) {

            return defaultPrintService;
        }

         // Not the same as default so proceed to get new PrintService.

        // clear defaultPrintService
        defaultPrintService = null;

        if (printServices != null) {
            for (int j=0; j<printServices.length; j++) {
                if (defaultPrinter.equals(printServices[j].getName())) {
                    defaultPrintService = printServices[j];
                    break;
                }
            }
        }

        if (defaultPrintService == null) {
            defaultPrintService = new Win32PrintService(defaultPrinter, peer);
        }
        return defaultPrintService;
     }
}
