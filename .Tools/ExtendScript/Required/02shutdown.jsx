/**************************************************************************
*
*  @@@BUILDINFO@@@ 02shutdown-2.jsx 2.5.0.2  08-August-2007
*  ADOBE SYSTEMS INCORPORATED
*  Copyright 2010 Adobe Systems Incorporated
*  All Rights Reserved.
* 
* NOTICE:  Adobe permits you to use, modify, and distribute this file in accordance
* with the terms of the Adobe license agreement accompanying it.  If you have 
* received this file from a source other than Adobe, then your use, modification, or 
* distribution of it requires the prior written permission of Adobe.
**************************************************************************/

app.onShutdown = function( shift )
{
	var ok = true;
	
	try
	{
		ok = targetMgr.queryExit( true );

		if( ok && docMgr.saveAll( true ) )
		{
			appInShutDown = true;
		    
			if( !shift )
			{
				docMgr.writePrefs();
			}
		        
			globalBroadcaster.notifyClients( 'shutdown', shift );
			
			if( !wsHideOnNextStartup )
				prefs.startup.hideWS = !workspaceVisible;
			
			if( !workspaceVisible )
				workspace.togglePalettes();
		}
		else
    		ok = false;
		
		//
		// remove backup files
		//
		if( ok )
		{
			var bkFolder = new Folder( app.prefsFolder + "/backup" );
			
			if( bkFolder && bkFolder.exists )
			{
				var bkFiles = bkFolder.getFiles( "*.jsx" );
				
				for( var bks=0; bks<bkFiles.length; bks++ )
					bkFiles[bks].remove();
			}
		}
	}
	catch( exc )
	{
		ok = true;
	}

	return ok;
}

