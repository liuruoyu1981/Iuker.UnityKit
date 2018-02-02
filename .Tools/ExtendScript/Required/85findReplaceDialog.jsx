/**************************************************************************
*
*  @@@BUILDINFO@@@ 85findReplaceDialog-2.jsx 3.0.0.24   05-May-2008
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

app.createFR = function( where )
{
	findReplaceObj = new DocFindReplace( where );
	findReplaceObj.initialShow = true;
    findReplaceObj.show();
	findReplaceObj.initialShow = false;
	globalBroadcaster.registerClient( findReplaceObj, 'shutdown,numDocsChanged,favoritesChanged' );	                                
}

///////////////////////////////////////////////////////////////////////////////
//
// F&R object
//
DocFindReplace.WHERE_CURRDOC    = 0;
DocFindReplace.WHERE_CURRSEL    = 1;
DocFindReplace.WHERE_ALLDOCS    = 2;	// multiple find
DocFindReplace.WHERE_FAVORITES  = 3;	// multiple find
DocFindReplace.WHERE_CURRENGINE = 4;	// multiple find

function DocFindReplace( where ) 
{ 
	this.strFind                = "$$$/ESToolkit/FindReplaceDlg/Find=&Find";
	this.strFindNext            = "$$$/ESToolkit/FindReplaceDlg/FindNext=&Find Next";
	this.strFindAll				= "$$$/ESToolkit/FindReplaceDlg/FindAll=&Find All";
	this.strSearchFailed        = "$$$/ESToolkit/FindReplaceDlg/Alerts/SearchFailed=No matches found";
	this.strSearchFailedRetry   = "$$$/ESToolkit/FindReplaceDlg/Alerts/SearchFailedRetry=The end of the document has been reached.^nContinue checking from the beginning?";
	this.strReplacementDone     = "$$$/ESToolkit/FindReplaceDlg/Alerts/ReplacementDone=Search complete. %1 changes were made.";
	this.strOneReplacementDone  = "$$$/ESToolkit/FindReplaceDlg/Alerts/OneReplacementDone=Search complete. One change was made.";

    //
	// The Find/Replace floating palette resource
	//
	var dlgResSpec = """group {
		alignment   : ['fill','top'],
		orientation : 'column',
		alignChildren: 'top',
		margins     : 5,
		spacing		: 5,
		topGroup : Group
		{
			alignment   : ['fill','top'],
			orientation : 'row',
			findGrp    :	Group
			{
				alignment       : ['fill','top'],
				orientation     : 'column',
				alignChildren   : 'left',
				margins			: 0,
				search          : Group
				{
					alignment       : ['fill','top'],
					orientation     : 'row',
					lbl             : StaticText
					{
						text            : '$$$/ESToolkit/FindReplaceDlg/FindLbl=Find:'
					},
					string          :	EditText
					{
        				alignment       : ['fill','top'],
						properties      : { enterKeySignalsOnChange : false }
					}
				},
				replace: Group
				{
					alignment       : ['fill','top'],
					orientation     : 'row',
					lbl             : StaticText { text : '$$$/ESToolkit/FindReplaceDlg/ReplaceWith=Replace With:' },
					string          : EditText
					{
        				alignment       : ['fill','top'],
						properties      : { enterKeySignalsOnChange : false }
					}
				},
				where:	Group
				{
					alignment		: ['fill','center'],
					orientation     : 'row',
					lbl             : StaticText
					{
        				alignment       : ['left','center'],
						text            : '$$$/ESToolkit/FindReplaceDlg/Where=&Search Where:'
					},
					list	       : DropDownList
					{
					    preferredSize   : [180,21],
        				alignment       : ['fill','center']
					},
				}
			},
			btnsGrp:	Group
			{
				alignment       : ['right','fill'],
				orientation     : 'column',
				alignChildren   : 'fill',
				margins			: 0,
				spacing			: 5,
				find            : Button
				{
					enabled         : true,
					text            : '$$$/ESToolkit/FindReplaceDlg/Find=&Find'
				},
				replace         : Button
				{
					enabled         : true,
					text            : '$$$/ESToolkit/FindReplaceDlg/Btn/Replace=&Replace'
				},
				replaceFind     : Button
				{
					enabled         : true,
					text            : '$$$/ESToolkit/FindReplaceDlg/Btn/ReplaceFind=Replace && Fi&nd'
				},
				replaceAll      : Button
				{
					enabled         : true,
					text    : '$$$/ESToolkit/FindReplaceDlg/Btn/ReplaceAll=Replace &All'
				}
			}
		},
		bottomGroup:	Group
		{
			orientation     : 'row',
			alignment		: ['left','top'],
			alignChildren   : ['left','top'],
			margins			: 0,
			left:	Group
			{
				orientation     : 'column',
				alignChildren   : ['left','top'],
				matchCase       : Checkbox
				{
					text            : '$$$/ESToolkit/FindReplaceDlg/MatchCase=Match &Case'
				},
				wholeWord       : Checkbox
				{
					text            : '$$$/ESToolkit/FindReplaceDlg/FindWholeWord=Match &Whole Word'
				},
			},
			right: Group
			{
				orientation     : 'column',
				alignChildren   : ['left','top'],
				matchRegExp     : Checkbox
				{
					text            : '$$$/ESToolkit/FindReplaceDlg/MatchRegExp=Match Regular E&xpression'
				},
				clearResults: Checkbox
				{
					text            : '$$$/ESToolkit/FindReplaceDlg/ClearResults=&Clear Old Results'
				}
			}
		}
	}""";

	// Create the floating palette, link it to the FindReplace object
	this.palette = ScriptUI.workspace.add( 'tab', 
	                                       localize('$$$/ESToolkit/FindReplaceDlg/Title=Find and Replace'), 
	                                       { name        : 'findAndReplace',
	                                         closeOnKey  : 'Escape', } );
		                                     
    this.palette.content = this.palette.add( dlgResSpec );
	this.palette.image   = ScriptUI.newImage( '#PFindReplace_N', undefined, undefined, '#PFindReplace_R' );
	
    this.palette.minimumSize = [335,168];
	this.palette.maximumSize = [5000,500];
	                                       
	this.palette.findReplaceObj = this; 
	
	this.baseFolder		= "";	// the base folder for Favorites search
	this.where			= DocFindReplace.WHERE_CURRDOC;

	this.searchEdit		= this.palette.content.topGroup.findGrp.search.string;
	this.replaceEdit	= this.palette.content.topGroup.findGrp.replace.string;
	this.whereList		= this.palette.content.topGroup.findGrp.where.list;
	this.findBtn		= this.palette.content.topGroup.btnsGrp.find;
	this.replaceBtn		= this.palette.content.topGroup.btnsGrp.replace;
	this.replaceFindBtn	= this.palette.content.topGroup.btnsGrp.replaceFind;
	this.replaceAllBtn	= this.palette.content.topGroup.btnsGrp.replaceAll;
	this.matchCase		= this.palette.content.bottomGroup.left.matchCase;
	this.wholeWord		= this.palette.content.bottomGroup.left.wholeWord;
	this.matchRegExp	= this.palette.content.bottomGroup.right.matchRegExp;
	this.clearResults	= this.palette.content.bottomGroup.right.clearResults;

	// Set update event handlers for Find and Replace strings
	this.searchEdit.onChanging  = 
	this.searchEdit.onChange    =
	this.replaceEdit.onChanging = 
	this.replaceEdit.onChange   = function ()
	{
		findReplaceObj.resetFindReplace (true);
	}

    this.searchEdit.onEnterKey = function()
	{
	    findReplaceObj.findBtn.notify();
	}
		
    this.replaceEdit.onEnterKey = function ()
	{
	    findReplaceObj.findBtn.notify();
	}

	// Set update event handlers for 'match options' checkboxes
	this.matchCase.onClick =
	this.wholeWord.onClick =
	this.matchRegExp.onClick = function ()
	{
		//	A match option is changing: reset FindReplace controls state
		findReplaceObj.resetFindReplace (true);
	}

	// Set onClick event handlers for the buttons pane
	this.findBtn.onClick = function ()
	{		
		findReplaceObj.find (false);
		/*	We want the search string to retain the focus after a
			Find/Find Next, so it is easy to change the string.
			Because Find/Find Next is the default button, it will
			get Enter keystrokes even if the search string has the
			focus, unlike in the Replace and Replace & Find cases below. */
		if (findReplaceObj.where < DocFindReplace.WHERE_ALLDOCS)
			findReplaceObj.resetFocus (findReplaceObj.searchEdit);
	}

	this.replaceBtn.onClick = function ()
	{
		var replaced = findReplaceObj.replace (false, false);
		/*	After a successful Replace, we want the focus to
			stay on the Replace edit field, so the user can easily do
			another Replace by typing Enter. If the Replace was
			unsuccessful, return the focus to the search string, so
			it can be easily changed. */
		if (replaced)
			findReplaceObj.resetFocus (findReplaceObj.replaceEdit);
		else
			findReplaceObj.resetFocus (findReplaceObj.searchEdit);
	}

	this.replaceFindBtn.onClick = function ()
	{
		var replacedAndFound = false;
		if (findReplaceObj.replace (false, false))
		{	
			if( findReplaceObj.where != DocFindReplace.WHERE_CURRSEL )
			{
				// Only do the 'follow on' find if the replace() matched
				replacedAndFound = findReplaceObj.find (false);
			}
		}

		/*	After a successful Replace & Find, we want the focus to
			stay on the Replace edit field, so the user can easily do
			another 'Replace & Find' by typing Enter. If the Replace was
			unsuccessful, return the focus to the search string, so
			it can be easily changed. */
		if (replacedAndFound)
			findReplaceObj.resetFocus (findReplaceObj.replaceEdit);
		else
			findReplaceObj.resetFocus (findReplaceObj.searchEdit);
	};
	this.replaceAllBtn.onClick = function ()
	{
		findReplaceObj.replace (true, false);
		/*	After a 'Replace All', always return the focus to the
			search string, so it can be easily changed. */
		findReplaceObj.resetFocus (findReplaceObj.searchEdit);
	};
	
	// initialize "where" popup
	this.updateTargetList();

    this.clearResults.enabled = false;

    this.whereList.onChange = function()
    {
        if( findReplaceObj )
        {
			if (this.selection)
			{
				findReplaceObj.where     = this.selection.where;
				findReplaceObj.wherePath = this.selection.path;
				findReplaceObj.filter	= this.selection.filter;
				findReplaceObj.recursive = this.selection.recursive;
			}
			else
			{
				findReplaceObj.where     = DocFindReplace.WHERE_CURRDOC;
				findReplaceObj.wherePath = '';
				findReplaceObj.filter	= "*.jsx";
				findReplaceObj.recursive = false;
			}
            findReplaceObj.resetFindReplace( true );
        }
    }
    
	this.palette.onClose = function()
	{
		// set focus to current document
		workspace.setActiveDocumentFocus( true );
	}
	
	// Define an onActivate event handler that will execute each time the dialog is activated
	this.palette.onShow = function ()
	{
	    //	On each 'show', reset the find & replace strings and the states of the buttons
	    /* Replace current 'Find' string if a selection is active in current document.
	       For multi-line selections, use only the first line */
	    var selection = ( document ? document.getTextSelection() : '' );
	    var nlIndex = selection.indexOf ('\n');
	    if (nlIndex >= 0)
		    selection = selection.substring (0, nlIndex);
	    if (selection.length == 0)
		    //	Use last known search string
		    findReplaceObj.searchEdit.text = findReplaceObj.searchEdit.activeSearchString;
	    else
		    //	Use the selection (up to first newline)
		    findReplaceObj.searchEdit.text = selection;
	    findReplaceObj.replaceEdit.text = findReplaceObj.replaceEdit.activeReplaceString;

		// Adjust widths of some text labels for alignment
		with (this.content.topGroup.findGrp)
		{
			var maxWidth = replace.lbl.size.width;
			var findWidth = search.lbl.size.width;
			if (findWidth > maxWidth)
				maxWidth = findWidth;
			var whereWidth = where.lbl.size.width;
			if (whereWidth > maxWidth)
				maxWidth = whereWidth;
			search.lbl.size.width = maxWidth;
			replace.lbl.size.width = maxWidth;
			where.lbl.size.width = maxWidth;
			this.layout.layout(true);
		}
	    findReplaceObj.resetFindReplace (true);

	    //	After the window is visible, give the 'search' edit field the focus
	    if( !findReplaceObj.initialShow )
	        addDelayedTask( findReplaceObj.resetFocus, findReplaceObj.searchEdit, findReplaceObj.palette, true );
	};

    this.palette.onResize = 
    this.palette.onResizing = function ()
    {
		this.layout.resize();
    }

	this.initFromPrefs();
}

