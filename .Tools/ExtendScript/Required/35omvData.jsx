/**************************************************************************
*
*  @@@BUILDINFO@@@ 35omvData-2.jsx 3.5.0.22	28-April-2009
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

// Object Model Viewer Data.
// For dynamic targets, the ESTK stores the mined XML into app.prefsFolder.
// The file name is omv$btid$dictname.xml, e.g. omv$indesign-6.0$CS5.xml.
// The locale is stripped of, as OMVs are English only.

// The location for static XML files.

OMVData.omvFolder = Folder.commonFiles + "/Adobe/Scripting Dictionaries CS6/";

//
// for transition time from CS5 to CS6
//
if( !OMVData.omvFolder )
	OMVData.omvFolder = Folder.commonFiles + "/Adobe/Scripting Dictionaries CS5/";

// The name of the default dictionary for InDesign.

OMVData.defaultDictName = "CS6";

// text search flags
OMVData.SEARCH_SUBSTRING      = 1;
OMVData.SEARCH_FULLTEXT       = 2;
OMVData.SEARCH_GLOBAL         = 4;
OMVData.SEARCH_CASE_SENSITIVE = 8;
OMVData.SEARCH_NO_GLOBAL	  = 16;

// Create an OMV data provider. Takes the BridgeTalk ID of the target.
// Properties:
// btID			the full BridgeTalk ID if installed, the bare name if not
// href			the href (either name# or name/dict#)
// xml			the original XML
// map			a copy of the XML map, single-level
// dict			the dictionary name for dynamic mining, null otherwise
// file			a File instance pointing to the OMV file to load
// title		the title of the TOC
// dynamic		true if the target is dynamic and can be mined
// loaded		true if the xml is complete
// menu			the menu command item (if we need to disable)
// asked		true if the user has been asked if he wants to load the OMV data
// loadOK		true if loading is OK because user agreed
// showProgress	true if the OMV UI should show a progress dialog
// isDflt		true if this is a default dictionary
// isCommon		true fo $COMMON dictionaries
// mined		true if this is a dynamic dict that has been mined

function OMVData (btID, dictName, title, fileName, isDflt, dyn)
{
	this.init (btID, dictName, title, fileName, isDflt, dyn);
}

OMVData.util = {};		// container for static helper functions

OMVData.prototype.init = function (btID, dictName, title, fileName, isDflt, dyn)
{
	// erase OMV registry
	if (this.btID)
	{
		var propName = OMVData.util.createHRef( this.btID );
		
		if (OMVData.hrefs [propName] == this)
			delete OMVData.hrefs [propName];
		if (OMVData.hrefs [this.href] == this)
			delete OMVData.hrefs [this.href];
	}

	// the btID is $COMMON" for estoolkit-3.8
	// create the href
	var href = OMVData.util.createHRef( btID, dictName );

	this.title		= title;
	this.isCommon	= (btID == "$COMMON");
	this.btID		= this.isCommon ? BridgeTalk.appSpecifier : btID;
	this.href		= href;
	this.dict		= dictName;
	this.file		= fileName;
	this.menu		= null;
	this.xml		=
	this.map		= null;
	this.loaded		= false;
	this.dynamic	= dyn;
	this.asked		=
	this.loadOK		= (btID == BridgeTalk.appSpecifier);
	this.showProgress = !this.loadOK;
	this.mined		= false;
	this.isDflt		= isDflt;

	OMVData.hrefs [href] = this;
	// set default OMV
	if (isDflt && dictName.length > 0 && !this.isCommon)
		OMVData.hrefs [OMVData.util.createHRef( btID )] = this;
}

OMVData.prototype.toString = function()
{
	// debug aid
	return "[OMV " + this.href + "]";
}

// OMVData.all is a flat list of all OMVs, sorted by group and alphabetically.

OMVData.all = [];

// OMVData.common is a flat list of all $COMMON OMVs, sorted by group and alphabetically.

OMVData.common = [];

// The OMVData.hrefs object stores all OMVs under their href. 

OMVData.hrefs = {}

// An object containing the last href, text, and result for getCodeHints().

OMVData.lastFindInfo = { href:"", text:"", found:null };

// Set up all OMV entries in the Help menu. When a menu item is selected,
// the OMV data is loaded, and the UI is created or made visible.

OMVData.setup = function()
{
	// Collect all XML info into two groups. 
	// Each element contains an OMVData instance
	var groups = [
		// Group 0 is for CommonFiles
		[],
		// Group 1 is for the other files. 
		[]
	];
	// grab *all* targets
	var btIDs = BridgeTalk.getTargets (-9999, null);

	for (var i = 0; i < btIDs.length; i++)
	{
		var btID = btIDs [i];
		// This is debug code - newer versions of ID and IC install themselves without
		// a locale
		if (btID == "indesign-6.0-en_us" || btID == "incopy-6.0-en_us")
			continue;

		var dictType = BridgeTalk.getOMVDictionaryType (btID);
		if (dictType.substr (0, 7) == "dynamic")
		{
			var name = this.getDisplayName (btID);
			// Should not happen in production code, but make sure to ignore CS3 apps
			if (name.indexOf ("CS3") >= 0 || name.indexOf ("CS 3") >= 0)
				continue;

			var split = btID.split ('-');
			var fileBase = "omv$" + split[0] + "-" + split[1];
			// Check all files for prefsFolder/omv$btid$*.xml
			var files = Folder (app.prefsFolder).getFiles (fileBase + "$*.xml");
			if (files.length > 0)
			{
				// It appears that the target has been mined
				for (var j = 0; j < files.length; j++)
				{
					// Create the file name: ESTK prefs folder/omv$btid$dict[$dflt].xml
					var parts = files[j].name.match (/omv\$(.+)\.xml/);
					if (parts)
					{
						parts = parts[1].split ('$');
						if (parts[1])
						{
							// if part [2] == "dflt", this is the default dict
							// a bug created "dlft" files :)
							var isDflt = ((parts[2] == "dflt") || (parts[2] == "dlft"));
							// read the title
							var title = OMVData.util._getOMVTitleFromFile (files [j]);
							if (!title)
								title = localize ("%1 (%2)", name, parts[1]);

							var data = new OMVData (btID, parts[1], title, files[j].absoluteURI, isDflt, true);
							data.mined = true;
							groups[1].push (data);
						}
					}
				}
			}
			else
			{
				// It appears that the target has not been mined yet, so create a dummy OMV
				// Create the file name: ESTK prefs folder/omv$btid$.xml
				var f = app.prefsFolder + "/" + fileBase + "$.xml";
				// We used to translate 'Object Model' but that led to inconsistent titles
				var _name = name;
				if( _name.indexOf( "Object Model" ) < 0 && _name.indexOf( "Object Library" ) < 0 && _name.indexOf( "Type Library" ) < 0 )
					_name += " Object Model";

				var data = new OMVData (btID, "", _name, f, true, true);
				data.mined = false;
				groups[1].push (data);
			}
		}
	}
	// Add all directories inside the Dictionaries folder
	var dictFolder = Folder (OMVData.omvFolder);
	var folders = dictFolder.getFiles();
	if (null == folders)
		folders = [];

	for (var i = 0; i < folders.length; i++)
	{
		var f = folders [i];
		if (!(f instanceof Folder))
			continue;

		var files = f.getFiles ("*.xml");
		for (var j = 0; j < files.length; j++)
		{
			// the title of the map
			var title = "";

			// Attempt to load the beginning of the XML file,
			// to find the official reference name plus any dynamic dict info
			var xmlfile = files [j];
			var title = OMVData.util._getOMVTitleFromFile (xmlfile);
			if (!title)
				// no title; this must be a sub-XML file
				continue;
			
			// determine the group; group 0 is for CommonFiles, group 1 for everything else
			btID = f.name;
			var dict = "";
			var grp = 1;
			if (btID.toUpperCase() == "COMMONFILES")
			{
				grp = 0;
				btID = "$COMMON";
				dict = xmlfile.name;
				// remove .xml
				dict = dict.substr (0, dict.indexOf (".xml"));
			}
			else
				btID = BridgeTalk.getSpecifier (btID);

			if (!btID)
				// use this for installed OMVs without the app installed
				btID = f.name;

			// create the object for the group
			data = new OMVData (btID, dict, title, xmlfile, true, false);
			groups [grp].push ( data );
		}
	}

	// Sort the groups by title
	function sortfn (a, b)
	{
		var quo = 1;

		//
		// same application of different versions?
		//
		var pos1 = a.title.lastIndexOf( "(" );
		var pos2 = b.title.lastIndexOf( "(" );

		if( pos1 > 0 && pos2 > 0 &&
			a.title.substr( 0, pos1 ) == b.title.substr( 0, pos2 ) )
			quo = -1;

		//
		// compare a and b
		//
		if (a.title < b.title)
			return -1 * quo;
		else if (a.title > b.title)
			return 1 * quo;
		else
			return 0;
	}
	for (grp = 0; grp < 2; grp++)
		groups [grp].sort (sortfn);
		
	// We got all groups lined up; now, set up the menu
	for (grp = 0; grp < 2; grp++)
	{
		for (var i = 0; i < groups [grp].length; i++)
		{
			var data = groups [grp][i];
			// this ensures the proper oder in the all[] array
			OMVData.all.push (data);
			// add to common array if appropriate
			if (data.isCommon)
				OMVData.common.push (data);
		}
	}
	menus.help.omv = new MenuElement ("command", "$$$/ESToolkit/OMV/Name=Object Model Viewer", "at the beginning of help", "help/omv");
	menus.help.omv.enabled = app.hasApe;
	menus.help.omv.onSelect = function()
	{
		if( app.modalState ) return;
		// If there is no UI yet, now it is the time to create one!
		if (!OMV.ui)
		{
			OMV.ui = new OMV();
		}
		OMV.ui.show();
	}
}

OMVData.initUi = function()
{
	var href = "$COMMON/javascript#";
	// load the current document's target if there is one
	if( document && document.getCurrentTarget )
	{
		var target = document.getCurrentTarget();

		if( target )
		{
			var temp = target.address.target;
			if (temp != BridgeTalk.appSpecifier)
				href = temp + "#";
		}
	}
	OMVData.util._loadOMVsIntoUI();
	OMV.ui.select (href);
	OMV.ui.setBookmarks (prefs.omv.bookmarks);
	OMV.ui.setPrefSearchDesc (prefs.omv.searchDesc);
			
	return;
}

// Update the document flyout menu with the list of OMVs.

OMVData.updateDocFlyout = function (autoComplete, menu)
{
	var currentHref = autoComplete.getHref();
	var wasCommon = false;

	for (var i = 0; i < OMVData.all.length; i++)
	{
		var omv = OMVData.all [i];
		var where = "at the end of " + menu.id;
		if (wasCommon && !omv.isCommon)
			where = '---' + where;	/// separator between common and other OMVs
		wasCommon = omv.isCommon;
		var item = new MenuElement( "command", omv.title, where);
		item.href = omv.href;
		item.autoComplete = autoComplete;
		item.alwaysLoaded = omv.isCommon;
		item.onDisplay = function()
		{
			if (this.alwaysLoaded)
			{
				this.enabled = false;
				this.checked = true;
			}
			else
				this.checked = (this.autoComplete.getHref() == this.href);
		}
		item.onSelect = function()
		{
			this.autoComplete.setHref (this.href);
		}
	}
}

// Load the data. If the data is dynamic, mine the target.

OMVData.prototype.load = function()
{
	if( this.loading )
		return true;

	this.loading = true;

	if (!this.asked)
	{
		this.asked = true;
		var name = BridgeTalk.getDisplayName (this.btID);
		this.loadOK = ( prefs.omv.confirmLoad.getValue( Preference.BOOLEAN ) )
					? dsaQueryBox ( "omv1", "$$$/ESToolkit/OMV/NotLoaded=The Object Model for %1 has not yet been loaded.^nLoad it now?", name)
					: true;
	}

	if (this.loadOK && !this.loaded)
	{
		// If this is a mining placeholder, mine now
		if (this.dynamic && !this.mined)
		{
			this.loading = false;
			return this._mineAllOMVs();
		}
		// Do progress messages for all OMV files except for the CommonFiles
		else if (this.showProgress)
		{
			var msg = localize ("$$$/ESToolkit/OMV/LoadingObjectModel=Loading Object Model for %1...", this.title);
			docMgr.showProgress( true );
			docMgr.setProgress (0, 100);
			docMgr.setProgressText (msg);
			// for _prepareXML()
			docMgr.maxProgress = 100;
			this._loadXML();
			docMgr.showProgress( false );
			if( OMV.ui )
				OMV.ui.activate();
		}
		else
			this._loadXML();
	}

	this.loading = false;

	return this.loaded && (this.xml != null);
}

// Get the display name for this OMV data instance

OMVData.prototype.getDisplayName = function()
{
	return this.title;
}

// Get the display name for a BT specifier

OMVData.getDisplayName = function (btID)
{
	var name = BridgeTalk.getDisplayName (btID);
	if (!name)
	{
		name = btID.split ('-')[0];
		if (name == "aftereffects")
			name = "After Effects";
		else
			// just make 1st character upper case
			name = name[0].toUpperCase() + name.substr (1);
	}
	return name;
}

// Find an OMV by href. The href is omvname[/dict]#.

OMVData.find = function (href)
{
	var data = OMVData.hrefs [href];
	if (!data)
	{
		// Try the default OMVData with an empty dict
		href = href.split ('/');
		if (href.length == 2)
			data = OMVData.hrefs [href[0] + '#'];
	}
	if (data && !data.loaded)
		data.load();
	return (data && data.xml) ? data : null;
}

/**
Load all XML files from disk, and merge their contents.
The returned value is true for OK, false for errors.

For dynamic targets, the ESTK loads the mined XML into app.prefsFolder.
The file name is omv$btid$[dictname], e.g. omv$indesign$CS5.xml.
If there is no file like this, attempt to connect to the target 
and ask the target for its dynamic OMV.

For static targets, check for a subfolder that has the same name as the
main XML file. If e.g. the main file is photoshop/omv.xml, check for
photoshop/omv/*.xml. Load and merge these files into the main XML file.
*/

