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

using System;
using System.Threading;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;
using java.awt.datatransfer;
using java.awt.image;
using java.awt.peer;
using java.net;
using java.util;

namespace ikvm.awt
{
	delegate void SetVoid();
	delegate void SetBool(bool b);
	delegate void SetXYWH(int x, int y, int w, int h);
	delegate void SetString(string s);
	delegate string GetString();
	delegate void SetStringInt(string s, int i);

	public class NetToolkit : java.awt.Toolkit
	{
		private static java.awt.EventQueue eventQueue = new java.awt.EventQueue();
		private static Form bogusForm;
		private static Delegate createControlInstance;

		private delegate Control CreateControlInstanceDelegate(Type type);

		private static void MessageLoop()
		{
			createControlInstance = new CreateControlInstanceDelegate(CreateControlImpl);
			Form form = new Form();
			form.CreateControl();
			// HACK I have no idea why this line is necessary...
			IntPtr p = form.Handle;
			bogusForm = form;
			Application.Run();
		}

		private static Control CreateControlImpl(Type type)
		{
			Control control = (Control)Activator.CreateInstance(type);
			control.CreateControl();
			// HACK here we go again...
			IntPtr p = control.Handle;
			return control;
		}

		internal static Control CreateControl(Type type)
		{
			return (Control)bogusForm.Invoke(createControlInstance, new object[] { type });
		}

		public NetToolkit()
		{
			lock(typeof(NetToolkit))
			{
				if(bogusForm != null)
				{
					throw new InvalidOperationException();
				}
				Thread thread = new Thread(new ThreadStart(MessageLoop));
				thread.Start();
				// TODO don't use polling...
				while(bogusForm == null)
				{
					Thread.Sleep(1);
				}
			}
		}

		protected override java.awt.peer.ButtonPeer createButton(java.awt.Button target)
		{
			return new NetButtonPeer(target, (Button)CreateControl(typeof(Button)));
		}

		protected override java.awt.peer.TextFieldPeer createTextField(java.awt.TextField target)
		{
			return new NetTextFieldPeer(target, (TextBox)CreateControl(typeof(TextBox)));
		}

		protected override java.awt.peer.LabelPeer createLabel(java.awt.Label target)
		{
			throw new NotImplementedException();
		}

		protected override java.awt.peer.ListPeer createList(java.awt.List target)
		{
			throw new NotImplementedException();
		}

		protected override java.awt.peer.CheckboxPeer createCheckbox(java.awt.Checkbox target)
		{
			throw new NotImplementedException();
		}

		protected override java.awt.peer.ScrollbarPeer createScrollbar(java.awt.Scrollbar target)
		{
			throw new NotImplementedException();
		}

		protected override java.awt.peer.ScrollPanePeer createScrollPane(java.awt.ScrollPane target)
		{
			throw new NotImplementedException();
		}

		protected override java.awt.peer.TextAreaPeer createTextArea(java.awt.TextArea target)
		{
			return new NetTextAreaPeer(target, (TextBox)CreateControl(typeof(TextBox)));
		}

		protected override java.awt.peer.ChoicePeer createChoice(java.awt.Choice target)
		{
			throw new NotImplementedException();
		}

		protected override java.awt.peer.FramePeer createFrame(java.awt.Frame target)
		{
			return new NetFramePeer(target, (Form)CreateControl(typeof(Form)));
		}

		protected override java.awt.peer.CanvasPeer createCanvas(java.awt.Canvas target)
		{
			throw new NotImplementedException();
		}

		protected override java.awt.peer.PanelPeer createPanel(java.awt.Panel target)
		{
			return new NetPanelPeer(target, (ContainerControl)CreateControl(typeof(ContainerControl)));
		}