DocFindReplace.prototype.updateTargetList = function()
{
    var sel = ( this.whereList.selection ? this.whereList.selection.text : '' );

    // force Current Doc if documents.length > 0 for the first time
	if (documents.length > 0 && this.whereList.items[0].where != DocFindReplace.WHERE_CURRDOC)
		sel = "";

    this.whereList.removeAll();
    
    var item = null;	
    
	var mustHaveDocs = (documents.length != 0);
	if ((this.docInfoInTargetList == undefined || mustHaveDocs != this.docInfoInTargetList) && mustHaveDocs)
	{
        item = this.whereList.add( 'item', localize( '$$$/ESToolkit/FindReplaceDlg/Where/CurrDoc=Current document' ) );
        item.where = DocFindReplace.WHERE_CURRDOC;
        item = this.whereList.add( 'item', localize( '$$$/ESToolkit/FindReplaceDlg/Where/CurrSel=Current selection' ) );
        item.where = DocFindReplace.WHERE_CURRSEL;
        item = this.whereList.add( 'item', localize( '$$$/ESToolkit/FindReplaceDlg/Where/AllDocs=All open documents' ) );
        item.where = DocFindReplace.WHERE_ALLDOCS;
        item = this.whereList.add( 'item', localize( '$$$/ESToolkit/FindReplaceDlg/Where/CurrEng=Current target/engine' ) );
        item.where = DocFindReplace.WHERE_CURRENGINE;
    }
    this.docInfoInTargetList = mustHaveDocs;

    if( favorites && favorites.length > 0 )
    {
		if (this.whereList.items.length == 0 && this.where != DocFindReplace.WHERE_FAVORITES)
			this.where = DocFindReplace.WHERE_FAVORITES;

		if (this.whereList.items.length)
			this.whereList.add ('separator', '');
			
        for( var i=0; i<favorites.length; i++ )
        {
            var item = this.whereList.add( 'item', favorites.items[i].name );
    	    item.where = DocFindReplace.WHERE_FAVORITES;
            item.path = favorites.items[i].path;
            item.filter = favorites.items[i].filter;
			item.recursive = favorites.items[i].recursive;
        }
    }

    this.whereList.selection = 0;

    if( sel.length > 0 )
    {
        for( var i=0; i<this.whereList.items.length; i++ )
        {
            if( this.whereList.items[i].text == sel )
            {
                this.whereList.selection = this.whereList.items[i];
				this.where = this.whereList.items[i].where;
                break;
            }
        }
    }
    
    this.resetFindReplace();
}

