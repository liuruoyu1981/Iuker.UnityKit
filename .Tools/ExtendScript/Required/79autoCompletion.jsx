/**************************************************************************
*
*  @@@BUILDINFO@@@ 79autoCompletion-2.jsx 3.5.0.22	28-April-2009
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

/*  Globals that our sort fn can access, these are 
    reset everytime suggestions are made to the user.
*/
var _token         = "";
var _caseSensitive = true;

function AutoCompletion (userFile)
{
	this.omvHref = "$COMMON/javascript#";

	this.loadingHref = "";

	this.userItemsGlobal = [];
	this.userItemsObject = [];
	
	this.keywords = [];
	
	this.functionsGlobal = [];
	this.functionsObject = [];
}

AutoCompletion.prototype.getHref = function()
{
	return this.omvHref;
}

AutoCompletion.prototype.setHref = function (href)
{
	if( href != this.loadingHref )
	{
		this.loadingHref = href;

		// do not remove - this also loads the OMV
		var data = OMVData.find (href);
		// should be a valid href of the form target[/session]#
		// but null is legal - maybe the load was aborted
		if (data)
			this.omvHref = data.href;

		this.loadingHref = "";
	}
}

AutoCompletion.prototype.setTargetAndSession = function (target, session)
{
	if( target )
	{
		var parts = target.address.target.split("-");
		var href = parts[0] + ( parts.length > 1 ? ( "-" + parts[1] ) : "" );

		if (href == "estoolkit")
			href = "$COMMON/javascript";
		else if (session)
			href += '/' + session.address.engine;
		href += '#';
		this.setHref (href); 
	}
}

/*	suggest (...)
		Returns a list of suggestions based on the current 
		sources of data stored by the AutoCompletion class.
*/
AutoCompletion.prototype.suggest = function (token, global)
{
	if (!token || token.length == 0)
		return;

	var tokens = token.split ('.', 2);
	var tokenWithoutClass = token;
	if (tokens.length > 1)
		tokenWithoutClass = tokens[1];

    /*  We only want to be case sensitive if the token is mixed case.  If the
        token is all lower or all upper case, then search case insensitive
    */
    var caseSensitive = true;
    if (tokenWithoutClass.toLowerCase() == tokenWithoutClass || tokenWithoutClass.toUpperCase() == tokenWithoutClass)
        caseSensitive = false;

    //  These are for use with our sort function (it can't access member variables)
    _token         = tokenWithoutClass;
    _caseSensitive = caseSensitive;

    /*  Searching while true==global means that we want methods and properties
        that do not belong to any class.  
    */
    var keywordSuggestions = this.searchKeywords (tokenWithoutClass, global, caseSensitive);
	var functionSuggestions = this.searchFunctionList (tokenWithoutClass, global, caseSensitive);
	var userSuggestions = this.searchUserItems (tokenWithoutClass, global, caseSensitive);
	var omvSuggestions = this.searchOmv (token, global, caseSensitive);
	
	/*  OMV suggestions will sometimes return with a "...".  If that's the case, 
	    we don't want to sort with that in there.  So pop it now, and push it
	    at the end
	*/
	var lastVal;
	if (omvSuggestions[omvSuggestions.length - 1] == "...")
	    lastVal = omvSuggestions.pop();
	    
	var retVal = this.mergeSearchResults (keywordSuggestions, functionSuggestions, userSuggestions, omvSuggestions);
	
	if (undefined != lastVal)
	    retVal.push(lastVal);
	    
	return retVal;
	 
}

/********************************************************************
	Setting the data sources
********************************************************************/

/* setUserItems (...)
		Takes a userFile and creates two arrays: 
		1. global related
		2. object related
*/
AutoCompletion.prototype.setUserItems = function (userFile)
{
	this.userItemsGlobal = [];
	this.userItemsObject = [];
}

