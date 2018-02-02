/**************************************************************************
*
*  @@@BUILDINFO@@@ 86preferences-2.jsx 3.5.0.47	09-December-2009
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

var preferenceDialog = null;

var locales = [];
const cShortcutTitleLength = 35;    

// This object contains all color icons under the name of the color.

var colorIcons = null;

///////////////////////////////////////////////////////////////////////////////
//
// preference dialog
//

function PreferenceDialog()
{
    //
	// Initialize the color icons for the Fonts and Colors prefs if we don't have them yet
	//
	if (!colorIcons)
	{
		colorIcons = {};

		for (var i in colors)
			colorIcons [i] = ScriptUI.newImage ("#" + i);
	}

	/////////////////////////////////////////////////////////////////////////
	//
	// The preferences dialog resource
    //
	var pw = 500;
	
	var prefsDlgResSpec =
	"""prefdialog {										
		text			: '$$$/ESToolkit/PreferencesDlg/Title=ExtendScript Toolkit Preferences', 
		orientation		: 'column',					
		alignment		: ['fill','fill'],			
		alignChildren	: ['fill','fill'],			
        properties      : { name : 'preferences' }, 
		listAndPanels	: Group						
		{											
			orientation		: 'row',				
			selector		: ListBox				
			{										
				preferredSize	: [155,145],		
				alignment		: ['left','fill'],	
				margins			: [15,20,15,15]		
			},										
			prefs			: Group					
			{										
				orientation		: 'column',			
				alignment		: ['fill','fill'],	
				alignChildren	: ['fill','fill'],	
				panes			: Group				
				{									
					minimumSize		: ["""+pw+""", 500],	
					maximumSize		: ["""+pw+""", 500],	
					alignment		: ['left','top'], 
					orientation		: 'stack',		
				},									
				divider			: Panel				
				{									
					orientation		: 'row',		
					preferredSize	: [10,1],		
					maximumSize		: [1000,1],		
					alignment		: ['fill','bottom'],
				}									
			}										
		},											
		btns			: Group {					
			orientation		: 'row',	        	
			alignment		: ['right','bottom'],  	
			defBtn			: Button				
			{										
				text			: '$$$/ESToolkit/PreferencesDlg/DefaultBtn=Default' }, 
				space			: Group				
				{									
					minimumSize		: [30,1],		
					preferedSize	: [30,1]		
				},					            	
				okBtn			: Button			
				{									
					text			: '$$$/CT/ExtendScript/UI/OK=OK', 
					properties		: {				
						name			:'ok'		
					}								
				},						        	
				cancelBtn		: Button			
				{									
					text			 : '$$$/CT/ExtendScript/UI/Cancel=Cancel', 
					properties		: {				
						name			: 'cancel'	
					}								
				}						        	
			}										
		}											
	}""";

	this.prefsDlg = new Window (prefsDlgResSpec);
	
	this.prefsDlg.currentPane = null;

	// Each Prefs object needs the following methods:
	// prefsObj.create (parentPane) - create the pane and return the Pane object.
	// prefsObj.preProcess()		- preprocess the pane just before being shown.
	// prefsObj.layoutDone()		- (optional) informs the pane that the initial layout is done (optional)
	// prefsObj.postProcess()		- do any work just before the pane is being hidden.
	// prefsObj.store()				- store the preferences. Return true if the Next Time dialog is needed.
	// prefsObj.toDefault()			- set back to default values
	// prefsObj.cancelled()			- (optional) Prefs dialog has been cancelled

	//	Initialize the 'selector' list that chooses which preference pane to show
    function sortPanes( a, b )
    {
        if( !a.sortOrder )
            return 1;
        if( !b.sortOrder )
            return -1;
            
        if( a.sortOrder < b.sortOrder )
	        return -1;
        else if( a.sortOrder > b.sortOrder )
	        return 1;
        else
	        return 0;
    }

	var selections = [];
	globalBroadcaster.notifyClients( 'initPrefPanes', selections );
	selections.sort( sortPanes );
	
	this.prefsDlg.listAndPanels.selector.stackPane = this.prefsDlg.listAndPanels.prefs.panes;
	
	//
	//	Load the list with preference pane titles
	//
	for (var i = 0; i < selections.length; i++) 
	{
		var sel = selections [i];
		item = this.prefsDlg.listAndPanels.selector.add ("item", localize(sel.title));
		item.prefsObject = sel;
		item.pane = null;
	}
	
	/*	When a list item is selected, hide the currently visible preference pane
		and show the one corresponding to the selected item. */
	this.prefsDlg.listAndPanels.selector.onChange = function ()
	{
		if (this.selection)
		{
			var created = false;
			if (!this.selection.pane)
			{
				created = true;
				app.setWaitCursor (true);
				var pane = this.selection.prefsObject.pane = this.selection.prefsObject.create (this.stackPane);
				pane.prefsObj = this.selection.prefsObject;
				this.selection.pane = pane;
				if( this.selection.prefsObject.preProcess )
				    this.selection.prefsObject.preProcess();
				this.window.layout.layout(true);
				if (this.selection.prefsObject.layoutDone)
					this.selection.prefsObject.layoutDone();
				app.setWaitCursor (false);
			}
			var paneToShow = this.selection.pane;
			if (this.window.currentPane)
			{
			    if( this.window.currentPane.prefsObj.postProcess )
				    this.window.currentPane.prefsObj.postProcess();
				this.window.currentPane.hide();
			}
			if (!created && paneToShow.prefsObj.preProcess)
				paneToShow.prefsObj.preProcess();
			paneToShow.show();
			this.window.currentPane = paneToShow;
		}
	}

	with( this.prefsDlg.btns ) 
	{
		//	Dialog window is 2 levels in hierarchy up from the buttons
		okBtn.onClick = function () 
		{	
			// Make sure to post-process the current pane
			if( this.window.currentPane && this.window.currentPane.prefsObj.postProcess )
				this.window.currentPane.prefsObj.postProcess();
		
			if( documents.length > 0 )
			{
				for( var i=0; i<documents.length; i++ )
				{
					var langID          = documents[i].langID;
					documents[i].langID = null;
					documents[i].setLanguage( langID );
				}
			}
			
			// close dialog
			this.window.close(1);
		}
		
		cancelBtn.onClick = function () 
		{ 
			// close dialog
			this.window.close(2); 
		}
		
		defBtn.onClick = function()
		{
			if( this.window.currentPane )
				this.window.currentPane.prefsObj.toDefault();
		}
	}
}

