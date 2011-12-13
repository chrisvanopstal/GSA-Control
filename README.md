# Google Search Appliance User Control

A user control you can quickly deploy and use as an interface to your Google Search Appliance (GSA).

The control uses a refactored version of the C# [GSALib library][1] to query the GSA and return results. Results are then rendered using ASP.NET/C# (No XSL required!)

## Features

**Responsive layout** The control will render gracefully on screens as narrow as 480px or as large as you want. The same control can be used on mobile devices and for your main search.

**Deploy ready** You can deploy the control in a few minutes. 

## Quickly Deploying

1. Add GSALib.dll to your bin.
2. Add "GSA Control Modern.ascx" to your project.
3. Add the code below to your search page, customizing the variables in uppercase.

	    <%@ Register TagName="Modern" TagPrefix="GSA" Src="~/YOUR-PATH/GSA Control Modern.ascx" %>

	    <GSA:Modern runat="server" 
	        Server="YOUR-SERVER" 
	        Frontend="YOUR-FRONTEND" 
	        SiteCollections="YOUR-COLLECTION" 
	        ResultsPerPage="15"  />
			
**Server:** The name (or IP address) of your GSA server.

**Frontend:** (Optional) The Front End to use for displaying your results. This will not affect the general appearance of the control but will affect any functionality you configured for the Front End including filtesr or URLs you may not want to appear in the results.

**SiteCollections:** (Optional) The collection to search.

**ResultsPerPage:** (Optional) How many results you want to appear per page.

## After Deploying

The user control contains a CSS block that should ideally be moved to one of your site's stylesheets.

  [1]: http://gsalib.codeplex.com/