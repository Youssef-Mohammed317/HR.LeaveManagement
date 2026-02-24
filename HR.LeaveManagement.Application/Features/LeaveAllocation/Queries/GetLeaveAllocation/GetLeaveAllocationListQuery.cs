using AutoMapper;
using HR.LeaveManagement.Application.Contracts.Presistance;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HR.LeaveManagement.Application.Features.LeaveAllocation.Queries.GetLeaveAllocation;

public record GetLeaveAllocationListQuery : IRequest<IReadOnlyList<LeaveAllocationDto>>;

public class GetLeaveAllocationListQueryHandler(ILeaveAllocationRepository leaveAllocationRepository,
    IMapper mapper) : IRequestHandler<GetLeaveAllocationListQuery, IReadOnlyList<LeaveAllocationDto>>
{
    public async Task<IReadOnlyList<LeaveAllocationDto>> Handle(GetLeaveAllocationListQuery request, CancellationToken cancellationToken)
    {
        var leaveAllocations = await leaveAllocationRepository.GetAsync(
            include: q => q.Include(p => p.LeaveType)
            );

        return mapper.Map<IReadOnlyList<LeaveAllocationDto>>(leaveAllocations);
    }
}
