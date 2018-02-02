/**************************************************************************
*
*  @@@BUILDINFO@@@ 21Console-2.jsx 3.0.0.14  27-February-2008
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

console =   {
                pane            : null,         // the pane
                name            : 'console',    // unique name
                title           : '$$$/ESToolkit/Panes/Console/Title=JavaScript Console',
                menu            : '$$$/ESToolkit/Menu/Window/Console/Title=&JavaScript Console',
                iconDefault     : '#PConsole_N',
                iconRollover    : '#PConsole_R',
                session         : null,
                debug           : false         // set to true by eval'ing the string "#debug", switch back with "#target"
            };          
          
globalBroadcaster.registerClient( console, 'initPanes' );

console.onNotify = function( reason )
{
    if( reason == 'initPanes' )
        this.init();
    else
    {
        var target = targetMgr.getActiveTarget();
        
        switch( reason )
        {
            case 'endConnect':
            case 'initialized':
            {
                if( target != arguments[1] )
                    break;
            }
            // fall throw   
            case 'changeActiveTarget':
            case 'changeActiveSession':
			case 'targetDied':
	        {
	            if( target && target.getFeature( Feature.EVAL ) && target.getConnected() )
	            {
	                var session = target.getActive();
	                
	                if( session )
	                    this.session = session;
	            }
	            
	            var consoleTarget = this.session.target;
				
				if( !consoleTarget )
				{
					consoleTarget = targetMgr.defaultTarget;
					
					if( consoleTarget )
						this.session = consoleTarget.getActive();
				}
	            
	            if( consoleTarget )
	            {
	                var targetName = consoleTarget.getTitle();
	                this.pane.infogrp.targetField.text = ( targetName ? targetName : consoleTarget.address.target );
	            }
	            else
	                this.pane.infogrp.targetField.text = '';
            }
            break;
        }
    }
}

console.init = function()
{
    //
    // create and register pane
    //
    var res = panes.createPaneObj( this,
                                    """output      : ConsoleEdit
                                       {
                                           properties      : { multiline: true },
                                           preferredSize   : [10, 20],
                                           alignment       : ['fill', 'fill' ]
                                       },
                                       infogrp     : Group
                                       {
                                           orientation     : 'row',
                                           margins         : 4,
                                           spacing         : 0,
                                           alignment	    : ['fill','bottom'],
                                           targetField     : StaticText
                                           {
                                               alignment	    : ['fill','bottom'],
                                               characters      : 40
                                           },
                                           sizeBoxSpacer   : Group
                                           {
                                               alignment	    : ['right','bottom'],
                                               preferredSize   : [14,1]
                                           }
                                       }""" );
                                   
    if( res )                                   
    {
        //
        // register for broadcast messages
        //
        targetMgr.registerClient( this, 'endConnect,changeActiveTarget,changeActiveSession,targetDied' );
        DebugSession.registerClient( this, 'initialized' );
        
        //
        // set initial session
        //
        var target = targetMgr.defaultTarget;
        
        if( target )
            this.session = target.getActive();

		//
		// get/set output font size
		//
		this.pane.output.getFontSize = function()
		{
			var size = -1;

	        var fs = this.graphics.font.toString();
			var p = fs.split(":");

			if( p.length > 1 )
			{
				var fsize = parseInt( p[p.length-1], 10 );

				if( !isNaN( fsize ) )
					size = fsize;
			}

			return size;
		}

		this.pane.output.setFontSize = function( size )
		{
			var ret = false;

			if( size >= 8 && size <= 20 )
			{
				var fs = this.graphics.font.toString();
				var p = fs.split(":");

				if( p.length > 1 )
				{
					fs = "";

					for( var i=0; i<p.length-1; i++ )
						fs += p[i] + ":";

					fs += size;

					if( !_win )
						workspace.setKeyboardFocus( this );

					this.graphics.font = fs;

					ret = true;
				}
			}

			return ret;
		}

		//
		// set initial font size of output
		//
		if( prefs.console.fontSize.hasValue( Preference.NUMBER ) )
			this.pane.output.setFontSize( prefs.console.fontSize.getValue( Preference.NUMBER ) );

        //
        // flyout menu
        //
        this.pane.menu = new MenuElement( "popupmenu", "Flyout", undefined, "console/flyout" );

	    var item = new MenuElement( 'command', '$$$/ESToolkit/Panes/Console/Flyout/Clear=Clear', "at the end of console/flyout", "console/flyout/clear" )
	    item.output = this.pane.output;
	    item.onSelect = function()
	    {
	        this.output.text = '';
	    }

	    var item = new MenuElement( 'command', localize( '$$$/ESToolkit/PreferencesDlg/textFont=&Font:' ) + " +", "at the end of console/flyout", "console/flyout/inc" )
	    item.output = this.pane.output;
	    item.onSelect = function()
	    {
			var size = this.output.getFontSize();

			if( size > 0 )
			{
				if( this.output.setFontSize( ++size ) )
					prefs.console.fontSize = size;
			}
	    }

	    var item = new MenuElement( 'command', localize( '$$$/ESToolkit/PreferencesDlg/textFont=&Font:' ) + " -", "at the end of console/flyout", "console/flyout/dec" )
	    item.output = this.pane.output;
	    item.onSelect = function()
	    {
			var size = this.output.getFontSize();

			if( size > 0 )
			{
				if( this.output.setFontSize( --size ) )
					prefs.console.fontSize = size;
			}
	    }

        //
        // UI handler functions
        //
	    this.pane.output.onEnterKey = function()
	    {
		    var text = this.textselection;
		    if (!text.length)
			    return false;
                
		    if (text[text.length-1] != "\n")
			    text += "\n";
    			
		    if (!this.isCaretInLastLine())
			    this.appendText (text);
		    else
			    this.appendText ("\n");
    		
		    console.eval( text );

		    this.active = true;
		    return true;
	    }

        //
        // console functions
        //
	    this.eval = function (script)
	    {
		    if (script == "#debug\n")
		    {
			    this.debug = true;
			    print ("# Switching to debug engine #");
			    return;
		    }
		    else if (script == "#target\n")
		    {
			    this.debug = false;
			    print ("# Switching to target engine #");
			    return;
		    }
		    var res;
		    if (this.debug)
		    {
			    var oldLevel = $.level;
			    $.level = 0;
			    try
			    {
				    res = eval (script);
				    this.print (localize ("$$$/ESToolkit/Panes/Console/Result=Result:") 
							    + " " + res);
			    }
			    catch (e)
			    {
				    this.print (localize ("$$$/ESToolkit/Panes/Console/Error=Error:") 
							    + " " + e.message);
			    }
			    $.level = oldLevel;
		    }
		    else
		    {
		        if( this.session )
		            this.session.eval( script );
		    }
    			
		    return res;
	    }

	    this.write = function()
	    {
		    var s = "";
		    for (var i = 0; i < arguments.length; i++)
			    s += arguments [i];
		    this.pane.output.appendText (s);
	    }
    	
	    this.print = function()
	    {
		    var s = "";
		    for (var i = 0; i < arguments.length; i++)
			    s += arguments [i];
		    this.pane.output.appendText (s + "\n");
	    }

		this.clear = function ()
		{
			workspace.resetFocus();
			this.pane.minimized = false;
			this.pane.show();

			this.pane.output.text = '';

			this.pane.output.active = false;
			this.pane.output.active = true;

			if( !_win )
				workspace.setKeyboardFocus( this.pane.output );
		}
	}
}
