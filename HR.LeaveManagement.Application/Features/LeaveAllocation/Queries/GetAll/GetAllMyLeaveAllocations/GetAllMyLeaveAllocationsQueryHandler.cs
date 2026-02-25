using AutoMapper;
using HR.LeaveManagement.Application.Contracts.Identity;
using HR.LeaveManagement.Application.Contracts.Presistance;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HR.LeaveManagement.Application.Features.LeaveAllocation.Queries.GetAll.GetMyLeaveAllocations;

public class GetAllMyLeaveAllocationsQueryHandler(ILeaveAllocationRepository leaveAllocationRepository,
    IUserService userService,
    IMapper mapper) : IRequestHandler<GetAllMyLeaveAllocationsQuery, IReadOnlyList<LeaveAllocationDto>>
{
    public async Task<IReadOnlyList<LeaveAllocationDto>> Handle(GetAllMyLeaveAllocationsQuery request, CancellationToken cancellationToken)
    {
        var userId = userService.UserId;

        if (string.IsNullOrEmpty(userId))
            throw new UnauthorizedAccessException("User is not authenticated");

        var leaveAllocations = await leaveAllocationRepository.GetAsync(
                   filter: q => q.EmployeeId == userId,
                   include: q => q.Include(p => p.LeaveType)
               );

        return mapper.Map<IReadOnlyList<LeaveAllocationDto>>(leaveAllocations);
    }
}
