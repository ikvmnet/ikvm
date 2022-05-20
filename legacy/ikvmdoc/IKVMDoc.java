/*

  This software is provided 'as-is', without any express or implied
  warranty.  In no event will the authors be held liable for any damages
  arising from the use of this software.

  Permission is granted to anyone to use this software for any purpose,
  including commercial applications, and to alter it and redistribute it
  freely.
  
*/

import java.io.*;
import java.util.*;

import javax.xml.parsers.*;

import org.xml.sax.*;
import org.xml.sax.helpers.*;

import com.sun.javadoc.*;
import com.sun.tools.javadoc.Main;

/**
 * Java Doclet for generating .NET XML API documentation.
 * <p>
 * The current implementation may has not been tested
 * with (and thus may not support) the following features:
 * {@code}; should be converted to <c> tags
 * {@docRoot}
 * {@inheritDoc}
 * {@literal}
 * {@value}
 * references to package documentation
 * annotations
 * <p>
 * Other issues:
 * <p>
 * <pre>
 * <b>Usage reports "javadoc" instead of "ikvmdoc"</b>:
 * 
 * usage: javadoc [options] [packagenames] [sourcefiles] [@files]
 * 
 * <b>should be:</b>
 *  
 * usage: ikvmdoc [options] [packagenames] [sourcefiles] [@files]
 * </pre>
 * <p>
 * HTML tag parsing is not forgiving; should be made more fault tolerant
 * <p>
 * Javadoc HTML -> .NET tag conversions that should be considered/evaluated:
 * <code>true</code> -> <see langref="true"/>
 * <code>false</code> -> <see langref="false"/>
 * <code>null</code> -> <see langref="null"/>
 * 
 * @author Brian Heineman
 */
public class IKVMDoc extends Doclet {
	/**
	 * Map of Java data types to .NET data types.
	 */
	private static final Map<String, String> DATA_TYPE_MAPPING = new HashMap<String, String>();
	
	/**
	 * Name of the assembly file parameter.
	 */
	private static final String ASSEMBLY_PARAMETER = "-assembly";

	/**
	 * Name of the HTML parameter.
	 */
	private static final String HTML_PARAMETER = "-nohtml";
	
	/**
	 * Name of the strict final field semantics parameter.
	 */
	private static final String STRICT_FINAL_FIELD_SEMANTICS_PARAMETER = "-strictfinalfieldsemantics";

	/**
	 * Name of the author parameter.
	 */
	private static final String AUTHOR_PARAMETER = "-author";
	
	/**
	 * Name of the deprecated parameter.
	 */
	private static final String DEPRECATED_PARAMETER = "-nodeprecated";
	
	/**
	 * Name of the since parameter.
	 */
	private static final String SINCE_PARAMETER = "-nosince";
	
	/**
	 * Name of the version parameter.
	 */
	private static final String VERSION_PARAMETER = "-version";
	
	/**
	 * The assembly file the .NET documentation will be generated for.
	 */
	private static File ASSEMBLY_FILE;

	/**
	 * Indicates if HTML should be included in the .NET XML documentation.
	 * Default is <code>true</code> to reflect the standard doclet behavior.
	 * <p>
	 * NOTE: The Java Runtime API contains invalid HTML which requires this
	 * option to be set to <code>false</code> when generating its
	 * .NET XML documentation.
	 */
	private static boolean OUTPUT_HTML = true;
	
	/**
	 * Indicates if the author information should be included in the .NET XML documentation.
	 * Default is <code>false</code> to reflect the standard doclet behavior.
	 */
	private static boolean OUTPUT_AUTHOR = false;

	/**
	 * Indicates if the deprecated information should be included in the .NET XML documentation.
	 * Default is <code>true</code> to reflect the standard doclet behavior.
	 */
	private static boolean OUTPUT_DEPRECATED = true;
	
