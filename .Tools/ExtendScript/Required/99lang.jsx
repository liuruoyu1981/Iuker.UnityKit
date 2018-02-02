/**************************************************************************
*
*  @@@BUILDINFO@@@ 99lang-2.jsx 3.0.0.14  27-February-2008
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

// The language definitions are in syntaxdefs.xml.

const lang = {
	fileExts: {},	// name: extension, value: language ID
	langIDs:  []	// a sorted array of all language IDs (syntaxdef.xml is sorted!)
};

//
// all languages preferences
//
var languages = new XML;

//
// all languages defaults
//
var languagesDefaults = null;

//
// all default fonts, styles, and colors
//
var defaults = new XML;

// set up the language data.

lang.initialize = function()
{
	// load the XML
	var xml = prefs.syntaxdefs.getXMLNode();
	// build these two objects
	this.fileExts = {};
	this.langIDs  = [];
	languages = xml.languages.children();
	defaults  = xml.xpath ('defaults[@os="' + Folder.fs + '"]');
	for (var i = 0; i < languages.length(); i++)
		this.langIDs.push (languages [i].localName());

	// get the XML object instead of the children
	languages = xml.languages;
	
	var dfltStyles = defaults.style;
	
	for (i = 0; i < this.langIDs.length; i++)
	{
		var langID  = this.langIDs [i];
		var langObj = languages [langID];
		var lexerRef = langObj.lexer.toString() == 'hypertext' ? 'html' : langObj.lexer.toString();
		// If the language does not carry a style, set the original lexer's style
		if (!langObj.style.length())
			langObj.style = languages [lexerRef].style;
		// If the language does not carry keywords, set the original lexer's keywords
		if (!langObj.keywords.length())
			langObj.keywords = languages [lexerRef].keywords;
		// If the language does not carry word chars, set the original lexer's word chars
		if (!langObj.wordChars.length())
			langObj.wordChars = languages [lexerRef].wordChars;
		
		// merge the default styles with language styles
		for (var n in dfltStyles)
		{
			if (n == "length")
				//MEF: ignore 'length' property - this is a temp BUG fix till Bernd can investigate
				continue;
			var index = dfltStyles[n].@index;
			// for text, just add Line Numbers and Control Characters
			if (langID == "text" && index != 33 && index != 36)
				continue;
			if (!langObj.xpath("style[@index=" + index + "]").length())
				langObj.style += dfltStyles [n];
		}
		// add the list of file types
		var list = langObj.fileExts.toString().split (';');
		for (var j = 0; j < list.length; j++)
		{
			var ext = list [j];
			if (ext[0] == '*' && ext[1] == '.')
				this.fileExts [ext.substr(2)] = langID;
		}
	}
	
	//
	// import old scintilla preference file
	//
	if( startupPrefsLoaded )
	{
	    var oldPrefs = lang.importScintillaPrefFile();
    	
	    if( oldPrefs )
	    {
            languages = oldPrefs.languages.copy();	
		    prefs.syntaxdefs.languages.setXMLNode( languages.copy() );
	    }
	}
}

// Build a list of file types for open/save dialogs

lang.buildFileTypes = function (optLangID)
{
	var types = "";
	var text = "";
	var exts = "";
	if (!optLangID)
	{
		// start with JavaScript files on Open
		text = languages.js.fileFilter.toString();
		exts = languages.js.fileExts.toString();
		types += localize ("$$$/ESToolkit/FileDlg/FileTypes=%1 files", text);
		types += ':';
		types += exts;
		// add All Files here
		types += "," + localize ("$$$/ESToolkit/FileDlg/AllTypes=All files:*.*");
	}
	
	for (i = 0; i < this.langIDs.length; i++)
	{
		var langID = this.langIDs [i];
		if (langID == "js" && !optLangID)
			continue;	// done already
		// if we have a language ID, just do this language
		if (optLangID && langID != optLangID)
			continue;
		text = languages [langID].fileFilter.toString();
		exts = languages [langID].fileExts.toString();
		if (text && exts)
		{
			if (types.length)
				types += ',';
			types += localize ("$$$/ESToolkit/FileDlg/FileTypes=%1 files", text);
			types += ':';
			types += exts;
		}
	}
	// add All Files here in save mode
	if (optLangID)
		types += "," + localize ("$$$/ESToolkit/FileDlg/AllTypes=All files:*.*");

	return types;
}

// Return the correct language for the given file.
// If there is no file type match, ask for the correct language.
// Return null if the user pressed Cancel at this point.

lang.getLanguageForFile = function (f)
{
	var langID = null;
	var name = f.name.split ('.');
	if (name.length >= 2)
	{
		var ext = name[name.length-1];
		ext = ext.toLowerCase();
		langID = this.fileExts [ext];
	}

	if (!langID)
	{	
		langID = null;
		
		// unknown file type: create dialog
		var d = new Window (
		 "prefdialog {																\
			text:'$$$/ESToolkit/Dialogs/Language/Title=Please select a language',\
			list: Group { orientation: 'row',									\
				list: ListBox { preferredSize: [200, 200] }						\
			},																	\
			buttons: Group { orientation: 'row',								\
			  okBtn: Button { text:'$$$/CT/ExtendScript/UI/OK=&OK', properties:{name:'ok'} },				\
			  cancelBtn: Button { text:'$$$/CT/ExtendScript/UI/Cancel=&Cancel', properties:{name:'cancel'} }	\
			}																	\
		 }");
		d.buttons.okBtn.onClick = function()
		{
			var dlg = this.parent.parent;
			dlg.langID = dlg.list.list.selection.langID;
			dlg.close (1);
		}
		d.buttons.cancelBtn.onClick = function()
		{
			var dlg = this.parent.parent;
			dlg.langID = null;
			dlg.close (0);
		}
		
		var textItem = null;
		for (var i = 0; i < this.langIDs.length; i++)
		{
			var curID = this.langIDs [i];
			var item = d.list.list.add ("item", languages [curID].fileFilter);
			item.langID = curID;
			if (curID == "text")
				textItem = item;
		}
		if (textItem)
			d.list.list.selection = textItem;
		d.center();
		if (d.show())
			langID = d.langID;
	}
	return langID;
}

// Get the lexer and styles for a given language ID.

lang.getLexerAndStyles = function (langID)
{
	return languages [langID];
}

// Create and return a JS object containing properties as strings:
// index - the index
// name - font name
// size - font size
// back - background color (number)
// fore - foreground color (number)
// backName = background color (color name)
// foreName = foreground color (color name)
// bold - true if bold
// italics - true if italics

lang.getStyleValues = function (xmlStyle)
{
	function getColor (s)
	{
		s = s.toString();
		if (s[0]>= 'A' && s[0] <= 'Z')
			return colors [s] ? colors [s] : 0;
		else
			return Number (s);
	}
	var obj = { index: 0, name: "", size: 0, back: 0xFFFFFF, fore: 0, backName:"White", foreName:"Black" };
	obj.index = Number (xmlStyle.@index);

	// set up the defaults for the font
	var xml = defaults.xpath('font [@id="base"]');
	obj.name = xml.@name.toString();
	obj.size = Number (xml.@size);
	
	var id = xmlStyle.@font.toString();
	if (id)
	{
		var xml = defaults.xpath ('font[@id = "' + id + '"]');
		if (xml.length())
		{
			obj.name = xml.@name.toString();
			obj.size = Number (xml.@size.toString());
		}
	}
	// name and size, if given, override the font reference
	var s = xmlStyle.@name.toString();
	if (s)
		obj.name = s;
	s = xmlStyle.@size.toString();
	if (s)
		obj.size = Number (s);
		
	// same game for color, fore and back
	id = xmlStyle.@color.toString();
	if (id)
	{
		var xml = defaults.xpath ('color[@id="' + id + '"]');
		if (xml.length())
		{
			s = xml.@fore.toString();
			if (s)
			{
				obj.fore = getColor (s);
				obj.foreName = s;
			}
			s = xml.@back.toString();
			if (s)
			{
				obj.back = getColor (s);
				obj.backName = s;
			}
		}
	}

	// fore and back, if given, override the color reference
	s = xmlStyle.@fore.toString();
	if (s)
	{
		obj.fore = getColor (s);
		obj.foreName = s;
	}
	s = xmlStyle.@back.toString();
	if (s)
	{
		obj.back = getColor (s);
		obj.backName = s;
	}
	// bold and italics
	obj.bold = (xmlStyle.@bold == "true");
	obj.italics = (xmlStyle.@italics == "true");
/*
s="";
for (var i in obj)
	s += i + ": " + obj[i] + "\n";
alert (s);
*/	return obj;
}

