/**************************************************************************
*
*  @@@BUILDINFO@@@ 35omvUI-2.jsx 3.5.0.47	09-December-2009
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

// Object Model Viewer.
// Current limitations:
// 1) The first TOC level is ignored.
// 2) Namespace and interface declarations are not interpreted.
// 3) Only the first <package> is interpreted.

OMV.debug = false;			// set to true to log callbacks to Console

// The delimiter used to separate bookmark entries in the prefs
// Changing this value will result in a situation where current
// bookmarks fail to load and display properly
var OMV_BKMK_DELIM = "#*"

// Create the OMV viewer.
// Properties:
// w				the window
// display			the swf
// currentOMVHref	the href of the current OMV
// omvItemsByHref	an object containing all OMV list items, with the href as property name
// stdClassHrefs	an object containing standard class names as name, and their hrefs as value
// omvClassHrefs	an object containing the current class names as name, and their hrefs as value

function OMV( silent )
{
	this.currentOMVHref = "";
	this.omvItemsByHref = {};
	this.omvClassHrefs = {};
	this.stdClassHrefs = {
		Object			:		"$COMMON/javascript#/Object",
		Array			:		"$COMMON/javascript#/Array",
		Math			:		"$COMMON/javascript#/Math",
		Date			:		"$COMMON/javascript#/Date",
		Function		:		"$COMMON/javascript#/Function",
		String			:		"$COMMON/javascript#/String",
		Number			:		"$COMMON/javascript#/Number",
		Boolean			:		"$COMMON/javascript#/Boolean",
		RegExp			:		"$COMMON/javascript#/RegExp",
		Error			:		"$COMMON/javascript#/Error",
		File			:		"$COMMON/javascript#/File",
		Folder			:		"$COMMON/javascript#/Folder",
		Socket			:		"$COMMON/javascript#/Socket",
		ReflectionInfo	:		"$COMMON/javascript#/ReflectionInfo",
		Reflection		:		"$COMMON/javascript#/Reflection",
		Dictionary		:		"$COMMON/javascript#/Dictionary",
		QName			:		"$COMMON/javascript#/QName",
		Namespace		:		"$COMMON/javascript#/Namespace",
		XML				:		"$COMMON/javascript#/XML",
		UnitValue		:		"$COMMON/javascript#/UnitValue",

		ScriptUI		:		"$COMMON/scriptui#/ScriptUI",
		Window			:		"$COMMON/scriptui#/Window",
		LayoutManager	:		"$COMMON/scriptui#/LayoutManager",
		ScriptUIGraphics:		"$COMMON/scriptui#/ScriptUIGraphics",
		ScriptUIPen		:		"$COMMON/scriptui#/ScriptUIPen",
		ScriptUIBrush	:		"$COMMON/scriptui#/ScriptUIBrush",
		ScriptUIPath	:		"$COMMON/scriptui#/ScriptUIPath",
		ScriptUIFont	:		"$COMMON/scriptui#/ScriptUIFont",
		ScriptUIImage	:		"$COMMON/scriptui#/ScriptUIImage",
		DrawState		:		"$COMMON/scriptui#/DrawState",
		StaticText		:		"$COMMON/scriptui#/StaticText",
		Button			:		"$COMMON/scriptui#/Button",
		IconButton		:		"$COMMON/scriptui#/IconButton",
		EditText		:		"$COMMON/scriptui#/EditText",
		ListBox			:		"$COMMON/scriptui#/ListBox",
		DropDownList	:		"$COMMON/scriptui#/DropDownList",
		ListItem		:		"$COMMON/scriptui#/ListItem",
		Checkbox		:		"$COMMON/scriptui#/Checkbox",
		Scrollbar		:		"$COMMON/scriptui#/Scrollbar",
		RadioButton		:		"$COMMON/scriptui#/RadioButton",
		Slider			:		"$COMMON/scriptui#/Slider",
		Progressbar		:		"$COMMON/scriptui#/Progressbar",
		TreeView		:		"$COMMON/scriptui#/TreeView",
		FlashPlayer		:		"$COMMON/scriptui#/FlashPlayer",
		Group			:		"$COMMON/scriptui#/Group",
		Panel			:		"$COMMON/scriptui#/Panel",
		Point			:		"$COMMON/scriptui#/Point",
		Dimension		:		"$COMMON/scriptui#/Dimension",
		Bounds			:		"$COMMON/scriptui#/Bounds",
		UIEvent			:		"$COMMON/scriptui#/UIEvent",
		MenuElement		:		"$COMMON/scriptui#/MenuElement"
	};

	if( OMV.winType == 'docked' )
	{
		this.w = OMV.palette;
	}
	else
	{
		var independent = ( OMV.winType == 'floating' ? 'true' : 'false' );

		this.w = new Window (
			"""prefwindow {											        
				text			: '$$$/ESToolkit/OMV/Name=Object Model Viewer', 
				preferredSize   : [550, 510],									
				orientation     : 'column',										
				margins         : [0, 0, 0, 0],                                            
				spacing         : 2,                                            
				properties      :                                               
				{
    				name			: '__omv__',
  					resizeable		: true,
					minimumSize     : [550, 350],								
					independent		: """ + independent + """
				},																 		
				display			: FlashPlayer {								
					minimumSize		: [550, 350],							
					alignment		: ['fill', 'fill' ]					
				},
				progressGroup	: Group
				{
					alignment	: ['fill','bottom'],
					margins		: 2,
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
                    sizeBoxSpacer    : Group {
                        size        : [20,1]
                    }
				}
			}""");

		this.w.content = this.w;
	}

	this.display = this.w.content.display;
	this.w.display = this.display;
	this.progress = this.w.content.progressGroup;
	this.display.omv = this;

	this.w.onNotify = function( reason )
	{
		switch( reason )
		{
			case 'shutdown':
			{
				globalBroadcaster.unregisterClient( this );

				var bookmarks = this.content.display.invokePlayerFunction ("getBookmarks");
				var searchDesc = this.content.display.invokePlayerFunction ("getPrefSearchDesc");
				
				prefs.omv.bookmarks = bookmarks.join(OMV_BKMK_DELIM).toString();
				prefs.omv.searchDesc = searchDesc;

				try
				{
					var omvPrefs = this.display.invokePlayerFunction( "storePrefs" );
					
					if( omvPrefs )
					{
						omvPrefs = omvPrefs.join( "§" );
						prefs.omv.UI = omvPrefs;
					}
				}
				catch( exc )
				{
					app.writeLog( "Didn't receive prefs from FP!" );
				}

				if( OMV.winType != prefs.omv.win.getValue( Preference.STRING ) && this.visible)
					this.hide();
			}
			break;
		}
	}

	this.w.onClose = function()
	{
	    var bookmarks = this.content.display.invokePlayerFunction ("getBookmarks");
	    var searchDesc = this.content.display.invokePlayerFunction ("getPrefSearchDesc");
	    
	    prefs.omv.bookmarks = bookmarks.join(OMV_BKMK_DELIM).toString();
	    prefs.omv.searchDesc = searchDesc;
		this.hide();
		workspace.setActiveDocumentFocus();	// set keyboard focus on current active document
		return false;
	}
	
	this.w.onShow = function()
	{
	    /*  The flashplayer size should be equal to the size (not frame size) 
	        of the window.
	        
	        A prefwindow might change its size/position prior to showing.
	        In that case, we have to resize the flashplayer to fit the window.
	    */
		if( OMV.winType != 'docked' )
		{
			var winWidth = this.size.width;//this.bounds.right - this.bounds.left;
			var fpWidth = this.content.display.bounds.right - this.content.display.bounds.left;
		    
			var winHeight = this.size.height;//this.bounds.top - this.bounds.bottom;
			var fpHeight = this.content.display.bounds.top - this.content.display.bounds.bottom;
		    
			if (winWidth != fpWidth || winHeight != fpHeight)
				this.layout.resize();
		}

	    // Give focus to the fp so that scroll wheel works without clicking
	    this.content.display.active = true;
	}

	globalBroadcaster.registerClient( this.w, 'shutdown,workspaceChanged' );
	
	this.w.onResize = function ()
	{
		this.layout.resize();
	}

	this._setupHTMLControl (this.w.content.display);

	// LAYOUT BUG: need to show now already
	if( !silent )
		this.w.show();

	this._startHTML();

	// Finally, register this UI as the only UI
	OMV.ui = this;
}

