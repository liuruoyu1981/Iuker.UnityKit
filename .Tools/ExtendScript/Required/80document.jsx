/**************************************************************************
*
*  @@@BUILDINFO@@@ 80document-2.jsx 3.5.0.25	05-May-2009
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

Document.FIND_WRAPAROUND    = 1;
Document.FIND_IGNORECASE    = 4;
Document.FIND_WORDS         = 8;
Document.FIND_REGEXP        = 16;
Document.FIND_REPLACEALL	= 32;
Document.FIND_SELECTION     = 64;

SourceDocument.AREA_LINENUMBERS	= 0;
SourceDocument.AREA_BREAKPOINTS	= 1;

//-----------------------------------------------------------------------------
// 
// createSourceDocument(...)
// 
// Purpose: factory function
// 
//-----------------------------------------------------------------------------

function createSourceDocument( title, langID, scriptID, readPreferences, master, dockToDocument )
{
    return new SourceDocument( title, langID, scriptID, readPreferences, master, dockToDocument );
}

///////////////////////////////////////////////////////////////////////////////
//
// class SourceDocument
//

//-----------------------------------------------------------------------------
// 
// SourceDocument(...)
// 
// Purpose: ctor
// 
//-----------------------------------------------------------------------------

function SourceDocument( title, langID, scriptID, readPreferences, master, dockToDocument )
{
    this.rootPane               = null;         // the root panel of the document UI
    this.editor                 = null;
    this.menu                   = null;         // the flyout menu
    this.langID                 = "";           // the lexer language
    this.badLine                = -1;           // If a syntax error was detected, this is the line (need to remove color on edits)
    this.status                 = undefined;    // exec/noexec
    this.currentPos	            = [0,0];        // current logical position
	this.lastAutoComplete	    = "";           // holds the last autocomplete string to inhibit double calls
	this.profileData		    = {};	        // property name == line number
	this.profileDataCount       = 0;
	this.profileLevel			= ProfileData.LEVEL_NONE;
	this.autoIndent             = prefs.document.autoIndent.getValue( Preference.NUMBER );
	this.acHelper			    = this.duplicate ? this.master.acHelper : new AutoCompletion();
	this.isSourceDocument       = true;         // this is a SourceDocument
	
	this.blockUIUpdate          = 0;
	
    this.blockCodeChange        = 0;            // ignore editor changes
    this.blockedCodeChangeHdl	= undefined;    // cached onChange handler
    this.blockedLineChangeHdl	= undefined;    // cached onChange handler
	
	this.backup					= null;			// backup file object
	this.backupTaskID			= -1;			// delayed task to backup document file
	this.bkModified				= false;		// document was modified since recent backup

	this.closed					= false;		// true after window got closed
    
    // 
	// The following properties were added to this object by the DocumentManager:
	//
    // busyID           - identifier of busy animation
    // paneTitle        - the original title to display
    // fileName	        - the file name part of the title
    // roState          - true if the document is logically read only (it can be modified, though)
    // scriptID         - the script ID; either a full pathname, (ScriptN) for new scripts, or a target ID
    // langID	        - the lexer language (default: js)
    // lf		        - the line endings for Save
    // encoding	        - the file encoding for Save
    // lastAutoComplete - holds the last autocomplete string to inhibit double calls
    // restored         - document preferences where restored
}

//-----------------------------------------------------------------------------
// 
// getBlockedUIUpdate(...)
// 
// Purpose: Blcok UI updates (document toolbar)
// 
//-----------------------------------------------------------------------------

SourceDocument.prototype.getBlockedUIUpdate = function()
{
    return ( this.blockUIUpdate > 0 );
}

SourceDocument.prototype.setBlockUIUpdate = function( doBlock )
{
    if( doBlock )
        this.blockUIUpdate++;
    else
        this.blockUIUpdate--;
        
    if( this.blockUIUpdate < 0 )
        this.blockUIUpdate = 0;
}

//-----------------------------------------------------------------------------
// 
// function(...)
// 
// Purpose: [Callback] Initialize instance. Called by the doc mgr after it
//                     created a new instance.
// 
//-----------------------------------------------------------------------------

SourceDocument.prototype.initialize = function( win )
{
    win.docObj = win.__docObj__;

	win.wc = win.add ( """group {             
	                orientation     : 'column',
	                alignment       : ['fill','fill'],
                    margins         : 0,
                    spacing         : 0,
                    properties      :
                    {
                        minimumSize     : [400, 180],
                    },
                    toolbarGroup    : Group
                    {
                        alignment       : ['fill', 'top' ],
                        orientation     : 'row',
                        margins         : 0,
                        spacing         : 0,
                        targetGroup     : Group
                        {
                            alignment       : ['fill','fill'],
                            alignChildren   : ['fill','fill'],
                            orientation     : 'row',
                            margins         : 0,
                            spacing         : 2,
                            btnCon          : IconButton
                            {
                                preferredSize    : [24, 24],
                                properties       : { style : 'toolbutton' },
                                alignment        : [ 'left', 'center' ],
                                helpTip          : '$$$/ESToolkit/Document/htCon=Click to connect to target application.'
                            },
                            targetDDL         : DropDownList
                            {
                                alignment        : [ 'fill', 'center' ],
                                helpTip          : '$$$/ESToolkit/Document/htTargets=Select the target application.'
                            },
                            imgstate         : Image
                            {
                                preferredSize   : [18, 18],
                                maximumSize     : [18, 18],
                                minimumSize     : [18, 18],
                                alignment        : [ 'fill', 'center' ],
                                helpTip          : '$$$/ESToolkit/Document/htState=Current state of selected engine.'
                            },
                            engineDDL         : DropDownList
                            {
                                alignment        : [ 'fill', 'center' ],
                                helpTip          : '$$$/ESToolkit/Document/htEngine=Select the engine of the target application.'
                            }
                        },
                        dummy01         : Group
                        {
                            preferredSize   : [15,22],
                            alignment       : ['right','fill']
                        },
                        debugGroup      : Group
                        {
                            alignment       : ['right','fill'],
                            orientation     : 'row',
                            margins         : 0,
                            spacing         : 2,
                            btnRun          : IconButton
                            {
                                preferredSize    : [24, 24],
                                properties       : { style : 'toolbutton' },
                                helpTip          : '$$$/ESToolkit/Document/htRun=Start running script.'
                            },
                            btnPause        : IconButton
                            {
                                preferredSize   : [24, 24],
                                properties       : { style : 'toolbutton' },
                                helpTip          : '$$$/ESToolkit/Document/htHalt=Halt execution of the script.'
                            },
                            btnStop         : IconButton
                            {
                                preferredSize   : [24, 24],
                                properties       : { style : 'toolbutton' },
                                helpTip          : '$$$/ESToolkit/Document/htStop=Stop execution of the script.'
                            },
                            btnStepover     : IconButton
                            {
                                preferredSize   : [24, 24],
                                properties       : { style : 'toolbutton' },
                                helpTip          : '$$$/ESToolkit/Document/htStepover=Step over current script line.'
                            },
                            btnStepinto     : IconButton
                            {
                                preferredSize   : [24, 24],
                                properties       : { style : 'toolbutton' },
                                helpTip          : '$$$/ESToolkit/Document/htStepinto=Step into function call in the current line.'
                            },
                            btnStepout      : IconButton
                            {
                                preferredSize   : [24, 24],
                                properties       : { style : 'toolbutton' },
                                helpTip          : '$$$/ESToolkit/Document/htStepout=Step out of current executed function.'
                            }
                        },
                        dummy02         : Group
                        {
                            preferredSize   : [15,22],
                            alignment       : ['right','fill']
                        },
                        extraGroup      : Group
                        {
                            alignment       : ['right','fill'],
                            orientation     : 'column',
                            margins         : 2,
                            spacing         : 2,
                            btnFlyout       : Image
                            {
                                preferredSize    : [18,13],
                                properties       : { style : 'toolbutton' },
                                helpTip          : '$$$/ESToolkit/Document/htFMenu=Click to open the menu.',
                                alignment       : ['left','fill'],
                            },
                            btnSplit        : IconButton
                            {
                                alignment       : ['right','fill'],
                                preferredSize   : [17, 11],
                                properties       : { style : 'toolbutton' },
                                helpTip          : '$$$/ESToolkit/Document/htSplit=Click to open a duplicate of the document.'
                            }
                        }
                    },
                    document        : Document
                    {
                        maximumSize     : [10000,10000],
                        preferredSize   : [390, 460],
                        alignment       : ['fill', 'fill' ],
                    }
                }""" );
	
    win.wc.maximumSize = [ 10000, 10000 ];

    //
    // add shortcut property 'rootPane' to every each child UI element
    //
    function addShortcut( pane, content )
    {
		try
		{
			for( var i=0; i<content.children.length; i++ )
			{
				content.children[i].rootPane = pane;
				addShortcut( pane, content.children[i] );
			}
		}
		catch( exc )
		{}
    }
    
    addShortcut( win, win.wc );

    ///////////////////////////////////////////////////////////////////////////////
    //
    // create shortcuts
    //
    win.toolbarGroup  = win.wc.toolbarGroup;
    win.targetDDL     = win.wc.toolbarGroup.targetGroup.targetDDL;
    win.engineDDL     = win.wc.toolbarGroup.targetGroup.engineDDL;
    win.btnCon        = win.wc.toolbarGroup.targetGroup.btnCon;
    win.btnRun        = win.wc.toolbarGroup.debugGroup.btnRun;
    win.btnStepover   = win.wc.toolbarGroup.debugGroup.btnStepover;
    win.btnStepinto   = win.wc.toolbarGroup.debugGroup.btnStepinto;
    win.btnStepout    = win.wc.toolbarGroup.debugGroup.btnStepout;
    win.btnStop       = win.wc.toolbarGroup.debugGroup.btnStop;
    win.btnPause      = win.wc.toolbarGroup.debugGroup.btnPause;
    win.imgstate      = win.wc.toolbarGroup.targetGroup.imgstate;
    win.btnFlyout     = win.wc.toolbarGroup.extraGroup.btnFlyout;
    win.btnSplit      = win.wc.toolbarGroup.extraGroup.btnSplit;
    win.document      = win.wc.document;
    
    win.document.docObj = this;

	if( this.duplicate )
		win.document.master = this.master.rootPane.document;

	workspace.setDocumentDefaultFocus( win, win.wc.document );
    
    ///////////////////////////////////////////////////////////////////////////////
    //
    // set widths
    //
	win.targetDDL.preferredSize.width = 150;
	win.engineDDL.preferredSize.width = 100;

    ///////////////////////////////////////////////////////////////////////////////
    //
    // debugging buttons
    //
    win.btnRun.icon         = ScriptUI.newImage( '#Run_R', '#Run_N', undefined, '#Run_O' );
    win.btnPause.icon       = ScriptUI.newImage( '#Pause_R', '#Pause_N', undefined, '#Pause_O' );
    win.btnStop.icon        = ScriptUI.newImage( '#Stop_R', '#Stop_N', undefined, '#Stop_O' );
    win.btnStepover.icon    = ScriptUI.newImage( '#StepOver_R', '#StepOver_N', undefined, '#StepOver_O' );
    win.btnStepinto.icon    = ScriptUI.newImage( '#StepInto_R', '#StepInto_N', undefined, '#StepInto_O' );
    win.btnStepout.icon     = ScriptUI.newImage( '#StepOut_R', '#StepOut_N', undefined, '#StepOut_O' );

    win.btnRun.onClick      = menus.debug.run.onSelect;
    win.btnPause.onClick    = menus.debug.pause.onSelect;
    win.btnStop.onClick     = menus.debug.stop.onSelect;
    win.btnStepover.onClick = menus.debug.over.onSelect;
    win.btnStepinto.onClick = menus.debug.into.onSelect;
    win.btnStepout.onClick  = menus.debug.out.onSelect;
    
    ///////////////////////////////////////////////////////////////////////////////
    //
    // connect button
    //
    win.btnCon.iconConnected      = ScriptUI.newImage( '#Connected_R', '#Connected_N', undefined, '#Connected_O' );
    win.btnCon.iconDisconnected   = ScriptUI.newImage( '#Disconnected_R', '#Disconnected_N', undefined, '#Disconnected_O' );
    win.btnCon.iconConnecting     = ScriptUI.newImage( '#Connecting_R', '#Connecting_N', undefined, '#Connecting_O' );
    
    win.btnCon.icon           = win.btnCon.iconConnected;
    
    win.btnCon.onClick = function()
    {        
		var target = this.rootPane.docObj.getCurrentTarget();
        
		if( target )
		{
			if( target.getConnected() )
				target.disconnect();
			else
			{
				var err = new ErrorInfo();
				var cb = new Callback( function(err){ if(err) err.display(); }, err );
				target.connect( undefined, cb, err );
				err.display();
			}
		}

		this.rootPane.docObj.activate();
    }

    ///////////////////////////////////////////////////////////////////////////////
    //
    // engine state icon
    //
    win.imgstate.iconInactive = ScriptUI.newImage( '#InactiveEngine', '#InactiveEngine' );
    win.imgstate.iconWait     = ScriptUI.newImage( '#WaitingEngine', '#InactiveEngine' );
    win.imgstate.iconRun      = ScriptUI.newImage( '#RunningEngine', '#InactiveEngine' );
    win.imgstate.iconStop     = ScriptUI.newImage( '#StoppedEngine', '#InactiveEngine' );
    
    win.imgstate.icon         = win.imgstate.iconInactive;

    ///////////////////////////////////////////////////////////////////////////////
    //
    // extra buttons
    //
    win.btnFlyout.iconD = ScriptUI.newImage( '#Flyout_N' );
    win.btnFlyout.iconO = ScriptUI.newImage( '#Flyout_R' );
    win.btnFlyout.iconP = ScriptUI.newImage( '#Flyout_P' );
    win.btnFlyout.state = 0;

    win.btnFlyout.icon = ScriptUI.newImage( '#Flyout_N', undefined, '#Flyout_R', '#Flyout_P' );
    win.btnSplit.icon  = ScriptUI.newImage( '#Split_N', undefined, '#Split_R', '#Split_R' );
    
    win.btnFlyout.addEventListener( "mouseover", 
                                      function(e)
                                      {
                                        e.target.icon = e.target.iconO;
                                        e.target.state = 1;
                                      },
                                      false );  
    win.btnFlyout.addEventListener( "mouseout", 
                                      function(e)
                                      {
                                        e.target.icon = e.target.iconD;
                                        e.target.state = 0;
                                      },
                                      false );  
    win.btnFlyout.addEventListener( "mousedown", 
                                      function(e)
                                      {
                                        e.target.icon = e.target.iconP;
                                        e.target.state = 2;
                                      },
                                      false );  
    win.btnFlyout.addEventListener( "mouseup", 
                                      function(e)
                                      {
                                        e.target.icon = e.target.iconD;
                                        e.target.state = 1;
                                        e.target.rootPane.docObj.updateFlyout();
                                        e.target.menu.show( e.screenX, e.screenY );
                                      },
                                      false );  
    
    ///////////////////////////////////////////////////////////////////////////////
    //
    // Duplicated document
    //
    win.btnSplit.onClick = function()
    {
        var docObj = this.rootPane.docObj;
        var master = ( docObj.duplicate ? docObj.master : docObj );
        
        docMgr.createDuplicate( master, docObj );
    }

    ///////////////////////////////////////////////////////////////////////////////
	//
	// targets & engines dropdown lists
	//
	
	//-----------------------------------------------------------------------------
	// 
	// win.getChangingTargetEngine(...)
	// 
	// Purpose:get/set change status of targets & engines - DropDownLists
	// 
	//-----------------------------------------------------------------------------
	
	win.getChangingTargetEngine = function()
	{
	    return ( targetMgr.isChanging() || this.userChangeTargetEngine );
	}
	
	win.setChangingTargetEngine = function( change )
	{
	    this.userChangeTargetEngine = change;
	    targetMgr.setIsChanging( change );
	}
	
	//-----------------------------------------------------------------------------
	// 
	// win.targetDDL.onChange(...)
	// 
	// Purpose:selection of targets list changed
	// 
	//-----------------------------------------------------------------------------

	function autoConnect( doc, target )
	{
		if( target && doc )
		{
			if( target.getChangeConnectState() )
				addDelayedTask( autoConnect, doc, target )
			else if( !target.getConnected() )
				doc.rootPane.btnCon.notify();
		}
	}
	
	win.targetDDL.onChange = function()
	{	
	    if( !appInShutDown && !this.rootPane.isClosing )
	    {
	        //
	        // we process only change events if the user selects a new target
	        //
	        if( !this.rootPane.getChangingTargetEngine() )
	        {
		        var targetItem = this.selection;

		        if( targetItem )
		        {		    
	                this.rootPane.setChangingTargetEngine( true );
        	        
	                //
	                // set active target at target manager
	                // (cause notify 'changeActiveSession')
	                //	                
	                targetMgr.setActive( targetItem.target, null );

	                this.rootPane.setChangingTargetEngine( false );
	            }
           
				if( targetItem && !targetMgr.getConnected( targetItem.target ) )
				{
					if( prefs.autolaunch.getValue( Preference.BOOLEAN ) )
						autoConnect( this.rootPane.docObj, targetItem.target );
				}
	        }
 	    }
	}

    //-----------------------------------------------------------------------------
    // 
    // win.engineDDL.onChange(...)
    // 
    // Purpose: selection of engines list changed
    // 
    //-----------------------------------------------------------------------------

	win.engineDDL.onChange = function()
	{	
	    if( !appInShutDown && !this.rootPane.isClosing )
	    {
	        //
	        // we process only change events if the user selects a new target
	        //
	        if( !this.rootPane.getChangingTargetEngine() )
	        {
	            var activeTarget = targetMgr.getActiveTarget();
	            var engineItem   = this.selection;
    	        
	            if( activeTarget && engineItem )
	            {
	                this.rootPane.setChangingTargetEngine( true );
    	            
	                //
	                // set active engine at current active target
	                // (cause notify 'changeActiveSession')
	                //
	                activeTarget.setActive( engineItem.engine );
    	            
					// update the HREF for code completion
					var doc         = this.rootPane.docObj;
					var currTarget  = doc.getCurrentTarget();
					var currSession = doc.getCurrentSession();
					doc.acHelper.setTargetAndSession( currTarget, currSession );

	                this.rootPane.setChangingTargetEngine( false );
	            }
	        }
	    }
	}

    ///////////////////////////////////////////////////////////////////////////////
    //
    // Window
    //
    
    //-----------------------------------------------------------------------------
    // 
    // w.updateToolbarState(...)
    // 
    // Purpose: Update UI states of toolbar area
    // 
    //-----------------------------------------------------------------------------
    
    win.updateToolbarState = function( connectionStateOnly )
    {
		if( !connectionStateOnly )
			connectionStateOnly = false;
			
        if( !appInShutDown && !this.docObj.getBlockedUIUpdate() && !this.isClosing )
        {
            var target  = this.docObj.getCurrentTarget();
            var session = this.docObj.getCurrentSession();
            
            //
            // disable engine DDL if target isn't connected
            //
            if( target && target.getConnected() || targetMgr.defaultTarget == target )
            {
                this.engineDDL.enabled  = true;
                this.btnCon.icon        = this.btnCon.iconConnected;
				this.btnCon.enabled		= true;
            }
            else
            {
                this.engineDDL.enabled  = false;
                this.btnCon.icon        = this.btnCon.iconDisconnected;
				this.btnCon.enabled		= true;
            }
            
            //
            // set connect button icon if we just about to connect
            //
            if( target && target.getChangeConnectState() )
			{
                this.btnCon.icon        = this.btnCon.iconConnecting;
				this.btnCon.enabled		= false;
			}
            
			if( !connectionStateOnly )
			{
				//
				// debugging state
				//
				
				//
				// Is there a debugger for the current target & engine ?
				// If there is one and if it's running then don't let
				// the user change target or engine!
				//        
				if( session && session.isDebugging( this.docObj ) )             
				{
					//
					// debugger of document is actually running, so disable target&engine DropDownList
					//
					this.targetDDL.enabled = false;
					this.engineDDL.enabled = false;
				}
				else
				{
					this.targetDDL.enabled = true;
					
					if( this.engineDDL.enabled )
						// don't enable if it was disabled before
						this.engineDDL.enabled = true;
				}
				
				//
				// Is there any other debugger for the current target?
				// If there is one (but not using the current engine) and
				// it's running then don't let the user start another
				// session at the same target but different engine!
				//
		// with ESTK3 we wanna allow multiple sessions on one target at the same time        
		//        var canDebug = this.document.canDebug();

				if( this.docObj == document )
				{
					// update menu items, too
					menus.debug.reflectState();
				}
				else
				{
					this.updateDebugButtonStates( target && target.getConnected() );
				}
								
				// update current engine state icon
				this.docObj.setEngineState( session );
			}
        }
    }
    
    //-----------------------------------------------------------------------------
    // 
    // win.updateDebugButtonStates(...)
    // 
    // Purpose: Update UI states of debugging buttons
    // 
    //-----------------------------------------------------------------------------
    
    win.updateDebugButtonStates = function( disable )
    {        
        function disableButtons( docWin )
        {
            docWin.btnRun.setEnabled( false );
            docWin.btnStepover.setEnabled( false );
            docWin.btnStepinto.setEnabled( false );
            docWin.btnStepout.setEnabled( false );
            docWin.btnStop.setEnabled( false );
            docWin.btnPause.setEnabled( false );
        }
        
        if( !this.docObj.getBlockedUIUpdate() && !this.isClosing )
        {
            var target  = this.docObj.getCurrentTarget();
            var session = this.docObj.getCurrentSession();

			if( session && session.isStopping() )
				disable = true;

            if( disable )
                disableButtons( this );
            else
            {
		        var running = false;
		        var stopped = false;
				
				var deep = false;
        		
		        if( session )
		        {
		            running = ( session.state == DebugSession.RUNNING );
		            stopped = ( session.state == DebugSession.STOPPED );
					
					deep = ( session.frame > 0 );
		        }
        		
		        switch( this.docObj.getStatus() )
		        {
			        case "noexec":
				        // not executable
				        disableButtons( this );
				        break;
        				
			        case "exec":
				        // standard executable
                        this.btnRun.setEnabled( ( !running && target.getFeature( Feature.START_EXECUTION, session ) ) || 
                                                ( stopped && target.getFeature( Feature.CONTINUE_EXECUTION, session ) ) );
                        this.btnStepover.setEnabled( ( !running || stopped ) && 
                                                     target.getFeature( Feature.STEP_OVER, session ) );
                        this.btnStepinto.setEnabled( ( !running || stopped ) && 
                                                     target.getFeature( Feature.STEP_INTO, session ) );
                        this.btnStepout.setEnabled( deep && stopped && 
                                                    target.getFeature( Feature.STEP_OUT, session ) );
                        this.btnStop.setEnabled( ( ( running || stopped ) && 
                                                   target.getFeature( Feature.STOP_EXECUTION, session ) ) || 
                                                 ( this.docObj.badLine >= 0 ) );
                        this.btnPause.setEnabled( running && 
                                                  target.getFeature( Feature.PAUSE_EXECUTION, session ) );
				        break;

			        case "dynamic":
				        // dynamic script, not saveable
                        this.btnRun.setEnabled( stopped && target.getFeature( Feature.CONTINUE_EXECUTION, session ) );
                        this.btnStepover.setEnabled( stopped && target.getFeature( Feature.STEP_OVER, session ) );
                        this.btnStepinto.setEnabled( stopped && target.getFeature( Feature.STEP_INTO, session ) );
                        this.btnStepout.setEnabled( deep && stopped && target.getFeature( Feature.STEP_OUT, session ) );
                        this.btnStop.setEnabled( ( stopped && target.getFeature( Feature.STOP_EXECUTION, session ) || ( this.docObj.badLine >= 0 ) ) );
                        this.btnPause.setEnabled( false );
				        break;
		        }
		    }
		}
    }
    
	///////////////////////////////////////////////////////////////////////////////
	//
	// Document UI handler
	//
	
	win.document.onLineClick = function( line, area )
	{
		if( !this.rootPane.isClosing )
		{
			switch( area )
			{
				case SourceDocument.AREA_LINENUMBERS:
				{
					if( menus && menus.edit && menus.edit.gotoLine )
					{
						menus.edit.gotoLine.onDisplay();

						if( menus.edit.gotoLine.enabled )
							menus.edit.gotoLine.notify();
					}
				}
				break;
				
				case SourceDocument.AREA_BREAKPOINTS:
					this.docObj.toggleBreakpoint(line);
					break;
			}
		}
	}

    win.document.onUpdateUI = function()
    {
		if( !this.rootPane.isClosing )
		{
			var pos = this.getLogicalPos();
			
			this.docObj.setCursorPos( pos[0], pos[1] );
	  
			if( prefs.autocompletion.getValue( Preference.BOOLEAN ) &&
				pos[1] == this.docObj.currentPos[1]+1 && pos[0] == this.docObj.currentPos[0] )
			{
				if( docMgr.autoCompletion )
					app.cancelTask( docMgr.autoCompletion.autoID );
				
				if( this.docObj.canAutoComplete( this.currentStyle ) )
				{
					var time = prefs.autotime.getValue( Preference.NUMBER );
					var id = app.scheduleTask( "docMgr.startAutoCompletion();", time * 1000, false );      
					docMgr.autoCompletion = { autoDoc : this.docObj , autoID : id };   
				}
			}
			
			this.docObj.currentPos = pos;
		}
    }        

    win.document.onAutocomplete = function( text )
    {
        if( !this.rootPane.isClosing && this.textselection.length == 0 && this.wordRight.length == 0 )
        {
            //
	        // The text format is [Classname.]Elementname[: help text]
	        //
			var fullText	= text.split(": ")[0];
            var classText   = fullText.split('.');
            var elementText = classText.length > 0 ? classText[classText.length-1] : '';
            
            var searchText  = this.wordLeft;

            //
            // Do a case insensitive replacement
            //
            if( elementText.length > 0 && ( !searchText || elementText.toLowerCase().indexOf( searchText.toLowerCase() ) == 0 ) )
            {
				this.docObj.lastAutoComplete = elementText;
				
				//
				// Update the OMV UI
				//
                var target  = this.docObj.getCurrentTarget();
                var session = this.docObj.getCurrentSession();
				
                //
                // Insert the completion by replacing the current search text
                //
				var curSel  = this.getSelection();

				if( searchText )
				{
					this.setSelection( curSel[0], curSel[1]-searchText.length, curSel[0], curSel[1]+searchText.length );
					this.replace( searchText, elementText, Document.FIND_SELECTION );
				}
				else
					this.insert( elementText );

                //
                // Replace will highlight the entire elementText.  We want to put the cursor at the end
                //
                curSel      = this.getSelection();
                this.setSelection( curSel[0], curSel[1]+elementText.length, curSel[0], curSel[1]+elementText.length );
            }
        }
    }
     
    win.document.onMouseOver = function( line, text, start )
	{
		if( !text || !start || !prefs.dynamicHelp.getValue( Preference.BOOLEAN ) )
		{
			this.helpTip = "";
			this.helpTipData = "";
			return;
		}
 
		if( !this.rootPane.isClosing && prefs.dynamicHelp.getValue( Preference.BOOLEAN ) )
		{
			var session = this.docObj.getCurrentSession();
			
			if( session && session.initialized() )
			{
				try
				{
					var job		= session.sessionObj.getValue( '', text, prefs.databrowser.maxArrayElements.getValue( Preference.NUMBER ) );
					job.doc		= this;
					job.varText	= text;

					this.helpTipData = "";
					
					job.onResult = function()
					{
						var variable = this.result[0];
						
						if( variable )
						{
							if( variable.type == Variable.TYPE_FUNCTION )
							{
								this.doc.helpTip = 'Function ' + variable.name;
							}
							else if( variable.ty == Variable.TYPE_UNDEFINED )
							{
								this.doc.helpTip = 'undefined';
							}
							else
							{
								var value = '';
	
								if( typeof( variable.value ) == "undefined" )
									value = "undefined";
								else
									value = variable.value.toString();
								
								if( variable.type == Variable.TYPE_STRING )
								{
									value = '"' + app.escape( value ) + '"';
									
									// Truncate a string after 20 characters
									if( value.length > 20 )
										value = value.substr( 0, 20 ) + '"...';
								}
								
								var dataType = '';
								
								switch( variable.type )
								{
									case Variable.TYPE_BOOLEAN: dataType = 'Boolean';   break;
									case Variable.TYPE_NUMBER:  dataType = 'Number';    break;
									case Variable.TYPE_STRING:  dataType = 'String';    break;
									case Variable.TYPE_BOOLEAN: dataType = 'Boolean';   break;
									default:                    dataType = 'Object';
								}
								
								this.doc.helpTip = dataType + ' ' + variable.name + ' = ' + value;
								this.doc.helpTipData = this.varText;
							}
						}
						else
							this.doc.helpTip = "";
					}
					
					job.onError = function()
					{
						this.doc.helpTip = "";
					}

					job.submit();

					if( line < 0 )
						app.pumpEventLoop( false, true );
				}
				catch( exc )
				{}
			}
		}
	}

    win.document.onActivate = function()
    {
//print( "{" + this.docObj.scriptID + "} onActivate()" );	
        if( !this.rootPane.isClosing && !this.inDocActivating )
        {
            this.inDocActivating = true;
            
	        if( this.docObj.paneTitle )
	        {	
		        if( this.docObj != document )
		        {
			        document = this.docObj;

			        if( !appInShutDown )
			            globalBroadcaster.notifyClients( 'activeDocChanged' );
		        }

                this.rootPane.updateToolbarState();
                
                if( !this.rootPane.active )
                    this.rootPane.active = true;
            }
            
            this.inDocActivating = false;
        }
    }
/*
    win.document.onDeactivate = function()
    {
	    if( !this.rootPane.isClosing && this.rootPane.updateToolbarState )
		    this.rootPane.updateToolbarState();
    }
*/

	win.document.cbMakeReadWrite = function()
	{
		this.readonly = false;
	}

	win.document.onKeyPressed = function( chr )
	{
		if( !this.rootPane.isClosing )
		{
			//
			// If we are debugging, we need to stop
			//
			var currSession = this.docObj.getCurrentSession();

			if( currSession && currSession.isDebugging( this.docObj ) )
			{
				if( dsaQueryBox( "doc3", "$$$/ESToolkit/Alerts/DebuggingUnchanged=The script of the document %1 is in debug mode.^nDo you want to stop debugging to enter your changes?", decodeURIComponent( this.docObj.fileName ) ) )
				{
					currSession.stop();
				}
				else
				{
					this.readonly = true;
					addDelayedTask( this, this.cbMakeReadWrite );
				}
			}
		}
	}

    win.document.onChange = function( position, length, startLine, endLine, lines )
    {
		if( !this.rootPane.isClosing )
		{
			//
			// update "bad" marked line
			//
			this.docObj.setBadline();
			
			//
			// update document title
			//
			if( this.docObj.paneTitle )
				this.docObj.setTitle();

			//
			// invoke function list update 
			//
			var delay = 1500;

			if( lines == 0 )
				delay = 5000;

			if( this.docObj.fnScanTaskId != undefined ) 
				app.cancelTask( this.docObj.fnScanTaskId );

			this.docObj.fnScanTaskId = app.scheduleTask( "var __doc__ = docMgr.find('" + this.docObj.scriptID + "'); if( __doc__ != undefined ) __doc__.fnScan();", delay );

			//
			// autocompletion
			//
			var pos = this.getLogicalPos();
	  
			if( prefs.autocompletion.getValue( Preference.BOOLEAN ) &&
				pos[1] == this.docObj.currentPos[1]+1 && pos[0] == this.docObj.currentPos[0] )
			{
				if( docMgr.autoCompletion )
					app.cancelTask( docMgr.autoCompletion.autoID );
				
				if( this.docObj.canAutoComplete( this.currentStyle ) )
				{
					var time = prefs.autotime.getValue( Preference.NUMBER );
					var id = app.scheduleTask( "docMgr.startAutoCompletion();", time * 1000, false );      
					docMgr.autoCompletion = { autoDoc : this.docObj , autoID : id };   
				}
			}
			
			this.docObj.currentPos = pos;
			
			//
			// backup
			//
			this.docObj.bkModified = true;
			
			if( !this.docObj.duplicate && !this.docObj.backup )
			{
				var backupDelay = prefs.document.backupDelay.getValue( Preference.NUMBER );
				
				if( !isNaN( backupDelay ) && backupDelay > 0 )
				{
					if( this.docObj.backupTaskID > -1 )
						cancelScheduledTask( this.docObj.backupTaskID );
						
					this.docObj.backupTaskID = addScheduledTask( backupDelay * 1000 , this.docObj, this.docObj.cbBackupDocument );
				}
			}
		}
    }

    win.document.onLineNumbersChanged = function()
    {
		if( !this.rootPane.isClosing )
		{
			var currTarget  = this.docObj.getCurrentTarget();
			var currSession = this.docObj.getCurrentSession();
			
			if( currTarget                                                       &&
				currTarget.getFeature( Feature.SET_BREAKPOINTS, currSession )    &&
				currTarget.getFeature( Feature.REMOVE_BREAKPOINTS, currSession )    )
			{
				breakpoints.updateFromDoc( this.docObj );
			}
		}
    }

    //
    // properties
    //
    this.rootPane   = win;
    this.editor     = win.document;
	
    if( !this.duplicate )
		this.clearUndo();
	    
	this.updateFlyout();
	
    globalBroadcaster.registerClient( this, 'shutdown,syntaxPrefsChanged,autoCompletionPrefsChanged,addSessionBreakpoints,functionListChanged,dynamicHelpPrefsChanged,newPrefs' );
    targetMgr.registerClient( this, 'startConnect,endConnect,changeActiveTarget,changeActiveSession,addTarget,changeTargets,removeSessions,changeSessions,targetDied,changeConnectionState,changingConnectionState' );
    DebugSession.registerClient( this, 'state' );

    targetMgr.setIsChanging( true ); 
    {   
        this.setupTargets();
        this.setupEngines();
    }
    targetMgr.setIsChanging( false );    
    					
	if( !appInShutDown )
	{
	    globalBroadcaster.notifyClients( 'activeDocChanged' );
	    globalBroadcaster.notifyClients( 'numDocsChanged' );
	}
    
    if( this.duplicate )
    {
        this.badLine = this.master.badLine;
        this.setBadline( this.badLine, true );
        
        var session = this.getCurrentSession();
        
        if( session )
            session.updateDuplicate( this );
    }
}

