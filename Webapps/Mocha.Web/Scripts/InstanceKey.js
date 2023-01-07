function InstanceKey(cid, iid)
{
	this.ClassIndex = cid;
	this.InstanceIndex = iid;
	
	this.toString = function()
	{
		return this.ClassIndex + "$" + this.InstanceIndex;
	};
}

InstanceKey.parse = function(str)
{
	var parts = str.split("$");
	var ikey = new InstanceKey(parts[0], parts[1]);
	return ikey;
};