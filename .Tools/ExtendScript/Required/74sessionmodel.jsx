/**************************************************************************
*
*  @@@BUILDINFO@@@ 74sessionmodel-2.jsx 3.5.0.48	14-December-2009
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

/**************************************************************************

Session Variable

This is the data structure behind a displayed item.

**************************************************************************/

function SessionVariable( parent, xml )
{
	if (xml)
	{
		this.name      = xml.@name.toString();						// the name
		this.value	   = xml.toString();							// the value
		this.type      = xml.@type.toString();						// the data type
		this.invalid   = xml.@invalid == "true";					// invalid?
		this.readonly  = xml.@readonly == "true";					// read only?
	}
	else
	{
		this.name      = "$.global";			// the name
		this.value     = "";					// the value
		this.type      = "";					// the data type
		this.invalid   = false;					// invalid?
		this.readonly  = false;					// read only?
	}
    this.parent    = parent ? parent : null;    // parent variable
	this.sortName  = this.name.toUpperCase();	// caseless sort
	this.children  = {};						// kids
	this.modified  = true;						// did the value change?
    this.expanded  = false;                     // is the treewiev item expanded?
	this.isObject  = false;						// true if this is an expandable object
	this.touched   = true;						// this variable has been touched during rebuild
	if (parent)
		// prepend the name with a '#' to avoid confusion with built-in JS elements
		parent.children ['#' + this.name] = this;
}

SessionVariable.prototype.erase = function()
{
	this.children  = {};                                
    this.expanded  = false;
}

SessionVariable.prototype.findChild = function (name)
{
	return this.children ['#' + name];
}

SessionVariable.prototype.remove = function()
{
	this.erase();
	if (this.parent)
		delete this.parent.children ['#' + this.name];
}

SessionVariable.prototype.toString = function()
{
	// debugging support
	return "[SessionVariable " + this.name + "]";
}

SessionVariable._sortfn = function (a, b)
{
    a = a.sortName;
    b = b.sortName;
	var na = Number (a);
	var nb = Number (b);
	// numeric index sort?
	if (!isNaN (na) && !isNaN (nb))
		return na - nb;
	// alphabetic sort
    if (a < b)
        return -1;
    else if (a > b)
        return 1;
    else
        return 0;
}

// return an array of sorted children

SessionVariable.prototype.sortChildren = function()
{
	// create a copy
	var arr = [];
	for each (var v in this.children)
		arr.push (v);
	arr.sort( SessionVariable._sortfn );
	return arr;
}

// make a path for the DataBrowser
// this is a slash-separated list of names, starting with $.global
// for local scopes, the first element is the local variable name

SessionVariable.prototype.makePath = function()
{
	var obj = this;

	// build the path to update for the DataBrowser
	var path = [];
	while (obj.parent)
	{
		path.unshift (obj.name);
		obj = obj.parent;
	}
	// Add the top-level object if needed (not needed for local scopes)
//	if (obj.name == "$.global")
		path.unshift (obj.name);

	return path.join ('/');
}

/**********************************************************************

Session Model

**********************************************************************/

function SessionModel( session )
{
    this.session			= session;
    this.variables			= new SessionVariable();
    this.baseScope			= '$.global';               // global scope
	this.pendingJobs		= 0;						// the number of jobs
	this.pendingJobsByID	= {};						// pending jobs by ID=instance
	this.pendingTasksByPath	= {};						// pending delayd tasks by path
}

//-----------------------------------------------------------------------------
// 
// setBaseScope(...)
// 
// Purpose: Set base scope
// 
//-----------------------------------------------------------------------------

SessionModel.prototype.setBaseScope = function( isGlobal )
{
	this.cancelUpdate();
    this.baseScope = ( isGlobal ? '$.global' : '$.local' );
	this.variables.name = this.baseScope;
}

SessionModel.prototype.getBaseScope = function()
{
    return this.baseScope;
}

//-----------------------------------------------------------------------------
// 
// setupModel(...)
// 
// Purpose: Remove existing model and get all variables
// 
//-----------------------------------------------------------------------------

SessionModel.prototype.setupModel = function()
{
    this.erase( true );
    this.updateModel( this.baseScope );
}

//-----------------------------------------------------------------------------
// 
// updateModel(...)
// 
// Purpose: Update the existing model
// 
//-----------------------------------------------------------------------------

SessionModel.prototype.updateModel = function( path )
{
    if( !this.session.target.getFeature( Feature.GET_VARIABLES, this.session ) )
        return;

	var scope = ( path ? path : this.baseScope );

	if( !this.pendingTasksByPath[scope] )
	{
		this.pendingTasksByPath[scope] = true;
		addDelayedTask( this, this.update, path, false );
	}
}

SessionModel.prototype.cancelUpdate = function()
{
	//
    // cancel any running updates
    //
	killDelayedTasks (this, this.update);
	this.pendingTasksByPath = {};

//	for each (var job in this.pendingJobsByID)
//		job.quitTask (true);
	this.pendingJobs = 0;
	this.pendingJobsByID = {};
}

//-----------------------------------------------------------------------------
// 
// erase(...)
// 
// Purpose: Remove existing model
// 
//-----------------------------------------------------------------------------

SessionModel.prototype.erase = function( quiet )
{
	this.cancelUpdate();
    this.variables  = new SessionVariable();

    if( !quiet )
    {
        DebugSession.notifyClients( 'startUpdateModel', this.session );
        DebugSession.notifyClients( 'endUpdateModel', this.session, true );
    }
}

//-----------------------------------------------------------------------------
// 
// update(...)
// 
// Purpose: Update model of variables
// This is called as a delayed task, or recursively
// 
//-----------------------------------------------------------------------------


