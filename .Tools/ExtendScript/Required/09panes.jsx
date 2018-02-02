/**************************************************************************
*
*  @@@BUILDINFO@@@ 09panes-2.jsx 3.0.0.14  27-February-2008
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

function Panes()
{
    this.panes = [];
}

Panes.prototype.init = function()
{
    globalBroadcaster.notifyClients( "initPanes" );
}

Panes.prototype.createPane = function( name, title, menuLabel, iconDefault, iconRollover, contentResource, defaultLocation )
{
    var ret = null;
    
    var obj = { pane                : null,
                name                : name,
                title               : title,
                menu                : menuLabel,
                iconDefault         : iconDefault,
                iconRollover        : iconRollover,
                locator             : defaultLocation
              };
              
    if( this.createPaneObj( obj, contentResource ) )
        ret = obj.pane;
        
    return ret;
}

Panes.prototype.createPaneObj = function( hostObject, contentResource )
{
    //
    // localize pane title and menu item label if required
    //
    var title = hostObject.title;
    var menu  = hostObject.menu;
    
    if( title[0] == '$' )   title = localize( title );
    if( menu[0] == '$' )    menu = localize( menu );

    //
    // create pane
    //
    var pane = ScriptUI.workspace.add( 'tab', 
                                       title, 
                                       {    name           : hostObject.name, 
                                            dockingLocater : hostObject.locator ? hostObject.locator : ''
                                       } );
                                       
    //
    // set margings and spacing
    //
    pane.margins	 = 2;
    pane.spacing	 = 2;
	pane.minimumSize = [150, 100];
    
    hostObject.pane = pane;
    
    //
    // set icons for iconified mode
    //
    var id = hostObject.iconDefault;
    var ir = hostObject.iconRollover;
    
    if( File(id).exists )   id = new File(id);
    if( File(ir).exists )   ir = new File(ir);
    
    if( id )
    {
        try
        {
            hostObject.pane.image = ScriptUI.newImage( hostObject.iconDefault, undefined, undefined, hostObject.iconRollover );
        }
        catch( exc )
        {}
    }
    
    //
    // add content to pane
    //
    var contentRes = """group {
			            properties  : 
			            { 
			                name : 'content',
			                borderstyle : 
			            },
	                    alignChildren:['center','bottom'],
	                    alignment:['fill','fill'],
			            margins     : 0,
                        spacing     : 0,
			            orientation : 'column'""";
			            
    if( contentResource && contentResource.length > 0 )
        contentRes += "," + contentResource + "}";
        
    var content = hostObject.pane.add( contentRes );
    hostObject.content = content;
    
    //
    // add properties to pane to access content elements
    //
    for( var i in content )
    {
        var obj = content[i];
        
        if( this.isContentChild( content, obj ) )
            pane[i] = obj;
    }
    
    //
    // add shortcut to all children to access pane
    //
    this.addShortcut( pane, content );
    
    //
    // store (localized) menu item label
    //
    hostObject.pane.menuText = menu;
    
    //
    // add pane to global array
    //
    this.panes.push( hostObject.pane );

    //
    // resize event handler
    //
	hostObject.pane.onResize = 
	hostObject.pane.onResizing = function ()
	{
		this.layout.resize();
	}

    //
    // create core elements
    //
    hostObject.pane.show();
    
    return pane;
}

Panes.prototype.isContentChild = function( content, obj )
{
    for( var c=0; c<content.children.length; c++ )
    {
        if( content.children[c] == obj )
            return true
    }
    
    return false;
}

Panes.prototype.addShortcut = function( pane, content )
{
    for( var i=0; i<content.children.length; i++ )
    {
        content.children[i].rootPane = pane;
        this.addShortcut( pane, content.children[i] );
    }
}