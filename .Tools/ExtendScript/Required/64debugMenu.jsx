/**************************************************************************
*
*  @@@BUILDINFO@@@ 64debugMenu-2.jsx 3.5.0.17	16-March-2009
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

//////////////////////////////////////////////////////////////////////////
// The Debug menu.

menus.debug				= new MenuElement ("menu", "$$$/ESToolkit/Menu/Debug=&Debug", 
										   "at the end of menubar", "debug");
menus.debug.run			= new MenuElement ("command", "$$$/ESToolkit/Menu/Debug/Run=&Run",
										   "at the end of debug",  "debug/run");
menus.debug.stop		= new MenuElement ("command", "$$$/ESToolkit/Menu/Debug/Stop=&Stop",
										   "at the end of debug",  "debug/stop");
menus.debug.pause		= new MenuElement ("command", "$$$/ESToolkit/Menu/Debug/Break=&Break", 
										   "at the end of debug", "debug/break");
menus.debug.into		= new MenuElement ("command", "$$$/ESToolkit/Menu/Debug/StepInto=Step &Into",
										   "at the end of debug",  "debug/into");
menus.debug.over		= new MenuElement ("command", "$$$/ESToolkit/Menu/Debug/StepOver=Step &Over",
										   "at the end of debug",  "debug/over");
menus.debug.out			= new MenuElement ("command", "$$$/ESToolkit/Menu/Debug/StepOut=Step Ou&t",
										   "at the end of debug",  "debug/out");
menus.debug.reset	    = new MenuElement ("command", "$$$/ESToolkit/Menu/Debug/Reset=Reset",
										   "--at the end of debug",  "debug/reset");
menus.debug.bptoggle	= new MenuElement ("command", "$$$/ESToolkit/Menu/Debug/ToggleBP=To&ggle Breakpoint",
										   "--at the end of debug",  "debug/bptoggle");
menus.debug.bpclearall	= new MenuElement ("command", "$$$/ESToolkit/Menu/Debug/ClearAllBP=&Clear All Breakpoints",
										   "at the end of debug",  "debug/bpclearall");
menus.debug.noErrors	= new MenuElement ("command", "$$$/ESToolkit/Menu/Debug/DontBreakOnErrors=Do Not Break on Guarded &Exceptions", 
											"--at the end of debug", "debug/noerrors");

globalBroadcaster.registerClient( menus.debug, 'shutdown' );
globalBroadcaster.notifyClients( 'updateMenu_Debug' );

//											
// handle Debugger broadcasts
// /registered in 72debugger.jsx)
//
menus.debug.onNotify = function( reason, param01, param02, param03 )											
{
    switch( reason )
    {
        case 'shutdown':
            DebugSession.unregisterClient( this );
            globalBroadcaster.unregisterClient( this );
            break;
            
        case 'state':   
            this.reflectState(); 
            break;
    }
}

// Disable the Debug menu altogether

menus.debug.disable = function()
{
	this.run.enabled		=
	this.into.enabled		=
	this.over.enabled		=
	this.out.enabled		=
	this.bptoggle.enabled	=
	this.bpclearall.enabled	=
	this.stop.enabled		=
	this.pause.enabled		= 
	this.noErrors.enabled   = false;
	
    if( docMgr.isActiveDocumentWin() &&document.rootPane.updateDebugButtonStates )
        document.rootPane.updateDebugButtonStates( true );
}

// Set the state of the debugging options according to the document settings

menus.debug.reflectState = function()
{
    var enableMenu = false;
    
    //
    // should debug menu be disabled?
    //
    var currSession = null;
    
    if( docMgr.isActiveDocumentWin() )
    {
		var currTarget  = ( document.getCurrentTarget ? document.getCurrentTarget() : undefined );
		currSession		= ( document.getCurrentSession ? document.getCurrentSession() : undefined );
        
        if( currTarget )
            enableMenu = ( ( !currTarget.getConnected() && currTarget.getFeature( Feature.COLD_START ) ) || currTarget.canDebug( currSession ) );
            
        if( currSession )
		{
			if( currSession.isStopping() && docMgr.related( currSession.document, document ) )
				enableMenu = false;
			else
				enableMenu = enableMenu || ( currSession.isDebugging() && docMgr.related( currSession.document, document ) );
		}
    }
//print( "debug.reflectState {"+(( docMgr.isActiveDocumentWin() )?document.scriptID:"nodoc")+"}: status="+(document?document.getStatus():"-")+
//       "   enableMenu="+enableMenu + " - target.canDebug()=" + document.getCurrentTarget().canDebug( document.getCurrentSession() ));
    if( enableMenu )
    {
		var running = false;
		var stopped = true;
		var deep = false;
		
		if( currSession )
		{
		    running = ( currSession.state == DebugSession.RUNNING );
		    stopped = ( currSession.state == DebugSession.STOPPED );
			
			deep = ( currSession.frame > 0 );
		}

		switch( document.getStatus() )
		{
			case "noexec":
				// not executable
				this.disable();
				break;

			case "exec":
				// standard executable
				this.run.enabled		= ( ( !running && currTarget.getFeature( Feature.START_EXECUTION, currSession ) ) || ( stopped && currTarget.getFeature( Feature.CONTINUE_EXECUTION, currSession ) ) );
				this.into.enabled		= ( !running || stopped ) && currTarget.getFeature( Feature.STEP_INTO, currSession );
				this.over.enabled		= ( !running || stopped ) && currTarget.getFeature( Feature.STEP_OVER, currSession );
				this.out.enabled		= deep && stopped && currTarget.getFeature( Feature.STEP_OUT, currSession );
				this.bptoggle.enabled	= true;
				this.bpclearall.enabled	= true;
				if(currSession) {
					this.stop.enabled		= ( ( running || stopped ) && currTarget.getFeature( Feature.STOP_EXECUTION, currSession ) ) || ( document ? ( document.badLine >= 0 ) : false );
				}
				this.pause.enabled		= running && currTarget.getFeature( Feature.PAUSE_EXECUTION, currSession );
				break;

			case "dynamic":
				// dynamic script, not saveable
				this.run.enabled		= stopped && currTarget.getFeature( Feature.CONTINUE_EXECUTION, currSession );
				this.into.enabled		= stopped && currTarget.getFeature( Feature.STEP_INTO, currSession );
				this.over.enabled		= stopped && currTarget.getFeature( Feature.STEP_OVER, currSession );
				this.out.enabled		= deep && stopped && currTarget.getFeature( Feature.STEP_OUT, currSession );
				this.bptoggle.enabled	= stopped;
				this.bpclearall.enabled	= stopped;
				if(currSession) {
					this.stop.enabled		= ( stopped && currTarget.getFeature( Feature.STOP_EXECUTION, currSession ) ) || ( document ? ( document.badLine >= 0 ) : false );
				}
				this.pause.enabled		= false;
				break;
		}
		
		document.rootPane.updateDebugButtonStates();
		this.noErrors.enabled = !running;
    }
	else
	{
	    this.disable();
	}
}

menus.debug.run.onDisplay		= 
menus.debug.stop.onDisplay		= 
menus.debug.pause.onDisplay		= 
menus.debug.into.onDisplay		= 
menus.debug.over.onDisplay		= 
menus.debug.out.onDisplay		= function()
{
	if( !document || ( document && !( document instanceof SourceDocument ) ) )
		menus.debug.reflectState();
}

menus.debug.run.onSelect = function()
{
	if( app.modalState ) return;

    var session = document ? document.getCurrentSession() : null;
    
    if( session && session.state != DebugSession.INACTIVE )
        session.command( DebugSession.CMD_CONTINUE );
    else if( document )
        DebugSession.prepareSession( document.getCurrentTarget(), 
                                     session, 
									 ExecutionContext.DBGLEVEL_BREAK, 
									 document, 
									 true );
}

menus.debug.into.onSelect = function()
{
	if( app.modalState ) return;

    var session = document ? document.getCurrentSession() : null;
        
    if( session && session.state != DebugSession.INACTIVE )
    {
        if( session.document && document && docMgr.related( session.document, document ) )
            session.document = document;
            
	    session.command( DebugSession.CMD_STEPINTO );
	}
    else if( document )
        DebugSession.prepareSession( document.getCurrentTarget(), 
									 session, 
									 ExecutionContext.DBGLEVEL_BREAKIMMIDEATE, 
									 document, 
									 true );
}

menus.debug.over.onSelect = function()
{
	if( app.modalState ) return;

    var session = document ? document.getCurrentSession() : null;
        
    if( session && session.state != DebugSession.INACTIVE )
    {
        if( session.document && document && docMgr.related( session.document, document ) )
            session.document = document;
            
	    session.command( DebugSession.CMD_STEPOVER );
	}
    else if( document )
        DebugSession.prepareSession( document.getCurrentTarget(), 
									 session, 
									 ExecutionContext.DBGLEVEL_BREAKIMMIDEATE, 
									 document, 
									 true );
}

menus.debug.out.onSelect = function()
{
	if( app.modalState ) return;

    var session = document ? document.getCurrentSession() : null;
        
    if( session && session.state != DebugSession.INACTIVE )
    {
        if( session.document && document && docMgr.related( session.document, document ) )
            session.document = document;
            
	    session.command( DebugSession.CMD_STEPOUT );
	}
}

menus.debug.pause.onSelect = function()
{
	if( app.modalState ) return;

    var session = document ? document.getCurrentSession() : null;
        
    if( session && session.isDebugging() )
    {
        if( session.document && document && docMgr.related( session.document, document ) )
            session.document = document;
            
	    session.command( DebugSession.CMD_PAUSE );
	    menus.debug.reflectState();
    }
}

menus.debug.stop.onSelect = function()
{
	if( app.modalState ) return;

    var session = document ? document.getCurrentSession() : null;
        
    if( session && session.isDebugging() )
    {
        if( session.document && document && docMgr.related( session.document, document ) )
            session.document = document;
            
        session.stop();
	    menus.debug.reflectState();
    }
    else if( document && document.isSourceDocument )
    {
        document.setBadline();
    }
}

menus.debug.reset.onDisplay = function()
{
	var enabled = false;
	
	if( document )
	{
		var currTarget  = ( document.getCurrentTarget ? document.getCurrentTarget() : undefined );
		var currSession = ( document.getCurrentSession ? document.getCurrentSession() : undefined );
        
        if( currTarget && currSession && !currSession.isDebugging() )
            enabled = currTarget.getFeature( Feature.RESET, currSession );
	}

	this.enabled = enabled;
}

menus.debug.bptoggle.onDisplay =  function()
{
	this.enabled = docMgr.isActiveDocumentWin();
	
	if( document )
	{
		var currTarget  = ( document.getCurrentTarget ? document.getCurrentTarget() : undefined );
		var currSession = ( document.getCurrentSession ? document.getCurrentSession() : undefined );
        
        if( currTarget )
            this.enabled = currTarget.getFeature( Feature.SET_BREAKPOINTS, currSession ) &&
                           currTarget.getFeature( Feature.REMOVE_BREAKPOINTS, currSession );
		else
			this.enabled = false;
	}
}

menus.debug.bpclearall.onDisplay = function()
{
	this.enabled = docMgr.isActiveDocumentWin();
	
	if( document )
	{
		var currTarget  = ( document.getCurrentTarget ? document.getCurrentTarget() : undefined );
		var currSession = ( document.getCurrentSession ? document.getCurrentSession() : undefined );
        
        if( currTarget )
            this.enabled = currTarget.getFeature( Feature.REMOVE_BREAKPOINTS, currSession );
		else
			this.enabled = false;
	}
}

menus.debug.reset.onSelect = function()
{
	if( app.modalState ) return;

	if( document )
	{
        var currSession = document.getCurrentSession();
        
        if( currSession )
            currSession.resetEngine();
	}
}

menus.debug.bptoggle.onSelect = function()
{
	if( app.modalState ) return;

    var editor = ( document ? document.getEditor() : null );
    
    if( editor )
    {
	    var line = editor.getSelection()[0];
	    document.toggleBreakpoint (line);
	}
}

menus.debug.bpclearall.onSelect = function()
{
	if( app.modalState ) return;

	document.removeAllBreakpoints();
}

menus.debug.noErrors.onDisplay = function()
{
	this.checked = prefs.debug.dontBreakOnErrors.getValue( Preference.BOOLEAN );
	this.enabled = docMgr.isActiveDocumentWin();
	
	if( document )
	{
		var currTarget  = ( document.getCurrentTarget ? document.getCurrentTarget() : undefined );
		var currSession = ( document.getCurrentSession ? document.getCurrentSession() : undefined );
        
        if( currTarget )
            this.enabled = currTarget.getFeature( Feature.DBGFLAG_DONTBREAK, currSession );
		else
			this.enabled = false;
	}
}

menus.debug.noErrors.onSelect = function()
{
	if( app.modalState ) return;

	prefs.debug.dontBreakOnErrors = !prefs.debug.dontBreakOnErrors.getValue( Preference.BOOLEAN );
}

//////////////////////////////////////////////////////////////////////////
// The Profile menu.
// Profile display mode: 0 - nothing, 1 - hits, 2 - time
// Profiling level:
// 0 - no profiling (default)
// 1 - function level profiling
// 2 - function level profiling with timing information
// 3 - line level profiling
// 4 - line level profiling with timing information.

menus.profile			= new MenuElement ("menu", "$$$/ESToolkit/Menu/Profile=&Profile",
										   "after debug", "profile");
menus.profile.off		= new MenuElement ("command", "$$$/ESToolkit/Menu/Profile/Off=&Off",
										   "at the end of profile", "profile/off");
menus.profile.functions	= new MenuElement ("command", "$$$/ESToolkit/Menu/Profile/Functions=&Functions",
										   "at the end of profile", "profile/functions");
menus.profile.lines		= new MenuElement ("command", "$$$/ESToolkit/Menu/Profile/Lines=&Lines",
										   "at the end of profile", "profile/lines");
//menus.profile.timing	= new MenuElement ("command", "$$$/ESToolkit/Menu/Profile/Timing=&Add Timing Info",
//										   "----at the end of profile", "profile/timing");
menus.profile.none		= new MenuElement ("command", "$$$/ESToolkit/Menu/Profile/None=&No Profile Data",
										   "----at the end of profile", "profile/none");
menus.profile.hits		= new MenuElement ("command", "$$$/ESToolkit/Menu/Profile/Hits=Show &Hit Count",
										   "at the end of profile", "profile/hits");
menus.profile.time		= new MenuElement ("command", "$$$/ESToolkit/Menu/Profile/Time=Show &Timing",
										   "at the end of profile", "profile/time");
menus.profile.clear		= new MenuElement ("command", "$$$/ESToolkit/Menu/Profile/Clear=&Clear Profile Data",
										   "----at the end of profile", "profile/clear");
menus.profile.save		= new MenuElement ("command", "$$$/ESToolkit/Menu/Profile/SaveAs=Save Profile Data &As...",
										   "----at the end of profile", "profile/save");

globalBroadcaster.notifyClients( 'updateMenu_Profile' );

menus.profile.off.onDisplay = function()
{
	this.checked = prefs.profiling.profileLevel.getValue( Preference.NUMBER ) == 0;
}

menus.profile.off.onSelect = function()
{
	if( app.modalState ) return;

	menus.profile.setLevel( ProfileData.LEVEL_NONE );
}

menus.profile.functions.onDisplay = function()
{
    var level = prefs.profiling.profileLevel.getValue( Preference.NUMBER );
	
	this.enabled = true;
	
	if( document )
	{
		var currTarget  = ( document.getCurrentTarget ? document.getCurrentTarget() : undefined );
		var currSession = ( document.getCurrentSession ? document.getCurrentSession() : undefined );
        
        if( currTarget && ( !document.profileLevel || ( document.profileLevel && document.profileLevel != ProfileData.LEVEL_LINES ) ) )
            this.enabled = currTarget.getFeature( Feature.PROFILE_FUNCTION, currSession );
		else
			this.enabled = false;

		if( level != 0 && document.profileLevel && document.profileLevel == ProfileData.LEVEL_FUNCTIONS )
			menus.profile.setLevel( ProfileData.LEVEL_FUNCTIONS );
	}

	// This needs to be done after checking document properties!
	this.checked = (level == 1 || level == 2);
}

menus.profile.functions.onSelect = function()
{
	if( app.modalState ) return;

	menus.profile.setLevel( ProfileData.LEVEL_FUNCTIONS );
}

menus.profile.lines.onDisplay = function()
{
    var level = prefs.profiling.profileLevel.getValue( Preference.NUMBER );

	this.enabled = true;
	
	if( document )
	{
		var currTarget  = ( document.getCurrentTarget ? document.getCurrentTarget() : undefined );
		var currSession = ( document.getCurrentSession ? document.getCurrentSession() : undefined );
        
        if( currTarget && ( !document.profileLevel || ( document.profileLevel && document.profileLevel != ProfileData.LEVEL_FUNCTIONS ) ) )
            this.enabled = currTarget.getFeature( Feature.PROFILE_LINE, currSession );
		else
			this.enabled = false;

		if( level != 0 && document.profileLevel && document.profileLevel == ProfileData.LEVEL_LINES )
			menus.profile.setLevel( ProfileData.LEVEL_LINES );
	}

	// This needs to be done after checking document properties!
	this.checked = (level == 3 || level == 4);
}

menus.profile.lines.onSelect = function()
{
	if( app.modalState ) return;

	menus.profile.setLevel( ProfileData.LEVEL_LINES );
}

menus.profile.none.onDisplay = function()
{
	this.enabled = ( prefs.profiling.profileLevel.getValue( Preference.NUMBER ) != 0 && (!document || !document.composing));
	this.checked = ( prefs.profiling.profDisplayMode.getValue( Preference.NUMBER ) == 0);
}

menus.profile.none.onSelect = function()
{
	if( app.modalState ) return;

	prefs.profiling.profDisplayMode = 0;
	docMgr.updateProfData();
}

menus.profile.hits.onDisplay = function()
{
	this.enabled = ( prefs.profiling.profileLevel.getValue( Preference.NUMBER ) != 0 && (!document || !document.composing));
	this.checked = ( prefs.profiling.profDisplayMode.getValue( Preference.NUMBER ) == 1);
	
	if( document )
	{
		var currTarget  = ( document.getCurrentTarget ? document.getCurrentTarget() : undefined );
		var currSession = ( document.getCurrentSession ? document.getCurrentSession() : undefined );
        
        if( currTarget )
            this.enabled = currTarget.getFeature( Feature.PROFILE_HITS, currSession );
		else
			this.enabled = false;
	}
}

menus.profile.hits.onSelect = function()
{
	if( app.modalState ) return;

	prefs.profiling.profDisplayMode = 1;
	docMgr.updateProfData();
}

menus.profile.time.onDisplay = function()
{
	this.enabled = ( prefs.profiling.profileLevel.getValue( Preference.NUMBER ) > 0 ) && (!document || !document.composing);
	this.checked = ( prefs.profiling.profDisplayMode.getValue( Preference.NUMBER ) == 2);
	
	if( document )
	{
		var currTarget  = ( document.getCurrentTarget ? document.getCurrentTarget() : undefined );
		var currSession = ( document.getCurrentSession ? document.getCurrentSession() : undefined );
        
        if( currTarget )
            this.enabled = currTarget.getFeature( Feature.PROFILE_TIME, currSession );
		else
			this.enabled = false;
	}
}

menus.profile.time.onSelect = function()
{
	if( app.modalState ) return;

	prefs.profiling.profDisplayMode = 2;
	docMgr.updateProfData();
}

menus.profile.clear.onDisplay = function()
{
	this.enabled = (!document || !document.composing);
}

menus.profile.clear.onSelect = function()
{
	if( app.modalState ) return;

	docMgr.clearProfileData();
}

menus.profile.save.onDisplay = function()
{
	this.enabled = ( prefs.profiling.profileLevel.getValue( Preference.NUMBER ) > 0);
}

menus.profile.save.onSelect = function()
{
	if( app.modalState ) return;

	docMgr.saveProfData();
}

menus.profile.setLevel = function (level)
{
    if( prefs.profiling.profDisplayMode.getValue( Preference.NUMBER ) > 0 )
        docMgr.setProfiling(level);
	
	prefs.profiling.profileLevel = level;
}