/// RCS_Remove
// This is the default font size of the OMV HTML viewer.

OMV.htmlFontSize = 11;

// The OMV icons are here, addressed by their names.

OMV.icons = {};

// The list of OMV that need to update their HTML.
// HTML is updated delayed.

OMV.update = [];

// Set up all OMV entries in the Help menu.

OMV.setupUI = function()
{
	if( !this.palette )
	{
		this.palette = ScriptUI.workspace.add( 'tab', 
										 localize('$$$/ESToolkit/OMV/Name=Object Model Viewer'), 
										 { name        : '__omv__' } );
		this.palette.margins = 0;
		this.palette.spacing = 0;
		this.palette.content = this.palette.add( """group {											        
				alignment		: ['fill', 'fill' ],
				orientation     : 'column',										
				margins         : 2,                                            
				spacing         : 2,                                            
				properties      :                                               
				{
					minimumSize     : [550, 350],                               
				},																 		
				display			: FlashPlayer {								
					minimumSize		: [550, 350],							
					alignment		: ['fill', 'fill' ]					
				},
				progressGroup	: Group
				{
					alignment	: ['fill','bottom'],
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
					},
                    sizeBoxSpacer    : Group {
                        size        : [20,1]
                    }
				}
			}""" );

		this.palette.minimumSize = [590, 430];
		this.palette.show();

		globalBroadcaster.registerClient( this.palette, 'workspaceChanged' );

		this.palette.onNotify = function( reason )
		{
			switch( reason )
			{
				case 'workspaceChanged':
				{
					if( OMV.winType != 'docked' && this.visible)
						this.hide();
				}
				break;
			}
		}
	}
}

OMV.initializeUI = function()
{
	if( OMV.winType == 'docked' )
	{
		if( !OMV.ui )
			OMV.ui = new OMV( true );
		
		if( OMV.palette.visible && !OMV.palette.minimized )
		{
			OMV.palette.layout.resize();
			OMV.ui.show(true);
		}
	}
}

OMV.setup = function()
{
	this.winType = prefs.omv.win.getValue( Preference.STRING );

	// Set up the OMVData
	OMVData.setup();

	// Set up the icons
	this.icons.ENUMERATION	= ScriptUI.newImage ("#Enumeration");
	this.icons.CLASS		= ScriptUI.newImage ("#Class");
	this.icons.METHOD		= ScriptUI.newImage ("#Method");
	this.icons.ROPROPERTY	= ScriptUI.newImage ("#PropertyRO");
	this.icons.RWPROPERTY	= ScriptUI.newImage ("#PropertyRW");
}

//////////////////////////////////////////////////////////////////
//
//	Public Interface
//
//////////////////////////////////////////////////////////////////
//
//	Call APIs
//	The APIs below need to be re-implemented in the UI.
//
//////////////////////////////////////////////////////////////////

/*	show()
Display the UI.
*/

OMV.prototype.show = function( dontExpand )
{
	if( document && document.isSourceDocument )
		addDelayedTask (this, this.select, document.acHelper.getHref());

	if( !dontExpand )
	{
		this.w.minimized = false;
	}

	this.w.show();
}

/* activate()
Bring OMV window to the front
*/
OMV.prototype.activate = function()
{
	if( !this.w.minimized )
	{
		if( this.w.active )
			this.w.active = false;

		this.w.active = true;
	}
}

