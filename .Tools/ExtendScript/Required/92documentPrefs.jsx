/**************************************************************************
*
*  @@@BUILDINFO@@@ 92documentPrefs-2.jsx 3.5.0.7		08-December-2008
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

// Each Prefs object needs five methods:
// prefsObject.create (parentPane)  - create the pane and return the Pane object.
// prefsObject.preProcess()         - preprocess the pane just before being shown.
// prefsObject.postProcess()        - do any work just before the pane is being hidden.
// prefsObject.store()              - store the preferences. Return true if the Next Time dialog is needed.
// prefsObject.toDefault()          - set back to default values

var documentPrefs = { title : "$$$/ESToolkit/PreferencesDlg/textTitle=Documents", sortOrder : 20 };

globalBroadcaster.registerClient( documentPrefs, 'initPrefPanes' );

documentPrefs.onNotify = function( reason )
{
    switch( reason )
    {
        case 'initPrefPanes':
        {
            if( arguments[1] instanceof Array )
                arguments[1].push( this );
        }
        break;
    }
}

documentPrefs.tabStopSettings   = [2, 4, 6, 8, 12, 16];
documentPrefs.langObjs          = [];
documentPrefs.lineendings       = [ 'Macintosh', 'Unix', 'Windows' ];

documentPrefs.create = function (parentPane)
{
	this.pane = parentPane.add (
	"""group {
		orientation		: 'column',
		alignChildren	: 'fill',
		visible			: false,
		alignment		: 'fill',
		general		: Panel {
			orientation		: 'column',
			alignment		: ['fill', 'top'],
			alignChildren	: 'left',
			spacing			: 4,
			text            : '$$$/ESToolkit/PreferenceDlg/generalTitle=General',
			autoReload		: Checkbox {
				text			: '$$$/ESToolkit/PreferencesDlg/AutoReload=&Automatic Reload of Changed Files'
			},
			floating			: Checkbox {
				text			: '$$$/ESToolkit/PreferencesDlg/floating=Create new Document in a &floating Window'
			},
			convtab			: Checkbox {
				text			: '$$$/ESToolkit/PreferencesDlg/ConvTab=Convert Tab Stops to Space Characters'
			},
			caretBackground	: Checkbox {
				text			: '$$$/ESToolkit/PreferencesDlg/caretBackground=Highlight current line'
			},
			bkGroup			: Group {
				margin	: 0,
				spacing	: 2,
				orientation	: 'row',
				enableBK	: Checkbox {
					text		: '$$$/ESToolkit/PreferencesDlg/enableBK=Back up documents automatically'
				},
				sizeBoxSpacer   : Group
				{
					alignment	    : ['right','bottom'],
					preferredSize   : [14,1]
				},
				static		: StaticText {
					text		: '$$$/ESToolkit/PreferencesDlg/After=After'
				},
				delayBK		: EditText {
					characters	: 5
				},
				static		: StaticText {
					text		: '$$$/ESToolkit/PreferencesDlg/Seconds=seconds'
				}
			}
		},
		newdocs		: Panel {
			orientation		: 'column',
			alignment		: ['fill', 'top'],
			alignChildren	: 'left',
			spacing			: 4,
			text            : '$$$/ESToolkit/PreferenceDlg/newdocsTitle=New Document',
			lineNums		: Checkbox {
				text			: '$$$/ESToolkit/PreferencesDlg/linenumbers=&Display Line Numbers'
			},
			wrap			: Checkbox {
				text			: '$$$/ESToolkit/PreferencesDlg/wraplines=&Word Wrap'
			},
			fold			: Checkbox {
				text			: '$$$/ESToolkit/PreferencesDlg/folding=&Code Collapse'
			},
			fields			: Group {
				orientation		: 'column',
				alignment		: ['left', 'top'],
				hilite			: Group {
					orientation		: 'row',
					alignment		: ['right', 'top'],
					lbl				: StaticText {
						text			: '$$$/ESToolkit/PreferencesDlg/hilite=Enable &Syntax Highlighting:'
					},
					list			: DropDownList {
						helpTip			: '$$$/ESToolkit/PreferencesDlg/htHilite=Highlight reserved words, comments, etc, with different colors.',
					},
				},
				tabs			: Group {
					orientation		: 'row',
					alignment		: ['right', 'top'],
					lbl				: StaticText {
						text			: '$$$/ESToolkit/PreferencesDlg/tabs=&Tab Stops:'
					},
					list			: DropDownList {
						helpTip			: '$$$/ESToolkit/PreferencesDlg/htTabs=The number of spaces between tab stops.',
					},
				},
				lfs				: Group {
					orientation		: 'row',
					alignment		: ['right', 'top'],
					lbl				: StaticText {
						text			: '$$$/ESToolkit/PreferencesDlg/lf=&Line Endings:'
					},
					list			: DropDownList {
						helpTip			: '$$$/ESToolkit/PreferencesDlg/htLF=The line feed character sequence.',
					},
				},
			}
		},
		signature		: Panel {
			orientation		: 'column',
			alignment		: ['fill', 'top'],
			alignChildren	: 'left',
			spacing			: 4,
			text            : '$$$/ESToolkit/PreferenceDlg/sigTitle=UTF-8 signature',
			sigNever		: RadioButton {
			    text            : '$$$/ESToolkit/PreferenceDlg/sigNever=Never write signature'
			},
			sigAlways		: RadioButton {
			    text            : '$$$/ESToolkit/PreferenceDlg/sigAlways=Always write signature'
			},
			sigPossible		: RadioButton {
			    text            : '$$$/ESToolkit/PreferenceDlg/sigPossible=Write signature if source contains non-ASCII characters'
			}
		}
	}""");
	
	this.pane.hiliteList    = this.pane.newdocs.fields.hilite.list;
	this.pane.tabsList	    = this.pane.newdocs.fields.tabs.list;
	this.pane.lfList	    = this.pane.newdocs.fields.lfs.list;
	this.pane.sigNever      = this.pane.signature.sigNever;
	this.pane.sigAlways     = this.pane.signature.sigAlways;
	this.pane.sigPossible   = this.pane.signature.sigPossible;
	this.pane.enableBK		= this.pane.general.bkGroup.enableBK;
	this.pane.delayBK		= this.pane.general.bkGroup.delayBK;

	this.pane.hiliteList.minimumSize.width =
	this.pane.tabsList.minimumSize.width   =
	this.pane.lfList.minimumSize.width	   = 120;

    this.pane.delayBK.onChange = function()
    {
		var max = isFinite( this.text ) ? parseInt( this.text, 10 ) : NaN;
        
        if( isNaN( max ) || max < 1 || max > 10000 )
            this.text = prefs.document.backupDelay.getValue( Preference.NUMBER );
    }
	
	this.pane.enableBK.onClick = function()
	{
		this.parent.delayBK.enabled = this.value;
		
		if( this.value )
		{
			var delay = isFinite( this.parent.delayBK.text ) ? parseInt( this.parent.delayBK.text ) : NaN;
			
			if( isNaN( delay ) || delay <= 0 )
				this.parent.delayBK.text = "20";
		}
	}

	this.loaded = false;
	this.pane.prefsObj = this;
	this.preProcess = function()
	{
		if (!this.loaded)
			this.load();
	}
	this.postProcess = function()
	{}

	this.toDefault = function()
	{
		this.pane.general.autoReload.value		= prefs.document.autoReload.getDefault( Preference.BOOLEAN );
		this.pane.general.convtab.value			= prefs.document.tabs2spaces.getDefault( Preference.BOOLEAN );
		this.pane.general.caretBackground.value	= prefs.document.caretBackground.getDefault( Preference.BOOLEAN );
		this.pane.newdocs.lineNums.value		= prefs.document.lineNumbers.getDefault( Preference.BOOLEAN );
		this.pane.newdocs.wrap.value			= prefs.document.wrap.getDefault( Preference.BOOLEAN );
		this.pane.newdocs.fold.value			= prefs.document.folding.getDefault( Preference.BOOLEAN );
		
		if( workspace.appFrame )
			this.pane.general.floating.value   = prefs.document.floatingAppFrame.getDefault( Preference.BOOLEAN );
		else
			this.pane.general.floating.value   = prefs.document.floatingNoAppFrame.getDefault( Preference.BOOLEAN );
		
		switch( prefs.document.BOM.getDefault( Preference.NUMBER ) )
		{
		    case 1:     this.pane.sigAlways.value   = true;     break;
		    case 2:     this.pane.sigPossible.value = true;     break;
		    default:    this.pane.sigNever.value    = true;
		}

	    //	Load the possible tab stops and select the preferred one
	    var tabs = prefs.document.tabs.getDefault( Preference.NUMBER );
		
	    for( i=0; i<documentPrefs.tabStopSettings.length; i++ ) 
	    {
		    if( this.pane.tabsList.items[i].text == tabs.toString() )
			    this.pane.tabsList.selection = this.pane.tabsList.items[i];
	    }

	    //	Create a list of languages			
	    var langID = prefs.document.language.getDefault( Preference.STRING );

		for (i = 0; i < documentPrefs.langObjs.length; i++)
	    {
		    if( this.pane.hiliteList.items[i].id == langID )
		        this.pane.hiliteList.selection = this.pane.hiliteList.items[i];
	    }

	    // create a list of lineendings 
	    var le = prefs.document.lineend.getDefault( Preference.STRING );
	    
		if( le == '' )
			// Use the OS line endings as default
			le = Folder.fs;

	    for( i=0; i<documentPrefs.lineendings.length; i++ ) 
	    {
		    if( this.pane.lfList.items[i].text == le.toString() )
			    this.pane.lfList.selection = this.pane.lfList.items[i];
	    }
		
		// backup settings
		var delay = prefs.document.backupDelay.getDefault( Preference.NUMBER );
		
		if( isNaN( delay ) || delay <= 0 )
		{
			this.pane.enableBK.value	= false;
			this.pane.delayBK.enabled	= false;
			this.pane.delayBK.text		= 0;
		}
		else
		{
			this.pane.enableBK.value	= true;
			this.pane.delayBK.enabled	= true;
			this.pane.delayBK.text		= delay;
		}
	}

	return this.pane;
}

/////////////////////////////////////////////////////////////////////////
// load Prefs into the text formatting options pane

documentPrefs.load = function()
{
 	if (this.loaded)
		return;
	this.loaded = true;

	var i;
	with (this.pane) 
	{
		general.autoReload.value		= prefs.document.autoReload.getValue( Preference.BOOLEAN );
		general.convtab.value			= prefs.document.tabs2spaces.getValue( Preference.BOOLEAN );
		general.caretBackground.value	= prefs.document.caretBackground.getValue( Preference.BOOLEAN );
		newdocs.lineNums.value			= prefs.document.lineNumbers.getValue( Preference.BOOLEAN );
		newdocs.wrap.value				= prefs.document.wrap.getValue( Preference.BOOLEAN );
		newdocs.fold.value				= prefs.document.folding.getValue( Preference.BOOLEAN );

		if( workspace.appFrame )
			general.floating.value	 = prefs.document.floatingAppFrame.getValue( Preference.BOOLEAN );
		else
			general.floating.value	 = prefs.document.floatingNoAppFrame.getValue( Preference.BOOLEAN );

		switch( prefs.document.BOM.getValue( Preference.NUMBER ) )
		{
		    case 1:     this.pane.sigAlways.value   = true;     break;
		    case 2:     this.pane.sigPossible.value = true;     break;
		    default:    this.pane.sigNever.value    = true;
		}

	    //	Load the possible tab stops and select the preferred one
	    var tabs         = prefs.document.tabs.getValue( Preference.NUMBER );
		tabsList.removeAll();
	    for( i=0; i<documentPrefs.tabStopSettings.length; i++ ) 
	    {
		    item = tabsList.add( 'item', documentPrefs.tabStopSettings[i] );
		
		    if( item.text == tabs.toString() )
			    tabsList.selection = item;
	    }

	    //	Create a list of languages			
		if( documentPrefs.langObjs.length <= 0 )
		{
			for (i = 0; i < lang.langIDs.length; i++)
			{
				var langID  = lang.langIDs [i];
				var langObj = languages [langID];
				if (langID != "text")
					// use the text without any Windows '&' escapes
					documentPrefs.langObjs.push ( { id	: langID,
									  text	: langObj.menu.toString().replace (/&([^&])/,"$1") } );
			}
		}
		
	    var langID = prefs.document.language.getValue( Preference.STRING );
        hiliteList.removeAll();
		for (i = 0; i < documentPrefs.langObjs.length; i++)
	    {
		    item = hiliteList.add ( "item", documentPrefs.langObjs [i].text );
		    item.id = documentPrefs.langObjs [i].id;				
			
		    if( item.id == langID )
		        hiliteList.selection = item;
	    }
		
	    // create a list of lineendings 
	    var le = prefs.document.lineend.getValue( Preference.STRING );
		if (le == "")
			// Use the OS line endings as default
			le = Folder.fs;
        
        lfList.removeAll();
	    for( i=0; i<documentPrefs.lineendings.length; i++ ) 
	    {
		    item = lfList.add( 'item', documentPrefs.lineendings[i] );
		
		    if( item.text == le.toString() )
			    lfList.selection = item;
	    }
		
		//	Set preferred width of all lists to widest width, with minimum width 100
		this.adjustWidthsToWidest ([hiliteList, tabsList, lfList], 100);

		// backup settings
		var delay = prefs.document.backupDelay.getValue( Preference.NUMBER );
		
		if( isNaN( delay ) || delay <= 0 )
		{
			this.pane.enableBK.value	= false;
			this.pane.delayBK.enabled	= false;
			this.pane.delayBK.text		= 0;
		}
		else
		{
			this.pane.enableBK.value	= true;
			this.pane.delayBK.enabled	= true;
			this.pane.delayBK.text		= delay;
		}
	}
}

/////////////////////////////////////////////////////////////////////////
// set preferences from the selected font options
documentPrefs.store = function()
{
	if (!this.loaded)
		return false;
	with (this.pane) 
	{
		prefs.document.autoReload		= general.autoReload.value;
		prefs.document.tabs2spaces		= general.convtab.value;
		prefs.document.caretBackground	= general.caretBackground.value;
		prefs.document.lineNumbers		= newdocs.lineNums.value;
		prefs.document.wrap				= newdocs.wrap.value;
		prefs.document.folding			= newdocs.fold.value;
		
		if( workspace.appFrame )
			prefs.document.floatingAppFrame	= general.floating.value;
		else
			prefs.document.floatingNoAppFrame	= general.floating.value;

		if( sigAlways.value )           prefs.document.BOM = 1;
		else if( sigPossible.value )    prefs.document.BOM = 2;
		else if( sigNever.value )       prefs.document.BOM = 0;
		
		if (tabsList.selection)
		    prefs.document.tabs     = tabsList.selection.text; 
		if (hiliteList.selection)
			prefs.document.language = hiliteList.selection.id;
		if (lfList.selection)
		    prefs.document.lineend  = lfList.selection.text;

		var delay = isFinite( delayBK.text ) ? parseInt( delayBK.text ) : NaN;
		
		if( isNaN( delay ) || delay <= 0 || !enableBK.value )
			prefs.document.backupDelay = 0;
		else
			prefs.document.backupDelay = delay;
	}
	return false;
}

/////////////////////////////////////////////////////////////////////////
// layout utility: find widest control in 'elements' array, use it as value of 
// preferredSize.width for all 'elements'. Use 'minWidth' as the minimum width.
documentPrefs.adjustWidthsToWidest = function (elements, minWidth)
{
	var i, widest = minWidth;
	for (i = 0; i < elements.length; i++)
		if (elements[i].preferredSize.width > widest)
			widest = elements[i].preferredSize.width;
	for (i = 0; i < elements.length; i++)
		elements[i].preferredSize.width = widest;
}