/////////////////////////////////////////////////////////////////////////
//	Define the DocFindReplace 'public method' functions

/*	find(searchWholeDoc)
	Search for the search text.
	* If 'searchWholeDoc' is true, search the entire document,
	  otherwise search from the insertion point to the end of the document.
	
	Returns true if the search was successful, false if not.
	
	Called from the Find button event handler, or
	from a keyboard shortcut handler.
*/
DocFindReplace.prototype.find = function(searchWholeDoc)
{
    if( this.where < DocFindReplace.WHERE_ALLDOCS )
        return this.findInDocument( document, searchWholeDoc )
	else
	    return this.findInFiles();
} // DFR_find

DocFindReplace.prototype.findInDocument = function( doc, searchWholeDoc )
{
    if (doc && doc.composing)
	    doc.endComposing();
	    
    var options = this.getMatchOptions (searchWholeDoc);
    
    var searchString = this.searchEdit.text;
    //	Remember that we searched for this text, for preferences
    this.searchEdit.activeSearchString = searchString;
    if (doc.find (searchString, options)) 
    {
	    //	Match found: reset search state
	    this.resetFindReplace (false);
	    return true;
    }
    else 
    {
	    /*	Match not found: if already in 'search the whole doc'
		    mode, note the search failure, else, ask if user
		    wants to search from the beginning */
	    if (searchWholeDoc) 
	    {
		    //	Already searched whole doc: we're done
		    app.beep();
		    messageBox (this.strSearchFailed);
		    this.resetFindReplace (true);
		    return false;
	    }
	    else if( dsaQueryBox( "fr1", this.strSearchFailedRetry)) 
	    {
		    //	Search again, from the top, but don't allow a global search again
		    this.resetFindReplace (false);
		    return this.find (true);
	    }
	    else 
	    {
		    this.resetFindReplace (true);
		    return false;
	    }
    }
}

