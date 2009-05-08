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
package sun.awt;

import java.awt.Button;
import java.awt.Canvas;
import java.awt.Checkbox;
import java.awt.CheckboxMenuItem;
import java.awt.Choice;
import java.awt.Dialog;
import java.awt.FileDialog;
import java.awt.Frame;
import java.awt.HeadlessException;
import java.awt.Label;
import java.awt.Menu;
import java.awt.MenuBar;
import java.awt.MenuItem;
import java.awt.Panel;
import java.awt.PopupMenu;
import java.awt.ScrollPane;
import java.awt.Scrollbar;
import java.awt.TextArea;
import java.awt.TextField;
import java.awt.Toolkit;
import java.awt.Window;
import java.awt.peer.ButtonPeer;
import java.awt.peer.CanvasPeer;
import java.awt.peer.CheckboxMenuItemPeer;
import java.awt.peer.CheckboxPeer;
import java.awt.peer.ChoicePeer;
import java.awt.peer.DialogPeer;
import java.awt.peer.FileDialogPeer;
import java.awt.peer.FontPeer;
import java.awt.peer.FramePeer;
import java.awt.peer.LabelPeer;
import java.awt.peer.ListPeer;
import java.awt.peer.MenuBarPeer;
import java.awt.peer.MenuItemPeer;
import java.awt.peer.MenuPeer;
import java.awt.peer.PanelPeer;
import java.awt.peer.PopupMenuPeer;
import java.awt.peer.ScrollPanePeer;
import java.awt.peer.ScrollbarPeer;
import java.awt.peer.TextAreaPeer;
import java.awt.peer.TextFieldPeer;
import java.awt.peer.WindowPeer;

/**
 * This class solve a compiler problem with changing the visibility of abstract methods in SunToolkit.
 * This class can be removed if this compiler problems are solved by IKVMC.
 */
public abstract class AbstractDummyToolkit extends Toolkit{

    @Override
    protected PanelPeer createPanel(Panel target) {
        return null;
    }

    @Override
    protected CanvasPeer createCanvas(Canvas target) {
        return null;
    }
    
    @Override
    protected WindowPeer createWindow(Window target) throws HeadlessException{
        return null;
    }


    @Override
    protected FramePeer createFrame(Frame target) throws HeadlessException{
        return null;
    }


    @Override
    protected DialogPeer createDialog(Dialog target) throws HeadlessException{
        return null;
    }


    @Override
    protected ButtonPeer createButton(Button target) throws HeadlessException{
        return null;
    }


    @Override
    protected TextFieldPeer createTextField(TextField target) throws HeadlessException{
        return null;
    }


    @Override
    protected ChoicePeer createChoice(Choice target) throws HeadlessException{
        return null;
    }


    @Override
    protected LabelPeer createLabel(Label target) throws HeadlessException{
        return null;
    }


    @Override
    protected ListPeer createList(java.awt.List target) throws HeadlessException{
        return null;
    }


    @Override
    protected CheckboxPeer createCheckbox(Checkbox target) throws HeadlessException{
        return null;
    }


    @Override
    protected ScrollbarPeer createScrollbar(Scrollbar target) throws HeadlessException{
        return null;
    }


    @Override
    protected ScrollPanePeer createScrollPane(ScrollPane target) throws HeadlessException{
        return null;
    }


    @Override
    protected TextAreaPeer createTextArea(TextArea target) throws HeadlessException{
        return null;
    }


    @Override
    protected FileDialogPeer createFileDialog(FileDialog target) throws HeadlessException{
        return null;
    }


    @Override
    protected MenuBarPeer createMenuBar(MenuBar target) throws HeadlessException{
        return null;
    }


    @Override
    protected MenuPeer createMenu(Menu target) throws HeadlessException{
        return null;
    }


    @Override
    protected PopupMenuPeer createPopupMenu(PopupMenu target) throws HeadlessException{
        return null;
    }


    @Override
    protected MenuItemPeer createMenuItem(MenuItem target) throws HeadlessException{
        return null;
    }


    @Override
    protected CheckboxMenuItemPeer createCheckboxMenuItem(CheckboxMenuItem target) throws HeadlessException{
        return null;
    }


    @Override
    protected FontPeer getFontPeer(String name, int style){
        return null;
    }
}
