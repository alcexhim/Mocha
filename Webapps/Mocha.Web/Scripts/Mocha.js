if (!System)
	var System = { };
	
System.GetCurrentTenantName = function()
{
	var path = window.location.href.substring(window.location.href.indexOf('/', window.location.href.indexOf('/') + 2) + 1);
	var tenantName = path.substring(0, path.indexOf('/'));
	return tenantName;
};

if (System.ExpandRelativePath)
{
	System._ExpandRelativePath = System.ExpandRelativePath;
	System.ExpandRelativePath = function(path)
	{
		var tenantName = System.GetCurrentTenantName();
		if (path.startsWith("~/"))
		{
			return System._ExpandRelativePath("~/" + tenantName + path.substring(1));
		}
		return path;
	};
}