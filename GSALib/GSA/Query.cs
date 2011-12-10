/********************************************************************************
 *  
 *  Product: GSALib
 *  Description: A C# API for accessing the Google Search Appliance.
 *
 *  (c) Copyright 2008 Michael Cizmar + Associates Ltd.  (MC+A)
 *  
********************************************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using GSALib.Utils;
using GSALib.Constants;
using System.Web;

namespace GSALib.GSA
{
    /// <summary>
    /// Class creates Query object that will be used for submitting http requests to GSA
    /// <para>Author Albert Ghukasyan</para>
    /// </summary>
    [Serializable]
    public sealed class Query
    {
        #region Variables

        public static int MAX_RESULTS = 1000;
        public static int MAX_RESULTS_PER_QUERY = 100;
        private QueryBuilder query;
        public QueryTerm queryTerm { get; set; }

        public string[] SiteCollections { get { return query.sites; } set { query.sites = value; } }
        public string Frontend { get { return query.client; } set { query.client = value; }}
        public string OutputFormat { get { return query.output; } set { query.output = value; } }
        public int MaxResults { get { return query.num; } set { query.num = Math.Min(value, MAX_RESULTS); } }
        public byte KeyMatches { get { return query.numgm; } set { query.numgm = value; } }
        public string SearchScope { get { return query.as_occt; } set { query.as_occt = value; } }
        public char Filter { get { return query.filter; } set { query.filter = value; } }
        public string QueryTerm { get { return query.q; } set { query.q = value; } }
        public string[] OrQueryTerms { get { return query.as_oq; } set { query.as_oq = value; } }
        public string[] AndQueryTerms { get { return query.as_q; } set { query.as_q = value; } }
        public string ExactPhraseQueryTerm { get { return query.as_epq; } set { query.as_epq = value; } }
        public string ExcludedQueryTerms { get { return query.as_eq; } set { query.as_eq = value; } }
        public string InputEncoding { get { return query.ie; } set { query.ie = value; } }
        public string OutputEncoding { get { return query.oe; } set { query.oe = value; } }
        public string Language { get { return query.lr; } set { query.lr = value; } }
        public string Sort { get { return query.sort; } set { query.sort = value; } }
        public long ScrollAhead { get { return query.start; } set { query.start = value; } }
        public Access Access { get { return new Access(query.access); } set { query.access = value.getValue(); } }
        public string ProxyCustom { get { return query.proxycustom; } set { query.proxycustom = value; } }
        public string ProxyStylesheet { get { return query.proxystylesheet; } set { query.proxystylesheet = value; } }
        public bool ProxyReload { get { return query.proxyreload; } set { query.proxyreload = value; } }
        public string SiteSearch { get { return query.as_sitesearch; } set { query.as_sitesearch = value; } }

        public Query()
        {
            queryTerm = new QueryTerm();
            query = new QueryBuilder("", null);
            query.setOutput(Output.XML_NO_DTD.getValue());            
        }

        public string[] GetMetaDataFields { get { return query.getfields; } set { query.getfields = value; } }
        public void setRequiredMetaFields(Hashtable requiredFields)
        {
            query.setRequiredfields(requiredFields, true);
        }

        public void setRequiredMetaFields(Hashtable requiredFields, bool orIfTrueAndIfFalse)
        {
            query.setRequiredfields(requiredFields, orIfTrueAndIfFalse);
        }

        public void setPartialMetaFields(Hashtable partialFields)
        {
            query.setPartialfields(partialFields, true);
        }

        public void setPartialMetaFields(Hashtable partialFields, bool orIfTrueAndIfFalse)
        {
            query.setPartialfields(partialFields, orIfTrueAndIfFalse);
        }
        
        public void setSortByDate(bool asc, char mode)
        {
            query.setSort("date:" + (asc
                    ? "A:"
                    : "D:") + mode + ":d1");
        }
        
        public void unsetSortByDate()
        {
            query.setSort(null);
        }

        public String getValue()
        {
            query.setQ(queryTerm.getValue());
            return query.getValue();
        }

        public String getQueryString()
        {
            return queryTerm.getValue();
        }

        /// <summary>
        /// Pass in a GSA querystring and the object will populate the associated properties.
        /// </summary>
        /// <param name="querystring"></param>
        public void Populate(System.Collections.Specialized.NameValueCollection querystring)
        {
            if (querystring["q"] != null) {
                this.queryTerm.Populate(querystring["q"]);
            }

            // get result start index
            this.ScrollAhead = querystring["start"] != null ? Convert.ToInt32(querystring["start"]) : 0;

            // get the sort order
            if (querystring["sort"] != null) {
                this.Sort = querystring["sort"];
            }

            // get any filters
            if (querystring["filter"] != null) {
                this.Filter = GSALib.Constants.Filtering.NO_FILTER.getValue();
            }

            // page-specific search = setAs_sitesearch
            if (querystring["As_SiteSearch"] != null) {
                this.SiteSearch = querystring["As_SiteSearch"];
                this.Filter = GSALib.Constants.Filtering.NO_FILTER.getValue();
            }

            // exact phrase 
            if (querystring["as_epq"] != null) {
                this.ExactPhraseQueryTerm = querystring["as_epq"];
            }

            // exclude terms
            if (querystring["as_eq"] != null) {
                this.ExcludedQueryTerms = querystring["as_eq"];
            }

            // search only in-url or in-title
            if (querystring["as_occt"] != null) {
                switch (querystring["as_occt"]) {
                    case "url":
                        this.SearchScope = SearchRegion.URL.getValue();
                        break;
                    case "title":
                        this.SearchScope = SearchRegion.TITLE.getValue();
                        break;
                    default:
                        throw new Exception("Invalid occurrence parameter provided. Expected 'url' or 'title.");
                }
            }

            // or query terms
            if (querystring["as_oq"] != null) {
                this.OrQueryTerms = querystring["as_oq"].Split(' ');
            }

            // and query terms
            if (querystring["as_q"] != null) {
                this.AndQueryTerms = querystring["as_q"].Split(' ');
            }

            // language
            if (!string.IsNullOrEmpty(querystring["lr"])) {
                this.Language = querystring["lr"];
            }

            // number of results per page
            if (querystring["num"] != null) {
                this.MaxResults = Convert.ToInt32(querystring["num"]);
            }

            // frontend
            if (querystring["client"] != null) {
                this.Frontend = querystring["client"];
            }

            // site
            if (querystring["site"] != null) {
                this.SiteCollections = HttpUtility.UrlDecode(querystring["site"]).Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
            }
        }
        #endregion
    }

    /// <summary>
    /// Class creates QueryTerm object that will be used for submitting http requests to GSA
    /// <para> Class Object should be used to specify additional options of Query</para>
    /// Author Albert Ghukasyan
    /// </summary>
    [Serializable]
    public sealed class QueryTerm
    {
        #region Variables

        /// <summary>
        /// Restricts the results to documents containing that word in the title.
        /// </summary>
        public ArrayList InTitleTerms { get; set; }

        /// <summary>
        /// Restricts the results to documents NOT containing that word in the title.
        /// </summary>
        public ArrayList NotInTitleTerms { get; set; }

        /// <summary>
        /// Restricts the results to documents containing that word in the result URL.
        /// </summary>
        public ArrayList InUrlTerms { get; set; }

        /// <summary>
        /// Restricts the results to documents NOT containing that word in the result URL.
        /// </summary>
        public ArrayList NotInUrlTerms { get; set; }

        /// <summary>
        /// Filters the results to include only documents with the specified file extension.
        /// </summary>
        public ArrayList IncludeFiletype { get; set; }

        /// <summary>
        /// Filters the results to exclude documents with the specified file extension.
        /// </summary>
        public ArrayList ExcludeFiletype { get; set; }

        /// <summary>
        /// Restricts the results to those with all of the query words in the result title.
        /// </summary>
        public ArrayList AllInTitleTerms { get; set; }

        /// <summary>
        /// Restricts the results to those with all of the query words in the result URL.
        /// </summary>
        public ArrayList AllInUrlTerms { get; set; }

        /// <summary>
        /// Lists web pages that have links to the specified web page
        /// </summary>
        public String Site { get; set; }
        public bool IncludeSite { get; set; }

        /// <summary>
        /// Restrict search to documents with modification dates that fall within a time frame.
        /// </summary>
        public String DateRange { get; set; }

        /// <summary>
        /// Returns a single result for the specified URL if the URL exists in the index
        /// </summary>
        public String WebDocLocation { get; set; }

        /// <summary>
        /// Returns the cached HTML version of the specified web document that the Google search crawled.
        /// </summary>
        public String CacheDocLocation { get; set; }

        /// <summary>
        /// Lists web pages that have links to the specified web page.
        /// </summary>
        public String Link { get; set; }

        /// <summary>
        /// The main query terms entered by the user.
        /// </summary>
        public String QueryString { get; set; }

        private const String _IN_TITLE = "intitle:";
        private const String _NOT_IN_TITLE = "-intitle:";
        private const String _IN_URL = "inurl:";
        private const String _NOT_IN_URL = "-inurl:";
        private const String _INCLUDE_FILETYPE = "filetype:";
        private const String _EXCLUDE_FILETYPE = "-filetype:";
        private const String _INCLUDE_SITE = "site:";
        private const String _EXCLUDE_SITE = "-site:";
        private const String _DATERANGE = "daterange:";
        private const String _ALL_IN_TITLE = "allintitle:";
        private const String _ALL_IN_URL = "allinurl:";
        private const String _INFO = "info:";
        private const String _CACHE = "cache:";
        private const String _LINK = "link:";
        private const String _OR = " OR ";
        private const String _SP = " ";
        #endregion

        #region Constructor
        public QueryTerm()
        {
            Init();
        }

        public QueryTerm(string queryString)
        {
            Init();
            this.Populate(queryString);
        }

        private void Init()
        {
            InTitleTerms = new ArrayList();
            NotInTitleTerms = new ArrayList();
            InUrlTerms = new ArrayList();
            NotInUrlTerms = new ArrayList();
            IncludeFiletype = new ArrayList();
            ExcludeFiletype = new ArrayList();
            AllInTitleTerms = new ArrayList();
            AllInUrlTerms = new ArrayList();
            IncludeSite = true;
        }

        /// <summary>
        /// Extracts QueryTerm properties from a querystring.
        /// </summary>
        /// <param name="queryString"></param>
        public void Populate(string queryString)
        {
            // pad the querystring with a space to make our job easier
            queryString += _SP;

            string[] flagArray = new string[] { _NOT_IN_TITLE, _NOT_IN_URL, _EXCLUDE_FILETYPE, _EXCLUDE_SITE, _IN_TITLE, _IN_URL, _INCLUDE_FILETYPE, _INCLUDE_SITE, _CACHE, _INFO, _LINK, _ALL_IN_TITLE, _ALL_IN_URL };
            foreach (string flag in flagArray) {
                // loop through all instances of the flag
                while (queryString.Contains(flag)) {

                    // extract the flag and value from our querystring for further processing
                    int startIndex = queryString.IndexOf(flag);
                    string extract = null;

                    if (flag == _ALL_IN_URL || flag == _ALL_IN_TITLE) {
                        // capture from flag till the next flag or the end of the line
                        int nextFlagIndex = queryString.IndexOfAny(flagArray);
                        extract = queryString.Substring(startIndex, nextFlagIndex != -1 ? nextFlagIndex : queryString.Length - startIndex);
                    } else { // capture till first space
                        extract = queryString.Substring(startIndex, queryString.IndexOf(_SP, startIndex) - startIndex);
                    }

                    // remove the extract from our querystring
                    queryString = queryString.Remove(startIndex, extract.Length);

                    // update the appropriate property
                    string value = extract.Remove(0, flag.Length);
                    switch (flag) {
                        case _IN_TITLE:
                            AllInTitleTerms.Add(value);
                            break;
                        case _NOT_IN_TITLE:
                            NotInTitleTerms.Add(value);
                            break;
                        case _IN_URL:
                            InUrlTerms.Add(value);
                            break;
                        case _NOT_IN_URL:
                            NotInTitleTerms.Add(value);
                            break;
                        case _INCLUDE_FILETYPE:
                            IncludeFiletype.Add(value);
                            break;
                        case _EXCLUDE_FILETYPE:
                            ExcludeFiletype.Add(value);
                            break;
                        case _INCLUDE_SITE:
                            IncludeSite = true;
                            Site = value;
                            break;
                        case _EXCLUDE_SITE:
                            IncludeSite = false;
                            Site = value;
                            break;
                        case _LINK:
                            Link = value;
                            break;
                        case _INFO:
                            WebDocLocation = value;
                            break;
                        case _CACHE:
                            CacheDocLocation = value;
                            break;
                        default:
                            throw new NotImplementedException();
                            break;
                    }
                }
            }

            // anything that is left over is the querystring
            this.QueryString = queryString.Trim();
        }
        #endregion

        #region Get/Set Properties
        public void setQueryString(String queryString)
        {
            this.QueryString = queryString;
        }

        public void setInTitle(ArrayList inTitleTerms)
        {
            this.InTitleTerms = inTitleTerms;
        }

        public void setNotInTitle(ArrayList notInTitleTerms)
        {
            this.NotInTitleTerms = notInTitleTerms;
        }

        public QueryTerm addInTitle(String term, bool include)
        {
            if (include) InTitleTerms.Add(term);
            else NotInTitleTerms.Add(term);
            return this;
        }

        public void setAllInTitle(ArrayList allInTitleTerms)
        {
            this.AllInTitleTerms = allInTitleTerms;
        }
               
        public void setInUrl(ArrayList inUrlTerms)
        {
            this.InUrlTerms = inUrlTerms;
        }

        public void setNotInUrl(ArrayList notInUrlTerms)
        {
            this.NotInUrlTerms = notInUrlTerms;
        }

        public QueryTerm addInUrl(String term, bool include)
        {
            if (include) InUrlTerms.Add(term);
            else NotInUrlTerms.Add(term);
            return this;
        }

        public void setAllInUrl(ArrayList allInUrlTerms)
        {
            this.AllInUrlTerms = allInUrlTerms;
        }

        public void setIncludeFileType(ArrayList filetype)
        {
            this.IncludeFiletype = filetype;
        }

        public void setExcludeFileType(ArrayList filetype)
        {
            this.ExcludeFiletype = filetype;
        }
       
        public QueryTerm addFileType(String term, bool include)
        {
            if (include)
            {
                if (null == IncludeFiletype) IncludeFiletype = new ArrayList();
                IncludeFiletype.Add(term);
            }
            else
            {
                if (null == ExcludeFiletype) ExcludeFiletype = new ArrayList();
                ExcludeFiletype.Add(term);
            }
            return this;
        }

        public void setSite(String site, bool include)
        {
            this.IncludeSite = include;
            this.Site = site;
        }

        public void setWebDocument(String docLocation)
        {
            this.WebDocLocation = docLocation;
        }

        public void setCachedDocument(String docLocation)
        {
            this.CacheDocLocation = docLocation;
        }

        public void setWithLinksTo(String link)
        {
            this.Link = link;
        }

        public void setDateRange(DateTime fromDate, DateTime toDate)
        {
            StringBuilder dateRange = new StringBuilder(fromDate.ToString("YYYY-MM-DD"));
            dateRange.Append("..");
            dateRange.Append(toDate.ToString("YYYY-MM-DD"));
            this.DateRange = dateRange.ToString();
        }
        #endregion

        #region getValue
        public String getValue()
        {
            String retval = null;
            StringBuilder qbuf = new StringBuilder();
            if (AllInTitleTerms != null && AllInTitleTerms.Count > 0) {
                qbuf.Append(_ALL_IN_TITLE).Append(Util.SeparatedString(AllInTitleTerms, null, _SP)).Append(' ');
            }
            if (AllInUrlTerms != null && AllInUrlTerms.Count > 0) {
                qbuf.Append(_ALL_IN_URL).Append(Util.SeparatedString(AllInUrlTerms, null, _SP)).Append(' ');
            }

            if (WebDocLocation != null && WebDocLocation.Length > 0) {
                qbuf.Append(_INFO).Append(WebDocLocation).Append(' ');
            }

            if (CacheDocLocation != null && CacheDocLocation.Length > 0) {
                qbuf.Append(_CACHE).Append(CacheDocLocation).Append(' ');
            }
            if (Link != null && Link.Length > 0) {
                qbuf.Append(_LINK).Append(Link);
            }

            if (InTitleTerms != null && InTitleTerms.Count > 0) {
                qbuf.Append(Util.SeparatedString(InTitleTerms, _IN_TITLE, _SP)).Append(' ');
            }
            if (NotInTitleTerms != null && NotInTitleTerms.Count > 0) {
                qbuf.Append(Util.SeparatedString(NotInTitleTerms, _NOT_IN_TITLE, _SP)).Append(' ');
            }

            if (InUrlTerms != null && InUrlTerms.Count > 0) {
                qbuf.Append(Util.SeparatedString(InUrlTerms, _IN_URL, _SP)).Append(' ');
            }
            if (NotInUrlTerms != null && NotInUrlTerms.Count > 0) {
                qbuf.Append(Util.SeparatedString(NotInUrlTerms, _NOT_IN_URL, _SP)).Append(' ');
            }

            if (Site != null && Site.Length > 0) {
                qbuf.Append(IncludeSite
                        ? _INCLUDE_SITE
                        : _EXCLUDE_SITE).Append(Site).Append(' ');
            }

            if (IncludeFiletype != null && IncludeFiletype.Count > 0) {
                qbuf.Append(Util.SeparatedString(IncludeFiletype, _INCLUDE_FILETYPE, _OR)).Append(' ');
            }
            if (ExcludeFiletype != null && ExcludeFiletype.Count > 0) {
                qbuf.Append(Util.SeparatedString(ExcludeFiletype, _EXCLUDE_FILETYPE, _SP)).Append(' ');
            }
            if (DateRange != null) {
                qbuf.Append(_DATERANGE).Append(DateRange).Append(' ');
            }

            if (QueryString != null) qbuf.Append(QueryString);

            retval = qbuf.ToString();
            return retval;
        }
        #endregion       
    }
}
