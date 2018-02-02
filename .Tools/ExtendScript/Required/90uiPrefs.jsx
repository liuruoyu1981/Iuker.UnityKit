/**************************************************************************
*
*  @@@BUILDINFO@@@ 90uiPrefs-2.jsx 3.0.0.27  22-May-2008
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

var uiPrefs = { title : "$$$/ESToolkit/PreferencesDlg/UITitle=User Interface", sortOrder : 12 };

globalBroadcaster.registerClient( uiPrefs, 'initPrefPanes' );

uiPrefs.onNotify = function( reason )
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

uiPrefs.create = function (parentPane)
{
	var paneRes = """group {									
						orientation		: 'column',				
						visible			: false,				
						alignment		: 'fill',		
						dock			: Panel					
						{										
							orientation		: 'column',			
							alignment		: 'fill',		
							alignChildren	: 'left',	
							text		    : '$$$/ESToolkit/PreferencesDlg/uiDockTitle=Panel docking position', 
							dockLeft			: Checkbox		
							{									
								text			: '$$$/ESToolkit/PreferencesDlg/uiDockLeft=Left'
							},									
							dockRight			: Checkbox		
							{									
								text			: '$$$/ESToolkit/PreferencesDlg/uiDockRight=Right'
							},									
							dockTop			: Checkbox		
							{									
								text			: '$$$/ESToolkit/PreferencesDlg/uiDockTop=Top'
							},									
							dockBottom			: Checkbox		
							{									
								text			: '$$$/ESToolkit/PreferencesDlg/uiDockBottom=Bottom'
							}
						},""";
							
	if( !_win )
		paneRes += """mode		: Panel
					  {
							orientation		: 'column',			
							alignment		: ['fill','top'],		
							alignChildren	: 'left',	
							text		    : '$$$/ESToolkit/PreferencesDlg/uiModeTitle=Display mode', 
							frame			: Checkbox		
							{									
								text			: '$$$/ESToolkit/PreferencesDlg/uiApplicationFrame=Application Frame', 
								helpTip			: '$$$/ESToolkit/PreferencesDlg/htUIApplicationFrame=Use a main window instead of having floating windows.' 
							}
					   },""";
					
    paneRes += """colors		: Panel
					  {
							orientation		: 'column',			
							alignment		: ['fill','top'],		
							alignChildren	: 'left',	
							text		    : '$$$/ESToolkit/PreferencesDlg/uiColorTitle=Brightness', 
							brightness		: Slider		
							{		
							    properties      :   { minvalue : 0, maxvalue : 100 },							
								helpTip			: '$$$/ESToolkit/PreferencesDlg/htUIBrightness=Set the brightness of the User Interface.' 
							}
					   },dummy01         : Group
                  {
                      preferredSize   : [15,22],
                      alignment       : ['fill','top']
                  },
                  reset      : Button
                  {
				      text			: '$$$/ESToolkit/PreferencesDlg/uiReset=Reset Dialogs', 
					  helpTip		: '$$$/ESToolkit/PreferencesDlg/htUIReset=Reset all "Don\\'t show again" Dialogs.' 
				  }
			    }""";

	this.pane = parentPane.add ( paneRes );
    
	this.pane.prefsObj = this;

	this.loaded = false;
	return this.pane;
}

uiPrefs.preProcess = function()
{
	if (!this.loaded)
		this.load();

    if( !_win )		
	    this.pane.mode.frame.value = ( prefs.Workspace.useMainFrame == "" ? true : prefs.Workspace.useMainFrame.getValue( Preference.BOOLEAN ) );
}
uiPrefs.postProcess = function()
{}

uiPrefs.toDefault = function()
{
    this.pane.dock.dockLeft.value    = true;
    this.pane.dock.dockRight.value   = true;
    this.pane.dock.dockTop.value     = true;
    this.pane.dock.dockBottom.value  = true;

    if( !_win )
        this.pane.mode.frame.value	 = true;

    if( _win )
        this.pane.colors.brightness.value = prefs.UIBrightness.win.getDefault( Preference.NUMBER );
    else
        this.pane.colors.brightness.value = prefs.UIBrightness.mac.getDefault( Preference.NUMBER );
        
    workspace.brightness = this.pane.colors.brightness.value;
    
    this.resetDialogs();
}

/////////////////////////////////////////////////////////////////////////
// Load prefs into the general prefs pane

uiPrefs.load = function()
{
 	if( this.loaded )
		return;
	
	this.loaded = true;

	with( this.pane ) 
	{
	    reset.onClick = function()
	    {
	        uiPrefs.resetDialogs();
	    }
	    
	    colors.brightness.onChanging = function()
	    {
	        workspace.brightness = this.value;
	    }
	    
	    var dockPrefs = this.getDockPrefs();
	    //
	    // there shouldn't be any default for Workspace prefs, so we have to 
	    // take another approach to find out if there are any preference values
	    //
        dock.dockLeft.value    = dockPrefs[0];
        dock.dockRight.value   = dockPrefs[1];
        dock.dockTop.value     = dockPrefs[2];
        dock.dockBottom.value  = dockPrefs[3];
        
        colors.brightness.value = prefs.UIBrightness;
	}
}

/////////////////////////////////////////////////////////////////////////
// set preferences from the selected general options
uiPrefs.store = function()
{
	var nextTimeNeeded = false;

	if( this.loaded )
	{
		with (this.pane) 
		{
		    var oldDockPrefs = this.getDockPrefs();
		    
			nextTimeNeeded	= ( ( dock.dockLeft.value != oldDockPrefs[0] )	||
								( dock.dockRight.value != oldDockPrefs[1] ) ||
								( dock.dockTop.value != oldDockPrefs[2] )	||
								( dock.dockBottom.value != oldDockPrefs[3] )	);
		
			prefs.Workspace.CDockLeft	= dock.dockLeft.value;
			prefs.Workspace.CDockRight	= dock.dockRight.value;
			prefs.Workspace.CDockTop	= dock.dockTop.value;
			prefs.Workspace.CDockBottom	= dock.dockBottom.value;
			
			prefs.UIBrightness          = colors.brightness.value;
			
			if( !_win )
			    prefs.Workspace.useMainFrame = mode.frame.value;
		}
	}

	return nextTimeNeeded;
}

uiPrefs.cancelled = function()
{
    workspace.brightness = this.pane.colors.brightness.value = prefs.UIBrightness;
}

uiPrefs.getDockPrefs = function()
{
    var ret = [ true, true, true, true ];
    
    //
    // there shouldn't be any default for Workspace prefs, so we have to 
    // take another approach to find out if there are any preference values
    //
    ret[0] = prefs.Workspace.CDockLeft.getValue( Preference.STRING ).length > 0 ?
             prefs.Workspace.CDockLeft.getValue( Preference.BOOLEAN ) : true;
    ret[1] = prefs.Workspace.CDockRight.getValue( Preference.STRING ).length > 0 ?
             prefs.Workspace.CDockRight.getValue( Preference.BOOLEAN ) : true;
    ret[2] = prefs.Workspace.CDockTop.getValue( Preference.STRING ).length > 0 ?
             prefs.Workspace.CDockTop.getValue( Preference.BOOLEAN ) : true;
    ret[3] = prefs.Workspace.CDockBottom.getValue( Preference.STRING ).length > 0 ?
             prefs.Workspace.CDockBottom.getValue( Preference.BOOLEAN ) : true;
             
    return ret;
}

uiPrefs.resetDialogs = function()
{
    var len = prefs.DSDialog.getLength();

    for( var i=0; i<len; i++ )
        prefs.DSDialog[i] = "";
}
