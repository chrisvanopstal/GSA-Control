<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>
<%@ Register TagName="Modern" TagPrefix="GSA" Src="~/GSA Control Modern.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

    <style type="text/css">
        body { font-family : Arial; margin:2em 7em; }

        @media screen and (max-device-width: 4800px) {
            body { margin:0; }
        }
    </style>
</head>
<body>
    <form runat="server">
    <div>

        <GSA:Modern runat="server" 
            Server="search.caltech.edu" 
            Frontend="default_frontend" 
            SiteCollections="default_collection" 
            ResultsPerPage="15"  />
    
    </div>
    </form>
</body>
</html>
