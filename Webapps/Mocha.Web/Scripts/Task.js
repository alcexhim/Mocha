window.addEventListener("load", function()
{
	var cmdPageFooterCancel = document.getElementById("cmdPageFooterCancel");
	
	cmdPageFooterCancel.addEventListener("click", function()
	{
		history.back();
		
		e.preventDefault();
		e.stopPropagation();
		return false;
	});
});