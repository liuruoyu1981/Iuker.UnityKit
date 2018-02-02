/**************************************************************************
*
*  @@@BUILDINFO@@@ 23callstack-2.jsx 3.0.0.14  27-February-2008
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

callstack =   {
                pane            : null,         // the pane
                name            : 'callstack',  // unique name
                title           : '$$$/ESToolkit/Panes/Stack/Title=Call Stack',
                menu            : '$$$/ESToolkit/Menu/Window/Stack/Title=&Call Stack',
                iconDefault     : '#PCallStack_N',
                iconRollover    : '#PCallStack_R'
            };          
          
globalBroadcaster.registerClient( callstack, 'initPanes' );

callstack.onNotify = function( reason )
{
    if( reason == 'initPanes' )
    {
        this.init();
    }
    else if( callstack.pane )
    {
	    var target = targetMgr.getActiveTarget();
	    
	    switch( reason )
	    {
	        case 'shutdown':
	            this.pane.output.removeAll();
	            globalBroadcaster.unregisterClient( this );
	            break;
	            
	        case 'endConnect':
	        case 'initialized':
	        {
	            if( target != arguments[1] )
	                break;
	        }
	        // fall throw   
	        case 'changeActiveTarget':
	        case 'changeActiveSession':
	        {
	            var enable = false;
	            
	            if( target && target.getConnected() )
	            {
	                var session = target.getActive();
	                
	                enable = ( session && session.initialized() && target.getFeature( Feature.CALLSTACK, session ) );
	            }
	            
	            this.enabled = enable;
	            
	            if( this.enabled )
	            {
	                var targetName = target.getTitle();
	                this.pane.infogrp.targetField.text = ( targetName ? targetName : target.address.target );
	            }
	            else
	            {
	                this.pane.infogrp.targetField.text = '';
	            }
            }
            break;
	    }
    }
}

callstack.init = function()
{
    //
    // create and register pane
    //
    var res = panes.createPaneObj( this,
	                                """output: ListBox
			                           {
				                           preferredSize: [10, 20],
				                           alignment: ['fill', 'fill']
			                           }
                                       infogrp : Group
                                       {
                                           orientation : 'row',
                                           margins      : 0,
                                           spacing      : 0,
                                           alignment	: ['fill','bottom'],
                                           targetField: StaticText
                                           {
                                               alignment	: ['fill','bottom'],
                                               characters : 40
                                           },
                                           sizeBoxSpacer    : Group
                                           {
                                               alignment	: ['right','bottom'],
                                               preferredSize        : [14,1]
                                           }
                                       }""" );

    //
    // register for additional broadcast messages
    //	
	globalBroadcaster.registerClient( this, 'shutdown' );
    targetMgr.registerClient( this, 'endConnect,changeActiveTarget,changeActiveSession' );
    DebugSession.registerClient( this, 'initialized' );

    ///////////////////////////////////////////////////////////////////////////////
    //
    // object methods
    //

	//-----------------------------------------------------------------------------
	// 
	// callstack.erase(...)
	// 
	// Purpose: Erase call stack list.
	// 
	//-----------------------------------------------------------------------------
	
	this.erase = function()
	{
		var text = localize ("$$$/ESToolkit/Panes/Stack/NoStack=[no stack]");
		this.pane.output.removeAll();
		
		if( text.length <= 0 ) text = ' ';	// ScriptUI might run into breakpoint with 0-length strings
		this.pane.output.add ("item", text);
	}
	
	//-----------------------------------------------------------------------------
	// 
	// callstack.switchToBottom(...)
	// 
	// Purpose: Select first stack frame of list.
	// 
	//-----------------------------------------------------------------------------
	
	this.switchToBottom = function()
	{
	    var session = targetMgr.getActiveSession();
	    
	    if( session )
	        session.switchFrame(0);
	}
	
	//-----------------------------------------------------------------------------
	// 
	// callstack.getFrames(...)
	// 
	// Purpose: Get number of stack frames
	// 
	//-----------------------------------------------------------------------------
	
	this.getFrames = function()
	{
		return this.pane.output.length;
	}
	
	//-----------------------------------------------------------------------------
	// 
	// callstack.updatePane(...)
	// 
	// Purpose: Update list.
	// 
	//-----------------------------------------------------------------------------
	
	this.updatePane = function( frames )
	{
		var enabled = this.pane.enabled;
		this.pane.enabled = true;
		var oldMax = this.pane.output.items.length - 1;
		var newSize = frames.length;
		for (var frame = 0; frame < newSize; frame++)
		{
			var curFrame = frames [frame];
			if (curFrame == "[toplevel]")
				curFrame = localize ("$$$/ESToolkit/Panes/Stack/Toplevel=[Top Level]");
			$.bp (curFrame.length == 0);
			if (oldMax < frame)
			{
				if( curFrame.length <= 0 ) curFrame = ' ';	// ScriptUI might run into breakpoint with 0-length strings
				this.pane.output.add ("item", curFrame);
			}
			else
				this.pane.output.items [frame].text = curFrame;
		}
		// delete excess frames
		while (this.pane.output.items.length > newSize)
			this.pane.output.remove (this.pane.output.items.length - 1);
		// Select the last frame
		this.pane.output.selection = this.pane.output.items.length - 1;
		this.pane.enabled = enabled;
	}

    ///////////////////////////////////////////////////////////////////////////////
    //
    // UI handler
    //
    
    //-----------------------------------------------------------------------------
    // 
    // callstack.output.onChange(...)
    // 
    // Purpose: The callback for the output list box is activated when the user
	//          selects a different stack frame. In this case, the debugger displays
	//          the selected stack frame if it is halted. If not, nothing happens.
    // 
    //-----------------------------------------------------------------------------

	this.pane.output.onChange = function()
	{
	    var session = targetMgr.getActiveSession();
	    
	    if( this.selection && session )
	        session.switchFrame( this.selection.index );
	}
}
