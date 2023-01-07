window.addEventListener("load", function()
{
	// set up validators for summary page components
	var summaryPageComponents = document.getElementsByClassName("mcx-summary");
	for (var i = 0; i < summaryPageComponents.length; i++)
	{
		var summaryPageComponent = summaryPageComponents[i];
		for (var j = 0; j < summaryPageComponent.rows.length; j++)
		{
			var ctl = summaryPageComponent.rows[j].cells[1].children[0];
			
		}
	}
});