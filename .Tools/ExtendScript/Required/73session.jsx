/**************************************************************************
*
*  @@@BUILDINFO@@@ 73session-2.jsx 3.5.0.48	14-December-2009
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
// current active session
//
DebugSession.current = null;

//
// active sessions
//
DebugSession.activeCount = 0;

DebugSession.addActiveSession = function()
{
	if( DebugSession.activeCount == 0 )
		app.prioritized = true;
		
	DebugSession.activeCount++;
}

DebugSession.removeActiveSession = function()
{
	DebugSession.activeCount--;

	if( DebugSession.activeCount < 0 )
		DebugSession.activeCount = 0;

	if( DebugSession.activeCount <= 0 )
		app.prioritized = false;
}

//
// session state constants
//  
DebugSession.RUNNING			= localize ("$$$/ESToolkit/Toolbar/Engine/State/Running=running");
DebugSession.STOPPED			= localize ("$$$/ESToolkit/Toolbar/Engine/State/Stopped=stopped");
//TODO when using that?? DebugSession.WAITING			= localize ("$$$/ESToolkit/Toolbar/Engine/State/Waiting=waiting");
DebugSession.INACTIVE			= "";

//
// debugging commands
//
DebugSession.CMD_CONTINUE       = "$$$/ESToolkit/Debug/Command/Continue:continue";
DebugSession.CMD_PAUSE          = "$$$/ESToolkit/Debug/Command/Continue:pause";
DebugSession.CMD_STEPOVER       = "$$$/ESToolkit/Debug/Command/Continue:step over";
DebugSession.CMD_STEPINTO       = "$$$/ESToolkit/Debug/Command/Continue:step into";
DebugSession.CMD_STEPOUT        = "$$$/ESToolkit/Debug/Command/Continue:step out";

//
// error strings
//
const kMsgHaltedExecution = localize( "$$$/CT/ExtendScript/Errors/Err34=Execution halted" );

const kErrMsgEngineBusy	  = "ENGINE BUSY";

//
// error codes
//
const kErrEngineNotExists	= 57;
const kErrEngineHalted		= -34;
const kErrBadAction			= 32;

//
// max restarts
//
DebugSession.kRestartSessionMAX = 3;

///////////////////////////////////////////////////////////////////////////////
//
// Debugger broadcaster
//
DebugSession.broadcaster = new Broadcaster;

DebugSession.registerClient = function( clientObj, msg )
{ return DebugSession.broadcaster.registerClient( clientObj, msg ); }

DebugSession.unregisterClient = function( clientObj, msg )
{ DebugSession.broadcaster.unregisterClient( clientObj, msg ); }

DebugSession.notifyClients = function( reason, param01, param02, param03, param04 )
{ DebugSession.broadcaster.notifyClients( reason, param01, param02, param03, param04 ); }

// register for Debugger broadcasts
DebugSession.registerClient( menus.debug, 'state' );

//-----------------------------------------------------------------------------
// 
// Session.prepareSession(...)
// 
// Purpose: Prepare to start a session
// 
//-----------------------------------------------------------------------------

DebugSession.prepareSession = function( target, session, dbgLevel, doc, saveDocs )
{
    //
    // Save all docs
    //
    if( prefs.debug.saveBeforeDebug.getValue( Preference.BOOLEAN ) && saveDocs && !docMgr.saveAll(true) )
	    return;

	//
	// document backup
	//
	docMgr.forceBackup();

    //
    // include path set
    //
    var includePath = '';
    
    if( !doc.includePath || ( doc.includePath && doc.includePath.length == 0 ) )
    {
        var f = new File( doc.scriptID );
        
        if( f.exists )
			doc.includePath = f.parent ? f.parent.absoluteURI : "/";
    }
    
    //
    // check the syntax
    //
    if( !target.isESTarget || ( target.isESTarget && doc.checkSyntax( doc.includePath ) ) )
    {
		var engineName = '';
		
        if( target.isESTarget )
        {
            //
            // check for target directive
            //

		    // returns [targetName,engineName]
            var directive = app.scanForTargetDirective( doc.getText(), doc.scriptID );
            
            if( directive )
            {
                var targetName = directive[0];
                engineName	   = directive[1] ? directive[1] : '';
                
                if( targetName || engineName.length > 0 )
                {
					//
					// test portal
					//
					if( targetName == 'estoolkit#dbg' )
					{
                        var f = new File( doc.scriptID );
                        
                        if( f.exists )
						{
							var doexe = true;
							
							if( doc.isDirty() )
								doexe = confirm( "The document has been modified without saving these changes!\nExecute the script anyway?" );
							
							if( doexe )
								print( $.evalFile( f ) );
						}
						else
						    errorBox( "Only file based documents can be executed in the internal engine!" );
						    
						return;
					}
					else
					{
						var newAddr   = null;
						var newTarget = null;
						
						if( targetName )
						{
							//
							// ask CDIC to create a valid address for the targetName
							//
							try
							{
								var job = target.cdic.createAddress( targetName );
								var res = CDICManager.getSynchronousResult( cdicMgr.callSynchronous( job ) );

								if( res && res.length )
								{
									newAddr		= new Address ( res[0] );
									newTarget	= targetMgr.findTarget( newAddr );
								}
								else
								{
									//
									// target name is unknown, stop here
									//
									errorBox( localize( "$$$/CT/ExtendScript/Errors/Err2=%1 is undefined", ( '"' + targetName + '"' ) ) + "!" );
									return;
								}
							}
							catch( exc )
							{
								InternalError();
							}

							//
							// if the target of the #target directive is the same a the selected
							// target (in the UI) and if there is no #targetengine directive
							// then take the engine that was selcted in the UI
							//
							if( !engineName && 
								session		&&
								target == newTarget )
								engineName = session.address.engine;
						}
						else
						{
							//
							// if there's no target name then there has to be an engine name
							//
							newAddr		= new Address( target.address );
							newTarget	= target;
						}
						
						newAddr.engine = engineName;

						if( newTarget )
						{
							// do we have to switch the current target?
							var changeActive = ( newTarget != target );

							// set target for debug session
							target = newTarget;

							// find session for new address
							newSession  = newTarget.findSession( newAddr );
							
							if( newSession )
								changeActive = changeActive || ( newSession != session );

							// set property 'session' even if 'newSession' is undefined
							session = newSession;	
							
							// set documents target/engine popup
							if( changeActive )
							{
								if( !session && !engineName )
									session = target.getActive();

								targetMgr.setActive( target, session, false, true );
							}

							//
							// if all engines of a target are known, but there's no session
							// for the current engine name then add a new session for the engine 
							// name, the target might to create the new engine
							//
							if( target.getConnected() && !session )
							{
								var tmp = target.addSession( newAddr );
								tmp.temporary = true;
							}
						}
						else
						{
							target = targetMgr.addTarget( newAddr, false, true );
							session	  = null;
						}
					}
                }
            }
        }
        
        //
        // start session, create before if not available
        //
        var data = { engine : engineName, session : session, target : target, document : doc, debugLevel : dbgLevel, error : new ErrorInfo() };
        
        if( target.getConnected() )
            DebugSession.startSession( data );
        else
		{
			target.prepareDebugSession = true;
            target.connect( false, new Callback( DebugSession.startSession, data ), data.error );
		}
    }
}

//-----------------------------------------------------------------------------
// 
// DebugSession.startSession(...)
// 
// Purpose: Start given session
// 
//-----------------------------------------------------------------------------

DebugSession.startSession = function( data )
{
    var session = data.session;
	
    //
    // get session from TargetInfo if not in data
    //
    if( !session && data.target )
    {
		if( data.engine )
			session = data.target.findSessionEngine( data.engine );
		else
			session = data.target.getActive();

		//
		// If session does not exists then add the session and 
		// give it a try. If the target response with error
		// "Engine does not exists" then the session will be
		// removed.
		//
        if( !session && data.engine )
		{
			var addr			= new Address( data.target.address );
			addr.engine			= data.engine;
            session				= data.target.addSession( addr );
			session.temporary	= true;
		}
			
		if( session )
			data.target.setActive( session, false, true );
			
        session = data.target.activeSession;
    }
    
    //
    // finaly start the session
    //
	if( data.target && app.targetAppRunning( data.target ) )
	{
		if( session )
		{
			session.startInfo = null;
			session.startSession( data.document, data.debugLevel, session, data.error );
		}
		else
		{
			if( data.target )
				data.target.prepareDebugSession = false;

			try
			{
				if( data.error.length() <= 0 )
					data.error.push( localize( "$$$/ESToolkit/Error/NoEngine=Target %1 provides no engine for debugging.", data.target.getTitle() ) );
					
				data.error.push( localize( "$$$/ESToolkit/Error/CantExec=Cannot execute script." ) );
				data.error.display();        
			}
			catch( exc )
			{}
		}
	}
}

//-----------------------------------------------------------------------------
// 
// DebugSession(...)
// 
// Purpose: ctor
// 
//-----------------------------------------------------------------------------

function DebugSession( addr, target, sessionObj )
{
    this.address        = addr;                     		// session address
    this.target         = target;                   		// target of session
    this.sessionObj     = sessionObj;               		// cdi session object
    this.document       = null;                     		// current document in debug session
    this.documents      = {};                       		// documents of debug session
    this.dbgLevel       = ExecutionContext.DBGLEVEL_BREAK;	// current debug level for execution
    this.state          = DebugSession.INACTIVE;    		// current state of session
    this.line           = -1;                       		// current line of debugging
    this.lineColor      = undefined;                        // remember color at line
    this.frame          = -1;                       		// current active frame
    this.stack          = [];                       		// last known stack
    this.busyID         = null;                     		// ID of current busy animation
    this.error          = null;                     		// last execution error message
    this.model          = new SessionModel( this ); 		// current tree of variables
    this.profileLevel   = ProfileData.LEVEL_NONE;   		// profile level
	
	this.startInfo		= null;								// store related data for start debug session in order to restart the session if it failed

	this.temporary		= false;							// if true and the execution fails then this session will be removed

	this.releasing		= false;							// if true the sessionObj is about to be released. NO command should be sent to the engine!
    
    this.finalized      = false;                    		// session was finalized after execution
    this.finalizing     = false;                            // true if just finalizing the session after execution

	this.stopExe		= false;							// true when about to stop script execution
	this.stopTaskID		= -1;
    
    this.tmpSilentStop  = undefined;                        // true if the debugger should stop without any notification
    
	this.isInitialized	= false;							// DebugSession initialized?
    this.initializing	= true;								// about to be initialized

	DebugSession.registerClient( this , 'initialized' );
    
    if( this.sessionObj )
    {
        this.sessionObj.onTask      = this.processTasks;
        this.sessionObj.dbgSession  = this;
        this.initializing           = false;
        
        if( this.sessionObj.enabled )
            this.model.setupModel();
            
        DebugSession.notifyClients( 'initialized', this.target, this, true );
    }
    else
    {
        if( this.target && this.target.cdic )
        {
            //
            // get cdi session object and connect
            //
            try
            {
                var job = this.target.cdic.acquireSession( this.address );
                job.dbgSession = this;
                
				if( !this.sessionObj && this.initializing )
				{
					//
					// we could setup the sessionObject right after
					// creating the job
					//
					this.sessionObj            = job.sessionObject;
					this.sessionObj.onTask     = this.processTasks;
					this.sessionObj.dbgSession = this;
				}

                job.onResult = function()
                {
                    var sessionObj = this.sessionObject;
                    
                    if( sessionObj )
                    {
                        if( this.dbgSession.initializing )
                        {
                            this.dbgSession.sessionObj            = sessionObj;
                            this.dbgSession.sessionObj.onTask     = this.dbgSession.processTasks;
                            this.dbgSession.sessionObj.dbgSession = this.dbgSession;

						    if( this.dbgSession.target.getFeature( Feature.CONNECT, this.dbgSession ) )
						    {
							    try
							    {           
								    var job = this.dbgSession.sessionObj.connect();
								    job.dbgSession = this.dbgSession;
    	                            
								    job.onResult = function()
								    {
									    this.dbgSession.initializing = false;	
										
										if( this.dbgSession.temporary )
											this.dbgSession.model.setupModel();
											
									    DebugSession.notifyClients( 'initialized', this.dbgSession.target, this.dbgSession, true );
								    }
    	                            
								    job.onError =  job.onTimeout = function()
								    {
									    //
									    // error on connecting, release session
									    //
									    this.dbgSession.initializing = false;
									    this.dbgSession.release( true );
									    DebugSession.notifyClients( 'initialized', this.dbgSession.target, this.dbgSession, false );
								    }
    	                            
								    job.submit();
							    }
							    catch( exc )
							    {
								    //
								    // error on connecting, release session
								    //
								    this.dbgSession.initializing = false;
								    this.dbgSession.release( true );
								    DebugSession.notifyClients( 'initialized', this.dbgSession.target, this.dbgSession, false );
							    }
						    }
						    else
						    {
							    this.dbgSession.initializing = false;
								
								if( !this.dbgSession.temporary )
									this.dbgSession.model.setupModel();
									
							    DebugSession.notifyClients( 'initialized', this.dbgSession.target, this.dbgSession, true );
						    }
                        }
                        else if( sessionObj != this.dbgSession.sessionObj )
                            this.dbgSession.target.cdic.releaseSession( sessionObj ).submit();
                    }
                }
                
                job.onError = job.onTimeout = function()
                {
	                this.dbgSession.initializing = false;
	                this.dbgSession.release( true );
	                DebugSession.notifyClients( 'initialized', this.dbgSession.target, this.dbgSession, false );
                }
                
                job.submit();
            }
            catch( exc )
            {
                this.initializing = false;
                this.release( true );
                DebugSession.notifyClients( 'initialized', this.target, this, false );
            }
        }
    }
}

DebugSession.prototype.setSessionObj = function( sessionObj )
{
	if( !this.sessionObj || ( this.sessionObj && !this.sessionObj.enabled ) )
	{
		if( sessionObj && !this.releasing )
		{
			this.sessionObj             = sessionObj;
			this.initializing           = false;
			this.sessionObj.onTask      = this.processTasks;
			this.sessionObj.dbgSession  = this;
			
			if( this.sessionObj.enabled )
				this.model.setupModel();
				
			if( !this.isInitialized )
				DebugSession.notifyClients( 'initialized', this.target, this, true );
		}
	}
}

//-----------------------------------------------------------------------------
// 
// onNotify(...)
// 
// Purpose: Handle broadcast messages
// 
//-----------------------------------------------------------------------------

DebugSession.prototype.onNotify = function( reason )
{
	if( reason == 'initialized' )
		this.isInitialized = true;
}

//-----------------------------------------------------------------------------
// 
// DebugSession(...)
// 
// Purpose: Release session, stop debugging if active
// 
//-----------------------------------------------------------------------------

DebugSession.prototype.release = function( quiet )
{
    if( this.sessionObj && !this.releasing )
    {
		this.releasing = true;
	
        if( this.isDebugging() )
            this.stop( new Callback( this.doRelease, this ) );
        else
            this.doRelease();
    }
}
    
DebugSession.prototype.doRelease = function( thisObj )
{
    if( !thisObj )
        thisObj = this;
        
	if( thisObj.target && thisObj.target.getFeature( Feature.DISCONNECT, thisObj ) && thisObj.sessionObj )
	{
		var job = thisObj.sessionObj.disconnect();
		job.dbgSession = thisObj;
		job.onResult = job.onError = job.onTimeout = function()
		{
			try
			{
				var job = this.dbgSession.target.cdic.releaseSession( this.dbgSession.sessionObj );
				
				if( job )
				    job.submit();
			}
			catch( exc )
			{}
			
			this.dbgSession.sessionObj = null;
			this.dbgSession.address    = null;
			this.dbgSession.target     = null;
		}
        
		job.submit();
	}
	else
	{
		if( thisObj.target && thisObj.sessionObj )
		{
			var job = thisObj.target.cdic.releaseSession( thisObj.sessionObj );
			
			if( job )
				job.submit();
		}
		
		thisObj.sessionObj = null;
		thisObj.address    = null;
		thisObj.target     = null;
	}
    
    thisObj.model.erase();
}

//-----------------------------------------------------------------------------
// 
// initialized(...)
// 
// Purpose: Is the session initialized
// 
//-----------------------------------------------------------------------------

DebugSession.prototype.initialized = function()
{
    return ( this.sessionObj != null );
}

//-----------------------------------------------------------------------------
// 
// getModel(...)
// 
// Purpose: Return current model
// 
//-----------------------------------------------------------------------------

DebugSession.prototype.getModel = function( path )
{
    var ret = this.model.variables;
    
    if( path )
        ret = this.model.findVariable( path );
        
    return ret;
}

//-----------------------------------------------------------------------------
// 
// updateModel(...)
// 
// Purpose: Update model for given path
// 
//-----------------------------------------------------------------------------

DebugSession.prototype.updateModel = function( path )
{
	if( !this.releasing )
		this.model.updateModel( path );
}

//-----------------------------------------------------------------------------
// 
// getProfileData(...)
// 
// Purpose: Get profile data from engine
// 
//-----------------------------------------------------------------------------

DebugSession.prototype.getProfileData = function()
{
	if( !this.releasing )
	{
		try
		{
			var docs = {};
	        
			for( i in this.documents )
				docs[i] = this.documents[i];
	            
			var job         = this.sessionObj.getProfile();
			job.dbgSession  = this;
			job.docs        = docs;

			job.onResult = function()
			{
				var xml = this.result[0];
				var files = xml.children();

				for (var i = 0; i < files.length(); i++)
				{
					var file = files [i];
					var docname = file.@name.toString();

					if( docname.indexOf("(") != 0 || docname.lastIndexOf(")") != docname.length-1 )
					{
						var f = new File (file.@name);
						docname = f.absoluteURI;
					}

					// Use the global document find in case a script #included a doc that we have open
					var doc = docMgr.find (docname);

					if (!doc)
						continue;

					// ignore the functions level for now, and use all data records
					var lines = file..data;
					for (var j = 0; j < lines.length(); j++)
					{
						var data = lines [j];
						doc.addProfileData( this.dbgSession.profileLevel, { line:+data.@line-1, time:+data.@time, hits:+data.@hits } );
					}
				}            
	            
				docMgr.updateProfData();
			}
	            
			job.onError = function()
			{
	// TODO            
			}
	        
			job.submit();
		}
		catch( exc )
		{
	// TODO    
		}
	}
}

//-----------------------------------------------------------------------------
// 
// setDocument(...)
// 
// Purpose: Set current document in debug session
// 
//-----------------------------------------------------------------------------

DebugSession.prototype.setDocument = function( doc, noErrors )
{
	var color = colors.LightGreen;
	var errorMsg = "";

	if( this.frame == this.stack.length-1 && !noErrors )
	{
		color = colors.Yellow;

		if( this.error )
		{
			color = colors.Coral;
			errorMsg = this.error;
		}
	}
	
    docMgr.setStatusLine( errorMsg, doc );
	
	if( this.line >= 0 )
	{
	    // set color of current line in document
		doc.setLineStatus (this.line, color);
		
		this.lineColor = color;
		
		if( doc.isSourceDocument )
		{
		    // scroll to that line
		    doc.setSelection (this.line, 0, true);
		}
	}
	
	// Add the doc to my documents list if not present
	var docObj = this.documents[doc.scriptID];
	
	if( !docObj )
		this.documents[doc.scriptID] = docObj = doc;
		
	// bring the document to front
	if( this.document != doc )
	{
	    if( this.document && this.document.isSourceDocument  )
    		this.document.setLineStatus( this.line, 0xFFFFFF );

		this.document = doc;
    }
    
	if( doc )
	{
		doc.activate();
	}
}

//-----------------------------------------------------------------------------
// 
// setLineColor(...)
// 
// Purpose: Set the line color of passed document
// 
//-----------------------------------------------------------------------------

DebugSession.prototype.setLineColor = function( doc, line, color )
{
    if( !doc )
        doc = this.document;
        
    if( !line )
        line = this.line;
        
    if( !color )
        color = this.lineColor;

	if( line >= 0 )
	{
	    // set color of current line in document
		doc.setLineStatus( line, color );
		
		if( doc.isSourceDocument )
		{
		    // scroll to that line
		    doc.setSelection( line, 0, true );
		}
	}
}

//-----------------------------------------------------------------------------
// 
// updateDuplicate(...)
// 
// Purpose: Set line status for the passed duplicate document
// 
//-----------------------------------------------------------------------------

DebugSession.prototype.updateDuplicate = function( duplicateDoc )
{
    if( docMgr.related( this.document, duplicateDoc ) )
    {
        if( !this.lineColor )
        {
	        var color = colors.LightGreen;
	        var errorMsg = "";

	        if( this.frame == this.stack.length-1 )
	        {
		        color = colors.Yellow;

		        if( this.error )
		        {
			        color = colors.Coral;
			        errorMsg = this.error;
		        }
	        }
	        
	        if( this.line >= 0 )
	            this.lineColor = color;
    	}
    	
        docMgr.setStatusLine( errorMsg, duplicateDoc );
    	
	    if( this.line >= 0 )
	    {
	        // set color of current line in document
		    duplicateDoc.setLineStatus( this.line, this.lineColor );
    		
		    if( duplicateDoc.isSourceDocument )
		    {
		        // scroll to that line
		        duplicateDoc.setSelection( this.line, 0, true );
		    }
	    }
    }
}

//-----------------------------------------------------------------------------
// 
// setState(...)
// 
// Purpose: Set the state of the debugger with reflection into the UI.
// 
//-----------------------------------------------------------------------------

DebugSession.prototype.setState = function (state)
{
    var oldstate = this.state;		
	this.state = state;

	DebugSession.notifyClients( 'state', this, oldstate, this.state );
}

//-----------------------------------------------------------------------------
// 
// isDebugging(...)
// 
// Purpose: Session in debug mode?
// 
//-----------------------------------------------------------------------------

DebugSession.prototype.isDebugging = function( doc )
{
    var ret = ( this.state != DebugSession.INACTIVE );
    
    if( ret && doc )
        ret = ( this.documents[doc.scriptID] ? true : false );
        
    return ret;
}

//-----------------------------------------------------------------------------
// 
// isStopping(...)
// 
// Purpose: Return true if the session is about to stop execution
// 
// Author : pwollek
// 
//-----------------------------------------------------------------------------

DebugSession.prototype.isStopping = function()
{
	return this.stopExe;
}

//-----------------------------------------------------------------------------
// 
// forwardBridgeTalk(...)
// 
// Purpose: Forward a BridgeTalk message to cdi
// 
//-----------------------------------------------------------------------------

DebugSession.prototype.forwardBridgeTalk = function( bt )
{
    if( bt && this.sessionObj)
        this.sessionObj.customCall( 'BridgeTalk', true, bt );
}

//-----------------------------------------------------------------------------
// 
// reset(...)
// 
// Purpose: Reset session
// 
//-----------------------------------------------------------------------------

DebugSession.prototype.reset = function()
{
    this.setState( DebugSession.INACTIVE );

	if( this.document && this.document.isSourceDocument )
		this.document.setLineStatus( this.line, 0xFFFFFF );

    this.document   = null;
    this.documents  = {};
    this.dbgLevel   = ExecutionContext.DBGLEVEL_BREAK;
    this.line       = -1;
    this.lineColor  = undefined;
    this.frame      = -1;
    this.stack      = '';
    this.error      = null;

	// for UI update
	this.setState( DebugSession.INACTIVE );
}

//-----------------------------------------------------------------------------
// 
// finalizeExecution(...)
// 
// Purpose: Finalize debugging session.
//          Reset the DebugSession, activate the last debugged document
//          and bring app to front
// 
//-----------------------------------------------------------------------------

DebugSession.prototype.finalizeExecution = function()
{    
    if( !this.finalized )
    {
        this.finalized = true;
        
        // get profile data
        if( this.profileLevel != ProfileData.LEVEL_NONE )
            this.getProfileData();

        var doc = this.document;

        // reset the session
        this.reset();

        // bring last debugged document to front
        if( doc )
        {
            doc.activate();
        }
            
        // stop busy animation
        app.stopBusyFor( this.busyID );
        
        // clear call stack
        callstack.erase();
        
        this.tmpSilentStop = undefined;
    }
        
    // update target
	if( this.target )
		this.target.checkConnection();
    
    // bring app to front
    app.toFront();
	
	DebugSession.removeActiveSession();
    
    this.finalizing = false;
}

//-----------------------------------------------------------------------------
// 
// DebugSession(...)
// 
// Purpose: Start execution
// 
//-----------------------------------------------------------------------------

DebugSession.prototype.startSession = function( doc, dbgLevel, thisObj, errorInfo )
{
	//
	// store parameters in order to try a restart if the execution failed
	//
	if( thisObj.startInfo )
	{	
		if( thisObj.startInfo.count > DebugSession.kRestartSessionMAX )
			thisObj.startInfo = null;
	}
	else
	{
		thisObj.startInfo = {	
								doc			: doc,
								dbgLevel	: dbgLevel,
								errorInfo	: errorInfo,
								count		: 0
							};
	}
	
	//
	// if we are waiting for the target to response on
	// a stop command then cancel that now, finalize
	// the previous session and start the new one
	//
	if( thisObj.stopTaskID > -1 )
	{
		cancelScheduledTask( thisObj.stopTaskID, true );
		thisObj.stopTaskID = -1;
	}

    //
    // thisObj represents "this", but since this function might be also called
    // my a delayed function call we have to pass in the "this"-object
    //
    thisObj.finalized = false;
    
    if( thisObj.sessionObj && thisObj.sessionObj.enabled )
    {
        //
        // clear existing profile data
        //
        docMgr.clearProfileData();
        
        //
        // set documents busy state
        //
        thisObj.busyID = doc.getBusyID();
        app.startBusyFor( thisObj.busyID, true );

        //
        // store current doc and debug&profile level
        //
        if( doc )
            thisObj.documents[ doc.scriptID ] = doc;

        thisObj.document       = doc;
        thisObj.dbgLevel       = dbgLevel;
        thisObj.profileLevel   = prefs.profiling.profileLevel.getValue( Preference.NUMBER );
        thisObj.error          = null;
        
        //
        // execution context
        //
        var flags = prefs.debug.dontBreakOnErrors.getValue( Preference.BOOLEAN ) ? 1 : 0;
        
        var context = null;
		
		try
		{
			context = new ExecutionContext( doc.scriptID, 
                                            doc.getText(), 
                                            dbgLevel, 
                                            thisObj.profileLevel, 
                                            flags );
		}
		catch( exc )
		{
			context = null;
		}

		if( context )
		{                     
			var breakpoints = [];  
			globalBroadcaster.notifyClients( 'addSessionBreakpoints', thisObj.sessionObj.address, breakpoints );
	        
			for( var i=0; i<breakpoints.length; i++ )
				context.addBreakpoint( breakpoints[i] );
	        
			//
			// set state of session
			//  
			thisObj.setState( DebugSession.RUNNING );

			//
			// start actual execution
			//
			try
			{
				var job         = thisObj.sessionObj.startExecution( context );
				job.dbgSession  = thisObj;
				job.errorInfo   = errorInfo;
	            
				job.onResult = function()
				{	
					if( this.dbgSession.target )
						this.dbgSession.target.prepareDebugSession = false;
					
					if( !this.dbgSession.isStopping() )
					{
						if( !this.dbgSession.releasing )
							this.dbgSession.model.updateModel();

						var msg = localize( "$$$/ESToolkit/Status/ExecFinished=Execution finished." );
						var resStr = "";
						var resValue = this.result[0];

						if( !this.result[0] )
							resValue = "undefined";

						resStr = localize( "$$$/ESToolkit/Panes/Console/Result=Result:" ) + " " + resValue;
		                    
						docMgr.setStatusLine( msg + " " + resStr );

						if( prefs.debug.printResult.getValue( Preference.BOOLEAN ) )
							print( resStr );

						this.dbgSession.stopExe = false;
						this.dbgSession.finalizeExecution();
					}
				}
	            
				job.onError = function()
				{
					if( this.dbgSession.target )
						this.dbgSession.target.prepareDebugSession = false;
					
					this.dbgSession.stopExe = false;
					
					if( !this.dbgSession.finalized && !this.dbgSession.finalizing )
					{
						var errCode = 0;

						if( !isNaN( this.errorCode ) )
							errCode = this.errorCode;

						var errMsg = this.errorMessage;

						//
						// on error code #1 (engine busy) retry to start the session
						//
						if( errCode == 1 && errMsg == kErrMsgEngineBusy && this.dbgSession.startInfo )
						{
							docMgr.setStatusLine( this.errorMessage );
							this.dbgSession.startInfo.count++;
							this.dbgSession.startSession( this.dbgSession.startInfo.doc, this.dbgSession.startInfo.dbgLevel, this.dbgSession, this.dbgSession.startInfo.errorInfo );
						}
						else
						{
							if( !this.dbgSession.releasing )
								this.dbgSession.model.updateModel();

							try
							{
								var errorInfo = this.errorInfo;
								
								if( !errorInfo )
									errorInfo = new ErrorInfo();
							   
								if( errCode == kErrEngineNotExists )
									errMsg = localize( "$$$/ESToolkit/Error/EngineNotExists=Engine '%1' does not exists!", thisObj.address.engine );

								if( errCode > 0 )
									errMsg = "(#" + errCode + ") " + errMsg;

								errorInfo.push( errMsg );
								
								var generallMsg = localize( "$$$/ESToolkit/Error/MissingEngine=Cannot execute script in target engine '%1'!", thisObj.address.engine );
								
								if( generallMsg	!= this.errorMessage	&& 
									errCode		!= kErrEngineHalted		&&
									errCode		!= kErrBadAction			)
									errorInfo.push( generallMsg );
						 
								if( !this.dbgSession.tmpSilentStop )           
									errorInfo.display( errCode != kErrEngineHalted );
							
								this.dbgSession.finalizeExecution();

								if( this.dbgSession.temporary )
								{
									if( errCode == kErrEngineNotExists || errCode == kErrBadAction )
										this.dbgSession.target.removeSession( this.dbgSession );
								}
							}
							catch(exc)
							{}
						}
					}
				}
	            
				job.submit(-1);	// never timeout
				
				DebugSession.addActiveSession();
			}
			catch( exc )
			{
				if( this.dbgSession.target )
					thisObj.target.prepareDebugSession = false;
				
				if( thisObj.address )
					errorInfo.push( localize( "$$$/ESToolkit/Error/MissingEngine=Cannot execute script in target engine '%1'!", thisObj.address.engine ) );
				InternalError( errorInfo );
	                        
				thisObj.finalizeExecution();
			}        
	        
			if( !thisObj.temporary )
				thisObj.model.setupModel();
		}
		else if( this.dbgSession.target )
			thisObj.target.prepareDebugSession = false;
    }
    else
    {
        if( thisObj.initializing )
        {
            //
            // we are still initializing (if initialization failed the flag
            // 'initializing' is set to false, so this isn't a endless loop!)
            //
            addDelayedTask( thisObj.startSession, doc, dbgLevel, thisObj, errorInfo );
        }
        else
        {
			if( this.dbgSession.target )
				thisObj.target.prepareDebugSession = false;

            errorInfo.push( localize( "$$$/ESToolkit/Debug/Error/NoSession:Can't establish debugging session" ) );
			
			try
			{
				errorInfo.push( localize( "$$$/ESToolkit/Error/MissingEngine=Cannot execute script in target engine '%1'!", thisObj.address.engine ) );
			}
			catch( exc )
			{}
            
            errorInfo.display();
            
            thisObj.finalizeExecution();
        }
    }
}

//-----------------------------------------------------------------------------
// 
// DebugSession.command(...)
// 
// Purpose: Continue execution
// 
//-----------------------------------------------------------------------------

DebugSession.prototype.command = function( cmd, dbgContext )
{
    this.model.cancelUpdate();
    this.setState( DebugSession.RUNNING );
    
    try
    {
        var context = ( dbgContext ? dbgContext : this.createCurrentContext() );
        
        if( cmd != DebugSession.CMD_PAUSE )
        {
		    if( this.error && dsaQueryBox( "cre", "$$$/ESToolkit/Alerts/ClearErrors=Clear runtime error?" ) )
			    context.flags |= 0x2;
		}
		
		this.error = null;

        var job = null;
        
        switch( cmd )
        {
            case DebugSession.CMD_CONTINUE:
                job = this.sessionObj.continueExecution( context );
                break;
                
            case DebugSession.CMD_PAUSE:
                job = this.sessionObj.pauseExecution( context );
                break;
                
            case DebugSession.CMD_STEPOVER:
                job = this.sessionObj.stepOver( context );
                break;
                
            case DebugSession.CMD_STEPINTO:
                job = this.sessionObj.stepInto( context );
                break;
                
            case DebugSession.CMD_STEPOUT:
                job = this.sessionObj.stepOut( context );
                break;
        }
        
        job.dbgSession  = this;
        job.cmd         = cmd;
		job.dbgContext	= dbgContext;
        
        job.onResult = function()
        {
            // no results are expected
        }
            
        job.onError = function()
        {
            ErrorInfo( localize( "Error during execute command %1", this.cmd ) ).display();
            
            this.dbgSession.stop( undefined, undefined, undefined, this.dbgContext );
        }
        
        job.submit(-1);	// never timeout
    }
    catch( exc )
    {
        var errorInfo = new ErrorInfo( localize( "Error during execute command %1", cmd ) );
        InternalError( errorInfo );
        
        this.stop( undefined, undefined, undefined, dbgContext );
    }
}

//-----------------------------------------------------------------------------
// 
// DebugSession(...)
// 
// Purpose: Stop execution
// 
//-----------------------------------------------------------------------------

DebugSession.postStop = function( dbgSession, callback, silent )
{
	if( dbgSession.isStopping() )
	{
		dbgSession.stopExe = false;
		dbgSession.setState( DebugSession.INACTIVE );

		if( dbgSession && !dbgSession.finalized && !dbgSession.finalizing )
		{
			dbgSession.finalizing = true;
	        
			if( !silent )
			{
				try
				{
					var errorInfo = new ErrorInfo( localize( "$$$/ESToolkit/Status/NoRespond=%1 did not respond", dbgSession.target.getTitle() ) );
					errorInfo.push( kMsgHaltedExecution );
					errorInfo.display( false );
				}
				catch( exp )
				{}
			}
	        
			dbgSession.finalizeExecution();
		}
	    
		if( callback )
			callback.call();
	}
	else if( callback )
		callback.call();
}

DebugSession.prototype.stop = function( callback, silent, force, dbgContext )
{
	//
	// if stop is forced then only silent
	//
	silent = ( force ? force : silent );

	if( !this.isStopping() )
	{
		docMgr.setStatusLine( localize( "$$$/ESToolkit/Document/htStop=Stop execution of the script." ) );

		this.tmpSilentStop = silent;
		this.stopExe	   = true;
	    
		this.model.cancelUpdate();

		try
		{
			if( !this.releasing )
			{
				var context    = ( dbgContext ? dbgContext : this.createCurrentContext() );
				var job        = this.sessionObj.stopExecution( context );
				job.dbgSession = this;
				job.callback   = callback;
				job.silent     = silent;

				if( !force )
				{
					job.onResult = function()
					{
						//
						// if the target application don't respond then
						// finalize the debug session
						//
						if( this.dbgSession.address )
						{
							const timeout = 5000;   // 5 sec. timeout
							this.dbgSession.stopTaskID = addScheduledTask( timeout, DebugSession.postStop, this.dbgSession, this.callback, this.silent );
						}
						else 
						{
							this.dbgSession.stopExe = false;
							this.dbgSession.setState( DebugSession.INACTIVE );

							if( this.callback )
								this.callback.call();
						}
					}
			            
					job.onError = job.onTimeout = function()
					{
						if( !this.silent )
						{
							try
							{
								var error = new ErrorInfo( localize( "$$$/ESToolkit/Status/NoRespond=%1 did not respond", this.dbgSession.target.getTitle() ) );
								error.push( kMsgHaltedExecution );
								error.display( false );
							}
							catch( exc )
							{}
						}
			            
						this.dbgSession.stopExe = false;
						this.dbgSession.setState( DebugSession.INACTIVE );
			            
						this.dbgSession.finalizeExecution();

						if( this.callback )
							this.callback.call();
					}
				}

				job.submit( 5000 ); // timeout 5sec.
			}

			if( force || this.releasing )
			{
				docMgr.setStatusLine( "" );

				this.stopExe = false;
				this.setState( DebugSession.INACTIVE );
		        
				this.finalizeExecution();

				if( this.callback )
					this.callback.call();
			}
		}
		catch( exc )
		{
			if( !silent )
			{
				var error = new ErrorInfo( localize( "$$$/CT/ExtendScript/Errors/Err59=No response" ) );
				error.push( kMsgHaltedExecution );
				InternalError( errorInfo, false );
			}
	        
			this.stopExe = false;
			this.setState( DebugSession.INACTIVE );
	        
			this.finalizeExecution();

			if( this.callback )
				this.callback.call();
		}
	}
}

//-----------------------------------------------------------------------------
// 
// eval(...)
// 
// Purpose: Eval source
// 
//-----------------------------------------------------------------------------

DebugSession.prototype.eval = function( source, noReply )
{
    this.model.cancelUpdate();

	if( !this.releasing )
	{
		try
		{
			var job        = this.sessionObj.eval( source );
			job.dbgSession = this;
			job.noReply    = noReply;

			job.onResult = function()
			{
				// the reply is datatype,result
				if( !this.noReply )
					print( localize( "$$$/ESToolkit/Panes/Console/Result=Result:" ), ' ', this.result[0] );

				this.dbgSession.model.updateModel();
			}
	            
			job.onError = function()
			{
				// the reply is the message
				print( localize( "$$$/ESToolkit/Panes/Console/Error=Error:" ), ' ', this.errorMessage );
				app.beep();
			}
	        
			job.submit();
		}
		catch( exc )
		{
			InternalError();
		}
	}
}

//-----------------------------------------------------------------------------
// 
// resetEngine(...)
// 
// Purpose: Reset the engine
// 
//-----------------------------------------------------------------------------

DebugSession.prototype.resetEngine = function()
{
    this.model.cancelUpdate();

	if( !this.releasing )
	{
		try
		{
			var job = this.sessionObj.reset();
			job.submit();
		}
		catch( exc )
		{
			InternalError();
		}

		this.model.setupModel();
	}
}

//-----------------------------------------------------------------------------
// 
// switchFrame(...)
// 
// Purpose: Create execution context for current session
// 
//-----------------------------------------------------------------------------

DebugSession.prototype.switchFrame = function( index )
{
    if( this.state == DebugSession.STOPPED && !this.releasing )
    {
	    if( this.frame != index )
	    {
            this.model.cancelUpdate();
        
            try
            {
                var job = this.sessionObj.switchFrame( index );
                job.submit();
            }
            catch( exc )
            {
                InternalError();  
            }
	    }
    }
}

//-----------------------------------------------------------------------------
// 
// DebugSession(...)
// 
// Purpose: Create execution context for current session
// 
//-----------------------------------------------------------------------------

DebugSession.prototype.createCurrentContext = function()
{
    var flags = prefs.debug.dontBreakOnErrors.getValue( Preference.BOOLEAN ) ? 1 : 0;
    
    var context = null;
	
	if( this.document )
	{
		context = new ExecutionContext( this.document.scriptID, 
                                        this.document.getText(), 
                                        this.dbgLevel, 
                                        this.profileLevel,
                                        flags );
	}

/*  pwollek 04/21/2008: 
    There's no need to collect and send all breakpoints with each debuggign command
    because the breakpoints are sent once when starting the debugging session and
    whenever a breakpoint change happend.
                
    var breakpoints = [];  
    globalBroadcaster.notifyClients( 'addSessionBreakpoints', this.sessionObj.address, breakpoints );
    
    for( var i=0; i<breakpoints.length; i++ )
        context.addBreakpoint( breakpoints[i] );
*/    
    return context;
}

