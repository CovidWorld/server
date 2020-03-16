using System.Collections.Generic;
using MediatR;
using Sygic.Corona.Domain;

namespace Sygic.Corona.Application.Queries
{
    public class GetProfilesInQuarantineQuery : IRequest<IEnumerable<Profile>>
    {
    }
}