	/**
	 * Indicates if the since information should be included in the .NET XML documentation.
	 * Default is <code>true</code> to reflect the standard doclet behavior.
	 */
	private static boolean OUTPUT_SINCE = true;
	
	/**
	 * Indicates if the version information should be included in the .NET XML documentation.
	 * Default is <code>false</code> to reflect the standard doclet behavior.
	 */
	private static boolean OUTPUT_VERSION = false;

	/**
	 * The reported used to report failures to. 
	 */
	private static DocErrorReporter ERROR_REPORTER;
	
	static {
		// Populate the Java->.NET data type mappings
		DATA_TYPE_MAPPING.put("boolean", "System.Boolean");
		DATA_TYPE_MAPPING.put("byte", "System.Byte");
		DATA_TYPE_MAPPING.put("char", "System.Char");
		DATA_TYPE_MAPPING.put("short", "System.Int16");
		DATA_TYPE_MAPPING.put("int", "System.Int32");
		DATA_TYPE_MAPPING.put("long", "System.Int64");
		DATA_TYPE_MAPPING.put("float", "System.Single");
		DATA_TYPE_MAPPING.put("double", "System.Double");
		DATA_TYPE_MAPPING.put("java.lang.Object", "System.Object");
		DATA_TYPE_MAPPING.put("java.lang.String", "System.String");
		DATA_TYPE_MAPPING.put("java.lang.Throwable", "System.Exception");
	}
	
	/**
	 * Generate the .NET XML Documentation.
	 * 
	 * @param root represents the root of the program structure information for one run of javadoc 
	 * @return <code>true</code> on success; <code>false</code> on failure
	 */
    public static boolean start(RootDoc root) {
    	String assemblyName = ASSEMBLY_FILE.getName();
    	int extensionIndex = assemblyName.lastIndexOf('.');
    	
    	if (extensionIndex != -1) {
    		assemblyName = assemblyName.substring(0, extensionIndex);
    	}
    	
    	File documentationFile = new File(ASSEMBLY_FILE.getParent(), assemblyName + ".xml");
    	PrintWriter pw = null;
    	
    	try {
	    	FileOutputStream fos = new FileOutputStream(documentationFile);
	    	OutputStreamWriter osw = new OutputStreamWriter(fos, "UTF-8");
	    	BufferedWriter bw = new BufferedWriter(osw);
	    	pw = new PrintWriter(bw);
	    	
	    	// Write the header
	    	pw.println("<?xml version=\"1.0\"?>");
	    	pw.println("<doc>");
	    	printIndent(pw, 1);
	    	pw.println("<assembly>");
	    	printIndent(pw, 2);
	    	pw.print("<name>");
	    	pw.print(assemblyName);
	    	pw.println("</name>");
	    	printIndent(pw, 1);
	    	pw.println("</assembly>");
	    	printIndent(pw, 1);
	    	pw.println("<members>");

	        ClassDoc[] classes = root.classes();
	        
	        for (ClassDoc classDoc : classes) {
	        	print(pw, classDoc);
	        }
	        
	    	// Write the footer
	    	printIndent(pw, 1);
	    	pw.println("</members>");
	    	pw.println("</doc>");
	    	pw.close();
	    	
	    	validate(documentationFile);
    	} catch (Exception e) {
    		e.printStackTrace();
    		return false;
    	} finally {
    		if (pw != null) {
    			pw.close();
    		}
    	}
        
        return true;
	}
    