OMVData.prototype._loadXML = function()
{
	this.xml = <dictionary><map/><package/></dictionary>;

	var f = File (this.file);

	if (f.exists)
	{
		// The progress is set up as follows:
		// 20% = loading and parsing all files
		// 80% = preparing XML
		// To have a good resolution, we set the max value to (#files*100), and increment by 2 * 10

		var text = docMgr.safeRead (f, true);
		if (text)
		{
			docMgr.incProgress (10);
			try
			{
				text = text.replace (/<dictionary.*>/, "<dictionary>");
				this.xml = new XML (text);
			}
			catch (e)
			{
				// XML error: ignore the temp file
				f.remove();
			}
		}
		if (!this._mineTargetIfNewer())
			return false;

		if (!this.dynamic && !this._loadSubXMLFiles (f))
		{
			this._badXML ("Bad sub XML files");
			return false;
		}

		this.loaded = true;

		return this._prepareXML();
	}
	else if (this.dynamic)
	{
		var target = targetMgr.findTargetSpecifier( this.btID );
		
		if(!target)
			return false;

		// need to mine the target
		if( !app.targetAppRunning( target ) )
		{
			var launchMsg = "$$$/ESToolkit/OMV/Launch=%1 must be running to retrieve Object Model data.^nDo you want to launch %1?";
			
			if( !app.launchTargetAppSynchronous( target, launchMsg, true ) )
				return false;
		}

		return (this._mineTargetIfNewer() && this._prepareXML());
	}
	else
	{
		this._badXML ("Cannot open " + f);
		return false;
	}
}