/////////////////////////////////////////////////////////////////////////
//	configureDialogFromPrefs()
//
//	Initialize dialog settings from the saved 'preferenceDialogPrefs' values.
//	Called on each 'show' of the preferences dialog.
//
PreferenceDialog.prototype.configureDialogFromPrefs = function()
{
	//	Location unknown: center over the main window
	this.prefsDlg.center();
	
	//	Show the last known preferences pane
	this.prefsDlg.listAndPanels.selector.selection = null;
	this.prefsDlg.listAndPanels.selector.selection = isNaN( parseInt(prefs.preferencesDialogPrefs.selectedPane,10) ) ? 0 : parseInt(prefs.preferencesDialogPrefs.selectedPane,10);
	this.prefsDlg.listAndPanels.selector.currPane = this.prefsDlg.listAndPanels.selector.selection.pane;
} // configureDialogFromPrefs

/////////////////////////////////////////////////////////////////////////
//	updateDialogPrefs()
//	Update the 'preferenceDialogPrefs' object with current dialog settings.
PreferenceDialog.prototype.updateDialogPrefs = function()
{		
	//	Remember what prefences pane was selected
	if(this.prefsDlg.listAndPanels.selector.selection) {
		prefs.preferencesDialogPrefs.selectedPane = this.prefsDlg.listAndPanels.selector.selection.index;
	}
} // updateDialogPrefs	

PreferenceDialog.prototype.show = function()
{
    return this.prefsDlg.show();
}