    /**
     * Prints the member documentation.
     * 
     * @param pw the writer to print the documentation to
     * @param programElementDoc the member to document
     */
    private static void print(PrintWriter pw, ProgramElementDoc programElementDoc) {
    	/*
    	 * Implementation of proposed @exclude tag: http://java.sun.com/j2se/javadoc/proposed-tags.html 
    	 */
    	if (programElementDoc.tags("@exclude").length > 0) {
    		return;
    	}
    	
    	printIndent(pw, 2);
    	pw.print("<member name=\"");
    	printReference(pw, programElementDoc, true);
    	pw.println("\">");
    	
    	printIndent(pw, 3);
    	pw.print("<summary>");
    	
    	if (OUTPUT_DEPRECATED) {
    		printTags(pw, programElementDoc, "DEPRECATED:", "@deprecated");
    	}
    	
		printComment(pw, programElementDoc, programElementDoc.inlineTags());
		
		if (OUTPUT_AUTHOR) {
			printTags(pw, programElementDoc, "Author:", "@author");
		}
		
		if (OUTPUT_VERSION) {
			printTags(pw, programElementDoc, "Version:", "@version");
		}
		
		if (OUTPUT_SINCE) {
			printTags(pw, programElementDoc, "Since:", "@since");
		}
		
    	printTags(pw, programElementDoc, "Serial:", "@serial");
    	printTags(pw, programElementDoc, "Serial Field:", "@serialField");
    	printTags(pw, programElementDoc, "Serial Data:", "@serialData");
    	
    	pw.println("</summary>");
    	
    	if (programElementDoc instanceof ExecutableMemberDoc) {
    		ExecutableMemberDoc executableMemberDoc = (ExecutableMemberDoc) programElementDoc;
    		
        	printParamTags(pw, executableMemberDoc);
        	printReturnTag(pw, executableMemberDoc);
        	printThrowsTags(pw, executableMemberDoc);
    	}

    	printSeeTags(pw, programElementDoc);
    	
    	printIndent(pw, 2);
    	pw.println("</member>");

    	// Document class members
    	if (programElementDoc instanceof ClassDoc) {
    		ClassDoc classDoc = (ClassDoc) programElementDoc;
        	FieldDoc[] fields = classDoc.fields();
        	
        	for (FieldDoc fieldDoc : fields) {
        		print(pw, fieldDoc);
        	}
        	
        	ConstructorDoc[] constructors = classDoc.constructors();
        	
        	for (ConstructorDoc constructorDoc : constructors) {
        		print(pw, constructorDoc);
        	}
        	
        	MethodDoc[] methods = classDoc.methods();
        	
        	for (MethodDoc methodDoc : methods) {
        		print(pw, methodDoc);
        	}
    	}
    }

    /**
     * Prints the parameter documentation.
     * 
     * @param pw the writer to print the parameter documentation to
     * @param memberDoc the member to document the parameters for
     */
    private static void printParameters(PrintWriter pw, ExecutableMemberDoc memberDoc) {
    	Parameter[] parameters = memberDoc.parameters();
    	
    	if (parameters.length > 0) {
    		pw.print("(");
    	}
    	
    	for (int i = 0; i < parameters.length; i++) {
    		Type parameterType = parameters[i].type();
    		
    		if (i != 0) {
    			pw.print(",");
    		}

    		String qualifiedTypeName = parameterType.qualifiedTypeName();
    		String mappedDataType = DATA_TYPE_MAPPING.get(qualifiedTypeName);

    		// Print the mapped data type if there is one
    		if (mappedDataType != null) {
    			pw.print(mappedDataType);
    		} else {
    			pw.print(qualifiedTypeName);
    		}

    		pw.print(parameterType.dimension());
    	}
    	
    	if (parameters.length > 0) {
    		pw.print(")");
    	}
    }
    
    /**
     * Prints the parameter documentation.
     * 
     * @param pw the writer to print the parameter documentation to
     * @param memberDoc the member to document the parameters for
     */
    private static void printParamTags(PrintWriter pw, ExecutableMemberDoc memberDoc) {
    	ParamTag[] paramTags = memberDoc.paramTags();
    	
    	for (ParamTag paramTag : paramTags) {
	    	printIndent(pw, 3);
    		pw.print("<param name=\"");
    		pw.print(paramTag.parameterName());
    		pw.print("\">");
    		printComment(pw, memberDoc, paramTag.inlineTags());
    		pw.println("</param>");
    	}
    }