// Load all sub-XML files. These files contain additional information. They are
// found in a subdirectory carrying the same name as the main XML file. The names
// of these files are irrelevant. Their XML is merged into the main XML.

OMVData.prototype._loadSubXMLFiles = function (f)
{
	// strip the .xml part
	var dir = Folder (f.fsName.substr (0, f.fsName.length - 4));
	var files = dir.getFiles ("*.xml");
	if (0 == files.length)
		return true;
	// The progress is set up as follows:
	// 20% = loading and parsing main files
	// 60% = loading and parsing sub files
	// 20% = preparing XML
	docMgr.incProgress (20);
	var increment = 60 / (files.length * 2);
	for (var i = 0; i < files.length; i++)
	{
		f = files [i];
		var resolved = f.resolve();
		if (resolved)
			f = resolved;
		if (f.exists)
		{
			docMgr.incProgress (increment);
			var text = docMgr.safeRead (f, true);
			if (text == null)
				return false;
			var xml;
			try
			{
				// Remove all namespaces
				text = text.replace (/<dictionary.*>/, "<dictionary>");
				xml = new XML (text);
			}
			catch (e)
			{
				return false;
			}
			docMgr.incProgress (increment);
			var globals = xml.map.topicref.xpath ('topicref[@href="#/global"]');
			if (globals.length() == 1)
			{
				// Update my own <map> element with the globals if not present
				delete globals.parent()[globals.childIndex()];
				var myGlobals = this.xml.map.topicref.xpath ('topicref[@href="#/global"]')
				if (myGlobals.length() == 0)
					this.xmp.map.appendChild (globals);

				// Add the globals instance stuff to my globals
				globals = xml['package'].xpath ('classdef[@name="global"]');
				myGlobals = this.xml['package'].xpath ('classdef[@name="global"]');
				if (globals.length() > 0)
				{
					if (myGlobals.length() == 0)
						this.xml['package'].appendChild (globals);
					else
					{
						var inst = globals.xpath ('elements[@type="instance"]');
						var myInst = myGlobals.xpath ('elements[@type="instance"]');
						if (myInst.length() == 0)
							myGlobals.appendChild (inst);
						else
							myInst.appendChild (inst.children());
					}
					// remove from the original tree
					delete globals.parent() [globals.childIndex()];
				}
			}
			// Add the other classes
			this.xml.map.appendChild (xml.map.children());
			this.xml['package'].appendChild (xml['package'].children());
		}
		else
			return false;
	}
	return true;
}


