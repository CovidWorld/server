﻿using System.Collections.Generic;
using MediatR;
using Sygic.Corona.Contracts.Responses;

namespace Sygic.Corona.Application.Queries
{
    public class GetQuarantineListQuery : IRequest<IEnumerable<GetQuarantineListResponse>>
    {
    }
}
