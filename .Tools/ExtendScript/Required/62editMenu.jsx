/**************************************************************************
*
*  @@@BUILDINFO@@@ 62editMenu-2.jsx 3.5.0.47	09-December-2009
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
// The Edit menu

menus.edit = new MenuElement ("menu", "$$$/ESToolkit/Menu/Edit=&Edit",
								 "at the end of menubar", "edit");

addDelayedTask (setupEditMenu);

function setupEditMenu()
{
	menus.edit.undo = new MenuElement ("command", "$$$/ESToolkit/Menu/Edit/Undo=&Undo",
									   "at the end of edit", "edit/undo");

	menus.edit.redo = new MenuElement ("command", "$$$/ESToolkit/Menu/Edit/Redo=&Redo",
									   "at the end of edit", "edit/redo");

	menus.edit.cut  = new MenuElement ("command", "$$$/ESToolkit/Menu/Edit/Cut=Cu&t",
										  "--at the end of edit", "edit/cut");

	menus.edit.copy = new MenuElement ("command", "$$$/ESToolkit/Menu/Edit/Copy=&Copy",
									   "at the end of edit", "edit/copy");

	menus.edit.paste = new MenuElement ("command", "$$$/ESToolkit/Menu/Edit/Paste=&Paste",
										"at the end of edit", "edit/paste");

	menus.edit.selectAll = new MenuElement ("command", "$$$/ESToolkit/Menu/Edit/SelectAll=Select &All",
											"--at the end of edit", "edit/selectAll");

	menus.edit.selectMatchingBrace = new MenuElement ("command", "$$$/ESToolkit/Menu/Edit/SelectMatchingBrace=Select to &Brace",
											"at the end of edit", "edit/selectMatchingBrace");

	menus.edit.selectToMatchingBrace = new MenuElement ("command", "$$$/ESToolkit/Menu/Edit/SelectIncludingMatchingBrace=Select &Including Brace",
											"at the end of edit", "edit/selectToMatchingBrace");

	menus.edit.checkSyntax = new MenuElement ("command", "$$$/ESToolkit/Menu/Edit/CheckSyntax=Check &Syntax",
											  "--at the end of edit", "edit/checkSyntax");

	menus.edit.version = new MenuElement ("command", "$$$/ESToolkit/Menu/Edit/Version=Insert &Version Tag",
											  "at the end of edit", "edit/version");
									          
	menus.edit.blockcomment = new MenuElement ("command", "$$$/ESToolkit/Menu/Edit/Comment=Co&mment or Uncomment Selection",
											  "at the end of edit", "edit/blockcomment");
									          
	menus.edit.findReplace = new MenuElement( "command", "$$$/ESToolkit/Menu/Edit/FindReplace=&Find and Replace",
											  "--at the end of edit", "edit/findReplace");

	menus.edit.findNext = new MenuElement("command", "$$$/ESToolkit/Menu/Edit/FindNext=Find &Next",
											  "at the end of edit", "edit/findNext");

	menus.edit.gotoLine = new MenuElement("command", "$$$/ESToolkit/Menu/Edit/GotoLine=&Go to Line...",
											  "at the end of edit", "edit/gotoline");

	menus.edit.clearConsole = new MenuElement ("command", "$$$/ESToolkit/Menu/Edit/ClearConsole=Clear Console",
											  "--at the end of edit", "edit/clearConsole");

	menus.edit.codeCollapse = new MenuElement ("menu", "$$$/ESToolkit/Menu/view/CodeCollapse=&Code Collapse",
											  "--at the end of edit", "edit/codeCollapse");

	menus.edit.codeCollapse.ccExpandAll = new MenuElement("command", "$$$/ESToolkit/Menu/Edit/ccExpandAll=&Expand All",
											  "at the end of edit/codeCollapse", "edit/codeCollapse/ccExpandAll");

	menus.edit.codeCollapse.ccCollapseAll = new MenuElement("command", "$$$/ESToolkit/Menu/Edit/ccCollapseAll=&Collapse All",
											  "at the end of edit/codeCollapse", "edit/codeCollapse/ccCollapseAll");

	menus.edit.bookmarks = new MenuElement ("menu", "$$$/ESToolkit/Menu/Edit/Bookmarks=Boo&kmarks",
											  "at the end of edit", "edit/bookmarks");

	menus.edit.bookmarks.toggleBM = new MenuElement("command", "$$$/ESToolkit/Menu/Edit/ToggleBM=&Toggle Bookmark",
											  "at the end of edit/bookmarks", "edit/bookmarks/toggleBM");

	menus.edit.bookmarks.remAllBMs = new MenuElement("command", "$$$/ESToolkit/Menu/Edit/RemAllBM=Remove &All Bookmarks",
											  "at the end of edit/bookmarks", "edit/bookmarks/remAllBM");

	menus.edit.bookmarks.gotoNextBM = new MenuElement("command", "$$$/ESToolkit/Menu/Edit/gotoNextBM=&Next Bookmark",
											  "at the end of edit/bookmarks", "edit/bookmarks/gotoNextBM");

	menus.edit.bookmarks.gotoPrevBM = new MenuElement("command", "$$$/ESToolkit/Menu/Edit/gotoPrevBM=&Previous Bookmark",
											  "at the end of edit/bookmarks", "edit/bookmarks/gotoPrevBM");

	menus.edit.lineEnds = new MenuElement ("menu", "$$$/ESToolkit/Menu/Edit/LineEnds=&Line Endings",
											  "at the end of edit", "edit/lineEnds");


	menus.edit.lineEnds.mac = new MenuElement ("command", "$$$/ESToolkit/Menu/Edit/LineEnds/Mac=&Macintosh",
											  "at the end of edit/lineEnds", "edit/lineEnds/mac");

	menus.edit.lineEnds.unix = new MenuElement ("command", "$$$/ESToolkit/Menu/Edit/LineEnds/Unix=&Unix",
											  "at the end of edit/lineEnds", "edit/lineEnds/unix");

	menus.edit.lineEnds.win = new MenuElement ("command", "$$$/ESToolkit/Menu/Edit/LineEnds/Win=&Windows",
											  "at the end of edit/lineEnds", "edit/lineEnds/win");

	menus.edit.tabs = new MenuElement ("menu", "$$$/ESToolkit/Menu/Edit/Tabs=Tab Stops",
											  "at the end of edit", "edit/tabs");

	menus.edit.tabs[2] = new MenuElement ("command", "2", "at the end of edit/tabs", "edit/tabs/2");
	menus.edit.tabs[4] = new MenuElement ("command", "4", "at the end of edit/tabs", "edit/tabs/4");
	menus.edit.tabs[6] = new MenuElement ("command", "6", "at the end of edit/tabs", "edit/tabs/6");
	menus.edit.tabs[8] = new MenuElement ("command", "8", "at the end of edit/tabs", "edit/tabs/8");
	menus.edit.tabs[12] = new MenuElement ("command", "12", "at the end of edit/tabs", "edit/tabs/12");
	menus.edit.tabs[16] = new MenuElement ("command", "16", "at the end of edit/tabs", "edit/tabs/16");

    menus.edit.preferences = new MenuElement ("command", "$$$/ESToolkit/Menu/Edit/Preferences=Pr&eferences...",
								              "--at the end of edit", "tools/preferences");

	menus.edit.undo.onDisplay = function()
	{
		this.enabled = docMgr.isActiveDocumentWin() ? document.canUndo() : false;

		if( this.enabled && docMgr.isActiveDocumentWin() && workspace.hasFocus( workspace.activeDocument ) )
		{
			var currSession = document.getCurrentSession();

			if(currSession) {
				this.enabled = !currSession.isDebugging( document );
			}
		}
	}

	menus.edit.redo.onDisplay = function()
	{
		this.enabled = docMgr.isActiveDocumentWin() ? document.canRedo() : false;

		if( this.enabled && docMgr.isActiveDocumentWin() && workspace.hasFocus( workspace.activeDocument ) )
		{
			var currSession = document.getCurrentSession();

			if(currSession) {
				this.enabled = !currSession.isDebugging( document );
			}
		}
	}

	menus.edit.cut.onDisplay = function()
	{
		this.enabled = app.canCopy();

		if( this.enabled && docMgr.isActiveDocumentWin() && workspace.hasFocus( workspace.activeDocument ) )
		{
			var currSession = document.getCurrentSession();

			if(currSession) {
				this.enabled = !currSession.isDebugging( document );
			}
		}
	}

	menus.edit.copy.onDisplay = function()
	{
		this.enabled = app.canCopy();
	}

	menus.edit.paste.onDisplay = function()
	{
		this.enabled = app.canPaste();

		if( this.enabled && docMgr.isActiveDocumentWin() && workspace.hasFocus( workspace.activeDocument ) )
		{
			var currSession = document.getCurrentSession();
			if(currSession) {
				this.enabled = !currSession.isDebugging( document );
			}
		}
	}

	menus.edit.selectAll.onDisplay = function()
	{
	    this.enabled = app.canSelectAll();
	}
	
	menus.edit.checkSyntax.onDisplay = 
	menus.edit.lineEnds.onDisplay = function()
	{
		this.enabled = docMgr.isActiveDocumentWin();
	}

	menus.edit.selectMatchingBrace.onDisplay =
	menus.edit.selectToMatchingBrace.onDisplay = function()
	{
		this.enabled = docMgr.isActiveDocumentWin();
	}

	menus.edit.version.onDisplay = function()
	{
		this.enabled = (document && document.isFile() && docMgr.isActiveDocumentWin() );
	}

	menus.edit.blockcomment.onDisplay = function()
	{
		this.enabled = ( document                                                   && 
		                 document.isSourceDocument                                  &&
		                 docMgr.isActiveDocumentWin()                               &&
						 languages[document.langID].comment.block.toString().length > 0 );
	}

	menus.edit.findReplace.onDisplay = function ()
	{
		this.enabled = true;//(document != null);
	}

	menus.edit.findReplace.onSelect = function ()
	{
		if( app.modalState ) return;

		findReplaceObj.show();
	}

	menus.edit.findReplace.close = function()
	{
		findReplaceObj.close();
	}

	menus.edit.findNext.onSelect = function ()
	{
		if( app.modalState ) return;

		findReplaceObj.find(false);
	}

	menus.edit.findNext.onDisplay = function()
	{
		this.enabled = (document != null) && findReplaceObj && (findReplaceObj.currentSearchString().length > 0);
	}
	
	menus.edit.gotoLine.onDisplay = function()
	{
	    this.enabled = document                                 && 
	                   document.isSourceDocument                &&
	                   docMgr.isActiveDocumentWin()             && 
	                   ( document.getLines().length > 1 );
	}

	menus.edit.gotoLine.onSelect = function()
	{
		if( app.modalState ) return;

	    if( document && document.isSourceDocument )
	    {
	        var lineNum = document.getLines().length;
    	    
            var dlg = new Window( 
                """prefdialog { 
                    text        : '$$$/ESToolkit/Dialog/Edit/GotoLine=Go to Line',
                    orientation : 'column',
                    properties  : { name : 'gotoline' },
                    ge  : Group
                    {
                        orientation : 'column',
                        tn          : StaticText
                        {
                            alignment   : 'left',
                            text        : ' '
                        },
                        lineNum     : EditText
                        {
                            alignment   : ['fill','top'],
                            helpTip     : '$$$/ESToolkit/Dialog/Edit/GotoLine/htLinenum=New line number.'
                        }
                    },
                    gb  : Group
                    {
                        orientation : 'row',
                        btOK        : Button
                        {
                            properties  : { name : 'ok' },
                            text        : '$$$/CT/ExtendScript/UI/OK=&OK'
                        },
                        btCancel    : Button
                        {
                            properties  : { name : 'cancel' },
                            text        : '$$$/CT/ExtendScript/UI/Cancel=&Cancel'
                        }
                    }
                }""" );
    	    
	        dlg.ge.tn.text = localize( '$$$/ESToolkit/Dialog/Edit/GotoLine/Range=Line number (1 - %1):', lineNum );
	        
	        dlg.onShow = function()
	        {
	            this.ge.lineNum.active = true;
	        }
			
			dlg.ge.lineNum.onChanging = function()
			{
				if( this.text && this.text.length > 0 )
				{
					var num = isFinite( this.text ) ? parseInt( this.text, 10 ) : NaN;
					
					if( isNaN( num ) || num <= 0 || num > lineNum )
						this.text = "";
				}
			}
    	    
			workspace.storeFocus();

	        if( dlg.show() )
	        {
				workspace.restoreFocus();

				var newLine = isFinite( dlg.ge.lineNum.text ) ? parseInt( dlg.ge.lineNum.text, 10 ) : NaN;
    	        
	            if( !isNaN( newLine ) && newLine > 0 && newLine <= lineNum )
	            {
	                document.editor.active = true;
	                document.setSelection( newLine-1, 0 );
	                document.scrollSelection();
	            }
	        }
			else
				workspace.restoreFocus();
	    }
	}

	function clearConsole()
	{
		if( console)
		{
			console.clear();
		}
	}

	menus.edit.clearConsole.onSelect = function()
	{
		if( app.modalState ) return;

		if( console && console.pane )
		{
			workspace.resetFocus();
			addDelayedTask( clearConsole );
		}
	}

	menus.edit.codeCollapse.ccExpandAll.onDisplay =
	menus.edit.codeCollapse.ccCollapseAll.onDisplay = function()
	{
		this.enabled = ( docMgr.isActiveDocumentWin() && document && document.isSourceDocument );
	}

	menus.edit.codeCollapse.ccExpandAll.onSelect = function()
	{
		if( app.modalState ) return;

	    if( document && document.editor )
			document.editor.toggleFoldingAll(1);
	}

	menus.edit.codeCollapse.ccCollapseAll.onSelect = function()
	{
		if( app.modalState ) return;

	    if( document && document.editor )
			document.editor.toggleFoldingAll(2);
	}

	menus.edit.bookmarks.toggleBM.onDisplay =
	menus.edit.bookmarks.remAllBMs.onDisplay =
	menus.edit.bookmarks.gotoNextBM.onDisplay =
	menus.edit.bookmarks.gotoPrevBM.onDisplay = function()
	{
		this.enabled = docMgr.isActiveDocumentWin();
	}

	menus.edit.bookmarks.toggleBM.onSelect = function()
	{
		if( app.modalState ) return;

	    if( document )
		    document.toggleBookmark();
	}

	menus.edit.bookmarks.remAllBMs.onSelect = function()
	{
		if( app.modalState ) return;

		if( document )
		    document.removeAllBookmarks();
	}

	menus.edit.bookmarks.gotoNextBM.onSelect = function()
	{
		if( app.modalState ) return;

		if( document )
		{
		    document.nextBookmark();
		    document.scrollSelection();
		}
	}

	menus.edit.bookmarks.gotoPrevBM.onSelect = function()
	{
		if( app.modalState ) return;

		if( document )
		{
		    document.previousBookmark();
		    document.scrollSelection();
		}
	}

	menus.edit.undo.onSelect = function()
	{
		if( app.modalState ) return;
		if( !this.enabled ) return;

		if (document) 
			document.undo();
	}

	menus.edit.redo.onSelect = function()
	{
		if( app.modalState ) return;
		if( !this.enabled ) return;

		if (document) 
			document.redo();
	}

	menus.edit.cut.onSelect = function()
	{
		if( app.modalState ) return;
		if( !this.enabled ) return;

		app.cut();
	}

	menus.edit.copy.onSelect = function()
	{
		if( app.modalState ) return;

		app.copy();
	}

	menus.edit.paste.onSelect = function()
	{
		if( app.modalState ) return;
		if( !this.enabled ) return;

		app.paste();
	}

	menus.edit.selectAll.onSelect = function()
	{
		if( app.modalState ) return;

	    app.selectAll();
	}

	menus.edit.selectMatchingBrace.onSelect= function()
	{
	    if( document.editor )
	    {
		    var braceStyle = languages[document.langID].braces.@style;
    	    
		    if( braceStyle && braceStyle.toString().length > 0 )
		    {
			    braceStyle = parseInt( braceStyle.toString(), 10 );
    	        
			    if( !isNaN( braceStyle ) )
			    {
				    if( document.editor.selectMatchingBrace( braceStyle ) )
					    document.scrollSelection();
			    }
		    }
		}
	}

	menus.edit.selectToMatchingBrace.onSelect = function()
	{
		if( app.modalState ) return;

	    if( document.editor )
	    {
		    var braceStyle = languages[document.langID].braces.@style;
    	    
		    if( braceStyle && braceStyle.toString().length > 0 )
		    {
			    braceStyle = parseInt( braceStyle.toString(), 10 );
    	        
			    if( !isNaN( braceStyle ) )
			    {
				    if( document.editor.selectBraces( braceStyle ) )
					    document.scrollSelection();
			    }
		    }
		}
	}

	menus.edit.checkSyntax.onSelect = function()
	{
		if( app.modalState ) return;

		if( document && document.isSourceDocument )
			document.checkSyntax();
	}

	menus.edit.version.onSelect = function()
	{
		if( app.modalState ) return;

		if( document && document.isSourceDocument )
			document.insertVersionTag();
	}

	menus.edit.blockcomment.onSelect = function()
	{
		if( app.modalState ) return;

		if( document && document.isSourceDocument )
			document.editor.comment( languages[document.langID].comment.block, true );
	}

	menus.edit.lineEnds.win.onDisplay = function()
	{
		if (docMgr.isActiveDocumentWin())
			this.checked = (document.lf == "windows");
	}

	menus.edit.lineEnds.mac.onDisplay = function()
	{
		if (docMgr.isActiveDocumentWin())
			this.checked = (document.lf == "macintosh");
	}

	menus.edit.lineEnds.unix.onDisplay = function()
	{
		if (docMgr.isActiveDocumentWin())
			this.checked = (document.lf == "unix");
	}

	menus.edit.lineEnds.win.onSelect = function()
	{
		if( app.modalState ) return;

		if (document)
			document.lf = "windows";
	}

	menus.edit.lineEnds.mac.onSelect = function()
	{
		if( app.modalState ) return;

		if (document)
			document.lf = "macintosh";
	}

	menus.edit.lineEnds.unix.onSelect = function()
	{
		if( app.modalState ) return;

		if (document)
			document.lf = "unix";
	}
	
	menus.edit.tabs[2].onDisplay = 
	menus.edit.tabs[4].onDisplay = 
	menus.edit.tabs[6].onDisplay = 
	menus.edit.tabs[8].onDisplay = 
	menus.edit.tabs[12].onDisplay = 
	menus.edit.tabs[16].onDisplay = function()
	{
		if( docMgr.isActiveDocumentWin() && document.getEditor() )
			this.checked = ( document.getEditor().tabs.toString() == this.text );
	}

	menus.edit.tabs[2].onSelect = 
	menus.edit.tabs[4].onSelect = 
	menus.edit.tabs[6].onSelect = 
	menus.edit.tabs[8].onSelect = 
	menus.edit.tabs[12].onSelect = 
	menus.edit.tabs[16].onSelect = function()
	{
		if( app.modalState ) return;

		if( document && document.getEditor() )
			document.getEditor().tabs = parseInt( this.text );
	}

	menus.edit.preferences.onSelect = function()
	{
		if( app.modalState ) return;

		workspace.storeFocus();
	    openPreferencesDialog();
		workspace.restoreFocus();
	}

    globalBroadcaster.notifyClients( 'updateMenu_Edit' );
}