    /**
     * Prints the return documentation.
     * 
     * @param pw the writer to print the return documentation to
     * @param memberDoc the member to document the return for
     */
    private static void printReturnTag(PrintWriter pw, ExecutableMemberDoc memberDoc) {
    	Tag[] returnDoc = memberDoc.tags("@return");
    	
    	if (returnDoc.length == 1) {
	    	printIndent(pw, 3);
    		pw.print("<returns>");
    		printComment(pw, memberDoc, returnDoc[0].inlineTags());
    		pw.println("</returns>");
    	} else if (returnDoc.length > 1) {
    		ERROR_REPORTER.printError("More than one return tag specified for '" + memberDoc.qualifiedName() + "'");
    	}
    }
    
    /**
     * Prints the exception documentation.
     * 
     * @param pw the writer to print the exception documentation to
     * @param memberDoc the member to document the exceptions for
     */
    private static void printThrowsTags(PrintWriter pw, ExecutableMemberDoc memberDoc) {
    	ThrowsTag[] throwsTags = memberDoc.throwsTags();
    	
    	for (ThrowsTag throwsTag : throwsTags) {
    		ClassDoc exceptionDoc = throwsTag.exception();
    		
    		if (exceptionDoc == null) {
    			ERROR_REPORTER.printError("Unable to locate class '" + throwsTag.exceptionName() + "' for '" + memberDoc.qualifiedName() + "'");
    			continue;
    		}
    		
	    	printIndent(pw, 3);
    		pw.print("<exception cref=\"");
    		printReference(pw, exceptionDoc, true);
    		pw.print("\">");
    		printComment(pw, memberDoc, throwsTag.inlineTags());
    		pw.println("</exception>");
    	}
    }

    /**
     * Prints the see tag documentation.
     * 
     * @param pw the writer to print the see tag documentation to
     * @param memberDoc the member to document the see tags for
     */
    private static void printSeeTags(PrintWriter pw, ProgramElementDoc memberDoc) {
    	SeeTag[] seeTags = memberDoc.seeTags();
    	
    	for (SeeTag seeTag : seeTags) {
    		printSeeTag(pw, memberDoc, seeTag, true);
    	}
    }

    /**
     * Prints the specified see tag.
     * 
     * @param pw the writer to print the see tag to
     * @param memberDoc the member to document the see tag for
     * @param seeTag the see tags to print
     * @param asSeeAlso <code>true</code> if a "seealso" tag should be printed;
     *   <code>false</code> if a "see" tag should be printed
     */
    private static void printSeeTag(PrintWriter pw, ProgramElementDoc memberDoc, SeeTag seeTag, boolean asSeeAlso) {
		String label = seeTag.label();
		String text = seeTag.text();
		boolean isAnchor = (text.startsWith("<a") && text.endsWith("</a>"));
		int endIndex = -1;
    	ProgramElementDoc referencedMemberDoc = seeTag.referencedMember();
    	
    	if (isAnchor) {
    		endIndex = text.indexOf('>') + 1;
    		
    		if (endIndex == text.length()) {
				ERROR_REPORTER.printError("Invalid anchor '" + text + "' for '" + memberDoc.qualifiedName() + "'");
    			
				printText(pw, text);
				return;
    		}
    	} else {
	    	// If the member reference is null, attempt to use the referenced class 
	    	if (referencedMemberDoc == null) {
	    		referencedMemberDoc = seeTag.referencedClass();
	    	}
			
			if (referencedMemberDoc == null) {
				ERROR_REPORTER.printError("Unable to locate reference '" + text + "' for '" + memberDoc.qualifiedName() + "'");
				
				if (label == null || label.trim().length() == 0) {
					printText(pw, text);
				} else {
					printText(pw, label);
				}
				
				return;
			}
    	}

		String type = (asSeeAlso) ? "seealso" : "see";

		if (asSeeAlso) {
			printIndent(pw, 3);
		}
		
		pw.print("<");
		pw.print(type);
		
		if (isAnchor) {
			pw.print(text.substring(2, endIndex));
			printText(pw, text.substring(endIndex, text.length() - 4));
		} else {
			pw.print(" cref=\"");
			printReference(pw, referencedMemberDoc, true);
			pw.print("\">");
			
			if (label == null || label.trim().length() == 0) {
				printReference(pw, referencedMemberDoc, false);
			} else {
				printText(pw, label);
			}
		}
		
		pw.print("</");
		pw.print(type);
		pw.print(">");
		
		if (asSeeAlso) {
			pw.println();
		}
    }