DocFindReplace.prototype.findInText = function( hiddenSearchDoc, text, title, scriptID, target, engine )
{
	var ok = true;

    var searchString    = this.searchEdit.text;
    var options         = this.getMatchOptions(false);
    var lastMatchedLine = -1;
    
    hiddenSearchDoc.text = text;

    while( ok && hiddenSearchDoc.find( searchString, options ) )
    {
		var sel = hiddenSearchDoc.getSelection();
        
        if( sel[0] != lastMatchedLine )
        {
            lastMatchedLine = sel[0];  
            
            var foundLine = {   text        : hiddenSearchDoc.lines [sel[0]],
                                scriptID    : scriptID,
                                title       : title,
								sel			: sel,
                                searchStr   : searchString,
                                where       : this.where,
                                target      : target,
                                engine      : engine
                            };
                      
            ok = findResult.addResult( foundLine );
		}
    }
	return ok;
}

DocFindReplace.prototype.onNotify = function( reason, param01, param02, param03, param04 )
{
    switch( reason )
    {
		case 'shutdown':
		{
			if( this.hiddenSearchWin )
				this.hiddenSearchWin = null;
				
			globalBroadcaster.unregisterClient( this );
			
			if( !param01 )
				this.updatePrefs();
		}
		break; 
	
        case 'numDocsChanged':
			var docsOpen = (documents.length != 0);
			if (this.docInfoInTargetList != docsOpen)
	            this.updateTargetList();
            break;

        case 'favoritesChanged':
			this.docInfoInTargetList = undefined;
			this.updateTargetList();
			break;
    }
}

