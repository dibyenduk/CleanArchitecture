using System;
using System.Collections.Generic;

namespace MES.Core.Features.ViewProcessOrder
{
    public class SearchResponse
    {
        public int Total { get; set; }
        
        public List<SearchResult> Results { get; set; }
    }

    public class SearchResult
    {
        public int Id { get; set; }

        public string Nbr { get; set; }        

        public DateTime CreatedDateTime { get; set; }

        public string CreatedBy { get; set; }

        public DateTime ModifiedDateTime { get; set; }

        public string ModifiedBy { get; set; }
    }
}
