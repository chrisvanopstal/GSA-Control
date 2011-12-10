using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GSALib.GSA;
using System.Collections;
using GSALib.Constants;
using System.Text;

public partial class GSA_Control_Modern : System.Web.UI.UserControl
{
    public string Server { get; set; }
    public string Frontend { get; set; }
    public string SiteCollections { get; set; }
    public int ResultsPerPage { get; set; }

    protected int currentPage = 1;
    protected long totalResults = 0;
    protected Query query = new GSALib.GSA.Query();
    protected QueryTerm queryTerm = new QueryTerm();

    protected void Page_Init(object sender, EventArgs e)
    {
        query.MaxResults = this.ResultsPerPage == 0 ? 15 : this.ResultsPerPage;
        query.Frontend = string.IsNullOrEmpty(this.Frontend) ? "default_frontend" : this.Frontend;
        query.SiteCollections = string.IsNullOrEmpty(this.SiteCollections) ? new string[] { "default_collection" } : this.SiteCollections.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
        
        query.OutputFormat = GSALib.Constants.Output.XML_NO_DTD.getValue();
        query.OutputEncoding = GSALib.Constants.Encoding.UTF8;
        query.Access = GSALib.Constants.Access.PUBLIC;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        ResetInterface();

        if (!IsPostBack) {
            PopulateControlsFromQueryString();
        }        
    }

