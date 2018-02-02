/**************************************************************************
*
*  @@@BUILDINFO@@@ 01startup-2.jsx 3.0.0.27  22-May-2008
*  ADOBE SYSTEMS INCORPORATED
*  Copyright 2010 Adobe Systems Incorporated
*  All Rights Reserved.
* 
* NOTICE:  Adobe permits you to use, modify, and distribute this file in accordance
* with the terms of the Adobe license agreement accompanying it.  If you have 
* received this file from a source other than Adobe, then your use, modification, or 
* distribution of it requires the prior written permission of Adobe.
**************************************************************************/

// This global variable reflects whether prefs have been loaded or not during startup.

var startupPrefsLoaded = false;

// This global variable reflects whether we were launched by another application (via IDEBackend::launchIDE) or not

var remoteLaunched = false;

// A method to load the menu keys from the menuKeys variable

menus.loadKeys = function()
{
    this.loadShortcutKeys( menus, '' );
}

menus.loadShortcutKeys = function( menu, keyName )
{
    for( var name in menu )
    {
        if( menu[name] instanceof MenuElement )
        {
            if( menu[name].type == 'command' )
                menus.setShortcutKey( menu[name] );
            else
                menus.loadShortcutKeys( menu[name], ( keyName.length > 0 ? keyName + '.' + name : name ) );
        }
    }
}