DocFindReplace.setScriptSource = function( source )
{
    findReplaceObj.awaitedScriptSource  = source;
    findReplaceObj.receivedScriptSource = true;
}

DocFindReplace.checkScriptSource = function()
{
    return !findReplaceObj.receivedScriptSource;
}

// Helper: count the number of all files to search

DocFindReplace.prototype.countFiles = function (folder, filter, recursive)
{
    var files = folder.getFiles( filter );
	var count = files.length;
	if (recursive)
	{
		files = folder.getFiles ();
		for (var i = 0; i < files.length; i++)
		{
			folder = files [i];
			if (folder instanceof Folder)
				count += this.countFiles (folder, filter, recursive);
		}
	}
	return count;
}

// Helper: search all files in a folder

DocFindReplace.prototype.searchInFolder = function (hiddenSearchDoc, folder, filter, recursive)
{
    var files = folder.getFiles( filter );

    for( var i=0; i<files.length; i++ )
    {
		var f = files [i];
		if (f.alias)
			f = files [i] = f.resolve();
		if( f && !( f instanceof Folder ) )
		{
			findResult.incProgress();
			
			var text = docMgr.safeRead (f);
			if (null == text)
				break;
			
			if( text.length > 0 )
			{
				var title = decodeURIComponent (files[i].getRelativeURI (this.baseFolder));
				if (!this.findInText( hiddenSearchDoc, text, title, f.absoluteURI ))
					return false;
			}
		}
    }

	if (recursive)
	{
		files = folder.getFiles ();
		for (var i = 0; i < files.length; i++)
		{
			folder = files [i];
			var resolved = folder.resolve();
			if (resolved)
				folder = resolved;

			if (folder instanceof Folder)
			{
				if (!this.searchInFolder (hiddenSearchDoc, folder, filter, recursive))
					return false;
			}
		}
	}
	return true;
}