/*	setContent (xml, cookie)

Set the content of either the list of OMVs, the list of classes, or a class
definition. The XML is one othe ones listed below. The cookie is either the
empty string, or a href that the UI has passed in to this code via the
cbGetContent() callback.

1. Load all OMVs. This XML is passed in shortly after the UI has been
   created. It is only passed in once.
	<omvs>
		<omv title="display name" href="href"/>
		...
	</omvs>

2. Load all classes out of the supplied <map> OMV element. Should blank the 
   types and properties elements, and clear the HTML display. Use the hrefs
   attached to each <topicref> entry to call cbGetContent().
	<map>
		<topicref...
	</map>

3. Load a class definition. This call should fill in the Types list box, select
   the Instance entry (or the Class entry if there are no Instance elements),
   list the corresponding elements, and clear the HTML display.
	<classdef>
		...
	</classdef>
*/
var _inSetContent_ = false;

OMV.prototype.setContent = function (xml)
{
	if( !_inSetContent_ )
	{
		_inSetContent_ = true;
		
		if (OMV.debug)
		{
			var s = xml.toXMLString();
			var idx = s.indexOf ('\n');
			if (idx >= 0)
				s = s.substr (0, idx) + "...";
			print ("OMV.setContent (" + s + ")");
		}
		
		this.display.invokePlayerFunction ("setContent", xml);
		
		switch (xml.localName())
		{
			case "map":
				this._loadTOC (xml);
			break;
			
			case "omvs":
				this._loadOMVs (xml);
			break;
		}

		_inSetContent_ = false;
	}
	else
		addDelayedTask (this, this.setContent, xml);
}

/*  setBookmarks (bookmarks)
    Restores the list of saved bookmarks
*/

OMV.prototype.setBookmarks = function (bkmrks)
{
	if (OMV.debug)
		print ("OMV.setBookmarks (" + bkmrks + ")");
    var bookmarks = bkmrks.getValue(Preference.STRING).split(OMV_BKMK_DELIM);
    
    this.display.invokePlayerFunction ("setBookmarks", bookmarks);
}

/*  setPrefSearchDesc (searchDesc)
    Sets the preference to search descriptions
*/

OMV.prototype.setPrefSearchDesc = function (searchDesc)
{
	if (OMV.debug)
		print ("OMV.setPrefSearchDesc (" + searchDesc + ")");
		
    var val = searchDesc.getValue(Preference.BOOLEAN);
    this.display.invokePlayerFunction ("setPrefSearchDesc", val);
}

/*	select (href)

Select an element. The href may be partial, meaning that certain elements are just missing. 
These are the allowed formats:
1. Select an OMV: omvname#
2. Select a class: omvname#/classname
3. Select a class elements container: omvname#/classname/container
4. Select a class element: omvname#/classname/container/elementname or omvname#/classname/elementname
All other hrefs formats are invalid. The UI may choose to pre-select defaults if a class, 
type, or element is missing from the href. The UI should call the cbGetContent() callback
to fill in subsequent elements.
Returns false if the hrefs could not be selected.
*/

OMV.prototype.select = function (href)
{
	if (OMV.debug)
		print ("OMV.select (" + href + ")");

	return this.display.invokePlayerFunction ("select", href);
}

//////////////////////////////////////////////////////////////////
//
//	Callback APIs
//	The APIs below are fully implemented here and do not need to
//	be changed.
//
//////////////////////////////////////////////////////////////////

/*	cbInitDone (href)

CALLBACK: Called after the Flex interface has completed
its initialization and the ExtendScript host can call 
its methods.
*/

OMV.prototype.cbInitComplete = function ()
{
	//
	// restore
	//
	var omvPrefs = prefs.omv.UI.getValue( Preference.STRING );
	omvPrefs = omvPrefs.split( "§" );
	this.display.invokePlayerFunction( "restorePrefs", omvPrefs );
	
	if (OMV.debug)
		print ("OMV.cbInitComplete()");
	
	OMVData.initUi();
}

/*	cbGetContent (href)

CALLBACK: Request the content for the given href. The href is either an OMV 
href of the form omv#, or a class definition href of the form [omv]#/classname.
Returns true if successful, false otherwise.
*/

OMV.prototype.cbGetContent = function (href)
{
	if (OMV.debug)
		print ("OMV.cbGetContent (" + href + ")");
	var parts = href.split ('#');
	var curHref;
	if (parts.length != 2)
		return;
	if (parts [0].length > 0)
	{
		// OMV part!
		curHref = parts [0] + '#';
		if (this.currentOMVHref != curHref)
		{
			// OMV switch or fill needed!
			var data = OMVData.find (curHref);
			if (!data)
				return false;
			this.currentOMVHref = curHref;
			// load the TOC for this OMV
			this.setContent (data.xml.map);
		}
	}

	// Get the current OMV
	data = OMVData.find (this.currentOMVHref);
	if (!data)
		return false;
 
    //  href was an OMV href, we're done
    if (parts[1].length == 0)
        return true;
 
	// the remainder is a class
	var xml = data.getXML (parts [1]);
	if (!xml)
		return false;
	
	this.setContent (xml);
	return true;
}

/*	cbGetOMVFolder (href)

CALLBACK: Return the folder for the given OMV href. The UI uses this callback to 
determine the location of the OMV file. The location is needed to determine the 
relative location of sample code files.
*/

OMV.prototype.cbGetOMVFolder = function (href)
{
	if (OMV.debug)
		print ("OMV.cbGetOMVFolder (" + href + ")");
	var data = OMVData.find (href);
	if (data)
		return File (data.file).parent.fsName;
	else
		return null;
}

/*	cbFindText (href, text, wholeWord)

CALLBACK: Find the XML for this text. The return value is a list of XML found for
the text, or null. The UI uses this callback to invoke the Find Text functionality 
when the user types in a text to be found.
*/