/*	setKeywords (...)
		Takes an array of groups of keywords that are associated with the 
		current syntax highlighting scheme and stores them.
*/
AutoCompletion.prototype.setKeywords = function (groupList)
{
	this.keywords = [];
	
	for (var i = 0; i < groupList.length; i++)
	{
	    if( groupList[i] )
	    {
		    var keywords = groupList[i].split (' ');
		    for (var j = 0; j < keywords.length; j++)
		    {
			    this.keywords.push (keywords[j]);
		    }
		}
	}
	
	this.keywords.sort (this.sortDataSource);
}

/*	setDocFunctions (...)
		Takes an array of FunctionData objects that corresponds to the 
		current active document.  Then splits the functions as either 
		global or object related.
*/
AutoCompletion.prototype.setDocFunctions = function (functionList)
{
	this.functionsGlobal = [];
	this.functionsObject = [];
	
	for (var i = 0; i < functionList.length; i++)
	{
		var fn = functionList[i];
		var fnHeader = fn.nameLong + " (";
		
		//	Add the parameters, e.g. param1, param2, param3
		for (var j = 0; j < fn.parameters.length; j++)
		{
			fnHeader += fn.parameters[j].name;
			if (j != fn.parameters.length-1)
				fnHeader += ", ";	
		}
		fnHeader += ")";
		
		//	Anything with a "." is an object function, else - global
		if (fn.nameLong.search (/\./g) >= 0)
			this.functionsObject.push (fnHeader);
		else
			this.functionsGlobal.push (fnHeader);
	}
	
	this.functionsGlobal.sort (this.sortDataSource);
	this.functionsObject.sort (this.sortDataSource);
}

/*	sortDataSource (...)
		Helper function used to sort String objects
		in an array alphabetically
*/
AutoCompletion.prototype.sortDataSource = function (a, b)
{
	var string1 = a.toLowerCase();
	var string2 = b.toLowerCase();
	
	if (a < b)
		return -1;
	else if (a > b)
		return 1;
	
	return 0
}

/********************************************************************
	Search/Merge the data sources
********************************************************************/

/*	mergeSearchResults (...)
		Expects multiple arrugments of Arrays.  Output is a single
		array that is sorted.
*/
AutoCompletion.prototype.mergeSearchResults = function ()
{
	if (arguments.length == 1)
		return arguments[0];
	
	var results = arguments[0];	
	for (var i = 1; i < arguments.length; i++)
	{
	    if (undefined != arguments[i])
		    results = results.concat (arguments[i]);
	}
	
	results.sort(this.sort);
	
	return results;
}

/*	sort (...)
		The sort fn for the array of suggestions. If the search 
		is case sensitive, then just perform an alphabetical 
		sort.  If not, then try to match with whatever case the
		user used.
*/
AutoCompletion.prototype.sort = function (a, b)
{
    /*  First, if classs name and description are present,
        split then off to get the element name.  We really
        want to sort by that only.
    */
	var aFullText  = a.split(": ")[0];
    var aClassText = aFullText.split('.');
    var a2         = aClassText.length > 0 ? aClassText[aClassText.length-1] : '';
    
    var bFullText  = b.split(": ")[0];
    var bClassText = bFullText.split('.');
    var b2         = bClassText.length > 0 ? bClassText[bClassText.length-1] : '';
	    
	if (_caseSensitive)
	    return _alphaSort (a2, b2);
	else
	{   
	    var a2_match = _token == a2.substring (0, _token.length);
	    var b2_match = _token == b2.substring (0, _token.length);
	    
	    if (a2_match && !b2_match)
            return -1;
        else if (!a2_match && b2_match)
            return 1;
        else // if (a2_match && b2_match || !a2_match && !b2_match)
            return _alphaSort(a2, b2);
	}
}

/*	alphaSort (...)
		generic alphabetical sort fn for use
		with Array.sort(sortFn)
*/
function _alphaSort (a, b)
{
	if (a < b)
    	return -1;
	else if (a > b)
		return 1;
	else
	    return 0;
}

