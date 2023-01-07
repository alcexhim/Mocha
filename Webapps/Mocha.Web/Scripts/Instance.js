function Instance()
{
}

Instance.Get = function()
{
	
};
Instance.GetByInstanceID = function(instanceId, callback, userParm)
{
	if (instanceId == null)
	{
		return null;
	}
	
	// var url = "http://127.0.0.1:8080/ccx/service/default/inst/" + classID + "$" + instanceID + ".htmld";
	var url = "~/api/inst/" + instanceId;
	var xhr = new XMLHttpRequest();
	xhr.open("GET", System.ExpandRelativePath(url));
	xhr.instanceId = instanceId;
	xhr.onreadystatechange = function()
	{
		if (xhr.readyState === XMLHttpRequest.DONE)
		{
			var status = xhr.status;
			if (status === 200)
			{
				// The request has been completed successfully
				var json = JSON.parse(xhr.responseText);
				callback(xhr.instanceId, json, userParm);
			}
		}
	};
	xhr.send(null);
};

function Clipboard()
{
}
Clipboard.SetText = function(text, parentElement)
{
	if (typeof(parentElement) === 'undefined')
		parentElement = document.body;
		
	var dummy = document.createElement("input");
	parentElement.appendChild(dummy);
	dummy.style.opacity = "0";
	dummy.type = "text";
	dummy.value = text;
	dummy.select();
	dummy.focus();
	
	document.execCommand("cut");
	
	parentElement.removeChild(dummy);
	dummy = null;
};

Instance.GetURL = function(instid)
{
	return System.ExpandRelativePath("~/d/inst/" + instid + ".htmld");
};

Instance.ShowContextMenu = function(e, element, instid, text, isField)
{
	// var instid = element.getAttribute("data-instance-id");
	var menu = new ContextMenu();
	if (isField)
	{
		menu.Items.push(new MenuItemCommand("ShowFieldProperties", "Show Field Properties", function(sender, e)
		{
		}));
		menu.Items.push(new MenuItemCommand("ShowFieldEC", "Show Field EC", function(sender, e)
		{
			window.open(Instance.GetURL(instid));
			// alert(odwParent.TextElement.href);
		}));
	}
	else
	{
		menu.Items.push(new MenuItemCommand("SeeInNewTab", "See in New Tab", function(sender, e)
		{
			window.open(element.NativeObject.TextLink.href);
		}));
		menu.Items.push(new MenuItemCommand("CopyURL", "Copy URL", function(sender, e)
		{
			// alert(odwParent.TextElement.href);
		}));
		menu.Items.push(new MenuItemCommand("CopyText", "Copy Text", function(sender, e)
		{
			Clipboard.SetText(text, sender.parentElement);
		}));
		menu.Items.push(new MenuItemSeparator());
		menu.Items.push(new MenuItemCommand("CopyInstanceID", "Copy Instance ID (" + instid + ")", function(sender, e)
		{
			Clipboard.SetText(instid, sender.parentElement);
		}));
		menu.Items.push(new MenuItemCommand("CopyTextAndInstanceID", "Copy Text and Instance ID", function(sender, e)
		{
			Clipboard.SetText(text + " (" + instid + ")", sender.parentElement);
		}));
		menu.Items.push(new MenuItemSeparator());
		menu.Items.push(new MenuItemCommand("ModifyInstance", "Modify Instance (" + instid + ")", function(sender, e)
		{
			window.location.href = System.ExpandRelativePath("~/instances/modify/" + instid);
		}));
		menu.Items.push(new MenuItemCommand("ModifyInstanceInNewWindow", "Modify Instance in New Window", function(sender, e)
		{
			window.open(System.ExpandRelativePath("~/instances/modify/" + instid));
		}));
		menu.Items.push(new MenuItemSeparator());
		menu.Items.push(new MenuItemCommand("SearchInstanceID", "Search Instance ID (" + instid + ")", function(sender, e)
		{
			
		}));
		menu.Items.push(new MenuItemCommand("SearchInstanceIDInNewWindow", "Search Instance ID in New Window", function(sender, e)
		{
			
		}));
		menu.Items.push(new MenuItemSeparator());
		menu.Items.push(new MenuItemCommand("ViewPrintableVersion", "View Printable Version", function(sender, e)
		{
			
		}));
		menu.Items.push(new MenuItemCommand("ExportToSpreadsheet", "Export to Spreadsheet", function(sender, e)
		{
			
		}));
		
		if (System.ClassList.Contains(element, "uwt-actionpreviewbutton"))
		{
			menu.Items[0].Visible = true; // See In New Tab
			menu.Items[1].Visible = true; // Copy URL
		}
		else
		{
			menu.Items[0].Visible = false; // See In New Tab
			menu.Items[1].Visible = false; // Copy URL
		}

		menu.Items[13].Visible = false; // View Printable Version
		menu.Items[14].Visible = false; // Export to Spreadsheet
		
		/*
		if (typeof(ZeroClipboard) === 'undefined' && typeof(ClipboardJS) == 'undefined')
		{
			console.warn("found neither ZeroClipboard nor ClipboardJS - cut/copy menu items will be unavailable");
			menu.Items[1].Visible = false;
			menu.Items[2].Visible = false;
			menu.Items[3].Visible = false;
			menu.Items[4].Visible = false;
			menu.Items[5].Visible = false;
		}
		*/
	}
	menu.Show(e.clientX, e.clientY, element);
};

Instance.BuildContextMenu = function(elemParent)
{
	var elem = elemParent;
	var instid = elem.getAttribute("data-instance-id");
	if (instid == "0$0") return;
	
	var elemText = elemParent;
	
	var elemIsTR = false;
	if (elem.tagName == "TR")
	{
		elemIsTR = true;
		elem = elemParent.children[0];
		elemText = elem.children[0];
	}
	elem.instid = instid;
	
	if (System.ClassList.Contains(elem, "uwt-actionpreviewbutton"))
	{
		// only affect the text and button
		var elem1 = elem.children[0];
		var elem2 = elem.children[1];
		
		elem1.instid = elem.instid;
		elem1.addEventListener("contextmenu", function(ee)
		{
			Instance.ShowContextMenu(ee, this.parentElement, this.instid, elemText.innerText, false);
			ee.preventDefault();
			ee.stopPropagation();
		});
		elem2.instid = elem.instid;
		elem2.addEventListener("contextmenu", function(ee)
		{
			Instance.ShowContextMenu(ee, this.parentElement, this.instid, elemText.innerText, false);
			ee.preventDefault();
			ee.stopPropagation();
		});
	}
	else
	{
		elem.addEventListener("contextmenu", function(ee)
		{
			Instance.ShowContextMenu(ee, this, this.instid, elemText.innerText, elemIsTR);
			ee.preventDefault();
			ee.stopPropagation();
		});
	}
};

window.addEventListener("load", function(e)
{
	$('[data-instance-id]').each(function()
	{
		Instance.BuildContextMenu(this);
	});
	/*
	$('.mcx-instancebrowser').each(function()
	{
		var ul = this.children[0];
		for (var i = 0; i < ul.children.length; i++)
		{
			Instance.BuildContextMenu(ul.children[i].children[0]);
		}
	});
	*/
});

