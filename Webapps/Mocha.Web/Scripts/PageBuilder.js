function PageBuilder()
{
}

PageBuilder.Begin = function()
{
	document.getElementById('ctl00_ctl00_aspcContent_aspcContent_pnlDashboard').style.display ='none';
	document.getElementById('ctl00_ctl00_aspcContent_aspcContent_pnlPageBuilder').style.display='block';
};

PageBuilder.AddComponent = function()
{
	var w = document.getElementById('ctl00_ctl00_aspcContent_aspcContent_wndPageBuilderAddComponent');
	w.NativeObject.ShowDialog();
};

PageBuilder.CreatePageComponentFromJSON = function(jsonObject)
{
	switch (jsonObject.type)
	{
		case "box":
		{
			var div = document.createElement("div");
			System.ClassList.Add(div, "uwt-layout");
			System.ClassList.Add(div, "uwt-layout-box");
			if (jsonObject.orientation == "horizontal")
			{
				System.ClassList.Add(div, "uwt-orientation-horizontal");
			}
			else
			{
				System.ClassList.Add(div, "uwt-orientation-vertical");
			}
			
			if (jsonObject.components)
			{
				for (var i = 0; i < jsonObject.components.length; i++)
				{
					var zqElem = PageBuilder.CreatePageComponentFromJSON(jsonObject.components[i]);
					div.appendChild(zqElem);
				}
			}				
			return div;
		}
		case "button":
		{
			var a = document.createElement("a");
			a.className = "uwt-button";
			a.href = "#";
			a.innerText = jsonObject.text;
			return a;
		}
		case "chart":
		{
			var div = document.createElement("div");
			div.className = "uwt-chart";
			if (jsonObject.chartType)
			{
				div.setAttribute("data-chart-type", jsonObject.chartType);
			}
			return div;
		}
		case "summary":
		{
			var table = document.createElement("table");
			table.className = "uwt-formview";
			for (var i = 0; i < jsonObject.items.length; i++)
			{
				var tr = document.createElement("tr");
				
				var td1 = document.createElement("td");
				var label = document.createElement("label");
				label.innerText = jsonObject.items[i].text;
				td1.appendChild(label);
				tr.appendChild(td1);
				
				var td2 = document.createElement("td");
				
				switch (jsonObject.items[i].type)
				{
					case "text":
					{
						var span = document.createElement("span");
						span.innerText = jsonObject.items[i].value;
						td2.appendChild(span);
						break;
					}
					case "instance":
					{
						var instanceKeys = null;
						if (jsonObject.items[i].value != null)
						{
							instanceKeys = jsonObject.items[i].value.split(",");
						}
						var div = McxInstanceBrowser.Create(instanceKeys);
						td2.appendChild(div);
						
						break;
					}
				}
				
				tr.appendChild(td2);
				table.appendChild(tr);
			}
			return table;
		}
	}
};

window.addEventListener("load", function()
{
	var pagebuilderPlaceholders = document.getElementsByClassName("mcx-pagebuilder-placeholder");
	for (var i = 0; i < pagebuilderPlaceholders.length; i++)
	{
		pagebuilderPlaceholders[i].addEventListener("click", function()
		{
			PageBuilder.AddComponent();
		});
	}
});