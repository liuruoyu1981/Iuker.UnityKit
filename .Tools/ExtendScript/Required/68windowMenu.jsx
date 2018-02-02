/**************************************************************************
*
*  @@@BUILDINFO@@@ 68windowMenu-2.jsx 3.5.0.17	16-March-2009
*  ADOBE SYSTEMS INCORPORATED
*  Copyright 2010 Adobe Systems Incorporated
*  All Rights Reserved.
* 
* NOTICE:  Adobe permits you to use,  modify, and  distribute this file in
* accordance with the terms of the Adobe license agreement accompanying it.
* If you have received this file from a source other than Adobe, then your
* use, modification, or distribution of it requires the prior written
* permission of Adobe.
*
**************************************************************************/

/////////////////////////////////////////////////////////////////////////////
// The Window menu

menus.window = new MenuElement ("menu", "$$$/ESToolkit/Menu/Window=&Window",
								"at the end of menubar", "window");

// Add this setup to the late startup tasks.
addDelayedTask( setupWindowsMenu );

function setupWindowsMenu()
{
    menus.updateWindowMenu();
}

// The Window menu is recreated every time a document opens or closes to 
// reflect the list of open documents.
// Note: do not call before 69helpMenu has been loaded.

function cleanupMenu( menu )
{
    if( menu )
    {
        for( var menuItem in menu )
        {
            if( menu[menuItem] instanceof MenuElement )
            {
                if( menu[menuItem].type == 'menu' )
                    cleanupMenu( menu[menuItem] );
                
                
                menu[menuItem].remove();
                menu[menuItem] = null;
            }
        }
    }
}

