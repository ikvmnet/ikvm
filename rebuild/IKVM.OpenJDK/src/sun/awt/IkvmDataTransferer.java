/*
  Copyright (C) 2009 Jeroen Frijters

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

package sun.awt;

import java.awt.Image;
import java.awt.datatransfer.DataFlavor;
import java.awt.datatransfer.FlavorTable;
import java.awt.datatransfer.Transferable;
import java.awt.datatransfer.UnsupportedFlavorException;
import java.io.BufferedInputStream;
import java.io.BufferedReader;
import java.io.ByteArrayInputStream;
import java.io.IOException;
import java.io.InputStream;
import java.io.InputStreamReader;
import java.io.UnsupportedEncodingException;
import java.net.URL;
import java.util.Arrays;
import java.util.Collections;
import java.util.HashMap;
import java.util.Map;
import java.util.SortedMap;

import sun.awt.datatransfer.DataTransferer;
import cli.System.Runtime.InteropServices.DllImportAttribute;

public abstract class IkvmDataTransferer extends DataTransferer {
	public static final int CF_TEXT = 1;
	public static final int CF_METAFILEPICT = 3;
	public static final int CF_DIB = 8;
	public static final int CF_ENHMETAFILE = 14;
	public static final int CF_HDROP = 15;
	public static final int CF_LOCALE = 16;

	public static final long CF_HTML = RegisterClipboardFormat("HTML Format");
	public static final long CFSTR_INETURL = RegisterClipboardFormat("UniformResourceLocator");
	public static final long CF_PNG = RegisterClipboardFormat("PNG");
	public static final long CF_JFIF = RegisterClipboardFormat("JFIF");
	
    private static final String[] predefinedClipboardNames = {
        "",
        "TEXT",
        "BITMAP",
        "METAFILEPICT",
        "SYLK",
        "DIF",
        "TIFF",
        "OEM TEXT",
        "DIB",
        "PALETTE",
        "PENDATA",
        "RIFF",
        "WAVE",
        "UNICODE TEXT",
        "ENHMETAFILE",
        "HDROP",
        "LOCALE",
        "DIBV5"
    };

    private static final Map<String, Long> predefinedClipboardNameMap;

    static {
        Map<String, Long> tempMap = new HashMap<String, Long>(predefinedClipboardNames.length, 1.0f);
        for (int i = 1; i < predefinedClipboardNames.length; i++) {
            tempMap.put(predefinedClipboardNames[i], Long.valueOf(i));
        }
        predefinedClipboardNameMap = Collections.synchronizedMap(tempMap);
    }

    private static final Long L_CF_LOCALE = (Long) predefinedClipboardNameMap.get(predefinedClipboardNames[CF_LOCALE]);
    
    @DllImportAttribute.Annotation(value = "user32.dll", EntryPoint = "RegisterClipboardFormat")
    private native static int _RegisterClipboardFormat(String format);

    @cli.System.Security.SecuritySafeCriticalAttribute.Annotation
    private static int RegisterClipboardFormat(String format)
    {
        return _RegisterClipboardFormat(format);
    }

    public SortedMap getFormatsForFlavors(DataFlavor[] flavors, FlavorTable map) {
        SortedMap retval = super.getFormatsForFlavors(flavors, map);

        // The Win32 native code does not support exporting LOCALE data, nor
        // should it.
        retval.remove(L_CF_LOCALE);

        return retval;
    }

    public String getDefaultUnicodeEncoding() {
        return "utf-16le";
    }

    public byte[] translateTransferable(Transferable contents,
                                        DataFlavor flavor,
                                        long format) throws IOException
    {
        byte[] bytes = super.translateTransferable(contents, flavor, format);

        if (format == CF_HTML) {
            bytes = HTMLCodec.convertToHTMLFormat(bytes);
        }
        return bytes;
    }

	protected byte[] imageToPlatformBytes(Image image, long format)
			throws IOException {
		String mimeType = null;
		if (format == CF_PNG) {
			mimeType = "image/png";
		} else if (format == CF_JFIF) {
			mimeType = "image/jpeg";
		}
		if (mimeType != null) {
			return imageToStandardBytes(image, mimeType);
		}
		throw new Error("Not implemented");
	}

	protected abstract byte[] imageToStandardBytes(Image image, String mimeType)
			throws IOException;

	protected Image platformImageBytesOrStreamToImage(InputStream str,
			byte[] bytes, long format) {
		throw new Error("Not implemented");
	}

	protected String[] dragQueryFile(byte[] bytes) {
		throw new Error("Not implemented");
	}

    protected String getNativeForFormat(long format) {
        return (format < predefinedClipboardNames.length && format>=0)
            ? predefinedClipboardNames[(int)format]
            : getClipboardFormatName(format);
    }

	protected Long getFormatForNativeAsLong(String str) {
        Long format = (Long)predefinedClipboardNameMap.get(str);
        if (format == null) {
            format = Long.valueOf(RegisterClipboardFormat(str));
        }
        return format;	}
	
	protected abstract String getClipboardFormatName(long format);

    public boolean isImageFormat(long format) {
        return format == CF_DIB || format == CF_ENHMETAFILE ||
               format == CF_METAFILEPICT || format == CF_PNG ||
               format == CF_JFIF;
    }

    public boolean isLocaleDependentTextFormat(long format) {
        return format == CF_TEXT || format == CFSTR_INETURL;
    }

    public boolean isFileFormat(long format) {
        return format == CF_HDROP;
    }

}

enum EHTMLReadMode {
    HTML_READ_ALL,
    HTML_READ_FRAGMENT,
    HTML_READ_SELECTION
}

/**
 * on decode: This stream takes an InputStream which provides data in CF_HTML format,
 * strips off the description and context to extract the original HTML data.
 *
 * on encode: static convertToHTMLFormat is responsible for HTML clipboard header creation
 */