    /**
     * Prints the documentation for the specified tag.
     * 
     * @param pw the writer to print the tag documentation to
     * @param referenceDoc the member to document the tags for
     * @param label the label to print for the tag documentation
     * @param tagName the name of the tags to print the documentation for
     */
    private static void printTags(PrintWriter pw, ProgramElementDoc referenceDoc, String label, String tagName) {
    	Tag[] tags = referenceDoc.tags(tagName);
    	
    	for (Tag tag : tags) {
    		pw.print("<para><c>");
    		pw.print(label);
    		pw.print("</c> ");
    		printComment(pw, referenceDoc, tag.inlineTags());
    		pw.println("</para>");
    	}
    }
    
    /**
     * Prints the specified reference.
     * 
     * @param pw the writer to print the reference to
     * @param referenceDoc the reference to print
     * @param includeType <code>true</code> if the type identifier should be included;
     *   <code>false</code> if the type identifier should be omitted
     */
    private static void printReference(PrintWriter pw, ProgramElementDoc referenceDoc, boolean includeType) {
    	ClassDoc classDoc = (referenceDoc.isClass() || referenceDoc.isInterface()) ? (ClassDoc) referenceDoc : referenceDoc.containingClass();
    	
    	if (includeType) {
	    	if (referenceDoc.isField()) {
	    		if (referenceDoc.isFinal() && !classDoc.isInterface()) {
	    			pw.print("P:");
	    		} else {
	    			pw.print("F:");
	    		}
	    	} else if (referenceDoc.isConstructor() || referenceDoc.isMethod()) {
	        	pw.print("M:");
	    	} else {
	        	pw.print("T:");
	    	}
    	}
    	
    	pw.print(classDoc.qualifiedName());
    	
		if (referenceDoc.isField()) {
			if (classDoc.isInterface()) {
	    		pw.print(".__Fields.");
			} else {
	    		pw.print(".");
			}
		
			pw.print(referenceDoc.name());
		} else if (referenceDoc.isConstructor()) {
    		pw.print(".#ctor");
    		printParameters(pw, (ConstructorDoc) referenceDoc);
		} else if (referenceDoc.isMethod()){
    		pw.print(".");
    		pw.print(referenceDoc.name());
    		printParameters(pw, (MethodDoc) referenceDoc);
		}
    }
    
    /**
     * Prints comment tags.
     * 
     * @param pw the writer to print the comment tags to
     * @param memberDoc the member to print the comment tags for
     * @param commentTags the comment tags to print
     */
    private static void printComment(PrintWriter pw, ProgramElementDoc memberDoc, Tag[] commentTags) {
    	for (Tag tag : commentTags) {
    		if (tag instanceof SeeTag) {
    			SeeTag seeTag = (SeeTag) tag;
    			
    			printSeeTag(pw, memberDoc, seeTag, false);
    		} else {
    			String text = tag.text();

    			printText(pw, memberDoc, text);
    		}
    	}
    }

