using MES.Core.Services.Inventory;
using MES.Core.Services.Inventory.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MES.Core.Tests.Services
{
    public class TestInventoryService : IInventoryService
    {
        public Func<InventoryDetails[]> GetInventoryDetails { get; set; }

        public TestInventoryService(Func<InventoryDetails[]> getInventoryDetails)
        {
            this.GetInventoryDetails = getInventoryDetails;
        }        

        public InventoryDetails[] GetInventory(string materialNbr)
        {
            if (GetInventoryDetails == null)
                return null;

            return GetInventoryDetails();
        }
    }
}
