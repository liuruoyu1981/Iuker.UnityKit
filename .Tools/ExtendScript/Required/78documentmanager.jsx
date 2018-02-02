/**************************************************************************
*
*  @@@BUILDINFO@@@ 78documentmanager-2.jsx 3.5.0.48	14-December-2009
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

///////////////////////////////////////////////////////////////////////////////
//
// Document manager
//

//
// Global properties are defined in 00global.jsx:
//
//  document    - The current active document
//  documents   - Array of all opened documents
//

function DocumentManager( factory )
{
    this.autoCompletion = null;
    this.defaultFactory = factory;
    
    this.factoriesExt   = {};
    this.factoriesLang  = {};
        
    this.windowID = 0;		// next source ID appended to title
    this.sourceID = 0;		// next script ID attached to name
    this.scriptID = 0;		// next document ID for the window
}

//-----------------------------------------------------------------------------
// 
// find(...)
// 
// Purpose: Find opened document with given scriptID
// 
//-----------------------------------------------------------------------------

DocumentManager.prototype.find = function( scriptID )
{
    for( var i=0; i<documents.length; i++ )
	{
		if( documents [i].scriptID == scriptID )
			return documents [i];
	}
	
	return null;
}

//-----------------------------------------------------------------------------
// 
// create(...)
// 
// Purpose: Create new document
// 
//-----------------------------------------------------------------------------

DocumentManager.prototype.create = function( title, text, langID, scriptID, readPreferences, master, dockToDocument, startup )
{
	workspace.resetFocus();	// remove any stored focus info
	
    var win = null;

    // reset global remote flag
    remoteLaunched = false;

    //
    // set defaults
    //    
    if( !title )
		title = localize( "$$$/ESToolkit/DefaultSourceName=Source%1", ++this.sourceID );
		
	if( !langID || langID.toString().length <= 0 )
	    langID = prefs.document.language.getValue( Preference.STRING );
	if( !langID || langID.toString().length <= 0 )
		langID = "js";
		
	if( !scriptID )
		scriptID = "(Script" + ++this.scriptID + ")";	

	if( !text )
	    text = "";
	    
	//
	// setup master
	//
	var masterDoc = master;
	
    while( master )
    {
        if( master.master )
            masterDoc = master.master;
            
        master = master.master;
    }

    //
    // get factory function for given language
    //	    
	var factory = this.findFactory( scriptID, langID );
	
	if( !factory )
	    factory = this.defaultFactory;
	
	if( factory )
	{
	    //
	    // create document window (document panel)
	    //
	    win        = this.createWindow( masterDoc, dockToDocument, readPreferences, startup );
	    var docObj = null;
	    
	    //
	    // create document object using the factory function
	    //
	    try
	    {
	        docObj = factory( title, langID, scriptID, readPreferences, master, dockToDocument );
	    }
	    catch( exc )
	    {
	    // TODO
	    }
	    
	    //
	    // setup document object
	    //
	    if( docObj )
	    {
    	    win.__docObj__  = docObj;
    	    docObj.rootPane = win;
    	    
            //-----------------------------------------------------------------------------
            // 
            // docObj.isFile(...)
            // 
            // Purpose: Is this document a disk file?
            // 
            //-----------------------------------------------------------------------------

	        docObj.isFile = function()
            {
                return docMgr.isFile( this.scriptID );
            }
	        
            //-----------------------------------------------------------------------------
            // 
            // docObj.setScriptID(...)
            // 
            // Purpose: Set documents scriptID
            // 
            //-----------------------------------------------------------------------------

            docObj.setScriptID = function( scriptID, masterCmd )
            {
                if( this.duplicate )
                {
                    if( masterCmd )
                        this.scriptID = scriptID;
                    else
                        this.master.setScriptID( scriptID );
                }
                else
                {
	                if( this.scriptID != scriptID )
	                {
		                if( this.isFile() )
			                app.removeFileWatch( this, File( this.scriptID ) );

		                this.scriptID = scriptID;
		                
		                for( var i=0; i<this.duplicates.length; i++ )
		                    this.duplicates[i].setScriptID( scriptID, true );
                		
		                if( this.isFile() )
			                app.addFileWatch( this, File( this.scriptID ) );						
	                }
	            }
				
				try
				{
					//
					// Mac: Show icon that the user can Cmd+Click on
					// to view the file's location
					//
					if( this.isFile() )							
						this.rootPane.proxy = this.scriptID;
				}
				catch( exc )
				{}
            }
	        
	        //-----------------------------------------------------------------------------
	        // 
	        // docObj.setCursorPos(...)
	        // 
	        // Purpose: Set position field of status bar
	        // 
	        //-----------------------------------------------------------------------------
	        
            docObj.setCursorPos = function( x, y )
            {
                this.rootPane.statusGroup.posStatus.statustext.text = localize( '$$$/ESToolkit/Panes/Statusbar/Line=Line' ) + ' ' + (x+1) + '    ' + localize( '$$$/ESToolkit/Panes/Statusbar/Column=Column' ) + ' ' + (y+1);
            }
            
            docObj.clearCursorPos = function()
            {
                this.rootPane.statusGroup.posStatus.statustext.text = '';
            }
	        
            //-----------------------------------------------------------------------------
            // 
            // docObj.setTitle(...)
            // 
            // Purpose: Set the title with readonly and modified indicators
            // 
            //-----------------------------------------------------------------------------

            docObj.setTitle = function( masterCommand )
            {
                if( this.duplicate && !masterCommand )
                    this.master.setTitle();
                else
                {
                    var titleLong   = this.paneTitle;
                    var titleShort  = decodeURIComponent (this.fileName);
                    var titleNormal = titleShort;
                    
                    if( titleLong.length )
                    {
                        if( this.roState )
                        {
                            titleLong = localize ("$$$/ESToolkit/Status/ReadOnlyIndicator=[R/O]") + " " + titleLong;
                            titleShort = localize ("$$$/ESToolkit/Status/ReadOnlyIndicator=[R/O]") + " " + titleShort;
                        }
                        
                        if( this.duplicate )
                        {
                            var num = 0;
                            
                            for( var i=0; i<this.master.duplicates.length; i++ )
                            {
                                num++;
                                
                                if( this.master.duplicates[i] == this )
                                    break;
                            }
                            
                            titleLong = "#" + num + " " + titleLong;
                            titleShort = "#" + num + " " + titleShort;
                        }
                        
                        if( this.isDirty() )
			                titleShort = "* " + titleShort;
            			                
                        if( this.rootPane.text != titleNormal )
                            this.rootPane.text = titleNormal;
                        
                        if( this.rootPane.extendedTitle != titleShort )
                            this.rootPane.extendedTitle = titleShort;
                        
                        if( this.rootPane.helpTip != titleLong )
                            this.rootPane.helpTip = titleLong;

						if( this.isFile())
							this.rootPane.filePath = titleLong;

						menus.updateDocumentsState();
                    }
                    
                    for( var i=0; i<this.duplicates.length; i++ )
                        this.duplicates[i].setTitle( true );
                }
            }
            
            //-----------------------------------------------------------------------------
            // 
            // docObj.close(...)
            // 
            // Purpose: Close the document (return false on error or abort)
            // 
            //-----------------------------------------------------------------------------

            docObj.close = function()
            {
                //
                // if this is a master document then the duplicates
                // are closed on the onClose handler
                //
                
	            // do not close non-documents
	            if( this.paneTitle && this.rootPane )
		            return this.rootPane.close();
		            
	            return true;
            }

            //-----------------------------------------------------------------------------
            // 
            // docObj.save(...)
            // 
            // Purpose: Save the document:
            //          saveAs  - invoke Save As
            //          ask     - ask before saving
            //
            //          retuns true if saved, false on errors or if the result of asking 
            //          was Cancel
            // 
            //-----------------------------------------------------------------------------

            docObj.save = function( saveAs, ask )
            {
                if( this.duplicate )
                    return this.master.save( saveAs, ask );
                else
                {
	                // do not save non-documents
	                if (!this.paneTitle)
		                return true;

	                if( ask )
	                {	            
                        if( app.enableStandardUI )
                        {
                            switch( app.saveAlert( decodeURIComponent( this.fileName ) ) )
                            {
                                case -1:	return false;
                                case 0:		return true;
                            }
                        }
                        else
                            return true;
	                }

	                var f = null;

	                if( saveAs )
	                {	
	                    if( !this.currentFolder )
	                        this.currentFolder = app.currentFolder;
                	        
                        var obj = lang.getLexerAndStyles( this.langID );	        
                        
                        var proposedName = this.fileName;
                        var fileExt      = '.' + obj.defFileExt;
                        
                        if( proposedName.indexOf( fileExt ) < 0 )
                            proposedName += fileExt;
                        	        
		                var f = File( this.currentFolder + '/' + proposedName );
		                f = f.saveDlg( localize( "$$$/ESToolkit/FileDlg/SaveAs=Save As" ), lang.buildFileTypes( this.langID ) );
                		
		                if( f )
		                    // remember the folder
		                    this.currentFolder = app.currentFolder = ( f.parent ? f.parent.absoluteURI : "/" );
                		
	                }
	                else
	                {
		                // normal save
		                // do nothing if doc is not modified
		                if( !this.isDirty() )
			                return true;
                			
		                if( this.scriptID[0] == '(' )
			                // no file name yet
			                return this.save( true );

                    // TODO: save to target
		                f = new File (this.scriptID);
	                }

	                if( f )
	                {
		                // if the selected file already is open, do not save
		                var doc = docMgr.find( f.absoluteURI );
    		            
		                if( doc && doc != this )
		                {
			                errorBox( localize( "$$$/ESToolkit/FileDlg/InUse=File %1 is currently opened and cannot be overwritten.", decodeURIComponent(f.name) ) );
			                return false;
		                }

		                // If the file is read-only, abort
		                if( f.readonly )
		                {
			                var msg = _win
					                ? "$$$/ESToolkit/FileDlg/ProtectedWin=Could not save as %1 because the file is locked.^nUse the 'Properties' command in the Windows Explorer to unlock the file."
					                : "$$$/ESToolkit/FileDlg/ProtectedMac=Could not save as %1 because the file is locked.^nUse the 'Get Info' command in the Finder to unlock the file.";
			                errorBox( localize( msg, decodeURIComponent(f.name) ) );
                			
			                return false;
		                }
                		
		                //
		                // save the source
		                //
                		
                        //
		                // Remove from Folder Watch to inhibit own notification
		                //
		                app.removeFileWatch( this, f );

                        var success = false;
                        		
		                try
		                {
		                    success = app.saveText( this.getText(), f, this.lf, this.encoding, prefs.document.BOM.getValue( Preference.NUMBER ) );
		                }
		                catch( error )
		                {
		                    var errMsg = localize( "$$$/ESToolkit/FileDlg/CannotWrite=Cannot write to file %1!", decodeURIComponent( f.name ) );
                		    
		                    if( error.toString().length > 0 )
		                        errMsg += "\n\n" + error.toString();
                		        
		                    errorBox( errMsg );
		                }
                		
		                //
		                // we are good citizen
		                // and re-enable file watch
		                //
						try
						{
							app.notifyFileChanged (f);
						}
						catch( exc )
						{}
						
		                app.addFileWatch( this, f );

                        if( success )
		                {
			                this.setSaved( f.absoluteURI );

	                        if( saveAs )
	                        {
	                            scripts.fillScripts();
	                            menus.updateWindowMenu();
                            }
                                    	    
			                return true;
		                }
	                }
                	
	                return false;
	            }
            }

            //-----------------------------------------------------------------------------
            // 
            // docObj.reload(...)
            // 
            // Purpose: Reload document contents (if not modified)
            // 
            //-----------------------------------------------------------------------------

            docObj.reload = function( thisObj )
            {
                var thisObject = thisObj;
                
                if( !thisObject )
                    thisObject = this;
                    
                if( thisObject.isFile() && !thisObject.isDirty() )
                {
                    var myFile = new File( thisObject.scriptID );
                    
		            myFile.encoding = thisObject.encoding;
		            var text = docMgr.safeRead( myFile );
		            
		            if (null != text)
		            {
			            thisObject.encoding = myFile.encoding;
		                thisObject.setText( text, false );
			            thisObject.setSavePoint();
			            thisObject.setTitle();
	                }

                }
            }

            //-----------------------------------------------------------------------------
            // 
            // docObj.setSaved(...)
            // 
            // Purpose: Set the saved state
            // 
            //-----------------------------------------------------------------------------

            docObj.setSaved = function( uri, masterCommand )
            {
                if( this.duplicate && !masterCommand )
                    this.master.setSaved( uri );
                else
                {
                    if( uri )
		                this.setScriptID( uri );

	                if( this.isFile() )
	                {
		                var f           = File (this.scriptID);
		                this.paneTitle  = f.fsName;
		                this.fileName   = f.name;
		                this.roState    = f.readonly;

                        if( !this.langID && uri )
                        {
                            var langID = lang.getLanguageForFile(f);
                    		
		                    if( langID )
			                    this.setLanguage( langID );
			            }

		                if( !this.duplicate && menus.file.recent )
						{
							var bkFolder = new Folder( app.prefsFolder + "/backup" );
							
							if( f.absoluteURI.indexOf( bkFolder.absoluteURI ) != 0 )
								menus.file.recent.add( this.scriptID, this.langID );
						}
	                }
                	
	                this.setSavePoint();
	                this.setTitle();
	                
	                for( var i=0; i<this.duplicates.length; i++ )
	                    this.duplicates[i].setSaved( uri, true );
	            }
            }
            
            //-----------------------------------------------------------------------------
            // 
            // docObj.getBusyID(...)
            // 
            // Purpose: Return array of busyID's
            // 
            //-----------------------------------------------------------------------------

            docObj.getBusyID = function()
            {
                if( this.duplicate )
                    return this.master.getBusyID();
                else
                    return this.busyIDs;
            }
            
            //-----------------------------------------------------------------------------
            // 
            // docObj.getDuplicateIndex(...)
            // 
            // Purpose: If this is an duplicate then return the array index of the duplicate
			//			in the duplicates array of the master. Otherwise return -1.
            // 
            //-----------------------------------------------------------------------------

			docObj.getDuplicateIndex = function()
			{
				var index = -1;

                if( this.duplicate )
                {
                    for( var i=0; i<this.master.duplicates.length; i++ )
                    {
                        if( this.master.duplicates[i] == this )
						{
							index = i;
                            break;
						}
                    }
                }

				return index;
			}

            // Each Document instance has the following extra properties:
            //
            // busyID    - identifier of busy animation
            // paneTitle - the original title to display
            // fileName	 - the file name part of the title
            // roState   - true if the document is logically read only (it can be modified, though)
            // scriptID  - the script ID; either a full pathname, (ScriptN) for new scripts, or a target ID
            // langID	 - the lexer language (default: js)
            // lf		 - the line endings for Save
            // encoding	 - the file encoding for Save
            // lastAutoComplete - holds the last autocomplete string to inhibit double calls
            // restored  - document preferences where restored
            
	        docObj.duplicate        = masterDoc ? true : false;         // am I a duplicate of another document?
	        docObj.master           = masterDoc ? masterDoc : null;     // the original document if I'm a duplicate
	        docObj.duplicates       = [];                               // my duplicates

            docObj.busyID           = app.initBusyPlaceholderImage( win.statusGroup.busyStatus.imgbusy );
            docObj.busyIDs          = [];

	        docObj.roState          = masterDoc ? masterDoc.roState : false;
	        docObj.lf				= masterDoc ? masterDoc.lf : prefs.document.lineend.getValue( Preference.STRING ).toLowerCase();
	        docObj.encoding	        = masterDoc ? masterDoc.encoding : "UTF-8";
	        docObj.includePath      = masterDoc ? masterDoc.includePath : '';
	        docObj.restored         = false;
	        
	        if( masterDoc )
	            masterDoc.duplicates.push( docObj );
	        
	        docObj.setScriptID( docObj.duplicate ? docObj.master.scriptID : scriptID, docObj.duplicate );

            if( docObj.duplicate )
            {
                docObj.paneTitle = masterDoc.paneTitle;
                docObj.fileName  = masterDoc.fileName;
                
                docObj.master.busyIDs.push( docObj.busyID );
                
                if( app.isBusy( docObj.master.busyID ) )
                    app.startBusyFor( docObj.busyID );
            }
            else
            {
	            if( docObj.isFile() )
	            {
		            var f = File( docObj.scriptID );
		            docObj.paneTitle = f.fsName;
		            docObj.fileName  = f.name;
		            docObj.roState	 = f.readonly;
	            }
	            else
	            {
		            docObj.paneTitle =
		            docObj.fileName  = decodeURIComponent( title );
	            }
	            
	            docObj.busyIDs.push( docObj.busyID );
	        }
	        
            // initialize document object
            if( docObj.initialize )
                docObj.initialize( win );
				
			//
			// add default function definitions if not defined
			//
			if( !docObj.setText )				docObj.setText				= function( source, silent )							{}
			if( !docObj.getText )				docObj.getText				= function()											{ return ""; }
			if( !docObj.isDirty )				docObj.isDirty				= function()											{ return false; }
			if( !docObj.setSavePoint )			docObj.setSavePoint			= function()											{}
			if( !docObj.setLineStatus )			docObj.setLineStatus		= function( line, color )								{}
			if( !docObj.getEditor )				docObj.getEditor			= function()											{ return null; }
			if( !docObj.getLines )				docObj.getLines				= function()											{ return []; }
			if( !docObj.getTextSelection )		docObj.getTextSelection		= function()											{ return ""; }
			if( !docObj.setSelection )			docObj.setSelection			= function( line1, column1, line2, column2, scroll )	{}
			if( !docObj.scrollSelection )		docObj.scrollSelection		= function()											{}
			if( !docObj.clearUndo )				docObj.clearUndo			= function()											{}
			if( !docObj.canUndo )				docObj.canUndo				= function()											{ return false; }
			if( !docObj.canRedo )				docObj.canRedo				= function()											{ return false; }
			if( !docObj.undo )					docObj.undo					= function()											{}
			if( !docObj.redo )					docObj.redo					= function()											{}
			if( !docObj.print )					docObj.print				= function( prtObj )									{ return false; }
			if( !docObj.setLanguage )			docObj.setLanguage			= function( langID )									{}
			if( !docObj.activate )				docObj.activate				= function()											{ return false; }
			if( !docObj.deactivate )			docObj.deactivate			= function()											{ return false; }
			if( !docObj.canClose )				docObj.canClose				= function()											{ return true; }
			if( !docObj.clearProfileData )		docObj.clearProfileData		= function()											{}
			if( !docObj.eraseProfileData )		docObj.eraseProfileData		= function()											{}
			if( !docObj.updateProfData )		docObj.updateProfData		= function()											{}
			if( !docObj.addProfileData )		docObj.addProfileData		= function( data )										{}
			if( !docObj.writePrefs )			docObj.writePrefs			= function()											{}
			if( !docObj.readPrefs )				docObj.readPrefs			= function()											{}
			if( !docObj.getAllBreakpoints )		docObj.getAllBreakpoints	= function( enabled )									{ return []; }
			if( !docObj.toggleBreakpoint )		docObj.toggleBreakpoint		= function( line )										{}
			if( !docObj.setBreakpoint )			docObj.setBreakpoint		= function( line, enabled, hits, condition, currHit )	{}
			if( !docObj.removeAllBreakpoints )	docObj.removeAllBreakpoints	= function()											{}
			if( !docObj.removeBreakpoint )		docObj.removeBreakpoint		= function( line )										{}
			if( !docObj.onWinActivate )			docObj.onWinActivate		= function()											{}
			if( !docObj.onWinDeactivating )		docObj.onWinDeactivating	= function()											{}
			if( !docObj.onWinDeactivated )		docObj.onWinDeactivated		= function()											{}
			if( !docObj.onWinClosing )			docObj.onWinClosing			= function()											{}
			if( !docObj.onWinClosed )			docObj.onWinClosed			= function()											{}
			if( !docObj.toggleBookmark )		docObj.toggleBookmark		= function()											{}
			if( !docObj.removeAllBookmarks )	docObj.removeAllBookmarks	= function()											{}
			if( !docObj.nextBookmark )			docObj.nextBookmark			= function()											{}
			if( !docObj.previousBookmark )      docObj.previousBookmark		= function()											{}
            if( !docObj.find )                  docObj.find                 = function( searchStr, flags )                          { return false; }
            if( !docObj.replace )               docObj.replace              = function( searchStr, replaceStr, flags )              { return 0; }
            

	        if( !masterDoc )
			{
	            docObj.setText( text );
				docObj.clearUndo();
				docObj.setSaved();
			}
			
	        docObj.setTitle();
	        docObj.setLanguage( langID );

	        // update the global document variables
	        document = docObj;
	        documents.push( docObj );
        	
	        if( !appInShutDown )
	        {
	            globalBroadcaster.notifyClients( 'activeDocChanged' );
	            globalBroadcaster.notifyClients( 'numDocsChanged' );
	        }

			menus.debug.reflectState();

            // add new document to window menu
            addDelayedTask( menus.updateWindowMenu );
	    }
	}
	
	win.layout.layout();
		
	return win;
}

//-----------------------------------------------------------------------------
// 
// create(...)
// 
// Purpose: Create new document
// 
//-----------------------------------------------------------------------------

DocumentManager.prototype.createDuplicate = function( master, dockToDocument )
{
    return this.create( undefined, undefined, undefined, undefined, undefined, master, dockToDocument );
}

//-----------------------------------------------------------------------------
// 
// createWindow(...)
// 
// Purpose: Create empty document window
// 
//-----------------------------------------------------------------------------

DocumentManager.prototype.createWindow = function( masterDocument, dockToDocument, readPrefs, readStartupPrefs )
{
	// disable application UI until the document panel became visible
	if( !app.remoteLaunched )
		app.enabled = false;
	
	var winID =  "document" + ++this.windowID;

    //
    // setup location creation property
    //
	var location = "default";
	
	if( masterDocument )
	{
	    location = ( dockToDocument ? dockToDocument.rootPane.text : masterDocument.rootPane.text );
	    
	    if( ScriptUI.environment.keyboardState.ctrlKey )
	        location = "below:" + location;
	    else
	        location = "after:" + location;
	}
	else if( readPrefs || readStartupPrefs )
	{
		if( readPrefs )
			location = "prefed";
		if( readStartupPrefs )
			location = "startup_prefed";
	}

	//
	// create document pane
	//
	var w  = null;

	try
	{
		w = ScriptUI.workspace.add( 'document', 
	                                '', 
	                                { name      : winID, 
	                                  locator   : location } );
	}
	catch( exc )
	{
		// give it another try
		w = ScriptUI.workspace.add( 'document', 
	                                '', 
	                                { name      : winID, 
	                                  locator   : location } );
	}
	
	w.minimumSize = [400, 180];
	w.maximumSize = [10000, 10000];
	w.margins     = 0;//2;
	w.spacing     = 2;

	w.id          = winID;
	
	//
	// create status bar
	//
	w.statusGroup = w.statusbar.add ( """group {             
                                        alignment       : ['fill','fill'],
                                        margins         : 0,
                                        spacing         : 0,
                                        statusGroup          : Group
                                        {
                                            alignment       : ['fill', 'bottom' ],
                                            orientation     : 'row',
                                            margins         : 0,
                                            spacing         : 0,
                                            maximumSize     : [1000,22],
                                            infoStatus     : Panel
                                            {
                                                alignment       : ['fill','center'],
                                                margins         : 0,
                                                spacing         : 0,
                                                properties      : { borderStyle : 'sunken' },
												infoGroup		: Group
												{
 													alignment		: ['left','top'],
													orientation		: 'stack',
												    statustext      : StaticText
													{
														properties  : { truncate : 'middle' },
														alignment   : ['fill','center'],
														characters  : 40
													},
													progressGroup	: Group
													{
														visible		: false,
														bar			: Progressbar
														{
															alignment   : ['left','center'],
															preferredSize:	[200, 10],
														},
														statustext      : StaticText
														{
															properties  : { truncate : 'end' },
															alignment   : ['fill','center'],
															characters  : 80
														}
													}
												}
                                            },
                                            posStatus        : Panel
                                            {
                                                alignment       : ['right','center'],
                                                margins         : 0,
                                                spacing         : 0,
                                                properties      : { borderStyle : 'sunken' },
                                                statustext      : StaticText
                                                {
                                                    properties  : { truncate : 'middle' },
                                                    alignment   : ['center','center'],
                                                    characters  : 20
                                                }
                                            },
                                            busyStatus       : Group
                                            {
                                                alignment       : ['right','center'],
                                                orientation     : 'row',
                                                margins         : [7,0,0,0],
                                                spacing         : 0,
                                                imgbusy         : Image
                                                {
                                                    alignment    : ['left', 'center'],
                                                    preferredSize   : [18, 18]
                                                },
                                                sizeBoxSpacer    : Group {
                                                    size        : [20,1]
                                                }
                                            }
                                        }
                                    }""" );
									
	w.statusbar.layout.layout();
    w.statusGroup   = w.statusGroup.statusGroup;
	w.statusGroup.message = w.statusGroup.infoStatus.infoGroup.statustext;
	w.statusGroup.progress = w.statusGroup.infoStatus.infoGroup.progressGroup;

	///////////////////////////////////////////////////////////////////////////////
	//
	// Window UI handler
	//
	w.blockActivate = 0;

	w.setBlockActivate = function( doBlock )
	{
	    if( doBlock )
	        this.blockActivate++;
	    else
	        this.blockActivate--;
	        
	    if( this.blockActivate < 0 )
	        this.blockActivate = 0;
	}
	
	w.isBlockingActivate = function()
	{
	    return ( this.blockActivate > 0 );
	}
	
	w.onActivate	= function() 
	{
//print("{"+this.text+"} onActivate");	

		if( !this.isClosing )
		{
			if( !this.isBlockingActivate() )
			{
				this.setBlockActivate( true );
				this.__docObj__.onWinActivate();
				this.setBlockActivate( false );
			}
			
			if( this.minimized )
				this.minimized = false;
				
			document = this.__docObj__;
		}
    }

    w.onDeactivate  = function()
    {
//print("{"+this.text+"} onDeactivate");

		if( !this.isClosing )
		{
			this.__docObj__.onWinDeactivating();
			
			if( document == this.__docObj__ )
			{
				document = null;
				
				if( !appInShutDown )
					globalBroadcaster.notifyClients( 'activeDocChanged' );
			}
			
			this.__docObj__.onWinDeactivated();
		}
    }
/*    
	w.onResize = w.onResizing = function() 
	{ 
        //
        // HOTFIX: The call "this.layout.resize();" causes an runtime error "Internal Error" very often
        //         but not always. This needs to be fixed most likely in ScriptUI
        //		
		try
		{
			this.layout.resize(); 
		}
		catch( exc )
		{
			var src = """for( var __i__=0; __i__<documents.length; __i__++ )
						 {
							 documents[__i__].rootPane.visible = false;
							 documents[__i__].rootPane.visible = true;
						 }""";
	
			app.scheduleTask( src, 100 );		
		}
	}
*/	
	w.statusbar.onResize = function() 
	{ 
		this.layout.resize(); 
	}

    w.onShow = function()
    {   
        if( this.__docObj__.readPrefs )
            this.__docObj__.readPrefs();
			
		if( !this.onResize )
		{
			this.onResize = this.onResizing = function()
			{
				if( !this.isClosing )
					this.layout.resize();
			}
			
			this.layout.layout();
		}
		
		// enable application UI
		if( !app.remoteLaunched )
			app.enabled = true;
    }

    w.onClose = function() 
    {
//print("{"+this.text+"} onClose");	
		this.isClosing = true;
		
        if( this.__docObj__.canClose() )
        {
            if( docMgr.autoCompletion )
                app.cancelTask( docMgr.autoCompletion.autoID );
			
			
			if( !this.__docObj__.duplicate )
            {
                if( this.__docObj__.isDirty() && !this.__docObj__.save( false, app.enableStandardUI ) )
				{
					this.isClosing = false;
                    return false;
				}
              
                this.__docObj__.writePrefs();
            }

            this.__docObj__.onWinClosing();

            //
            // remove from documents list
            //
		    for( var i=0; i<documents.length; i++ )
		    {
                if( documents[i] == this.__docObj__ )
			    {
				    documents.splice( i, 1 );
				    break;
			    }
		    }
		}
		else
		{
			this.isClosing = false;
		    return false;
		}
		
		//
		// switch to last opened document
		//
		if( !appInShutDown )
		{
		    globalBroadcaster.notifyClients( 'activeDocChanged' );
		    globalBroadcaster.notifyClients( 'numDocsChanged' );
		}

		if( document == this.__docObj__ )
		{
		    document = null;

		    this.__docObj__.onWinClosed();
			
			if( documents.length )
				documents[documents.length-1].activate();
			else
			    menus.debug.reflectState();
		}
		else
			this.__docObj__.onWinClosed();
		
		addDelayedTask( menus.updateWindowMenu );

		this.__docObj__.setScriptID( undefined, this.__docObj__.duplicate );
		
		//
		// handle duplicates
		//
		if( this.__docObj__.duplicate )
		{
		    for( var i=0; i<this.__docObj__.master.busyIDs.length; i++ )
		    {
		        if( this.__docObj__.busyID == this.__docObj__.master.busyIDs[i] )
		        {
				    this.__docObj__.master.busyIDs.splice( i, 1 );
				    break;
		        }
		    }
		    
		    for( var i=0; i<this.__docObj__.master.duplicates.length; i++ )
		    {
                if( this.__docObj__.master.duplicates[i] == this.__docObj__ )
			    {
				    this.__docObj__.master.duplicates.splice( i, 1 );
				    break;
			    }
		    }

			// pwollek 11/24/2009: don't set the title if we about to close
		    // this.__docObj__.setTitle();
		    this.__docObj__.master = null;
		}
		else
		{
		    while( this.__docObj__.duplicates.length > 0 )
		    {
		        var d = this.__docObj__.duplicates.pop();
		        d.close();
		    }
		}
		
		//
		// we are closing (don't reset the flag
		// here this.isClosing = false; )
		//
		
		return true;
    }

	return w;
}