DocFindReplace.prototype.findInFiles = function()
{
    var searchString    = this.searchEdit.text;
    var options         = this.getMatchOptions(true);
	var found			= false;

	if( !this.hiddenSearchWin )
	{
		this.hiddenSearchWin = new Window( 'palette', 'hidden' );
		this.hiddenSearchWin.hiddenSearchDoc = this.hiddenSearchWin.add( 'document' );
		this.hiddenSearchWin.show();
		this.hiddenSearchWin.hide();
	}
	
    switch( this.where )
    {
        case DocFindReplace.WHERE_ALLDOCS:
        {
            findResult.startSearch( searchString, 
                                        documents.length, 
                                        this.clearResults.value,
										"" );
            
            for( var i=0; i<documents.length; i++ )
            {
                findResult.incProgress();
                if (!this.findInText( this.hiddenSearchWin.hiddenSearchDoc, documents[i].getText(), documents[i].paneTitle, documents[i].scriptID ))
					break;
            }

            found = findResult.endSearch();
        }
        break;
        
        case DocFindReplace.WHERE_FAVORITES:
        {
            var folder = new Folder( this.wherePath );
            
            if( folder.exists )
            {
				var resolved = folder.resolve();
				if (resolved)
					folder = resolved;

				this.baseFolder = folder.absoluteURI;
                var files = folder.getFiles( '*.jsx' );

                findResult.startSearch( searchString, 
                                            files.length, 
                                            this.clearResults.value );
                
				this.searchInFolder (this.hiddenSearchWin.hiddenSearchDoc, folder, this.filter, this.recursive)
                
                found = findResult.endSearch();
				this.baseFolder = "";
            }
        }
        break;
        
        case DocFindReplace.WHERE_CURRENGINE:
        {
            var target  = targetMgr.getActiveTarget();
            var session = targetMgr.getActiveSession();

            if( target && session && target.getConnected() )
            {
				var scriptList = this.getScriptList( session );

                if( scripts )
                {
                    findResult.startSearch( searchString, 
                                                scriptList.length, 
                                                this.clearResults.value );

                    for( var i=0; i<scriptList.length; i++ )
                    {
                        findResult.incProgress();
                        
                        var source = '';
                        
                        var doc = docMgr.find( scriptList[i].scriptID );
                        
                        if( doc )
                            source = doc.getText();
                        else
                        {
							this.awaitedScriptSource  = null;
							this.receivedScriptSource = false;
							
							scripts.getSource( scriptList[i], target, session, new Callback( DocFindReplace.setScriptSource ) )
							
                            if( wait( DocFindReplace.checkScriptSource ) )
                                source = findReplaceObj.awaitedScriptSource;
						}
						
						if( source && source.length > 0 )
						{
							if (!this.findInText( this.hiddenSearchWin.hiddenSearchDoc, source, scriptList[i].label, scriptList[i].scriptID, target.address.label, session.address.engine ))
								break;
						}
                    }
            
                    found = findResult.endSearch();
                }
            }
        }
        break;
    }
	
	return found;
}

/*
Get a flattened list of scripts. If the target returns a folder,
search this folder recursively.
*/

DocFindReplace.prototype.getScriptList = function( session )
{
    var ret = [];
    
    if( session && session.initialized() )
    {
        try
        {
            var job    = session.sessionObj.getScripts();
            var result = cdicMgr.callSynchronous( job );
            if( result && result.length == 1 )
			{
                ret = result[0];
				var arr = ret;
				ret = [];
				for (var i = 0; i < arr.length; i++)
				{
					// Top-level joy: check out folders and call _getScriptListInFolder() for folders
					var file = File (arr[i].scriptID);
					if (file instanceof Folder)
						this._getScriptListInFolder( file, arr );
					else
						// file or something else
						ret.push( arr[i] );
				}
			}
        }
        catch( exc )
        {
            
        }
    }
    else
    {
//TODO: not connected
    }    

    return ret;
}

// Recursive worker for above method.

