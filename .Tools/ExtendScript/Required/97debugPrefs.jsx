/**************************************************************************
*
*  @@@BUILDINFO@@@ 97debugPrefs-2.jsx 3.0.0.14  27-February-2008
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

var debugPrefs = { title : "$$$/ESToolkit/PreferencesDlg/debuggerTitle=Debugging", sortOrder : 40 };

globalBroadcaster.registerClient( debugPrefs, 'initPrefPanes' );

debugPrefs.onNotify = function( reason )
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

debugPrefs.create = function (parentPane)
{
	this.pane = parentPane.add (
	"""group {
		orientation		: 'column',
		alignChildren	: 'fill',
		visible			: false,
		alignment		: 'fill',
		dataBrowser		: Panel 
		{
			text			: '$$$/ESToolkit/Panes/Variables/Title=Display in Data Browser',
			alignChildren	: 'fill',
			alignment		: 'fill',
			showUndefined	:	Checkbox 
			{
				text			: '$$$/ESToolkit/PreferencesDlg/Debugger/ShowUndefined=&Undefined Variables'
			},
			showFunctions	:	Checkbox 
			{
				text			: '$$$/ESToolkit/PreferencesDlg/Debugger/ShowFunctions=&Functions'
			},
			showCore		:	Checkbox 
			{
				text			: '$$$/ESToolkit/PreferencesDlg/Debugger/ShowCore=&Core JavaScript Elements'
			},
			showPrototype	:	Checkbox 
			{
				text			: '$$$/ESToolkit/PreferencesDlg/Debugger/ShowPrototype=All &Prototype Elements'
			},
			fields			: Group 
			{
				orientation		: 'column',
				alignment		: 'fill',
				maxElements			: Group 
				{
					orientation		: 'row',
					alignment		: 'fill',
					alignChildren	: ['left','center'],
					lbl				: StaticText 
					{
						text			: '$$$/ESToolkit/PreferencesDlg/Debugger/MaxElements1=&Display Up To:'
					},
					edit			: EditText 
					{
						preferredSize	: [40, 20]
					},
					lbl2			: StaticText 
					{
						text			: '$$$/ESToolkit/PreferencesDlg/Debugger/MaxElements2=Array Elements'
					},
				},
				maxChars			: Group 
				{
					orientation		: 'row',
					alignment		: 'fill',
					alignChildren	: ['left','center'],
					lb3				: StaticText 
					{
						text			: '$$$/ESToolkit/PreferencesDlg/Debugger/MaxElements1=&Display Up To:'
					},
					editChar		: EditText 
					{
						preferredSize	: [40, 20]
					},
					lbl4			: StaticText 
					{
						text			: '$$$/ESToolkit/PreferencesDlg/Debugger/MaxElements3=Character'
					},
				}
			}
		},
		targetInteraction	: Panel 
		{
			text	: '$$$/ESToolkit/Panes/TargetInt/Title=Target Interaction',
			alignChildren	: 'fill',
			alignment		: 'fill',
			autolaunch		: Checkbox				
			{										
				text			: '$$$/ESToolkit/PreferencesDlg/autolaunch=Auto-Launch Application when Setting Target'
			},										
			toFront	:	Checkbox 
			{
				text			: '$$$/ESToolkit/PreferencesDlg/Debugger/ToFront=&Bring target application to front'
			}
		}
	}""");

	this.pane.showUndefined = this.pane.dataBrowser.showUndefined;
	this.pane.showCore		= this.pane.dataBrowser.showCore;
	this.pane.showFunctions	= this.pane.dataBrowser.showFunctions;
	this.pane.showPrototype	= this.pane.dataBrowser.showPrototype;
	this.pane.maxElementsEdit = this.pane.dataBrowser.fields.maxElements.edit;
	this.pane.maxCharacterEdit = this.pane.dataBrowser.fields.maxChars.editChar;
	this.pane.autolaunch	  = this.pane.targetInteraction.autolaunch;
	this.pane.toFront		  = this.pane.targetInteraction.toFront;
	this.loaded = false;
	this.pane.prefsObj = this;
	this.preProcess = function()
	{
		if( !this.loaded )
			this.load();
	}
	this.postProcess = function()
	{}

    this.pane.maxElementsEdit.onChange = function()
    {
		var max = isFinite( this.text ) ? parseInt( this.text, 10 ) : NaN;
        
        if( isNaN( max ) || max < 1 || max > 1000 )
            this.text = prefs.databrowser.maxArrayElements.getValue( Preference.NUMBER );
    }
    this.pane.maxCharacterEdit.onChange = function()
    {
		var max = isFinite( this.text ) ? parseInt( this.text, 10 ) : NaN;
        
        if( isNaN( max ) || max < 1 || max > 5000 )
            this.text = prefs.databrowser.maxStringLength.getValue( Preference.NUMBER );
    }

	this.toDefault = function()
	{
		this.pane.showUndefined.value    = prefs.databrowser.showUndefined.getDefault( Preference.BOOLEAN );
		this.pane.showFunctions.value    = prefs.databrowser.showFunctions.getDefault( Preference.BOOLEAN );
		this.pane.showCore.value	     = prefs.databrowser.showCore.getDefault( Preference.BOOLEAN );
		this.pane.showPrototype.value    = prefs.databrowser.showPrototype.getDefault( Preference.BOOLEAN );
		this.pane.maxElementsEdit.text   = prefs.databrowser.maxArrayElements.getDefault( Preference.NUMBER );
		this.pane.maxCharacterEdit.text	 = prefs.databrowser.maxStringLength.getDefault( Preference.NUMBER );
	    this.pane.autolaunch.value       = prefs.autolaunch.getDefault( Preference.BOOLEAN );
	    this.pane.toFront.value			 = prefs.toFront.getDefault( Preference.BOOLEAN );
	}

	return this.pane;
}

debugPrefs.load = function()
{
 	if (this.loaded)
		return;
	this.loaded = true;

	with (this.pane) 
	{
		showUndefined.value  = prefs.databrowser.showUndefined.getValue( Preference.BOOLEAN );
		showFunctions.value  = prefs.databrowser.showFunctions.getValue( Preference.BOOLEAN );
		showCore.value	     = prefs.databrowser.showCore.getValue( Preference.BOOLEAN );
		showPrototype.value	 = prefs.databrowser.showPrototype.getValue( Preference.BOOLEAN );
		maxElementsEdit.text = prefs.databrowser.maxArrayElements.getValue( Preference.NUMBER );
		maxCharacterEdit.text= prefs.databrowser.maxStringLength.getValue( Preference.NUMBER );
		autolaunch.value     = prefs.autolaunch.getValue( Preference.BOOLEAN );
		toFront.value        = prefs.toFront.getValue( Preference.BOOLEAN );
	}
}

debugPrefs.store = function()
{
	if (!this.loaded)
		return false;

	var ret = false;

	with (this.pane) 
	{
		var n = Number (maxElementsEdit.text);
		if (n < 0)	n = 0;
		prefs.databrowser.maxArrayElements	= n;

		n = Number (maxCharacterEdit.text);
		if (n < 0)	n = 0;
		prefs.databrowser.maxStringLength	= n;

		prefs.databrowser.showUndefined		= (showUndefined.value == 1);
		prefs.databrowser.showFunctions		= (showFunctions.value == 1);
		prefs.databrowser.showCore			= (showCore.value == 1);	
		prefs.databrowser.showPrototype		= (showPrototype.value == 1);	

		prefs.autolaunch					= autolaunch.value;

		ret = ( prefs.toFront.getValue( Preference.BOOLEAN ) != toFront.value );

		prefs.toFront						= toFront.value;
	}
	globalBroadcaster.notifyClients( 'newDebugPrefs' );
	
	return ret;
}
