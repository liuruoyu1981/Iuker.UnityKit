/**************************************************************************
*
*  @@@BUILDINFO@@@ 95favoritesPrefs-2.jsx 3.0.0.14  27-February-2008
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

var favoritesPrefs = { title : "$$$/ESToolkit/PreferencesDlg/favoritesTitle=Favorites",
                       todefault : false, 
                       sortOrder : 60 };

globalBroadcaster.registerClient( favoritesPrefs, 'initPrefPanes' );

favoritesPrefs.onNotify = function( reason )
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

favoritesPrefs.create = function (parentPane)
{
    this.pane = parentPane.add (
                "group {																	                                    \
	                orientation:'column', alignChildren:['fill','fill'], visible:false, alignment: ['fill','fill'],				\
	                list        : ListBox                                                                                       \
	                {                                                                                                           \
		                helpTip : '$$$/ESToolkit/PreferencesDlg/hiFavrites=List of defined favorites used in the Scripts panel' \
	                },                                                                                                          \
	                buttons     : Group                                                                                         \
	                {                                                                                                           \
		                orientation : 'row',                                                                                    \
		                alignment   : ['left','bottom']\
		                btAdd       : Button                                                                                    \
		                {                                                                                                       \
			                text        : '$$$/ESToolkit/PreferencesDlg/addFav=Add...'											\
		                },                                                                                                      \
		                btModify    : Button                                                                                    \
		                {                                                                                                       \
			                text        : '$$$/ESToolkit/PreferencesDlg/modFav=&Modify'											\
		                }                                                                                                       \
		                btRemove    : Button                                                                                    \
		                {                                                                                                       \
			                text        : '$$$/ESToolkit/PreferencesDlg/remFav=&Remove'											\
		                }                                                                                                       \
	                }                                                                                                           \
                }");
                
	this.pane.prefsObj = this;
	
	this.layoutDone = function()
	{
		this.pane.list.textWidth = this.pane.list.size.width / 2;
		var items = this.pane.list.items;
		for (var i = 0; i < items.length; i++)
		{
			var item = items [i];
			var favorite = item.favorite;
			item.setTabbedText (favorite.name, this.pane.list.textWidth, decodeURIComponent( favorite.path ) );
		}
	}
	
	this.preProcess = function()
	{
		// Needs always to be loaded - favorites may have changed
		this.load();
	}
	this.postProcess = function()
	{}

	this.toDefault = function()
	{
		for( var i=0; i<this.pane.list.items.length; i++ )
		{
			if( !this.pane.list.items[i].favorite.isDefault )
			{
				this.pane.list.remove(i);
				i--;
			}
		}
	}

	return this.pane;
}

/////////////////////////////////////////////////////////////////////////
// initialize the favorites options pane

favoritesPrefs.load = function()
{
	with (this.pane) 
	{
//	    list.graphics.font = ScriptUI.newFont( "FixedWidth" );
	    
	    list.onChange = function()
	    {
	        this.parent.buttons.btModify.enabled = 
	        this.parent.buttons.btRemove.enabled = ( this.selection && !this.selection.favorite.isDefault );
	    }
	    
	    buttons.btAdd.onClick = function()
	    {
	        var newItem = favorites.dialog();
	        
	        if( newItem )
	        {
	            var item        = list.add( 'item', "");
	            item.favorite   = new FavoriteItem( newItem.name, newItem.path, false, newItem.filter, newItem.recursive );
				item.setTabbedText (newItem.name, list.textWidth, decodeURIComponent( newItem.path ) );
	        }
	    }
	    
	    buttons.btRemove.onClick = function()
	    {
			var list = this.parent.parent.list;
	        var selItem = list.selection;
	        if( selItem )
	        {
	            list.remove( selItem );
				list.selection = 0;
	        }
	    }
	    
	    buttons.btModify.onClick = function()
	    {
			var list = this.parent.parent.list;
	        var selItem = list.selection;
	        
	        if( selItem )
	        {
	            var newItem = favorites.dialog( list.selection.favorite );
	            
	            if( newItem )
	            {
	                selItem.favorite = newItem;
					selItem.setTabbedText (newItem.name, list.textWidth, decodeURIComponent( newItem.path ) );
	            }
	        }
	    }
	    
	    list.removeAll();
	    
	    if( favorites )
	    {
	        for( var i=0; i<favorites.length; i++ )
	        {
	            var item        = list.add( 'item', "" );
	            item.favorite   = new FavoriteItem( favorites.items[i].name, 
	                                                favorites.items[i].path, 
	                                                favorites.items[i].isDefault, 
	                                                favorites.items[i].filter, 
	                                                favorites.items[i].recursive );
				if (list.textWidth)
					item.setTabbedText (favorites.items[i].name, list.textWidth, decodeURIComponent( favorites.items[i].path ) );
	        }
	        
	        list.selection = 0;
	    }
	}
}

/////////////////////////////////////////////////////////////////////////
// store preferences from the selected favorites options
favoritesPrefs.store = function()
{
    favorites.removeAll();
    
    if( this.pane.list.items.length > 0 )
    {
        for( var i=0; i<this.pane.list.items.length; i++ )
        {
            if( !this.pane.list.items[i].favorite.isDefault )
                favorites.add( this.pane.list.items[i].favorite.name, 
                               this.pane.list.items[i].favorite.path, 
                               false, 
                               this.pane.list.items[i].favorite.filter,
                               this.pane.list.items[i].favorite.recursive );
        }
    }
	
	globalBroadcaster.notifyClients( 'newFavorites' );
	
	return false;
}