class HTMLCodec extends InputStream {
    //static section
    public static final String ENCODING = "UTF-8";

    public static final String VERSION = "Version:";
    public static final String START_HTML = "StartHTML:";
    public static final String END_HTML = "EndHTML:";
    public static final String START_FRAGMENT = "StartFragment:";
    public static final String END_FRAGMENT = "EndFragment:";
    public static final String START_SELECTION = "StartSelection:"; //optional
    public static final String END_SELECTION = "EndSelection:"; //optional

    public static final String START_FRAGMENT_CMT = "<!--StartFragment-->";
    public static final String END_FRAGMENT_CMT = "<!--EndFragment-->";
    public static final String SOURCE_URL = "SourceURL:";
    public static final String DEF_SOURCE_URL = "about:blank";

    public static final String EOLN = "\r\n";

    private static final String VERSION_NUM = "1.0";
    private static final int PADDED_WIDTH = 10;

    private static String toPaddedString(int n, int width) {
        String string = "" + n;
        int len = string.length();
        if (n >= 0 && len < width) {
            char[] array = new char[width - len];
            Arrays.fill(array, '0');
            StringBuffer buffer = new StringBuffer(width);
            buffer.append(array);
            buffer.append(string);
            string = buffer.toString();
        }
        return string;
    }