//-----------------------------------------------------------------------------
// 
// setBlockCodeChange(...)
// 
// Purpose: Ignore changes of editor
// 
//-----------------------------------------------------------------------------

SourceDocument.prototype.setBlockCodeChange = function( block, masterCommand )
{
    if( this.duplicate && !masterCommand )
        this.master.setBlockCodeChange( block );
    else
    {
        if( block )
        {
            if( this.blockCodeChange <= 0 )
            {
                this.blockedCodeChangeHdl           = this.editor.onChange;
                this.blockedLineChangeHdl           = this.editor.onLineNumbersChanged;
                this.editor.onChange                = undefined;
                this.editor.onLineNumbersChanged    = undefined;
            }
            
            this.blockCodeChange++;
        }
        else
        {
            this.blockCodeChange--;
            
            if( this.blockCodeChange <= 0 )
            {
                this.blockCodeChange                = 0;
                this.editor.onChange                = this.blockedCodeChangeHdl;
                this.editor.onLineNumbersChanged    = this.blockedLineChangeHdl;
                this.blockedCodeChangeHdl           = undefined;
                this.blockedLineChangeHdl           = undefined;
            }
        }

        for( var i=0; i<this.duplicates.length; i++ )
            this.duplicates[i].setBlockCodeChange( block, true );
    }
}

