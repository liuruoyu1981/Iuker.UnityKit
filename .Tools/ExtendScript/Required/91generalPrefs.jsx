/**************************************************************************
*
*  @@@BUILDINFO@@@ 91generalPrefs-2.jsx 3.0.0.14  27-February-2008
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

var generalPrefs = { title : "$$$/ESToolkit/PreferencesDlg/StartupTitle=Startup", sortOrder : 10 };

globalBroadcaster.registerClient( generalPrefs, 'initPrefPanes' );

generalPrefs.onNotify = function( reason )
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

generalPrefs.create = function (parentPane)
{
	this.pane = parentPane.add (
	"""group {									
		orientation		: 'column',				
		alignChildren	: 'left',				
		visible			: false,				
		alignment		: ['left','top'],		
		startup			: Group					
		{										
			orientation		: 'column',			
			alignChildren	: 'left',			
			opt1			: RadioButton		
			{									
			    text			: '$$$/ESToolkit/PreferencesDlg/startNewDoc=Open New &Document'
			},									
			opt2			: RadioButton		
			{									
			    text			: '$$$/ESToolkit/PreferencesDlg/startLastDoc=Open Last &Open Documents'
			},									
			opt3			: RadioButton		
			{									
			    text			: '$$$/ESToolkit/PreferencesDlg/startNoDoc=&No Document'
            }									
        },										
        remrec		: Checkbox				
		{										
		    text			: '$$$/ESToolkit/PreferencesDlg/remRecent=&Remove unresolveable recent files entries.', 
            helpTip			: '$$$/ESToolkit/PreferencesDlg/htRemRecent=Remove a file from the recent files list if it can not be resolved, e.g unresolveable network path.' 
        },										
        recent			: Group					
		{										
            orientation		: 'row',			
            lbr				: StaticText		
			{									
				text			: '$$$/ESToolkit/PreferencesDlg/maxRecent=Max.entries of recent files:' 
			},									
			rmax		: EditText	            
			{				
			    characters      : 4,					
				helpTip			: '$$$/ESToolkit/PreferencesDlg/htMaxRecent=The maximum number of entries of the recent files list'
			}									
		},										
        locale			: Group					
		{										
            orientation		: 'row',			
            lbl				: StaticText		
			{									
				text			: '$$$/ESToolkit/PreferencesDlg/languages=&Languages:' 
			},									
			list			: DropDownList		
			{									
				helpTip			: '$$$/ESToolkit/PreferencesDlg/htLanguage=Select the language to use in the user interface'
			}									
		}										
	}""");

	this.pane.locale.list.minimumSize.width = 120;
/*	if (_win)
	{
		var fontSize = this.pane.locale.list.graphics.font.size;
		// Make sure to select a Unicode font for Japanese and other languages
		this.pane.locale.list.graphics.font = ScriptUI.newFont( "Arial Unicode MS", fontSize );
	}
*/

    this.pane.recent.rmax.onChange = function()
    {
		var max = isFinite( this.text ) ? parseInt( this.text, 10 ) : NaN;
        
        if( isNaN( max ) || max < 1 || max > 20 )
            this.text = prefs.recent.max.getValue( Preference.NUMBER );
    }
    
	this.pane.prefsObj = this;

	this.loaded = false;
	return this.pane;
}

generalPrefs.preProcess = function()
{
	if (!this.loaded)
		this.load();
}
generalPrefs.postProcess = function()
{}

generalPrefs.toDefault = function()
{
    this.pane.startup.opt1.value    = prefs.startup.newDoc.getDefault( Preference.BOOLEAN );
    this.pane.startup.opt2.value    = prefs.startup.oldDocs.getDefault( Preference.BOOLEAN );
    this.pane.startup.opt3.value    = prefs.startup.noDocs.getDefault( Preference.BOOLEAN );
    this.pane.remrec.value          = prefs.recent.remunres.getDefault( Preference.BOOLEAN );
    this.pane.recent.rmax.text      = prefs.recent.max.getDefault( Preference.NUMBER );
    this.pane.locale.list.selection = 0;
}

/////////////////////////////////////////////////////////////////////////
// Load prefs into the general prefs pane

generalPrefs.load = function()
{
 	if (this.loaded)
		return;
	this.loaded = true;

	if( locales.length == 0 )
    {
        //
        // take only estk dat files
        //
		var datFiles = Folder( app.requiredFolder ).getFiles( '??_??.dat' );

        for( var i=0; i<datFiles.length; i++ )
        {
			var f = datFiles[i];
			if (f.open())
			{
				// Search for "$$$/ESToolkit/A/Language=XXXX"
				var name = null;
				for (var j = 0; !name && j < 10; j++)
				{
					var text = f.readln();
					name = /"\$\$\$\/ESToolkit\/A\/Language=([^"]+)"/.exec (text);
				}
				f.close();
				if (name)
				{
					var locale = datFiles[i].name.substr( 0, 5 );
		            locales.push( [ locale, name[1] ] );
				}
			}
        }
        
        locales.sort();
    }
    
	with (this.pane) 
	{
	    startup.opt1.value    = prefs.startup.newDoc.getValue( Preference.BOOLEAN );
	    startup.opt2.value    = prefs.startup.oldDocs.getValue( Preference.BOOLEAN );
	    startup.opt3.value    = prefs.startup.noDocs.getValue( Preference.BOOLEAN );
	    
        remrec.value          = prefs.recent.remunres.getValue( Preference.BOOLEAN );
        recent.rmax.text      = prefs.recent.max.getValue( Preference.NUMBER );

	    var selItem = null;
	    
		var curLocale = prefs.locale.getValue( Preference.STRING );

        locale.list.removeAll();
		var item = locale.list.add( 'item', localize( "$$$/ESToolkit/PreferencesDlg/DefaultLanguage=Default" ) );
		item.locale = "";
		selItem = item;
		locale.list.add( 'separator', "" );

		for( var i=0; i<locales.length; i++ )
	    {
	        var item = locale.list.add( 'item', locales[i][1] );
	        item.locale = locales[i][0];
	        
	        if( item.locale == curLocale )
	            selItem = item;
	    }
	    
	    locale.list.selection = selItem;
	}
}

/////////////////////////////////////////////////////////////////////////
// set preferences from the selected general options
generalPrefs.store = function()
{
	var nextTimeNeeded = false;
	if (this.loaded)
	{
		with (this.pane) 
		{
			prefs.startup.newDoc    = startup.opt1.value;
			prefs.startup.oldDocs   = startup.opt2.value;
			prefs.startup.noDocs    = startup.opt3.value;
            prefs.recent.remunres   = remrec.value;
            prefs.recent.max        = recent.rmax.text;
		    
			if( locale.list.selection )
			{
				nextTimeNeeded = (prefs.locale != locale.list.selection.locale);
				prefs.locale = locale.list.selection.locale;
			}
		}
	}
	return nextTimeNeeded;
}
