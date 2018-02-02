/**************************************************************************
*
*  @@@BUILDINFO@@@ 87findResult-2.jsx 3.0.0.24   05-May-2008
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
// host object for findResult pane
//
findResult =   {
                    pane            : null,             // the pane
                    name            : 'findresults',    // unique name
                    title           : '$$$/ESToolkit/Panes/FindResult/Title=Find results',
                    menu            : '$$$/ESToolkit/Menu/Window/FindResult/Title=&Find Results',
                    iconDefault     : '#PFindResults_N',
                    iconRollover    : '#PFindResults_R',
					awaitedScriptID	: null
                };          

//
// register for panes initialization
//
globalBroadcaster.registerClient( findResult, 'initPanes' );

//-----------------------------------------------------------------------------
// 
// findResult.onNotify(...)
// 
// Purpose: Process broadcast messages
// 
//-----------------------------------------------------------------------------

findResult.onNotify = function( reason )
{
    if( reason == 'initPanes' )
    {
        this.init();
    }
}

findResult.init = function()
{
    //
    // create and register pane
    //
    var res = panes.createPaneObj( this,
                                    """output          : ListBox
			                           {
				                           alignment           : ['fill', 'fill']
                                       },
                                       statusGroup          : Group
                                       {
                                           alignment       : ['fill', 'bottom'],
                                           orientation     : 'row',
                                           margins         : 0,
                                           spacing         : 0,
                                           statustext      : StaticText
                                           {
                                               properties  : { truncate : 'middle' },
                                               alignment   : ['fill','center'],
                                               characters  : 40
                                           },
                                           progress        : Progressbar
                                           {
					                           visible			: false,
                                               alignment       : ['right','center']
                                           },
                                           cancelBtn       : Button
                                           {
					                           text			: '$$$/CT/ExtendScript/UI/Cancel=&Cancel',
					                           enabled			: false,
					                           properties		: { name:'cancel' },
                                               alignment       : ['right','center']
                                           },
                                           sizeBoxSpacer    : Group
                                           {
                                               preferredSize   : [20,1],
                                               alignment       : ['right','center']
                                           }
                                       }""" );
                                   
	res.minimumSize = [220, 100];

    ///////////////////////////////////////////////////////////////////////////////
    //
    // short cuts
    //
    this.pane.statustext = this.pane.statusGroup.statustext;
    this.pane.progress   = this.pane.statusGroup.progress;
    this.pane.cancelBtn  = this.pane.statusGroup.cancelBtn;
                                       

    ///////////////////////////////////////////////////////////////////////////////
    //
    // object member
    //

    this.initialized	= false;	// true if initialized
	this.inSearch		= false;	// flags an active search
	this.searchCount	= 0;		// the number of entries
	this.cancelSearch	= false;	// true if user clicked Cancel
	this.findText		= "";		// the text to find
	this.filePrefix	    = "";		// the file prefix to be ignored when creating output lines
    
    ///////////////////////////////////////////////////////////////////////////////
    //
    // object methods
    //

    this.startSearch = function( what, count, clear, filePrefix )
    {
        if( !this.inSearch )
        {
			this.filePrefix	  = filePrefix;
			this.cancelSearch = false;
            if( clear )
                this.pane.output.removeAll();
            else
                this.pane.output.add( 'item', '------------------------------' );
            var text = "$$$/ESToolkit/Panes/FindResult/FindText=Find \"%1\"";
			this.findText			= what;
            this.pane.statustext.text    = localize( text, what);
            this.pane.progress.maxvalue  = count;
            this.pane.progress.value     = 0;
            this.pane.progress.show();
            
            this.inSearch			= true;
            this.searchCount		= 0;
			this.fileCount			= 0;
			this.lastFile			= '';
			this.pane.cancelBtn.enabled	= true;

            this.pane.show();
            this.pane.active = true;
        }
    }
    
	// add a result, and return false if the user closed the ESTK or aborted the search.
    this.addResult = function( lineObj, incProgress )
    {
		var aborted = this.cancelSearch;
        if( this.inSearch && !aborted )
        {
			aborted = !app.pumpEventLoop (true);
			var text = lineObj.title + '(' + (lineObj.sel[0] + 1) + "): " + lineObj.text.replace(/\t/g, " ");
            var item = this.pane.output.add( 'item', text );
            item.info = lineObj;
			this.pane.output.selection = item;
            this.searchCount++;
			
			if( lineObj.scriptID != this.lastFile )
				this.fileCount++;
			this.lastFile = lineObj.scriptID;
                
            if( incProgress )
                this.pane.progress.value++;
        }
		return !aborted;
    }
    
	// end a search, and return true if anything was found.
    this.endSearch = function()
    {
        if( this.inSearch )
        {
			var text = "$$$/ESToolkit/Panes/FindResult/FindResult=%1, %2 matches in %3 files.";
            this.pane.progress.hide();
            this.pane.statustext.text    = localize (text, this.findText, 
                                                        this.searchCount,
                                                        this.fileCount);

			this.cancelSearch					= false;
			this.pane.cancelBtn.enabled		= false;
            this.inSearch                       = false;
			this.pane.output.selection		= null;
            this.pane.progress.hide();
		}
		return (this.searchCount > 0);
    }
    
    this.incProgress = function()
    {
        this.pane.progress.value++;
    }
    
    this.docAvailable = function()
    {
		// this is a wait() abort function, so it must return false as soon as the doc has arrived...
        return ( null == docMgr.find( findResult.awaitedScriptID ) );
    }

    ///////////////////////////////////////////////////////////////////////////////
    //
    // UI handler
    //
  
    this.pane.cancelBtn.onClick = function ()
    {
	    findResult.cancelSearch = true;
    }
    
    this.pane.output.onDoubleClick = function()
    {
        if( this.selection )
        {
	        var doc = docMgr.find( this.selection.info.scriptID );
	        
	        if( !doc )
	        {
	            if( this.selection.info.target )
	            {
                    findResult.awaitedScriptID    = this.selection.info.scriptID;

                    var info = new ScriptInfo( this.selection.info.scriptID, this.selection.info.title, true, false, '' );
	                scripts.loadScript( info, this.selection.info.target, this.selection.info.engine );
                
                    if( wait( findResult.docAvailable ) )
                    {
                        doc = docMgr.find( findResult.awaitedScriptID );
                        findResult.awaitedScriptID    = null;
                    }
	            }
	            else
	            {
	                var file = new File( this.selection.info.scriptID )
    	            
	                if( file.exists )
	                    doc = docMgr.load( file );
	            }
	        }
	         
	        if( doc )
	        {     
				doc.setSelection( this.selection.info.sel[0], this.selection.info.sel[1],
								  this.selection.info.sel[2], this.selection.info.sel[3] );
				docMgr.activateDocument( doc );
            }
	    }
    }
    
    this.pane.onShow = function()
    {
        if( !this.initialized )
            this.layout.layout();
    }

    ///////////////////////////////////////////////////////////////////////////////
    //
    // flyout menu
    //
    this.pane.menu = new MenuElement( "popupmenu", "Flyout", undefined, "findresults/flyout" );
    
	var item = new MenuElement( 'command', '$$$/ESToolkit/Panes/FindResult/Flyout/Clear=Clear', "at the end of findresults/flyout", "findresults/flyout/clear" )
	item.output = this.pane.output;
	item.onSelect = function()
	{
	    this.output.removeAll();
	}
}
