/**************************************************************************
*
*  @@@BUILDINFO@@@ 100expoint-2.jsx 3.0.0.14  27-February-2008
*  ADOBE SYSTEMS INCORPORATED
*  Copyright 2010 Adobe Systems Incorporated
*  All Rights Reserved.
* 
* NOTICE:  Adobe permits you to use, modify, and distribute this file in accordance
* with the terms of the Adobe license agreement accompanying it.  If you have 
* received this file from a source other than Adobe, then your use, modification, or 
* distribution of it requires the prior written permission of Adobe.
**************************************************************************/

/*
    This files contains functions for extending the ExtendScript Toolkit.
    
    The functions below gives access to the application scripts and the
    infrastructure implemented by these scripts
*/

expoint = null;

function ExPoint()
{
    //-----------------------------------------------------------------------------
    // 
    // registerForBroadcast(...)
    // 
    // Purpose: Register the passed object for the list of breadcast messages.
    //          The passed object needs to implement the method 'onNotify' to
    //          receive broadcast messages.
    //          Broadcast messages are identified by a string that needs to be
    //          passed for registering. If registering for multiple broadcast
    //          messages, then separat each identifier by a comma.
    //
    //          E.g.:
    //          expoint.registerBroadcast( myObj, 'msg1,msg2,msg3' );
    // 
    //-----------------------------------------------------------------------------

    this.registerBroadcast = function( obj, messages )
    {
        try
        {
            globalBroadcaster.registerClient( obj, messages );
        }
        catch( ex )
        {}
    }

    //-----------------------------------------------------------------------------
    // 
    // addPane(...)
    // 
    // Purpose:     Create a new dockable pane and add to the Windows menu.
    //          
    // Parameters:  name            - a unique name
    //              title           - title string (ZStrings permitted)
    //              menuTitle       - title string of menu item (ZStrings permitted)
    //              iconDefault     - default icon if pane gets iconified (path string)
    //              iconRollover    - rollover icon if pane gets iconified (path string)
    //              contentResource - resource string of the content of the pane
    //              defaultLocation - {TBD}
    //
    //-----------------------------------------------------------------------------

    this.addPane = function( name, title, menuTitle, iconDefault, iconRollover, contentResource, defaultLocation )
    {
        var pane = null;
        
        try
        {
            pane = panes.createPane( name, title, menuTitle, iconDefault, iconRollover, contentResource, defaultLocation );
        }
        catch( ex )
        {}
        
        return pane;
    }
    
    //-----------------------------------------------------------------------------
    // 
    // registerDocument(...)
    // 
    // Purpose: Register a custom document with a factory function and a comma separated
	//			list of filename extensions or a comma separated list of language identifier
    // 
    //-----------------------------------------------------------------------------

    this.registerDocument = function( factoryFct, extension, language )
    {
		if( this.registerDocs )
		{
			var docObj = { factory : factoryFct, ext : extension, lang : language };
			this.customDocs.push( docObj );
		}

		return this.registerDocs;
    }
    
    //-----------------------------------------------------------------------------
    // 
    // getTargets(...)
    // 
    // Purpose: Returns an array of all available TargetInfo objects.
    // 
    //-----------------------------------------------------------------------------

    this.getTargets = function()
    {
        var ret = [];
        
        if( targetMgr )
            ret = targetMgr.targetsInfo;
            
        return ret;
    }
    
    //-----------------------------------------------------------------------------
    // 
    // getCurrentTarget(...)
    // 
    // Purpose: Return the current active TargetInfo object.
    // 
    //-----------------------------------------------------------------------------

    this.getCurrentTarget = function()
    {
        var ret = null;
        
        if( targetMgr )
            ret = targetMgr.getActiveTarget();
            
        return ret;
    }
    
    //-----------------------------------------------------------------------------
    // 
    // setCurrentTarget(...)
    // 
    // Purpose: Set the current active TargetInfo object.
    // 
    //-----------------------------------------------------------------------------

    this.setCurrentTarget = function( targetInfo )
    {
        var ret = null;
        
        if( targetMgr && targetInfo instanceof TargetInfo )
            ret = targetMgr.setActive( targetInfo );
            
        return ret;
    }
    
    //-----------------------------------------------------------------------------
    // 
    // getSessions(...)
    // 
    // Purpose: Return array of all available Session objects of the current
    //          active TargetInfo.
    // 
    //-----------------------------------------------------------------------------

    this.getSessions = function()
    {
        var ret = null;
        
        if( targetMgr )
        {
            var ct = targetMgr.getActiveTarget();
            
            if( ct )
                ret = ct.sessions
        }
            
        return ret;
    }
    
    //-----------------------------------------------------------------------------
    // 
    // getCurrentSessions(...)
    // 
    // Purpose: Return current active Session object.
    // 
    //-----------------------------------------------------------------------------

    this.getCurrentSession = function()
    {
        var ret = null;
        
        if( targetMgr )
            ret = targetMgr.getActiveSession();
            
        return ret;
    }
    
    //-----------------------------------------------------------------------------
    // 
    // setCurrentSessions(...)
    // 
    // Purpose: Set current active Session object.
    // 
    //-----------------------------------------------------------------------------

    this.setCurrentSession = function( debugSession )
    {
        var ret = null;
        
        if( targetMgr && debugSession instanceof DebugSession )
        {
            var t = targetMgr.getActiveTarget();
            targetMgr.setActive( t, debugSession );
        }
           
        return ret;
    }
    
    //-----------------------------------------------------------------------------
    // 
    // dbgRun(...)
    // 
    // Purpose: Start debug session of current active Session object.
    // 
    //-----------------------------------------------------------------------------

    this.dbgRun = function()
    {
        menus.debug.run.onSelect();
    }
    
    //-----------------------------------------------------------------------------
    // 
    // dbgStop(...)
    // 
    // Purpose: Stop debug session of current active Session object.
    // 
    //-----------------------------------------------------------------------------

    this.dbgStop = function()
    {
        menus.debug.stop.onSelect();
    }
    
    //-----------------------------------------------------------------------------
    // 
    // dbgPause(...)
    // 
    // Purpose: Pause debug session of current active Session object.
    // 
    //-----------------------------------------------------------------------------

    this.dbgPause = function()
    {
        menus.debug.pause.onSelect();
    }
    
    //-----------------------------------------------------------------------------
    // 
    // dbgStepOver(...)
    // 
    // Purpose: Execute current line of current active Session object.
    // 
    //-----------------------------------------------------------------------------

    this.dbgStepOver = function()
    {
        menus.debug.over.onSelect();
    }
    
    //-----------------------------------------------------------------------------
    // 
    // dbgStepInto(...)
    // 
    // Purpose: Execute current line by step into function call (if there is a the
    //          current line) of current active Session object.
    // 
    //-----------------------------------------------------------------------------

    this.dbgStepInto = function()
    {
        menus.debug.into.onSelect();
    }
    
    //-----------------------------------------------------------------------------
    // 
    // dbgStepOut(...)
    // 
    // Purpose: Execute current line by step out of function call (if currently
    //          executing a function) of current active Session object.
    // 
    //-----------------------------------------------------------------------------

    this.dbgStepOut = function()
    {
        menus.debug.out.onSelect();
    }
    
    //-----------------------------------------------------------------------------
    // 
    // enabledDbgRun(...)
    // 
    // Purpose: Return the enabled state of the debugging command "run"
    // 
    //-----------------------------------------------------------------------------

    this.enabledDbgRun = function()
    {
        menus.debug.reflectState();
        return menus.debug.run.enabled;
    }
    
    //-----------------------------------------------------------------------------
    // 
    // enabledDbgStop(...)
    // 
    // Purpose: Return the enabled state of the debugging command "stop"
    // 
    //-----------------------------------------------------------------------------

    this.enabledDbgStop = function()
    {
        menus.debug.reflectState();
        return menus.debug.stop.enabled;
    }
    
    //-----------------------------------------------------------------------------
    // 
    // enabledDbgPause(...)
    // 
    // Purpose: Return the enabled state of the debugging command "pause"
    // 
    //-----------------------------------------------------------------------------

    this.enabledDbgPause = function()
    {
        menus.debug.reflectState();
        return menus.debug.pause.enabled;
    }
    
    //-----------------------------------------------------------------------------
    // 
    // enabledDbgStepOver(...)
    // 
    // Purpose: Return the enabled state of the debugging command "step over"
    // 
    //-----------------------------------------------------------------------------

    this.enabledDbgStepOver = function()
    {
        menus.debug.reflectState();
        return menus.debug.over.enabled;
    }
    
    //-----------------------------------------------------------------------------
    // 
    // enabledDbgStepInto(...)
    // 
    // Purpose: Return the enabled state of the debugging command "step into"
    // 
    //-----------------------------------------------------------------------------

    this.enabledDbgStepInto = function()
    {
        menus.debug.reflectState();
        return menus.debug.into.enabled;
    }
    
    //-----------------------------------------------------------------------------
    // 
    // enabledDbgStepOut(...)
    // 
    // Purpose: Return the enabled state of the debugging command "step out"
    // 
    //-----------------------------------------------------------------------------

    this.enabledDbgStepOut = function()
    {
        menus.debug.reflectState();
        return menus.debug.out.enabled;
    }    

	///////////////////////////////////////////////////////////////////////////////
	//
	// PRIVATE
	//

	this.registerDocs	= true;		// is it still possible to register custom documents
	this.customDocs		= [];

	this.onNotify = function( msg )
	{
		switch( msg )
		{
			case 'registerDocumentTypes':
			{
				for( var i=0; i<this.customDocs.length; i++ )
					docMgr.registerFactory( this.customDocs[i].factory, this.customDocs[i].ext, this.customDocs[i].lang );

				this.registerDocs = false;
			}
			break;
		}
	}

	globalBroadcaster.registerClient( this, 'registerDocumentTypes' );
}

expoint = new ExPoint();