// Create a single-level TOC, ignoring the top level structure for now, and sort the TOC.
// Store this TOC under this.map. Returns false on abort.

OMVData.prototype._prepareXML = function()
{
	if (!this.xml)
		return false;

	var pkg = this.xml['package'];
	// Get rid of the level 1 entries for now
	var list = this.xml.map.children().children();
	// Remove duplicates
	this.map = XML ('<map/>');
	// set the OMV title if present
	var title = this.map.@title.toString();
	if (title)
		this.title = title;

	// Set up the classes at the OMV object
	this.classNames = {};

	var increment = docMgr.maxProgress / list.length() / 20;
	
	// Record class types in an associative array
	// Types are associated to a class name
	var classes = this.xml['package'].classdef;
	var classTypes = {};
	for each (var clss in classes)
	{
	    classTypes[clss.@name] = clss.@enumeration;
	}
	
	for (var i = 0; i < list.length(); i++)
	{
	    // We want to append the enumeration type to the TOC
	    // because the OMV UI displays the class type
        list[i].@enumeration = classTypes[list[i].@navtitle];
	    
		if (!(i % 20))
			docMgr.incProgress (increment);
		var name = list [i].@navtitle.toString();
		if (this.classNames [name])
			continue;
		this.classNames [name] = list [i].@href.toString();
		this.map.appendChild (list [i]);
	}
	return true;
}