DocFindReplace.prototype._getScriptListInFolder = function( folder, arr )
{
	var files = folder.getFiles();
	for (var i = 0; i < files.length; i++)
	{
		var file = files [i];
		if (file instanceof Folder)
			this._getScriptListInFolder( file, arr );
		else
		{
			var ext = file.name;
			ext = ext.substr (ext.lastIndexOf ('.'));
			if (ext == ".jsx" || ext == ".jsxinc" || ext == ".js")
				// ScriptInfo: ID = path, name = name, (executable), readonly
				arr.push( new ScriptInfo( file.absoluteURI, file.name, (ext == ".jsx"), file.readOnly ));
		}
    }
}    

/*	replace(replaceAll, searchWholeDoc)
	Replace the currently selected text with the contents of
	findGrp.replaceEdit.text.
	* If 'replaceAll' is true, replace all instances of the selected
	  text in the document.
	* If 'searchWholeDoc' is true, search the entire document,
	  otherwise search from the insertion point to the end of the document.
	Return:
	* true if a match was found
	
	Called from the Replace, Replace & Find, and Replace All button
	event handlers, or from a keyboard shortcut handler.
*/
DocFindReplace.prototype.replace = function(replaceAll, searchWholeDoc)
{
	var options = this.getMatchOptions (searchWholeDoc);
	if (replaceAll)
		options += Document.FIND_REPLACEALL;

	var searchString = this.searchEdit.text;
	var replaceString = this.replaceEdit.text;
	//	Remember that we searched for this text, for preferences
	this.searchEdit.activeSearchString = searchString;
	//	Remember that we used this text for replacements, for preferences
	this.replaceEdit.activeReplaceString = replaceString;
	var replacements = 0;
	
    replacements = document.replace (searchString, replaceString, options);
	    
	if (replacements > 0) 
	{
		//	Match(es) found: reset search state
		this.resetFindReplace (false);
		if (replaceAll)
		{
			//	Tell user how many replacements were made
			if (replacements == 1)
				messageBox (this.strOneReplacementDone);
			else
				messageBox (this.strReplacementDone, replacements);
		}
		return true;
	}
	else 
	{
		/*	Match not found: if already in 'search the whole doc'
			mode, note the search failure, else, ask if user
			wants to search from the beginning */
		if (searchWholeDoc) {
			//	Already searched whole doc: we're done
			messageBox (this.strSearchFailed);
			this.resetFindReplace (true);
			return false;
		}
		else if( dsaQueryBox( "fr2", this.strSearchFailedRetry)) {
			//	Search again, from the top, but don't allow a global search again
			this.resetFindReplace (false);
			return this.replace (replaceAll, true);
		}
		else {
			this.resetFindReplace (true);
			return false;
		}
	}
} // DFR_replace

/*	show()
	show the Find/Replace palette. If this is the first call to show()
	in this invocation of the IDE, load (or create) the find/replace preferences.
	Called from the Find/Replace menu item onSelect handler
*/
DocFindReplace.prototype.show = function()
{
    if( this.palette.minimized )
        this.palette.minimized = false;
        
    if( this.palette.visible )
		addDelayedTask( this.resetFocus, this.searchEdit, this.palette );
    else
	    this.palette.show();
} // DFR_show

///////////////////////////////////////////////////////////////////////////////
//
// 
//

/*	initFromPrefs()
	Initialize values of various controls from the saved 'preference' values.
	Called from first 'show' of the F/R palette.
*/

DocFindReplace.prototype.initFromPrefs = function()
{
	if (prefs.findReplacePrefs.paletteLocation != null)
		this.palette.frameLocation = prefs.findReplacePrefs.paletteLocation;
	else
		this.palette.center(window);

	this.searchEdit.activeSearchString   = prefs.findReplacePrefs.searchString.getValue( Preference.STRING );
	this.replaceEdit.activeReplaceString = prefs.findReplacePrefs.replaceString.getValue( Preference.STRING );
	this.matchCase.value			     = prefs.findReplacePrefs.matchCase.getValue( Preference.BOOLEAN );
	this.wholeWord.value			     = prefs.findReplacePrefs.wholeWord.getValue( Preference.BOOLEAN );
	this.matchRegExp.value				 = prefs.findReplacePrefs.matchRegExp.getValue( Preference.BOOLEAN );
	this.clearResults.value				 = prefs.findReplacePrefs.clearresults.getValue( Preference.BOOLEAN );
	
	if( this.searchEdit.activeSearchString.length > 0 )
		//	Initially, if there's a search string, make the
		//	Find button respond to Enter key
		this.palette.defaultElement = this.findBtn;

	this.resetFindReplace (true);

} // DFR_initFromPrefs