//-----------------------------------------------------------------------------
// 
// suspendHandler_activate(...)
// 
// Purpose: suspend/resume onActivate/onDeactivate handler of all documents
// 
//-----------------------------------------------------------------------------

DocumentManager.prototype.suspendHandler_activate = function()
{
	for( var i=0; i<documents.length; i++ )
	{
		documents[i].__onActivate__			= documents[i].rootPane.onActivate;
		documents[i].rootPane.onActivate	= undefined;
		documents[i].__onDeactivate__		= documents[i].rootPane.onDeactivate;
		documents[i].rootPane.onDeactivate	= undefined;
	}
}

DocumentManager.prototype.resumeHandler_activate = function()
{
	for( var i=0; i<documents.length; i++ )
	{
		if( documents[i].__onActivate__ )
			documents[i].rootPane.onActivate	= documents[i].__onActivate__;
			
		if( documents[i].__onDeactivate__ )
			documents[i].rootPane.onDeactivate	= documents[i].__onDeactivate__;
	}
}

//-----------------------------------------------------------------------------
// 
// load(...)
// 
// Purpose: load file
// 
//-----------------------------------------------------------------------------

DocumentManager.prototype.load = function( file, langID, ignoreError, startup )
{
	if( !ignoreError )
		ignoreError = false;

	if( !startup )
		startup = false;
		
	if( !(file instanceof File) )
		return null;

	var doc = this.find( file.absoluteURI );

	if( doc )
	{
		// already loaded
		doc.activate();
		return doc;
	}

	if (!langID)
		langID = lang.getLanguageForFile (file);

	if (!langID)
		return null;

	var text = this.safeRead( file, ignoreError );

	if (null == text)
		return null;

    var w = this.create( file.name, "", langID, file.absoluteURI, true, undefined, undefined, startup );
	
	w.__docObj__.roState  = file.readonly;
	w.__docObj__.encoding = file.encoding;
	w.__docObj__.setText( text, true );
	w.__docObj__.clearUndo();
	w.__docObj__.setSaved();

	// remeber folder whenever loading a document
	app.currentFolder = ( file.parent ? file.parent.absoluteURI : "/" );

	return w.__docObj__;
}