OMV.prototype.cbFindText = function (omvHref, text, fullText)
{

 	if (OMV.debug)
		print ("OMV.cbFindText (" + omvHref + ', "' + text + '", ' + fullText + ")");
    
	if (omvHref)
	{
		var flags = OMVData.SEARCH_SUBSTRING;
		if (fullText)
			flags |= OMVData.SEARCH_FULLTEXT;
		
		/*  I need a wait to show that searching is in
		    progress, but the Flash viewer resets the cursor
		    right after I call the following....
		    
		    app.setWaitCursor (true);
		    
		    This would be after .find
		    app.setWaitCursor (false);
		*/
		var data = OMVData.find (omvHref);
		
		if (data)
			return data.findString (text, flags);
		else
			return null;
	}
}

//////////////////////////////////////////////////////////////////
//
//	Implementation APIs
//
//////////////////////////////////////////////////////////////////

// Load all OMVs. XML is a list of:
// <omv text="display name" href="href"/>

OMV.prototype._loadOMVs = function (xml)
{
	this.omvItemsByHref = {};

	// use the list of entries
	xml = xml.children();
	for (var i = 0; i < xml.length(); i++)
	{
		var entry  = xml [i];
		var item   = new Object();
		item.title = entry.@title;
		item.href  = entry.@href.toString();
		item.omv   = this;
		
		this.omvItemsByHref [item.href] = item;
	}
}

// Load the TOC into the Classes listbox. Returns false if aborted.

OMV.prototype._loadTOC = function (mapXML)
{
	// Ignore the top-level <topicref> tag
	mapXML = mapXML.topicref.children();
	var list = this._sortedXML (mapXML, "navtitle");
	this._setHTML (null);

	this.omvClassHrefs = {};

	if (this.dlg)
		this.dlg.setProgress (0, list.length);

	for (var i = 0; i < list.length; i++)
	{
		if (this.dlg && !this.dlg.setProgress (i))
		{
			return false;
		}
		var href = list[i].@href.toString();
		var clsName = list[i].@navtitle.toString();
		// add the data to the global tables
		this.omvClassHrefs [clsName] = href;
	}
	return true;
}

// Load the next-level class info (which is the elements group).
/// RCS_Remove
OMV.prototype._loadClassdef = function (xml)
{	
	return;
	var types = [ "constructor", "class", "instance", "prototype", "event" ];
	var typeNames = [ 
		"$$$/ESToolkit/OMV/Types/Constructors=Constructors", 
		"$$$/ESToolkit/OMV/Types/ClassElements=Class Elements", 
		"$$$/ESToolkit/OMV/Types/InstanceElements=Instance Elements",
		"$$$/ESToolkit/OMV/Types/PrototypeElements=Prototype Elements", 
		"$$$/ESToolkit/OMV/Types/Events=Events" ];

	for (var i = 0; i < types.length; i++)
	{
		var container = xml.elements.(@type == types [i]);
		if (container.length() > 0)
		{
			item = this.types.add ("item", localize (typeNames [i]));
			item.href = "#/" + xml.@name + "/" + types [i];
			item.xml = container;
			item.omv = this;
		}
	}

}

// Fill in the elements for a class. The xml is the <elements> container.
// This routine attempts to replace the elements and either append or truncate,
// depending on the initial length of the list box to minimize flicker.

OMV.prototype._loadClassElements = function (xml)
{
	var count = 0;

	var props = this._sortedXML (xml.property, "name");
	var meths = this._sortedXML (xml.method, "name");

	var hrefBase = "#/" + xml.parent().@name + "/" + xml.@type + "/";
	var isEnum = (xml.parent().@enumeration == "true");
	var name, type, item, i;
 
	for (i = 0; i < props.length; i++)
	{
		name = this._makeElementName (props [i]);
		if (count++ < this.elements.items.length)
		{
			item = this.elements.items [i];
			item.text = name;
		}
		else
			item = this.elements.add ("item", name);
		item.href = hrefBase + props[i].@name;
		item.xml = props [i];
		item.omv = this;
		if (props[i].@name == "activeDocument")
			item.yes = true;
		if (isEnum)
			item.icon = OMV.icons.ENUMERATION;
		else
			item.icon = (props[i].@rwaccess == "readonly") ? OMV.icons.ROPROPERTY : OMV.icons.RWPROPERTY;
	}
	for (i = 0; i < meths.length; i++)
	{
		name = this._makeElementName (meths [i]);
		if (count++ < this.elements.items.length)
		{
			item = this.elements.items [i];
			item.text = name;
		}
		else
			item = this.elements.add ("item", name);
		item.href = hrefBase + meths[i].@name;
		item.omv = this;
		item.icon = OMV.icons.METHOD;
		item.xml = meths [i];
	}
	// remove excess elements
	while (count < this.elements.items.length)
		this.elements.remove (this.elements.items.length-1);

	// Methods/properties change notification
	this.elements.onChange = function()
	{
		var item = this.selection;
		if (item)
		{
			var html = this.omv._getHTML (item.xml);
			this.omv._setHTML (html, item.href);
		}
	}
	// select the first element if the length is 1, deselect otherwise
	this.elements.selection = (count == 1) ? 0 : null;
}

// Create an element name. Properties get a colon and the data type,
// and methods get the parameter list and the return type. If a data
// type is linkable, makeTypeInfo() creates a link.

OMV.prototype._makeElementName = function (xml)
{
	var name = xml.@name;
	var type = this._makeTypeInfo (xml.datatype, false);
	if (xml.localName() != "method")
	{
		if (type == "")
			type = "any";
		name += ": " + type;
	}
	else
	{
		var args = "(";
		var params = xml.parameters.children();
		for (var j = 0; j < params.length(); j++)
		{
			if (j)
				args += ", ";
			args += params [j].@name;
		}
		args += ")";
		if (args.length > 2)
			name += " ";
		name += args;
		if (type != "")
			name += ": " + type;
	}
	return name;
}