//-----------------------------------------------------------------------------
// 
// revert(...)
// 
// Purpose: Revert last changes
// 
//-----------------------------------------------------------------------------

SourceDocument.prototype.revert = function( autoIndent )
{
    var oldind                  = autoIndent;
    
    this.setBlockCodeChange( true );
    
    //
    // revert text changes
    //
    this.undo();
    
    this.autoIndent             = oldind;
    
    this.setBlockCodeChange( false );
    
    //
    // revert title
    //
    this.setTitle();
    
    //
    // set colored line
    //
    var s = this.getCurrentSession();
    
    if( s )
        s.setLineColor( this );

    //
    // force visual update, otherwise an update could cause another
    // change event of the editor
    //
    this.editor.update();        
    
    //
    // delayed reset onChange handler
    //
    addScheduledTask( 100, this, this._revert_ );
}

SourceDocument.prototype._revert_ = function()
{
    //
    // reset onChange handler
    //
    this.setBlockCodeChange( false );
    
    //
    // set keyboard focus
    //
    this.editor.active = true;
}

//-----------------------------------------------------------------------------
// 
// isDirty(...)
// 
// Purpose: Return dirty state of the document
// 
//-----------------------------------------------------------------------------

SourceDocument.prototype.isDirty = function()
{
	return this.editor.isModified();
}