//-----------------------------------------------------------------------------
// 
// saveAll(...)
// 
// Purpose: Save all open documents
// 
//-----------------------------------------------------------------------------

DocumentManager.prototype.saveAll = function( ask )
{
	var ok     = true;
	var oldDoc = document;

	try
	{
		var countModified = 0;
		
		for( var i=0; i<documents.length; i++ )
			countModified += ( documents[i].isDirty() ? 1 : 0 );
		
		if( countModified > 0 )
		{
			if( countModified == 1 )
			{
				for( var i=0; i<documents.length && ok; i++ )
				{
					if( documents[i].isDirty() )
						ok = documents[i].save( !File( documents[i].scriptID ).exists, ask );
				}
			}
			else
			{
				var doSave = true;
				
				if( ask )
				{
                        if( app.enableStandardUI )
                        {
                            switch( app.saveAlert( "", true ) )
                            {
                                case 1:
                                    doSave	= true;
                                    ok		= true;
                                    break;
                                case 0:
                                    doSave	= false;
                                    ok		= true;
                                    break;
                                case -1:
                                    doSave	= false;
                                    ok		= false;
                                    break;
                            }
                        }
                        else
                        {
                            doSave	= false;
                            ok		= true;
                        }
				}
				
				if( doSave )
				{
					for( var i=0; i<documents.length && ok; i++ )
					{
						if( documents[i].isDirty() )
							ok = documents[i].save( !File( documents[i].scriptID ).exists, false );
					}
				}
			}	
		}
	}
	catch (e)
	{
		ok = false;
	}
	
	if( oldDoc )
	{
		oldDoc.activate();
	}
		
	return ok;
}