//////////////////////////////////////////////////////////////////
//
//	HTML Generation
//
//////////////////////////////////////////////////////////////////

// Generate HTML for a given XML. This XML could be one or more
// class-level entries, or one or more element level entries.

OMV.prototype._getHTML = function (xml, parent, grandParent)
{
	var html = null;
	if (!xml)
		html = <html><body/></html>;
	else
	{
		if (xml.length() == 1)
		{
			switch (xml.localName())
			{
				case "classdef":
					html = this._getClassHTML (xml);
					break;
				case "method":
					html = this._getMethodHTML (xml, parent, grandParent);
					break;
				case "property":
					html = this._getPropertyHTML (xml, parent, grandParent);
					break;
			}
		}
		if (html == null)
		{
			html = <html><body/></html>;
			if (xml)
			{
				var br = <br/>;
				var sorted = this._sortedXML (xml, "name");

				for (var i = 0; i < sorted.length; i++)
				{
					if (sorted [i].localName() == "classdef")
						html.body.appendChild (this._makeLink (this._getHrefForXML (sorted [i]), sorted [i].@name.toString()));
					else
					{
						var name;
						if (sorted[i].parent().@type == "constructor")
							name = "new " + this._makeElementName (sorted [i]);
						else
						{
							name = sorted [i].parent().parent().@name + ".";
							// do not prefix the global elements
							if (name == "global.")
								name = "";
							name += this._makeElementName (sorted [i]);
						}
						html.body.appendChild (this._makeLink (this._getHrefForXML (sorted [i]), name));
					}
					html.body.appendChild (br);
				}
			}
		}
	}
	return html;
}

// Generate and return HTML for a class.

OMV.prototype._getClassHTML = function (xml)
{
	var html = <html><body/></html>;
	html.body.appendChild (<p><font size="+2"><b>{xml.@name}</b></font></p>);
	if (xml.superclass.length() > 0)
	{
		var p = <p/>;
		p.appendChild (<b>Base Class: </b>);
		var href = xml.superclass.@href.toString();
		if (href == "")
			href = this._getHrefForXML (xml.superclass);
		p.appendChild (this._makeLink (href, xml.superclass.toString()));
		html.body.appendChild (p);
	}
	this._appendOmvTitle (html.body, xml);
	this._appendHelpText (html.body, xml);
	this._appendExamples (html.body, xml);
	return html;
}

// Generate and return HTML for a property.