//-----------------------------------------------------------------------------
// 
// setSavePoint(...)
// 
// Purpose: Set save point for undo stack
// 
//-----------------------------------------------------------------------------

SourceDocument.prototype.setSavePoint = function()
{
    this.editor.setSavePoint();

	// remove backup file
	if( !this.duplicate && this.backup )
	{
		if( this.backupTaskID > -1 )
			cancelScheduledTask( this.backupTaskID );
		
		this.backupTaskID = -1;
		
		this.backup.remove();
		this.backup = null;
	}
}

//-----------------------------------------------------------------------------
// 
// clearUndo(...)
// 
// Purpose: Clear undo stack
// 
//-----------------------------------------------------------------------------

SourceDocument.prototype.clearUndo = function()
{
    this.editor.clearUndo();
}

//-----------------------------------------------------------------------------
// 
// setText(...)
// 
// Purpose: get/set text in editor
// 
//-----------------------------------------------------------------------------

SourceDocument.prototype.setText = function( source, silent, masterCommand )
{
    if( this.editor.text != source )
    {
        if( this.duplicate && !masterCommand )
            this.master.setText( text, silent );
        else
        {
            if( typeof silent == 'undefined' )
                silent = true;
            
            if( silent )
                this.setBlockCodeChange( true );
                
            this.editor.text           = source;
            
            if( silent )
            {
                this.editor.setSavePoint();
                this.setBlockCodeChange( false );
            }
            
            for( var i=0; i<this.duplicates.length; i++ )
                this.duplicates[i].setText( text, silent, true );
        }
    }
}

SourceDocument.prototype.getText = function()
{
	return this.editor.text;
}

//-----------------------------------------------------------------------------
// 
// setLanguage(...)
// 
// Purpose: Set the lexer language.  Also, pass the list of keywords to
//			Auto Completion helper.
// 
//-----------------------------------------------------------------------------

SourceDocument.prototype.setLanguage = function( langID )
{
    if( langID != this.langID )
    {
        //
        // get language data for id
        //
        var xml = lang.getLexerAndStyles( langID );
    	
        //
        // get the default style (which is #32)
        //
        var style   = xml.xpath( 'style[@index=32]' );
        var obj     = lang.getStyleValues( style );
        
        this.editor.clearStyles( obj.fore, obj.back, obj.name, obj.size, obj.bold, obj.italics );
        this.setStyles( xml.style );
    	
        //
        // Convert the keywords to a simple array
        //
        var kwds = [];
    	
        for( var i=0; i<xml.keywords.length(); i++ )
            kwds[ Number( xml.keywords[i].@index ) ] = xml.keywords[i].toString();

        //
        // set lexer
        //
        this.editor.setLexer( xml.lexer.toString(), kwds, xml.wordChars.toString() );
    	
        //
        // pass keywords to Auto Completion helper
        //
        this.acHelper.setKeywords (kwds);
    	
        //
        // store my language identifier
        //
        this.langID = langID;
    }

    this.status = undefined;
}

//-----------------------------------------------------------------------------
// 
// activate(...)
// 
// Purpose: Activate the editor
// 
//-----------------------------------------------------------------------------
var __docActivateID__ = -1;

SourceDocument.prototype.activate = function( delayed )
{
//print( "{" + this.scriptID + "} activate()   active=" + this.rootPane.active +"\n"+$.stack+"\n\n");

	try
	{
		if( !this.rootPane.isClosing )
		{
			var prevDoc = document;
			
			if( this != document )
			{
				document = this;

			}
				
			if( !this.rootPane.active )
			{
				if( !workspace.isDocumentMinimized( this.rootPane ) )
					this.rootPane.active = true;
			}
			else
				this.rootPane.updateToolbarState();
			
			// activate editor after the window got activated!
			
			if( !delayed )
			{
				if( !workspace.isDocumentMinimized( this.rootPane ) )
					this.editor.active = true;
			}
			else
			{
				if( __docActivateID__ >= 0 )
				{
					app.cancelTask(__docActivateID__);
					__docActivateID__ = -1;
				}

				var dupID = -1;

				if( this.duplicate )
					dupID = this.getDuplicateIndex();

				__docActivateID__ = app.scheduleTask( """var di = """ + dupID + """;
														 var __doc__ = docMgr.find('""" + this.scriptID + """'); 
														 if( __doc__ && __doc__ instanceof SourceDocument ) 
														 { 
															if( di >= 0 )
															{
																if( __doc__.duplicate )
																	__doc__ = __doc__.master;

																__doc__ = __doc__.duplicates[di];
															}

															if( __doc__ && __doc__ instanceof SourceDocument ) 
															{
																docMgr.suspendHandler_activate();

																if( _win )
																	__doc__.editor.active=true; 
																else
																	workspace.setActiveDocumentFocus(true);
																__docActivateID__ = -1;
																
																docMgr.resumeHandler_activate();
															}
														 }""", 10 );
			}
								 
			if( document != prevDoc && !appInShutDown )
				globalBroadcaster.notifyClients( 'activeDocChanged' );
		}
	}
	catch( exc )
	{}
}

SourceDocument.prototype.deactivate = function()
{
	try
	{
		if( !this.rootPane.isClosing )
			this.editor.active = false;
	}
	catch( exc )
	{}
}

//-----------------------------------------------------------------------------
// 
// canClose(...)
// 
// Purpose: Is it allowed to close the document?
// 
//-----------------------------------------------------------------------------

SourceDocument.prototype.canClose = function()
{
    var ret = true;
    
    if( !this.duplicate )
    {
        var currSession = this.getCurrentSession();
        
        if( currSession && currSession.isDebugging( this ) )
        {
            docMgr.setStatusLine( '$$$/ESToolkit/Status/StopDebugForClose=Stop debugging before closing document', this ); 
            ret = false;
        }
    }
    
    return ret;
}
    
//-----------------------------------------------------------------------------
// 
// addTarget(...)
// 
// Purpose: add new target to list
// 
//-----------------------------------------------------------------------------

SourceDocument.prototype.addTarget = function( target )
{
	try
	{
		var oldSelection = null;
	    
		if( this.rootPane.targetDDL.selection )
			oldSelection = this.rootPane.targetDDL.selection;
	        
		this.rootPane.setChangingTargetEngine( true );
	    
		var item                          = this.rootPane.targetDDL.add( 'item', target.getTitle() );                
		item.target                       = target;
	    
		this.rootPane.targetDDL.selection = oldSelection;

		this.rootPane.setChangingTargetEngine( false );
	}
	catch( exc )
	{}
}

//-----------------------------------------------------------------------------
// 
// setupTargets(...)
// 
// Purpose: setup targets list
// 
//-----------------------------------------------------------------------------

SourceDocument.prototype.setupTargets = function()
{
	try
	{
		this.rootPane.setChangingTargetEngine( true );
	    
		this.rootPane.targetDDL.removeAll();
		this.rootPane.engineDDL.removeAll();

		var alltargets = targetMgr.getTargets();
	    
		if( alltargets )
		{
			for( var i=0; i<alltargets.length; i++ )
				this.addTarget( alltargets[i] );

			this.selectActiveTarget( targetMgr.getActiveTarget() );
		}

		this.rootPane.setChangingTargetEngine( false );
	}
	catch( exc )
	{}
}

//-----------------------------------------------------------------------------
// 
// setupEngines(...)
// 
// Purpose: setup engines list
// 
//-----------------------------------------------------------------------------

SourceDocument.prototype.setupEngines = function( targetObj )
{
	try
	{
		if( !targetObj )
			targetObj = targetMgr.getActiveTarget();
	        
		if( !this.populateEngineDDL( targetObj ) )
		{
			this.rootPane.setChangingTargetEngine( true );
			{
				this.rootPane.engineDDL.selection = 0;     
			}
			this.rootPane.setChangingTargetEngine( false );
		}
	}
	catch( exc )
	{}
}

//-----------------------------------------------------------------------------
// 
// populateEngineDDL(...)
// 
// Purpose: Add engine names to dropdown list
// 
//-----------------------------------------------------------------------------