    /**
     * convertToHTMLFormat adds the MS HTML clipboard header to byte array that
     * contains the parameters pairs.
     *
     * The consequence of parameters is fixed, but some or all of them could be
     * omitted. One parameter per one text line.
     * It looks like that:
     *
     * Version:1.0\r\n                -- current supported version
     * StartHTML:000000192\r\n        -- shift in array to the first byte after the header
     * EndHTML:000000757\r\n          -- shift in array of last byte for HTML syntax analysis
     * StartFragment:000000396\r\n    -- shift in array jast after <!--StartFragment-->
     * EndFragment:000000694\r\n      -- shift in array before start  <!--EndFragment-->
     * StartSelection:000000398\r\n   -- shift in array of the first char in copied selection
     * EndSelection:000000692\r\n     -- shift in array of the last char in copied selection
     * SourceURL:http://sun.com/\r\n  -- base URL for related referenses
     * <HTML>...<BODY>...<!--StartFragment-->.....................<!--EndFragment-->...</BODY><HTML>
     * ^                                     ^ ^                ^^                                 ^
     * \ StartHTML                           | \-StartSelection | \EndFragment              EndHTML/
     *                                       \-StartFragment    \EndSelection
     *
     *Combinations with tags sequence
     *<!--StartFragment--><HTML>...<BODY>...</BODY><HTML><!--EndFragment-->
     * or
     *<HTML>...<!--StartFragment-->...<BODY>...</BODY><!--EndFragment--><HTML>
     * are vailid too.
     */
    public static byte[] convertToHTMLFormat(byte[] bytes) {
        // Calculate section offsets
        String htmlPrefix = "";
        String htmlSuffix = "";
        {
            //we have extend the fragment to full HTML document correctly
            //to avoid HTML and BODY tags doubling
            String stContext = new String(bytes);
            String stUpContext = stContext.toUpperCase();
            if( -1 == stUpContext.indexOf("<HTML") ) {
                htmlPrefix = "<HTML>";
                htmlSuffix = "</HTML>";
                if( -1 == stUpContext.indexOf("<BODY") ) {
                    htmlPrefix = htmlPrefix +"<BODY>";
                    htmlSuffix = "</BODY>" + htmlSuffix;
                };
            };
            htmlPrefix = htmlPrefix + START_FRAGMENT_CMT;
            htmlSuffix = END_FRAGMENT_CMT + htmlSuffix;
        }

        String stBaseUrl = DEF_SOURCE_URL;
        int nStartHTML =
            VERSION.length() + VERSION_NUM.length() + EOLN.length()
            + START_HTML.length() + PADDED_WIDTH + EOLN.length()
            + END_HTML.length() + PADDED_WIDTH + EOLN.length()
            + START_FRAGMENT.length() + PADDED_WIDTH + EOLN.length()
            + END_FRAGMENT.length() + PADDED_WIDTH + EOLN.length()
            + SOURCE_URL.length() + stBaseUrl.length() + EOLN.length()
            ;
        int nStartFragment = nStartHTML + htmlPrefix.length();
        int nEndFragment = nStartFragment + bytes.length - 1;
        int nEndHTML = nEndFragment + htmlSuffix.length();

        StringBuilder header = new StringBuilder(
            nStartFragment
            + START_FRAGMENT_CMT.length()
        );
        //header
        header.append(VERSION);
        header.append(VERSION_NUM);
        header.append(EOLN);

        header.append(START_HTML);
        header.append(toPaddedString(nStartHTML, PADDED_WIDTH));
        header.append(EOLN);

        header.append(END_HTML);
        header.append(toPaddedString(nEndHTML, PADDED_WIDTH));
        header.append(EOLN);

        header.append(START_FRAGMENT);
        header.append(toPaddedString(nStartFragment, PADDED_WIDTH));
        header.append(EOLN);

        header.append(END_FRAGMENT);
        header.append(toPaddedString(nEndFragment, PADDED_WIDTH));
        header.append(EOLN);

        header.append(SOURCE_URL);
        header.append(stBaseUrl);
        header.append(EOLN);

        //HTML
        header.append(htmlPrefix);

        byte[] headerBytes = null, trailerBytes = null;

        try {
            headerBytes = new String(header).getBytes(ENCODING);
            trailerBytes = htmlSuffix.getBytes(ENCODING);
        } catch (UnsupportedEncodingException cannotHappen) {
        }

        byte[] retval = new byte[headerBytes.length + bytes.length +
                                 trailerBytes.length];

        System.arraycopy(headerBytes, 0, retval, 0, headerBytes.length);
        System.arraycopy(bytes, 0, retval, headerBytes.length,
                       bytes.length - 1);
        System.arraycopy(trailerBytes, 0, retval,
                         headerBytes.length + bytes.length - 1,
                         trailerBytes.length);
        retval[retval.length-1] = 0;

        return retval;
    }

    ////////////////////////////////////
    //decoder instance data and methods:

    private final BufferedInputStream bufferedStream;
    private boolean descriptionParsed = false;
    private boolean closed = false;

     // InputStreamReader uses an 8K buffer. The size is not customizable.
    public static final int BYTE_BUFFER_LEN = 8192;

    // CharToByteUTF8.getMaxBytesPerChar returns 3, so we should not buffer
    // more chars than 3 times the number of bytes we can buffer.
    public static final int CHAR_BUFFER_LEN = BYTE_BUFFER_LEN / 3;

    private static final String FAILURE_MSG =
        "Unable to parse HTML description: ";
    private static final String INVALID_MSG =
        " invalid";

    //HTML header mapping:
    private long   iHTMLStart,// StartHTML -- shift in array to the first byte after the header
                   iHTMLEnd,  // EndHTML -- shift in array of last byte for HTML syntax analysis
                   iFragStart,// StartFragment -- shift in array jast after <!--StartFragment-->
                   iFragEnd,  // EndFragment -- shift in array before start <!--EndFragment-->
                   iSelStart, // StartSelection -- shift in array of the first char in copied selection
                   iSelEnd;   // EndSelection -- shift in array of the last char in copied selection
    private String stBaseURL; // SourceURL -- base URL for related referenses
    private String stVersion; // Version -- current supported version

    //Stream reader markers:
    private long iStartOffset,
                 iEndOffset,
                 iReadCount;

    private EHTMLReadMode readMode;

    public HTMLCodec(
        InputStream _bytestream,
        EHTMLReadMode _readMode) throws IOException
    {
        bufferedStream = new BufferedInputStream(_bytestream, BYTE_BUFFER_LEN);
        readMode = _readMode;
    }

