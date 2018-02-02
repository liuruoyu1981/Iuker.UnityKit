/**************************************************************************
*
*  @@@BUILDINFO@@@ 07workspace-2.jsx 3.5.0.47	09-December-2009
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

workspace.setupMenu = function()
{
    if( workspace.menu )
        workspace.menu.remove();
    
    workspace.menu = new MenuElement( "popupmenu", "$$$/ESToolkit/Menu/Window/Workspaces=&Workspaces", undefined, "workspaces" );
												              
	workspace.menu.cmdAdd = new MenuElement ("command", "$$$/ESToolkit/Menu/Window/Workspaces/Add=&Create new Workspace...",
	                                                   "at the end of workspaces", "workspaces/add" );
	workspace.menu.cmdRem = new MenuElement ("command", "$$$/ESToolkit/Menu/Window/Workspaces/Rem=&Remove current Workspace",
	                                                   "at the end of workspaces", "workspaces/rem" );
	                                                   
	workspace.menu.cmdAdd.onSelect = function()
	{
	    var name = workspace.createNewDialog();
	    
        if( name )
        {
            if( workspace.addWorkspace( name ) )
                addDelayedTask( workspace.setupMenu );
        }
	}
    
	workspace.menu.cmdRem.onDisplay = function()
	{
	    this.enabled = workspace.isUserName( workspace.workspace );
	}

	workspace.menu.cmdRem.onSelect = function()
	{
	    if( workspace.removeWorkspace( workspace.workspace ) )
	        addDelayedTask( workspace.setupMenu );
	}
    
    for( var i=0; i<workspace.defaultWorkspaces.length; i++ )
    {
        var wsMenuPropName = "cmdWS"+i;
        var posPrefix      = ( i == 0 ? '---' : '' );
        var label = "$$$/ESToolkit/Workspace/Defaults/" + workspace.defaultWorkspaces[i] + "=" + workspace.defaultWorkspaces[i];
		workspace.menu[wsMenuPropName] = new MenuElement( "command", label, posPrefix+"at the end of workspaces", "workspaces/ws"+i );
		workspace.menu[wsMenuPropName].wsName = workspace.defaultWorkspaces[i];
		
		workspace.menu[wsMenuPropName].onDisplay = function()
		{
			this.checked = workspace.workspace == this.wsName;
		}
		workspace.menu[wsMenuPropName].onSelect = function()
		{
		    workspace.workspace = this.wsName;
			workspace.checkAndEnableRemoveMenu();
		}
    }
	
    for( var i=0; i<workspace.userWorkspaces.length; i++ )
    {
        var wsMenuPropName = 'cmdWS' + ( i + workspace.defaultWorkspaces.length );
        var posPrefix      = ( i == 0 ? '---' : '' );
		workspace.menu[wsMenuPropName] = new MenuElement ("command", workspace.userWorkspaces[i], 
		                                                                posPrefix+"at the end of workspaces", 
		                                                                "workspaces/ws"+(i+workspace.defaultWorkspaces.length));
		
		workspace.menu[wsMenuPropName].onDisplay = function()
		{
			this.checked = workspace.workspace == this.text;
		}
		workspace.menu[wsMenuPropName].onSelect = function()
		{
		    workspace.workspace = this.text;
			workspace.checkAndEnableRemoveMenu();
			globalBroadcaster.notifyClients( 'workspaceChanged' );
		}
    }
	
	addDelayedTask(workspace.checkAndEnableRemoveMenu);
}

workspace.checkAndEnableRemoveMenu = function()
{
	workspace.menu.cmdRem.enabled = workspace.isUserName( workspace.workspace );
}

workspace.createNewDialog = function()
{
    var name    = null;
    var repeat  = false;

    do
    {
        repeat  = false;
        
        name = smartPrompt( localize( "$$$/ESToolkit/Workpsace/NewWorkspaceDis=Enter name for new Workspace" ),
                            localize( "$$$/ESToolkit/Workspaces/NewWorkspace=New Workspace" ) );
                            
        if( name )
        {
            var existsDef  = workspace.isDefaultName( name );
            var existsUser = workspace.isUserName( name );
            
            if( existsDef )
            {
                if( !dsaQueryBox( 'ws1', '$$$/ESToolkit/Workspace/ExistsDefault=A default Workspace already exists with the name "%1".^nDo you want to chose a different name?', name ) )
                    return;

                repeat = true
            }
            
            if( existsUser )
            {
                if( !dsaQueryBox( 'ws2', '$$$/ESToolkit/Workspace/ExistsUser=A Workspace already exists with the name "%1".^nDo you want to replace the existing Workspace?', name ) )
                    return;

                repeat = false
            }
        }
    
    } while( repeat );
    
    return name;
}

workspace.isUserName = function( name )
{
    for( var i=0; i<workspace.userWorkspaces.length; i++ )
    {
        if( name == workspace.userWorkspaces[i] )
            return true;
    }
    
    return false;   
}

workspace.isDefaultName = function( name )
{
    for( var i=0; i<workspace.defaultWorkspaces.length; i++ )
    {
        if( name == workspace.defaultWorkspaces[i] )
            return true;
    }
    
    return false;   
}

workspace.onNotify = function( reason )
{
	if( reason == 'newPrefs' )
	{
		if( prefs.Workspace.useMainFrame.getValue( Preference.BOOLEAN ) != workspace.appFrame )
		{
			workspace.appFrame = !workspace.appFrame;
			globalBroadcaster.notifyClients( 'appFrameChanged' );
		}
	}
}

workspace.initialize = function()
{
	//
	// If the ESTK was launched with preferences then check if 
	// user workspaces needs to be imported
	//
	if( startupPrefsLoaded && !prefs.wsimported.getValue( Preference.BOOLEAN ) )
	{
        var folder = new Folder( app.prefsFolder.parent.absoluteURI + "/3.5" );

		if( folder.exists )
		{
			var wsFiles = folder.getFiles( "*.ws" );

			for( var i=0; i<wsFiles.length; i++ )
			{
				var src  = wsFiles[i];
				var dest = new File( app.prefsFolder.absoluteURI + "/" + src.name );

				if( src.open("r") && dest.open("w") )
				{
					dest.write( src.read() );
				}

				src.close();
				dest.close();
			}
		}

		prefs.wsimported = true;
		workspace.userWorkspaces = true;
		addDelayedTask( workspace.setupMenu );
	}
}

globalBroadcaster.registerClient( workspace, 'newPrefs' );

addDelayedTask( workspace.setupMenu );