OMV.prototype._getPropertyHTML = function (propXML, parent, grandParent)
{
	var html = <html><body/></html>;
	var type = this._makeTypeInfo (propXML.datatype, true);
	if (type == "")
		type = "any";
	var rw = "";
	switch (propXML.@rwaccess.toString())
	{
		case "readonly":	rw = "(Read Only)"; break;
		case "writeonly":	rw = "(Write Only)"; break;
	}
	var name = grandParent.@name  + '.';
	if (name == "global.")
		name = "";
	html.body.appendChild (<p><font size="+2"><b>{name + propXML.@name}</b></font>&#160;{rw}</p>);
	html.body.appendChild (<p><b>Data Type: </b> {this._makeFullTypeInfo (propXML.datatype, true)}</p>);
	this._appendOmvTitle (html.body, propXML);
	this._appendHelpText (html.body, propXML);
	this._appendExamples (html.body, propXML);
	return html;
}

// Generate and return HTML for a method.

OMV.prototype._getMethodHTML = function (methXML, parent, grandParent)
{
	var html = <html><body/></html>;
	// is this a ctor?
	var isCtor = (parent.@type == "constructor");
	// create the bold method name
	var block = <b/>;
	var name = grandParent.@name  + '.';
	// remove the prefix "global", and the class name for constructors
	if (name == "global." || isCtor)
		name = "";
	block.appendChild (name + methXML.@name + " (");
	var params = methXML.parameters.children();
	for (var j = 0; j < params.length(); j++)
	{
		var text = (j > 0) ? ", " : "";
		text += params [j].@name + ":";
		block.appendChild (text);
		var typeXML = this._makeTypeInfo (params [j].datatype, true, true);
		if (typeXML.length() == 0)
			typeXML = "any";
		block.appendChild (typeXML);
	}
	block.appendChild (")");
	if (!isCtor)
	{
		// Only for non-constructors
		var type = this._makeTypeInfo (methXML.datatype, true);
		if (type.toString() != "")
		{
			block.appendChild (":");
			block.appendChild (type);
		}
	}
	block.normalize();
	var p = <p><font size="+2"/></p>;
	p.font.appendChild (block);
	html.body.appendChild (p);
	this._appendOmvTitle (html.body, methXML);
	this._appendHelpText (html.body, methXML);
	for (var j = 0; j < params.length(); j++)
	{
		var param = params [j];
		var type = this._makeFullTypeInfo (param.datatype, true);
		if (type == "")
			type = "any";
		var tempXML = '<p><b>' + param.@name;
		if (param.@optional == "true" || param.datatype.value != "")
			tempXML += " (optional)";
		tempXML += ': </b></p>';
		var p = XML (tempXML);
		p.appendChild ("Data Type: ");
		p.appendChild (type);
		html.body.appendChild (p);
		this._appendHelpText (html.body, param);
	}
	this._appendExamples (html.body, methXML);
	return html;
}

// Error message for bad HTML, or missing HTML display.

OMV.prototype._badHTML = function()
{
	errorBox( localize( '$$$/ESToolkit/Alerts/BadFlash=Cannot display HTML!\nHelp is not available.' ) );
}

// Sort the given XML list and return an Array obbject wth the sorted elements.
// The sort is case insensitive.

OMV.prototype._sortedXML = function (xml, attrname)
{
	var arr = [];
	for (var i = 0; i < xml.length(); i++)
	{
		var s = xml [i]['@' + attrname].toString().toUpperCase();
		arr.push (s + '\n' + i);
	}
	arr.sort();
	for (i = 0; i < arr.length; i++)
		arr [i] = xml [arr [i].split ('\n')[1]];
	return arr;
}

// Create a type info string out of a <datatype> element
// datatypeXML - the <datatype> element
// doXML       - return XML rather than a string
// onlyType    - return type only, no details

OMV.prototype._makeTypeInfo = function (datatypeXML, doXML, onlyType, includeDesc)
{
	// make sure that we process only the first element of a possible list
	if (datatypeXML.length() > 1)
		datatypeXML = datatypeXML [0];

	// we don't want localization here for now.
	var xml = <list/>;
	var displayType = datatypeXML.type.toString();
	var realType = displayType;
	// The data type may have the form displayname=realname
	displayType = displayType.split ('=');
	if (displayType.length == 2)
		realType = displayType [1];
	displayType = displayType [0];
	// small fixups
	if (displayType == "bool")
		displayType = "Boolean";
	if (realType == "bool")
		realType = "Boolean";
	if (displayType == "string")
	    realType = "String";
	if (displayType == "number")
	    realType = "Number";

	var elem = datatypeXML.array;
	var fmt;
	if (elem.length())
		fmt = (elem.@size == "")
			  ? "Array of %1"
			  : "Array [%2] of %1";
	else
		fmt = "%1";

	if (doXML)
	{
		// find (or generate) the href for this data type (if the data type is a class)
		// if there is a href, this will be a clickable link
		var href = datatypeXML.type.@href.toString();

		if (!href.length)
		{
			href = null;
			// check if the class is the own classdef - no href needed in that case
			for (var parent = datatypeXML.parent(); 
				 null != parent && parent.localName() != "classdef"; 
				 parent = parent.parent()) {}
			if (null == parent || parent.@name != realType)
			{
				// try to find a known class href
				href = this.stdClassHrefs [realType];
				if (!href)
					href = this.omvClassHrefs [realType];
			}
		}
		else
		{
			// validate the OMV portion of the href
			var temp = href.split ('#');
			if (temp.length == 1 || (temp[0].length && !this.omvItemsByHref [temp[0] + '#']))
				href = null;
		}

		// here is a validated href (for external hrefs, only the OMV portion is validated)
		if (href)
		{
			// Create a two-element array with the class name set to \n
			// so we can split the message at the class name
			var s = localize (fmt, "\n", elem.@size);
			s = s.split ("\n");
			// 1st part of the message
			xml.appendChild (s [0]);
			xml.appendChild (this._makeLink (href, displayType));
			xml.appendChild (s [1]);
			// show that we dont need to append anything
			fmt = null;
		}
	}
	if (fmt)
		xml.appendChild (localize (fmt, displayType, elem.@size));
	if (!onlyType)
	{
		elem = datatypeXML.minimum;
		if (elem.length())
			xml.appendChild (", " + localize ("Minimum: %1", elem.toString()));
		elem = datatypeXML.maximum;
		if (elem.length())
			xml.appendChild (", " + localize ("Maximum: %1", elem.toString()));
		elem = datatypeXML.value.toString();
		if (elem != "")
		{
			// If the type is Number, check for possible display as 4-byte const
			if (realType == "number")
			{
				var n = Number (elem);
				if (n >= 0x20202020)
				{
					var chars = "";
					for (var i = 0; i < 4; i++, n <<= 8)
					{
						var ch = String.fromCharCode ((n >> 24) & 0xFF);
						if (ch >= " " && ch <= "~")
							chars += ch;
					}
					if (chars.length == 4)
						elem += " ('" + chars + "')";
				}
			}
			fmt = (datatypeXML.parent().@rwaccess == "readonly")
				? "Value: %1"
				: "Default Value: %1";
			xml.appendChild (", " + localize (fmt, elem));
		}
	}

	// short descriptiuon if available
	if( includeDesc )
	{
		var text = datatypeXML.shortdesc;
		var p;
		if (text.length())
		{
			text = text.toString();
			p = new XML( "<i> (" + text + ")</i>" );
			this._expandLinks (p);
			xml.appendChild (p);
		}
	}

	xml = xml.children();
	xml.normalize();

	return doXML ? xml : xml.toString();
}

// Create type info XML out of a <datatype> element list
// If the data type is a list, createXML like "XXX or YYY"
// datatypeXML - the <datatype> element - maybe a list

OMV.prototype._makeFullTypeInfo = function (datatypeXML, includeDesc)
{
	// root element is ignored
	var s = <type/>;
	for (var i = 0; i < datatypeXML.length(); i++)
	{
		if (i > 0)
			s.appendChild ("or");
		s.appendChild (this._makeTypeInfo (datatypeXML [i], true, false, includeDesc));
	}
	return s;
}

// Append the title of the current OMV
OMV.prototype._appendOmvTitle = function (html, xml)
{
    if (!this.currentOMVHref)
        return;

    var data = OMVData.find (this.currentOMVHref);
    if (!data)
        return;
        
    var text = data.getDisplayName();
    
    if (text.length)
        html.appendChild(<p><i>{text}</i></p>);
}

// Append the short and/or the full description to given HTML.

OMV.prototype._appendHelpText = function (html, xml, usePara)
{
	var text = xml.shortdesc;
	var p;
	if (text.length())
	{
		p = text.copy();
		p.setName ("p");
		this._expandLinks (p);
		html.appendChild (p);
	}
	text = xml.description;
	if (text.length())
	{
		p = text.copy();
		p.setName ("p");
		this._expandLinks (p);
		html.appendChild (p);
	}
}

// Append any examples to the given HTML.

OMV.prototype._appendExamples = function (html, xml)
{
	var ex = xml.example;
	if (ex.length() > 0)
		html.appendChild (ex.length() == 1 ? <p><b>Example:</b></p> : <p><b>Examples:</b></p>);

	for (var i = 0; i < ex.length(); i++)
	{
		var p;
		var elem = ex [i];
		var href = elem.@href.toString();
		if (href && this.currentOMVHref != "")
		{
			var path = this.cbGetOMVFolder (this.currentOMVHref);
			if (!path)
				// Error handling?
				return;
			var f = new File (path);
			f.changePath (href);
			if (f.open())
			{
				p = <p><font face="_typewriter"/></p>;
				var body = p.font;
				while (!f.eof)
				{
					var text = f.readln();
					// Change leading whitespace to the non-breaking variant
					var text2 = "";
					var nbsp = String.fromCharCode (160);
					for (var ch = 0; ch < text.length; ch++)
					{
						switch (text [ch])
						{
							case "\t":	text2 += nbsp + nbsp + nbsp + nbsp; break;
							case  ' ':	text2 += nbsp; break;
							default:	text2 += text.substr (ch); text = "";
						}
					}
					p.font.appendChild (text2);
					p.font.appendChild (<br/>);
				}
				f.close();
				html.appendChild (p);
			}
		}
		else
		{
			html.appendChild (this._expandLinks (elem.children().copy()));
//			this._expandLinks (html);
		}
	}
}

// Extend all <a> elements in the given HTML to
// <a href="event:xxxx"><font color="#0000FF">link text</font></a>

OMV.prototype._expandLinks = function (html)
{
	if (html.localName() == "a")
	{
		var href = html.@href.toString();
		// Do not insert "event:" if the Href starts with http: or file:
		if (href.indexOf ("http:") < 0 && href.indexOf ("file:") < 0)
			href = "event:" + href;
		return <a href={href}><font color="#0000FF">{html.toString()}</font></a>;
	}
	else if (html.nodeKind() == "element")
	{
		var kids = html.children();
		var newKids = new XMLList();
		for (var i = 0; i < kids.length(); i++)
			newKids += this._expandLinks (kids [i]);
		html.setChildren (newKids);
	}
	return html;
}

// Create a HREF attribute for the given href.
// This href has the form [file]#/Classname[/Containertype/Elementname]
// to be used as argument to findText(). 
// The first argument is either XML, or the raw link text.
// The return value of the complete <a> tag.

OMV.prototype._makeLink = function (href, text)
{
	if (href=="")
		// no HREF? Then no link
		return XML (text);

	if (!text)
	{
		var part1 = href.split ('#');
		var part2 = part1 [part1.length-1].split ('/');
		if (part2.length >= 3)
		{
			text = part2[2];
			if (part2.length == 4)
				text += '.' + part2[3];
		}
	}
	href = "event:" + href;
	return <a href={href}><font color="#0000FF">{text}</font></a>;
}

//////////////////////////////////////////////////////////////////
//
//	Searching
//
//////////////////////////////////////////////////////////////////

// Find a string. If the string contains a dot, split into class name and 
// element name. If words is true, the search must be accurate (whole words).

OMV.prototype._findText = function (what, wholeWord)
{
	if (this.currentOMVHref != "")
	{
		var data = OMVData.find (this.currentOMVHref);
		if (!data)
			return;
		var xml = data.findXML (what, wholeWord);
		if (!xml)
			return;

		// Display the found XML and select the appropriate lists.
		// If the XML is multiple elements, select all elements and
		// create a list of clickable elements in the HTML display.
		// The XML may be a list of classes or a list of elements in 
		// a single container, but not both classes and elements, or
		// multiple element containers.
		this._selectXML (xml);
	}
}

// Helper: find a list element that has the given XML appended.

OMV.prototype._getListItemForXML = function (parent, xml)
{
	var items = parent.items;
	for (var i = 0; i < items.length; i++)
	{
		var item = items [i];
		if (item.xml == xml)
			return item;
	}
	return null;
}

// Select the given XML in the appropriate lists.
// If the XML is multiple elements, select no elements.
// The XML may be a list of classes or a list of elements in 
// a single container, but not both classes and elements, or
// multiple element containers.

OMV.prototype._selectXML = function (xml)
{
	if (xml)
	{
		var isClass = (xml.localName() == "classdef");

		if (xml.length() > 1)
		{
			// multiple results: deselect classes, if classdef
			if (isClass)
				this.classes.selection = null;
			else
				this.types.selection = null;
		}
		else
		{
			// select types
			var containerxml, elementxml;

			if (isClass)
			{
				this._loadClass (xml);
				// select a container
				containerxml = xml.elements;
				if (containerxml.length() != 1)
				{
					// if there are multiple, start with instance elements if possible
					containerxml = xml.elements.(@type == "instance");
					if (containerxml.length() == 0)
						containerxml = xml.elements [0];
				}
			}
			else
			{
				// element XML (can be multiple!)
				containerxml = xml.parent();
				if (containerxml.length() > 1)
				{
					// need to deselect all classes if multiple classes
					if (containerxml.parent().length() > 1)
						this.classes.selection = null;
					else 
					{
						if (containerxml.length() > 1)
							this.types.selection = null;
						else
							this._loadClass (containerxml.parent());
					}
				}
			}

			// load the container if there is only one
			if (containerxml.length() == 1)
			{
				item = this._getListItemForXML (this.types, containerxml);
				if (item)
				{
					this.types.selection = item;
					this._loadClassElements (containerxml);
					// select the element if there is only one
					if (!isClass && xml.length() == 1)
						this.elements.selection = this._getListItemForXML (this.types, containerxml);
				}
				else
				{
					this.types.selection = null;
					this.elements.xml = null;
					this.elements.onChange = null;
					this.elements.removeAll();
				}
			}
		}
		// Finally, check if there is more than one XML element.
		// If not, the HTML control needs to display a list of entries.
		if (xml.length() > 1)
			this._setHTML (this._getHTML (xml));
	}
	else
		this._setHTML (null);
}

OMV.prototype._getHrefForXML = function (xml)
{
	var href = "";
	switch (xml.localName())
	{
		case "method":
		case "property":
			// try to find a known class href
			href = this.stdClassHrefs [xml.parent().parent().@name];
			if (!href)
				href = this.omvClassHrefs [xml.parent().parent().@name];
			if (!href)
				href = "#/" + xml.parent().parent().@name;
			href += "/" + xml.parent().@type
				  + "/" + xml.@name;
			break;
		case "superclass":
			// try to find a known class href for <superclass>Classname</superclass>
			href = this.stdClassHrefs [xml];
			if (!href)
				href = this.omvClassHrefs [xml];
			if (!href)
				href = "#/" + xml;
			break;
		default:
			href = "#/" + xml.@name;
			break;
	}
	return href;
}

//////////////////////////////////////////////////////////////////
//
//	HTML Control
//
//////////////////////////////////////////////////////////////////

OMV.prototype._setupHTMLControl = function (ctrl)
{
	/// This is the HTML page stack.
	this.htmlStack = [];
	this.htmlPos   = -1;

	/// Register callbacks with flashplayer
	ctrl.localize = function()
	{
	    return localize.apply ($.global, arguments);
	}
	
	ctrl.isLocaleja_JP = function()
	{
	    // The FP needs to change its font if the locale is ja_JP
	    var locale = "";
	    
	    if (prefs.locale.hasValue())
	        locale = prefs.locale.getValue(Preference.STRING);
	    else
	        locale = $.locale;

	    return locale == "ja_JP";
	}
	
	ctrl.initComplete = function () 
	{
		this.omv.cbInitComplete();
	}

	ctrl.findText = function (string, href, fullText)
	{
		var xml = this.omv.cbFindText (href, string, fullText);
		return xml.toXMLString();
	}	

	ctrl.getContent = function (href)
	{
		return this.omv.cbGetContent (href);
	}

	ctrl.getOMVFolder = function (href)
	{
		this.omv.cbGetOMVFolder (href);
	}
	
	ctrl.getHtml = function (xml, parent, grandParent)
	{
		return this.omv._getHTML (XML(xml), XML(parent), XML(grandParent));
	}

	/*	This function is called from the htmlViewer control via ExternalInterface::call
		to get the page content for the given 'link' */
	ctrl.getContentForLink = function (theCookie, href)
	{
		var html;
		// The href format is [omv]#/classname[/containertype/elementname].
		// select that item - may be a class or a class element
		
        // print ("getContentForLink("+href+")");
		if (this.omv.select (href))
		{
			var item = this.omv.elements.selection;
			if (!item)
				item = this.omv.classes.selection;
			if (item)
			{
				// add the href to the HTML stack
				this.omv._pushHTMLStack (item.href);
				html = this.omv._getHTML (item.xml);
			}
		}
		if (!html)
		{
			html = <html><body><b>Broken link:&#160;<font color = "#FF0000">{href}</font></b></body></html>;
			html = this.omv._prepareHTML (html);
		}
		return html;
	}
}

// Start the HTML "movie"

OMV.prototype._startHTML = function()
{
	var movieToPlay = new File (app.requiredFolder + "/more/ESTK_HTML.swf");
	if (!movieToPlay.exists)
		this._badHTML();
	else 
	{
		try 
		{
			this.display.loadMovie (movieToPlay);
		}
		catch (e) 
		{
			this._badHTML();
		}
	}
}

// Prepare the given HTML (which is complete) for the HTML widget.
// The returned value is the HTML as string.

OMV.prototype._prepareHTML = function (html)
{
	if (!html)
		html = this.display.html;
	if (!html)
		return this.display.html = <html><body/></html>;

	html = html.removeNamespace ("http://www.w3.org/2001/XMLSchema-instance");
	html = html.body.children();
	this.display.html = html;
	var s = html.toXMLString();
	s = s.replace (/\n/mg, "");
	XML.prettyPrinting = false;
	var newhtml = <font size={OMV.htmlFontSize}/>;
	newhtml.appendChild (html);
	html = newhtml.toXMLString();
	XML.prettyPrinting = true;
	return html;
}

// Set the HTML at the HTML widget.

OMV.prototype._setHTML = function (html, href)
{
	if (null == html)
		this.display.html = null;
	html = this._prepareHTML (html);
	if (href)
		this._pushHTMLStack (href);
	OMV.update.push (this);
	addDelayedTask (this._updateOMV);
}

OMV.prototype._updateOMV = function()
{
	for (var i = 0; i < OMV.update.length; i++)
	{
		var omv = OMV.update [i];
		omv.display.invokePlayerFunction("htmlViewerInitialize", "cookie", omv.display.html,
		{ 
			flexStyles: {
				fontGridFitType: "subpixel",
				fontThickness:	50,
				fontSharpness:	50
			}, 
			flexProperties: { 
				condenseWhite: true
			}
		});
	}
	OMV.update = [];
}

// Push an entry on the HTML stack. The entry is the href to display.

OMV.prototype._pushHTMLStack = function (href)
{
	// make the href a fill href
	if (href[0] == '#')
		href = this.currentOMVHref + href.substr (1);
	// do not push the same href twice
	for (var i = 0; i < this.htmlStack.length; i++)
	{
		if (this.htmlStack [i] == href)
			return;
	}
	this.htmlStack.splice (this.htmlPos+1);
	this.htmlStack.push (href);
	this.htmlPos = this.htmlStack.length - 1;
	this.back.enabled = (this.htmlPos > 0);
	this.fwd.enabled = false;
//	print ("#### HTML STACK ####");
//	for (var i = 0; i < this.htmlStack.length; i++)
//		print (i + " " + this.htmlStack [i]);
}