		protected override java.awt.peer.WindowPeer createWindow(java.awt.Window target)
		{
			throw new NotImplementedException();
		}
		protected override java.awt.peer.DialogPeer createDialog(java.awt.Dialog target)
		{
			throw new NotImplementedException();
		}
		protected override java.awt.peer.MenuBarPeer createMenuBar(java.awt.MenuBar target)
		{
			throw new NotImplementedException();
		}
		protected override java.awt.peer.MenuPeer createMenu(java.awt.Menu target)
		{
			throw new NotImplementedException();
		}
		protected override java.awt.peer.PopupMenuPeer createPopupMenu(java.awt.PopupMenu target)
		{
			throw new NotImplementedException();
		}
		protected override java.awt.peer.MenuItemPeer createMenuItem(java.awt.MenuItem target)
		{
			throw new NotImplementedException();
		}
		protected override java.awt.peer.FileDialogPeer createFileDialog(java.awt.FileDialog target)
		{
			throw new NotImplementedException();
		}
		protected override java.awt.peer.CheckboxMenuItemPeer createCheckboxMenuItem(java.awt.CheckboxMenuItem target)
		{
			throw new NotImplementedException();
		}
		protected override java.awt.peer.FontPeer getFontPeer(string name, int style)
		{
			throw new NotImplementedException();
		}
		public override java.awt.Dimension getScreenSize()
		{
			throw new NotImplementedException();
		}
		public override int getScreenResolution()
		{
			throw new NotImplementedException();
		}
		public override ColorModel getColorModel()
		{
			throw new NotImplementedException();
		}
		public override string[] getFontList()
		{
			throw new NotImplementedException();
		}
		public override java.awt.FontMetrics getFontMetrics(java.awt.Font font)
		{
			throw new NotImplementedException();
		}
		public override void sync()
		{
			throw new NotImplementedException();
		}
		public override java.awt.Image getImage(string filename)
		{
			throw new NotImplementedException();
		}
		public override java.awt.Image getImage(URL url)
		{
			throw new NotImplementedException();
		}
		public override java.awt.Image createImage(string filename)
		{
			throw new NotImplementedException();
		}
		public override java.awt.Image createImage(URL url)
		{
			throw new NotImplementedException();
		}
		public override bool prepareImage(java.awt.Image image,
			int width,
			int height,
			java.awt.image.ImageObserver observer)
		{
			throw new NotImplementedException();
		}
		public override int checkImage(java.awt.Image image,
			int width,
			int height,
			java.awt.image.ImageObserver observer)
		{
			throw new NotImplementedException();
		}
		public override java.awt.Image createImage(java.awt.image.ImageProducer producer)
		{
			throw new NotImplementedException();
		}
		public override java.awt.Image createImage(sbyte[] imagedata,
			int imageoffset,
			int imagelength)
		{
			throw new NotImplementedException();
		}
		public override java.awt.PrintJob getPrintJob(java.awt.Frame frame,
			string jobtitle,
			Properties props)
		{
			throw new NotImplementedException();
		}
		public override void beep()
		{
			throw new NotImplementedException();
		}
		public override java.awt.datatransfer.Clipboard getSystemClipboard()
		{
			throw new NotImplementedException();
		}

		protected override java.awt.EventQueue getSystemEventQueueImpl()
		{
			return eventQueue;
		}

		public override java.awt.dnd.peer.DragSourceContextPeer createDragSourceContextPeer(java.awt.dnd.DragGestureEvent dge)
		{
			throw new NotImplementedException();
		}

		public override Map mapInputMethodHighlight(java.awt.im.InputMethodHighlight highlight)
		{
			throw new NotImplementedException();
		}
	}

	class NetComponentPeer : ComponentPeer
	{
		internal readonly java.awt.Component component;
		internal readonly Control control;

		public NetComponentPeer(java.awt.Component component, Control control)
		{
			this.control = control;
			this.component = component;
			java.awt.Container parent = component.getParent();
			if(parent != null)
			{
				control.Parent = ((NetComponentPeer)parent.getPeer()).control;
			}
			setEnabled(component.isEnabled());
			component.invalidate();
			//setBounds(component.getX(), component.getY(), component.getWidth(), component.getHeight());
			control.Invoke(new SetVoid(Setup));
		}

		private void Setup()
		{
			// TODO we really only should hook these events when they are needed...
			control.KeyDown += new KeyEventHandler(OnKeyDown);
			control.KeyUp += new KeyEventHandler(OnKeyUp);
			control.KeyPress += new KeyPressEventHandler(OnKeyPress);
		}

		private static int MapKeyCode(Keys key)
		{
			switch(key)
			{
				default:
					return (int)key;
			}
		}

		protected virtual void OnKeyDown(object sender, KeyEventArgs e)
		{
			// TODO set all this stuff...
			long when = 0;
			int modifiers = 0;
			int keyCode = MapKeyCode(e.KeyCode);
			char keyChar = ' ';
			int keyLocation = 0;
			postEvent(new java.awt.@event.KeyEvent(component, java.awt.@event.KeyEvent.KEY_PRESSED, when, modifiers, keyCode, keyChar, keyLocation));
		}

