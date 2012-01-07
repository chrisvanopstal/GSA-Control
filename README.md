# Google Search Appliance User Control

An ASP.NET User Control that you can embed into your project and search against a Google Search Appliance (GSA). This alleviates the difficulty of having to skin your GSA to look like your site and allows you to modify the look-and-feel of the search using CSS, C#, and standard ASP.NET controls (no more xsl!).

The control uses a refactored version of the C# [GSALib library][1] to query the GSA and return results.

##Demo

[Demo using Caltech's GSA][2]

## Responsive layout
 
The control uses a responsive layout and will render gracefully in most resolutions, including mobile devices.

Typical resolution  
[![](http://s3.amazonaws.com/cp-screenshots/gsa-control-screenshot-wide-thumbnail.png)](http://s3.amazonaws.com/cp-screenshots/gsa-control-screenshot-wide.png)

Narrow resolution  
[![Narrow resolution](http://s3.amazonaws.com/cp-screenshots/gsa-control-screenshot-narrow-thumbnail.png)](http://s3.amazonaws.com/cp-screenshots/gsa-control-screenshot-narrow.png)

iPhone  
[![Narrow resolution](http://s3.amazonaws.com/cp-screenshots/gsa-control-iphone-thumbnail.png)](http://s3.amazonaws.com/cp-screenshots/gsa-control-iphone.png)


## Deploying

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

**Frontend:** (Optional) The Front End to use for displaying your results. This will not affect the general appearance of the control but will affect any functionality you configured for the Front End including filters or URLs you may not want to appear in the results.

**SiteCollections:** (Optional) The collection to search.

**ResultsPerPage:** (Optional) How many results you want to appear per page.

## After Deploying

The user control contains a CSS block that should ideally be moved to one of your site's stylesheets.

  [1]: http://gsalib.codeplex.com/
  [2]: http://chrispebble.com/gsacontrol/