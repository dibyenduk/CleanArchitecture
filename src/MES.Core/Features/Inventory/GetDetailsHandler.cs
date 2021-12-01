using MediatR;
using MES.Core.Infrastructure;
using MES.Core.Services.Inventory;
using MES.Persistence;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MES.Core.Features.Inventory
{
    /// <summary>
    /// IRequestHandler<,> => Async with result
    /// RequestHandler<,> => Sync with result
    /// IRequestHandler<> => Async with no result => Task/Unit/
    /// AsyncRequestHandler<> => Async with no result => Task
    /// RequestHandler<> => Sync with no result => void
    /// IRequest<T> => Request with response
    /// IRequest => Request with no response
    /// </summary>
    public class GetDetailsHandler : IRequestHandler<GetDetailsQuery, GetDetailsResponse>
    {
        private readonly IInventoryService inventoryService;

        public GetDetailsHandler(IInventoryService inventoryService)
        {            
            this.inventoryService = inventoryService;
        }

        public Task<GetDetailsResponse> Handle(GetDetailsQuery request, CancellationToken cancellationToken)
        {
            var inventoryDetails = this.inventoryService.GetInventory(request.MaterialNbr);

            return Task.FromResult(new GetDetailsResponse()
            {
                Total = inventoryDetails.Count(),
                Results = inventoryDetails.Select(t => new GetDetailsResult()
                {
                    MaterialNbr = t.MaterialNbr,
                    BatchNbr = t.BatchNbr,
                    Quantity = t.Quantity,
                    QuantityUOM = t.QuantityUOM
                }).ToList()
            });
        }            
    }
}
