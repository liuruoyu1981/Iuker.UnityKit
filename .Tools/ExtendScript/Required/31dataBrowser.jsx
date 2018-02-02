/**************************************************************************
*
*  @@@BUILDINFO@@@ 31dataBrowser-2.jsx 3.5.0.25	05-May-2009
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
// host object for databrowser pane
//
databrowser =   {
                    pane            : null,             // the pane
                    name            : 'databrowser',    // unique name
					enabled			: false,			// disabled if no session or session is disconnected
					target			: null,				// active target (session may be null)
					session			: null,				// active session
                    title           : '$$$/ESToolkit/Panes/Variables/Title=Data Browser',
                    menu            : '$$$/ESToolkit/Menu/Window/DataBrowser/Title=&Data Browser',
                    iconDefault     : '#PDataBrowser_N',
                    iconRollover    : '#PDataBrowser_R'
                };          

//
// register for panes initialization
//
globalBroadcaster.registerClient( databrowser, 'initPanes' );

databrowser.onNotify = function( reason )
{
    if( reason == 'initPanes' )
    {
        this.init();
    }
    else if( this.pane )
    {
	    switch (reason)
	    {
		    case 'shutdown':
		        globalBroadcaster.unregisterClient( this );
				this.pane.list.onChange   = null;
				this.pane.list.onExpand   = null;
				this.pane.list.onCollapse = null;
			    this.pane.list.removeAll();
			    break;
    			
		    case 'newDebugPrefs':
			    this.showUndefined	  = prefs.databrowser.showUndefined.getValue( Preference.BOOLEAN );
			    this.showCore		  = prefs.databrowser.showCore.getValue( Preference.BOOLEAN );
			    this.showFunctions	  = prefs.databrowser.showFunctions.getValue( Preference.BOOLEAN );
			    this.showPrototype	  = prefs.databrowser.showPrototype.getValue( Preference.BOOLEAN );
			    this.maxArrayElements = prefs.databrowser.maxArrayElements.getValue( Preference.NUMBER );
			    // during startup, this is null
			    if( targetMgr.getActiveSession() && this.updatePane )
				    this.updatePane (true);
			    break;
                
            case 'endConnect':
            case 'initialized':
            {
                if( targetMgr.getActiveTarget() == arguments[1] )
				{
					this.target = arguments[1];
					this.session = this.target.getActive();
					this.updatePane (true);
				}
            }
            break;

            case 'changeActiveSession':
			{
				if( this.target != arguments[1] )
				{
					// argument[1] is a target
					this.target = arguments[1];
					this.session = this.target.getActive();
					this.updatePane (true);
				}
			}
            break;
            
            case 'endUpdateModel':
            {
				// coming in after the entire visible data tree has been updated
                if( targetMgr.getActiveSession() == arguments[1] )
                {
                    this.session	= arguments[1];
					var erase		= arguments[2];
					this.pane.infogrp.targetField.text = localize( "$$$/ESToolkit/DataBrowser/Updating=Updating..." );
/// MEF: remove the following lines to enable extended updates
//if ($.os.substr (0, 3) == "Mac")
	erase = true;
					addDelayedTask( databrowser, databrowser.updatePane, erase );
                }
            }
            break;
	    }
	}
}

databrowser.init = function()
{
    //
    // create and register pane
    //
    var res = panes.createPaneObj( this,
                                    """edit			: EditText 
                                       {
				                            properties		: { enterKeySignalsOnChange: true },
				                            preferredSize	: [40, 20],
				                            alignment		: 'fill',
				                            enabled			: false,
                                            helpTip			: '$$$/ESToolkit/Panes/Variables/htEdit=Assign a new value to the selected variable here.'
			                            },
			                            list			: TreeView 
			                            {
				                            preferredSize	: [10, 20],
				                            alignment		: ['fill', 'fill']
			                            },
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

	this.icons = {};
	this.icons.UNDEFINED	= ScriptUI.newImage ("#Undefined");
	this.icons.NULL			= ScriptUI.newImage ("#Null");
	this.icons.BOOLEAN		= ScriptUI.newImage ("#Boolean");
	this.icons.NUMBER		= ScriptUI.newImage ("#Number");
	this.icons.STRING		= ScriptUI.newImage ("#String");
	this.icons.OBJECT		= ScriptUI.newImage ("#Object");
	this.icons.FUNCTION		= ScriptUI.newImage ("#Function");
	this.icons.INVALID		= ScriptUI.newImage ("#Invalid");
	this.icons.UNDEFINED_RO	= ScriptUI.newImage ("#Undefined_RO");
	this.icons.NULL_RO		= ScriptUI.newImage ("#Null_RO");
	this.icons.BOOLEAN_RO	= ScriptUI.newImage ("#Boolean_RO");
	this.icons.NUMBER_RO	= ScriptUI.newImage ("#Number_RO");
	this.icons.STRING_RO	= ScriptUI.newImage ("#String_RO");
	this.icons.OBJECT_RO	= ScriptUI.newImage ("#Object_RO");
	this.icons.FUNCTION_RO	= ScriptUI.newImage ("#Function_RO");
	this.icons.INVALID_RO	= ScriptUI.newImage ("#Invalid_RO");

	// used properties:
	// target - the current target
	// session - the current session
	// baseScope = the scope
	this.target  = null;
	this.session = null;
	this.baseScope  = '$.global';

	globalBroadcaster.registerClient( this, 'shutdown,newDebugPrefs' );
	targetMgr.registerClient( this, 'changeActiveSession,changeActiveTarget,endConnect' );
	DebugSession.registerClient( this, 'initialized,endUpdateModel' );

    ///////////////////////////////////////////////////////////////////////////////
    //
    // object methods
    //
    
    //
    // clear list
    //
    this.erase = function()
    {
        this.pane.list.removeAll();
        this.pane.edit.text = '';
    }
	
	// Update the pane. This is a delayed task.
	// Force update if the flag is true.
	this.updatePane = function (forceUpdate)
	{
		if( !prefs.databrowser.displayData.getValue( Preference.BOOLEAN ) )
			return;

		// set the target display
		this.pane.infogrp.targetField.text = this.target ? this.target.getTitle() : "";
		// determine the enabled state and set the target display
		this.enabled = false;
		if( this.session && this.session.initialized() )
			this.enabled = this.target.getFeature( Feature.GET_VARIABLES, this.session );

		// prepare to re-fill the pane
	    this.pane.edit.enabled = false;
		var treeView = this.pane.list;
		var onChange = treeView.onChange;
		var onExpand = treeView.onExpand;
		var onCollapse = treeView.onCollapse;

		treeView.onExpand =
		treeView.onCollapse = null;

		if (!this.enabled)
			this.erase();
		else
		{
			this.baseScope = this.session.model.getBaseScope();
			var variables = this.session.getModel( this.baseScope );
		    
			this.oldSelection = null;

			try
			{
				if( treeView.selection && treeView.selection.data )
					this.oldSelection = this.makePath( treeView.selection );
			}
			catch(e)
			{}

			if( variables )
			{
				this.fillNode( treeView, variables, forceUpdate );

				if( this.oldSelection )
				{
					var selItem = this.findItem (this.oldSelection);
		            
					if( selItem )
						treeView.selection = selItem;
				}
			}
		}

		treeView.onExpand = onExpand;
		treeView.onCollapse = onCollapse;
	}
			
	/* Fill a node.
	 * This method throws the sorted list of variables
	 * on the item list, replacing icons and values as
	 * needed. If the type changed (because an item was
	 * inserted or deleted before this item, or the data
	 * type has changed between object and non-object),
	 * it is recreated.
	 */

	this.fillNode = function( node, variables, forceUpdate )
	{
		if (forceUpdate)
			node.removeAll();

		var arr = variables.sortChildren();
		for (var i = 0; i < arr.length; i++)
        {
            var icon = databrowser.icons.UNDEFINED;
            var itemType   = 'item';
            var showName   = true;
			var sessionVar = arr[i];

			// set up the icon and type
			switch( sessionVar.type )
            {
                case "null":
					icon = ( sessionVar.readonly ? databrowser.icons.NULL_RO : databrowser.icons.NULL );
                    break;
                case "undefined":
					icon = ( sessionVar.readonly ? databrowser.icons.UNDEFINED_RO : databrowser.icons.UNDEFINED );
                    break;
                case "boolean":
					icon = ( sessionVar.readonly ? databrowser.icons.BOOLEAN_RO : databrowser.icons.BOOLEAN );
                    break;
                case "number":
					icon = ( sessionVar.readonly ? databrowser.icons.NUMBER_RO : databrowser.icons.NUMBER );
                    break;
                case "string":
					icon = ( sessionVar.readonly ? databrowser.icons.STRING_RO : databrowser.icons.STRING );
                    break;
				case "error":
					icon = ( sessionVar.readonly ? databrowser.icons.INVALID_RO : databrowser.icons.INVALID );
					break;
                case "Function":
					icon = ( sessionVar.readonly ? databrowser.icons.FUNCTION_RO : databrowser.icons.FUNCTION );
                    // There is only a need to implement this as node if Core JS 
					// or Prototypes are selected
					if (this.showCore || this.showPrototype)
						itemType = 'node';
					// show name if the property name is different from the function name
                    showName = (sessionVar.name != sessionVar.value.split ('(')[0]);
                    break;
                default:
                    if( sessionVar.invalid )
						icon = ( sessionVar.readonly ? databrowser.icons.INVALID_RO : databrowser.icons.INVALID );
					else
                    {
						icon = ( sessionVar.readonly ? databrowser.icons.OBJECT_RO : databrowser.icons.OBJECT );
                        itemType = 'node';
                    }
                    break;
            }

			// set up the text
			var value = sessionVar.value;
	        var itemText = showName ? ( sessionVar.name + " = " + value ) : value;
			
	        var index = itemText.indexOf ('\n');
	        if (index >= 0)
		        itemText = itemText.substr (0, index);
	        index = itemText.indexOf ('\r');
	        if (index >= 0)
		        itemText = itemText.substr (0, index);

			var maxItemTextLength = prefs.databrowser.maxStringLength.getValue( Preference.NUMBER );

	        if (itemText.length > maxItemTextLength )
		        itemText = itemText.substr (0, maxItemTextLength) + "...";

			// Is there an item already?
			var item;
			if (i < node.items.length)
			{
				item = node.items [i];
				// check its type; if this does not match, the item needs to be re-created
				if (item.type != itemType)
				{
					node.remove (i);
					item = node.add (itemType, itemText, i);
				}
				else
					item.text = itemText;

				item.data = sessionVar;
				item.image = icon;
			}
			else
			{
				// beyond the existing items - need to append
				item       = node.add( itemType, itemText );
				item.data  = sessionVar;
				item.image = icon;
			}

			// if the item was expanded, it needs to be refilled
			if (sessionVar.expanded)
			{			
			
				this.fillNode (item, sessionVar, forceUpdate);

				if( _win )
					item.expanded = true;
				else
				{
					if( node == this.pane.list )
					{
						item.expanded = true;
						expandSubItems( item );
					}
				}
			}
			else if (item.type == 'node')
			{
				item.expanded = false;
			}
        }

		// Erase excess elements
		while (i < node.items.length)
			node.remove (node.items.length-1);
	}

	// (Mac) expand nodes starting at the root node
	function expandSubItems( item )
	{
		for( var i=0; i<item.items.length; i++ )
		{
			if( item.items[i].type == 'node' &&
			    item.items[i].data.expanded )
			{
				item.items[i].expanded = true;
				expandSubItems( item.items[i] );
			}
		}
	}
	
	databrowser.makePath = function( item )
	{
		var scopes = [];

		try
		{
			while( item && item != this.pane.list )
			{
				scopes.unshift( item.data.name );
				item = item.parent;
			}
			
			scopes.unshift( this.baseScope );
		}
		catch( exc )
		{
			scopes = [ "$.global" ];
		}

		return scopes.join( '/' );
	}

	// Find a tree view item by path.

	databrowser.findItem = function (path)
	{
		var item = this.pane.list;
        var names = path.split( '/' );
        
        if( names.length > 1 )
        {
            var node = this.pane.list;
            
            for( var i=1; i<names.length; i++ )
            {
                item = null;
                
                for( var j=0; j<node.items.length; j++ )
                {
                    if( node.items[j].data.name == names[i] )
                    {
                        node = node.items[j];
                        item = node;
                        break;
                    }
                }
            }
		}
		return item;
	}

    ///////////////////////////////////////////////////////////////////////////////
    //
    // UI handler
    //
    
	// The callback for a new value selection.
	this.pane.list.onChange = function()
	{
	    var text    = '';
	    var enabled = false;
	    var item    = this.selection;
	    
	    if( item && item.data && !item.data.invalid )
	    {
		    enabled = !item.data.readonly;
		    text    = item.data.value;
	    }
	    
		const kMaxEditLength = 20000;

        if( text.length > kMaxEditLength )
		{
	        databrowser.pane.edit.text		= text.substr( 0, kMaxEditLength ) + "...";
			databrowser.pane.edit.enabled	= false;
		}
		else
		{
			databrowser.pane.edit.text       = text;
			databrowser.pane.edit.enabled    = enabled;
		}
	}

	// The callback if the user clicked on a node to expand it,
	// or the expanded property has been set.
	this.pane.list.onExpand = function (item)
	{	
		if( item.type == 'node' && 
		    databrowser.session && databrowser.session.initialized() )
		{
			var onChange		= this.onChange;
			var onExpand		= this.onExpand;
			var onCollapse		= this.onCollapse;
			this.onChange		= null;
			this.onExpand		= null;
			this.onCollapse		= null;
			// not really expanded yet
			item.data.expanded	= true;
			item.expanded		= false;
			item.text			= localize ("$$$/ESToolkit/DataBrowser/Pending=pending...");
 			
            var path			= databrowser.makePath( item );
			databrowser.session.updateModel( path );
		this.selection = item;

			this.onChange		= onChange;
			this.onExpand		= onExpand;
			this.onCollapse		= onCollapse;
		}
	}

	// The callback if the user clicked on a node to collapse it,
	// or the expanded property has been set.
	this.pane.list.onCollapse = function (item)
	{
		if( item.data )
			item.data.erase();
	}

	// The callback if the user typed in a new value.

	this.pane.edit.onChange = function()
	{
		var text = this.text;
		var item = databrowser.pane.list.selection;

		databrowser.setVariable( item, text, false, true );
		this.text = "";
	}

	databrowser.setVariable = function( item, text, isString, retry )
	{
		if( item && typeof(text) != "undefined" )
		{
			if( !isString )	isString = false;
			if( !retry )	retry = false;

			if( isString )
				text = "'''" + escape(text) + "'''";

// TODO: cdi support to check syntax        
			var syntaxText = "__SC__= " + text;
			var result = app.checkSyntax( syntaxText, document.scriptID, document.includePath );

			if( result.error )
			{
				if( retry )
					databrowser.setVariable( item, text, true, false );
				else
				{
					app.beep();
					docMgr.setStatusLine( result.message );
				}
			}
			else
			{
				// eval the expression, but do not print the reply
				if( item && item.data && !item.data.readonly )
				{
					var scope = databrowser.makePath(item);

					if( databrowser.session && databrowser.session.initialized()    )
					{
						var job			= databrowser.session.sessionObj.setValue( scope, item.data.name, text );
						job.itemPath    = scope;
						job.updatePath	= databrowser.makePath( ( item != databrowser.pane.list ? item.parent : item ) ); 
						job.text		= text;
						job.retry		= retry;
			            
						job.onResult = function()
						{
							databrowser.session.updateModel( this.updatePath );
						}
			            
						job.onError = job.onTimeout = function()
						{
							if( this.retry )
							{
								var item = databrowser.findItem( this.itemPath );

								if( item )
									databrowser.setVariable( item, this.text, true, false );
							}
							else
								databrowser.session.updateModel( this.itemPath );
						}
			            
						job.submit();
					}
				}
			}

		}
	}

    ///////////////////////////////////////////////////////////////////////////
    //
    // flyout menu
    //
    this.pane.menu = new MenuElement( "popupmenu", "Flyout", undefined, "databrowser/flyout" );
    
	var item = new MenuElement( 'command', '$$$/ESToolkit/Panes/DataBrowser/Flyout/ShowUndefined=&Undefined Variables', 
								 "at the end of databrowser/flyout", "databrowser/flyout/undefined" );
	item.onDisplay = function()
	{
		this.checked = databrowser.showUndefined;
		this.enabled = databrowser.enabled && 
                       databrowser.target.getFeature( Feature.VARFILTER_UNDEFINED, databrowser.session );
	}
	item.onSelect = function()
	{
		databrowser.showUndefined       = !databrowser.showUndefined;
		prefs.databrowser.showUndefined = databrowser.showUndefined;
		
		if( databrowser.session )
		    databrowser.session.updateModel();
	}

	item = new MenuElement( 'command', '$$$/ESToolkit/Panes/DataBrowser/Flyout/ShowFunctions=&Functions', 
								 "at the end of databrowser/flyout", "databrowser/flyout/functions" );
	item.onDisplay = function()
	{
		this.checked = databrowser.showFunctions;
		this.enabled = databrowser.enabled && 
                       databrowser.target.getFeature( Feature.VARFILTER_FUNCTIONS, databrowser.session );
	}
	item.onSelect = function()
	{
		databrowser.showFunctions       = !databrowser.showFunctions;
		prefs.databrowser.showFunctions = databrowser.showFunctions;
		
		if( databrowser.session )
		    databrowser.session.updateModel();
	}

	item = new MenuElement( 'command', '$$$/ESToolkit/Panes/DataBrowser/Flyout/ShowCore=\&Core JavaScript Elements', 
								 "at the end of databrowser/flyout", "databrowser/flyout/core" );
	item.onDisplay = function()
	{
		this.checked = databrowser.showCore;
		this.enabled = databrowser.enabled && 
                       databrowser.target.getFeature( Feature.VARFILTER_CORE, databrowser.session );
	}
	item.onSelect = function()
	{
		databrowser.showCore        = !databrowser.showCore;
		prefs.databrowser.showCore  = databrowser.showCore;
		
		if( databrowser.session )
		    databrowser.session.updateModel();
	}

	item = new MenuElement( 'command', '$$$/ESToolkit/Panes/DataBrowser/Flyout/ShowPrototype=&Prototype Elements', 
								 "at the end of databrowser/flyout", "databrowser/flyout/proto" );
	item.onDisplay = function()
	{
		this.checked = databrowser.showPrototype;
		this.enabled = databrowser.enabled && 
                       databrowser.target.getFeature( Feature.VARFILTER_PROTOTYPE, databrowser.session );
	}
	item.onSelect = function()
	{
		databrowser.showPrototype       = !databrowser.showPrototype;
		prefs.databrowser.showPrototype = databrowser.showPrototype;
		
		if( databrowser.session )
		    databrowser.session.updateModel();
	}    

    this.onNotify ('newDebugPrefs');
}
