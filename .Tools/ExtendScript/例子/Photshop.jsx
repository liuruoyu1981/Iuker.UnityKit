var groupNames = new Array();	//	Psd文件中所有的图层数组
var groups = new Array();
var allLayerGroup = app.activeDocument.layerSets;

// 添加最外层的图层组
if(allLayerGroup.length > 0){
	for(var i = 0; i < allLayerGroup.length; i++)
	{
        var ap= allLayerGroup[i];
		groupNames.push(ap.name);
		groups.push(ap);
	}
}
// 添加嵌套的图层组
for	(var i = 0; i < allLayerGroup.length; i++){
	getgroups(allLayerGroup[i]);
}

for	(var i = 0; i < groups.length; i++)
{
	var tempP = groups[i];
	alert(tempP);
	groups[i].merge();
}

function getgroups(group)
{
	if(group.layerSets.length > 0 )
	{
		for (var i = 0; i < group.layerSets.length; i++)
		{
			var tempGroup = group.layerSets[i];
			getgroups(tempGroup);
		}
	}
	else
	{
		if(contains(groupNames,group.name))
		{
			// alert("发现重名的资源，名字为" + group.name);
		}
		else
		{
			groupNames.push(group.name);
			groups.push(group);
		}
	}
}

function contains(array,e){
	for (var i = 0; i < array.length; i++){
		var arrayE = array[i];
		if(arrayE == e){
			return true;
		}
	}
	
	return false;
}


