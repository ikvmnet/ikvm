/*
  Copyright (C) 2002 Jeroen Frijters

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

package ikvm.awt;

import java.awt.*;
import java.awt.datatransfer.*;
import java.awt.image.*;
import java.awt.peer.*;
import java.net.*;
import java.util.*;
import java.awt.List;

public class NetToolkit extends Toolkit
{
	private static EventQueue eventQueue = new EventQueue();

	// NOTE "native" is just an easy way to say I haven't implemented it yet

	protected java.awt.peer.ButtonPeer createButton(Button target) throws HeadlessException
	{
		return new NetButtonPeer();
	}

	protected java.awt.peer.TextFieldPeer createTextField(TextField target) throws HeadlessException
	{
		return new NetTextFieldPeer();
	}

	protected native java.awt.peer.LabelPeer createLabel(Label target) throws HeadlessException;
	protected native java.awt.peer.ListPeer createList(List target) throws HeadlessException;
	protected native java.awt.peer.CheckboxPeer createCheckbox(Checkbox target)
		throws HeadlessException;
	protected native java.awt.peer.ScrollbarPeer createScrollbar(Scrollbar target)
		throws HeadlessException;
	protected native java.awt.peer.ScrollPanePeer createScrollPane(ScrollPane target)
		throws HeadlessException;

	protected java.awt.peer.TextAreaPeer createTextArea(TextArea target) throws HeadlessException
	{
		return new NetTextAreaPeer();
	}

	protected native java.awt.peer.ChoicePeer createChoice(Choice target)
		throws HeadlessException;

	protected java.awt.peer.FramePeer createFrame(Frame target) throws HeadlessException
	{
		return new NetFramePeer();
	}

	protected native java.awt.peer.CanvasPeer createCanvas(Canvas target);

	protected java.awt.peer.PanelPeer createPanel(Panel target)
	{
		return new NetPanelPeer();
	}

	protected native java.awt.peer.WindowPeer createWindow(Window target)
		throws HeadlessException;
	protected native java.awt.peer.DialogPeer createDialog(Dialog target)
		throws HeadlessException;
	protected native java.awt.peer.MenuBarPeer createMenuBar(MenuBar target)
		throws HeadlessException;
	protected native java.awt.peer.MenuPeer createMenu(Menu target)
		throws HeadlessException;
	protected native java.awt.peer.PopupMenuPeer createPopupMenu(PopupMenu target)
		throws HeadlessException;
	protected native java.awt.peer.MenuItemPeer createMenuItem(MenuItem target)
		throws HeadlessException;
	protected native java.awt.peer.FileDialogPeer createFileDialog(FileDialog target)
		throws HeadlessException;
	protected native java.awt.peer.CheckboxMenuItemPeer createCheckboxMenuItem(CheckboxMenuItem target)
		throws HeadlessException;
	protected native java.awt.peer.FontPeer getFontPeer(String name, int style);
	public native Dimension getScreenSize() throws HeadlessException;
	public native int getScreenResolution() throws HeadlessException;
	public native ColorModel getColorModel()
		throws HeadlessException;
	public native String[] getFontList();
	public native FontMetrics getFontMetrics(Font font);
	public native void sync();
	public native Image getImage(String filename);
	public native Image getImage(URL url);
	public native Image createImage(String filename);
	public native Image createImage(URL url);
	public native boolean prepareImage(Image image,
		int width,
		int height,
		ImageObserver observer);
	public native int checkImage(Image image,
		int width,
		int height,
		ImageObserver observer);
	public native Image createImage(ImageProducer producer);
	public native Image createImage(byte[] imagedata,
		int imageoffset,
		int imagelength);
	public native PrintJob getPrintJob(Frame frame,
		String jobtitle,
		Properties props);
	public native void beep();
	public native Clipboard getSystemClipboard()
		throws HeadlessException;

	protected EventQueue getSystemEventQueueImpl()
	{
		return eventQueue;
	}

//	public native java.awt.dnd.peer.DragSourceContextPeer createDragSourceContextPeer(DragGestureEvent dge)
//		throws InvalidDnDOperationException;
//	public native Map mapInputMethodHighlight(InputMethodHighlight highlight)
//		throws HeadlessException;
}

class NetComponentPeer implements ComponentPeer
{
	public native int checkImage(Image img, int width, int height, ImageObserver ob);
	public native Image createImage(ImageProducer prod);
	public native Image createImage(int width, int height);
	public native void disable();
	public native void dispose();
	public native void enable();
	public native ColorModel getColorModel();
	public native FontMetrics getFontMetrics(Font f);
	public native Graphics getGraphics();
	public native Point getLocationOnScreen();
	public native Dimension getMinimumSize();

	public Dimension getPreferredSize()
	{
		System.out.println("NOTE: NetComponentPeer.getPreferredSize not implemented");
		return new Dimension(0, 0);
	}

	public Toolkit getToolkit()
	{
		return Toolkit.getDefaultToolkit();
	}

	public native void handleEvent(AWTEvent e);
	public native void hide();
	public native boolean isFocusTraversable();
	public native Dimension minimumSize();

	public Dimension preferredSize()
	{
		System.out.println("NOTE: NetComponentPeer.preferredSize not implemented");
		return new Dimension(0, 0);
	}

	public native void paint(Graphics graphics);
	public native boolean prepareImage(Image img, int width, int height, ImageObserver ob);
	public native void print(Graphics graphics);
	public native void repaint(long tm, int x, int y, int width, int height);
	public native void requestFocus();
	public native void reshape(int x, int y, int width, int height);
	public native void setBackground(Color color);
	public native void setBounds(int x, int y, int width, int height);
	public native void setCursor(Cursor cursor);

	public void setEnabled(boolean enabled)
	{
		System.out.println("NOTE: NetComponentPeer.setEnabled not implemented");
	}

	public native void setFont(Font font);
	public native void setForeground(Color color);

	public void setVisible(boolean visible)
	{
		System.out.println("NOTE: NetComponentPeer.setVisible not implemented");
	}

	public native void show();
	public native GraphicsConfiguration getGraphicsConfiguration();

	public void setEventMask (long mask)
	{
		System.out.println("NOTE: NetComponentPeer.setEventMask not implemented");
	}
}

class NetButtonPeer extends NetComponentPeer implements ButtonPeer
{
	public native void setLabel(String label);
}

class NetTextComponentPeer extends NetComponentPeer implements TextComponentPeer
{
	public native int getSelectionEnd();
	public native int getSelectionStart();

	public String getText()
	{
		System.out.println("NOTE: NetTextComponentPeer.getText not implemented");
		return "";
	}

	public void setText(String text)
	{
		System.out.println("NOTE: NetTextComponentPeer.setText not implemented");
	}

	public native void select(int start_pos, int end_pos);
	public native void setEditable(boolean editable);
	public native int getCaretPosition();
	public native void setCaretPosition(int pos);
}

class NetTextFieldPeer extends NetTextComponentPeer implements TextFieldPeer
{
	public native Dimension minimumSize(int len);
	public native Dimension preferredSize(int len);
	public native Dimension getMinimumSize(int len);

	public Dimension getPreferredSize(int len)
	{
		System.out.println("NOTE: NetTextFieldPeer.getPreferredSize not implemented");
		return new Dimension(0, 0);
	}

	public native void setEchoChar(char echo_char);
	public native void setEchoCharacter(char echo_char);
}

class NetTextAreaPeer extends NetTextComponentPeer implements TextAreaPeer
{
	public void insert(String text, int pos)
	{
		System.out.println("NOTE: NetTextAreaPeer.insert not implemented");
	}

	public native void insertText(String text, int pos);
	public native Dimension minimumSize(int rows, int cols);
	public native Dimension getMinimumSize(int rows, int cols);
	public native Dimension preferredSize(int rows, int cols);

	public Dimension getPreferredSize(int rows, int cols)
	{
		System.out.println("NOTE: NetTextAreaPeer.getPreferredSize not implemented");
		return new Dimension(0, 0);
	}

	public native void replaceRange(String text, int start_pos, int end_pos);
	public native void replaceText(String text, int start_pos, int end_pos);
}

class NetContainerPeer extends NetComponentPeer implements ContainerPeer
{
	public native Insets insets();

	public Insets getInsets()
	{
		System.out.println("NOTE: NetContainerPeer.getInsets not implemented");
		return new Insets(0, 0, 0, 0);
	}

	public void beginValidate()
	{
		System.out.println("NOTE: NetContainerPeer.beginValidate not implemented");
	}

	public void endValidate()
	{
		System.out.println("NOTE: NetContainerPeer.endValidate not implemented");
	}
}

class NetPanelPeer extends NetContainerPeer implements PanelPeer
{
}

class NetWindowPeer extends NetContainerPeer implements WindowPeer
{
	public native void toBack();

	public void toFront()
	{
		System.out.println("NOTE: NetWindowPeer.toFront not implemented");
	}
}

class NetFramePeer extends NetWindowPeer implements FramePeer
{
	NetFramePeer()
	{
	}

	public native void setIconImage(Image image);
	public native void setMenuBar(MenuBar mb);
	public native void setResizable(boolean resizable);
	public native void setTitle(String title);
}