OMVData.prototype.getClassInfo = function( prefix, group, noErrorMsg )
{
    var xml = null;
    var target = targetMgr.findTargetSpecifier( this.btID );
    
    if( target )
    {
		if (!app.targetAppRunning( target ) )
		{
			var launchMsg = "$$$/ESToolkit/OMV/Launch=%1 must be running to retrieve Object Model data.^nDo you want to launch %1?";
	        if( !app.launchTargetAppSynchronous( target, launchMsg, true ) )
		        return false;
		}
        if( !prefix )
            prefix = "";
        if (!group)
			group = "";
        var job = target.cdic.getDictionaryClassInfo( target.address, this.dict, prefix, group );
        var res = cdicMgr.callSynchronous( job, 300000 );

        if( res.length == 1 )
		{
			try
			{
				xml = new XML( res[0][0] );
			}
			catch( exc )
			{
				xml = null;
			}
		}

        if( !xml && !noErrorMsg )
            this._badXML ("App did not return valid XML");
    }
    
    return xml;
}

// Error message for bad XML.

OMVData.prototype._badXML = function (reason)
{
	docMgr.showProgress( false );
	if( OMV.ui )
		OMV.ui.activate();

	errorBox( localize( '$$$/ESToolkit/Alerts/_badXML=Cannot load XML!\nHelp is not available.' ) );
	this.xml = null;
	this.loaded = true;

	if ($.version.indexOf ("(debug)") > 0)
	{
		print ("Cannot load XML! Reason: " + reason);
		print ($.stack);
	}
}

// Finds the given text, 'what', in class names, property/method 
// names, short descriptions, and descriptions.  Case does not
// matter.  This method could take a few minutes on larger 
// data sets.  I'm open to ideas for improving its performance.
// Returns an xml, <results>, of <item href="..." anchor="..."/>
// items.  @href is the fully qualified href (including OMV entry)
// and @anchor is either the name of the class or the name of
// the class element.

OMVData.prototype.findString = function (what, flags)
{
	// Note that app.searchOMV returns a string formatted as XML!
	var ret = null;

	try
	{
		ret = new XML (app.searchOMV (this.xml, this.href, what, flags));
	}
	catch( exc )
	{
		ret = null;
	}

	return ret;
}

// Get the XML for a href

OMVData.prototype.getXML = function (href)
{
	var omv = this;
	var parts = href.split ('#');
	if (parts.length == 2)
	{
		omv = OMVData.find (parts [0] + '#');
		href = parts [1];
		if (!omv)
			return null;
	}
	parts = href.split ('/');
	if (parts.length < 2)
		return null;
	
	var xml = omv.xml['package'].classdef.(@name == parts [1]);
	if (xml.length() == 0)
		return null;

	xml = xml [0];
	if (parts.length > 2)
	{
		// container
		var list = null;
		try
		{
			list = xml.elements.(@type == parts [2]);
		}
		catch (e) {}
		if (!list) try
		{
			// no found container: retry instance in case of (faulty) Array/slice
			list = xml.elements.(@type == "instance");
		}
		catch (e) {}
		if (!list)
			return null;
		xml = list[0];
	}
	if (parts.length == 4)
	{
		// element
		try
		{
			var list = xml.children().(@name == parts [3]);
			xml = list [0];
		}
		catch (e)
		{
			xml = null;
		}
	}
	return xml;
}

// Split up a HREF. Returns an object with these properties:
// file - the resolved file portion, "" if no file portion
// pkg  - the package name, "" for no package name
// cls  - the class name
// type - the element container type (e.g. class, instance, "" if not present)
// name - the element name ("" if not present)

