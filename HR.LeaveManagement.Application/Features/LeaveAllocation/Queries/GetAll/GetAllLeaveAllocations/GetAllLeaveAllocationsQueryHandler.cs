using AutoMapper;
using HR.LeaveManagement.Application.Contracts.Presistance;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HR.LeaveManagement.Application.Features.LeaveAllocation.Queries.GetAll.GetAllLeaveAllocations;

public class GetAllLeaveAllocationsQueryHandler(ILeaveAllocationRepository leaveAllocationRepository,
    IMapper mapper) : IRequestHandler<GetAllLeaveAllocationsQuery, IReadOnlyList<LeaveAllocationDto>>
{
    public async Task<IReadOnlyList<LeaveAllocationDto>> Handle(GetAllLeaveAllocationsQuery request, CancellationToken cancellationToken)
    {

        var leaveAllocations = await leaveAllocationRepository.GetAsync(
            filter: q => request.EmployeeId == null || q.EmployeeId == request.EmployeeId,
            include: q => q.Include(p => p.LeaveType)
            );

        return mapper.Map<IReadOnlyList<LeaveAllocationDto>>(leaveAllocations);
    }
}
