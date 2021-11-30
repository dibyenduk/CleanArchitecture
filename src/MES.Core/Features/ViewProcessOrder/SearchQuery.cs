using MediatR;

namespace MES.Core.Features.ViewProcessOrder
{
    public class SearchQuery : IRequest<SearchResponse>
    {
        public int Id { get; set; }
    }
}