//-----------------------------------------------------------------------------
// 
// forceBackup(...)
// 
// Purpose: Trigger backup for all source documents
// 
//-----------------------------------------------------------------------------

DocumentManager.prototype.forceBackup = function()
{
	for( var i=0; i<documents.length; i++ )
	{
		if( documents[i] && documents[i].isSourceDocument )
			documents[i].forceBackupDocument();
	}
}

//-----------------------------------------------------------------------------
// 
// activateDocument(...)
// 
// Purpose: Activate passed document as soon as it was restored.
//          {Also set active target/session afterwards)
// 
//-----------------------------------------------------------------------------

DocumentManager.prototype.activateDocument = function( doc, dbgSession )
{
	try
	{
		if( doc && doc.rootPane && !doc.rootPane.isClosing )
		{
			if( doc.restored )
			{
				workspace.resetFocus();		// remove stored focus
				
				doc.activate();
	            
				if( dbgSession )
					targetMgr.setActive( dbgSession.target, dbgSession );

				doc.scrollSelection();
			}
			else
				addDelayedTask( docMgr.activateDocument, doc, dbgSession );
		}
	}
	catch( exc )
	{}
}

//-----------------------------------------------------------------------------
// 
// safeRead(...)
// 
// Purpose: Do a safe read. Open the file, read it, and close it.
//			On errors, display an error message (if quiet is undefined/false) 
//			and return null; otherwise,	return the text read.
// 
//-----------------------------------------------------------------------------