OMVData.prototype._splitHref = function (href)
{
	var obj = { file:"", cls:"", type:"", name:"" };

	var file = href.toString().split ('#');
	if (file.length == 2)
		href = file [1], file = file [0];
	else
		href = file [0], file = "";

	if (file.length)
	{
		if (file.indexOf ("$COMMON/") == 0)
		{
			// Replace the shortcut with the real name
			file = OMVData.omvFolder + "/CommonFiles" + file.substr (7);
			if (file.substr (file.length - 4).toUpperCase() != ".XML")
				file += ".xml";
		}
		else
		{
			// This is the root folder of the OMV files for a target
			file = OMVData.omvFolder + "/" + file;
		}
		obj.file = file;
	}

	// just in case someone forgot to add the leading slash
	if (href.length == 0)
		// assume all classes
		href = "/";
	else if (href[0] != '/')
		href = '/' + href;
	href = href.split ('/');
	obj.pkg = href [0];
	if (href.length > 1)
		obj.cls = href [1];
	if (href.length > 2)
	{
		// if the container type is missing, assume "instance"
		if (href.length == 3)
			obj.type = "instance",
			obj.name = href [2];
		else
			obj.type = href [2],
			obj.name = href [3];
	}
	return obj;
}

// Code Completion interface.

OMVData.getCodeHints = function (href, text, flags)
{
	if (OMVData.lastFindInfo.href == href
	 && OMVData.lastFindInfo.text == text
	 && OMVData.lastFindInfo.flags == flags)
		return OMVData.lastFindInfo.found;

	// Create a complete array of all OMVs
	var data = [];
	var omv  = OMVData.find (href);
	if (omv)
		data.push (omv);

	// add all common OMVs
	for (var i = 0; i < OMVData.common.length; i++)
	{
		var commonOMV = OMVData.common [i];
		if (commonOMV != omv)
		{
			// make sure that all are loaded
			if (commonOMV.loaded || commonOMV.load())
				data.push (OMVData.common [i]);
		}
	}

	var xml = new XMLList();
	var count = 0;

outer:
	for (var i = 0; i < data.length; i++)
	{
		var results = data [i].findString (text, flags).children();
		for (var j = 0; j < results.length(); j++)
		{
			var elemXML = data [i].getXML (results [j].@href);
			// should always return something
			$.bp (elemXML == null);
			if (elemXML)
			{
				xml += elemXML;
				if (++count > 20)
					break outer;
			}
		}
	}

	// return an array of strings for Code Completion
	var returnArray = [];
	// Add an hrefs{} object to the found info, where each href is stored
	// as a property under the generated array element's text
	this.lastFindInfo = { href:href, text:text, found:returnArray, flags:flags, hrefs: {} };

	if (xml)
	{
		for (var i = 0; i < xml.length(); i++)
		{
			var elem = xml [i];
			var name;
			var clsName = "";
			var grpName = "";
			var elemName = "";
			if (elem.localName() == "classdef")
				clsName =
				name = elem.@name.toString();
			else
			{
				clsName =
				name = elem.parent().parent().@name;
				grpName = elem.parent().@name;
				// Avoid global.name
				if (name == "global")
					name = elem.@name.toString();
				// Avoid Array.Array etc
				else if (name != elem.@name)
					elemName = elem.@name,
					name += "." + elem.@name;
				if (elem.localName() == "method")
				{
					var args = "(";
					var params = elem.parameters.children();
					for (var j = 0; j < params.length(); j++)
					{
						if (j)
							args += ", ";
						args += params [j].@name;
					}
					args += ")";
					if (args.length > 2)
						name += " ";
					name += args;
				}
			}
			var href = "#/" + clsName + '/' + grpName + '/' + elemName;
			var help = this.flatten (elem.shortdesc);
			if (help.length)
				name += ": " + help;
			returnArray.push (name);
			this.lastFindInfo.hrefs [name] = href;
		}
		returnArray.sort();
		if (count > 20)
			returnArray.push ("...");
	}

	return returnArray;
}

// Update any UI with the code hint selected. The given text
// is the original text of the array element that was returned.

OMVData.displaySelectedCodeHint = function (href, text)
{
	if (OMVData.lastFindInfo.href != href)
		return;

	if (!href)
		href = OMVData.lastFindInfo.hrefs [text];
	if (href && OMV.ui)
		OMV.ui.select (href);
}

// Flatten an XML text to a string.

OMVData.flatten = function (xml)
{
	var s = "";
	xml = xml.children();
	for (var i = 0; i < xml.length(); i++)
	{
		if (s.length)
			s += " ";
		if (xml[i].nodeKind() == "text")
		{
			var text = xml[i].toString();
			// Replace &#160; with ' '
			text = text.replace (/\xA0/g, " ");
			s += text;
		}
		else
			s += this.flatten (xml [i]);
	}
	return s;
}

//////////////////////////////////////////////////////////////////////////

// Check the time stamp in a loaded TOC from temp XML and a running dynamic target.
// The XML is assumed to have been loaded from a temp file already.
// If the XML needs to be re-mined, do so immediately without asking.
// The progress bar is set to 20%.