    protected void Page_PreRender()
    {
        PopulateQueryWithControlValues(query);

        if (!string.IsNullOrEmpty(query.queryTerm.getValue())) {
            try {
                pnlResultsHeader.Visible = pnlResultsRightBar.Visible = pnlPager.Visible = true;

                GSALib.GSA.ClientAccess ca = new GSALib.GSA.ClientAccess(Server);
                GSALib.GSA.Response res = default(GSALib.GSA.Response);
                res = ca.getGSAResponse(query, null);

                totalResults = res.getNumResults();
                pnlResultsRightBar.Visible = totalResults > 0;

                // determine whether to display the panel
                // allowing users to re-submit search
                // with omitted results included
                int currentStartIndex = Convert.ToInt32(query.ScrollAhead);
                if (currentStartIndex > totalResults) {
                    pnlRepeatSearch.Visible = true;
                    litRepeatSearch_NumResults.Text = totalResults.ToString();

                    // get the url for the unfiltered query
                    char initialFilter = query.Filter;
                    query.Filter = GSALib.Constants.Filtering.NO_FILTER.getValue();

                    litRepeatSearch_URL.Text = "<a href=\"?" + query.getValue() + "\">repeat the search with the omitted results included</a>";

                    // reset query
                    query.Filter = initialFilter;

                    // reset the current index
                    currentStartIndex = (int)totalResults;
                }

                // calculate current page
                currentPage = Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(currentStartIndex + 1) / ResultsPerPage));

                // update header
                litStartIndex.Text = res.getStartIndex().ToString();
                litEndIndex.Text = res.getEndIndex().ToString();
                litTotalResults.Text = res.getNumResults().ToString();
                litQuery.Text = res.getQuery();
                litSearchTime.Text = res.getSearchTime().ToString("0.00");

                var results = res.getResults();

                // GSA will, on rare occassions return results with null titles
                // strip these out.
                results = results.Where(r => !string.IsNullOrEmpty(r.Title)).ToList();

                rptResults.DataSource = results;
                rptResults.DataBind();
            } catch (GSALib.Exceptions.GSANeedNetworkCredentailsException ex) {
                Response.Write(ex.Message);
            }
        }
    }

    private void ResetInterface()
    {
        pnlRepeatSearch.Visible = pnlResultsHeader.Visible = pnlResultsRightBar.Visible = pnlPager.Visible = false;
    }

    private void PopulateControlsFromQueryString()
    {
        // re-populate the state of the query based on the querystring parameters
        query.Populate(Page.Request.QueryString);

        // sort
        hdnSort.Value = query.Sort;

        // ### Advanced options ###
        // re-populate the advanced options
        txtAdvanced_AndQueryTerms.Text = string.Join(" ", query.AndQueryTerms);
        txtAdvanced_ExactPhrase.Text = query.ExactPhraseQueryTerm;
        txtAdvanced_OrQueryTerms.Text = string.Join(" ", query.OrQueryTerms);
        txtAdvanced_ExcludeQueryTerms.Text = query.ExcludedQueryTerms;
        ddlAdvanced_Language.SelectedValue = query.Language;

        // results per page
        if (ddlAdvanced_ResultsPerPage.Items.FindByValue(query.MaxResults.ToString()) != null) {
            ddlAdvanced_ResultsPerPage.SelectedValue = query.MaxResults.ToString();
        }

        // language
        if (ddlAdvanced_Language.Items.FindByValue(query.Language) != null) {
            ddlAdvanced_Language.SelectedValue = query.Language;
        }

        // filetypes
        if (query.queryTerm.IncludeFiletype.Count > 0) {
            string fileType = query.queryTerm.IncludeFiletype[0].ToString().Trim();
            if (ddlAdvanced_Filetypes.Items.FindByValue(fileType) != null) {
                ddlAdvanced_Filetypes.SelectedValue = fileType;
            }
        }

        // occurences
        if (ddlAdvanced_Occurences.Items.FindByValue(query.SearchScope) != null) {
            ddlAdvanced_Occurences.SelectedValue = query.SearchScope;
        }

        // search string
        if (Page.Request.QueryString["q"] != null && string.IsNullOrEmpty(txtSearch.Text)) {
            txtSearch.Text = HttpUtility.UrlDecode(query.queryTerm.QueryString);
        }

        // show the advanced search panel if any of the options
        // are different from their defaults
        if (!string.IsNullOrEmpty(txtAdvanced_AndQueryTerms.Text)
            || !string.IsNullOrEmpty(txtAdvanced_ExactPhrase.Text)
            || !string.IsNullOrEmpty(txtAdvanced_OrQueryTerms.Text)
            || !string.IsNullOrEmpty(txtAdvanced_ExcludeQueryTerms.Text)
            || ddlAdvanced_Language.SelectedIndex != 0
            || ddlAdvanced_Filetypes.SelectedIndex != 0
            || ddlAdvanced_Occurences.SelectedIndex != 0) {
            pnlAdvancedOptions.Visible = true;
            btnShowAdvancedOptions.Visible = false;
        }
    }

    /// <summary>
    /// Checks the advanced search parameter controls and appends
    /// their values to the query if they have any.
    /// </summary>
    private void PopulateQueryWithControlValues(Query query)
    {
        // base search string
        query.queryTerm.setQueryString(txtSearch.Text);

        // sort
        if (hdnSort.Value.ToUpper().Contains("DATE")) {
            query.setSortByDate(false, 'S');
        }

        // ## set advanced search parameters ##
        // and query terms
        if (!string.IsNullOrEmpty(txtAdvanced_AndQueryTerms.Text)) {
            query.AndQueryTerms = txtAdvanced_AndQueryTerms.Text.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        }

        // exact phrase
        if (!string.IsNullOrEmpty(txtAdvanced_ExactPhrase.Text)) {
            query.ExactPhraseQueryTerm = txtAdvanced_ExactPhrase.Text;
        }

        // or query terms
        if (!string.IsNullOrEmpty(txtAdvanced_OrQueryTerms.Text)) {
            query.OrQueryTerms = txtAdvanced_OrQueryTerms.Text.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        }

        // exclude query terms
        if (!string.IsNullOrEmpty(txtAdvanced_ExcludeQueryTerms.Text)) {
            query.ExcludedQueryTerms = txtAdvanced_ExcludeQueryTerms.Text;
        }

        // update results per page
        if (!string.IsNullOrEmpty(ddlAdvanced_ResultsPerPage.SelectedValue)) {
            query.MaxResults = Convert.ToInt32(ddlAdvanced_ResultsPerPage.SelectedValue);
        }

        // set language
        query.Language = ddlAdvanced_Language.SelectedValue;

        // set file inclusions/exclusions
        if (!string.IsNullOrEmpty(ddlAdvanced_Filetypes.SelectedValue)) {
            ArrayList fileTypes = new ArrayList();
            fileTypes.Add(ddlAdvanced_Filetypes.SelectedValue);
            query.queryTerm.setIncludeFileType(fileTypes);
        }

        // occurences
        if (!string.IsNullOrEmpty(ddlAdvanced_Occurences.SelectedValue)) {
            switch (ddlAdvanced_Occurences.SelectedValue) {
                case "url":
                    query.SearchScope = SearchRegion.URL.getValue();
                    break;
                case "title":
                    query.SearchScope = SearchRegion.TITLE.getValue();
                    break;
                default:
                    throw new Exception("Invalid occurrence parameter provided. Expected 'url' or 'title.");
            }
        }
    }

    protected void ShowAdvancedOptions(object sender, EventArgs e)
    {
        pnlAdvancedOptions.Visible = true;
        btnShowAdvancedOptions.Visible = false;
    }

    protected void HideAdvancedOptions(object sender, EventArgs e)
    {
        pnlAdvancedOptions.Visible = false;
        btnShowAdvancedOptions.Visible = true;
    }

    protected string RenderCacheDocSize(object input)
    {
        if (input != null && !string.IsNullOrEmpty(input.ToString())) {
            return " - " + input.ToString();
        }

        return string.Empty;
    }

    protected string RenderMoreDetailsUrl(object input)
    {
        string output = string.Empty;
        if (input != null && !string.IsNullOrEmpty(input.ToString())) {
            string siteSearchInitialValue = query.SiteSearch;
            query.SiteSearch = input.ToString();
            output = "[ <a href=\"?" + query.getValue() + "\">More results from " + input + "</a> ]";
            query.SiteSearch = siteSearchInitialValue;
        }

        return output;
    }







    protected string RenderSortLinks()
    {
        string output = string.Empty;
        string initialSortValue = query.Sort;

        if (initialSortValue != null && initialSortValue.ToUpper().Contains("DATE")) {
            query.unsetSortByDate();
            output = "Sort by date / <a href=\"?" + query.getValue() + "\">Sort by relevance</a>";
        } else {
            query.setSortByDate(true, 'S');
            output = "<a href=\"?" + query.getValue() + "\">Sort by date</a> / Sort by relevance";
        }

        // reset the sort to the initial value
        query.Sort = initialSortValue;

        return output;
    }

    protected string RenderPager()
    {
        int pagerBound = 5;
        int pageCount = Convert.ToInt32(Math.Ceiling((decimal)totalResults / (decimal)query.MaxResults));

        // hide the pager if we only have one page
        if (pageCount == 1) {
            return string.Empty;
        }

        int pagerStart = 1;
        int pagerEnd = pageCount;

        // If the number of pages exceeds the pager link limit
        // bound the pager so that the current page is in the 
        // middle of the pager.
        if (pageCount > pagerBound) {
            // set pager start
            if (currentPage - pagerBound > 0) {
                pagerStart = Convert.ToInt32(currentPage - pagerBound);
            } else {
                pagerStart = 1;
            }

            // set pager end
            if (currentPage + pagerBound <= pageCount) {
                pagerEnd = Convert.ToInt32(currentPage + pagerBound);
            } else {
                pagerEnd = pageCount;
            }
        }

        StringBuilder sb = new StringBuilder();
        sb.Append(@"<table class=""gsa-pager"" border=""0"" cellpadding=""3"" align=""center"">");

        // append previous link
        sb.Append(@"<td class=""gsa-pager-previous"">");
        if (currentPage > 1) {
            query.ScrollAhead = (currentPage - 1) * query.MaxResults;
            sb.Append("<a href=\"?" + query.getValue() + "\">Previous</a>");
        }
        sb.Append("</td>");

        // append each pager link
        for (int i = pagerStart; i <= pagerEnd; i++) {
            if (currentPage == i) {
                sb.Append("<td><strong>" + i.ToString() + "</strong></td>");
            } else {
                query.ScrollAhead = (i - 1) * query.MaxResults;
                sb.Append("<td><a href=\"?" + query.getValue() + "\">" + i.ToString() + "</a></td>");
            }
        }

        // append next pager link
        sb.Append(@"<td class=""gsa-pager-next"">");
        if (currentPage < Math.Ceiling((double)totalResults) / (double)query.MaxResults) {
            query.ScrollAhead = (currentPage + 1) * query.MaxResults;
            sb.Append("<a href=\"?" + query.getValue() + "\">Next</a>");
        }
        sb.Append("</td>");

        sb.Append("</table>");
        return sb.ToString();
    }
}