		protected virtual void OnKeyUp(object sender, KeyEventArgs e)
		{
			// TODO set all this stuff...
			long when = 0;
			int modifiers = 0;
			int keyCode = MapKeyCode(e.KeyCode);
			char keyChar = ' ';
			int keyLocation = 0;
			postEvent(new java.awt.@event.KeyEvent(component, java.awt.@event.KeyEvent.KEY_RELEASED, when, modifiers, keyCode, keyChar, keyLocation));
		}

		protected virtual void OnKeyPress(object sender, KeyPressEventArgs e)
		{
			// TODO set all this stuff...
			long when = 0;
			int modifiers = 0;
			int keyCode = 0;
			char keyChar = e.KeyChar;
			int keyLocation = 0;
			postEvent(new java.awt.@event.KeyEvent(component, java.awt.@event.KeyEvent.KEY_TYPED, when, modifiers, keyCode, keyChar, keyLocation));
		}

		protected void postEvent(java.awt.AWTEvent evt)
		{
			getToolkit().getSystemEventQueue().postEvent(evt);
		}

		public int checkImage(java.awt.Image img, int width, int height, java.awt.image.ImageObserver ob)
		{
			throw new NotImplementedException();
		}
		public java.awt.Image createImage(java.awt.image.ImageProducer prod)
		{
			throw new NotImplementedException();
		}
		public java.awt.Image createImage(int width, int height)
		{
			throw new NotImplementedException();
		}
		public void disable()
		{
			throw new NotImplementedException();
		}
		public void dispose()
		{
			throw new NotImplementedException();
		}
		public void enable()
		{
			throw new NotImplementedException();
		}
		public ColorModel getColorModel()
		{
			throw new NotImplementedException();
		}
		public java.awt.FontMetrics getFontMetrics(java.awt.Font f)
		{
			throw new NotImplementedException();
		}
		public java.awt.Graphics getGraphics()
		{
			throw new NotImplementedException();
		}
		public java.awt.Point getLocationOnScreen()
		{
			throw new NotImplementedException();
		}
		public java.awt.Dimension getMinimumSize()
		{
			throw new NotImplementedException();
		}

		public virtual java.awt.Dimension getPreferredSize()
		{
			Console.WriteLine("NOTE: NetComponentPeer.getPreferredSize not implemented");
			return new java.awt.Dimension(0, 0);
		}

		public java.awt.Toolkit getToolkit()
		{
			return java.awt.Toolkit.getDefaultToolkit();
		}

		public void handleEvent(java.awt.AWTEvent e)
		{
			Console.WriteLine("NOTE: NetComponentPeer.handleEvent not implemented");
		}
		public void hide()
		{
			throw new NotImplementedException();
		}
		public bool isFocusTraversable()
		{
			throw new NotImplementedException();
		}
		public java.awt.Dimension minimumSize()
		{
			throw new NotImplementedException();
		}

		public java.awt.Dimension preferredSize()
		{
			Console.WriteLine("NOTE: NetComponentPeer.preferredSize not implemented");
			return new java.awt.Dimension(0, 0);
		}

		public void paint(java.awt.Graphics graphics)
		{
			throw new NotImplementedException();
		}
		public bool prepareImage(java.awt.Image img, int width, int height, ImageObserver ob)
		{
			throw new NotImplementedException();
		}
		public void print(java.awt.Graphics graphics)
		{
			throw new NotImplementedException();
		}
		public void repaint(long tm, int x, int y, int width, int height)
		{
			throw new NotImplementedException();
		}
		public void requestFocus()
		{
			throw new NotImplementedException();
		}
		public void reshape(int x, int y, int width, int height)
		{
			throw new NotImplementedException();
		}
		public void setBackground(java.awt.Color color)
		{
			throw new NotImplementedException();
		}

		private void setBoundsImpl(int x, int y, int width, int height)
		{
			control.SetBounds(x, y, width, height);
		}

		public void setBounds(int x, int y, int width, int height)
		{
			control.Invoke(new SetXYWH(setBoundsImpl), new object[] { x, y, width, height });
		}

		public void setCursor(java.awt.Cursor cursor)
		{
			throw new NotImplementedException();
		}

		private void setEnabledImpl(bool enabled)
		{
			control.Enabled = enabled;
		}

		public void setEnabled(bool enabled)
		{
			control.Invoke(new SetBool(setEnabledImpl), new object[] { enabled });
		}

		public void setFont(java.awt.Font font)
		{
			throw new NotImplementedException();
		}
		public void setForeground(java.awt.Color color)
		{
			throw new NotImplementedException();
		}

		private void setVisibleImpl(bool visible)
		{
			control.Visible = visible;
		}

