/**************************************************************************
*
*  @@@BUILDINFO@@@ 96shortcutPrefs-2.jsx 3.0.0.22  18-April-2008
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

// Each Prefs object needs the following methods:
// prefsObj.create (parentPane) - create the pane and return the Pane object.
// pane.preProcess()			- preprocess the pane just before being shown.
// pane.layoutDone()			- informs the pane that the initial layout is done (optional)
// pane.postProcess()			- do any work just before the pane is being hidden.
// pane.store()					- store the preferences. Return true if the Next Time dialog is needed.
// pane.toDefault()				- set back to default values
// pane.cancelled()				- Prefs dialog has been cancelled

// On Windows, the "Cmd" modifier is mapped to the Ctrl modifier, so Ctrl-X combos end up as
// Cmd-X combos on the Mac.

var shortcutPrefs = { title : "$$$/ESToolkit/PreferencesDlg/shortcutTitle=Keyboard Shortcuts", sortOrder : 70 };

globalBroadcaster.registerClient( shortcutPrefs, 'initPrefPanes' );

shortcutPrefs.onNotify = function( reason )
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

shortcutPrefs.create = function (parentPane)
{
	this.pane = parentPane.add (
	"group {	                                    \
		orientation		:'column',					\
		alignChildren	: ['fill','fill'],			\
		visible			: false,					\
		alignment		: ['fill','fill'],			\
		list			: ListBox {					\
			helpTip			: '$$$/ESToolkit/PreferencesDlg/shortcutTitle=Keyboard Shortcuts', \
			preferredSize	: [365,270]				\
		},											\
		options			: Group {					\
		    orientation		: 'row',				\
		    alignment		: ['left','bottom'],	\
			modifiers       : Group {				\
		        orientation		: 'column',			\
		        alignment		: ['left','top'],	\
                alignChildren	: 'left',			\
				modShift		: Checkbox {		\
			        text			: '$$$/ESToolkit/PreferencesDlg/ModShift=Shift' \
		        },									\
				modCmd			: Checkbox {		\
			        text			: '$$$/ESToolkit/PreferencesDlg/ModCtrl=Ctrl' \
		        },									\
				modAlt			: Checkbox {		\
			        text			: '$$$/ESToolkit/PreferencesDlg/ModAlt=Alt'	\
		        },									\
				modCtrl			: Checkbox {		\
			        text			: '$$$/ESToolkit/PreferencesDlg/ModCtrl=Ctrl',	\
			        visible			: false			\
		        }									\
		    },										\
			keyset		: Group {					\
		        orientation : 'column',				\
		        alignment   : ['left','top']		\
				newkeyset   : Group {				\
		            orientation : 'row',			\
		            alignment   : ['left','top']	\
					keylist		: DropDownList {	\
		            },                              \
					btAssign	: Button {			\
		                text        : '$$$/ESToolkit/PreferencesDlg/assignShortcut=Assign' \
		            },								\
					btRemove		: Button {		\
		                text        : '$$$/ESToolkit/PreferencesDlg/removeShortcut=Remove' \
		            }								\
		        },									\
				oldkeyset	: StaticText {			\
				    properties  : { multiline : true }, \
					text        : ' ',				\
					minimumSize	: [100, 50 ],		\
					maximumSize	: [310, 50],		\
					characters  : 50				\
				}									\
			}										\
        }											\
	}");
	this.pane.modShift 	= this.pane.options.modifiers.modShift;
	this.pane.modCmd  	= this.pane.options.modifiers.modCmd;
	this.pane.modAlt   	= this.pane.options.modifiers.modAlt;
	this.pane.modCtrl   = this.pane.options.modifiers.modCtrl;
	this.pane.keyList  	= this.pane.options.keyset.newkeyset.keylist;
	this.pane.btAssign 	= this.pane.options.keyset.newkeyset.btAssign;
	this.pane.btRemove 	= this.pane.options.keyset.newkeyset.btRemove;
	this.pane.oldKeyset	= this.pane.options.keyset.oldkeyset;

	this.pane.keyList.minimumSize.width = 80;
	
	// On Windows, modCmd and modCtrl are the same, and only modCmd is displayed as modCtrl

	if (Folder.fs == "Macintosh")
	{
		this.pane.modAlt.text	  = localize ('$$$/ESToolkit/PreferencesDlg/ModOpt=Option');
		this.pane.modAlt.helpTip  = localize ('$$$/ESToolkit/PreferencesDlg/htModOpt=Option key');
		this.pane.modCtrl.visible = true;
		this.pane.modCmd.text     = '      ';
		this.pane.modCmd.text     = localize( '$$$/ESToolkit/PreferencesDlg/ModCmd=Cmd' );
		this.pane.modCmd.helpTip  = localize( '$$$/ESToolkit/PreferencesDlg/htModCmd=Apple key' );
	}
	this.loaded = false;
	this.pane.prefsObj = this;

	// the item that has the current assignment of a key
	this.usedItem = null;

	this.preProcess = function()
	{
		// not there yet before final layout has been done
		if( this.pane.list.onChange )
			this.pane.list.onChange();
	}

	this.postProcess = function()
	{}

	this.pane.list.textWidth  = 100;		// set by layoutDone()
	
	// Set the text width of the menu text to be displayed
	this.layoutDone = function()
	{
		this.load();

		// This should be the longest to expect
		var str = shortcutPrefs.createDisplayKeyStr ("Shift+Ctrl+Alt+Cmd+X");
		var list = this.pane.list;
		var width = list.size.width;
		var shortcutWidth = list.graphics.measureString (str)[0];
		list.textWidth = width - shortcutWidth;
		// Re-set the item texts
		var items = list.items;
		for (var i = 0; i < items.length; i++)
			list.setItemText (items [i]);
	}
	
	this.pane.cancelled = function()
	{
		var sel = this.list.selection;

		var items = this.list.items;
		for (var i = 0; i < items.length; i++)
		{
			var item = items [i];
			item.shortcutInfo = item.originalShortcutInfo;
	        this.list.setItemText (item);
		}
		if (sel)
			this.list.selection = sel;
	}

	this.pane.list.setItemText = function (item)
	{
		item.setTabbedText (item.menuText, this.textWidth, item.shortcutInfo.text);
	}

	this.toDefault = function()
	{
		var items = this.pane.list.items;
		
		var suffix = Folder.fs == 'Windows' ? '.win' : '.mac'
	    for( var i=0; i<items.length; i++ )
	    {
			var item = items [i];
	        if( item.prefKey )
	        {
	            var defKey = prefs.shortcutkeys.getDefault( item.prefKey, Preference.STRING );
	            
	            if( defKey.length == 0 )
	                defKey = prefs.shortcutkeys.getDefault( item.prefKey + suffix, Preference.STRING );
	            
	            var shortcutInfo    = shortcutPrefs.createShortcutInfo( defKey );
	            shortcutInfo.newkey = true;
                item.shortcutInfo   = shortcutInfo;
				this.pane.list.setItemText (item);
            }
	    }
		// Update current selection
		if (this.pane.list.selection)
			this.pane.list.onChange();
	}

	return this.pane;
}

shortcutPrefs.load = function()
{
 	if (this.loaded)
		return;
	this.loaded = true;

    with( this.pane )
    {
        if( Folder.fs == 'Macintosh' )
        {
            modCtrl.visible = true;
			modCmd.text    = '      ';
            modCmd.text    = localize( '$$$/ESToolkit/PreferencesDlg/ModCmd=Cmd' );
            modCmd.helpTip = localize( '$$$/ESToolkit/PreferencesDlg/htModCmd=Apple key' );
        }

        //
        // fill list box
        //
		list.removeAll();
        
        var menuStructure = this.createMenuStructure();
        
        for( var i=0; i<menuStructure.length; i++ )
        {
            var item            = list.add( 'item', "" );
			item.menuText		= menuStructure[i].entryText;
            item.menu           = menuStructure[i].menu;
            item.prefKey        = menuStructure[i].preferenceKey;
            item.path           = menuStructure[i].pathString;
			
			if( item.menu && item.menu.type == 'menu' )
			{
				item.shortcutInfo = {   key     : "",
										shift   : false,
										alt     : false,
										cmd     : false,
										ctrl    : false,
										text	: "",
										removed : true,
										newkey  : false
								   };
			}
			else
				item.shortcutInfo   = shortcutPrefs.createShortcutInfo( item.menu.shortcutKey );
			
            if( item.menu && item.menu.shortcutKey == '' )
			{
                item.shortcutInfo.removed = true;
				item.shortcutInfo.key = "";
				item.shortcutInfo.text = "";
			}
			// save for restore during preprocess()
			item.originalShortcutInfo = item.shortcutInfo;

            list.setItemText (item);
        }
        
        list.onChange = function()
        {
            if( this.selection )
            {
                shortcutPrefs.reflectShortcut( this.selection.shortcutInfo );
                shortcutPrefs.showCurrentShortcut();
            }	                
        }
        
        list.selection = 0;
        
        //
        // fill key DDL
        //
        var keys = "A|B|C|D|E|F|G|H|I|J|K|L|M|N|O|P|Q|R|S|T|U|V|W|X|Y|Z|1|2|3|4|5|6|7|8|9|0|<|>|,|.|-|+|#";

		if( !_win )
			keys += "|?";

		keys += "|F1|F2|F3|F4|F5|F6|F7|F8|F9|F10|F11|F12";
        keys = keys.split( "|" );
        
        for( var i=0; i<keys.length; i++ )
            keyList.add( 'item', keys[i] );	            
            
        //
        // options change handler
        //
        options.modifiers.modCmd.onClick          =
        options.modifiers.modShift.onClick        =
        options.modifiers.modAlt.onClick          = 
        options.modifiers.modCtrl.onClick         = function()
        {
            shortcutPrefs.showCurrentShortcut();
            shortcutPrefs.updateButtons();
        }
        keyList.onChange  = function()
        {
            shortcutPrefs.showCurrentShortcut();
            shortcutPrefs.updateButtons();
        }
        
        btAssign.onClick = function()
        {
            var listitem = list.selection;
            
            if( listitem )
            {
				 var shortcutInfo = {   key     : keyList.selection.text,
										shift   : modShift.value,
										alt     : modAlt.value,
										cmd     : modCmd.value,
										ctrl    : modCtrl.value,
										text	: "",
										removed : false,
										newkey  : true
								   };
                
                if (_win)
                    //  OSCmd == Ctrl on Windows
                    shortcutInfo.cmd = shortcutInfo.ctrl = (shortcutInfo.cmd || shortcutInfo.ctrl);
								   
				var shortcutStr = shortcutPrefs.createShortcutKeyStr( shortcutInfo );
				shortcutInfo.text = shortcutPrefs.createDisplayKeyStr( shortcutStr );
	            
				listitem.shortcutInfo = shortcutInfo;
                list.setItemText (listitem);

				if (shortcutPrefs.usedItem && shortcutPrefs.usedItem != listitem)
				{
					// Remove a previously used shortcut
					shortcutInfo = {	key     : "",
										shift   : false,
										alt     : false,
										cmd     : false,
										ctrl    : false,
										text	: "",
										removed : true,
										newkey  : false
								   };
					shortcutPrefs.usedItem.shortcutInfo = shortcutInfo;
					list.setItemText (shortcutPrefs.usedItem);

					shortcutPrefs.usedItem = null;
				}

	            shortcutPrefs.showCurrentShortcut();
				shortcutPrefs.updateButtons();
            }
        }
		btRemove.onClick = function()
        {
            var listitem = list.selection;
            
            if( listitem )
            {
				var shortcutInfo = {    key     : "",
										shift   : false,
										alt     : false,
										cmd     : false,
										ctrl    : false,
										text	: "",
										removed : true,
										newkey  : false
								   };
				shortcutPrefs.pane.oldKeyset.text = '';
				listitem.shortcutInfo = shortcutInfo;
				list.setItemText (listitem);
				shortcutPrefs.updateButtons();
				shortcutPrefs.reflectShortcut( shortcutInfo );
			}
		}
   }
}

shortcutPrefs.store = function()
{
	if (!this.loaded)
		return false;

	for( var i=0; i<this.pane.list.items.length; i++ )
    {
		var item = this.pane.list.items[i];
        
        if( item.menu && item.menu.type == "command" )
        {
            if( item.shortcutInfo.removed )
                prefs.shortcutkeys[item.prefKey] = 'none';
            else if( item.shortcutInfo.newkey )
                prefs.shortcutkeys[item.prefKey] = this.createShortcutKeyStr( item.shortcutInfo );

			// save here for later retrieval
			item.originalShortcutInfo = item.shortcutInfo;
        }
    }

    menus.loadKeys();
	return false;
}
   
////////////////////////////////////////////////////////////////

shortcutPrefs.createShortcutInfo = function( shortcutKeyStr )
{
    var shortcutInfo = {    key     : '',
                            shift   : ( shortcutKeyStr.search( /shift/gi ) >= 0 ),
                            alt     : ( shortcutKeyStr.search( /alt/gi ) >= 0 ),
                            cmd     : ( shortcutKeyStr.search( /oscmnd/gi ) >= 0 ),
                            ctrl    : ( shortcutKeyStr.search( /ctrl/gi ) >= 0 ),
							text	: this.createDisplayKeyStr (shortcutKeyStr),
                            removed : false,
                            newkey  : false
                       };
	if (_win)
		//  OSCmd == Ctrl on Windows
		shortcutInfo.cmd = shortcutInfo.ctrl = (shortcutInfo.cmd || shortcutInfo.ctrl);

    var parts = shortcutKeyStr.split('+');

    if( parts.length > 0 )
        shortcutInfo.key = parts[parts.length-1];
        
    return shortcutInfo;
}

shortcutPrefs.createShortcutKeyStr = function( shortcutInfo )
{
    var keyStr = '';
    
    if( shortcutInfo.shift )    keyStr += ( keyStr.length > 0 ? '+' : '' ) + 'Shift';
    if( shortcutInfo.cmd )      keyStr += ( keyStr.length > 0 ? '+' : '' ) + 'OSCmnd';
    if( shortcutInfo.alt )      keyStr += ( keyStr.length > 0 ? '+' : '' ) + 'Alt';
	if (Folder.fs == "Macintosh" && shortcutInfo.ctrl )     
		keyStr += ( keyStr.length > 0 ? '+' : '' ) + 'Ctrl';
    
    keyStr += ( keyStr.length > 0 ? '+' : '' ) + shortcutInfo.key;
    
    return keyStr;
}

shortcutPrefs.createDisplayKeyStr = function( shortcutKeyStr )
{
    var ret = '';
    var entries = shortcutKeyStr.split('+');
    for( var i=0; i<entries.length; i++ )
    {
        if( i > 0 )
            ret += '+';
            
        if( Folder.fs == 'Windows' )
            entries[i] = entries[i].replace( /OSCmnd/g, 'Ctrl' )
        else
		{
            entries[i] = entries[i].replace( /OSCmnd/g, 'Cmd' )
            entries[i] = entries[i].replace( /Alt/g, 'Opt' )
		}

		var toLocalize = "$$$/CT/ExtendScript/UI/MenuShortcut" + entries [i] + "=" + entries [i];
		ret += localize (toLocalize);
    }
    
    return ret;
}

shortcutPrefs.reflectShortcut = function( shortcutInfo )
{
    with( this.pane )
    {
		modCmd.value    = ( shortcutInfo ? shortcutInfo.cmd : false );
        modShift.value  = ( shortcutInfo ? shortcutInfo.shift : false );
        modAlt.value    = ( shortcutInfo ? shortcutInfo.alt : false );
        modCtrl.value   = ( shortcutInfo ? shortcutInfo.ctrl : false );
    }

    var ddl = this.pane.keyList;
    var sel = null;
    
    if( shortcutInfo )
    {
        for( var i=0; i < ddl.items.length; i++ )
        {
            if( ddl.items[i].text == shortcutInfo.key )
            {
                sel = ddl.items[i];
                break;
            }
        }
    }
    ddl.selection = sel;
    this.updateButtons();
}

shortcutPrefs.updateButtons = function()
{
	var selItem = this.pane.list.selection;
	var selKey  = this.pane.keyList.selection;

	// all UI: Enabled if anything selected at all, and not a submenu

	var uiEnabled = (selItem != null && selItem.menu.type == "command");

	// Assign button

	var enabled = uiEnabled && (selKey != null);
	// Enable Assign if either a standard character together with a modifier,
	// or an F key i.e. a character with a length > 0 :)
	if (enabled)
	{
		var mods = this.pane.modCmd.value    ||
				   this.pane.modShift.value  ||
				   this.pane.modAlt.value    ||
				   this.pane.modCtrl.value;
		var fkey = (selKey.text.length > 1);
		enabled = mods || fkey;
	}
    this.pane.btAssign.enabled = enabled;

	// Remove button
	enabled = uiEnabled && (selKey != null);
	// Enable if anything is assigned
	if (enabled)
		enabled = (selItem.shortcutInfo && selItem.shortcutInfo.text != "");
    this.pane.btRemove.enabled = enabled;

	// Other UI
	if (!uiEnabled)
		this.pane.oldKeyset.text = "";
	this.pane.modShift.enabled =
	this.pane.modCmd.enabled   = 
	this.pane.modAlt.enabled   =
	this.pane.modCtrl.enabled  =
	this.pane.keyList.enabled  = uiEnabled;
}

shortcutPrefs.createMenuStructure = function( menu, level, pathStr )
{
    var struct = [];
    
    if( !menu )     menu    = menus;
    if( !level )    level   = 0;
    if( !pathStr || level == 0 )  pathStr = '';

    for( var name in menu )
    {
        if( menu[name] instanceof MenuElement )
        {
            var text = '';
            
            for( var m=0; m<level; m++ )
                text += '    ';
			
			var menuText = menu[name].text.replace( /&/g, '' );
            text += stripTrailingWS (menuText);
            
            if( menu[name].type == 'command' )
            {
                var menuPath = pathStr;
                
                if( menuPath.length > 0 ) 
                    menuPath += ' - ';
                    
                menuPath += menuText;

				struct.push( { menu          : menu[name], 
                               entryText     : text, 
                               preferenceKey : menu[name].id.replace( /\//g, '_' ), 
                               pathString    : menuPath 
                             } 
                           );
            }
            else
            {
                var menuPath = pathStr;
                
                if( level == 0 )
                    menuPath = menuText;
                else
                {
                    if( menuPath.length > 0 ) 
                        menuPath += ' - ';
                    menuPath += menuText;
                }
                
                struct.push( { menu          : menu[name], 
                               entryText     : text, 
                               preferenceKey : menu[name].id.replace( /\//g, '_' ), 
                               pathString    : menuPath 
                             } 
                           );
                
                var sub = shortcutPrefs.createMenuStructure( menu[name], level+1, menuPath );
                
                for( var i=0; i<sub.length; i++ )
                    struct.push( sub[i] );
            }
        }
    }
    
    return struct;
}

// Show the current shortcut setting
// Show the text where it is currently used from the attached shortcut Keys

shortcutPrefs.showCurrentShortcut = function()
{
 	this.usedItem = null;
	this.pane.oldKeyset.text = '';
    
    var key = '';
    
    if( this.pane.keyList.selection )
        key = this.pane.keyList.selection.text;
        
    var shortcutInfo = {    key     : key,
                            shift   : this.pane.modShift.value,
                            alt     : this.pane.modAlt.value,
                            cmd     : this.pane.modCmd.value,
                            ctrl    : this.pane.modCtrl.value,
                            removed : false,
                            newkey  : false
                       };

    if (_win)
        //  OSCmd == Ctrl on Windows
        shortcutInfo.cmd = shortcutInfo.ctrl = (shortcutInfo.cmd || shortcutInfo.ctrl);

    var searchCr = shortcutPrefs.createShortcutKeyStr( shortcutInfo );

    searchCr = searchCr.split('+');

    for( var i=0; i<this.pane.list.items.length; i++ )
    {
		var item = this.pane.list.items[i];
		var curShortcutInfo = item.shortcutInfo;
		if (curShortcutInfo.text  != ""
		 && curShortcutInfo.shift == shortcutInfo.shift
		 && curShortcutInfo.alt   == shortcutInfo.alt
		 && curShortcutInfo.cmd   == shortcutInfo.cmd
		 && curShortcutInfo.ctrl  == shortcutInfo.ctrl
		 && curShortcutInfo.key   == shortcutInfo.key )
		{
			this.pane.oldKeyset.text = localize('$$$/ESToolkit/PreferencesDlg/usedShortcut=Currently used for : ') + this.pane.list.items[i].path;
			this.usedItem = item;
			break;
        }
    }
}

