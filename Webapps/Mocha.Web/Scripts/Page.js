window.addEventListener("load", function(e)
{
	var sidebarMenu = document.getElementById("ctl00_ctl00_aspcBeforeContent_ul"); // yeah...no
	if (sidebarMenu != null)
	{
		var sidebarMenuItems = sidebarMenu.children;
		for (var i = 0; i < sidebarMenuItems.length; i++)
		{
			if (window.location.href == sidebarMenuItems[i].children[0].href)
			{
				System.ClassList.Add(sidebarMenuItems[i], "uwt-selected");
			}
			else
			{
				System.ClassList.Remove(sidebarMenuItems[i], "uwt-selected");
			}
		}
	}

	var cmdPageFooterCancel = document.getElementById("cmdPageFooterCancel");
	cmdPageFooterCancel.addEventListener("click", function(e)
	{
		history.back();
		
		e.preventDefault();
		return false;
	});
	
	var controlbox = document.getElementById("ctl00_ctl00_aspcBeforePageContent_pnlTaskControlBox");
	if (controlbox !== undefined)
	{
		var ccbConfigure = controlbox.children[0].children[0];
		var ccbDownload = controlbox.children[1].children[0];
		var ccbPrint = controlbox.children[2].children[0];
		
		ccbConfigure.addEventListener("click", function(ee)
		{
			/**
				Displays a popup menu with configuration options for this page.
			*/
			var wu = new ContextMenu();
			wu.Items =
			[
				{ "ClassName": "MenuItemCommand", "Title": "Edit Page", "Visible": true },
				{ "ClassName": "MenuItemCommand", "Title": "View Page Instance", "Visible": true },
				{ "ClassName": "MenuItemSeparator", "Visible": true },
				{ "ClassName": "MenuItemCommand", "Title": "About Mocha", "Visible": true }
			];
			wu.Show(ee.x, ee.y, document.body);
			
			ee.preventDefault();
			ee.stopPropagation();
			return false;
		});
		ccbPrint.addEventListener("click", function(ee)
		{
			print();
			
			ee.preventDefault();
			ee.stopPropagation();
			return false;
		});
	}
});