		public void setVisible(bool visible)
		{
			control.Invoke(new SetBool(setVisibleImpl), new object[] { visible });
		}

		public void show()
		{
			throw new NotImplementedException();
		}
		public java.awt.GraphicsConfiguration getGraphicsConfiguration()
		{
			throw new NotImplementedException();
		}

		public void setEventMask (long mask)
		{
			Console.WriteLine("NOTE: NetComponentPeer.setEventMask not implemented");
		}
	}

	class NetButtonPeer : NetComponentPeer, ButtonPeer
	{
		public NetButtonPeer(java.awt.Button awtbutton, Button button)
			: base(awtbutton, button)
		{
			setLabel(awtbutton.getLabel());
			control.Invoke(new SetVoid(Setup));
		}

		private void Setup()
		{
			((Button)control).Click += new EventHandler(OnClick);
		}

		private void OnClick(object sender, EventArgs e)
		{
			// TODO set all these properties correctly
			string cmd = "";
			long when = 0;
			int modifiers = 0;
			postEvent(new java.awt.@event.ActionEvent(component, java.awt.@event.ActionEvent.ACTION_PERFORMED, cmd, when, modifiers));
		}

		private void setLabelImpl(string label)
		{
			control.Text = label;
		}

		public void setLabel(string label)
		{
			control.Invoke(new SetString(setLabelImpl), new object[] { label });
		}

		public override java.awt.Dimension getPreferredSize()
		{
			// TODO get the size from somewhere...
			return new java.awt.Dimension(80, 15);
		}
	}

	class NetTextComponentPeer : NetComponentPeer, TextComponentPeer
	{
		public NetTextComponentPeer(java.awt.TextComponent textComponent, TextBox textBox)
			: base(textComponent, textBox)
		{
		}

		protected override void OnKeyPress(object sender, KeyPressEventArgs e)
		{
			base.OnKeyPress(sender, e);
			// TODO for TextAreas this probably isn't the right behaviour
			if(e.KeyChar == '\r')
			{
				// TODO set all these properties correctly
				string cmd = "";
				long when = 0;
				int modifiers = 0;
				postEvent(new java.awt.@event.ActionEvent(component, java.awt.@event.ActionEvent.ACTION_PERFORMED, cmd, when, modifiers));
			}
		}

		public int getSelectionEnd()
		{
			throw new NotImplementedException();
		}
		public int getSelectionStart()
		{
			throw new NotImplementedException();
		}

		private string getTextImpl()
		{
			return control.Text;
		}

		public string getText()
		{
			return (string)control.Invoke(new GetString(getTextImpl));
		}

		private void setTextImpl(string text)
		{
			control.Text = text;
		}

		public void setText(string text)
		{
			control.Invoke(new SetString(setTextImpl), new object[] { text });
		}

		public void select(int start_pos, int end_pos)
		{
			throw new NotImplementedException();
		}
		public void setEditable(bool editable)
		{
			throw new NotImplementedException();
		}
		public int getCaretPosition()
		{
			throw new NotImplementedException();
		}
		public void setCaretPosition(int pos)
		{
			throw new NotImplementedException();
		}
	}

	class NetTextFieldPeer : NetTextComponentPeer, TextFieldPeer
	{
		public NetTextFieldPeer(java.awt.TextField textField, TextBox textBox)
			: base(textField, textBox)
		{
		}

		public java.awt.Dimension minimumSize(int len)
		{
			throw new NotImplementedException();
		}
		public java.awt.Dimension preferredSize(int len)
		{
			throw new NotImplementedException();
		}
		public java.awt.Dimension getMinimumSize(int len)
		{
			throw new NotImplementedException();
		}

		public java.awt.Dimension getPreferredSize(int len)
		{
			TextBox b = (TextBox)control;
			// TODO use control.Invoke
			return new java.awt.Dimension(200, b.PreferredHeight);
		}

		public void setEchoChar(char echo_char)
		{
			throw new NotImplementedException();
		}
		public void setEchoCharacter(char echo_char)
		{
			throw new NotImplementedException();
		}
	}

	class NetTextAreaPeer : NetTextComponentPeer, TextAreaPeer
	{
		public NetTextAreaPeer(java.awt.TextArea textArea, TextBox textBox)
			: base(textArea, textBox)
		{
			control.Invoke(new SetVoid(Setup));
		}

		private void Setup()
		{
			((TextBox)control).ReadOnly = !((java.awt.TextArea)component).isEditable();
			((TextBox)control).WordWrap = false;
			((TextBox)control).ScrollBars = ScrollBars.Both;
			((TextBox)control).Multiline = true;
		}

