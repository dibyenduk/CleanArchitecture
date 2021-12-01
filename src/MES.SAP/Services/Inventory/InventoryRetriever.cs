using MES.Core.Services.Inventory.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MES.SAP.Services.Inventory
{
    public class InventoryRetriever
    {
        public InventoryDetails[] GetInventory(string materialNbr)
        {            
            return new[] { new InventoryDetails() {
                MaterialNbr = materialNbr,
                Quantity = new Random(1).Next(1, 50),
                QuantityUOM = "KG"
            }};
        }
    }
}