SourceDocument.prototype.populateEngineDDL = function( targetObj )
{
	var ret = false;

	try
	{
		if( targetObj)
		{
			//
			// remember selection
			//
			var selAdr = null;

			if( this.rootPane.engineDDL.selection				&&
				this.rootPane.engineDDL.selection.engine		&&
				this.rootPane.engineDDL.selection.engine.address	)
				selAdr = this.rootPane.engineDDL.selection.engine.address.toString();

			//
			// populate DDL
			//
			this.rootPane.setChangingTargetEngine( true );
			this.rootPane.engineDDL.removeAll();        

			var sessions = targetObj.getSessions();
	        
			if( sessions )
			{
				for( var i=0; i<sessions.length; i++ )
				{
					var item = this.rootPane.engineDDL.add( 'item', sessions[i].address.engine );
					item.engine = sessions[i];
				}
			}

			//
			// restore selection
			//
			if( selAdr )
			{
				for( var f=0; f<this.rootPane.engineDDL.items.length; f++ )
				{
					if( this.rootPane.engineDDL.items[f].engine.address.toString() == selAdr )
					{
						this.rootPane.engineDDL.selection = this.rootPane.engineDDL.items[f];
						ret = true;
						break;
					}
				}
			}

			this.rootPane.setChangingTargetEngine( false );
		}
	}
	catch( exc )
	{}

	return ret;
}

//-----------------------------------------------------------------------------
// 
// selectActiveTarget(...)
// 
// Purpose: select active target
// 
//-----------------------------------------------------------------------------

SourceDocument.prototype.selectActiveEngine = function( sessionObj )
{
	if( sessionObj && sessionObj.target == this.getCurrentTarget() )
	{
		if( !this.rootPane.engineDDL.selection || 
			( this.rootPane.engineDDL.selection && 
			  this.rootPane.engineDDL.selection.engine != sessionObj ) )
		{
			this.rootPane.setChangingTargetEngine( true );
            
			for( var i=0; i<this.rootPane.engineDDL.items.length; i++ )
			{
				if( this.rootPane.engineDDL.items[i].engine == sessionObj )
				{
					this.rootPane.engineDDL.selection = this.rootPane.engineDDL.items[i];
					break;
				}
            }
                   
			this.rootPane.setChangingTargetEngine( false );
		}
	}
}

SourceDocument.prototype.selectActiveTarget = function( activeTargetObj )
{
	function selectEngine (window, activeTargetObj)
	{
		if( !window.engineDDL.selection || 
			( window.engineDDL.selection && 
			  window.engineDDL.selection.engine != activeTargetObj.getActive() ) )
		{
			window.setChangingTargetEngine( true );
            
			for( var i=0; i<window.engineDDL.items.length; i++ )
			{
				if( window.engineDDL.items[i].engine == activeTargetObj.getActive() )
				{
					window.engineDDL.selection = window.engineDDL.items[i];
					break;
				}
            }
                   
			window.setChangingTargetEngine( false );
		}
	}

    if( activeTargetObj )
    {
        var isActiveDocument = ( this == document || this.master == document );
        
        if( !isActiveDocument && !this.duplicate )
        {
            for( var i=0; i<this.duplicates.length && !isActiveDocument; i++ )
                isActiveDocument = ( this.duplicates[i] == document );
        }
        
		try
		{
			if( isActiveDocument )
			{
				//
				// handle active (top-most) document
				//
	        
				if( !this.rootPane.targetDDL.selection || 
					( this.rootPane.targetDDL.selection && 
					  this.rootPane.targetDDL.selection.target != activeTargetObj ) )
				{
					//
					// set target at DropDownList
					//
					this.rootPane.setChangingTargetEngine( true );
	                
					for( var i=0; i<this.rootPane.targetDDL.items.length; i++ )
					{
						if( this.rootPane.targetDDL.items[i].target == activeTargetObj )
						{
							this.rootPane.targetDDL.selection = this.rootPane.targetDDL.items[i];
							break;
						}
					}
	                        
					this.rootPane.setChangingTargetEngine( false );
				}
	            
				if( activeTargetObj.getConnected() || activeTargetObj.isDefault())
				{                
					this.populateEngineDDL( activeTargetObj );
	                
					//
					// target is connected, select active engine in DropDownList
					// if not selected, yet
					//
					selectEngine (this.rootPane, activeTargetObj);
				}
				else
				{
					//
					// target not yet connected
					//
					this.rootPane.engineDDL.removeAll();
				}
			}
			else
			{
				//
				// handle not-active document
				//
	        
				if( this.rootPane.targetDDL.selection && this.rootPane.targetDDL.selection.target == activeTargetObj )
				{
					if( !activeTargetObj.getConnected() && !activeTargetObj.isDefault() )
					{
						//
						// target not yet connected
						//
						this.rootPane.engineDDL.removeAll();
					}
				}
			}

			//
			// update the HREF for code completion
			//
			if( _win )
				workspace.storeFocus();

			var currTarget  = this.getCurrentTarget();
			var currSession = this.getCurrentSession();
			this.acHelper.setTargetAndSession( currTarget, currSession );

			if( _win )
				workspace.restoreFocus();

		}
		catch( exc )
		{}
    }
}

//-----------------------------------------------------------------------------
// 
// getCurrentTarget(...)
// 
// Purpose: Get current target or session
// 
//-----------------------------------------------------------------------------

SourceDocument.prototype.getCurrentTarget = function()
{
    var target = null;
    
	try
	{
		if( this.rootPane.targetDDL.selection )
			target = this.rootPane.targetDDL.selection.target;
	}
	catch( exc )
	{}

    return target;
}

SourceDocument.prototype.getCurrentSession = function()
{
    var session = null;
    
	try
	{
		if( this.rootPane.engineDDL.selection )
			session = this.rootPane.engineDDL.selection.engine;
	}
	catch( exc )
	{}
        
    return session;
}
    
//-----------------------------------------------------------------------------
// 
// setEngineState(...)
// 
// Purpose: Reflect state of current engine
// 
//-----------------------------------------------------------------------------

SourceDocument.prototype.setEngineState = function( forSession )
{
    var session = this.getCurrentSession();
    
    if( session == forSession )
    {
		try
		{
			var item = null;
	        
			for( var i=0; i<this.rootPane.engineDDL.items.length; i++ )
				if( this.rootPane.engineDDL.items[i].engine == forSession )
					item = this.rootPane.engineDDL.items[i];
	                
			if( item )
			{
				var icon = this.rootPane.imgstate.iconInactive;

				switch( session.state )
				{
					case DebugSession.RUNNING:	icon = this.rootPane.imgstate.iconRun;  break;
					case DebugSession.STOPPED:	icon = this.rootPane.imgstate.iconStop; break;
					case DebugSession.WAITING:	icon = this.rootPane.imgstate.iconWait; break;
				}
	            
				if( 0 )
					item.icon = icon;
	            
				if( this.rootPane.engineDDL.selection && this.rootPane.engineDDL.selection == item )
					this.rootPane.imgstate.icon = icon;     
				else               
					this.rootPane.imgstate.icon = this.rootPane.imgstate.iconInactive;
			}
		}
		catch( exc )
		{}
    }
}

//-----------------------------------------------------------------------------
// 
// setLineStatus(...)
// 
// Purpose: Set line color of passed line
// 
//-----------------------------------------------------------------------------

SourceDocument.prototype.setLineStatus = function( line, color, masterCommand )
{
    if( this.duplicate && !masterCommand )
        this.master.setLineStatus( line, color );
    else
    {
        this.editor.setCurrentLine( line, color );
        
        for( var i=0; i<this.duplicates.length; i++ )
            this.duplicates[i].setLineStatus( line, color, true );
    }
}

//-----------------------------------------------------------------------------
// 
// updateFlyout(...)
// 
// Purpose: Update the flyout menu
// 
//-----------------------------------------------------------------------------

SourceDocument.prototype.updateFlyout = function( masterCommand )
{
    if( this.duplicate && !masterCommand )
        this.master.updateFlyout();
    else
    {
        if( this.menu )
            this.menu.remove();
            
		try
		{
			this.menu = this.rootPane.btnFlyout.menu = new MenuElement( "popupmenu", "Flyout" );
	        
			// supply the AutoCompletion instance, and the menu
			OMVData.updateDocFlyout (this.acHelper, this.menu);

			var fctList = ( this.duplicate ? this.master.functionList : this.functionList );

			if( fctList )
			{
				var item = null;
	            
				for( var i=0; i<fctList.length; i++ )
				{
					item = new MenuElement( "command", 
											fctList[i].nameLong,
											( i==0 ? "-":"") + "at the end of " + this.menu.id );
					item.fnLine		= fctList[i].line;
					item.fnPosition = fctList[i].position;
					item.document   = this;
	                
					item.onSelect = function()
					{
						this.document.editor.setSelection( this.fnLine, this.fnPosition, this.fnLine, this.fnPosition, true );
						this.document.editor.rootPane.active=false;
						this.document.activate();
					}
				}
			}
		}
		catch( exc )
		{}
        
        for( var i=0; i<this.duplicates.length; i++ )
            this.duplicates[i].updateFlyout( true );
    }                                            
}

///////////////////////////////////////////////////////////////////////////////
//
// profiling
//

SourceDocument.prototype.clearProfileData = function( masterCommand )
{
    if( this.duplicate && !masterCommand )
        this.master.clearProfileData();
    else
    {
        this.editor.clearProfileData();
        
        for( var i=0; i<this.duplicates.length; i++ )
            this.duplicates[i].clearProfileData( true );
    }
}

SourceDocument.prototype.eraseProfileData = function( masterCommand )
{
    if( this.duplicate && !masterCommand )
        this.master.eraseProfileData();
    else
    {
		this.profileData        = {};
		this.profileDataCount   = 0;
		this.profileLevel		= ProfileData.LEVEL_NONE;

        this.editor.profiling = 0;
        this.editor.clearProfileData();
        
        for( var i=0; i<this.duplicates.length; i++ )
            this.duplicates[i].eraseProfileData( true );
    }
}

SourceDocument.prototype.updateProfData = function( masterCommand )    
{
    if( this.duplicate && !masterCommand )
        this.master.updateProfData();
    else if (0 != this.profileDataCount)
    {
        for( var line in this.profileData )
            this.editor.setProfileData( line, this.profileData[line].hits, this.profileData[line].time );

        this.editor.profiling = 0;     // switch to 0 before to force a redraw
        
        var display = prefs.profiling.profDisplayMode.getValue( Preference.NUMBER );
        
        if( display > 0 )
        {
            var level                = prefs.profiling.profileLevel.getValue( Preference.NUMBER );
            this.editor.profiling    = ( display == 1 ? level-1 : level );
        }
        
        for( var i=0; i<this.duplicates.length; i++ )
            this.duplicates[i].updateProfData( true );
    }
}

SourceDocument.prototype.addProfileData = function( level, data, masterCommand )
{
    if( this.duplicate && !masterCommand )
        this.master.addProfileData( level, data );
    else
    {
		if (!this.profileData [data.line])
			this.profileDataCount++;
		
		this.profileData [data.line] = data;
		this.profileLevel			 = level;
        
        for( var i=0; i<this.duplicates.length; i++ )
            this.duplicates[i].addProfileData( level, data, true );
    }
}

//-----------------------------------------------------------------------------
// 
// onNotify(...)
// 
// Purpose: notify handler
// 
//-----------------------------------------------------------------------------

