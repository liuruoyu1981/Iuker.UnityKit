/**************************************************************************
*
*  @@@BUILDINFO@@@ 98workspacePrefs-2.jsx 3.0.0.23  27-February-2008
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

// Each Prefs object needs the following methods:
// prefsObj.create (parentPane) - create the pane and return the Pane object.
// pane.preProcess()			- preprocess the pane just before being shown.
// pane.layoutDone()			- informs the pane that the initial layout is done (optional)
// pane.postProcess()			- do any work just before the pane is being hidden.
// pane.store()					- store the preferences. Return true if the Next Time dialog is needed.
// pane.toDefault()				- set back to default values

var workspacePrefs = { title : "$$$/ESToolkit/PreferencesDlg/workspaceTitle=Workspaces", sortOrder : 80 };

globalBroadcaster.registerClient( workspacePrefs, 'initPrefPanes' );

workspacePrefs.onNotify = function( reason )
{
    switch( reason )
    {
        case 'initPrefPanes':
        {
            if( arguments[1] instanceof Array )
                arguments[1].push( this );
        }
        break;
    }
}

workspacePrefs.create = function (parentPane)
{
    this.pane = parentPane.add (
                "group {																	                                    \
	                orientation:'column', alignChildren:['fill','fill'], visible:false, alignment: ['fill','fill'],				\
	                list        : ListBox                                                                                       \
	                {                                                                                                           \
	                },                                                                                                          \
	                buttons     : Group                                                                                         \
	                {                                                                                                           \
		                orientation : 'row',                                                                                    \
		                alignment   : ['left','bottom']\
		                btAdd       : Button                                                                                    \
		                {                                                                                                       \
			                text        : '$$$/ESToolkit/PreferencesDlg/addWorkspace=Add...'							      \
		                },                                                                                                      \
		                btRemove    : Button                                                                                    \
		                {                                                                                                       \
			                text        : '$$$/ESToolkit/PreferencesDlg/remWorkspace=&Remove'									\
		                }                                                                                                       \
	                }                                                                                                           \
                }");
                
	this.pane.prefsObj = this;
	
	this.layoutDone = function()
	{}
	
	this.preProcess = function()
	{
		// Needs always to be loaded - favorites may have changed
		this.load();
	}
	this.postProcess = function()
	{}

	this.toDefault = function()
	{
	    if( queryBox( '$$$/ESToolkit/PreferenceDlg/wantRemWorkspaces=This will remove all user defined Workspaces!^nDo you really want them?' ) )
	    {
		    for( var i=0; i<this.pane.list.items.length; i++ )
		    {
			    if( workspace.removeWorkspace( this.pane.list.items[i].text ) )
			    {
				    this.pane.list.remove(i);
				    i--;
			    }
		    }
		    
		    workspace.setupMenu();
		}
	}

	return this.pane;
}

/////////////////////////////////////////////////////////////////////////
// initialize the favorites options pane

workspacePrefs.load = function()
{
	with (this.pane) 
	{
	    list.onChange = function()
	    {
	        this.parent.buttons.btRemove.enabled = ( this.selection && workspace.isUserName( this.selection.text ) );
	    }
	    
	    buttons.btAdd.onClick = function()
	    {
			//debugger;
	        var newName = workspace.createNewDialog();
	        
	        if( newName )
	        {
                var existsDef  = workspace.isDefaultName( newName );
				var existsUser = workspace.isUserName( newName );
	        
	            if( workspace.addWorkspace( newName ) )
	            {
	                workspace.setupMenu();
					if(!existsDef && !existsUser)
					{
						list.add( 'item', newName );
					}
	            }
	        }
	    }
	    
	    buttons.btRemove.onClick = function()
	    {
	        if( list.selection && workspace.isUserName( list.selection.text ) )
	        {
	            if( workspace.removeWorkspace( list.selection.text ) )
	            {
	                workspace.setupMenu();
	                list.remove( list.selection );
	            }
	        }
	    }
	    
	    list.removeAll();
	    
        for( var i=0; i<workspace.defaultWorkspaces.length; i++ )
		{
			var label = localize( "$$$/ESToolkit/Workspace/Defaults/" + workspace.defaultWorkspaces[i] + "=" + workspace.defaultWorkspaces[i] );
            list.add( 'item', label );
		}

        for( var i=0; i<workspace.userWorkspaces.length; i++ )
            list.add( 'item', workspace.userWorkspaces[i] );
            
        list.onChange();
	}
}

/////////////////////////////////////////////////////////////////////////
// store preferences from the selected favorites options
workspacePrefs.store = function()
{
	return false;
}

