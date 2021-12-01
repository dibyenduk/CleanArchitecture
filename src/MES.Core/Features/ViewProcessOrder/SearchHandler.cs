using MediatR;
using MES.Core.Infrastructure;
using MES.Core.Services.Inventory;
using MES.Persistence;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MES.Core.Features.ViewProcessOrder
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
    public class SearchHandler : IRequestHandler<SearchQuery, SearchResponse>
    {
        private readonly MESDbContext dbContext;

        public SearchHandler(MESDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public Task<SearchResponse> Handle(SearchQuery request, CancellationToken cancellationToken)
        {
            var query = this.dbContext.ProcessOrders.Where(t => t.Id == request.Id);

            List<SearchResult> searchResults = query.Select(t => new SearchResult { 
                Id = t.Id, 
                Nbr = t.Nbr,
                CreatedBy = t.CreatedBy,
                CreatedDateTime = t.CreatedDateTime,
                ModifiedBy = t.ModifiedBy,
                ModifiedDateTime = t.ModifiedDateTime
            }).ToList();

            return Task.FromResult(new SearchResponse()
            {
                Total = searchResults.Count(),
                Results = searchResults
            });
        }            
    }
}