SourceDocument.prototype.onNotify = function( reason )
{    
    var currTarget  = this.getCurrentTarget();
    var currSession = this.getCurrentSession();

    switch( reason )
    {
		case 'filechanged' :
		{
		    var newFile = arguments[1];
	
		    if( newFile.absoluteURI == File( this.scriptID ).absoluteURI )
		    {
				var reload = true;
				
                var currSession = this.getCurrentSession();

                // If we are debugging, we need to stop
                if( currSession && currSession.isDebugging( this ) )
                {
					if( dsaQueryBox( "doc1", "$$$/ESToolkit/Alerts/DebugreloadChanged=File %1^nhas been changed.^nDo you want to stop debugging to reload it?",
									      decodeURIComponent( newFile.fsName ) ) )
					{
					    currSession.stop( null, true );
					}
				}
                else if( this.isDirty() )
                {
					// Always ask if the file has been modified
					reload = dsaQueryBox( "doc1", "$$$/ESToolkit/Alerts/AutoreloadChanged=File %1^nhas been changed.^nDo you want to reload it and lose your changes?",
									      decodeURIComponent( newFile.fsName ) );
									      
					if( reload )
					    this.setSavePoint();
			    }
				else if( !prefs.document.autoReload.getValue( Preference.BOOLEAN ) )
				{
					// Autoreload off: ask if not modified
					reload = dsaQueryBox( "doc2", "$$$/ESToolkit/Alerts/AutoreloadUnchanged=File %1^nhas been changed.^nDo you want to reload it?",
									      decodeURIComponent( newFile.fsName ) );
				}
				
				if( reload )
				    this.reload();
			}
		}
		break;
		
        case 'startConnect':
        case 'endConnect':
		case 'targetDied':
        {
            if( currTarget == arguments[1] )
            {
                this.selectActiveTarget( arguments[1] );
				try
				{
					this.rootPane.updateToolbarState();
				}
				catch( exc )
				{}
            }
        }
        break;
        
        case 'state':
        {
            if( currSession == arguments[1] )
            {
                this.setEngineState( arguments[1] );
				try
				{
					this.rootPane.updateToolbarState();
				}
				catch( exc )
				{}

				if( !this.rootPane.isClosing							&& 
					prefs.dynamicHelp.getValue( Preference.BOOLEAN )	&&
					this.editor.helpTipData									)
					addDelayedTask( this.editor, this.editor.onMouseOver, -1, this.editor.helpTipData, true );
            }
        }
        break;
		
		case 'changeConnectionState':
		case 'changingConnectionState':
		{
			try
			{
				if( currTarget == arguments[1] )
					this.rootPane.updateToolbarState( true );
			}
			catch( exc )
			{}
		}
		break;
        
        case 'changeActiveTarget':
        {        
            //
            // active session (and probably the target) changed at target manager
            //            
            this.selectActiveTarget( arguments[1] );

			try
			{
				this.rootPane.updateToolbarState();
			}
			catch( exc )
			{}
        }
        break;
        
        case 'changeActiveSession':
        {        
            //
            // active session (and probably the target) changed at target manager
            //            
			if( currTarget != arguments[1] )
				this.selectActiveTarget( arguments[1] );
			else
				this.selectActiveEngine( currTarget.getActive() );

			try
			{
				this.rootPane.updateToolbarState();
			}
			catch( exc )
			{}
        }
        break;
        
        case 'addTarget':               // new target added
		{
			this.addTarget( arguments[1] );

			try
			{
				this.rootPane.updateToolbarState();
			}
			catch( exc )
			{}
		}
        break;

        case 'changeTargets':           // new targets list
            this.setupTargets();
            break;

		case 'removeSessions':
        case 'changeSessions':           // new engines list
		{
			if( currTarget == arguments[1] )
			{
				this.setupEngines( arguments[1] );

				try
				{
					this.rootPane.updateToolbarState();
				}
				catch( exc )
				{}
			}
		}
		break;

		case 'autoCompletionPrefsChanged':
			if (docMgr.autoCompletion)
			{
				// cancel any pending timer
				app.cancelTask (docMgr.autoCompletion.autoID);
				docMgr.autoCompletion = null;
			}
			break;
			
		case 'addSessionBreakpoints':
		{
		    var sessionAddr = arguments[1];
	        var breakpoints = arguments[2];
		    
		    if( currSession && breakpoints                              &&
		        currSession.address.type        == sessionAddr.type     &&
		        currSession.address.target      == sessionAddr.target   &&
		        currSession.address.instance    == sessionAddr.instance &&
		        currSession.address.engine      == sessionAddr.engine       )
		    {
                var lines = this.editor.getBreakpoints();
                			    
                for( var i=0; i<lines.length; i++ )
                {
                    var line    = lines[i];
	                var bp      = this.editor.getBreakpoint( line );
            		
	                if( bp[0] != -1 )
	                {
	                    var bpObj = new Breakpoint( line + 1, this.scriptID, ( bp[0] != 0 ), bp[1], bp[3], bp[2] );
	                    breakpoints.push( bpObj );
	                }
                }
            }			    
		}
		break;
			
		case 'syntaxPrefsChanged':
        {
            var langID = this.langID;
            delete this.langID;
            this.setLanguage(langID);
        }
        break;

		case 'newPrefs':
		{
			this.editor.convertTabs	= prefs.document.tabs2spaces.getValue( Preference.BOOLEAN );
			this.editor.caretBackground	= prefs.document.caretBackground.getValue( Preference.BOOLEAN );
			
			
			var backupDelay = prefs.document.backupDelay.getValue( Preference.NUMBER );
			
			if( !isNaN( backupDelay ) && backupDelay > 0 && this.isDirty() )
				this.backupTaskID = addScheduledTask( backupDelay * 1000, this, this.cbBackupDocument );
		}
		break;
			
		case 'shutdown':
		{
			try
			{
				globalBroadcaster.unregisterClient( this );
				this.rootPane.setChangingTargetEngine( true );
				this.rootPane.targetDDL.removeAll();
				this.rootPane.engineDDL.removeAll();
			}
			catch( exc )
			{}
		}
	    break;

		case 'functionListChanged':
		{
			if (this.functionList)
				this.acHelper.setDocFunctions (this.functionList);
		}
		break;

		case 'dynamicHelpPrefsChanged':
			this.editor.onMouseOver( -1, '' );			
			break;
    }
}

SourceDocument.prototype.canAutoComplete = function( style )
{
    //
    // TODO: this information should come from syntaxdefs.xml
    //
    var ret = true;
    
    switch( style )
    {
        case 1:
        case 2:
        case 3:
        case 6:
        case 7:
        case 12:
        case 13:
        case 15:
            ret = false;
    }
    
    return ret;
}

SourceDocument.prototype.startAutoCompletion = function()
{
    if( this.editor.textselection.length == 0 && this.editor.wordRight.length == 0 )
    {
        var pos = this.editor.getLogicalPos();
		var sel = this.editor.getSelection();
        
        var line        = this.editor.lines[pos[0]];
        var searchStr   = this.editor.wordLeft;

		var isGlobal    = line.charAt( sel[1] - searchStr.length - 1 ) != '.';

        //
        // Don't suggest if we determine the user is defining their own identifier.
        // If prevWord is 'var', 'const', or 'function' - then don't suggest.
        //
        var prevWordPos = sel[1] - searchStr.length - 1;
        var prevWord    = this.editor.getWordAt( pos[2] - line.length + prevWordPos);
        
        while( prevWordPos >= 0 && line.charAt( prevWordPos ) == '.')
        {
            prevWordPos -= ( prevWord.length + 1 );
        
            if( prevWordPos < 0 )
                break;

            prevWord = this.editor.getWordAt( pos[2] - line.length + prevWordPos );
        }
        
        if ( prevWord == 'var' || prevWord == 'const' || prevWord == 'function' )
            return;

		//
        // include classname
        //
		if( !isGlobal )
		{
            var classCandidate = this.editor.getWordAt( pos[2] - searchStr.length - 1 );
            
            if( classCandidate.length > 0 && classCandidate.charCodeAt(0) > 64 && classCandidate.charCodeAt(0) < 91 )
                searchStr = classCandidate + '.' + searchStr;
		}

		if( searchStr.length > 0 && searchStr != this.lastAutoComplete )
		{
			workspace.storeFocus();

			//
            // ask for code suggestions (an array is returned)
	        // every line has the format [Classname.]Elementname[ - helptext]
            //               
			var strs = this.acHelper.suggest( searchStr, isGlobal );
			
			//
            // show auto completion listbox
            //
			if( strs && strs.length > 0 )
				this.editor.showAutoCompletion( strs );
			else
				workspace.restoreFocus();
		}
	}
}

//-----------------------------------------------------------------------------
// 
// insertVersionTag(...)
// 
// Purpose: Insert a Version tag
// 
//-----------------------------------------------------------------------------

SourceDocument.prototype.insertVersionTag = function()
{
    if( this.isFile() )
    {
	    var name = decodeURIComponent( File (this.scriptID).name );
	    var text = "@@@" + "BUILDINFO" + "@@@ " + name + " !Version! " + new Date().toString();
	    
	    if( this.editor.find( "@@@" + "BUILDINFO" + "@@@", 2 ) )
	    {
		    // replace current string
		    var sel = this.editor.getSelection();
		    this.editor.setSelection (sel[0], sel [1], sel[0]+1, 0);
		    this.editor.textselection = text + "\n";
	    }
	    else
	    {
		    var sel = this.editor.getSelection();
		    this.editor.setSelection (sel[0], 0);
		    this.editor.textselection = "/**\n* " + text + "\n*/\n";
	    }
    }
}

//-----------------------------------------------------------------------------
// 
// exportAsBinary(...)
// 
// Purpose: Export script as jsxbin
// 
//-----------------------------------------------------------------------------

SourceDocument.prototype.exportAsBinary = function()
{
    var s;

    // do not save non-documents or docs that are not JS
    if (!this.paneTitle || (this.langID != "js"))
	    return true;

	var f = null;

    if( !this.includePath || ( this.includePath && this.includePath.length == 0 ) )
    {
        f = new File( this.scriptID );
        
        if( f.exists )
			this.includePath = f.parent ? f.parent.absoluteURI : "/";
		else
			f = null;
    }

	if( !this.checkSyntax( this.includePath ) )
	    return false;
    try
    {
		s = app.compile( this.getText(), ( f ? f.absoluteURI : undefined ), this.includePath );
    }
    catch (e)
    {
	    return false;
    }

    if( !this.currentFolder )
        this.currentFolder = app.currentFolder;

    var pos = this.fileName.lastIndexOf ('.');
	
    if( pos < 0 )
	    pos = this.fileName.length;
		
    var proposedName = this.fileName.substr( 0, pos ) + ".jsxbin";
    var f = File( this.currentFolder );
    f.changePath( proposedName );
    f = f.saveDlg( localize( "$$$/ESToolkit/FileDlg/ExportAsBinary=Export To Binary JavaScript" ),
			       localize( "$$$/ESToolkit/FileDlg/JSXBIN=Binary JavaScript files:*.jsxbin" ));
    if(f)
    {
        // remember the folder
        this.currentFolder = app.currentFolder = 
		    f.parent ? f.parent.absoluteURI : "/";

	    // If the file is read-only, abort
	    if (f.readonly)
	    {
		    var msg = _win
				    ? "$$$/ESToolkit/FileDlg/ProtectedWin=Could not save as %1 because the file is locked.^nUse the 'Properties' command in the Windows explorer to unlock the file."
				    : "$$$/ESToolkit/FileDlg/ProtectedMac=Could not save as %1 because the file is locked.^nUse the 'Get Info' command in the Finder to unlock the file.";
		    errorBox( localize( msg, decodeURIComponent( f.name ) ) );
		    return false;
	    }
	    if (f.open ("w"))
	    {
		    f.lineFeed = this.lf;
		    f.encoding = this.encoding;
		    f.write (s);
		    f.close();
			
			try
			{
				// we are good citizen
				app.notifyFileChanged (f);
			}
			catch( exc )
			{}
	    }
	    if (f.error != "")
		    errorBox( localize( "$$$/ESToolkit/FileDlg/CannotWrite=Cannot write to file %1!", decodeURIComponent( f.name ) ) );
	    else
		    return true;
    }
    return false;
}