/*	updatePrefs()
	Update the find/replace 'preferences' object with current values from the controls.
*/

DocFindReplace.prototype.updatePrefs = function()
{
	prefs.findReplacePrefs.paletteLocation = this.palette.frameLocation;
	
	prefs.findReplacePrefs.searchString     = this.searchEdit.activeSearchString;
	prefs.findReplacePrefs.replaceString    = this.replaceEdit.activeReplaceString;
	prefs.findReplacePrefs.matchCase        = this.matchCase.value;
	prefs.findReplacePrefs.wholeWord        = this.wholeWord.value;
	prefs.findReplacePrefs.matchRegExp      = this.matchRegExp.value;
	prefs.findReplacePrefs.clearresults     = this.clearResults.value;
} // DFR_updatePrefs


/*	resetFindReplace()
	Reset states of the 'find' and 'replace' controls after a state change.
	'newSearch' indicates a new search string: false means searching again
	for same string.
*/
DocFindReplace.prototype.resetFindReplace = function(newSearch)
{
	var palette             = this.palette;
	var haveSearchString    = this.currentSearchString().length > 0;
	var docIsWriteable      = true; // replace with "! document.readOnly" if we need to disable F/R for RO docs
	var multiFind			= (this.where >= DocFindReplace.WHERE_ALLDOCS);

	var btnText;
	if (multiFind)
		btnText = this.strFindAll;
	else
		btnText = ( newSearch || documents.length == 0 ) ? this.strFind : this.strFindNext;

	this.findBtn.text           = localize ( btnText );
	this.findBtn.enabled        = haveSearchString;
	this.replaceBtn.enabled     =
	this.replaceFindBtn.enabled	=
	this.replaceAllBtn.enabled  = ( haveSearchString                             && 
	                                documents.length > 0                         &&
	                                docIsWriteable                               && 
	                                ( this.where == DocFindReplace.WHERE_CURRDOC || 
	                                  this.where == DocFindReplace.WHERE_CURRSEL ) );
	this.clearResults.enabled = multiFind;
} // DFR_resetFindReplace


/*	resetFocus(control)
	Set keyboard focus to the given 'control', or to the palette if control == null.
*/
DocFindReplace.prototype.resetFocus = function( control, palette, onShow )
{
	var palette = palette;
	
	if( !palette )
		palette = this.palette;
	
	if( palette && !palette.active )
		palette.active = true;

	if (control)
	{
		if( _win && onShow && control.active )
			control.active = false;

		if( !_win && control.active )
			control.active = false;

		control.active = true;
	}
} // DFR_resetFocus


/*	getMatchOptions()
	Return the 'match options' flags to pass to Document.find() or
	Document.replace(), based on the current dialog match options
	checkboxes and the 'searchWholeDoc' parameter.
*/
DocFindReplace.prototype.getMatchOptions = function(searchWholeDoc)
{
	var options = 0;
	if (searchWholeDoc)
		options += Document.FIND_WRAPAROUND;
	if (this.matchCase.value == 0)
		options += Document.FIND_IGNORECASE;
	if (this.wholeWord.value != 0)
		options += Document.FIND_WORDS;
	if (this.matchRegExp.value != 0)
		options += Document.FIND_REGEXP;
	if( this.where == DocFindReplace.WHERE_CURRSEL )
	    options += Document.FIND_SELECTION;
	return options;
} // DFR_getMatchOptions

/*	currentSearchString()
	Return the current string to search for: may be empty.
*/
DocFindReplace.prototype.currentSearchString = function()
{
	return this.searchEdit.text;
} // currentSearchString
