﻿using MES.Core.Services.Inventory.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MES.Core.Services.Inventory
{
    public interface IInventoryService
    {
        InventoryDetails[] GetInventory(string materialNbr);
    }
}