//-----------------------------------------------------------------------------
// 
// setStyles(...)
// 
// Purpose: Set an XML list of styles
// 
//-----------------------------------------------------------------------------

SourceDocument.prototype.setStyles = function (styles)
{
    if( styles )
    {
	    for( var i=0; i<styles.length(); i++ )
	    {
		    var style = styles [i];
		    // Never set the default style
		    if (style.@index == 32)
			    continue;
		    // <style title fore back color name size bold italics />
		    var obj = lang.getStyleValues (style);
		    this.editor.setStyle (obj.index, obj.fore, obj.back, obj.name, obj.size, obj.bold, obj.italics);
	    }
    }
}


//-----------------------------------------------------------------------------
// 
// getStatus(...)
// 
// Purpose: Return executable state of the document
// 
//-----------------------------------------------------------------------------

SourceDocument.prototype.getStatus = function()
{
    this.status = 'noexec';
    
    var target = this.getCurrentTarget();
    
    if( target && target.isExec( this.langID ) )
        this.status = 'exec';
    
    return this.status
}

//-----------------------------------------------------------------------------
// 
// checkSyntax(...)
// 
// Purpose: Do a syntax check and display the result. Return false on errors.
// 
//-----------------------------------------------------------------------------

SourceDocument.prototype.checkSyntax = function (includes)
{
    if( !includes )
	    includes = this.isFile() ? File (this.scriptID).parent : "";
	
	// get all breakpoints, and create an array of enabled ones
	var allBpLines = this.editor.getBreakpoints();
	var bpLines = [];
	for (var i = 0; i < allBpLines.length; i++)
	{
		var line = allBpLines[i];
		var bp = this.editor.getBreakpoint (line);
		if (bp[0] == 1)
			bpLines.push (line);
	}
    var result = app.checkSyntax( this.getText(), this.scriptID, includes, bpLines );
    
    if( result.error )
    {
	    app.beep();
		
	    var doc = this;
		
	    if( this.scriptID != result.file && result.file.length > 0 )
	    {
	        var f = new File( result.file );
	        doc = docMgr.load( f );
	    }

	    if( result.line1 == result.line2 )
	    {
		    // Set this.badLine, which is removed on the next edit
		    doc.setBadline( result.line1 );
	    }

	    doc.editor.setSelection (result.line1, result.col1, result.line2, result.col2, true);
        docMgr.setStatusLine( result.error, doc );
    }
    else
        docMgr.setStatusLine( '$$$/ESToolkit/Status/NoErrors=No Errors', this );

	// disable any invalid breakpoints
	// The array only contains enabled breakpoints (see above)
	for (i = 0; i < result.badBreakpoints.length; i++)
		this.toggleBreakpoint (result.badBreakpoints [i]);

    return ( result.error ? false : true );
}

//-----------------------------------------------------------------------------
// 
// setBadline(...)
// 
// Purpose: Set/reset bad line 
// 
//-----------------------------------------------------------------------------

SourceDocument.prototype.setBadline = function( line, masterCommand )
{
    if( this.duplicate && !masterCommand )
        this.master.setBadline( line );
    else
    {
        if( !line )
            line = -1;
            
		try
		{
			if( line >= 0 )
			{
				// Set this.badLine, which is removed on the next edit
				this.badLine = line;
				this.setLineStatus( line, colors.Coral );
				this.rootPane.updateToolbarState();
			}
			else
			{
				if( this.badLine )
				{
					this.setLineStatus( this.badLine, colors.White );
					delete this.badLine;
					docMgr.setStatusLine( '', this );
		            
					this.rootPane.updateToolbarState();
				}
			}
		}
		catch( exc )
		{}
        
        for( var i=0; i<this.duplicates.length; i++ )
            this.duplicates[i].setBadline( line, true );
    }
}

//-----------------------------------------------------------------------------
// 
// writePrefs(...)
// 
// Purpose: Write document preferences
// 
//-----------------------------------------------------------------------------

SourceDocument.prototype.writePrefs = function()
{
    // do not save empty files, only disk files
    if( this.isFile() )
    {
        var index = prefs.documents.getLength();
        
        for( var i=0; i<index; i++ )
        {
            if( this.scriptID == prefs.documents[i].scriptID )
            {
                index = i;
                break;
            }
        }
    
        //
        // lexer
        //
	    prefs.documents['documents'+index].langID   = this.langID;

        //
        // view prefs
        //
        prefs.documents['documents'+index].wrap      = this.editor.wrap.toString();
	    prefs.documents['documents'+index].folding   = ( this.editor.folding == 3 ? 'true' : 'false' );
        prefs.documents['documents'+index].lineNum   = this.editor.lineNumbers.toString();
        
        //
        // line endings
        //
        prefs.documents['documents'+index].lineend   = this.lf;
        
        //
        // bookmarks
        //
        prefs.documents['documents'+index].bookmarks = this.editor.bookmarks.toString();

        //
        // documents scriptID
        //
        prefs.documents['documents'+index].scriptID = this.scriptID;    // this need to be the last entry!!
    }    
}

//-----------------------------------------------------------------------------
// 
// readPrefs(...)
// 
// Purpose: Read document preferences
// 
//-----------------------------------------------------------------------------

SourceDocument.prototype.readPrefs = function()
{
    if( !this.restored )
    {
        this.restored               = true;
        
        this.setBlockCodeChange( true );
        
        this.editor.lineNumbers	    = prefs.document.lineNumbers.getValue( Preference.BOOLEAN );
        this.editor.wrap			= prefs.document.wrap.getValue( Preference.BOOLEAN );
        this.editor.folding		    = prefs.document.folding.getValue( Preference.BOOLEAN ) ? 3 : 0;
        this.editor.tabs            = prefs.document.tabs.getValue( Preference.NUMBER );
		this.editor.convertTabs		= prefs.document.tabs2spaces.getValue( Preference.BOOLEAN );
		this.editor.caretBackground	= prefs.document.caretBackground.getValue( Preference.BOOLEAN );
    	
        var docs = prefs.documents.getLength();

        for( var i=0; i<docs; i++ )
        {
            if( prefs.documents['documents'+i].scriptID == this.scriptID )
            {
                //
                // view prefs
                //                                
                this.editor.wrap           = prefs.documents['documents'+i].wrap.getValue( Preference.BOOLEAN );
		        this.editor.folding        = ( prefs.documents['documents'+i].folding.getValue( Preference.BOOLEAN ) ? 3 : 0 );
                this.editor.lineNumbers    = prefs.documents['documents'+i].lineNum.getValue( Preference.BOOLEAN );
                    
                //
                // line endings
                //
                this.lf                    = prefs.documents['documents'+i].lineend.getValue( Preference.STRING );
            	
                if( this.lf.length == 0 )
                    this.lf = Folder.fs.toLowerCase();
    	            
                //
                // bookmarks
                //
                var bmstr = prefs.documents['documents'+i].bookmarks.getValue( Preference.STRING );
                
                if( bmstr.length > 0 )
                {
                    var bms = bmstr.split(',');
                    
                    for( var b=0; b<bms.length; b++ )
                        this.editor.addBookmark( parseInt( bms[b] ), 10 );
                }
                
                break;
            }
        }

        this.setBlockCodeChange( false );        
    }
}

//-----------------------------------------------------------------------------
// 
// getAllBreakpoints(...)
// 
// Purpose: Get a list of all breakpoints. 
// 
//-----------------------------------------------------------------------------

SourceDocument.prototype.getAllBreakpoints = function( enabled )
{
    var bps     = [];
    var lines   = this.editor.getBreakpoints();

    for( var i=0; i<lines.length; i++ )
    {
        var line    = lines[i];
	    var bp      = this.editor.getBreakpoint( line );
		
	    if( bp[0] != -1 )
	        bps.push( new Breakpoint( line, this.scriptID, (bp[0] != 0), bp[1], bp[3], bp[2] ) );
    }
	
    return bps;
}

//-----------------------------------------------------------------------------
// 
// toggleBreakpoint(...)
// 
// Purpose: Toggle a breakpoint from off to enabled to disabled.
// 
//-----------------------------------------------------------------------------

SourceDocument.prototype.toggleBreakpoint = function( line, masterCommand )
{
    if( this.duplicate && !masterCommand )
        this.master.toggleBreakpoint( line );
    else
    {
        //
        // breakpoint array:
        // element 0 : state
        //         1 : hitcount
        //         2 : condition
        //         3 : current hits
        //
        var bp = this.editor.getBreakpoint( line );

        if( !bp )
	        bp = [ -1, 1, '', 0 ];
    		
        switch( bp[0] )
        {
	        case -1:	bp[0] = 1; break;	// hidden -> enabled
	        case 0:		bp[0] = -1; break;	// disabled -> hidden
	        default:	bp[0] = 0;			// enabled -> disabled
        }
    	
        //
        // update document window
        //
        this.editor.setBreakpoint( line, bp[0], bp[1], bp[2], bp[3] );
    	
        //
        // send if connected
        //
        if( !this.duplicate )
            targetMgr.sendBreakpoints();

        //	
        // Update the Breakpoints pane
        //
        breakpoints.updatePane();
        
        for( var i=0; i<this.duplicates.length; i++ )
            this.duplicates[i].toggleBreakpoint( line, true );
    }
}

SourceDocument.prototype.setBreakpoint = function( line, enabled, hits, condition, currHit )
{
	this.editor.setBreakpoint( line, enabled, hits, condition, currHit );
}

//-----------------------------------------------------------------------------
// 
// removeAllBreakpoints(...)
// 
// Purpose: Remove all breakpoints and conditions.
// 
//-----------------------------------------------------------------------------

SourceDocument.prototype.removeAllBreakpoints = function()
{
    var lines = this.editor.getBreakpoints();
	
    for( var i=0; i<lines.length; i++ )
	    this.editor.removeBreakpoint( lines[i] );
		
    if( lines.length )
    {
        //	
        // Update the Breakpoints pane
        //
        breakpoints.updatePane();
    }

	targetMgr.sendBreakpoints();
}

//-----------------------------------------------------------------------------
// 
// removeBreakpoint(...)
// 
// Purpose: Remove single breakpoint
// 
//-----------------------------------------------------------------------------

SourceDocument.prototype.removeBreakpoint = function( line )
{
	this.editor.removeBreakpoint( line );
}

//-----------------------------------------------------------------------------
// 
// fnScan(...)
// 
// Purpose: Scan document for function definitions
// 
//-----------------------------------------------------------------------------

SourceDocument.prototype.fnScan = function()
{
	//
	// for now only JavaScript is supported
	//
	if( this.langID.indexOf( "js" ) == 0 )
	{
		if( this.duplicate )
			this.master.fnScan();
		else
		{
			var currTarget = this.getCurrentTarget();
			if( currTarget )
			{
				if( currTarget.getFeature( Feature.SCAN_FUNCTIONS ) )
				{
					globalBroadcaster.notifyClients( 'functionListScanBegin' );
		            
					var job = currTarget.cdic.scanFunctions( this.getText(), this.scriptID );
					job.doc = this;

					job.onResult = function()
					{
						// if the current document is the same as the one we scanned
						if( docMgr.related( this.doc, document ) )
						{
							if( this.result != undefined )
							{
								this.doc.functionList = this.result;
		    					
								//
								//	This tells the functionList this is a brand new list
								//	and not just the active document changing
								//
								this.doc.functionList.isNew = true;
		    					
								globalBroadcaster.notifyClients( 'functionListChanged' );
							}
						}
						globalBroadcaster.notifyClients( 'functionListScanEnd' );
					}
			        
					job.submit(-1);	// never time out
				}
			}
		}
	}
}