    public synchronized String getBaseURL() throws IOException
    {
        if( !descriptionParsed ) {
            parseDescription();
        }
        return stBaseURL;
    }
    public synchronized String getVersion() throws IOException
    {
        if( !descriptionParsed ) {
            parseDescription();
        }
        return stVersion;
    }

    /**
     * parseDescription parsing HTML clipboard header as it described in
     * comment to convertToHTMLFormat
     */
    private void parseDescription() throws IOException
    {
        stBaseURL = null;
        stVersion = null;

        // initialization of array offset pointers
        // to the same "uninitialized" state.
        iHTMLEnd =
            iHTMLStart =
                iFragEnd =
                    iFragStart =
                        iSelEnd =
                            iSelStart = -1;

        bufferedStream.mark(BYTE_BUFFER_LEN);
        String astEntries[] = new String[] {
            //common
            VERSION,
            START_HTML,
            END_HTML,
            START_FRAGMENT,
            END_FRAGMENT,
            //ver 1.0
            START_SELECTION,
            END_SELECTION,
            SOURCE_URL
        };
        BufferedReader bufferedReader = new BufferedReader(
            new InputStreamReader(
                bufferedStream,
                ENCODING
            ),
            CHAR_BUFFER_LEN
        );
        long iHeadSize = 0;
        long iCRSize = EOLN.length();
        int iEntCount = astEntries.length;
        boolean bContinue = true;

        for( int  iEntry = 0; iEntry < iEntCount; ++iEntry ){
            String stLine = bufferedReader.readLine();
            if( null==stLine ) {
                break;
            }
            //some header entries are optional, but the order is fixed.
            for( ; iEntry < iEntCount; ++iEntry ){
                if( !stLine.startsWith(astEntries[iEntry]) ) {
                    continue;
                }
                iHeadSize += stLine.length() + iCRSize;
                String stValue = stLine.substring(astEntries[iEntry].length()).trim();
                if( null!=stValue ) {
                    try{
                        switch( iEntry ){
                        case 0:
                            stVersion = stValue;
                            break;
                        case 1:
                            iHTMLStart = Integer.parseInt(stValue);
                            break;
                        case 2:
                            iHTMLEnd = Integer.parseInt(stValue);
                            break;
                        case 3:
                            iFragStart = Integer.parseInt(stValue);
                            break;
                        case 4:
                            iFragEnd = Integer.parseInt(stValue);
                            break;
                        case 5:
                            iSelStart = Integer.parseInt(stValue);
                            break;
                        case 6:
                            iSelEnd = Integer.parseInt(stValue);
                            break;
                        case 7:
                            stBaseURL = stValue;
                            break;
                        };
                    } catch ( NumberFormatException e ) {
                        throw new IOException(FAILURE_MSG + astEntries[iEntry]+ " value " + e + INVALID_MSG);
                    }
                }
                break;
            }
        }
        //some entries could absent in HTML header,
        //so we have find they by another way.
        if( -1 == iHTMLStart )
            iHTMLStart = iHeadSize;
        if( -1 == iFragStart )
            iFragStart = iHTMLStart;
        if( -1 == iFragEnd )
            iFragEnd = iHTMLEnd;
        if( -1 == iSelStart )
            iSelStart = iFragStart;
        if( -1 == iSelEnd )
            iSelEnd = iFragEnd;

        //one of possible modes
        switch( readMode ){
        case HTML_READ_ALL:
            iStartOffset = iHTMLStart;
            iEndOffset = iHTMLEnd;
            break;
        case HTML_READ_FRAGMENT:
            iStartOffset = iFragStart;
            iEndOffset = iFragEnd;
            break;
        case HTML_READ_SELECTION:
        default:
            iStartOffset = iSelStart;
            iEndOffset = iSelEnd;
            break;
        }

        bufferedStream.reset();
        if( -1 == iStartOffset ){
            throw new IOException(FAILURE_MSG + "invalid HTML format.");
        }
        iReadCount = bufferedStream.skip(iStartOffset);
        if( iStartOffset != iReadCount ){
            throw new IOException(FAILURE_MSG + "Byte stream ends in description.");
        }
        descriptionParsed = true;
    }

    public synchronized int read() throws IOException {
        if( closed ){
            throw new IOException("Stream closed");
        }

        if( !descriptionParsed ){
            parseDescription();
        }
        if( -1 != iEndOffset && iReadCount >= iEndOffset ) {
            return -1;
        }

        int retval = bufferedStream.read();
        if( retval == -1 ) {
            return -1;
        }
        ++iReadCount;
        return retval;
    }

    public synchronized void close() throws IOException {
        if( !closed ){
            closed = true;
            bufferedStream.close();
        }
    }
}