menus.setShortcutKey = function( item )
{
    var prefsKey = item.id.replace( /\//g, '_' );
    
    if( !prefs.shortcutkeys.hasValue( prefsKey, Preference.STRING ) )
        prefsKey += '/' + ( $.os.indexOf( 'Windows' ) > -1 ? 'win' : 'mac' );

    var shortcutKey =  '';

    if( prefs.shortcutkeys.hasValue( prefsKey, Preference.STRING ) )
    {
        shortcutKey = prefs.shortcutkeys.getValue( prefsKey, Preference.STRING );
		
		if( shortcutKey == 'none' )
			shortcutKey = '';
    }    
    
    if( shortcutKey.length > 0 || item.shortcutKey.length > 0 )
        item.shortcutKey = shortcutKey;
}

///////////////////////////////////////////////////////////////////////////////
//
// check targets
//

app.scheduleTargetChecker = function( seconds )
{
    if( !seconds )
        seconds = 2;

    this.checkTargetsTaskDelay = seconds;
    
    if( this.checkTargetsTaskID )
        this.cancelTask( this.checkTargetsTaskID );

    this.checkTargetsTaskID = this.scheduleTask( "app.suspendTargetChecker();targetMgr.checkConnections();app.resumeTargetChecker();", seconds * 1000, false );
}

app.suspendTargetChecker = function()
{
    if( this.checkTargetsTaskID )
        this.cancelTask( this.checkTargetsTaskID );
}

app.resumeTargetChecker = function()
{
    this.scheduleTargetChecker( this.checkTargetsTaskDelay );
}

///////////////////////////////////////////////////////////////////////////////
//
// startup
//

app.onStartup = function( shift, remote, docFile )
{
	// disable application UI during startup
	app.enabled = false;
	
	startupPrefsLoaded  = !shift;
	remoteLaunched      = remote;

    globalBroadcaster.registerClient( app, 'shutdown,preferencesDialog,appFrameChanged' );

    // initialize the lang object
    lang.initialize();
	// initialize workspaces
	workspace.initialize();
	
	cdicMgr   = new CDICManager();
    targetMgr = new TargetManager();
    docMgr    = new DocumentManager( createSourceDocument );
    globalBroadcaster.notifyClients( 'registerDocumentTypes' );
	
    favorites = new Favorites();
    panes     = new Panes();

	// Set the current folder to Adobe Scripts
	var f = Folder (Folder.myDocuments + "/Adobe Scripts");
	this.currentFolder = String (f.exists ? f : f.parent);
    
	if( !shift )
	{
        app.loadPrefs();
        favorites.readPrefs();
    }
    
    // broadcasts message to create panes
    panes.init();    
    this.createFR( "after:'none'", shift, remoteLaunched );
	
	//
	// setup OMV UI if APE is installed
	//
	if(app.hasApe) {
		OMV.setupUI();
		addDelayedTask( OMV.initializeUI );
	}

	// appbar
	if( !_win )
	{
		var offsetY						   = ( workspace.appbar.size.height - 22 ) / 2;
		workspace.appbar.btn			   = workspace.appbar.add( "iconbutton", [ 0, offsetY, 22, 22 + offsetY ] );
		workspace.appbar.btn.iconToCompact = ScriptUI.newImage( '#SwitchToCompact_R', '#SwitchToCompact_N', undefined, '#SwitchToCompact_O' );
		workspace.appbar.btn.iconToFull	   = ScriptUI.newImage( '#SwitchToFull_R', '#SwitchToFull_N', undefined, '#SwitchToFull_O' );
		workspace.appbar.btn.tipToFull	   = localize( "$$$/ESToolkit/A/turnOffAppFrame=Turn Off Application Frame" );
		workspace.appbar.btn.tipToComapct  = localize( "$$$/ESToolkit/A/turnOnAppFrame=Turn On Application Frame" );
		
		workspace.appbar.btn.icon		   = ( workspace.appFrame ? workspace.appbar.btn.iconToFull : 
																	workspace.appbar.btn.iconToCompact );
		workspace.appbar.btn.helpTip	   = ( workspace.appFrame ? workspace.appbar.btn.tipToFull : 
																	workspace.appbar.btn.tipToComapct) ;
		
		workspace.appbar.btn.onClick = function()
		{
			workspace.appFrame = !workspace.appFrame;
			globalBroadcaster.notifyClients( 'appFrameChanged' );
		}
		
		workspace.appbar.btn.update = function()
		{
			this.icon	 = ( workspace.appFrame ? this.iconToFull : this.iconToCompact );
			this.helpTip = ( workspace.appFrame ? this.tipToFull : this.tipToComapct );
		}
	}
	
	// workspace brightness
    if( !prefs.UIBrightness.hasValue() )
    {
        if( _win )
            prefs.UIBrightness = prefs.UIBrightness.win.getDefault();
        else
            prefs.UIBrightness = prefs.UIBrightness.mac.getDefault()
    }
    
    workspace.brightness = prefs.UIBrightness.getValue( Preference.NUMBER );
    
    // init targets
    targetMgr.loadTargets();

    // start checking targets
    this.scheduleTargetChecker();

    // show main window 
    addDelayedTask( app.showMainWindow );

    // open initial document when the UI is up and running
    if( docFile )
		this.initialDocument = docFile;
	
	this.remoteLaunched = remoteLaunched;
	
	// initiate workspace
	addDelayedTask (app.initWorkspace);

    globalBroadcaster.notifyClients( 'startup', shift, remoteLaunched );

	// start running any late startup tasks that have been scheduled
	// during startup
	startDelayedTasks();

	// enable application UI after startup
	app.enabled = true;
	
    return true;
}

app.showMainWindow = function()
{
    if( Folder.fs == 'Windows' && workspace.mainWindow && !workspace.mainWindow.visible )
        workspace.mainWindow.show();
}

app.initWorkspace = function()
{
    app.showMainWindow();

    // workspace visibility
    workspaceVisible = !prefs.startup.hideWS.getValue( Preference.BOOLEAN );
    
    if( !workspaceVisible )
        workspace.togglePalettes();    

	if( app.initialDocument )
	{
		docMgr.load( app.initialDocument );
	}
	else if( !app.remoteLaunched )
	{
		if( !prefs.startup.noDocs.getValue( Preference.BOOLEAN ) )
		{
			var docwin = null;
			
			if( prefs.startup.newDoc.getValue( Preference.BOOLEAN ) || !startupPrefsLoaded )
				docwin = docMgr.create();
			else if( prefs.startup.oldDocs.getValue( Preference.BOOLEAN ) )
			{
				if( !docMgr.readPrefs() )
					docwin = docMgr.create();
			}
			
			if( docwin && docwin.docObj )
				docMgr.activateDocument( docwin.docObj );
		}
	}
}
