/**************************************************************************
*
*  @@@BUILDINFO@@@ 63viewMenu-2.jsx 3.5.0.17	16-March-2009
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

//////////////////////////////////////////////////////////////////////////
// The View menu.

menus.view = new MenuElement ("menu", "$$$/ESToolkit/Menu/View=&View",
								 "at the end of menubar", "view");

addDelayedTask (setupViewMenu);

function setupViewMenu()
{
	menus.view.wrap = new MenuElement ("command", "$$$/ESToolkit/Menu/view/Wrap=&Word Wrap",
											  "at the end of view", "view/wrap");

	menus.view.lineNum = new MenuElement ("command", "$$$/ESToolkit/Menu/view/LineNum=&Line Numbers",
											  "--at the end of view", "view/lineNum");

	menus.view.folding = new MenuElement ("command", "$$$/ESToolkit/Menu/view/CodeCollapse=&Code Collapse",
											  "at the end of view", "view/collapse");

	menus.view.lineNum.onDisplay = function()
	{
		if( document && document.editor && docMgr.isActiveDocumentWin() )
		{
			this.enabled = true;
			this.checked = document.editor.lineNumbers;
		}
		else
			this.checked = this.enabled = false;
	}

	menus.view.wrap.onDisplay = function()
	{
		if( document && document.editor && docMgr.isActiveDocumentWin() )
		{
			this.enabled = true;
			this.checked = document.editor.wrap;
		}
		else
			this.checked = this.enabled = false;
	}

	menus.view.folding.onDisplay = function()
	{
		if( document && document.editor && docMgr.isActiveDocumentWin() )
		{
			this.enabled = true;
			this.checked = (document.editor.folding != 0);
		}
		else
			this.checked = this.enabled = false;
	}

	menus.view.lineNum.onSelect = function()
	{
		if( app.modalState ) return;

		if( document && document.editor )
			document.editor.lineNumbers = !document.editor.lineNumbers;
	}

	menus.view.wrap.onSelect = function()
	{
		if( app.modalState ) return;

		if( document && document.editor )
			document.editor.wrap = !document.editor.wrap;
	}

	menus.view.folding.onSelect = function()
	{
		if( app.modalState ) return;

		if( document && document.editor )
			document.editor.folding = (document.editor.folding > 0) ? 0 : 3;
	}

	// Create the Language menu
	function addLanguage (langID, text, sep)
	{
		var menu = new MenuElement ("command", text, "at the end of language" + sep);
		menu.langID = langID;
		menu.onDisplay = function()
		{
			this.checked = document ? (document.langID == this.langID) : false;
		}
		menu.onSelect = function()
		{
			if( app.modalState ) return;

			if (document)
				document.setLanguage (this.langID);
		}
	}

	menus.view.language = new MenuElement ("menu", "$$$/ESToolkit/Menu/view/Hightlight=&Syntax Highlighting",
										   "--at the end of view", "language");

	menus.view.language.onDisplay = function()
	{
		this.enabled = docMgr.isActiveDocumentWin();
	}
	
	addLanguage (languages.text.localName(), localize ("$$$/ESToolkit/LanguagesMenu/None=&None"), "---");
	var langObjs = [];

	for (var i = 0; i < lang.langIDs.length; i++)
	{
		var langID  = lang.langIDs [i];
		var langObj = languages [langID];
		if (langID != "text")
			// use the text without any WIndows '&; escapes for comparisons
			langObjs.push ( { id		: langID,
							  text		: langObj.menu.toString(),
							  compare	: langObj.menu.toString().replace (/&([^&])/,"$1") } );
	}

	for (i = 0; i < langObjs.length; i++)
		addLanguage (langObjs [i].id, langObjs [i].text, "");
		
    globalBroadcaster.notifyClients( 'updateMenu_View' );
}