    /**
     * Prints the specified javadoc text in a .NET XML documentation format.
     * 
     * @param pw the writer to print the comment tags to
     * @param memberDoc the member to print the comment tags for
     * @param text the text to print
     */
    private static void printText(PrintWriter pw, ProgramElementDoc memberDoc, String text) {
    	char[] characters = text.toCharArray();
    	
    	for (int index = 0; index < characters.length; index++) {
			char character = characters[index];
			
			switch (character) {
			case '<':
				int x = Character.offsetByCodePoints(text, 0, index);
				if (x != index) {
					System.out.println("x = " + x);
				}
				
				int endIndex = text.indexOf('>', index);
				
				// Handle invalid HTML (use of "<" or "<>" in text)
				if (endIndex == -1 || endIndex - index < 2) {
					pw.print("&lt;");
					continue;
				}
				
				String tag = text.substring(index + 1, endIndex).trim().toLowerCase();
				boolean isEndTag = false;
				boolean isStandAloneTag = false;

				if (tag.length() > 1) {
					if (tag.startsWith("/")) {
						tag = tag.substring(1);
						isEndTag = true;
					} else if (tag.endsWith("/")) {
						tag = tag.substring(0, tag.length() - 1);
						isStandAloneTag = true;
					}
				}
				
				/*
				 * Process/convert HTML tags to .NET XML
				 */
				
				if ("p".equals(tag)) {
					// Translate <p> to <para/>; ignore end tags
					if (!isEndTag) {
						pw.print("<para/>");
					}
					
					index = endIndex;
				} else if ("br".equals(tag) || "hr".equals(tag) || "img".equals(tag)) {
					if (!isEndTag) {
						pw.print("<");
						pw.print(tag);
						pw.print("/>");
					}
					
					index = endIndex;
				} else if (OUTPUT_HTML) {
					if ("code".equals(tag)) {
						// Translate "code" tags to "c" tags
						tag = "c";
					} else if ("li".equals(tag)) {
						// Translate "li" tags to "item" tags
						tag = "item";
					} else if ("ol".equals(tag)) {
						// Translate "ol" tags to "list" tags
						tag = "list";
						
						if (!isEndTag) {
							tag += " type=\"number\"";
						}
					} else if ("pre".equals(tag)) {
						// Translate "pre" tags to "code" tags
						tag = "code";
					} else if ("ul".equals(tag)) {
						// Translate "ul" tags to "list" tags
						tag = "list";
						
						if (!isEndTag) {
							tag += " type=\"bullet\"";
						}
					}
					
					pw.print("<");
					
					if (isEndTag) {
						pw.print("/");
					}
					
					pw.print(tag);
					
					if (isStandAloneTag) {
						pw.print("/");
					}
					
					pw.print(">");
					index = endIndex;
				} else {
					pw.print("&lt;");
				}
				
				break;
            case '>':
                pw.print("&gt;");
                break;
            case '&':
            	// TODO: Update to handle HTML escape sequences (&nbsp;, &#nnnn;, etc) 
                pw.print("&amp;");
                break;
            case '\'':
                pw.print("&apos;");
                break;
            case '"':
                pw.print("&quot;");
                break;
			default:
				pw.print(character);
			}
    	}
    }

    /**
     * Prints the specified text and escapes any XML characters.
     * 
     * @param pw the writer to print the text to
     * @param text the text to print
     */
    private static void printText(PrintWriter pw, String text) {
    	char[] characters = text.toCharArray();
    	
    	for (int index = 0; index < characters.length; index++) {
			char character = characters[index];
			
			switch (character) {
			case '<':
                pw.print("&lt;");
				break;
            case '>':
                pw.print("&gt;");
                break;
            case '&':
            	// TODO: Update to handle HTML escape sequences (&nbsp;, &#nnnn;, etc) 
                pw.print("&amp;");
                break;
            case '\'':
                pw.print("&apos;");
                break;
            case '"':
                pw.print("&quot;");
                break;
			default:
				pw.print(character);
			}
    	}
    }
    