/*	searchUserItems(...)
		Search the user defined items for any matches
		with the given token.  Returns an array of strings.
*/
AutoCompletion.prototype.searchUserItems = function (token, global, caseSensitive)
{
	if (!token || 0 == token.length)
		return [];
		
    //  ** NOT YET IMPLEMENTED **
}

/*	searchKeywords (...)
		Search the keywords for any matches with the
		given token. (case insensitive)
		Returns an array of strings.
*/
AutoCompletion.prototype.searchKeywords = function (token, global, caseSensitive)
{
    /*  All keywords are global, so if global is false, 
        also return an empty array
    */
	if (!token || 0 == token.length || (undefined != global && false == global) )
		return [];

	var suggestions = [];
	var localToken = caseSensitive ? token : token.toLowerCase();
	
	for (var i = 0; i < this.keywords.length; i++)
	{
		if( localToken.length < this.keywords[i].length )
		{
			var currentKw = this.keywords[i].substring (0, token.length);
			if (false == caseSensitive)
				currentKw = currentKw.toLowerCase();
				
			if (currentKw == localToken)
				suggestions.push (this.keywords[i]);
		}
	}
	
	return suggestions;
}

/*	searchFunctionList (...)
		Search the document's function list for any matches
		with the given token.  (case insensitive)
		Returns an array of strings.
*/
AutoCompletion.prototype.searchFunctionList = function (token, global, caseSensitive)
{	
	if (!token || 0 == token.length)
		return [];

	var suggestions = [];
	var localToken = caseSensitive ? token : token.toLowerCase();
	
	if (global)
	{
		for (var i = 0; i < this.functionsGlobal.length; i++)
		{
			if( localToken.length < this.functionsGlobal[i].length )
			{
				var currentFn = this.functionsGlobal[i].substring (0, token.length);
				if (false == caseSensitive)
					currentFn = currentFn.toLowerCase();
				
				if (currentFn == localToken)
					suggestions.push (this.functionsGlobal[i]);
			}
		}
	}
	else
	{
		for (var i = 0; i < this.functionsObject.length; i++)
		{
			/*	Try to match with the short name of the function.
				That is, the last identifier.  e.g: with ident1.ident2.ident3
				we would want to match against ident3
			*/
			var regex = /([$_a-zA-Z](?:[$_a-zA-Z0-9])*)\s/g;
			var match;
			var fnName;
			if ((match = regex.exec (this.functionsObject[i])) != null)
			{
				fnName = match[1];
			}
			
			if( localToken.length < fnName.length )
			{
				var currentFn = fnName.substring (0, token.length)
				if (false == caseSensitive)
					currentFn = currentFn.toLowerCase();
				
				if (currentFn == localToken)
					suggestions.push (this.functionsObject[i]);
			}
		}
	}
	
	return suggestions;
}

/*	searchOmv (...)
		Search the current dictionary for any matches
		with the given token.  Returns an array of strings.
*/
AutoCompletion.prototype.searchOmv = function (token, global, caseSensitive)
{
	if (!token || 0 == token.length)
		return [];

    var flags = 0;
    if (global)
        flags |= OMVData.SEARCH_GLOBAL;
    else
		flags |= OMVData.SEARCH_NO_GLOBAL;
    
    //  Default search is case insensitive
    if (caseSensitive == true)
        flags |= OMVData.SEARCH_CASE_SENSITIVE;
		
	var res = OMVData.getCodeHints (this.omvHref, token, flags);

	if( !res )
		res = [];
		
	var ret = [];

	for( var i=0; i<res.length; i++ )
	{
		var fullText	= res[i].split(": ")[0];
		var classText   = fullText.split('.');
		var elementText = classText.length > 0 ? classText[classText.length-1] : '';
		
		if( token.length < elementText.length || fullText.indexOf( token ) == 0 )
			ret.push( res[i] );
			
	}
	
	return ret;
}
