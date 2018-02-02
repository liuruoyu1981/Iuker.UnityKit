/**************************************************************************
*
*  @@@BUILDINFO@@@ 61fileMenu-2.jsx 3.5.0.17	16-March-2009
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
// The File menu

// Set up the File menu.

menus.file = new MenuElement ("menu", "$$$/ESToolkit/Menu/File=&File",
								 "at the end of menubar", "file");

addDelayedTask (setupFileMenu);

function setupFileMenu()
{
	///////////////////////////////////////////////////////////////////////////////

	menus.file.newFile = new MenuElement ("command", "$$$/ESToolkit/Menu/File/New=&New JavaScript",
									 "at the beginning of file", "file/new");

	menus.file.newFile.onSelect = function()
	{
		if( app.modalState ) return;
		docMgr.create();
	}

	///////////////////////////////////////////////////////////////////////////////

	menus.file.open = new MenuElement ("command", "$$$/ESToolkit/Menu/File/Open=&Open...",
									   "at the end of file", "file/open");

	menus.file.open.onSelect = function()
	{
		if( app.modalState ) return;
		app.open();
	}

	///////////////////////////////////////////////////////////////////////////////

	// The Recent Files menu
	// This menu holds as many as 9 entries. The entries are part of the this menu's
	// elements array. The menu needs to be set up as the number of entries grow.

	menus.file.recent = new MenuElement ("menu", "$$$/ESToolkit/Menu/File/Recent=Recent &Files",
											  "at the end of file", "file/recent");

	// the elements of this array are objects with { file:uri, langID:langID }

	menus.file.recent.elements = [];

	menus.file.recent.updateMenu = function()
	{
		// Cannot use "this", since we call this from a dealyed task
		var self = menus.file.recent;

		// Discard the Recent menu contents
		for (var i = 1; i <= menus.file.recent.elements.length/*prefs.recent.max.getValue( Preference.NUMBER )*/; i++)
		{
			var cmdID = "file/recent/" + i;
			var element = MenuElement.find (cmdID);
			if (element)
				element.remove();
		}

		// Strip the own user folder
		var userFolder = File ('~').fsName;

		for (i = 0; i < self.elements.length; i++)
		{
			var obj = self.elements [i];
			var cmdID = "file/recent/" + (i + 1);
			var text = "";
			if( $.os.indexOf("Win") >= 0 && !obj.backup )
				// Windows: start with an index
				text = "&" + (i+1) + " ";
			var name = decodeURIComponent (File (obj.file).fsName);
			if (name.indexOf (userFolder) == 0)
				name = "~" + name.substr (userFolder.length);
			if (name.length > 200)
				name = "..." + name.substr (name.length-197);
			// Make sure that ampersands are displayed correctly
			name.replace (/&/, "&&");
			text += name;
			element = new MenuElement ("command", text, "at the end of file/recent", cmdID);
			element.file = obj.file;
			element.index = i;
			element.enabled = true;
			element.onSelect = function()
			{	
				if( app.modalState ) return;
				// Move the entry to the top
				obj = menus.file.recent.elements [this.index];
				menus.file.recent.elements.splice (this.index, 1);
				menus.file.recent.elements.unshift (obj);

				var f = new File (this.file);
				if (docMgr.load (f) == null)
				{
				    if( prefs.recent.remunres.getValue( Preference.BOOLEAN ) )
				    {
					    // Remove from menu
					    for (var i = 0; i < menus.file.recent.elements.length; i++)
					    {
						    if( menus.file.recent.elements [i].file == f.absoluteURI ||
						        menus.file.recent.elements [i].file == this.file )
						    {
							    menus.file.recent.elements.splice (i, 1);
							    break;
						    }
					    }
					}
				}
				// Reflect in menu, but please after we leave!
				addDelayedTask (menus.file.recent.updateMenu);
			}
		}
	}

	menus.file.recent.add = function( path, langID, bkFile )
	{	
		if( !bkFiles )
			bkFiles = false;
			
		path = path.toString();
		
		for (var i = 0; i < this.elements.length; i++)
		{
			if (this.elements [i].file == path)
			{
				// remove if present
				this.elements.splice (i, 1);
				break;
			}
		}
		
		if( !bkFile && this.elements.length == prefs.recent.max.getValue( Preference.NUMBER ) - 1 )
			this.elements.pop();
			
		// insert the new name at the top
		this.elements.splice (0, 0, { file : path, langID : langID, backup : bkFile } );
		
		// update the Recent Files menu, delayed
		addDelayedTask (this.updateMenu);
	}

	menus.file.recent.onDisplay = function()
	{
		this.enabled = (this.elements.length > 0);
	}

	menus.file.recent.loadPrefs = function()
	{		
		this.elements = [];
		var count = prefs.recent.files.getLength();
		
		if( count > prefs.recent.max.getValue( Preference.NUMBER ) )
		    count == prefs.recent.max.getValue( Preference.NUMBER );
		    
		if (count == "")
			count = 0;
		if (count)
		{
			for( var i=count-1; i>=0; i-- )
			    this.add( prefs.recent.files.getValue( 'files'+i, Preference.STRING ), 
			              prefs.recent.langIDsl.getValue( 'files'+i, Preference.STRING ) );
				
			// update the Recent Files menu
		    this.updateMenu();
		}
	}

	menus.file.recent.writePrefs = function()
	{	
        var length = this.elements.length;
        
		var max = prefs.recent.max.getValue( Preference.NUMBER );
		var n   = 0;
            
		for( var i = 0; i < length; i++ )
		{
			if( n < max && !this.elements[i].backup )
			{			
				prefs.recent.files ['files'+i] = this.elements [i].file;
				prefs.recent.langIDs ['files'+i] = this.elements [i].langID;
				
				n++;
			}
		}
	}

	menus.file.recent.onNotify = function( reason, shift )
	{
		if( reason == 'shutdown' )
		{
		    globalBroadcaster.unregisterClient( this );
		    
		    if( !shift )
			    this.writePrefs();
		}
	}

	///////////////////////////////////////////////////////////////////////////////

	menus.file.save = new MenuElement ("command", "$$$/ESToolkit/Menu/File/Save=&Save",
									   "at the end of file", "file/save");

	menus.file.save.onDisplay = function()
	{
		this.enabled = document && document.isDirty() && docMgr.isActiveDocumentWin();
	}

	menus.file.save.onSelect = function()
	{
		if( app.modalState ) return;
		
		if (document)
			document.save();
	}

	///////////////////////////////////////////////////////////////////////////////

	menus.file.saveAs = new MenuElement ("command", "$$$/ESToolkit/Menu/File/SaveAs=Save &As...",
										 "at the end of file", "file/saveAs");

	menus.file.saveAs.onDisplay = function()
	{
		this.enabled = docMgr.isActiveDocumentWin();
	}

	menus.file.saveAs.onSelect = function()
	{
		if( app.modalState ) return;

		if (document)
			document.save (true);
	}

	///////////////////////////////////////////////////////////////////////////////

	menus.file.saveAll = new MenuElement ("command", "$$$/ESToolkit/Menu/File/SaveAll=Save A&ll",
										  "at the end of file", "file/saveAll");

	menus.file.saveAll.onDisplay = function()
	{
		var countModified = 0;
		
		for( var i=0; i<documents.length; i++ )
			countModified += ( documents[i].isDirty() ? 1 : 0 );

		this.enabled = ( countModified > 0 );
	}

	menus.file.saveAll.onSelect = function()
	{
		if( app.modalState ) return;

		docMgr.saveAll();
	}

	///////////////////////////////////////////////////////////////////////////////

	menus.file.exportAs = new MenuElement ("command", "$$$/ESToolkit/Menu/File/ExportAs=&Export as Binary...",
										   "---at the end of file", "file/exportAs");

	menus.file.exportAs.onDisplay = function()
	{
		this.enabled = document && (document.langID == "js") && docMgr.isActiveDocumentWin();
	}

	menus.file.exportAs.onSelect = function()
	{
		if( app.modalState ) return;

		if( document && document.isSourceDocument )
			document.exportAsBinary();
	}

	///////////////////////////////////////////////////////////////////////////////

	menus.file.close = new MenuElement ("command", "$$$/ESToolkit/Menu/File/Close=&Close",
										"---at the end of file", "file/close");

	menus.file.close.onDisplay = function()
	{
		this.enabled = ( docMgr.isActiveDocumentWin() || ( OMV.ui && OMV.ui.w && OMV.ui.w.visible && prefs.omv.win.getValue( Preference.STRING ) != "docked" ) );
	}

	menus.file.close.onSelect = function()
	{
		if( app.modalState ) return;

		if( OMV.ui && OMV.ui.w && OMV.ui.w.visible && OMV.ui.w.active && workspace.hasFocus( OMV.ui.w ) && prefs.omv.win.getValue( Preference.STRING ) != "docked" ) 
		{
			OMV.ui.w.close();
		}
		else if (document)
			document.close();
	}

	///////////////////////////////////////////////////////////////////////////////

	menus.file.closeAll = new MenuElement ("command", "$$$/ESToolkit/Menu/File/CloseAll=Close All",
										   "at the end of file", "file/closeAll");

	menus.file.closeAll.onDisplay = function()
	{
		this.enabled = (documents.length > 0);
	}

	menus.file.closeAll.onSelect = function()
	{
		if( app.modalState ) return;

	    var tmp = [];
	    
	    for( var i=0; i<documents.length; i++ )
	        tmp.push( documents[i] );
	    
	    for( i=0; i<tmp.length; i++ )
	        tmp[i].close();
	}

	///////////////////////////////////////////////////////////////////////////////

	menus.file.page = new MenuElement ("command", "$$$/ESToolkit/Menu/File/PageSetup=Page Set&up...",
										"---at the end of file", "file/page");

	menus.file.print = new MenuElement ("command", "$$$/ESToolkit/Menu/File/Print=&Print...",
										   "at the end of file", "file/print");

	menus.file.page.onDisplay = function()
	{
		this.enabled = (documents.length > 0 );
	}
	menus.file.print.onDisplay = function()
	{
		this.enabled = docMgr.isActiveDocumentWin();
	}

	menus.file.page.onSelect = function()
	{
		if( app.modalState ) return;

		app.print.pageSetup();
	}

	menus.file.print.onSelect = function()
	{
		if( app.modalState ) return;

		if( document                                                            && 
		    document.isSourceDocument                                           && 
		    app.print.printSettings( document.getTextSelection().length > 0 )     )
		{
			if( !document.print( app.print ) )
				errorBox( localize( "$$$/ESToolkit/Error/Print=Can't print %1", document.scriptID ) );
		}
	}

	///////////////////////////////////////////////////////////////////////////////

	var itemText = '';
	
	if( _win )
		itemText = localize( "$$$/ESToolkit/Menu/File/Exit=E&xit" );
	else
		itemText = localize( "$$$/ESToolkit/Menu/File/Quit=Quit %1", app.name );
		
	menus.file.quit = new MenuElement ("command", itemText, "---at the end of file", "file/quit" );

	menus.file.quit.onSelect = function()
	{
		if( app.modalState ) return;

		app.quit();
	}

	// Finally, load the Recent Files from prefs
    if( startupPrefsLoaded )
		menus.file.recent.loadPrefs();
		
	// check kfor backup files
	var bkFolder = new Folder( app.prefsFolder + "/backup" );
	
	if( bkFolder && bkFolder.exists )
	{
		var bkFiles = bkFolder.getFiles( "*.jsx" );
		
		for( var bks=0; bks<bkFiles.length; bks++ )
			menus.file.recent.add( bkFiles[bks].absoluteURI, lang.getLanguageForFile( bkFiles[bks] ), true );
	}

    globalBroadcaster.registerClient( menus.file.recent, 'shutdown' );
    
    globalBroadcaster.notifyClients( 'updateMenu_File' );
}