//-----------------------------------------------------------------------------
// 
// setBreakpoints(...)
// 
// Purpose: Set an array of breakpoints
// 
//-----------------------------------------------------------------------------

DebugSession.prototype.setBreakpoints = function( breakpoints )
{
    if( this.sessionObj && !this.releasing )
    {
        try
        {
            var job = this.sessionObj.setBreakpoints( breakpoints );
            job.submit();
        }
        catch( exc )
        {
            InternalError();
        }
    }
}

//-----------------------------------------------------------------------------
// 
// getBreakpoints(...)
// 
// Purpose: Get all breakpoints
// 
//-----------------------------------------------------------------------------

DebugSession.prototype.getBreakpoints = function( callback )
{
    if( this.sessionObj && !this.releasing )
    {
        try
        {
            var job = this.sessionObj.getBreakpoints();
            job.cb  = callback;
            
            job.onResult = function()
            {
                if( this.cb )
                    this.cb.call( this.result );
            }
            
            job.onError = job.onTimeout = function()
            {
                if( this.cb )
                    this.cb.call( [] );
            }
            
            job.submit();
        }
        catch( exc )
        {
            if( callback )
                callback.call( [] );
        }
    }
}

//-----------------------------------------------------------------------------
// 
// removeBreakpoints(...)
// 
// Purpose: Remove all breakpoints
// 
//-----------------------------------------------------------------------------