DocumentManager.prototype.safeRead = function( file, quiet )
{
	var text  = null;
	var error = "";

	if( file.open() )
	{
		switch( file.encoding )
		{
			case "UCS-2BE":
			case "UCS-2LE":
			case "UCS-4BE":
			case "UCS-4LE":
			case "UTF-8":
			case "UTF8":
				// Auto-detected encoding
				text  = file.read();
				error = file.error;
				break;
				
			default:
				// no autodetection: force UTF-8 as 1st attempt
				file.encoding = "UTF-8";
				text = file.read();
				error = file.error;
				if (error != "" && file.encoding != app.systemEncoding)
				{
					// retry with the system encoding
					file.encoding = app.systemEncoding;
					file.seek (0);
					text = file.read();
					error = file.error;
				}
		}
		
		file.close();
	}
	else
		error = file.error;

	if( error.length )
	{
		if( !quiet )
		{
			text = localize ("$$$/ESToolkit/FileDlg/CannotOpen=Cannot open file %1!", decodeURIComponent( file.name ) );
			text += "\n" + file.error;
			errorBox (text);
		}
		text = null;
	}

	return text;
}

//-----------------------------------------------------------------------------
// 
// startAutoCompletion(...)
// 
// Purpose: Start auto completion. Therefore get code hints from omv 
//          for the search string (from cursor position back to previous 
//          whitespace) and fill listbox with code hints
// 
//-----------------------------------------------------------------------------

