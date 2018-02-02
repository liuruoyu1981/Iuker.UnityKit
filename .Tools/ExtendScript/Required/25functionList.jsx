/**************************************************************************
*
*  @@@BUILDINFO@@@ 25functionList-2.jsx 3.5.0.42	13-November-2009
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

functionList =   {
                pane            : null,         // the pane
                name            : 'functionlist',  // unique name
                title           : '$$$/ESToolkit/Panes/FunctionList/Title=Functions',
                menu            : '$$$/ESToolkit/Menu/Window/FunctionList/Title=&Functions',
                iconDefault     : '#PFunctionList_N',
                iconRollover    : '#PFunctionList_R'
            };          
          
globalBroadcaster.registerClient( functionList, 'initPanes, shutdown, functionListScanBegin, functionListScanEnd, functionListChanged' );

functionList.onNotify = function( reason )
{
    if( reason == 'initPanes' )
    {    
        this.init();
    }
    else
    {
		switch( reason )
		{	
			case "shutdown":
				this.savePrefs();
				globalBroadcaster.unregisterClient( this );
			break;
			
			case "functionListScanBegin":
			{
			    this.pane.desc.text = localize( "$$$/ESToolkit/DataBrowser/Updating=Updating..." );
			    this.pane.list.removeAll();
			    this.pane.enabled = false;
			}
			break;
			
			case "functionListScanEnd":
			{
			    this.pane.desc.text = "";
			    this.pane.enabled = true;
			}
			break;
			
			case "functionListChanged":
			{
				var update = false;
				var sort = false;
				if( this.document != document )
				{
					if( document )
					{
						this.document = document;

						if( this.document.duplicate )
							this.document = this.document.master;

						update = true;
						if( this.document.functionList && this.document.functionList.isNew == true )
							sort = true;
					}
					else
					{
						this.pane.list.removeAll();
						this.document = null;
					}
				}
				else if( this.document && this.document.functionList && this.document.functionList.isNew == true )
				{
					sort = true;
					update = true;
				}

				if( this.document && this.document.functionList && update ) 
				{
					this.updateList( sort );
					this.document.functionList.isNew = false;
				}
			}
			break;
		}
    }
}

functionList.init = function()
{
    //
    // create and register pane
    //
    var res = panes.createPaneObj( this,
	                                """list: ListBox
			                           {
				                           alignment: ['fill', 'fill']
			                           },
			                           desc: StaticText
			                           {
				                           preferredSize: [100, -1],
				                           alignment: ['fill', 'bottom']
			                           }""" );
									   
	res.functionList = this;

    ///////////////////////////////////////////////////////////////////////////////
    //
    // object methods
    //
	functionList.formatParamsList = function( paramList )
	{
		var retVal = "(";
		for( var i=0; i<paramList.length; i++ )
		{
			retVal += paramList[i].name;
			if (i != paramList.length-1)
				retVal += ", ";			
		}
		retVal += ")";
		
		return retVal;
	}

	functionList.updateList = function( sort )
	{
		app.setWaitCursor( true );
		
		if( sort == true )
			this.sort();
		
		//
		// Remove all the items and empty the description
		//
		this.pane.desc.text = localize( "$$$/ESToolkit/DataBrowser/Updating=Updating..." );
		this.pane.list.removeAll();
		this.pane.desc.helpTip = null;
		
		if( this.document && this.document.functionList )
		{						
			for( var i=0; i<this.document.functionList.length; i++ )
			{
				var fn = this.document.functionList[i];
				
				//
				// Text display depends on long or short
				// name preference (flyoutmenu)
				//
				var label = "";
				if( this.shortName )
					label = fn.nameShort;
				else if( this.longName )
					label = fn.nameLong;
					
				var parameterList = this.formatParamsList( fn.parameters )
				label += " " + parameterList;
				var item = this.pane.list.add( "item", label );
				
				//
				// Add the position and the full
				// name to the item for easy access
				//
				item.fnLine		= fn.line;
				item.fnPosition = fn.position;
				item.fullName	= fn.nameLong + " " + parameterList;
			}
		}
		
		this.pane.desc.text = "";
		app.setWaitCursor( false );
	}
	
	functionList.sort = function()
	{
		if( this.document && this.document.functionList )
		{
			if( functionList.sortByAlpha )
				this.document.functionList.sort( this.sortFnByAlpha );
			else if( functionList.sortByPos )
				this.document.functionList.sort( this.sortFnByPos );	
		}
	}

	functionList.sortFnByPos = function( a, b )
	{
		if( a.line<b.line )
			return -1;
		else if( a.line>b.line )
			return 1;
		
		return 0;
	}
	
	functionList.sortFnByAlpha = function( a, b )
	{
		var string1 = "";
		var string2 = "";
		
		if( functionList.shortName==true )
		{
			string1 = a.nameShort.toLowerCase();
			string2 = b.nameShort.toLowerCase();
		}
		else if( functionList.longName == true )
		{
			string1 = a.nameLong.toLowerCase();
			string2 = b.nameLong.toLowerCase();
		}
		
		if( string1<string2 )
			return -1;
		else if( string1>string2 )
			return 1;
		
		return 0;
	}

	functionList.savePrefs = function ()
	{
		prefs.functionList.shortName	= functionList.shortName;
		prefs.functionList.longName		= functionList.longName;
		
		prefs.functionList.sortByPos	= functionList.sortByPos;
		prefs.functionList.sortByAlpha	= functionList.sortByAlpha;
	}

    ///////////////////////////////////////////////////////////////////////////////
    //
    // UI handler
    //
	functionList.pane.list.onDoubleClick = function()
	{
		var item = this.selection;
		var doc = document;
		if( item && doc && doc instanceof SourceDocument )
		{
			doc.editor.setSelection( item.fnLine, item.fnPosition, item.fnLine, item.fnPosition, true );
			doc.rootPane.active=false;
			doc.activate();
		}
		
		this.selection = null;
	}
	
	functionList.pane.list.onChange = function()
	{
		if( this.selection != null )
		{
			var item = this.selection;
			this.rootPane.desc.text = item.fullName;
			this.rootPane.desc.helpTip = item.fullName;
		}
		else
		{
			this.rootPane.desc.text = "";
			this.rootPane.desc.helpTip = null;
		}
	}
    
	///////////////////////////////////////////////////////////////////////////
    //
    // flyout menu
    //
    this.pane.menu = new MenuElement( "popupmenu", "Flyout", undefined, "functionlist/flyout" );
    
    var item;
    item = new MenuElement( 'command', '$$$/ESToolkit/Panes/FunctionList/Flyout/ShortName=&Short Name',
								"at the end of functionlist/flyout", "functionlist/flyout/shortname" );
								
	item.onDisplay = function()
	{
		this.checked = functionList.shortName;
	}
	
	item.onSelect = function()
	{
		functionList.shortName			= !functionList.shortName;
		functionList.longName			= !functionList.longName;
		
		//
		// Only sort if the sort-order is by alpha
		//
		functionList.updateList( functionList.sortByAlpha );
	}
	
    item = new MenuElement( 'command', '$$$/ESToolkit/Panes/FunctionList/Flyout/LongName=&Long Name',
								"at the end of functionlist/flyout", "functionlist/flyout/longname" );
								
	item.onDisplay = function()
	{
		this.checked = functionList.longName;
	}								

	item.onSelect = function()
	{
		functionList.shortName			= !functionList.shortName;
		functionList.longName			= !functionList.longName;
		
		//
		// Only sort if the sort-order is by alpha
		//
		functionList.updateList( functionList.sortByAlpha );
	}
								
    item = new MenuElement( 'command', '$$$/ESToolkit/Panes/FunctionList/Flyout/SortByAlpha=Sort &Alphabetically', 
								 "---at the end of functionlist/flyout", "functionlist/flyout/alpha" );
    
	item.onDisplay = function()
	{
		this.checked = functionList.sortByAlpha;
	}
	
	item.onSelect = function()
	{
		functionList.sortByPos			= !functionList.sortByPos;
		functionList.sortByAlpha		= !functionList.sortByPos;
		
		//
		// Update the list. 'true' to sort it first
		//
		functionList.updateList( true );
	}
    
	item = new MenuElement( 'command', '$$$/ESToolkit/Panes/FunctionList/Flyout/SortByPos=Sort by &Position', 
								 "at the end of functionlist/flyout", "functionlist/flyout/pos" );
								 
	item.onDisplay = function()
	{
		this.checked = functionList.sortByPos;
	}
	
	item.onSelect = function()
	{
		functionList.sortByPos			= !functionList.sortByPos;
		functionList.sortByAlpha		= !functionList.sortByPos;
		
		//
		// Update the list. 'true' sort it first
		//
		functionList.updateList( true );
	}
	
    ///////////////////////////////////////////////////////////////////////////////
    //
    // read preferences
    //

	functionList.sortByAlpha = prefs.functionList.sortByAlpha.getValue( Preference.BOOLEAN );
	functionList.sortByPos	 = prefs.functionList.sortByPos.getValue( Preference.BOOLEAN );
	
	functionList.shortName	 = prefs.functionList.shortName.getValue( Preference.BOOLEAN );
	functionList.longName	 = prefs.functionList.longName.getValue( Preference.BOOLEAN );
}