//-----------------------------------------------------------------------------
// 
// onWinActivate(...)
// 
// Purpose: [Callback] The hosting window (or the hosting root panel) gets
//                     activated
// 
//-----------------------------------------------------------------------------

SourceDocument.prototype.onWinActivate = function()
{
    this.setBlockUIUpdate( true );
    this.activate( true ); 

    var currentTargetIsValid = false;

    if( this.rootPane.targetDDL.selection && this.rootPane.engineDDL.selection )
    {
        var target  = this.getCurrentTarget();
        var session = this.getCurrentSession();
    	
        if( target && session && target.findSession( session.address ) )
	    {        
	        currentTargetIsValid = true;
	        
		    //
		    // if target&engine is not active at the moment
		    // set them active
		    //
		    var activeTarget  = targetMgr.getActiveTarget();
		    var activeSession = null;
    		
		    if( activeTarget )
			    activeSession = activeTarget.getActive();
    		
		    if( activeTarget != target || activeSession != session )
		    {
			    targetMgr.setActive( target, session );
		    }
	    }
    }
    else if( this.rootPane.targetDDL.selection && null == this.rootPane.engineDDL.selection )
    {
        var target       = this.getCurrentTarget();
        var activeTarget = targetMgr.getActiveTarget();
        
        if( !target.getConnected() )
        {
            //
            // target isn't connected so we're not 
            // surprised there is no engine selected
            // then set target active if it isn't
            //
            currentTargetIsValid = true;
            
            if( activeTarget != target )
                targetMgr.setActive( target );
        }
    }
    
    if( !currentTargetIsValid )
    {
        //
        // target was not properly set
        // it's probably a new document
        //
        this.selectActiveTarget( targetMgr.getActiveTarget() )
    }
    
    this.setBlockUIUpdate( false );
    this.rootPane.updateToolbarState();
    menus.debug.reflectState();

    var pos = this.editor.getLogicalPos();

    this.setCursorPos( pos[0], pos[1] );

    if (this.functionList)
	    globalBroadcaster.notifyClients( 'functionListChanged' );
    else
    {
        if( this.fnScanTaskId != undefined ) 
            app.cancelTask( this.fnScanTaskId );

        this.fnScanTaskId = app.scheduleTask( "var __doc__ = docMgr.find('" + this.scriptID + "'); if( __doc__ != undefined ) __doc__.fnScan();", 1000 );
    }
    
    this.editor.activeStyle = true;
}

//-----------------------------------------------------------------------------
// 
// onWinDeactivating(...)
// 
// Purpose: [Callback] The hosting window (or hosting root panel) is about
//                     to be deactivated
// 
//-----------------------------------------------------------------------------

SourceDocument.prototype.onWinDeactivating = function()
{
    this.clearCursorPos();
    this.editor.activeStyle = false;
}

//-----------------------------------------------------------------------------
// 
// onWinDeactivated(...)
// 
// Purpose: [Callback] The hosting window (or hosting root panel) got deactivated
// 
//-----------------------------------------------------------------------------

SourceDocument.prototype.onWinDeactivated = function()
{
	this.rootPane.updateToolbarState();
}

//-----------------------------------------------------------------------------
// 
// onWinClosing(...)
// 
// Purpose: [Callback] The hosting window (or hosting root panel) is about
//                     to be closed
// 
//-----------------------------------------------------------------------------
    
SourceDocument.prototype.onWinClosing = function()
{
    globalBroadcaster.unregisterClient( this );            
    targetMgr.unregisterClient( this );            
    DebugSession.unregisterClient( this );            
}

//-----------------------------------------------------------------------------
// 
// onWinClosed(...)
// 
// Purpose: [Callback] The hosting window (or hosting root panel) got closed
// 
//-----------------------------------------------------------------------------

SourceDocument.prototype.onWinClosed = function()
{
	this.functionList = null;
	//
	//	If no document left, update the fn list so it'll empty itself
	//
	globalBroadcaster.notifyClients( 'functionListChanged' );

	try
	{
		this.rootPane.document.docObj = null;
		this.rootPane.document.master = null;
		this.rootPane = null;
		this.editor = null;
	}
	catch( exc )
	{}
	
	// remove backup file
	if( !this.duplicate && this.backup )
	{
		if( this.backupTaskID > -1 )
			cancelScheduledTask( this.backupTaskID );
		
		this.backupTaskID = -1;
		
		this.backup.remove();
		this.backup = null;
	}

	// we are closed (just in case someone keeps a reference to this object)
	this.closed = true;
}

//-----------------------------------------------------------------------------
// 
// canUndo(...)
// 
// Purpose: Can undo/redo
// 
//-----------------------------------------------------------------------------

SourceDocument.prototype.canUndo = function()
{
	var session = this.getCurrentSession();
	
	if( session && session.isDebugging( this ) )
		return false;
		
    return this.editor.canUndo();
}

SourceDocument.prototype.canRedo = function()
{
	var session = this.getCurrentSession();
	
	if( session && session.isDebugging( this ) )
		return false;
		
    return this.editor.canRedo();
}

SourceDocument.prototype.undo = function()
{
    this.editor.undo();
}

SourceDocument.prototype.redo = function()
{
    this.editor.redo();
}

//-----------------------------------------------------------------------------
// 
// getEditor(...)
// 
// Purpose: Return sourcecode editor
// 
//-----------------------------------------------------------------------------

SourceDocument.prototype.getEditor = function()
{
    return this.editor;
}

//-----------------------------------------------------------------------------
// 
// getLines(...)
// 
// Purpose: Return array of source code lines
// 
//-----------------------------------------------------------------------------

SourceDocument.prototype.getLines = function()
{
	return this.editor.lines;
}

//-----------------------------------------------------------------------------
// 
// getTextSelection(...)
// 
// Purpose: Return text selection array
// 
//-----------------------------------------------------------------------------

SourceDocument.prototype.getTextSelection = function()
{
	return this.editor.textselection;
}

//-----------------------------------------------------------------------------
// 
// print(...)
// 
// Purpose: Print source code
// 
//-----------------------------------------------------------------------------

SourceDocument.prototype.print = function( prtObj )
{
	return this.editor.print( prtObj );
}

//-----------------------------------------------------------------------------
// 
// setSelection(...)
// 
// Purpose: Set text selection
// 
//-----------------------------------------------------------------------------

SourceDocument.prototype.setSelection = function( line1, column1, line2, column2, scroll )
{
	// also setSelection( line1, column1, scroll )
	
	if( typeof line2 == "boolean" )
	{
		scroll = line2;
		line2 = undefined;
	}
	
	if( !line2 )	line2 = line1;
	if( !column2 )	column2 = column1;

	this.editor.setSelection( line1, column1, line2, column2, scroll );
}

SourceDocument.prototype.getSelection = function()
{
	return this.editor.getSelection();
}

//-----------------------------------------------------------------------------
// 
// find(...)
// 
// Purpose: find/replace
// 
//-----------------------------------------------------------------------------

SourceDocument.prototype.find = function( searchStr, flags )
{
	return this.editor.find( searchStr, flags );
}

SourceDocument.prototype.replace = function( searchStr, replaceStr, flags )
{
	return this.editor.replace( searchStr, replaceStr, flags );
}

//-----------------------------------------------------------------------------
// 
// scrollSelection(...)
// 
// Purpose: Scroll editor to curretn selection
// 
//-----------------------------------------------------------------------------

SourceDocument.prototype.scrollSelection = function()
{
	this.editor.scrollSelection();
}

//-----------------------------------------------------------------------------
// 
// toggleBookmark(...)
// 
// Purpose: Toggle bookmark
// 
//-----------------------------------------------------------------------------

SourceDocument.prototype.toggleBookmark = function()
{
	this.editor.toggleBookmark();
}

//-----------------------------------------------------------------------------
// 
// removeAllBookmarks(...)
// 
// Purpose: Remove all bookmarks
// 
//-----------------------------------------------------------------------------

SourceDocument.prototype.removeAllBookmarks = function()
{
	this.editor.removeAllBookmarks();
}

//-----------------------------------------------------------------------------
// 
// nextBookmark(...)
// 
// Purpose: Jump to next bookmark
// 
//-----------------------------------------------------------------------------

SourceDocument.prototype.nextBookmark = function()
{
	this.editor.nextBookmark( true );
}

//-----------------------------------------------------------------------------
// 
// previousBookmark(...)
// 
// Purpose: Jump to previous bookmark
// 
//-----------------------------------------------------------------------------

SourceDocument.prototype.previousBookmark = function()
{
	this.editor.nextBookmark( false );
}

//-----------------------------------------------------------------------------
// 
// forceBackupDocument(...)
// 
// Purpose: Force document backup if backup is switched on
// 
//-----------------------------------------------------------------------------

SourceDocument.prototype.forceBackupDocument = function()
{
	var backupDelay = prefs.document.backupDelay.getValue( Preference.NUMBER );
	
	if( !isNaN( backupDelay ) && backupDelay > 0 )
	{
		if( this.backupTaskID > -1 )
			cancelScheduledTask( this.backupTaskID );

		this.doBackupDocument( true );
	}
}

//-----------------------------------------------------------------------------
// 
// cbBackupDocument(...)
// 
// Purpose: Delayed document backup
// 
//-----------------------------------------------------------------------------

SourceDocument.prototype.cbBackupDocument = function()
{
	if( !this.closed )
		this.doBackupDocument( ( this.backupTaskID > -1 && this.isDirty() && this.bkModified ) );
}

//-----------------------------------------------------------------------------
// 
// doBackupDocument(...)
// 
// Purpose: Do actual document backup
// 
//-----------------------------------------------------------------------------

SourceDocument.prototype.doBackupDocument = function( doBackup )
{
	if( !this.closed )
	{
		if( doBackup )
		{
			this.bkModified = false;
			
			if( !this.backup )
			{
				var tmp = new File( this.fileName );

				if( tmp.absoluteURI.indexOf( app.prefsFolder.absoluteURI ) == 0 )
				{
					//
					// DON'T BACKUP a BACKUP-FILE
					//
					this.backup			= null;
					this.backupTaskID	= -1;
					
					return;
				}
				
				var name = this.fileName;
				
				if( this.isFile() )
					name = tmp.name;
				
				if( name.lastIndexOf( ".jsx" ) != name.length-4 )
					name += ".jsx";

				var now = new Date();
				name = "BK#" + now.valueOf() + "#" + name;
				
				this.backup = new File( app.prefsFolder + "/backup/" + name );
			}
			
			var bkFolder = new Folder( app.prefsFolder + "/backup" );
			
			if( !bkFolder.exists )
				bkFolder.create();

			app.saveText( this.getText(), this.backup, this.lf, this.encoding, prefs.document.BOM.getValue( Preference.NUMBER ) );
		}

		//
		// trigger next backup
		//
		var backupDelay = prefs.document.backupDelay.getValue( Preference.NUMBER );
		
		if( !isNaN( backupDelay ) && backupDelay > 0 )
			this.backupTaskID = addScheduledTask( backupDelay * 1000, this, this.cbBackupDocument );
	}
}