DocumentManager.prototype.startAutoCompletion = function()
{
    if( document && document.startAutoCompletion && this.autoCompletion && this.autoCompletion.autoDoc == document )
        document.startAutoCompletion();
}

//-----------------------------------------------------------------------------
// 
// setStatusLine(...)
// 
// Purpose: Set main text in the status line
// 
//-----------------------------------------------------------------------------

DocumentManager.prototype.setStatusLine = function( text, doc )
{
    if( !text )
        text = " ";
        
    if( text[0] == "$" )
		text = localize( text );
		
    var currentDoc = document;
    
    if( doc )
        currentDoc = doc;
    
    if( currentDoc && currentDoc.duplicate )
        currentDoc = currentDoc.master;
        
    if( currentDoc && !currentDoc.rootPane.isClosing )
    {
		var statusGrp = currentDoc.rootPane.statusGroup;
		statusGrp.progress.hide();
		statusGrp.message.show();
        statusGrp.message.text = text;
        
        for( var i=0; i<currentDoc.duplicates.length; i++ )
		{
			statusGrp = currentDoc.duplicates[i].rootPane.statusGroup;
			statusGrp.progress.hide();
			statusGrp.message.show();
            statusGrp.message.text = text;
		}
    }
}

//-----------------------------------------------------------------------------
// 
// showProgress(show)
// 
// Purpose: Display or hide the progress bar in the status line(s)
// 
//-----------------------------------------------------------------------------

