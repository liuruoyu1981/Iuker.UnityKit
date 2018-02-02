/**************************************************************************
*
*  @@@BUILDINFO@@@ 93fontPrefs-2.jsx 3.5.0.47	09-December-2009
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
// prefsObject.cancelled()			- Prefs dialog has been cancelled

var fontPrefs = { title : "$$$/ESToolkit/PreferencesDlg/fontTitle=Font and Colors", sortOrder : 30 };

globalBroadcaster.registerClient( fontPrefs, 'initPrefPanes' );

fontPrefs.onNotify = function( reason )
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

fontPrefs.create = function (parentPane)
{
	this.pane = parentPane.add (
	"group {											\
		orientation		: 'column',						\
		alignChildren	: 'left',						\
		visible			: false,						\
		alignment		: ['fill','fill'],				\
		language		: Group							\
		{												\
			orientation		: 'column',					\
			alignment		: 'fill',					\
			alignChildren	: 'fill',					\
			spacing			: 4,						\
			lblLang			: StaticText				\
			{											\
				alignment	: ['fill','top'],			\
				text		: '$$$/ESToolkit/PreferencesDlg/language=&Show Settings For:' \
			},											\
			grp				: Group						\
			{											\
				orientation		: 'row',				\
				list			: DropDownList			\
				{										\
					alignment		: ['fill','top']	\
				}										\
			},											\
		},												\
		nameSize		: Group							\
		{												\
			orientation		: 'row', 					\
			alignChildren	: 'fill',					\
			alignment		: 'fill',					\
			fontName		: Group						\
			{											\
				orientation		: 'column',				\
				spacing			: 4,					\
				alignment		: ['fill','top'],		\
				alignChildren	: 'fill',				\
				lbl				: StaticText			\
				{										\
					alignment		: ['left','top'],	\
					text			: '$$$/ESToolkit/PreferencesDlg/textFont=&Font:' \
				},										\
				list			: DropDownList			\
				{										\
					alignment		: ['left','top']	\
				},										\
			},											\
			fontSize		: Group						\
			{											\
				orientation		: 'column',				\
				spacing			: 4,					\
				alignChildren	: 'fill',				\
				lbl				: StaticText			\
				{										\
					alignment	: ['fill','top'],		\
					text		: '$$$/ESToolkit/PreferencesDlg/lblSize=Si&ze:' \
				},										\
				list			: DropDownList			\
				{										\
					alignment	: ['fill','top']		\
				},										\
			},											\
		},												\
		lowerGrp		: Group							\
		{												\
			orientation		: 'row',					\
			alignment		: ['fill','fill'],			\
			alignChildren	: 'fill',					\
			lowerLeftGrp	: Group						\
			{											\
				orientation		: 'column',				\
				alignment		: ['left','fill'],		\
				alignChildren	: 'fill',				\
				spacing			: 5,					\
				allBox			: Checkbox			\
				{									\
					text			: '$$$/ESToolkit/PreferencesDlg/allDisItems=&All Display Items'	\
				},									\
				lbl				: StaticText			\
				{										\
					alignment		: ['fill','top'],	\
					text			: '$$$/ESToolkit/PreferencesDlg/lblDiplayItems=&Display Items:' \
				},										\
				styles:	ListBox							\
				{										\
					properties		:					\
					{									\
						multiselect		: true			\
					},									\
					alignment		: ['left','fill']	\
				}										\
			},											\
			lowerRightGrp	: Group						\
			{											\
				orientation		: 'column',				\
				alignment		: ['fill','fill'],		\
				alignChildren	: 'fill',				\
				foreground		: Group					\
				{										\
					orientation		: 'column',			\
					alignment		: ['fill','top'],	\
					alignChildren	: ['fill','top'],	\
					spacing			: 4,				\
					lbl				: StaticText		\
					{									\
						alignment		: ['fill','top'], \
						text			: '$$$/ESToolkit/PreferencesDlg/lblForeground=Item Fo&reground:' \
					},									\
					list			: DropDownList		\
					{									\
						alignment		: ['fill','top']\
					}									\
				},										\
				background		: Group					\
				{										\
					orientation		: 'column',			\
					alignment		: ['fill','top'],	\
					alignChildren	: ['fill','top'],	\
					spacing			: 4,				\
					lbl				: StaticText		\
					{									\
						alignment		: ['fill','top'], \
						text			: '$$$/ESToolkit/PreferencesDlg/lblBackground=Item Bac&kground::' \
					},									\
					list			: DropDownList		\
					{									\
						alignment		: ['fill','top']\
					}									\
				},										\
				txtStyle		: Group					\
				{										\
					orientation		: 'row',			\
					boldBox			: Checkbox			\
					{									\
						text			: '$$$/ESToolkit/PreferencesDlg/bold=&Bold'\
					},									\
					italicBox		: Checkbox			\
					{									\
						text			: '$$$/ESToolkit/PreferencesDlg/italic=&Italics'\
					}									\
				},										\
				keywordsGrp		: Group					\
				{										\
					orientation		: 'column',			\
					alignment		: ['fill','fill'],	\
					alignChildren	: ['fill','fill'],	\
					spacing			: 4,				\
					lbl				: StaticText		\
					{									\
							alignment	: ['left','top'],\
							text		: '$$$/ESToolkit/PreferencesDlg/lblKeywords=Key&words:' \
					},									\
					list			: DropDownList		\
					{									\
						alignment		: ['fill','top']\
					},									\
					edit			: EditText			\
					{									\
						alignment		: ['fill','fill'],\
						preferredSize	: [10,100],		\
						properties		:				\
						{								\
							multiline		: true		\
						}								\
					}									\
				}										\
			}											\
		}												\
	}");

    this.scintillaPrefs = new Object;
    
	// Set up element shortcuts
	var pane = this.pane;
	pane.langList		= pane.language.grp.list;
	pane.fontNameList	= pane.nameSize.fontName.list;
	pane.fontSizeList	= pane.nameSize.fontSize.list;
	pane.foreList		= pane.lowerGrp.lowerRightGrp.foreground.list;
	pane.backList		= pane.lowerGrp.lowerRightGrp.background.list;
	pane.boldBox		= pane.lowerGrp.lowerRightGrp.txtStyle.boldBox; 
	pane.italicBox		= pane.lowerGrp.lowerRightGrp.txtStyle.italicBox; 
	pane.styleList		= pane.lowerGrp.lowerLeftGrp.styles;
	pane.kwdsList		= pane.lowerGrp.lowerRightGrp.keywordsGrp.list;
	pane.kwdsEdit		= pane.lowerGrp.lowerRightGrp.keywordsGrp.edit;
	pane.allBox         = pane.lowerGrp.lowerLeftGrp.allBox;

	// This may not be wider
	pane.styleList.maximumSize.width = 180;
	// make sure that huge font names do not push the size DDL away
	pane.fontNameList.maximumSize.width = 300;
	
	pane.langList.pane		=
	pane.fontNameList.pane	=
	pane.fontSizeList.pane	=
	pane.foreList.pane		=
	pane.backList.pane		=
	pane.boldBox.pane		=
	pane.italicBox.pane		=
	pane.styleList.pane		= 
	pane.kwdsEdit.pane		= 
	pane.kwdsList.pane		= pane;

	this.loaded = false;
	this.pane.prefsObj = this;
	this.preProcess = function()
	{
	    if( !fontPrefs.scintillaPrefs.xml )
	    {
		    fontPrefs.scintillaPrefs.xml                      = new XML( "<syntaxdefs/>" );
		    fontPrefs.scintillaPrefs.xml.languages            = languages.copy();
	    }
	    
	    fontPrefs.scintillaPrefs.backup = fontPrefs.scintillaPrefs.xml;

		if (!this.loaded)
			this.load();

		// Fonts may have changed...
		this.setupFonts();
		
		this.pane.allBox.value = prefs.preferencesDialogPrefs.fontPrefs.allDisplayItems.getValue( Preference.BOOLEAN );
		this.pane.allBox.onClick();
		this.pane.styleList.onChange();
	}
	this.postProcess = function()
	{
		this.pane.kwdsEdit.onChange();
	}

	this.toDefault = function()
	{
		if (this.pane.langList.selection)
		{
			var langID = this.pane.langList.selection.id;
			if (langID != "text")
			{
				var defaults = lang.getLanguagesDefaults();
				fontPrefs.scintillaPrefs.xml.languages[langID] = defaults[langID];
			}			
			this.pane.styleList.onChange();
		}

		this.pane.allBox.value = prefs.preferencesDialogPrefs.fontPrefs.allDisplayItems.getDefault( Preference.BOOLEAN );
		this.pane.allBox.onClick();
	}

	return pane;
}

/////////////////////////////////////////////////////////////////////////
// load the font options pane

fontPrefs.load = function()
{
 	if (this.loaded)
		return;
	this.loaded = true;

	// this takes a bit...
	app.setWaitCursor (true);

	var pane = this.pane;
	var item, i;
	
	////////////////////////////// Helpers ////////////////////////////
	// Populate a color list with standard colors.
	function setupColors (listBox)
	{
	    listBox.removeAll();
	    
		for (var i in colors)
		{
			var item = listBox.add ( "item", " " + localize( "$$$/ESToolkit/Colors/" + i ) );
			item.icon = colorIcons [i];
		}
	}
	
	//////////////////////// End Helpers ////////////////////////////

	//	Create a list of languages			
	var langObjs = [];

	for (i = 0; i < lang.langIDs.length; i++)
	{
		var langID  = lang.langIDs [i];
		var langObj = languages [langID];
//		if (langID != "text")
			// use the text without any Windows '&' escapes
			langObjs.push ( { id	: langID,
							  text	: langObj.menu.toString().replace (/&([^&])/,"$1") } );
	}
	function sortfn (a, b)
	{
		if (a.text < b.text)
			return -1;
		if (a.text > b.text)
			return 1;
		return 0;
	}
	langObjs.sort (sortfn);

    pane.langList.removeAll();
	for (i = 0; i < langObjs.length; i++) 
	{
		var item = pane.langList.add ( "item", langObjs[i].text );
		item.id = langObjs[i].id;				
	}
	
	this.setupFonts();

	//	Create a list of text font sizes
	var sizes = [6,7,8,9,10,11,12,14,16,18,20,22,24,28,32,36,40,48,54,60,72];
	pane.fontSizeList.removeAll();
    for( i = 0; i < sizes.length; i++)
        pane.fontSizeList.add ( "item", sizes [i] );
        
	// Setup colors.
	setupColors (pane.foreList);
	setupColors (pane.backList);

	// language changes, updates styles
	pane.langList.onChange = function()
	{
		if (!this.selection)
			return;
		if( this.selection.text == fontPrefs.scintillaPrefs.xml.language )
			return;
			
		// temporary switch off CheckBox "All Display Items"
		var allState = this.pane.allBox.value;
		this.pane.allBox.value = false;
		// temporary remove onChange handler for ListBox "Display Items"
		var tmpHdl = this.pane.styleList.onChange;
		this.pane.styleList.onChange = undefined;
		
		fontPrefs.scintillaPrefs.xml.language = this.selection.text;
		this.pane.styleList.removeAll();
		this.pane.styleList.selection = null;
		if( this.selection != null )
		{											
			// get the language information						
			var id = this.selection.id;						
			var oneLang = fontPrefs.scintillaPrefs.xml.languages[id];
			
			this.pane.styleList.selection = null;

			// set keywords.
			this.pane.kwdsList.removeAll();
			this.pane.kwdsList.selection = null;

			var groupLists = oneLang.keywords.@index;
			this.pane.kwdsList.removeAll();
			for( i = 0; i < groupLists.length(); ++i )
			{
				var itemText = localize ("$$$/ESToolkit/PreferencesDlg/Fonts/GroupNumber=Group %1", (i+1));
				item = this.pane.kwdsList.add ( "item", itemText );									
				item.id = groupLists[i];
				if( this.pane.kwdsList.selection == null ) 
					this.pane.kwdsList.selection = item;							
			}
			
			//	Create a list of styles for the selected language
			// get titles
			titles = oneLang.style.@title;
			var titleArray = [];
			for (var i = 0; i < titles.length(); i++)
				titleArray.push ( { index:i, text: titles [i].toString() } );
			function sortfn (a, b)
			{
				if (a.text < b.text)
					return -1;
				if (a.text > b.text)
					return 1;
				return 0;
			}
			titleArray.sort (sortfn);

			var selStyles = oneLang.selection.toString().split (',');
			var styleItem = [];
			this.pane.styleList.removeAll();
			for( i = 0; i < titleArray.length; ++i )	
			{
				item = this.pane.styleList.add ( "item", titleArray [i].text );
				item.styleIndex = titleArray [i].index;
				// select the last style(s)
				for (var j = 0; j < selStyles.length; j++)
				{
					if ( selStyles[j] == item.index )
						styleItem.push (item);
				}
			}
			
			if( styleItem != null )
			{
				this.pane.styleList.selection = styleItem;
			}
			else
			{
				// reset the other controls...
				pane.fontNameList.selection = null;
				pane.fontSizeList.selection = null;
				pane.foreList.selection = null;
				pane.backList.selection = null;
				pane.boldBox.value = false; 
				pane.italicBox.value = false; 
			}
		}
		
		// reset onChange handler of ListBox "Display Items"
		this.pane.styleList.onChange = tmpHdl;
		// reset state of CheckBox "All Display Items"
		this.pane.allBox.value = allState;
		this.pane.allBox.onClick();
	}

	// helper: get a style XML by index
	pane.getStyleByIndex = function (index)
	{	
		if (!this.langList.selection)
			return null;
		var langID = this.langList.selection.id;
		var oneLang = fontPrefs.scintillaPrefs.xml.languages[langID];
		return oneLang.style [index];
	}

	// font name of style of language changes
	pane.fontNameList.onChange = function()
	{
		if( this.pane.styleList.selection != null && this.selection != null )
		{
			for (var sel = 0; sel < this.pane.styleList.selection.length; sel++)
			{
				var xml = this.pane.getStyleByIndex (this.pane.styleList.selection[sel].styleIndex);
				if (xml)
					xml.@name = this.selection.text;
			}
		}
	}

	// font name of style of language changes
	pane.fontSizeList.onChange = function()
	{
		if( this.pane.styleList.selection != null && this.selection != null )
		{
			for (var sel = 0; sel < this.pane.styleList.selection.length; sel++)
			{
				var itemText = this.selection.text;
				if( itemText[0] == ' ' )
					itemText = itemText[1];
				var langID = this.pane.langList.selection.id;
				var xml = this.pane.getStyleByIndex (this.pane.styleList.selection[sel].styleIndex);
				if (xml)
					xml.@size = itemText;
			}
		}
	}

	// foreground color of style of language changes
	pane.foreList.onChange = function()
	{
		if( this.pane.styleList.selection != null && this.selection != null)
		{
			for (var sel = 0; sel < this.pane.styleList.selection.length; sel++)
			{
				var xml = this.pane.getStyleByIndex (this.pane.styleList.selection[sel].styleIndex);
				if (xml)
					xml.@fore = this.selection.text.substr (1);
			}
		}
	}

	// background color of style of language changes
	pane.backList.onChange = function()
	{
		if( this.pane.styleList.selection != null && this.selection != null )
		{
			for (var sel = 0; sel < this.pane.styleList.selection.length; sel++)
			{
				var xml = this.pane.getStyleByIndex (this.pane.styleList.selection[sel].styleIndex);
				if (xml)
					xml.@back = this.selection.text.substr (1);
			}
		}
	}

	// bold of style of language changes;
	pane.boldBox.onClick = function()
	{
		if( this.pane.styleList.selection != null )
		{
			var value = this.value ? "true" : "false";
			for (var sel = 0; sel < this.pane.styleList.selection.length; sel++)
			{
				var xml = this.pane.getStyleByIndex (this.pane.styleList.selection[sel].styleIndex);
				if (xml)
					xml.@bold = value;
			}
		}
	}

	// italic of style of language changes
	pane.italicBox.onClick = function()
	{
		if( this.pane.styleList.selection != null )
		{
			var value = this.value ? "true" : "false";
			for (var sel = 0; sel < this.pane.styleList.selection.length; sel++)
			{
				var xml = this.pane.getStyleByIndex (this.pane.styleList.selection[sel].styleIndex);
				if (xml)
					xml.@italics = value;
			}
		}
	}
	
	// keywords group of style of language changes
	pane.kwdsList.onChange = function()
	{
		this.pane.kwdsEdit.onChange();
	}

	// keywords of style of language changes
	var oldLangID = "";
	var oldGroup = "";
	pane.kwdsEdit.onChange = function()
	{
		// apply to the old language and group
		if( oldLangID != "" )
		{
			fontPrefs.scintillaPrefs.xml.languages[oldLangID].keywords[oldGroup].setChildren( this.text );
		}
	
		if( this.pane.kwdsList.selection == null 
		 || this.pane.kwdsList.selection == null )
		{
			oldLangID = "";
			oldGroup = "";
			this.text = "";
		}
		else
		{
			// set the text from the new language and group
			var langID = this.pane.langList.selection.id;
			var currentGroup = this.pane.kwdsList.selection.index;
			this.text = fontPrefs.scintillaPrefs.xml.languages[langID].keywords[currentGroup].toString();
			
			// save the old language and group
			oldLangID = langID;
			oldGroup = currentGroup;
		}
	}
	
	// styles changes, updates fonts and colors
	// If multiselect, deselect anything that differs between styles
	pane.styleList.onChange = function()
	{
		if( this.selection != null && this.pane.langList.selection != null )
		{
			// Need to collect font information; if there are multiple values,
			// do not select anything
			// Start with the first style, and set differing styles to NOT_SET (which is an item never found)
			var NOT_SET = "***";
			var xml = this.pane.getStyleByIndex (this.selection[0].styleIndex);
			
			if( xml )
			{
			    var values = lang.getStyleValues( xml );
			    var fontName = values.name;
			    var fontSize = values.size;
			    var fontBold = values.bold;
			    var fontItal = values.italics;
			    var fontFore = values.foreName;
			    var fontBack = values.backName;
			    // This array collects style indexes
			    var selStyles = [];
			    // Start off again at 0
			    for (var sel = 0; sel < this.selection.length; sel++)
			    {
				    var currentStyle = this.selection[sel].index;
				    selStyles.push (currentStyle);
				    var current = this.pane.getStyleByIndex (this.selection[sel].styleIndex);
				    values = lang.getStyleValues( current );
				    // Update XML
				    if( current.@name == "" )
					    current.@name = values.name;
				    if( current.@size == "" )
					    current.@size = values.size;
				    if( current.@fore == "" )
					    current.@fore = values.foreName;
				    if( current.@back == "" )
					    current.@back = values.backName;
				    if( current.@bold == "" && values.bold)
					    current.@bold = values.bold;
				    if( current.@italics == "" && values.italics)
					    current.@italics = values.italics;
				    // Check for the same values
				    if (values.name != fontName)
					    fontName = NOT_SET;
				    if (values.size != fontSize)
					    fontSize = NOT_SET;
				    if (values.bold != fontBold)
					    fontBold = NOT_SET;
				    if (values.italics != fontItal)
					    fontItal = NOT_SET;
				    if (values.backName != fontBack)
					    fontBack = NOT_SET;
				    if (values.foreName != fontFore)
					    fontFore = NOT_SET;
			    }
			    // Remember this with the XML
			    var langID = this.pane.langList.selection.id;
			    fontPrefs.scintillaPrefs.xml.languages [langID].selection = selStyles.toString();
			    // set font name
			    var item = this.pane.fontNameList.find( fontName );
			    this.pane.fontNameList.selection = item;
			    // set font size
			    item = this.pane.fontSizeList.find( fontSize );
			    this.pane.fontSizeList.selection = item;

			    // set bold 
			    this.pane.boldBox.value = (fontBold == true);
			    // set italic 
			    this.pane.italicBox.value = (fontItal == true);

			    // set foreground color
			    var sel = null;
			    var items = this.pane.foreList.items;
			    for( i = 0; i < items.length; i++ )
			    {
				    if( items[i].text == " " + fontFore )
				    {
					    sel = items[i];
					    break;
				    }
			    }
			    this.pane.foreList.selection = sel;
			    // set background color
			    sel = null;
			    items = this.pane.backList.items;
			    for( i = 0; i < items.length; i++ )
			    {
				    if( items[i].text == " " + fontBack )
				    {
					    sel = items[i];
					    break;
				    }
			    }						
			    this.pane.backList.selection = sel;
			}
		}
	}

    pane.allBox.onClick = function()
    {
        if( this.value )
        {
            this.oldSel = this.parent.styles.selection ? this.parent.styles.selection : 0;
            
            //
            // select all items and disable listbox
            //
            this.parent.styles.enabled = false;
            
            var selItems = [];
            
            for( var i=0; i<this.parent.styles.items.length; i++ )
                selItems.push(i);
                
            this.parent.styles.selection = selItems;
        }
        else
        {
            //
            // select first item of listbox and enable the listbox
            //
            this.parent.styles.enabled   = true;
            this.parent.styles.selection = null;
            
            this.parent.styles.selection = ( this.oldSel && this.oldSel.length == 1 ) ? this.oldSel : 0;
        }
    }
    
	// select language
	if( pane.langList.selection == null )
	{
		item = pane.langList.find( fontPrefs.scintillaPrefs.xml.language );
		if( item == null )
			item = pane.langList.find( "JavaScript" );
		if( item !=	null )
		{
			// select item
			fontPrefs.scintillaPrefs.xml.language = "";
			pane.langList.selection = item;
		}
	}
	app.setWaitCursor (false);
}

fontPrefs.setupFonts = function()
{
	app.setWaitCursor (true);
	var currentFont = this.pane.fontNameList.selection ? this.pane.fontNameList.selection.text : null;
	var currentItem = null;
	//	Create a list of text font names
	var fontNames = app.getInstalledFonts();
	fontNames.sort();
	this.pane.fontNameList.removeAll();
	for( var i in fontNames )
	{
		var item = this.pane.fontNameList.add ( "item", fontNames[i]);
		if (fontNames[i] == currentFont)
			currentItem = item;
	}
	if (currentItem)
		this.pane.fontNameList.selection = currentItem;
	app.setWaitCursor (false);
}

/////////////////////////////////////////////////////////////////////////
// store preferences from the selected font options
fontPrefs.store = function()
{
	if (!this.loaded)
		return false;
	with (this.pane) 
	{
		if (language.grp.list.selection != null)
			fontPrefs.scintillaPrefs.xml.language = language.grp.list.selection.text;
	}
		
	languages = this.scintillaPrefs.xml.languages.copy();
	prefs.syntaxdefs.languages.setXMLNode( languages.copy() );
	prefs.preferencesDialogPrefs.fontPrefs.allDisplayItems = this.pane.allBox.value;
	
	globalBroadcaster.notifyClients( 'syntaxPrefsChanged' );
	
	return false;
}

fontPrefs.cancelled = function()
{
    fontPrefs.scintillaPrefs.xml = fontPrefs.scintillaPrefs.backup;
}