		private void insertImpl(string text, int pos)
		{
			((TextBox)control).Text = ((TextBox)control).Text.Insert(pos, text);
		}

		public void insert(string text, int pos)
		{
			control.Invoke(new SetStringInt(insertImpl), new Object[] { text, pos });
		}

		public void insertText(string text, int pos)
		{
			throw new NotImplementedException();
		}
		public java.awt.Dimension minimumSize(int rows, int cols)
		{
			throw new NotImplementedException();
		}
		public java.awt.Dimension getMinimumSize(int rows, int cols)
		{
			throw new NotImplementedException();
		}
		public java.awt.Dimension preferredSize(int rows, int cols)
		{
			throw new NotImplementedException();
		}

		public java.awt.Dimension getPreferredSize(int rows, int cols)
		{
			Console.WriteLine("NOTE: NetTextAreaPeer.getPreferredSize not implemented");
			return new java.awt.Dimension(10 * cols, 15 * rows);
		}

		public void replaceRange(string text, int start_pos, int end_pos)
		{
			throw new NotImplementedException();
		}
		public void replaceText(string text, int start_pos, int end_pos)
		{
			throw new NotImplementedException();
		}
	}

	class NetContainerPeer : NetComponentPeer, ContainerPeer
	{
		public NetContainerPeer(java.awt.Container awtcontainer, ContainerControl container)
			: base(awtcontainer, container)
		{
		}

		public java.awt.Insets insets()
		{
			throw new NotImplementedException();
		}

		public virtual java.awt.Insets getInsets()
		{
			Console.WriteLine("NOTE: NetContainerPeer.getInsets not implemented");
			return new java.awt.Insets(0, 0, 0, 0);
		}

		public void beginValidate()
		{
			Console.WriteLine("NOTE: NetContainerPeer.beginValidate not implemented");
		}

		public void endValidate()
		{
			Console.WriteLine("NOTE: NetContainerPeer.endValidate not implemented");
		}
	}

	class NetPanelPeer : NetContainerPeer, PanelPeer
	{
		public NetPanelPeer(java.awt.Panel panel, ContainerControl container)
			: base(panel, container)
		{
		}
	}

	class NetWindowPeer : NetContainerPeer, WindowPeer
	{
		public NetWindowPeer(java.awt.Window window, Form form)
			: base(window, form)
		{
		}

		public void toBack()
		{
			throw new NotImplementedException();
		}

		public void toFront()
		{
			Console.WriteLine("NOTE: NetWindowPeer.toFront not implemented");
		}
	}

	class NetFramePeer : NetWindowPeer, FramePeer
	{
		public NetFramePeer(java.awt.Frame frame, Form form)
			: base(frame, form)
		{
			setTitle(frame.getTitle());
			control.Invoke(new SetVoid(Setup));
		}

		private void Setup()
		{
			Form form = (Form)control;
			form.Resize += new EventHandler(Resize);
			form.Closing += new CancelEventHandler(Closing);
		}

		private void Closing(object sender, CancelEventArgs e)
		{
			e.Cancel = true;
			postEvent(new java.awt.@event.WindowEvent((java.awt.Window)component, java.awt.@event.WindowEvent.WINDOW_CLOSING));
		}

		private void Resize(object sender, EventArgs e)
		{
			// TODO I have no clue what I should do here...
			Rectangle r = control.Bounds;
			component.setBounds(r.X, r.Y, r.Width, r.Height);
			component.invalidate();
			component.validate();
			postEvent(new java.awt.@event.ComponentEvent(component, java.awt.@event.ComponentEvent.COMPONENT_RESIZED));
		}

		public void setIconImage(java.awt.Image image)
		{
			throw new NotImplementedException();
		}
		public void setMenuBar(java.awt.MenuBar mb)
		{
			throw new NotImplementedException();
		}
		public void setResizable(bool resizable)
		{
			throw new NotImplementedException();
		}
		private void setTitleImpl(string title)
		{
			control.Text = title;
		}
		public void setTitle(string title)
		{
			control.Invoke(new SetString(setTitleImpl), new object[] { title });
		}

		public override java.awt.Insets getInsets()
		{
			// NOTE that we're not returning the "real" insets, but the result is equivalent (I think)
			// and it doesn't require me to remap the client coordinates
			Rectangle client = control.ClientRectangle;
			// TODO use control.Invoke
			return new java.awt.Insets(0, 0, control.Height - client.Height, control.Width - client.Width);
		}
	}
}
