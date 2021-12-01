using MediatR;

namespace MES.Core.Features.Inventory
{
    public class GetDetailsQuery : IRequest<GetDetailsResponse>
    {
        public string MaterialNbr { get; set; }
    }
}