DocumentManager.prototype.showProgress = function( show )
{
    var currentDoc = document;
    
    if( currentDoc && currentDoc.duplicate )
        currentDoc = currentDoc.master;
        

	if( OMV.ui && OMV.ui.w )
	{
		OMV.ui.progress.statustext.text = '';

		var bar = OMV.ui.progress.bar;
		bar.value = 0;

		if( show )
			OMV.ui.progress.show();
		else
			OMV.ui.progress.hide();
	}

	if( currentDoc && !currentDoc.rootPane.isClosing )
    {
		var statusGrp = currentDoc.rootPane.statusGroup;
		if ( show )
		{
			statusGrp.message.hide();
			statusGrp.progress.show();
			statusGrp.progress.statustext.text = "";
			statusGrp.progress.bar.value = 0;

			if( OMV.ui && OMV.ui.w )
			{
				OMV.ui.progress.statustext.text = "";
				OMV.ui.progress.bar.value = 0;
			}
		}
		else
		{
			statusGrp.progress.hide();
			statusGrp.message.show();
		}

        for( var i=0; i<currentDoc.duplicates.length; i++ )
		{
			statusGrp = currentDoc.duplicates[i].rootPane.statusGroup;
			if ( show )
			{
				statusGrp.message.hide();
				statusGrp.progress.show();
				statusGrp.progress.statustext.text = "";
				statusGrp.progress.bar.value = 0;
			}
			else
			{
				statusGrp.progress.hide();
				statusGrp.message.show();
			}
		}

		// this is modal
		app.enabled = !show;
		app.pumpEventLoop( true );
    }
}

//-----------------------------------------------------------------------------
// 
// setProgressText(...)
// 
// Purpose: Set the progress message
// 
//-----------------------------------------------------------------------------

DocumentManager.prototype.setProgressText = function( msg )
{
	if (msg [0] == "$")
		msg = localize.apply (this, arguments);

   var currentDoc = document;
    
    if( currentDoc && currentDoc.duplicate )
        currentDoc = currentDoc.master;
        
	if( OMV.ui && OMV.ui.w )
		OMV.ui.progress.statustext.text = msg;

    if( currentDoc && !currentDoc.rootPane.isClosing )
    {
		currentDoc.rootPane.statusGroup.progress.statustext.text = msg;

		for( var i=0; i<currentDoc.duplicates.length; i++ )
		{
			currentDoc.duplicates[i].rootPane.statusGroup.progress.statustext.text = msg;
		}

		app.pumpEventLoop( true );
    }
}

//-----------------------------------------------------------------------------
// 
// getProgress()
// 
// Purpose: Get the progress bar value
// 
//-----------------------------------------------------------------------------

DocumentManager.prototype.getProgress = function()
{
	var n = 0;
	var currentDoc = document;
    
    if( currentDoc && currentDoc.duplicate )
        currentDoc = currentDoc.master;
        
    if( currentDoc && !currentDoc.rootPane.isClosing )
    {
		var bar = currentDoc.rootPane.statusGroup.progress.bar;
		n = bar.value;
    }
	else if( OMV.ui && OMV.ui.w )
	{
		var bar = OMV.ui.progress.bar;
		n = bar.value;
	}

	return n;
}

//-----------------------------------------------------------------------------
// 
// getRemainingProgress (value)
// 
// Purpose: Get the remaining progress value
// 
//-----------------------------------------------------------------------------

DocumentManager.prototype.getRemainingProgress = function ()
{
	var n = 0;
	var currentDoc = document;
    
    if( currentDoc && currentDoc.duplicate )
        currentDoc = currentDoc.master;
        
    if( currentDoc && !currentDoc.rootPane.isClosing )
    {
		var bar = currentDoc.rootPane.statusGroup.progress.bar;
		n = bar.maxvalue - bar.value;
    }
	else if( OMV.ui && OMV.ui.w )
	{
		var bar = OMV.ui.progress.bar;
		n = bar.maxvalue - bar.value;
	}

	return n;
}

//-----------------------------------------------------------------------------
// 
// incProgress (value)
// 
// Purpose: Increment the progress bar
// 
//-----------------------------------------------------------------------------

DocumentManager.prototype.incProgress = function (n)
{
	if (!n)
		n = 1;

	var currentDoc = document;
    
    if( currentDoc && currentDoc.duplicate )
        currentDoc = currentDoc.master;
        
	if( OMV.ui && OMV.ui.w )
	{
		var bar = OMV.ui.progress.bar;
		bar.value += n;
	}

    if( currentDoc && !currentDoc.rootPane.isClosing )
    {
		var bar = currentDoc.rootPane.statusGroup.progress.bar;
		bar.value += n;

		for( var i=0; i<currentDoc.duplicates.length; i++ )
		{
			bar = currentDoc.duplicates[i].rootPane.statusGroup.progress.bar;
			bar.value += n;
		}

		app.pumpEventLoop( true );
    }
}

//-----------------------------------------------------------------------------
// 
// setProgress (value [, max])
// 
// Purpose: Set the progress bar
// 
//-----------------------------------------------------------------------------

DocumentManager.prototype.setProgress = function (n, end)
{
	var currentDoc = document;
    
    if( currentDoc && currentDoc.duplicate )
        currentDoc = currentDoc.master;
        
	if( OMV.ui && OMV.ui.w )
	{
		var bar = OMV.ui.progress.bar;
		if (end != undefined)
			bar.maxvalue = end;
		bar.value = n;
	}

    if( currentDoc && !currentDoc.rootPane.isClosing )
    {
		var bar = currentDoc.rootPane.statusGroup.progress.bar;
		if (end != undefined)
			bar.maxvalue = end;
		bar.value = n;

		for( var i=0; i<currentDoc.duplicates.length; i++ )
		{
			bar = currentDoc.duplicates[i].rootPane.statusGroup.progress.bar;
			if (end != undefined)
				bar.maxvalue = end;
			bar.value = n;
		}

		app.pumpEventLoop( true );
    }
}

//-----------------------------------------------------------------------------
// 
// isFile(...)
// 
// Purpose: Is the passed scriptID a file?
// 
//-----------------------------------------------------------------------------

DocumentManager.prototype.isFile = function( scriptID )
{
    // true if the scriptID contains an absolute URI.
    // Do not use File.exists - it may be slow on networked files
    return( scriptID && scriptID.length && ( scriptID[0] == '/' || scriptID[0] == '~' ) );
}

//-----------------------------------------------------------------------------
// 
// isActiveDocumentWin(...)
// 
// Purpose: Is the passed document the current active document window?
// 
//-----------------------------------------------------------------------------

DocumentManager.prototype.isActiveDocumentWin = function( doc )
{
    if( !doc )
        doc = document;
        
    return ( doc && ( doc.rootPane == workspace.activeDocument ) );
}

//-----------------------------------------------------------------------------
// 
// related(...)
// 
// Purpose: Is the passed doc1 equal to or a duplicate of doc2?
// 
//-----------------------------------------------------------------------------

