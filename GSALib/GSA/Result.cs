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

namespace GSALib.GSA
{
    /// <summary>
    /// Class for creating object that contains resuls of Request
    /// <para>Author Albert Ghukasyan</para>
    /// </summary>
    [Serializable]
    public sealed class Result
    {
        #region Variables

        public int Index { get; set; }
        public String MimeType { get; set; }
        public int Indentation { get; set; }
        public String Url { get; set; }
        public String EscapedUrl { get; set; }
        public String Title { get; set; }
        public int Rating { get; set; }
        public Dictionary<string, string> Metas { get; set; }
        public Dictionary<string, string> Fields { get; set; }
        public String Summary { get; set; }
        public String Language { get; set; }
        public String CacheDocId { get; set; }
        public String CacheDocEncoding { get; set; }
        public String CacheDocSize { get; set; }
        public String CrawelDate { get; set; }
        public string MoreDetailsUrl { get; set; }
        #endregion

        #region Constructor

        internal Result()
        {
            Metas = new Dictionary<string, string>();
            Fields = new Dictionary<string, string>();
        }

        #endregion

        #region ToString

        public override string ToString()
        {
            StringBuilder buffer = new StringBuilder();

            buffer.Append("GSAResult[");
            buffer.Append("mimeType = ").Append(MimeType);
            buffer.Append(", indentation = ").Append(Indentation);
            buffer.Append(", url = ").Append(Url);
            buffer.Append(", escapedUrl = ").Append(EscapedUrl);
            buffer.Append(", title = ").Append(Title);
            buffer.Append(", rating = ").Append(Rating);
            buffer.Append(", metas = ").Append(Metas);
            buffer.Append(", fields = ").Append(Fields);
            buffer.Append(", summary = ").Append(Summary);
            buffer.Append(", language = ").Append(Language);
            buffer.Append(", cacheDocId = ").Append(CacheDocId);
            buffer.Append(", cacheDocEncoding = ").Append(CacheDocEncoding);
            buffer.Append(", cacheDocSize = ").Append(CacheDocSize);
            buffer.Append("]");

            return buffer.ToString();
        }

        #endregion
    }
}