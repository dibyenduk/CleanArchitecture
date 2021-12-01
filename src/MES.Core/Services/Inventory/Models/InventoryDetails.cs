using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MES.Core.Services.Inventory.Models
{
    public class InventoryDetails
    {
        public string MaterialNbr { get; set; }

        public string BatchNbr { get; set; }

        public int  Quantity { get; set; }

        public string QuantityUOM { get; set; }
    }
}