DocumentManager.prototype.related = function( doc1, doc2 )
{
    if( !doc1 || !doc2 )
        return false;
        
    var _doc1_ = ( doc1.duplicate ? doc1.master : doc1 );
    var _doc2_ = ( doc2.duplicate ? doc2.master : doc2 );
    
    return ( _doc1_ == _doc2_ );
}

//-----------------------------------------------------------------------------
// 
// readPrefs(...)
// 
// Purpose: Read/write prefs
// 
//-----------------------------------------------------------------------------

DocumentManager.prototype.readPrefs = function()
{
	var openedDocs  = 0;
	var docs        = prefs.opendocs.getLength();

    for( var i=docs-1; i>=0; i-- )
    {
        var scriptID = prefs.opendocs['opendocs'+i].scriptID.getValue( Preference.STRING );
        
        if( scriptID && scriptID.length && ( scriptID[0] == '/' || scriptID[0] == '~' ) )
        {
            if( this.load( new File( scriptID ), prefs.opendocs['opendocs'+i].langID.getValue( Preference.STRING ), true, true ) )
                openedDocs++
        }
    }

	//
	// activate previous front most doc
	//
	if( openedDocs > 0 )
	{
		var activeDocID = prefs.activeDoc.getValue( Preference.STRING );
		
		if( activeDocID )
			app.scheduleTask( "var __doc__ = docMgr.find('" + activeDocID + "'); if( __doc__ != undefined ) __doc__.rootPane.active = true;", 1 );
	}

    return ( openedDocs > 0 );
}

DocumentManager.prototype.writePrefs = function()
{
	var docs = prefs.opendocs.getLength();

    //
    // clear existing entries
    //
    for( var i=0; i<docs; i++ )
        prefs.opendocs['opendocs'+i].scriptID = '';
    
    //
    // write each open document
    //
    for( var i=0; i<documents.length; i++ )
    {
        prefs.opendocs['opendocs'+i].scriptID = documents[i].scriptID;
        prefs.opendocs['opendocs'+i].langID   = documents[i].langID;
        
        if( documents[i].isFile() )
            documents[i].writePrefs();
    }
	
	//
	// store front most doc
	//
	if( workspace.activeDocument && workspace.activeDocument.__docObj__ )
		prefs.activeDoc = workspace.activeDocument.__docObj__.scriptID;
}

///////////////////////////////////////////////////////////////////////////////
//
// profiling
//

DocumentManager.prototype.clearProfData = function()
{
    for( var i=0; i<documents.length; i++ )
        if( documents[i].clearProfileData )
            documents[i].clearProfileData();
}

DocumentManager.prototype.setProfiling = function( profType )
{
    for( var i=0; i<documents.length; i++ )
    {
        if( documents[i].profiling != profType )
            documents[i].profiling = 0;
    }
}

DocumentManager.prototype.clearProfileData = function()
{
    for( var i=0; i<documents.length; i++ )
        if( documents[i].eraseProfileData )
            documents[i].eraseProfileData();
}

DocumentManager.prototype.updateProfData = function()
{
    for( var i=0; i<documents.length; i++ )
    {
        if( documents[i].clearProfileData && documents[i].updateProfData )
        {
            if( prefs.profiling.profDisplayMode.getValue( Preference.NUMBER ) == 0 )
            {
                documents[i].profiling = 0;
                documents[i].clearProfileData();
            }

			documents[i].updateProfData();
        }
    }
}

// Dump the profiler data into a file.

DocumentManager.prototype.saveProfData = function()
{
	var f = new File (app.currentFolder + "/profileData.csv");
	f = f.saveDlg (
			localize ("$$$/ESToolkit/ProfSaveDlg/Title=Save Profiler Data"),
			localize ("$$$/ESToolkit/ProfSaveDlg/Types=CSV (Comma Delimited):*.csv"));

	if (f)
	{
		if (f.open ("w"))
		{
			f.encoding = "UTF-8";
			for (var i = 0; i < documents.length; i++)
			{
				var doc = documents[i];
				var scriptID = doc.scriptID;
				if (scriptID[0] == '/' || scriptID [0] == '~')
				{
					var tmp = new File (scriptID);
					scriptID = tmp.fsName;
				}
				if (0 == doc.profileDataCount)
					continue;

				// create a sorted array of data
				var sortArray = [];
				for (var line in doc.profileData)
					sortArray.push (doc.profileData [line]);
				sortArray.sort ((function(a,b) { return a.line - b.line; }));
				for (var j = 0; j < sortArray.length; j++)
				{
					var line = sortArray [j];
					f.write (scriptID + ',' + (line.line+1) + ',' + line.time + ',' + line.hits + '\n');
				}
			}
		}
		if (f.error.length || !f.close())
		{
			errorBox( localize( "$$$/ESToolkit/FileDlg/CannotWrite=Cannot write to file %1!", decodeURIComponent( f.fsName ) ) );
			f.remove();
		}
			
		try
		{
			// we are good citizen
			app.notifyFileChanged (f);
		}
		catch( exc )
		{}
	}
}

//-----------------------------------------------------------------------------
// 
// registerFactory(...)
// 
// Purpose: Register a factory for passed filename extensions and language IDs
//          Parameter extensions and languages should be a comma separated
//          list. One of them could be undefined.
// 
//-----------------------------------------------------------------------------

DocumentManager.prototype.registerFactory = function( factory, extensions, languages )
{
    var ret = false;
    
    if( factory && typeof( factory ) == "function" && ( extensions || languages ) )
    {
        var extList  = [];
        var langList = [];
        
        if( extensions )
            extList = extensions.split(",");
        if( languages )
            langList = languages.split(",");
            
        for( var e=0; e<extList.length; e++ )
            this.factoriesExt[extList[e]] = factory;
            
        for( var l=0; l<langList.length; l++ )
            this.factoriesLang[langList[l]] = factory;
    }
    
    return ret;
}

//-----------------------------------------------------------------------------
// 
// function(...)
// 
// Purpose: Find factory for passed scriptID or langID.
//          The algorithm is:
//          1. Take filename extension from scriptID (if available) and find match
//          2. If 1. failed, then find match for passed langID
// 
//-----------------------------------------------------------------------------

DocumentManager.prototype.findFactory = function( scriptID, langID )
{
    var factory = null;
    
    if( this.isFile( scriptID ) )
    {
        // try to find filename extension
        var pos = scriptID.lastIndexOf( "." );
        
        if( pos > 0 )
            factory = this.factoriesExt[ scriptID.substr( pos+1 ) ];
    }
    
    if( !factory )
        factory = this.factoriesLang[langID];
    
    return factory;
}