OMVData.prototype._mineTargetIfNewer = function()
{
	docMgr.incProgress (20);

	var target = targetMgr.findTargetSpecifier( this.btID );

	if( this.dynamic && target && app.targetAppRunning( target ) )
	{
		var map = this._getDictsOrTOC( false );
		if (!map)
			return false;

		var oldStamp = this.xml ? this.xml.map.@time.toString() : "";
		var newStamp = map.@time.toString();
		if (newStamp.length)
		{
			// Always re-mine if the dynamic TOC had a stamp, but the loaded XML had not
			var remove = true;
			if (oldStamp.length)
			{
				var oldDate = new Date (oldStamp);
				var newDate = new Date (newStamp);
				remove = (newDate > oldDate);
			}
			if (remove)
			{
				File (this.file).remove();

				// Mine the target. 

				this.xml = null;

				// Try to load the entire dictionary in one chunk
				var xml = this.getClassInfo( null, null, true);
				if (xml)
				{
					// good load
					this.xml = xml;
					docMgr.incProgress (40);
				}
				else
				{
					this._badXML ("App does not support full getClassInfo");
					return false;
				}

				var title = this.xml.map.@title.toString();
				if (title == "")
				{
					title = this.getDisplayName();/* + " Object Model"*/;
					if( title.indexOf( "Object Model" ) < 0 && title.indexOf( "Object Library" ) < 0 && title.indexOf( "Type Library" ) < 0 )
						title += " Object Model";
					this.xml.map.@title = title;
				}
				this.loaded = true;

				// Save the XML to the prefs area.
				var f = File (this.file);
				if (f.open ("w"))
				{
					docMgr.setProgressText ('$$$/ESToolkit/OMV/Saving=Saving Object Model...');
					// stamp the time if not given
					if (this.xml.map.@time.toString() == "")
						this.xml.map.@time = new Date().toString();
					f.encoding = "UTF-8";
					f.write (this.xml.toXMLString());
					f.close();
					if (f.error != "")
					{
						f.remove();
						this._badXML ("Unable to write XML file");
						return false;
					}
					else
						this.file = f;
				}
			}
		}
	}
	return true;
}

// Mine all OMVs of that target. The "this" OMVData instance will become the 1st instance,
// and other instances will be inserted after this instance. If needed, launch the target.

OMVData.prototype._mineAllOMVs = function()
{
	var name = BridgeTalk.getDisplayName (this.btID);

	try
	{
		var dfltOMV = this;
		var omvList = [];
		// get dicts, launch the app if needed
		var xml = this._getDictsOrTOC( true );
		if (!xml)
			return false;
		xml = xml.children();

		// Get the default; if there is none, use CS6 as default.
		var dfltDictName = OMVData.defaultDictName;
		for (var i = 0; i < xml.length(); i++)
		{
			if (xml [i].@default == "true")
			{
				dfltDictName = xml [i].toString();
				break;
			}
		}

		for (var i = 0; i < xml.length(); i++)
		{
			var dictName = xml [i].toString();

			// Ignore the empty dictionary
			if (dictName == "")
				continue;

			// Create the file name: ESTK prefs folder/omv$btid$dict[$dflt].xml
			var split = this.btID.split('-');
			var f = app.prefsFolder + "/omv$" + split [0] + "-" + split[1] + "$" + dictName;

			var dflt = (dictName == dfltDictName);
			if (dflt)
				f += "$dflt";
			f += ".xml";

			if (0 == omvList.length)
			{
				// re-use this as the first element
				this.init (this.btID, dictName, localize ("%1 (%2)", name, dictName) + " Object Model", f, dflt, true);
				omvList.push (this);
			}
			else
			{
				var data = new OMVData (this.btID, dictName, localize ("%1 (%2)", name, dictName) + " Object Model", f, dflt, true);
				// sort that new OMV into the correct location of the OMVList array
				var found = false;
				for (var j = 0; j < omvList.length; j++)
				{
					if (data.title < omvList[j].title)
					{
						omvList.splice (j, 0, data);
						found = true;
						break;
					}
				}
				if (!found)
				{
					omvList.push (data);
				}
				if (dflt)
					dfltOMV = data;
			}
		}
		if (0 == omvList.length)
		{
			// it seems that there was only a single empty dictionary
			var split = this.btID.split('-');
			var f = app.prefsFolder + "/omv$" + split [0] + "-" + split[1] + "$" + dictName + ".xml";
			this.init (this.btID, "", name, f, true, true);
			omvList.push (this);
		}

		function sortfn (a, b)
		{
			if (a.title < b.title)
				return 1;
			else if (a.title > b.title)
				return -1;
			else
				return 0;
		}
		omvList.sort(sortfn);

		// At this point, omvList has a list of all OMVs to be mined.
		// Need to insert the sorted list into OMV.all at the position of the 1st entry.
		for (i = 0; i < OMVData.all.length; i++)
		{
			if (OMVData.all [i] == this)
			{
				var temp = [i, 1];
				temp = temp.concat (omvList);
				OMVData.all.splice.apply (OMVData.all, temp);
				break;
			}
		}
		docMgr.showProgress( true );
		docMgr.setProgress (0, 100 * omvList.length);
		docMgr.maxProgress = 0;

		for (var i = 0; i < omvList.length; i++)
		{
			if (!(i % 50))
				app.pumpEventLoop();

			// for the progress indicator in _prepareXML()
			docMgr.maxProgress += 100;
			var msg = localize ("$$$/ESToolkit/OMV/LoadingObjectModel=Loading Object Model for %1...", omvList[i].dict);
			docMgr.setProgressText (msg);
			if (!omvList[i]._loadXML())
			{
				docMgr.showProgress( false );
				if( OMV.ui )
					OMV.ui.activate();
				// very important: remove all mined files to tell the ESTK
				// to re-mine the target the next time
				for (var j = 0; j < omvList.length; j++)
				{
					omvList[j].mined = false;
					omvList[j].xml = null;
					File (omvList[j].file).remove();
				}
				return false;
			}
			else
			{
				omvList[i].mined = true;
				docMgr.setProgress (docMgr.maxProgress);
			}
		}

		delete this.maxProgress;

		// we are done - please refresh the list of OMVs, and select the default OMV
		docMgr.showProgress( false );
		if (OMV.ui)
		{
			OMV.ui.activate();	
			OMVData.util._loadOMVsIntoUI();
			// select the default OMV (or this OMV)
			// using a delayed task, because selecting an OMV might 
			// preempt the previous call to setContent (external interface call)
			addDelayedTask ( OMV.ui, OMV.ui.select, dfltOMV.href );
		}
		return true;
	}
	catch (e)
	{
		this._badXML ("Could not mine target");
		return false;
	}
}

