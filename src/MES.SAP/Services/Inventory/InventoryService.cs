using MES.Core.Services.Inventory;
using MES.Core.Services.Inventory.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MES.SAP.Services.Inventory
{
    public class InventoryService : IInventoryService
    {
        public InventoryDetails[] GetInventory(string materialNbr)
        {
            return new InventoryRetriever().GetInventory(materialNbr);
        }
    }
}