DebugSession.prototype.removeBreakpoints = function()
{
    if( this.sessionObj && !this.releasing )
    {
        try
        {
            var job = this.sessionObj.removeBreakpoints();
            job.submit();
        }
        catch( exc )
        {
            InternalError();
        }
    }
}

//-----------------------------------------------------------------------------
// 
// setBreakpoint(...)
// 
// Purpose: Set a single breakpoint
// 
//-----------------------------------------------------------------------------

DebugSession.prototype.setBreakpoint = function( bp )
{
    if( this.sessionObj && !this.releasing )
    {
        try
        {
            var job = this.sessionObj.setBreakpoint( bp );
            job.submit();
        }
        catch( exc )
        {
            InternalError();
        }
    }
}

//-----------------------------------------------------------------------------
// 
// removeBreakpoint(...)
// 
// Purpose: Remove a single breakpoint
// 
//-----------------------------------------------------------------------------

DebugSession.prototype.removeBreakpoint = function( bp )
{
    if( this.sessionObj && !this.releasing )
    {
        try
        {
            var job = this.sessionObj.removeBreakpoint( bp );
            job.submit();
        }
        catch( exc )
        {
            InternalError();
        }
    }
}

//-----------------------------------------------------------------------------
// 
// DebugSession(...)
// 
// Purpose: Process incoming tasks
// 
//-----------------------------------------------------------------------------

