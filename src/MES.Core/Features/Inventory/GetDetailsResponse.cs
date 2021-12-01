using System;
using System.Collections.Generic;

namespace MES.Core.Features.Inventory
{
    public class GetDetailsResponse
    {
        public int Total { get; set; }
        
        public List<GetDetailsResult> Results { get; set; }
    }

    public class GetDetailsResult
    {
        public string MaterialNbr { get; set; }

        public string BatchNbr { get; set; }

        public int Quantity { get; set; }

        public string QuantityUOM { get; set; }
    }
}
