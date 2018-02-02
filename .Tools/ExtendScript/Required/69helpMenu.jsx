/**************************************************************************
*
*  @@@BUILDINFO@@@ 69helpMenu-2.jsx 3.5.0.17	16-March-2009
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

///////////////////////////////////////////////////////////////////////////////
//
// help menu
//

menus.help = new MenuElement ("menu", "$$$/ESToolkit/Menu/Help=&Help",
							  "at the end of menubar", "help");

// Add this setup to the late startup tasks.
addDelayedTask (setupHelpMenu);

function setupHelpMenu()
{
	setupHelpDocs();

	menus.help.about = new MenuElement ("command", 
						localize ("$$$/ESToolkit/Menu/Help/About=&About %1...", app.title), 
								  "at the end of help", "help/about");

	menus.help.about.onSelect = function()
	{
		if( app.modalState ) return;

		workspace.storeFocus();
		app.showAboutBox();
		workspace.restoreFocus();
	}

	OMV.setup();

    globalBroadcaster.notifyClients( 'updateMenu_Help' );
}

//-----------------------------------------------------------------------------
// 
// setupHelpDocs(...)
// 
// Purpose: Populate help menu with PDF files of the application folder (or sub folder)
// 
//-----------------------------------------------------------------------------

function setupHelpDocs()
{
    var startFolder = Folder.appPackage;
    
    if( !startFolder )
        return;
    
    if( !_win )
        startFolder = startFolder.parent;
        
    var tree = [];

    //
    // create file info tree 
    //
	const maxDepth = 2;
    iterateHelpDocs( startFolder, tree, true, maxDepth );
    
    //
    // populate help menu
    //
    populateHelpDocs( tree, 'help', true );

    //
    // find PDF files in given folder
    //
    function iterateHelpDocs( folder, info, recursive, maximalDepth, currentDepth )
    {
		if( !currentDepth )
			currentDepth = 0;
			
        if( folder instanceof Folder )
        {
            var subFolders = folder.getFiles();
            
            for( var i=0; i<subFolders.length; i++ )
            {
		        if( subFolders[i] instanceof Folder )
		        {
					if( recursive )
					{
						if( currentDepth <= maximalDepth )
							iterateHelpDocs( subFolders[i], info, true, maximalDepth, currentDepth+1 );
					}
		        }
		        else if( subFolders[i].name.lastIndexOf( '.pdf' ) == subFolders[i].name.length-4 )
		            info.push( subFolders[i] );
            }        
        }
    }

    //
    // populate help menu with PDF files
    //
    function populateHelpDocs( pdfs, parentMenuID, separator )
    {
        for( var i=0; i<pdfs.length; i++ )
        {
            var addFile     = false;
            var displayName = ( pdfs[i].displayName.length > 0 ? pdfs[i].displayName : decodeURI(pdfs[i].name) );

            if( displayName.lastIndexOf( '.pdf' ) == displayName.length-4 )
                displayName = displayName.substr( 0, displayName.length-4 );
            
			if( displayName.length > 0 )
			{
				//
				// try to match the intro to scripting guide w/ the current locale
				//
				if( displayName.toLowerCase() == "adobe intro to scripting" )
				{
					var parentName = unescape( pdfs[i].parent.name );
					var locale     = prefs.locale.getValue( Preference.STRING );
					
					switch( locale )
					{
						case "de_DE":
							if( parentName == "German" )
								addFile = true;
							break;
						case "fr_FR":
							if( parentName == "French" )
								addFile = true;
							break;
						case "ja_JP":
							if( parentName == "Japanese" )
								addFile = true;
							break;
						default:
							if( parentName == "English" )
								addFile = true;
					}
					
					if( addFile )
						displayName = localize( "$$$/ESToolkit/Docs/IntroScripting=Adobe Intro To Scripting" );
				}
				else
				{
					//
					// add any pdf unconditionally if it's not 'adobe intro to scripting'
					// pdf.  this includes the javascript tools guide
					//
					addFile = true;
				}
					
				if( addFile )
				{
					var menuID    = parentMenuID + '/sub' + pdfs[i].name;
					var location  = 'at the end of ' + parentMenuID;
					
					var item      = new MenuElement( "command", displayName, location, menuID );
					item.file     = pdfs[i];
					item.disabled = false;
					separator     = false;
					
					item.onDisplay = function()
					{
						this.enabled = !this.disabled;
					}
					
					item.onSelect = function()
					{
						if( app.modalState ) return;

						this.disable = !this.file.execute();
					}
				}
			}
        }
    }
}
