<%@ Control Language="C#" AutoEventWireup="true" CodeFile="GSA Control Modern.ascx.cs" Inherits="GSA_Control_Modern" %>
<style type="text/css">
    /*******************************************
     MOVE THIS CSS

     This CSS is provided inline to allow you to
     easily test this control in your environment.
     For production, copy/paste this CSS to one
     of your stylesheets.
    ********************************************/

    .gsa-left-col { float:left; padding:0 0 0 20px; width:120px; color:#888;  }
    .gsa-right-col { float:left; padding:0 0 0 20px; min-width:400px; }

    /* advanced search */
    .gsa-search-advanced { border-top:1px solid #eee; padding:10px 0px 10px 0px; float:left; width:100%; }
    .gsa-search-advanced .gsa-right-col { color:#555; }

    .gsa-search-advanced input, .gsa-search-advanced select { padding:2px 3px; }
    .gsa-search-advanced input { width:15em; }

    .gsa-hide-advanced-options { float:right; padding-right:20px; }

    /* search box */
    .gsa-search { background:#F5F5F5; border-bottom:1px solid #eee; padding:20px 0; float:left; width:100%; }
    .gsa-search-wrapper { min-width:415px; float:left;} 
    .gsa-search-box { font-size:16px; padding:3px 5px; width: 300px; color:#222; }
    .gsa-show-advanced-options { float:right; line-height:30px; padding-right:10px; }

    /* results header */
    .gsa-results-header { color:#888; padding:20px 0; border-bottom:1px solid #eee; float:left; width:100%; }
    .gsa-results-header strong { color:#555; }

    /* results filter (not implemented by default) */
    .gsa-results-filter {}
    .gsa-results-filter ul { list-style:none; margin:0 10px 0 0; padding:0; font-size:14px; }
    .gsa-results-filter li { }
    .gsa-results-filter a { color:#222; display:block; padding:6px 10px 6px 20px; margin-bottom:4px; text-decoration:none;  }
    .gsa-results-filter a:hover { background: #F5F5F5;}
    .gsa-results-filter a.active { font-weight:bold; }

    /* results box */
    .gsa-results { padding:10px 0px 10px 0px; float:left; width:100%; }
    .gsa-results .gsa-right-col { padding-left:20px; }
    .gsa-results .gsa-left-col { padding-top:10px; }
    .gsa-sort { padding:3px 10px 3px 3px; font-size:12px; float:right; }
    .gsa-results ol { padding:0; margin:0; list-style:none; }
    .gsa-results .gsa-left-col { }
     
    /* individual result items */
    .gsa-result { margin: 0 0 1em 0; padding:0;  }
    .gsa-result-title { }
    .gsa-result-url { color:Green; font-size:12px; line-height:16px; overflow:hidden; white-space:nowrap; text-overflow: ellipsis; max-width:400px; }
    .indentation2 .gsa-result-url { max-width:350px;  }
    .gsa-result-summary { font-size:14px; max-width:500px; }

     /* indentation levels for results */
    .indentation1 {}
    .indentation2 { margin-left:3em;  }

    /* pager */
    .gsa-pager-wrapper { margin-top:1em; border-top:1px solid #eee; padding-top:1em; margin-bottom:2em; float:left; width:100%; }
    .gsa-pager { padding: 2x 5px; }
    .gsa-pager-next { font-weight:bold; padding:0 10px 0 30px; } 
    .gsa-pager-previous { font-weight:bold; padding:0 30px 0 10px; }

    /* buttons */
    .gsa-search-button
    {
        background: -webkit-gradient( linear, left top, left bottom, color-stop(0.05, #4D90FE), color-stop(1, #4D90FE) );
        background: -moz-linear-gradient( center top, #4D90FE 5%, #4D90FE 100% );
        filter: progid:DXImageTransform.Microsoft.gradient(startColorstr='#4D90FE', endColorstr='#4D90FE');
        background-color: #63b8ee;
        -moz-border-radius: 3px;
        -webkit-border-radius: 3px;
        border-radius: 3px;
        border: 1px solid #3079ED;
        display: inline-block;
        color: #ffffff;
        font-family: arial;
        font-size: 15px;
        padding: 5px 20px;
        text-decoration: none;
        cursor:pointer;
        max-height:30px;
        vertical-align:bottom;
    }

    .gsa-search-button:hover, .gsa-search-button:active {
        background: -webkit-gradient( linear, left top, left bottom, color-stop(0.05, #4D90FE), color-stop(1, #3181ff) );
        background: -moz-linear-gradient( center top, #4D90FE 5%, #3181ff 100% );
        filter: progid:DXImageTransform.Microsoft.gradient(startColorstr='#4D90FE', endColorstr='#3181ff');
        background-color: #63b8ee;
        border: 1px solid #4771b2;
    }

    @media screen and (max-device-width: 4800px) {
        .gsa-results-header {font-size:14px; }
        .gsa-results-header .gsa-left-col { width:auto; padding-right: 1em; }

    }
</style>

<div class="gsa-search-advanced" runat="server" id="pnlAdvancedOptions" visible="false">
    <asp:LinkButton runat="server" ID="btnHideAdvancedSearch" Text="Hide advanced search" CssClass="gsa-hide-advanced-options" OnClick="HideAdvancedOptions" />
    <div class="gsa-left-col">
        <div>Advanced Options</div>
    </div>
    <div class="gsa-right-col">
        <table>
            <tbody>
                <tr>
                    <td>
                        <strong>all</strong> these words:
                    </td>
                    <td>
                        <asp:TextBox runat="server" ID="txtAdvanced_AndQueryTerms" />
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td>
                        this <strong>exact phrase</strong>:
                    </td>
                    <td>
                        <asp:TextBox runat="server" ID="txtAdvanced_ExactPhrase" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <strong>at least one</strong> of the words:
                    </td>
                    <td>
                        <asp:TextBox runat="server" ID="txtAdvanced_OrQueryTerms" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <strong>without</strong> the words:
                    </td>
                    <td>
                        <asp:TextBox runat="server" ID="txtAdvanced_ExcludeQueryTerms" />
                    </td>
                </tr>

                 <td>
                        results per page:
                    </td>
                    <td valign="top">
                        <asp:DropDownList runat="server" ID="ddlAdvanced_ResultsPerPage">
                            <asp:ListItem Value=""></asp:ListItem>
                            <asp:ListItem Value="25">25 results</asp:ListItem>
                            <asp:ListItem Value="50">50 results</asp:ListItem>
                            <asp:ListItem Value="100">100 results</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                <tr>
                    <td>
                        language:
                    </td>
                    <td>
                        <asp:DropDownList runat="server" ID="ddlAdvanced_Language">
                            <asp:ListItem Value="">any language</asp:ListItem>
                            <asp:ListItem Value="lang_ar">Arabic</asp:ListItem>
                            <asp:ListItem Value="lang_zh-CN">Chinese (Simplified)</asp:ListItem>
                            <asp:ListItem Value="lang_zh-TW">Chinese (Traditional)</asp:ListItem>
                            <asp:ListItem Value="lang_cs">Czech</asp:ListItem>
                            <asp:ListItem Value="lang_da">Danish</asp:ListItem>
                            <asp:ListItem Value="lang_nl">Dutch</asp:ListItem>
                            <asp:ListItem Value="lang_en">English</asp:ListItem>
                            <asp:ListItem Value="lang_et">Estonian</asp:ListItem>
                            <asp:ListItem Value="lang_fi">Finnish</asp:ListItem>
                            <asp:ListItem Value="lang_fr">French</asp:ListItem>
                            <asp:ListItem Value="lang_de">German</asp:ListItem>
                            <asp:ListItem Value="lang_el">Greek</asp:ListItem>
                            <asp:ListItem Value="lang_iw">Hebrew</asp:ListItem>
                            <asp:ListItem Value="lang_hu">Hungarian</asp:ListItem>
                            <asp:ListItem Value="lang_is">Icelandic</asp:ListItem>
                            <asp:ListItem Value="lang_it">Italian</asp:ListItem>
                            <asp:ListItem Value="lang_ja">Japanese</asp:ListItem>
                            <asp:ListItem Value="lang_ko">Korean</asp:ListItem>
                            <asp:ListItem Value="lang_lv">Latvian</asp:ListItem>
                            <asp:ListItem Value="lang_lt">Lithuanian</asp:ListItem>
                            <asp:ListItem Value="lang_no">Norwegian</asp:ListItem>
                            <asp:ListItem Value="lang_pl">Polish</asp:ListItem>
                            <asp:ListItem Value="lang_pt">Portuguese</asp:ListItem>
                            <asp:ListItem Value="lang_ro">Romanian</asp:ListItem>
                            <asp:ListItem Value="lang_ru">Russian</asp:ListItem>
                            <asp:ListItem Value="lang_es">Spanish</asp:ListItem>
                            <asp:ListItem Value="lang_sv">Swedish</asp:ListItem>
                            <asp:ListItem Value="lang_tr">Turkish</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                        file type:
                    </td>
                    <td>
                        <asp:DropDownList runat="server" ID="ddlAdvanced_Filetypes">
                            <asp:ListItem Value="" Selected="" Text="any format" />
                            <asp:ListItem Value="pdf" Text="Adobe Acrobat PDF (.pdf)" />
                            <asp:ListItem Value="ps" Text="Adobe Postscript (.ps)" />
                            <asp:ListItem Value="doc" Text="Microsoft Word (.doc)" />
                            <asp:ListItem Value="xls" Text="Microsoft Excel (.xls)" />
                            <asp:ListItem Value="ppt" Text="Microsoft Powerpoint (.ppt)" />
                            <asp:ListItem Value="rtf" Text="Rich Text Format (.rtf)" />
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                        search:
                    </td>
                    <td>
                        <asp:DropDownList runat="server" ID="ddlAdvanced_Occurences">
                            <asp:ListItem Value="" Text="anywhere in the page" />
                            <asp:ListItem Value="title" Text="in the title of the page" />
                            <asp:ListItem Value="url" Text="in the URL of the page" />
                        </asp:DropDownList>
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
</div>

<div class="gsa-search">
    <div class="gsa-left-col">
        <div style="line-height:30px;">Search</div>
    </div>
    <asp:LinkButton runat="server" ID="btnShowAdvancedOptions" onclick="ShowAdvancedOptions" Text="Advanced search" class="gsa-show-advanced-options" /> 
    <div class="gsa-right-col">
        <div class="gsa-search-wrapper">
            <asp:TextBox runat="server" ID="txtSearch" class="gsa-search-box" size="40" /> 
            <asp:Button runat="server" ID="btnSearch" Text="Search" class="gsa-search-button" />
        </div>
        
    </div>
</div>

<div class="gsa-results-header" runat="server" id="pnlResultsHeader">
    <div class="gsa-left-col">Results</div>
    <div class="gsa-right-col">
        <strong><asp:Literal runat="server" ID="litStartIndex" /></strong> - <strong><asp:Literal runat="server" ID="litEndIndex" /></strong>
        of about <strong><asp:Literal runat="server" ID="litTotalResults" /></strong> for <strong><asp:Literal runat="server" ID="litQuery" /></strong>.
        Search took <strong><asp:Literal runat="server" ID="litSearchTime" /></strong> seconds.
    </div>
</div>

<div class="gsa-results">
    <div class="gsa-left-col">
    
        <div class="gsa-results-filter">
        <%--
            You can add custom filters in this section. Example below.
            
            <div style="margin:5px 0 10px 20px; color:#888;">
                Show
            </div>
            <ul>
                <li><a href="#" class="active">Everything</a></li>
            </ul>
        --%>
        </div>
             
    </div>

           <div class="gsa-sort">
            <%=RenderSortLinks() %>
            <asp:HiddenField runat="server" ID="hdnSort" Value="Relevance" />
        </div>


    <div class="gsa-right-col" runat="server" id="pnlResultsRightBar">

 
        <ol>
            <asp:Repeater runat="server" ID="rptResults">
            <ItemTemplate>
                <%#Eval("indentation").ToString() == "2" ? "<ol type=\"a\">" : string.Empty %>
                    <li class="gsa-result indentation<%#Eval("indentation") %>"> 
                        <a class="gsa-result-title" ctype="c" rank="<%#Eval("index") %>" href="<%#Eval("escapedUrl")%>"><span class="l"><%#HttpUtility.HtmlDecode(Eval("Title").ToString())%></span></a>
                        <div class="gsa-result-url">
                            <%#HttpUtility.UrlDecode(Eval("url").ToString().Replace("http://", string.Empty)) %> <%#RenderCacheDocSize(Eval("cacheDocSize")) %>
                        </div>
                        <div class="gsa-result-summary">
                            <%#HttpUtility.HtmlDecode(Eval("summary").ToString()).Replace("<br>", string.Empty)%>
                        </div>
  
                        <%#RenderMoreDetailsUrl(Eval("MoreDetailsUrl"))%>

                    </li>
                <%#Eval("indentation").ToString() == "2" ? "</ol>" : string.Empty %>
            </ItemTemplate>
            </asp:Repeater>
        </ol>

        <div runat="server" id="pnlRepeatSearch">
            <em>
                In order to show you the most relevant results, we have omitted some entries very similar to the <asp:Literal runat="server" ID="litRepeatSearch_NumResults" /> already displayed.
                <br />If you like, you can <asp:Literal runat="server" ID="litRepeatSearch_URL" />.
            </em>
        </div>
    </div>
    <div class="gsa-pager-wrapper" runat="server" id="pnlPager">
        <%=RenderPager()%>
    </div>
</div>