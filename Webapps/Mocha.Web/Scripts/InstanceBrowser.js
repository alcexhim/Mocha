function McxInstanceBrowser(parentElement)
{
	this.ParentElement = parentElement;
	this.TextBoxElement = this.ParentElement.children[0];
	this.PopupElement = this.ParentElement.children[3];
	
	this.SelectedListElement = this.ParentElement.children[1];
	this.MenuElement = this.PopupElement.children[1].children[0];
	
	this.TextBoxElement.NativeObject.AddItem = function(textbox, item)
	{
		textbox.SelectedItems = new Array();
		textbox.SelectedItems.push(item);
		console.log(item);
		textbox.SetText(item.title);
		return true;
	};
	
	this.TextBoxElement.NativeObject.EventHandlers.DropDownClosed.Add(function(sender, e)
	{
		System.ClassList.Add(sender.ParentElement, "uwt-color-success");
	});
	
	this.TextBoxElement._McxNativeObject = this;
	this.TextBoxElement.addEventListener("blur", function(e)
	{
		System.ClassList.Remove(this._McxNativeObject.ParentElement, "mcx-active");
		System.ClassList.Remove(this._McxNativeObject.PopupElement, "uwt-visible");
	});
	this.TextBoxElement.addEventListener("focus", function(e)
	{
		System.ClassList.Add(this._McxNativeObject.ParentElement, "mcx-active");
		System.ClassList.Add(this._McxNativeObject.PopupElement, "uwt-visible");
	});
	
	this.TextBoxElement.NativeObject.SuggestionURL = System.ExpandRelativePath("~/api/search?q=%1&valid_class_ids=" + this.ParentElement.getAttribute("data-valid-class-ids"));
	this.TextBoxElement.NativeObject.SetLoading = function(value)
	{
		if (value)
		{
			System.ClassList.Add(this.TextBoxElement._McxNativeObject.ParentElement, "uwt-loading");
		}
		else
		{
			System.ClassList.Remove(this.TextBoxElement._McxNativeObject.ParentElement, "uwt-loading");
		}
	};
	
	this.AddMenuItem = function()
	{
	};
	
	this.ListElement = this.ParentElement.children[2];
	for (var i = 0; i < this.ListElement.children.length; i++)
	{
		var apb = this.ListElement.children[i].children[0];
		if (apb.className == "mcx-count-link")
		{
			apb.NativeObject_InstanceBrowser = this;
			apb.addEventListener("click", function()
			{
				var rcid = this.NativeObject_InstanceBrowser.ParentElement.parentElement.getAttribute("data-rcid");
				var xhr = new XMLHttpRequest();
				xhr.rcid = rcid;
				xhr.riid = this.NativeObject_InstanceBrowser.ParentElement.parentElement.parentElement.getAttribute("data-instance-id");
				xhr.open("GET", "/" + System.GetCurrentTenantName() + "/api/drilldown/" + rcid);
				
				xhr.onreadystatechange = function()
				{
					if (xhr.readyState === XMLHttpRequest.DONE)
					{
						if (xhr.status == 200)
						{
							alert("Stil not implemented: get all instances for (report column iid: " + this.rcid + ", row iid: " + this.riid + ")");
						}
						else
						{
							Alert.Show("AJAX request failed(MADI internal call: McxCountLink)", "Error", "uwt-color-danger");
						}
					}
				};
				xhr.send(null);
			});
			continue;
		}
		
		apb.children[1].NativeObject_InstanceBrowser = this;
		apb.children[1].Index = i;
		apb.children[1].addEventListener("click", function(e)
		{
			var instIds = this.NativeObject_InstanceBrowser.ParentElement.getAttribute("data-instance-ids").split(",");
			var xhr = new XMLHttpRequest();
			xhr.instId = instIds[this.Index];
			xhr.open("GET", "/" + System.GetCurrentTenantName() + "/api/preview/" + instIds[this.Index]);
			xhr.NativeObject_InstanceBrowser = this.NativeObject_InstanceBrowser;
			
			// IMPORTANT: WE NEED TO go through all these hoops because JavaScript thinks that apb is the last in the list
			xhr.NativeObject = xhr.NativeObject_InstanceBrowser.ListElement.children[this.Index].children[0].NativeObject;
			xhr.onreadystatechange = function()
			{
				if(xhr.readyState === XMLHttpRequest.DONE)
				{
					var status = xhr.status;
					if (status === 0 || (status >= 200 && status < 400))
					{
						// The request has been completed successfully
						var json = JSON.parse(xhr.responseText);
						
						if (json.result == "failure")
						{
							Alert.Show(json.message, "Error", "uwt-color-danger");
							return;
						}
						
						xhr.NativeObject.ClearPreviewMenuItems();
						if (typeof(json.preview) !== 'undefined')
						{
							if (typeof(json.preview.actions) !== 'undefined')
							{
								for (var i = 0; i < json.preview.actions.length; i++)
								{
									for (var j = 0; j < json.preview.actions[i].actions.length; j++)
									{
										if (typeof(json.preview.actions[i].actions[j].url) === 'undefined' &&
											(typeof(json.preview.actions[i].actions[j].instanceId !== 'undefined')))
										{
											json.preview.actions[i].actions[j].url = System.ExpandRelativePath("~/d/inst/" + this.instId + "/rel-task/" + json.preview.actions[i].actions[j].instanceId + ".htmld");
										}
									}
									xhr.NativeObject.AddPreviewMenuItem(json.preview.actions[i].title, json.preview.actions[i].actions, json.preview.actions[i].newWindow);
								}
							}
							
							var zqElemContainer = document.createElement("div");
							zqElemContainer.className = "uwt-content";
							if (typeof(json.preview.components) !== 'undefined')
							{
								for (var i = 0; i < json.preview.components.length; i++)
								{
									var zqElem = PageBuilder.CreatePageComponentFromJSON(json.preview.components[i]);
									zqElemContainer.appendChild(zqElem);
								}
							}
							
							var elem = document.createElement("div");
							var elemTitle = document.createElement("div");
							elemTitle.className = "apb-title";
							
							var elemClassTitle = document.createElement("h1");
							elemClassTitle.innerText = json.preview.classTitle;
							elemTitle.appendChild(elemClassTitle);
							
							var elemInstanceTitle = document.createElement("h2");
							var elemInstanceLink = document.createElement("a");
							elemInstanceLink.href = System.ExpandRelativePath("~/d/inst/" + this.instId + ".htmld");
							elemInstanceLink.innerText = json.preview.instanceTitle;
							elemInstanceTitle.appendChild(elemInstanceLink);
							
							elemTitle.appendChild(elemInstanceTitle);
							elem.appendChild(elemTitle);
							
							elem.appendChild(zqElemContainer);
							
							xhr.NativeObject.SetPreviewContentElement(elem);
						}
						
						xhr.NativeObject.SetLoading(false);
					}
					else
					{
						// Oh no! There has been an error with the request!
						Alert.Show("AJAX request failed", "Error", "uwt-color-danger");
					}
				}
			};
			xhr.send(null);
			
			e.preventDefault();
			e.stopPropagation();
		});
	}
}