    /**
     * Prints an indentation a specified number of times.
     * 
     * @param pw the writer to print the indentations to
     * @param indentations the number of indentations to print
     */
    public static void printIndent(PrintWriter pw, int indentations) {
    	for (int i = 0; i < indentations; i++) {
    		pw.write("\t");
    	}
    }
    
    /**
     * Validates the specified file is well formed XML.
     * 
     * @param file the file to validate
     * @throws Exception if a failure occurs while validating the file
     */
    private static void validate(File file) throws Exception {
        SAXParserFactory factory = SAXParserFactory.newInstance();
		SAXParser parser = factory.newSAXParser();
		
		parser.parse(file, new DefaultHandler() {
			public void error(SAXParseException e) throws SAXException {
				fatalError(e);
			}
			
			public void fatalError(SAXParseException e) throws SAXException {
				ERROR_REPORTER.printError(e.getMessage());
				ERROR_REPORTER.printError("Line: " + e.getLineNumber() + ", Column: " + e.getColumnNumber());
				throw e;
			}
		});
    }

    /**
     * Execute IKVMDoc without the use of the javadoc executable.
     * 
     * @param args the program arguments
     */
	public static void main(String[] args) {
		Main.execute("ikvmdoc", IKVMDoc.class.getName(), args);		
	}

    /**
     * Check for ikvmdoc specific options. Returns the number of arguments that must be specified on the command
     * line for the given option. For example, "-assembly IKVM.OpenJDK.ClassLibrary.dll" would return 2.
     * 
     * @param option the option to evaluate and return the number of arguments for 
     * @return number of arguments on the command line for an option including the option name itself.
     *   Zero return means option not known. Negative value means error occurred.
     */
    public static int optionLength(String option) {
    	if (ASSEMBLY_PARAMETER.equals(option)) {
        	return 2;
    	} else if (STRICT_FINAL_FIELD_SEMANTICS_PARAMETER.equals(option)) {
    		return 1;
    	} else if (HTML_PARAMETER.equals(option)) {
    		return 1;
    	} else if (AUTHOR_PARAMETER.equals(option)) {
    		return 1;
    	} else if (DEPRECATED_PARAMETER.equals(option)) {
    		return 1;
    	} else if (SINCE_PARAMETER.equals(option)) {
    		return 1;
    	} else if (VERSION_PARAMETER.equals(option)) {
    		return 1;
    	}

    	return 0;
    }
    
    /**
     * Check that ikvmdoc options have the correct arguments.
     * 
     * @param options the options to check
     * @param reporter the error reported used to report any failures to
     * @return <code>true</code> if the options are valid;
     *   <code>false</code> if the options are invalid
     */
    public static boolean validOptions(String[][] options, DocErrorReporter reporter) {
    	ERROR_REPORTER = reporter;
    	
    	for (String[] option : options) {
    		if (ASSEMBLY_PARAMETER.equals(option[0])) {
    			ASSEMBLY_FILE = new File(option[1]);
    			
    	    	if (!ASSEMBLY_FILE.isFile() || !ASSEMBLY_FILE.exists()) {
    	    		reporter.printError("The assembly file specified '" + ASSEMBLY_FILE.getAbsolutePath() + "' is invalid.");
    	    		return false;
    	    	}
    		} else if (HTML_PARAMETER.equals(option[0])) {
    			OUTPUT_HTML = false;
    		} else if (AUTHOR_PARAMETER.equals(option[0])) {
    			OUTPUT_AUTHOR = true;
    		} else if (DEPRECATED_PARAMETER.equals(option[0])) {
    			OUTPUT_DEPRECATED = false;
    		} else if (SINCE_PARAMETER.equals(option[0])) {
    			OUTPUT_SINCE = false;
    		} else if (VERSION_PARAMETER.equals(option[0])) {
    			OUTPUT_VERSION = true;
    		}
    	}
    	
    	return true;
    }
}
