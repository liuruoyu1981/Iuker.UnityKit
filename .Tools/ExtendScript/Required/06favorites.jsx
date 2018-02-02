/**************************************************************************
*
*  @@@BUILDINFO@@@ 06favorites-2.jsx 3.0.0.15  04-March-2008
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

function Favorites()
{
    this.length = 0;
    this.items  = [];

    var trusted = Folder.myDocuments;
    trusted.changePath( 'Adobe Scripts' );
    
    if( !trusted.exists )
        trusted.create();
        
    this.add( localize( "$$$/ESToolkit/Panes/Scripts/DefaultFavorite=Default" ), 
			  trusted.absoluteURI, true, "*.jsx", true );
    globalBroadcaster.registerClient( this, 'shutdown' );
}

Favorites.prototype.add = function( name, path, isDefault, filter, recursive, filterDot )
{
    for( var i=0; i<this.items.length; i++ )
    {
        if( name        == this.items[i].name       &&
            path        == this.items[i].path       &&
            isDefault   == this.items[i].isDefault  &&
            filter      == this.items[i].filter     &&
            recursive   == this.items[i].recursive  &&
            filterDot   == this.items[i].filterDot    )
            // exactly the same Favorite already exists
            return this.items[i];
    }

    for( var i=0; i<this.items.length; i++ )
    {
        if( name == this.items[i].name )
        {
            var count = 1;
            var failed = false;
            
            do
            {
                failed = false;
                var tmp = name + count;
                
                for( var j=0; j<this.items.length; j++ )
                {
                    if( tmp == this.items[j].name )
                    {
                        count++;
                        failed = true;
                        break;
                    }
                }
                
            } while( failed );
            
            name += count;
            break;
        }
    }
            
    var item = new FavoriteItem( name, path, isDefault, filter, recursive, filterDot );
    this.items.push( item );
    this.length++;
    globalBroadcaster.notifyClients( 'favoritesChanged' );
	
	return item;
}

Favorites.prototype.remove = function( favorite )
{
    for( var i=0; i<this.items.length; i++ )
    {
        if( favorite.name        == this.items[i].name       &&
            favorite.path        == this.items[i].path       &&
            favorite.isDefault   == this.items[i].isDefault  &&
            favorite.filter      == this.items[i].filter     &&
            favorite.recursive   == this.items[i].recursive  &&
            favorite.filterDot   == this.items[i].filterDot     )
        {
            this.items.splice(i,1);
            this.length--;
            globalBroadcaster.notifyClients( 'favoritesChanged' );
            break;
        }
    }
}

Favorites.prototype.removeAll = function()
{
    this.length = 0;
    this.items  = [];

    var trusted = Folder.myDocuments;
    trusted.changePath( 'Adobe Scripts' );
    this.add( localize( "$$$/ESToolkit/Panes/Scripts/DefaultFavorite=Default" ), 
			  trusted.absoluteURI, true, "*.jsx", true );
}

Favorites.prototype.replace = function( oldItem, newItem )
{
    for( var i=0; i<this.length; i++ )
    {
        if( this.items[i] == oldItem )
        {
            oldItem.name       = newItem.name;
            oldItem.path       = newItem.path;
            oldItem.isDefault  = newItem.isDefault;
            oldItem.filter     = newItem.filter;
            oldItem.recursive  = newItem.recursive;
            oldItem.filterDot  = newItem.filterDot;
            
            globalBroadcaster.notifyClients( 'favoritesChanged' );
            
            break;
        }
    }
}

Favorites.prototype.readPrefs = function()
{
    var length = prefs.scriptFavorites.getLength( true );
    
    if( length > 0 )
    {
        while( this.items.length > 1 )
            if( !this.items[this.items.length-1].isDefault )
                this.items.pop();
        
        for( var i=0; i<length; i++ )
        {
            var name        = prefs.scriptFavorites['scriptFavorites'+i].name.getValue( Preference.STRING );
            var path        = prefs.scriptFavorites['scriptFavorites'+i].path.getValue( Preference.STRING );
            var filter      = prefs.scriptFavorites['scriptFavorites'+i].filter.getValue( Preference.STRING );
            var recursive   = prefs.scriptFavorites['scriptFavorites'+i].recursive.getValue( Preference.BOOLEAN );
            var filterDot   = prefs.scriptFavorites['scriptFavorites'+i].filterDot.getValue( Preference.BOOLEAN );
 
			if( name.length > 0 )
				this.add( name, path, false, filter, recursive, filterDot );
        }
    }
}

Favorites.prototype.writePrefs = function()
{
    var c = 0;
        
    if( this.length > 0 )
    {
        for( var i=0; i<this.length; i++ )
        {
            if( !this.items[i].isDefault && this.items[i].name.length > 0 )
            {
                prefs.scriptFavorites['scriptFavorites'+c].name       = this.items[i].name;
                prefs.scriptFavorites['scriptFavorites'+c].path       = this.items[i].path;
                prefs.scriptFavorites['scriptFavorites'+c].filter     = this.items[i].filter;
                prefs.scriptFavorites['scriptFavorites'+c].recursive  = this.items[i].recursive;
                prefs.scriptFavorites['scriptFavorites'+c].filterDot  = this.items[i].filterDot;
                
                c++;
            }
        }
    }
}

Favorites.prototype.onNotify = function( reason, param01 )
{
    if( reason == 'shutdown' )
    {
        globalBroadcaster.unregisterClient( this );
        
        if( !param01 )
            this.writePrefs();
    }
}

Favorites.prototype.dialog = function( item )
{
    var ret = null;
    var text = '';
    var touchedName = false;
    
	workspace.storeFocus();

    if( item )
    {
        text = "text:'$$$/ESToolkit/Dialog/Favorites/TitleMod=Modify Favorites'";
        touchedName = true;
    }
    else
        text = "text:'$$$/ESToolkit/Dialog/Favorites/TitleNew=Create New Favorite'";

    var dlg = new Window( 
        """prefdialog { """ + text + """,
            orientation : 'column',
            properties : { name : 'favorites' },
            g1  : Group
            {
                alignChildren:'left',
                orientation : 'column',
                grp1 : Group
                {
                    orientation : 'row',
                    namegrp : Group
                    {
                        orientation : 'column',
                        tn  : StaticText
                        {
                            alignment : 'left',
                            text    : '$$$/ESToolkit/Dialog/Favorites/Name=Name'
                        },
                        favName    : EditText
                        {
                            alignment : ['fill','top'],
                            characters  : 40
                        }
                    },
                    filtergrp   : Group
                    {
                        orientation : 'column',
                        fn  : StaticText
                        {
                            alignment : 'left',
                            text    : '$$$/ESToolkit/Dialog/Favorites/Filter=Filter'
                        },
                        filter  : EditText
                        {
                            alignment : ['fill','top'],
                            charachters : 15
                        }
                    }
                }
                path    : Group
                {
                    orientation : 'row',
                    pathStr : EditText
                    {
                        alignment:'fill',
                        text    : '',
                        characters  : 40,
                        properties :{readonly    : true }
                    },
                    button  : IconButton
                    {
                        alignment:'right',
                        icon    : '#FolderOpened',
                        preferredSize: [20,20],
                        helpTip : '$$$/ESToolkit/Dialog/Favorites/htFolder=Select a folder for this Favorite.'
                    }
                },
                grp2 : Group
                {
                    orientation : 'row',
                    alignment   : ['fill','fill'],
                    recursive   : Checkbox
                    {
                        alignment   : ['left','top'],
                        text    : '$$$/ESToolkit/Dialog/Favorites/Recursive=Recursive Folders'
                    },
                    filterDot   : Checkbox
                    {
                        alignment   : ['right','top'],
                        text    : '$$$/ESToolkit/Dialog/Favorites/FilterDot=Ignore Files starting with a dot'
                    }
                }
            },
            g2  : Group
            {
                orientation : 'row',
                btOK  : Button
                {
                    text : '$$$/CT/ExtendScript/UI/OK=&OK'
                },
                btCancel  : Button
                {
                    text : '$$$/CT/ExtendScript/UI/Cancel=&Cancel'
                }
            }
        }""" );
            
    dlg.nameField   = dlg.g1.grp1.namegrp.favName;
    dlg.filterField = dlg.g1.grp1.filtergrp.filter;
    dlg.pathField   = dlg.g1.path.pathStr;
    dlg.pathBtn     = dlg.g1.path.button;
    dlg.recursBtn   = dlg.g1.grp2.recursive;
    dlg.filterDot   = dlg.g1.grp2.filterDot;
    
    if( item )
    {
        dlg.nameField.text      = item.name;
        dlg.pathField.text      = File(item.path).fsName;
        dlg.filterField.text    = item.filter;
        dlg.recursBtn.value     = item.recursive;
        dlg.filterDot.value     = item.filterDot;
    }
    else
    {
        dlg.nameField.text      = localize( "$$$/ESToolkit/Dialog/Favorites/NewFavorite=New Favorite" );
        dlg.pathField.text      = '';
        dlg.filterField.text    = '*.jsx';
        dlg.recursBtn.value     = false;
        dlg.filterDot.value     = true;
    }
    
    dlg.nameField.onChange   = 
	dlg.nameField.onChanging = function()
    {
        touchedName = true;
    }
    
    dlg.pathBtn.onClick = function()
    {
        var defFolder = ( item ? Folder( item.path ) : app.currentFolder );
        var path = Folder.selectDialog( localize( '$$$/ESToolkit/FavDlg/selectScripts=Select a scripts folder' ), defFolder ); 
        
        if( path )
        {
			var pathName = path.fsName;
			if (pathName == "")
				pathName = "/";
            this.window.pathField.text = pathName;
            
            if( !touchedName )
				this.window.nameField.text = (pathName == "/") ? pathName : decodeURI( path.name );
        }
    }
    
    dlg.g2.btCancel.onClick = function()
    {
        this.window.close();
    }
    
    dlg.g2.btOK.onClick = function()
    {
        if( this.window.pathField.text.length > 0 )
            ret = new FavoriteItem( this.window.nameField.text, 
                                    this.window.pathField.text, 
                                    false, 
                                    this.window.filterField.text, 
                                    this.window.recursBtn.value,
                                    this.window.filterDot.value );
            
        this.window.close();
    }
    
    dlg.onShow = function()
    {
        this.window.nameField.active = true;
    }
    
    dlg.defaultElement = dlg.g2.btOK;
    dlg.cancelElement  = dlg.g2.btCancel;
    dlg.show();
    
	workspace.restoreFocus();

    return ret;
}

function FavoriteItem( name, path, isDefault, filter, recursive, filterDot )
{
    this.name       = name;
    this.path       = path;
    this.isDefault  = ( typeof(isDefault)   == "undefined" ? false : isDefault );
    this.filter     = ( typeof(filter)      == "undefined" ? '*.*' : filter );
    this.recursive  = ( typeof(recursive)   == "undefined" ? false : recursive );
    this.filterDot  = ( typeof(filterDot)   == "undefined" ? true  : filterDot );
}

FavoriteItem.prototype.equal = function( favorite )
{
	return ( favorite.name        == this.name       &&
 			 favorite.path        == this.path       &&
 			 favorite.isDefault   == this.isDefault  &&
			 favorite.filter      == this.filter     &&
			 favorite.recursive   == this.recursive  &&
			 favorite.filterDot   == this.filterDot      );
}