DebugSession.prototype.processTasks = function( task, dbgSession )
{
    //
    // if this function was called by a delayed task, then 'this' is the 
    // global object and the DebugSession object needs to be passed in.
    // 
    if( !dbgSession )
        dbgSession = this.dbgSession;

	try
	{
		if( dbgSession.initializing || ( dbgSession.target && dbgSession.target.getChangeConnectState() ) )
			addDelayedTask( dbgSession.processTasks, task, dbgSession );
		else
		{
			switch( task.name )
			{
				case Job.PRINT:
				{
					print( task.message );
				}
				break;
	                    
				case Job.BREAKPOINTS:
				{
				}
				break;
	            
				case Job.ERROR:
				{
					dbgSession.error = task.errorMessage;
					docMgr.setStatusLine( dbgSession.error );
					app.beep();
					// fall thru
				}
				break;
	            
				case Job.EXECUTION_BREAK:
				case Job.FRAME_SWITCHED:
				{   
					// if we were about to stop then cancel it
					dbgSession.stopExe = false;

					app.checkForFileChanges();

					// clear flag if set
					remoteLaunched = false;

					if( dbgSession.state == DebugSession.INACTIVE )
						DebugSession.addActiveSession();
						
					dbgSession.finalized = false;
	                  
					//     
					// If in a modal state, do not stop
					//
					if( app.modalState )
					{						
						dbgSession.state = DebugSession.STOPPED;
						var context = new ExecutionContext( task.context.scriptID, 
															task.context.source, 
															dbgSession.dbgLevel, 
															dbgSession.profileLevel,
															( prefs.debug.dontBreakOnErrors.getValue( Preference.BOOLEAN ) ? 1 : 0 ) );

						task.quitTask();
						dbgSession.command( DebugSession.CMD_CONTINUE, context );
						break;
					}
	    			
					var context = task.context;
	                
					if( context )
					{
						//
						// set up the document
						//
						var doc = dbgSession.document;
	        			
						if( !doc || ( context.scriptID.length > 0 && doc.scriptID != context.scriptID ) )
							doc = docMgr.find( context.scriptID );

						var newDocCreated = false;

						if( !doc )
						{
							//
							// no doc yet - it came from the target
							//
							if( File( context.scriptID ).exists )
							{
								doc = scripts.loadFile( context.scriptID );
							}
							else if( context.source )		// supplied by break
							{
								var docWin = docMgr.create( dbgSession.title, context.source, undefined, context.scriptID );
	        					
								if( docWin )
									doc = docWin.docObj;
							}
							else
							{
								// no source supplied (pre-x41): set off an async get script request

		//TODO: this.target, this.engine???				        
								scripts.loadScript( context.scriptID, dbgSession.title, dbgSession.target, dbgSession.engine );
								// may be there already...
								doc = docMgr.find( context.scriptID );
							}
						}
						else
						{
							if( docMgr.related( dbgSession.document, doc ) )
								doc = dbgSession.document;
						}

						if( doc )
						{
							//
							// activate document and set target&engine popup
							//
							docMgr.activateDocument( doc, dbgSession );
						}
						else
							break;
				            
						//
						// add file type
						//
						try
						{
							dbgSession.target.addFiletypes( doc.langID );
							
							if( doc.isFile() )
							{
								var pos = doc.scriptID.lastIndexOf( '.' );
								
								if( pos >= 0 )
								{
									var ext = doc.scriptID.substring( pos );
									
									if( ext.length > 0 )
										dbgSession.target.addFiletype( ext );
								}
							}
						}
						catch( exc )
						{}

						//
						// If we came here, the debugger needs to be activated, and
						// the breakpoint needs to be displayed
						//

						var frameChange         = dbgSession.frame != context.frame;
						var stackChanged        = false;
						dbgSession.line	        = context.line;
						dbgSession.lineColor    = undefined;
						dbgSession.frame	    = context.frame;
	    		        
						if( context.stack && task.name == Job.EXECUTION_BREAK )
						{
							stackChanged = true;
	    			        
							if( dbgSession.stack.length == context.stack.length )
							{
								stackChanged = false;
	    			            
								for( var i=0; i<context.stack.length; i++ )
								{
									if( dbgSession.stack[i] != context.stack[i] )
									{
										stackChanged = true;
										break;
									}
								}
							}
	    			        
							dbgSession.stack = context.stack;
							callstack.updatePane( context.stack );
						}

						if( dbgSession.busyID && !docMgr.related( doc, dbgSession.document ) )
						{
							if( app.isBusy( dbgSession.busyID ) )
								app.stopBusyFor( dbgSession.busyID );
	                        
							dbgSession.busyID = null;
						}

						dbgSession.setDocument( doc );

						if( !dbgSession.busyID )
						{
							dbgSession.busyID = doc.getBusyID();
							app.startBusyFor( dbgSession.busyID, true );
						}			            
	    
						dbgSession.setState( DebugSession.STOPPED );

						// Get local variables, get all variables if the stack trace changed
						dbgSession.model.setBaseScope( dbgSession.frame == 0 );

						if( frameChange )
							dbgSession.model.setupModel();
						else
							dbgSession.model.updateModel();

						// get profile data
						if( dbgSession.profileLevel != ProfileData.LEVEL_NONE )
							dbgSession.getProfileData();

						app.toFront();
					}
	                
					task.quitTask();
				}
				break;
	            
				case Job.EXECUTION_EXIT:
				{
					dbgSession.model.updateModel();
					dbgSession.stopExe = false;

					if( !dbgSession.finalized && !dbgSession.finalizing )
					{
						docMgr.setStatusLine( localize( "$$$/ESToolkit/Status/ExecFinished=Execution finished." ) );
						dbgSession.finalizeExecution();
					}
	                
					// bring app to front
					app.toFront();

					task.quitTask();
				}
				break;
			}
		}
	}
	catch( exc )
	{}
}