//
// extract file extensions from lang info
//
lang.getFileExtensions = function( langID )
{
    var obj      = this.getLexerAndStyles( langID );	        
    var fileExts = obj.fileExts.toString().split(';');
    
    for( var i=0; i<fileExts.length; i++ )
    {
        var pos = fileExts[i].indexOf( '*.' );
        
        if( pos >= 0 )
            fileExts[i] = fileExts[i].substring( pos+2 );
    }
    
    return fileExts;
}

lang.getLanguagesDefaults = function()
{
    if( languagesDefaults == null )
    {
	    // load the XML
	    var xml = prefs.syntaxdefs.getDefaultXMLNode();
	    languagesDefaults = xml.languages.children();
	    var defaults  = xml.xpath ('defaults[@os="' + Folder.fs + '"]');

	    var fileExts = {};
	    var langIDs  = [];
	    for (var i = 0; i < languagesDefaults.length(); i++)
		    langIDs.push (languagesDefaults [i].localName());

	    // get the XML object instead of the children
	    languagesDefaults = xml.languages;
    	
	    var dfltStyles = defaults.style;
    	
	    for (i = 0; i < this.langIDs.length; i++)
	    {
		    var langID  = langIDs [i];
		    var langObj = languagesDefaults [langID];
		    var lexerRef = langObj.lexer.toString() == 'hypertext' ? 'html' : langObj.lexer.toString();
    		
		    // If the language does not carry a style, set the original lexer's style
		    if (!langObj.style.length())
			    langObj.style = languagesDefaults [lexerRef].style;
		    // If the language does not carry keywords, set the original lexer's keywords
		    if (!langObj.keywords.length())
			    langObj.keywords = languagesDefaults [lexerRef].keywords;
		    // If the language does not carry word chars, set the original lexer's word chars
		    if (!langObj.wordChars.length())
			    langObj.wordChars = languagesDefaults [lexerRef].wordChars;
    		
		    // merge the default styles with language styles
		    for (var n in dfltStyles)
		    {
			    if (n == "length")
				    //MEF: ignore 'length' property - this is a temp BUG fix till Bernd can investigate
				    continue;
			    var index = dfltStyles[n].@index;
			    if (!langObj.xpath("style[@index=" + index + "]").length())
				    langObj.style += dfltStyles [n];
		    }
		    // add the list of file types
		    var list = langObj.fileExts.toString().split (';');
		    for (var j = 0; j < list.length; j++)
		    {
			    var ext = list [j];
			    if (ext[0] == '*' && ext[1] == '.')
				    fileExts [ext.substr(2)] = langID;
		    }
	    }
    }
    
    return languagesDefaults;
}

//
// load ol scintilla preference file
// and delete the file afterwards
//
lang.importScintillaPrefFile = function()
{
    var xml = null;

    if( !prefs.sciprefsimported.getValue( Preference.BOOLEAN ) )
    {    
        var folder = app.prefsFolder.parent.absoluteURI + "/2.0";
        var f = new File( folder + "/scintilla.xml" );
    	
        if( f.exists && f.open ("r") )
        {
	        f.encoding = "UTF-8";

			try
			{
				xml = new XML( f.read() );
				xml.languages.js.lexer = 'js';
				xml.languages.js.style += <style title="Tripple Double quoted string" index="20" color="string"/>;
				xml.languages.js.style += <style title="Tripple Single quoted string" index="21" fore="Purple"/>;
				xml.defaults.languages.js.lexer = 'js';
				xml.defaults.languages.js.style += <style title="Tripple Double quoted string" index="20" color="string"/>;
				xml.defaults.languages.js.style += <style title="Tripple Single quoted string" index="21" fore="Purple"/>;
			}
			catch( exc )
			{}

	        f.close();

	        prefs.sciprefsimported = true;
        }
    }    
    
    return xml;
}

