/**************************************************************************
*
*  @@@BUILDINFO@@@ 00globals-2.jsx 3.5.0.43	20-November-2009
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

//
// check for BridgeTalk, emulate if not available
//
if( typeof BridgeTalk == 'undefined' )
{
    function BridgeTalk()
    {
        this.sender  = '';
        this.target  = '';
        this.timeout = 0;
        this.type    = '';
        this.body    = '';
        this.headers = {};
    }
    BridgeTalk.prototype.send       = function(){ return false; }
    BridgeTalk.prototype.sendResult = function(){ return false; }
    BridgeTalk.prototype.pump       = function(){ return false; }

    BridgeTalk.launch               = function(){}
    BridgeTalk.isRunning            = function(){ return false; }
    BridgeTalk.pump                 = function(){ return false; }
    BridgeTalk.loadAppScript        = function(){ return false; }
    BridgeTalk.getSpecifier         = function(){ return ''; }
    BridgeTalk.getTargets           = function(){ return []; }
    BridgeTalk.getDisplayName       = function(){ return ''; }
    BridgeTalk.bringToFront         = function(){}
    BridgeTalk.getStatus            = function(){ return ''; }
    BridgeTalk.ping                 = function(){ return ''; }
    BridgeTalk.getAppPath           = function(){ return ''; }
    BridgeTalk.getPreferredApp      = function(){ return ''; }
    BridgeTalk.pendingResponses     = function(){ return 0; }
    BridgeTalk.supportsESTK         = function(){ return false; }
    BridgeTalk.getOMVDictionaryType = function(){ return false; }
    BridgeTalk.getInfo              = function(){ return ''; }
    BridgeTalk.isInstalled          = function(){ return false; }
    BridgeTalk.updateConnectorCache = function(){ return false; }

	BridgeTalk.appSpecifier    = '';
	BridgeTalk.appName         = '';
	BridgeTalk.appVersion      = '';
	BridgeTalk.appLocale       = '';
	BridgeTalk.appInfo         = '';
	BridgeTalk.appInstance     = '';
	BridgeTalk.appStatus       = '';;
	
	BridgeTalk.emu             = true;
}

//
// set app.resourceFolder and app.requiredFolder
//
if( $.os.indexOf( 'Windows' ) > -1 )
{
    app.resourceFolder = Folder (Folder.appPackage + "/required");
    var f = app.resourceFolder.resolve();
    if (f)
        app.resourceFolder = f;

    app.requiredFolder = app.resourceFolder;
    app.resourceFolder += '/icons/';
}
else
{
	app.requiredFolder = Folder.appPackage.absoluteURI + '/Contents/SharedSupport/Required/';
    app.resourceFolder = Folder.appPackage.absoluteURI + '/Contents/Resources/';
}

///////////////////////////////////////////////////////////////////////////////

const _win = (File.fs == "Windows");

// the list of all non-document panes
var panes = null;

// document manager
var docMgr = null;

// The list of all document
var documents = [];

// The current document
var document = null;

// All menus, grouped by name (like e.g. menus.file)
const menus = {};

// target manager
var targetMgr = null;

// global broadcaster
var globalBroadcaster = new Broadcaster;

// favorites
var favorites = null;

// workspace visibility
var menuToggleWS = null;
var workspaceVisible = true;
var wsHideOnNextStartup = false;

// in shutdown
var appInShutDown = false;

///////////////////////////////////////////////////////////////////////////////

// A list of predefined HTML colors.

const colors = {
	AliceBlue			: 0xF0F8FF,
	AntiqueWhite  		: 0xFAEBD7,
	Aqua  				: 0x00FFFF,
	Aquamarine  		: 0x7FFFD4,
	Azure  				: 0xF0FFFF,
	Beige  				: 0xF5F5DC,
	Bisque  			: 0xFFE4C4,
	Black  				: 0x000000,
	BlanchedAlmond  	: 0xFFEBCD,
	Blue  				: 0x0000FF,
	BlueViolet  		: 0x8A2BE2,
	Brown  				: 0xA52A2A,
	BurlyWood  			: 0xDEB887,
	CadetBlue			: 0x5F9EA0,
	Chartreuse  		: 0x7FFF00,
	Chocolate			: 0xD2691E,
	Coral  				: 0xFF7F50,
	CornflowerBlue  	: 0x6495ED,
	Cornsilk  			: 0xFFF8DC,
	Crimson  			: 0xDC143C,
	Cyan  				: 0x00FFFF,
	DarkBlue  			: 0x00008B,
	DarkCyan  			: 0x008B8B,
	DarkGoldenRod		: 0xB8860B,
	DarkGray  			: 0xA9A9A9,
	DarkGreen  			: 0x006400,
	DarkKhaki			: 0xBDB76B,
	DarkMagenta  		: 0x8B008B,
	DarkOliveGreen  	: 0x556B2F,
	Darkorange  		: 0xFF8C00,
	DarkOrchid  		: 0x9932CC,
	DarkRed  			: 0x8B0000,
	DarkSalmon  		: 0xE9967A,
	DarkSeaGreen		: 0x8FBC8F,
	DarkSlateBlue		: 0x483D8B,
	DarkSlateGray		: 0x2F4F4F,
	DarkTurquoise		: 0x00CED1,
	DarkViolet  		: 0x9400D3,
	DeepPink  			: 0xFF1493,
	DeepSkyBlue  		: 0x00BFFF,
	DimGray  			: 0x696969,
	DodgerBlue  		: 0x1E90FF,
	Feldspar  			: 0xD19275,
	FireBrick			: 0xB22222,
	FloralWhite			: 0xFFFAF0,
	ForestGreen			: 0x228B22,
	Fuchsia  			: 0xFF00FF,
	Gainsboro  			: 0xDCDCDC,
	GhostWhite  		: 0xF8F8FF,
	Gold  				: 0xFFD700,
	GoldenRod  			: 0xDAA520,
	Gray  				: 0x808080,
	Green  				: 0x008000,
	GreenYellow  		: 0xADFF2F,
	HoneyDew  			: 0xF0FFF0,
	HotPink  			: 0xFF69B4,
	IndianRed   		: 0xCD5C5C,
	Indigo   			: 0x4B0082,
	Ivory				: 0xFFFFF0,
	Khaki				: 0xF0E68C,
	Lavender  			: 0xE6E6FA,
	LavenderBlush  		: 0xFFF0F5,
	LawnGreen			: 0x7CFC00,
	LemonChiffon  		: 0xFFFACD,
	LightBlue			: 0xADD8E6,
	LightCoral  		: 0xF08080,
	LightCyan			: 0xE0FFFF,
	LightGoldenRodYellow: 0xFAFAD2,
	LightGray			: 0xD3D3D3,
	LightGreen  		: 0x90EE90,
	LightPink  			: 0xFFB6C1,
	LightSalmon  		: 0xFFA07A,
	LightSeaGreen  		: 0x20B2AA,
	LightSkyBlue  		: 0x87CEFA,
	LightSlateBlue  	: 0x8470FF,
	LightSlateGray  	: 0x778899,
	LightSteelBlue  	: 0xB0C4DE,
	LightYellow  		: 0xFFFFE0,
	Lime  				: 0x00FF00,
	LimeGreen  			: 0x32CD32,
	Linen  				: 0xFAF0E6,
	Magenta  			: 0xFF00FF,
	Maroon  			: 0x800000,
	MediumAquaMarine  	: 0x66CDAA,
	MediumBlue  		: 0x0000CD,
	MediumOrchid  		: 0xBA55D3,
	MediumPurple  		: 0x9370D8,
	MediumSeaGreen  	: 0x3CB371,
	MediumSlateBlue  	: 0x7B68EE,
	MediumSpringGreen  	: 0x00FA9A,
	MediumTurquoise  	: 0x48D1CC,
	MediumVioletRed  	: 0xC71585,
	MidnightBlue  		: 0x191970,
	MintCream  			: 0xF5FFFA,
	MistyRose			: 0xFFE4E1,
	Moccasin  			: 0xFFE4B5,
	NavajoWhite  		: 0xFFDEAD,
	Navy				: 0x000080,
	OldLace				: 0xFDF5E6,
	Olive				: 0x808000,
	OliveDrab  			: 0x6B8E23,
	Orange  			: 0xFFA500,
	OrangeRed  			: 0xFF4500,
	Orchid  			: 0xDA70D6,
	PaleGoldenRod  		: 0xEEE8AA,
	PaleGreen  			: 0x98FB98,
	PaleTurquoise  		: 0xAFEEEE,
	PaleVioletRed  		: 0xD87093,
	PapayaWhip  		: 0xFFEFD5,
	PeachPuff  			: 0xFFDAB9,
	Peru  				: 0xCD853F,
	Pink  				: 0xFFC0CB,
	Plum  				: 0xDDA0DD,
	PowderBlue  		: 0xB0E0E6,
	Purple  			: 0x800080,
	Red  				: 0xFF0000,
	RosyBrown  			: 0xBC8F8F,
	RoyalBlue  			: 0x4169E1,
	SaddleBrown  		: 0x8B4513,
	Salmon  			: 0xFA8072,
	SandyBrown  		: 0xF4A460,
	SeaGreen  			: 0x2E8B57,
	SeaShell  			: 0xFFF5EE,
	Sienna  			: 0xA0522D,
	Silver  			: 0xC0C0C0,
	SkyBlue  			: 0x87CEEB,
	SlateBlue  			: 0x6A5ACD,
	SlateGray  			: 0x708090,
	Snow  				: 0xFFFAFA,
	SpringGreen  		: 0x00FF7F,
	SteelBlue  			: 0x4682B4,
	Tan  				: 0xD2B48C,
	Teal  				: 0x008080,
	Thistle  			: 0xD8BFD8,
	Tomato  			: 0xFF6347,
	Turquoise  			: 0x40E0D0,
	Violet  			: 0xEE82EE,
	VioletRed  			: 0xD02090,
	Wheat				: 0xF5DEB3,
	White				: 0xFFFFFF,
	WhiteSmoke  		: 0xF5F5F5,
	Yellow  			: 0xFFFF00,
	YellowGreen  		: 0x9ACD32
};

//-----------------------------------------------------------------------------
// 
// messageBox(...)
// 
// Purpose: Global Alert box that is localizable and displays an error title
// 
//-----------------------------------------------------------------------------

function messageBox (msg)
{
	workspace.storeFocus();
	var title = app.title + ' ' + app.version;
	if (msg [0] == "$")
		msg = localize.apply (this, arguments);
	alert (msg, title);
	workspace.restoreFocus();
}

//-----------------------------------------------------------------------------
// 
// queryBox(...)
// 
// Purpose: Global Confirm box that is localizable and displays the right title
// 
//-----------------------------------------------------------------------------

function queryBox (msg)
{
	workspace.storeFocus();
	var title = app.title + ' ' + app.version;
	if (msg [0] == "$")
		msg = localize.apply (this, arguments);
	ret = confirm (msg, false, title);
	workspace.restoreFocus();

	return ret;
}

//-----------------------------------------------------------------------------
// 
// dsaQueryBox(...)
// 
// Purpose: Global Confirm box that is localizable and displays the right title
//          and "don't show again" checkbox
// 
//-----------------------------------------------------------------------------

function dsaQueryBox( name, msg )
{
	if( !app.enableStandardUI )
	{
		app.writeLog( "NO UI: dsaQueryBox( '"+name+"' , '"+msg+"' )" );
		return true;
	}
		
    var ret = false;

	workspace.storeFocus();
    
	var title = app.title + ' ' + app.version;
	
	//
	// localize message string
	//
	if( msg[0] == "$" )
	{
	    var args = [msg];
	    
	    if( arguments.length > 2 )
	    {
	        for( var i=2; i<arguments.length; i++ )
	            args.push( arguments[i] );
	    }
	    
		msg = localize.apply( this, args );
	}
		
	if( name )
	{
	    //
	    // if there's a preference value for this dialog then skip 
	    // the dialog UI and return the preference value instead
	    //
	    if( prefs.DSDialog[name].hasValue() )
	        ret = prefs.DSDialog[name].getValue( Preference.BOOLEAN );
	    else
	    {
	        //
	        // create dialog
	        //
	        var dlgRes  = """prefdialog { text : '""" + title + """',
	                              orientation : 'column',
	                              properties  : { name : '""" + name + """'},
	                              gm          : Group
	                              {
                                     orientation    : 'row',
                                     alignment      : ['left', 'top'],
                                     image          : Image
                                     {
                                        minimumSize     : [32, 32]
                                     },
                                     gt     : Group
                                     {
                                        orientation : 'column',
                                        alignment   : ['left', 'top'],
                                        spacing     : 1,
                                        statictext  : StaticText
                                        {
                                            text        : "",
                                            alignment   : "left",
                                            properties  : { multiline : true }
                                        }
                                     
                                     } 
                                  },
	                              gb          : Group
	                              {
                                     orientation    : 'row',
	                                 gc             : Group
	                                 {
										alignment      : ['left', 'top'],
                                        dshow   : Checkbox
                                        {
                                            text    : "$$$/ESToolkit/Dialog/DontShowAgain=Don't show again",
                                            helpTip : "$$$/ESToolkit/Dialog/htDontShowAgain=Don't show this dialog again."
                                        }
	                                 },
	                                 gt             : Group
	                                 {
										alignment      : ['right', 'top'],
                                        orientation : 'row',
                                        btOK        : Button
                                        {
                                            properties  : { name : 'ok' },
                                            text        : '$$$/CT/ExtendScript/UI/Yes=&Yes'
                                        },
                                        btCancel    : Button
                                        {
                                            properties  : { name : 'cancel' },
                                            text        : '$$$/CT/ExtendScript/UI/No=&No'
                                        }
	                                 }
	                              }
	                           }""";

	        var dlg = new Window( dlgRes );
	        
	        dlg.gm.gt.statictext.text = msg;
	        
	        dlg.gm.image.icon = ScriptUI.newImage( 'SystemQueryIcon' );
	        
	        //
	        // show dialog
	        //
            ret = ( dlg.show() == 1 );

            //
            // if "don't show again" was checked then store the return
            // value of the dialog in the preferences
            //
            if( dlg.gb.gc.dshow.value )
                prefs.DSDialog[name] = ret;
        }
	}
	else
	    queryBox( msg );

	workspace.restoreFocus();
	    
	return ret;
}

// Custom prompt dialog

function smartPrompt( text, value )
{
    var ret = value;

	workspace.storeFocus();
    
    var title   = app.title + ' ' + app.version;
    var dlgname = encodeURI( title );

	if( text[0] == "$")
		text = localize.apply( this, arguments );
    
    var dlg = new Window( 
        "prefdialog { text : '" + title + "',                                                                                                                                                     \
            orientation : 'column',                                                                                                                                                     \
            properties : { name : '" + dlgname + "' },                                                                                                                                        \
            grp1  : Group                                                                                                                                                                 \
            {                                                                                                                                                                           \
                orientation : 'row',                                                                                                                                                 \
                alignment : ['fill', 'top'],                                                                                                                                         \
                spacing         : 10,                                                                                                                                                                \
                grp11 : Group                                                                                                                                                             \
                {                                                                                                                                                                       \
                    orientation : 'row',                                                                                                                                                \
                    alignment : ['left', 'top'],                                                                                                                                         \
                    text : StaticText                                                                                                                                                     \
                    {                                                                                                                                                                   \
                        alignment : 'left',                                                                                                                                         \
                        text    : '" + text + "'                                                                                                        \
                    },                                                                                                                                                                  \
                }                                                                                                                                                                       \
                grp12: Group                                                                                                                                                         \
                {                                                                                                                                                                       \
                    orientation : 'column',                                                                                                                                                \
                    alignment : ['right', 'top'],                                                                                                                                         \
                    btOK : Button                                                                                                                                                  \
                    {                                                                                                                                                                   \
                        text : '$$$/CT/ExtendScript/UI/OK=&OK',                                                                                                                                               \
                        properties :{ name : 'ok' }                                                                                                                               \
                    },                                                                                                                                                                  \
                    btCancel  : Button                                                                                                                                                \
                    {                                                                                                                                                                   \
                        text : '$$$/CT/ExtendScript/UI/Cancel=&Cancel',                                                                                                                                               \
                        properties :{ name : 'cancel' }                                                                                                                               \
                    }                                                                                                                                                                   \
                },                                                                                                                                                                      \
            },                                                                                                                                                                          \
            grp2  : Group                                                                                                                                                                 \
            {                                                                                                                                                                           \
                spacing         : 0,                                                                                                                                                                \
                value : EditText                                                                                                                                                  \
                {                                                                                                                                                                   \
                    alignment:'fill',                                                                                                                                               \
                    text    : '" + value + "',                                                                                                                                                   \
                    characters  : 40,                                                                                                                                               \
                },                                                                                                                                                                  \
            }                                                                                                                                                                           \
        }" );
        
    dlg.onShow = function()
    {
        dlg.grp2.value.active = true;
    }

    value = ( dlg.show() == 1 ? dlg.grp2.value.text : null );
    
	workspace.restoreFocus();

    return value;
}

/////////////////////////////////////////////////////
//
// error box
//

function errorBox( head, details )
{
    function escapeText( str, ignoreLE )
    {
        var ret = "";
        
        for( var i=0; i<str.length; i++ )
        {
            if( str[i] == "\n" )
            {
				if( !ignoreLE )
					ret += "\\n";
                continue;
            }
            if( str[i] == "\\" )
                ret += "\\";
            if( str[i] == "\"" )
                ret += "\\";
            ret += str[i];
        }
        
        return ret;
    }

	//
	// setup text
	//
    var headText = escapeText( head, true );
    var msgText = head;
    
    for( var i=1; i<arguments.length; i++ )
        msgText += "\n" + arguments[i];
      
    msgText = escapeText(msgText);

	//
	// write error to log
	//
    app.writeLog( msgText );

    if( app.enableStandardUI )
    {
        workspace.storeFocus();
      
		//
		// create dialog window
		//
        var title = app.title + ' ' + app.version;
                                                    
        var w = new Window( """dialog {
                                properties  :
                                {
                                    resizeable  : true
                                },
                                text    : """" + title + """",
                                orientation : "column",
                                content : Group
                                {
                                    alignment   : ["fill","fill"],
                                    orientation : "row",
                                    image          : Image
                                    {
                                        alignment   : ["left","top"],
                                        minimumSize     : [32, 32]
                                    },
                                    right   : Group
                                    {
                                        alignment   : [ "fill","fill"],
                                        orientation : "column",
                                        head        : StaticText
                                        {
                                            alignment   : ["fill","top"],
                                            text    : """" + headText + """"
                                        },
                                        grSwitch    : Group
                                        {
                                            margins : 0,
                                            spacing : 4,
                                            orientation : "row",
                                            alignment   : ["left","top"],
                                            btnSwitch  : Image
                                            {
                                                alignment   : "left",
                                                minimumSize : [10,10],
                                                properties       : { style : 'toolbutton' }
                                            },
                                            static  : StaticText
                                            {
                                                text    : "$$$/ESToolkit/Dialogs/Details=Details"
                                            }
                                        },
                                        details : EditText
                                        {
                                            alignment   : ["fill","fill"],
                                            visible : false,
                                            preferredSize   : [1,1],
                                            properties  :
                                            {
                                                multiline   : true,
                                                readonly    : true
                                            },
                                            text    : """" + msgText + """"
                                        }
                                    },
                                },
                                btnOK   : Button
                                {
                                    alignment   : ["center","bottom"],
                                    properties  : { name : 'ok' },
                                    text    : "OK"
                                }
                            }""" );
          
		//
		// setup window
		//
        if( _win )
            w.minimumSize = [260, 120];
        else
            w.minimumSize = [260, 130];                            
            
        w.content.image.icon = ScriptUI.newImage( 'SystemStopIcon' );      
        
        w.content.right.grSwitch.btnSwitch.iconExpand = ScriptUI.newImage( 'SystemExpand' );      
        w.content.right.grSwitch.btnSwitch.iconCollapse = ScriptUI.newImage( 'SystemCollapse' );      
        
        w.content.right.grSwitch.btnSwitch.icon = w.content.right.grSwitch.btnSwitch.iconExpand;
        w.content.right.grSwitch.btnSwitch.expanded = false;
           
        const minDetailHeight = 150;
     
        var font = w.content.right.head.graphics.font;
        var fontNew = font;

        try
        {
            if( _win )
                fontNew = ScriptUI.newFont( "", "BOLD", font.size+1 );
            else
                fontNew = ScriptUI.newFont( font.family, "BOLD", font.size+2 );
        }
        catch( exc )
        {
            fontNew = font;
        }
        
        w.content.right.head.graphics.font = fontNew;
        
        //
		// expand/collapse details
		//
        w.content.right.grSwitch.btnSwitch.addEventListener( "mousedown", function(e) { e.target.onClick(); }, false );  
        
        w.content.right.grSwitch.btnSwitch.onClick = function()
        {
            this.expanded = !this.expanded;
            
            if( this.expanded )
                this.icon = this.iconCollapse;
            else
                this.icon = this.iconExpand;

            this.parent.parent.details.visible = !this.parent.parent.details.visible;

            if( this.parent.parent.details.visible )
            {
                var dh = 0;
                
                if( prefs.ErrorDlg.detailheight.hasValue( Preference.NUMBER ) )
                    dh = prefs.ErrorDlg.detailheight.getValue( Preference.NUMBER );
                    
                if( dh < minDetailHeight )
                    dh = minDetailHeight;
                    
                if( this.window.size.height <= dh )
                    this.window.size.height = dh;
            }
            else
            {
                prefs.ErrorDlg.detailheight = this.window.size.height;
                this.window.size.height -= this.parent.parent.details.size.height;
            }
        }

        w.onResize = function() 
        { 
            this.layout.resize(); 
        }

        w.onShow = function()
        {
            var w = this.size.width;
            var h = this.size.height;
            var x = this.location.x;
            var y = this.location.y;

            if( prefs.ErrorDlg.width.hasValue( Preference.NUMBER ) )
            {
                var pw = prefs.ErrorDlg.width.getValue( Preference.NUMBER );
                
                if( pw > w )    
                    w = pw;
            }

            if( prefs.ErrorDlg.height.hasValue( Preference.NUMBER ) )
            {
                var ph = prefs.ErrorDlg.height.getValue( Preference.NUMBER );
                
                if( ph > h )    
                    h = ph;
            }

            if( prefs.ErrorDlg.posx.hasValue( Preference.NUMBER ) )
                x = prefs.ErrorDlg.posx.getValue( Preference.NUMBER );

            if( prefs.ErrorDlg.posy.hasValue( Preference.NUMBER ) )
                y = prefs.ErrorDlg.posy.getValue( Preference.NUMBER );

            this.size.width     = w;
            this.size.height    = h;

            if( x > 0 )
                this.location.x     = x;
            if( y > 0 )
                this.location.y     = y;
                
            if( prefs.ErrorDlg.details.hasValue( Preference.BOOLEAN ) &&
                prefs.ErrorDlg.details.getValue( Preference.BOOLEAN ) != this.content.right.details.visible )
            {
                this.content.right.grSwitch.btnSwitch.onClick();
            }
                
            this.layout.resize(); 
        }

        w.onClose = function()
        {
            prefs.ErrorDlg.width    = this.size.width;
            prefs.ErrorDlg.height   = this.size.height;
            prefs.ErrorDlg.posx     = this.location.x;
            prefs.ErrorDlg.posy     = this.location.y;

            prefs.ErrorDlg.details  = this.content.right.details.visible;
            
            if( this.content.right.details.visible )
                prefs.ErrorDlg.detailheight = this.size.height;
        }

        w.show();
    }
}

///////////////////////////////////////////////////////////////////////////////
// A little modeless dialog floating in the middle of the window
// This dialog displays short, one-line messages. Use an empty message
// or no arguments to hide the dialog. The message may take additional
// arguments, and it is passed to localize().
// If cancelCB is supplied, the floatingMessage has a Cancel button,
// and if that button is clicked, cancelCB (userData) is called.

var floatDlg = null;
var floatDlgCB = null;

function floatingMessage (msg)
{
	// Hide any callback floater
	if (floatDlgCB && floatDlgCB.visible)
	{
		floatDlgCB.maxWidth = 0;
		floatDlgCB.hide();
	}
	if (!floatDlg && msg)
	{
		floatDlg = new Window ("palette { msg: StaticText { } }");
		floatDlg.text = app.title + ' ' + app.version;
		floatDlg.maxWidth = 0;
	}
	if (msg)
	{
		if (msg [0] == "$")
			msg = localize.apply (this, arguments);
		var uiMsg = floatDlg.msg;
		uiMsg.text = msg;
		var size = uiMsg.graphics.measureString (msg);
		if (size.width > floatDlg.maxWidth)
		{
			floatDlg.maxWidth = size.width;
			uiMsg.size = size;
			floatDlg.layout.layout (true);
		}
		floatDlg.center (Window.children[0]);
		floatDlg.show();
	}
	else if (floatDlg)
	{
		floatDlg.maxWidth = 0;
		floatDlg.hide();
	}
}

// Same as above; additional arguments to the message follow the userData argument.
// The floatingMessage has a Cancel button, and if that button is clicked, 
// cancelCB (userData) is called.
// IMPORTANT: while processing, call app.pumpEventLoop() to enable the Cancel button!

function floatingMessageCB (msg, cancelCB, userData)
{
	// Hide any non-callback floater
	if (floatDlg && floatDlg.visible)
	{
		floatDlg.maxWidth = 0;
		floatDlg.hide();
	}
	if (!floatDlgCB && msg)
	{
		floatDlgCB = new Window (
		"palette { msg: StaticText { },\
		btn: Button { text:'$$$/CT/ExtendScript/UI/Cancel=&Cancel', properties:{name:'cancel'} } }");
		floatDlgCB.text = app.title + ' ' + app.version;
		floatDlgCB.btn.onClick = function()
		{
			this.cancelCB (this.userData);
		}
	}
	if (msg)
	{
		floatDlgCB.btn.cancelCB = cancelCB;
		floatDlgCB.btn.userData = userData;
		if (msg [0] == "$")
		{
			var args = [msg];
			for (var i = 3; i < arguments.length; i++)
				args.push (arguments [i]);
			msg = localize.apply (this, args);
		}
		var uiMsg = floatDlgCB.msg;
		uiMsg.text = msg;
		var size = uiMsg.graphics.measureString (msg);
		if (size.width > floatDlgCB.maxWidth)
		{
			floatDlgCB.maxWidth = size.width;
			uiMsg.size = size;
			floatDlgCB.layout.layout (true);
		}
		floatDlgCB.center (Window.children[0]);
		floatDlgCB.show();
		// Disable the app
		app.enabled = false;
	}
	else if (floatDlgCB)
	{
		floatDlgCB.maxWidth = 0;
		floatDlgCB.hide();
		// re-enable the app
		app.enabled = true;
	}
}

///////////////////////////////////////////////////////////////////
//
//	ProgressBox
//
//	The ProgressBox displays a progress dialog. The ctor takes a 
//	text which is localized. The ctor may take additional arguments.
//	We have these functions:
//	setText (msg) - set text (additional args permitted)
//	setProgress (val[, max]) - set progress. Returns false on abort.
//	stop() - hide the dialog.
//
///////////////////////////////////////////////////////////////////

function ProgressBox (msg)
{
	this.dlg = new Window (
	"palette { \
	msg:	StaticText {				\
		alignment	: 'left',			\
		},								\
		bar:	Progressbar {			\
			preferredSize:	[200, 20],	\
		},								\
		btn:	Button {				\
			text:			'$$$/CT/ExtendScript/UI/Cancel=&Cancel',	\
			properties: {				\
				name		:'cancel'	\
			}							\
		}								\
	}");
	if (msg [0] == "$")
		msg = localize.apply (this, arguments);
	this.dlg.text = app.title + ' ' + app.version;
	this.dlg.btn.self = this;
	this.dlg.btn.onClick = function()
	{
		this.self.aborted = true;
		this.self.stop();
	}
	this.dlg.bar.maxvalue = 100;
	this.maxWidth = 0;
	this.aborted = false;

	this.setText (msg);
	this.dlg.center ();
	this.dlg.show();
	app.enabled = false;
}

ProgressBox.prototype.setText = function (msg)
{
	if (this.dlg)
	{
		if (msg [0] == "$")
			msg = localize.apply (this, arguments);
		var uiMsg = this.dlg.msg;
		uiMsg.text = msg;
		var size = uiMsg.graphics.measureString (msg);
		if (size.width > this.maxWidth)
		{
			this.maxWidth = size.width;
			uiMsg.size = size;
			this.dlg.layout.layout (true);
		}
	}
}

ProgressBox.prototype.increment = function (n)
{
	if (!this.dlg || this.aborted)
		return false;

	if (!n)
		n = 1;
	return this.setProgress (this.dlg.bar.value + n);
}

ProgressBox.prototype.getProgress = function()
{
	var n = 0;
	if (this.dlg)
		n = this.dlg.bar.value;
	return n;
}

ProgressBox.prototype.setProgress = function (n, end)
{
	var ok = app.pumpEventLoop (true);
	ok &= (!this.aborted && (this.dlg != null));
	if (ok)
	{
		if (end != undefined)
			this.dlg.bar.maxvalue = end;
		this.dlg.bar.value = n;
	}
	return ok;
}

ProgressBox.prototype.getRemainingProgress = function ()
{
	var n = 0;
	if (this.dlg)
		n = this.dlg.bar.maxvalue - this.dlg.bar.value;
	return n;
}

ProgressBox.prototype.disableCancel = function()
{
	this.dlg.btn.enabled = false;
}

ProgressBox.prototype.stop = function()
{
	if (this.dlg)
		this.dlg.hide();
	this.dlg = null;
	app.enabled = true;
}

///////////////////////////////////////////////////////////////////
//
//	print()
//
///////////////////////////////////////////////////////////////////

function print()
{
	if (console && console.print)
	{
		var s = "";
		for (var i = 0; i < arguments.length; i++)
			s += arguments [i];
		console.print (s);
	}
}

function log()
{
		var s = "";
		for (var i = 0; i < arguments.length; i++)
			s += arguments [i];
		_log (s);
}

//
// wait for BridgeTalk
//
function wait( abortFct, timeout )
{
    var ret = false;
    
    if( abortFct )
    {
        if( !timeout )
            timeout = 50000;
            
	    var then = new Date;
	    
	    while( abortFct() )
	    {
		    BridgeTalk.pump();
		    cdi.pump();
		    
		    var now = new Date;
		    
		    if( (now - then) > timeout )
		        return ret;
	    }
	    
	    ret = true;
	}
	
	return ret;
}

//
// enable/disable icon buttons
//
IconButton.prototype.setEnabled = function( state )
{
	if( !this.stateCount )
		this.stateCount = 0;
		
	if( state )
	{
		this.stateCount--;
		
		if( this.stateCount < 0 )
			this.stateCount = 0;
	}
	else
	{
		this.stateCount++;
	}

	this.enabled = state;//(this.stateCount == 0 );
//    this.icon	 = this.enabled ? this.enabledIcon : this.disabledIcon;
}

///////////////////////////////////////////////////////////////////////////////
//
// Error Info class
//

function ErrorInfo( error )
{
    this.errors = [];
    
    if( error )
        this.errors.push( error );
}

ErrorInfo.prototype.push = function( error )
{
    this.errors.push( error );
}

ErrorInfo.prototype.pop = function()
{
    this.errors.pop();
}

ErrorInfo.prototype.length = function()
{
    return this.errors.length;
}

ErrorInfo.prototype.display = function( inDialog, inStatusbar )
{
    var ret = false;
    
    if( typeof(inDialog) == "undefined" )       inDialog    = true;
    if( typeof(inStatusbar) == "undefined" )    inStatusbar = true;

    if( this.errors.length > 0 )
    {
        var main = this.errors[ this.errors.length - 1 ];
        
        if( inStatusbar )
		{
			var status = main;

			if( this.errors.length > 1 )
				status += " : " + this.errors[ this.errors.length - 2 ];

            docMgr.setStatusLine( status );
		}
            
        if( inDialog )
		{
			var msg = "";
            
			if( this.errors.length > 1 )
			{
				for( var i=this.errors.length-2; i>=0; i-- )
				{
					if( i > this.errors.length-2 )
						msg += "\n";
                        
					msg += this.errors[i];   
				}
			}

			if( app.enableStandardUI )
				errorBox( main, msg );
			else
				app.writeLog( "NO UI: ErrorInfo.display( '"+main+"\n"+msg+"' )" );
        }
    }
    
    return ret;
}

function InternalError( errorInfo, inDialog, inStatusbar )
{
    var error = errorInfo;
    
    if( !error )
        error = new ErrorInfo();
        
    error.push( localize( "$$$/CT/ExtendScript/Errors/InternalError=InternalError" ) );
    error.display( inDialog, inStatusbar );
}

///////////////////////////////////////////////////////////////////
//
//	Delayed Tasks
//	call addDelayedTask (foo) to add a function that is executed
//	about 20 msecs after this function has been called. If there
//	are multiple delayed functions, the latest function is added
//	to the stack of functions to be executed. The function should
//	not take long to execute, and it should not alter any visible.
//	Only one instance of a function/argument combo is added. If
//  the first argument is not a function, but the second is, the 
//  first argument is consider to be "this".
//
///////////////////////////////////////////////////////////////////

function addDelayedTask()
{
	var fn = arguments[0];
	var self = $.global;
	var arg1 = 1;
	if (typeof fn != "function")
	{
		self = fn;
		fn = arguments [1];
		arg1 = 2;
		if (typeof self != "object")
			self = $.global;
	}
	$.bp (typeof fn != "function");
	for (var i = 0; i < delayedTasks.length; i++)
	{
		var obj = delayedTasks [i];
		if( obj.fn == fn && obj.self == self && obj.args.length == arguments.length )
		{
			var same = true;
			for (var j = 0; same && j < arguments.length; j++)
				same = (arguments [j] === obj.args [j]);
			if (same)
			{
				// foo is there already - remove
				delayedTasks.splice (i, 1);
				break;
			}
		}
	}
	var args = [];
	for (i = arg1; i < arguments.length; i++)
		args.push (arguments [i]);

	delayedTasks.push( {self:self, fn:fn, args:args} );
	// app.onStartup sets delayedTaskID to 0 to signal that running
	// these tasks is OK now.
	if (delayedTasks.length && delayedTaskID == 0)
		delayedTaskID = app.scheduleTask ("delayedTaskExec()", 20, true);
}

// app.onStartup() calls this function at the end to get things going
// that have accumulated during startup.

function startDelayedTasks()
{
	delayedTaskID = 0;
	if (delayedTasks.length)
		delayedTaskID = app.scheduleTask ("delayedTaskExec()", 20, true);
}

// Kill all delayed tasks with the given "this" value (if given) and the given function.

function killDelayedTasks()
{
	var fn = arguments[0];
	var self = $.global;
	if (typeof fn != "function")
	{
		self = fn;
		fn = arguments [1];
		if (typeof self != "object")
			self = $.global;
	}
	$.bp (typeof fn != "function");
	for (var i = 0; i < delayedTasks.length; )
	{
		var obj = delayedTasks [i];
		if( obj.fn == fn && obj.self == self )
			delayedTasks.splice (i, 1);
		else
			i++;
	}
	if (!delayedTasks.length && delayedTaskID)
	{
		app.cancelTask (delayedTaskID);
		delayedTaskID = 0;
	}
}

var delayedTasks  = [];
var delayedTaskID = -1;

function delayedTaskExec()
{
	var obj = delayedTasks[0];
	delayedTasks.shift();

	var fnstr = "";

	if( app.logFile )
	{
		fnstr = "[" + obj.fn.name + "] : ";
		
		var fo = obj.fn.toString().split("\n");

		for( var i=0; i<6; i++ )
		{
			if( fo[i] )
				fnstr += fo[i];
		}

		fnstr += " ...";

		app.writeLog( "Start DELAYED TASK: " + fnstr );
	}

	obj.fn.apply (obj.self, obj.args);

	if( app.logFile )
		app.writeLog( "Finished DELAYED TASK: " + fnstr );

	if (!delayedTasks.length && delayedTaskID)
	{
		app.cancelTask (delayedTaskID);
		delayedTaskID = 0;
	}
}

///////////////////////////////////////////////////////////////////
//
//	Scheduled Tasks
//	call addScheduledTask (time,foo) to add a function that is executed
//	after the passed time (msec) elapsed.
//  If the first argument is not a function, but the second is, the 
//  first argument is consider to be "this".
//
///////////////////////////////////////////////////////////////////

function addScheduledTask()
{
    var time = arguments[0];
	var fn = arguments[1];
	var self = $.global;
	var arg1 = 2;
	
	if( typeof fn != "function" )
	{
		self = fn;
		fn = arguments [2];
		arg1 = 3;
		
		if( typeof self != "object" )
			self = $.global;
	}
	
	$.bp( typeof fn != "function" );

	var args = [];
	for (i = arg1; i < arguments.length; i++)
		args.push (arguments [i]);

    nextScheduledTaskID++
	var taskData = { self:self, fn:fn, args:args, id:nextScheduledTaskID, tid:-1 };
	scheduledTasks[nextScheduledTaskID] = taskData;
	
	scheduledTasks[nextScheduledTaskID].tid = app.scheduleTask( "scheduledTaskExec(" + taskData.id + " )", time );

	return taskData.id;
}

function cancelScheduledTask( id, exe )
{
	if( scheduledTasks[id] )
	{
		var taskID = scheduledTasks[id].tid;

		if( taskID > -1 )
			app.cancelTask( taskID );

		if( exe )
			scheduledTaskExec( id );

		delete scheduledTasks[id];
	}
}

var scheduledTasks = {};
var nextScheduledTaskID = 1;

function scheduledTaskExec( id )
{
    var taskData = scheduledTasks[id];
    
    if( taskData )
    {
		var fnstr = "";

		if( app.logFile )
		{
			fnstr = "[" + taskData.fn.name + "] : ";
			
			var fo = taskData.fn.toString().split("\n");

			for( var i=0; i<6; i++ )
			{
				if( fo[i] )
					fnstr += fo[i];
			}

			fnstr += " ...";

			app.writeLog( "Start SCHEDULED TASK: " + fnstr );
		}

        taskData.fn.apply( taskData.self, taskData.args );
        delete scheduledTasks[id];

		if( app.logFile )
			app.writeLog( "Finished SCHEDULED TASK: " + fnstr );
    }
}

///////////////////////////////////////////////////////////////////////////////
//
// string utils
//

function stripWS( str )
{
    var tmp = stripLeadingWS( str );
    tmp     = stripTrailingWS( tmp );
    
    return tmp;
}

function stripLeadingWS( str )
{
    for( var i=0; i<str.length; i++ )
    {
        var ch = str.charAt(i);
        
        if( ch != ' ' && ch != '\t' && ch != '\n' )
        {
            if( i+1 >= str.length )
                return '';
            else
                return str.substring( i+1 );
        }
    }
    
    return str;
}

function stripTrailingWS( str )    
{
    for( var i=str.length-1; i>=0; i-- )
    {
        var ch = str.charAt(i);
        
        if( ch != ' ' && ch != '\t' && ch != '\n' )
        {
            if( i+1 >= str.length )
                return str;
            else
                return str.substr( 0, i+1 );
        }
    }
    
    return str;
}

//////////////////////////////////////////////////////////////////

function switchDbgLog( enable )
{
	for( var i in cdicMgr.cdic )
	{
		try
		{
			cdicMgr.cdic[i].customCall( 'dbgLog', false, enable ).submit();
		}
		catch(exc)
		{
		}
	}
}