McxInstanceBrowser.Create = function(instanceIds)
{
	var div = document.createElement("div");
	div.className = "mcx-instancebrowser";
	div.setAttribute("data-valid-class-ids", "");
	
	var inputText = document.createElement("input");
	inputText.type = "text";
	inputText.setAttribute("autocomplete", "off");
	div.appendChild(inputText);
	
	var inputHidden = document.createElement("input");
	inputHidden.type = "hidden";
	div.appendChild(inputHidden);
	
	var ul = document.createElement("ul");
	
	if (typeof(instanceIds) !== 'undefined')
	{
		if (instanceIds != null)
		{
			if (!Array.isArray(instanceIds))
			{
				// assume single instance ID passed in as parameter
				instanceIds = [instanceIds];
			}
			div.setAttribute("data-instance-ids", instanceIds.join(","));
			
			for (var i = 0; i < instanceIds.length; i++)
			{
				var li = document.createElement("li");
				var apb = AdditionalDetailWidget.Create("", System.ExpandRelativePath("~/d/inst/" + instanceIds[i] + ".htmld"), System.ExpandRelativePath("~/api/preview/" + instanceIds[i]), { "data-instance-id": instanceIds[i] });
				li.appendChild(apb);
				ul.appendChild(li);
				
				Instance.GetByInstanceID(instanceIds[i], function(instanceId, json, userParm)
				{
					userParm.NativeObject.SetTitle(json.items[0].title);
				}, apb);
			}
		}
		else
		{
			div.setAttribute("data-instance-ids", "");
			System.ClassList.Add(div, "mcx-empty");
		}
	}
	div.appendChild(ul);
	
	var divPopup = document.createElement("div");
	divPopup.className = "uwt-popup uwt-loading";
	var divSpinner = document.createElement("div");
	divSpinner.className = "uwt-spinner";
	divPopup.appendChild(divSpinner);
	
	var divPopupContent = document.createElement("div");
	divPopupContent.className = "uwt-dropdown-content";
	divPopup.appendChild(divPopupContent);
	div.appendChild(divPopup);
	
	window.debugDiv = div;
	
	// NOTE: must be called AFTER all controls have been added, but BEFORE NativeObject is set on the McxInstanceBrowser
	inputText.NativeObject = new TextBox(inputText);
	
	div.NativeObject = new McxInstanceBrowser(div);
	
	return div;
};

window.addEventListener("load", function()
{
	/*
	$('.mcx-instancebrowser').select2({
	  ajax: {
	    url: Mocha.GetCurrentTenantURL() + '/api/search.aspx',
	    dataType: 'json'
	    // Additional AJAX parameters go here; see the end of this chapter for the full code of this example
	  }
	});
	*/
	
	var items = document.getElementsByClassName("mcx-instancebrowser");
	for (var i = 0; i < items.length; i++)
	{
		items[i].NativeObject = new McxInstanceBrowser(items[i]);
	}
});