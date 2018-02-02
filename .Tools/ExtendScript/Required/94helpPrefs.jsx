/**************************************************************************
*
*  @@@BUILDINFO@@@ 94helpPrefs-2.jsx 3.0.0.14  27-February-2008
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

var helpPrefs = { title : "$$$/ESToolkit/PreferencesDlg/helpTitle=Help", sortOrder : 50 };

globalBroadcaster.registerClient( helpPrefs, 'initPrefPanes' );

helpPrefs.onNotify = function( reason )
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

helpPrefs.create = function (parentPane)
{
	this.pane = parentPane.add (
		"""group {
				orientation		: 'column',
				alignChildren	: 'left',
				visible			: false,
				alignment		: ['left','top'],
				dynamicHelp		: Checkbox
				{
					text			: '$$$/ESToolkit/PreferencesDlg/dynamichelp=&Display JavaScript Variables'
				},
				omvWin			: Checkbox
				{
					text			: '$$$/ESToolkit/PreferencesDlg/omvwin=Display &Object Model Viewer in a floating window'
				},
				autoComplete	: Checkbox
				{
					text			: '$$$/ESToolkit/PreferencesDlg/autocomplete=&Enable Auto Completion'
				},
				fields			: Group 
				{
					orientation		: 'column',
					alignment		: 'fill',
					alignChildren	: 'left',
					auto			: Group 
					{
						orientation		: 'row',
						alignChildren	: ['right','center'],
						lbl				: StaticText 
						{
							text			: '$$$/ESToolkit/PreferencesDlg/AutoComplete=&Auto-Completion Delay:'
						},
						edit			: EditText 
						{
							preferredSize	: [100, 20]
						},
						lbl2			: StaticText 
						{
							text			: '$$$/ESToolkit/PreferencesDlg/Seconds=seconds'
						}
					}
				}
		}""");

	this.pane.autoEdit	 = this.pane.fields.auto.edit;

	this.pane.autoEdit.preferredSize.width = 50;

	this.pane.autoComplete.autoGrp = this.pane.fields.auto;
	this.pane.autoComplete.onClick = function()
	{
		this.autoGrp.lbl.enabled	=
		this.autoGrp.lbl2.enabled	=
		this.autoGrp.edit.enabled	= (this.value != 0);
	}

    this.pane.autoEdit.onChange = function()
    {
		var max = isFinite( this.text ) ? parseFloat( this.text, 10 ) : NaN;

        if( isNaN( max ) || max < 0.1 || max > 30 )
            this.text = prefs.autotime.getValue( Preference.NUMBER );
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
		this.pane.dynamicHelp.value  = prefs.dynamicHelp.getDefault( Preference.BOOLEAN );
		this.pane.omvWin.value		 = ( prefs.omv.win.getDefault( Preference.STRING ) == 'floating' );
		this.pane.autoComplete.value = prefs.autocompletion.getDefault( Preference.BOOLEAN );
		var i = prefs.autotime.getDefault( Preference.NUMBER );
		this.pane.autoEdit.text = i;
		// Set the enabled state of the autocomplete time field
		this.pane.autoComplete.onClick();
	}

	return this.pane;
}

helpPrefs.load = function()
{
	if (this.loaded)
		return;
	this.loaded = true;

	with (this.pane) 
	{
		//	Load the current help preferences
		dynamicHelp.value  = prefs.dynamicHelp.getValue( Preference.BOOLEAN );
		omvWin.value	   = ( prefs.omv.win.getValue( Preference.STRING ) == 'floating' );
		autoComplete.value = prefs.autocompletion.getValue( Preference.BOOLEAN );
		var i = prefs.autotime.getValue( Preference.NUMBER );
		autoEdit.text = i;
		// Set the enabled state of the autocomplete time field
		autoComplete.onClick();
	}
}

helpPrefs.store = function()
{
	if( !this.loaded )
		return false;
	
	var ret = false;

	with( this.pane ) 
	{
		prefs.dynamicHelp = dynamicHelp.value;

		if( !dynamicHelp.value )
			globalBroadcaster.notifyClients( 'dynamicHelpPrefsChanged' );

		var old = prefs.omv.win.getValue( Preference.STRING );
		prefs.omv.win	  = ( omvWin.value ?  'floating' : 'docked' );

		ret = ( old != prefs.omv.win );

		var n = Number (autoEdit.text);
		var autoChanged =  (autoComplete.value != prefs.autocompletion.getValue( Preference.BOOLEAN ))
						|| (n > 0 && n != prefs.autotime.getValue( Preference.NUMBER ));
		
		prefs.autocompletion = autoComplete.value;
		
		if (n > 0)
			prefs.autotime = n;
		
		if (autoChanged)
			globalBroadcaster.notifyClients ('autoCompletionPrefsChanged');
	}
	
	return ret;
}