menus.updateWindowMenu = function()
{
    //
    // remove all menu items of window menu
    //
    cleanupMenu( menus.window );

    //
    // remove menu items for documents (separat array, so they don't appear in shortcut prefs)
    //
    cleanupMenu( menus.window.docs );

    //
    // add palette windows
    //
	for (var i = 0; i < panes.panes.length; i++)
	{
		var paneMenuPropName = "pane"+i;
		menus.window[paneMenuPropName] = new MenuElement ("command", panes.panes[i].menuText, "at the end of window", "window/pane"+i);
		
		menus.window[paneMenuPropName].window = panes.panes[i];
		menus.window[paneMenuPropName].onDisplay = function()
		{
			this.checked = workspace.effectiveVisiblePalette( this.window );
		}
		menus.window[paneMenuPropName].onSelect = function()
		{
			if( app.modalState ) return;

		    if( workspace.effectiveVisiblePalette( this.window ) )
		    {
				if( this.window.active )
					this.window.visible = false;
				else
					this.window.active = true;
		    }
		    else
		    {
			    this.window.visible = true;
			    this.window.active = true;
		    }
		}
	}

	//
	// toggle panels visibility
	//
	menus.window.menuToggleWS = new MenuElement ("command", "$$$/ESToolkit/Menu/Window/HidePanels=Hide Pane&ls",
												 "at the end of window", "window/toggle");

	if( workspaceVisible )
	    menus.window.menuToggleWS.text = localize( "$$$/ESToolkit/Menu/Window/HidePanels=Hide Pane&ls" );
    else
	    menus.window.menuToggleWS.text = localize( "$$$/ESToolkit/Menu/Window/ShowPanels=Show Pane&ls" );

	menus.window.menuToggleWS.onSelect = function()
	{
		if( app.modalState ) return;

		workspace.togglePalettes();
		
		workspaceVisible = !workspaceVisible;
		
		if( workspaceVisible )
		    this.text = localize( "$$$/ESToolkit/Menu/Window/HidePanels=Hide Pane&ls" );
        else
		{
		    this.text = localize( "$$$/ESToolkit/Menu/Window/ShowPanels=Show Pane&ls" );
			if (document)
			{
				document.deactivate();
				document.activate();
			}
		}
	}

    //
    // arrangement of documents
    //
	menus.window.nextdoc = new MenuElement ("command", "$$$/ESToolkit/Menu/Window/Next=&Next Document",
							"---at the end of window", "window/next");
	menus.window.nextdoc.onSelect = function()
	{
		if( app.modalState ) return;

		workspace.activateNextDocWindow();
	}
	menus.window.prevdoc = new MenuElement ("command", "$$$/ESToolkit/Menu/Window/Prev=&Previous Document",
							"at the end of window", "window/previous");
	menus.window.prevdoc.onSelect = function()
	{
		if( app.modalState ) return;

		workspace.activatePreviousDocWindow();
	}
	
	menus.window.casc = new MenuElement ("command", "$$$/ESToolkit/Menu/Window/Cascade=C&ascade",
							"at the end of window", "window/cascade");
	menus.window.casc.onSelect = function()
	{
		if( app.modalState ) return;

		workspace.cascadeDocuments();
	}
	
	menus.window.tile = new MenuElement ("command", "$$$/ESToolkit/Menu/Window/Tile=Tile &Documents",
							"at the end of window", "window/tile");
	menus.window.tile.onSelect = function()
	{
		if( app.modalState ) return;

		workspace.tileDocuments( "horizontal" );
	}
	
    menus.window.casc.onDisplay = 
    menus.window.tile.onDisplay = 							
	menus.window.nextdoc.onDisplay  = 
	menus.window.prevdoc.onDisplay  = function()
	{
	    var docwinCount = documents.length;
	    
	    if( docwinCount < 2 && OMV.ui )
            docwinCount++;
	    
		this.enabled = (docwinCount > 1);
	}
	
	//
	// (Mac only) Bring all document windows to front and turn Application Frame on or off
	// 
	if( !_win )
	{
	    // (no onSelect function required, Mac handles it by itself)
		menus.window.allToFront = new MenuElement( "command", "$$$/ESToolkit/Menu/Window/allToFront=Bring All to Front",
											       "at the end of window", "tools/allToFront");

		menus.window.appFrame = new MenuElement( "command", "$$$/ESToolkit/Menu/Window/applicationFrame=Application Frame",
											       "---at the end of window", "tools/toggleScreen");
								
		menus.window.appFrame.onSelect = function()
		{
			if( app.modalState ) return;

			workspace.appFrame = !workspace.appFrame;
			globalBroadcaster.notifyClients( 'appFrameChanged' );
		}	
		
		menus.window.appFrame.onDisplay = function()
		{
			this.checked = workspace.appFrame;
		}
	}

    //
    // document windows
    //
	var separator = "---";

    menus.window.docs = [];
    
	for (i = 0; i < documents.length; i++)
	{
		var doc = documents [i];
		var win = doc.rootPane;
		var text = "";

		if (i < 9)
			text = "&" + (i+1) + " ";

		if( doc.isDirty() )
			text += "* ";

		text += doc.rootPane.helpTip;
				
		menus.window.docs[i] = new MenuElement ("command", text, separator + "at the end of window");
		separator = "";
		menus.window.docs[i].window = win;
		menus.window.docs[i].onSelect = function()
		{
			if( app.modalState ) return;

		    this.window.active = true;
		    this.window.minimized = false;
		}
	}
	
	// Reload all hot keys
	menus.loadKeys();

    globalBroadcaster.notifyClients( 'updateMenu_Window' );
}

menus.updateDocumentsState = function()
{
	if( menus.window && menus.window.docs )
	{
		for( var i=0; i<documents.length; i++ )
		{
			var menu = menus.window.docs[i];

			if( menu && menu.window && menu.window.docObj )
			{
				var text = "";

				if( i < 9 )
					text = "&" + (i+1) + " ";

				if( menu.window.docObj.isDirty() )
					text += "* ";

				text += menu.window.helpTip;

				menu.text = text;
			}
		}
	}
}