// Retrieve either the TOC of a dynamic target, or the list of dictionaries.
// Launch the target is needed, and wait for up to three minutes, resending
// the job, until the target responds (if not launched)

OMVData.prototype._getDictsOrTOC = function (dicts, noErrorMsg)
{
	var xml = null;
	var job, res;

    var target = targetMgr.findTargetSpecifier( this.btID );
	if (!target)
		return null;

	if( app.targetAppRunning( target ) )
	{
		// send a single request to the target
		job = dicts 
			? target.cdic.getDictionaries (target.address)
			: target.cdic.getDictionaryTOC( target.address, this.dict, "" );
		res = cdicMgr.callSynchronous( job, 300000 );
		if( res.length == 1 )
		{
			try
			{
				xml = new XML( res[0][0] );
			}
			catch( exc )
			{
				xml = null;
			}
		}
	}
	else
	{
		// need to launch and wait
        var launchMsg = "$$$/ESToolkit/OMV/Launch=%1 must be running to retrieve Object Model data.^nDo you want to launch %1?";
        if( !app.launchTargetAppSynchronous( target, launchMsg, true ) )
	        return false;

		// do 180 attempts to execute the command
		// one each second
		for (var i = 0; i < 180; i++)
		{
			job = dicts 
				? target.cdic.getDictionaries (target.address)
				: target.cdic.getDictionaryTOC( target.address, this.dict, "" );
			res = cdicMgr.callSynchronous( job, 300000 );
			if( res.length == 1 )
			{
				try
				{
					xml = new XML( res[0][0] );
				}
				catch( exc )
				{
					xml = null;
				}

				break;
			}
			$.sleep (1000);
		}
	}
    if( !xml && !noErrorMsg )
        this._badXML ("App did not return valid XML");
	return xml;
}

//////////////////////////////////////////////////////////////
// Utility functions
//////////////////////////////////////////////////////////////

// Extract the OMV title from a file by reading the 1st 1024 characters
// and using a RegExp to find the title. Return null if there is no title.

OMVData.util._getOMVTitleFromFile = function (f)
{
	var resolved = f.resolve();
	if (resolved)
		f = resolved;
	// Open this file, and read the first 1024 bytes to find the <map> element
	if (!f.open())
		// IO error - ignore
		return null;

	var s = f.read (1024);
	f.close();
	var re = /<map .*title="(.+?)"/;
	title = re.exec (s);
	if (title)
		title = title [1];
	return title;
}

// Load all OMVs into the UI.

OMVData.util._loadOMVsIntoUI = function (href)
{
	var xml = <omvs/>;
	for (var i = 0; i < OMVData.all.length; i++)
	{
		var omv = OMVData.all [i];
		var entry = <omv title={omv.title} href={omv.href}/>;
		xml.appendChild (entry);
	}
	// Load the list of OMVs into the UI
	OMV.ui.setContent (xml);
}

OMVData.util.createHRef = function( btID, dictName )
{
	var parts = btID.split( '-' );
	var href  = parts[0];

	if( parts.length > 1 )
		href += '-' + parts[1];

	if( dictName )
		href += '/' + dictName;

	href += '#';

	return href;
}