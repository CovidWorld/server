using MediatR;
using Sygic.Corona.Contracts.Responses;

namespace Sygic.Corona.Application.Queries
{
    public class GetAllPresenceChecksQuery : IRequest<GetAllPresenceChecksResponse>
    {
    }
}