function openPreferencesDialog( initState )
{
//    if( !preferenceDialog )
        preferenceDialog = new PreferenceDialog();
	
	/////////////////////////////////////////////////////////////////////////
	//
	// Configure the dialog, show it, and set any changed preferences
	//
	preferenceDialog.configureDialogFromPrefs();

	if( initState && initState == "UI" )
	{
		for( var p=0; p<preferenceDialog.prefsDlg.listAndPanels.selector.items.length; p++ )
		{
			if( preferenceDialog.prefsDlg.listAndPanels.selector.items[p].text == localize( "$$$/ESToolkit/PreferencesDlg/UITitle=User Interface" ) )
			{
				preferenceDialog.prefsDlg.listAndPanels.selector.selection = p;
				preferenceDialog.prefsDlg.listAndPanels.selector.currPane = preferenceDialog.prefsDlg.listAndPanels.selector.selection.pane;
				break;
			}
		}
	}

	var ok = preferenceDialog.show() == 1;
	preferenceDialog.updateDialogPrefs ();

	if( ok ) 
    {
        //
		// call store() only if there actually is a loaded pane
		//
        var nextTimeNeeded  = false;
        
        for( var i=0; i<preferenceDialog.prefsDlg.listAndPanels.selector.items.length; i++ )
        {
            nextTimeNeeded |= ( preferenceDialog.prefsDlg.listAndPanels.selector.items[i].prefsObject.pane      ?
                                preferenceDialog.prefsDlg.listAndPanels.selector.items[i].prefsObject.store()   :
                                false                                                               );
        }
		
		if( nextTimeNeeded )
			// tell the user that the settings are delayed if needed
			messageBox( "$$$/ESToolkit/PreferencesDlg/NewSettingsNextTime=The new settings will take effect the next time the program starts." );
			
		globalBroadcaster.notifyClients( 'newPrefs' );
	}
	else
	{
	    //
		// call canceled() only if there actually is a loaded pane
		//
        for( var i=0; i<preferenceDialog.prefsDlg.listAndPanels.selector.items.length; i++ )
        {
            if( preferenceDialog.prefsDlg.listAndPanels.selector.items[i].prefsObject.pane &&
                preferenceDialog.prefsDlg.listAndPanels.selector.items[i].prefsObject.cancelled )
            {
                preferenceDialog.prefsDlg.listAndPanels.selector.items[i].prefsObject.cancelled();
            }
        }
	}
}

// Set the text of a list item by using two texts and a dot position for the second text

ListItem.prototype.setTabbedText = function (text1, tab, text2)
{
	// First, set the width of small and large spaces
	var list = this.parent;
	if (!list.spaceWidth)
	{
		list.smallSpace			= " ";//String.fromCharCode (" "); // Tahoma does not support small-width spaces (0x200A);
		list.ellipsis			= String.fromCharCode (0x2026);
		list.ellipsisWidth		= list.graphics.measureString (list.ellipsis)[0];
		list.spaceWidth			= list.graphics.measureString (" ")[0];
		list.smallSpaceWidth	= list.graphics.measureString (list.smallSpace)[0];
	}
	// strip trailing "..."
	var i = text1.lastIndexOf ("...");
	if (i > 0)
		text1 = text1.substr (0, i);

	var width = list.graphics.measureString (text1)[0];
	var maxWidth = tab - list.ellipsisWidth - 5 * list.smallSpaceWidth;
	var halfSpace = 0.5 * list.smallSpaceWidth;
	if (width > (maxWidth + halfSpace))
	{
		// Need to shrink
		while (width > maxWidth)
		{
			text1 = text1.substr (0, text1.length-1);
			width = list.graphics.measureString (text1)[0];
		}
		// append Unicode ellipsis string
		text1 += list.ellipsis;
		width  = list.graphics.measureString (text1)[0];
	}
	
	// First, approach with spaces
	var oldWidth = width;
	while (width < tab)
	{
		text1 += " ";
		width += list.spaceWidth;
	}
	
	// Then, fine adjust
	width = list.graphics.measureString (text1)[0];
	if (width > tab && width > (oldWidth + halfSpace))
	{
//		print (tab + " " + list.graphics.measureString (text1)[0] + " Fixing " + text1);
		text1 = text1.substr (0, text1.length-1);
		width = list.graphics.measureString (text1)[0];
	}
	while (width < (tab - halfSpace))
	{
		text1 += list.smallSpace;
		width = list.graphics.measureString (text1)[0];
	}
//	print (tab + " " + list.graphics.measureString (text1)[0] + " " + text1);
	this.text = text1 + text2;
}