SessionModel.prototype.update = function( path, forceUpdate )
{
	if( !prefs.databrowser.collectData.getValue( Preference.BOOLEAN ) )
		return;

	if (!this.session.initialized())
		return;
		
	if( this.session.releasing )
		return;

    var scope = this.baseScope;
    
    if( path )
        scope = path;

    if( scope )
    {
		delete this.pendingTasksByPath[scope];
        
		var maxArrayElements = prefs.databrowser.maxArrayElements.getValue( Preference.NUMBER );
        var excludes = 0;

        if( !prefs.databrowser.showUndefined.getValue( Preference.BOOLEAN ) )   
            excludes |= Variable.TYPE_UNDEFINED;
        if( !prefs.databrowser.showCore.getValue( Preference.BOOLEAN ) )         
            excludes |= Variable.TYPE_CORE;
        if( !prefs.databrowser.showFunctions.getValue( Preference.BOOLEAN ) )   
            excludes |= Variable.TYPE_FUNCTION;
        if( !prefs.databrowser.showPrototype.getValue( Preference.BOOLEAN ) )   
            excludes |= Variable.TYPE_PROTOTYPE;

		var job = null;
		
		try
		{
			job = this.session.sessionObj.getVariables( scope, excludes, maxArrayElements );
		}
		catch( exc )
		{}

		if( job )
		{
			job.model       = this;
			job.scope       = scope;
			job.forceUpdate	= forceUpdate;

			job.onResult = function()
			{
				if( this.model.pendingJobs && this.model.pendingJobsByID [this.id] )
				{
					this.model.pendingJobs--;
					delete this.model.pendingJobsByID [this.id];

					var parent = this.model.findVariable( this.scope );
					if( parent )
					{
						// true if the item is an object
						var isObject = false;
						/*
						The XML has this format:
						<properties object="object">
							<property name="name" type="type" [invalid="true" readonly="true"]>
								the value as string (possible multiple lines)
							</property>
							...
						<properties>
						type is one of undefined, null, boolean, number, string, error, or a class name
						*/
						var xml = this.result[0];
						// this array takes all expanded variables
						var expandedVars = [];
						var i = 0;

						for each (var element in xml)
						{
							// pump in between
							if (!(++i % 50))
								app.pumpEventLoop();

							// Fix the value for better display
							var value = element.toString();

							switch (element.@type.toString())
							{
								case "undefined":	
								case "null":		
								case "boolean":		
								case "number":		
								case "string":
								case "error":
									break;

								case "Function":
									isObject = true;
									// a function may be defined in several ways:
									// Function,foo,foo()   defined as function foo(){}
									// Function,foo,()      defined as foo = function()
									// Function,bar,foo()   defined as bar = foo
									if (value[0] == '(')
										element.replace (0, element.@name + value);
									break;

								default:
									isObject = true;
									// show the class if the value does not start with '['
									if( value == "undefined")
										value = '[' + element.@type + ']';
									else if( value.length && value[0] != '[' )
										value = '[' + element.@type + '] ' + value;
									element.replace (0, value);
							}

							// update existing pane with new or removed variables
							var varName = this.scope + '/' + element.@name;
							var sessionVar = this.model.findVariable( varName );
							if (!sessionVar)
								sessionVar = new SessionVariable( parent, element );
							else
							{
								sessionVar.modified	= ( value != sessionVar.value ) || ( element.@type.toString() != sessionVar.type );
								sessionVar.isObject	= isObject;
								sessionVar.type		= element.@type.toString();
								sessionVar.value	= value;
								sessionVar.touched	= true;
								sessionVar.invalid	= ( element.@invalid == "true" );					// invalid?
								sessionVar.readonly	= ( element.@readonly == "true" );					// read only?

								if( sessionVar.expanded )
									expandedVars.push (sessionVar);
							}
						}

						// detect and remove deleted variables
						for each (sessionVar in parent.children)
						{
							if (!sessionVar.touched)
								sessionVar.remove();
							else
								sessionVar.touched = false;
						}
						// finally, set off update requests (eventually forced) for expanded elements
						for (var j = 0; j < expandedVars.length; j++)
						{
							sessionVar = expandedVars [j];
							// may have been removed
							if (sessionVar.expanded)
								this.model.update( sessionVar.makePath(), forceUpdate );
						}
					}

					if (this.model.pendingJobs == 0)
						DebugSession.notifyClients( 'endUpdateModel', this.model.session, forceUpdate );
				}
		   }
		    
			job.onError = job.onTimeout = function()
			{
				if (this.model.pendingJobs)
				{
					this.model.pendingJobs--;
					delete this.model.pendingJobsByID [this.id];
					if (this.model.pendingJobs == 0)
						DebugSession.notifyClients( 'endUpdateModel', this.model.session, true );
				}
			}

			job.submit();

			// register the job for early quitting
			this.pendingJobsByID [job.id] = job;
			if (++this.pendingJobs == 1)
				DebugSession.notifyClients( 'startUpdateModel', this.session );
		}
    }
}

SessionModel.prototype.findVariable = function( path )
{
    var ret = null;

    var names = path.split( '/' );
	// local scope paths start with the first local variable
	if (this.baseScope == '$.local' && names[0] != this.baseScope)
		names.unshift (this.baseScope);

	// either $.global or $.local
    if( names[0] == this.baseScope )
    {
        ret = this.variables;
        
        for( var i=1; i<names.length; i++ )
		{
            ret = ret.findChild (names[i]);
			if (!ret)
				break;
		}
    }
    
    return ret;
}
