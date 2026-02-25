using AutoMapper;
using HR.LeaveManagement.Application.Contracts.Identity;
using HR.LeaveManagement.Application.Contracts.Presistance;
using HR.LeaveManagement.Application.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HR.LeaveManagement.Application.Features.LeaveAllocation.Queries.GetAll.GetAllLeaveAllocations;

public class GetAllLeaveAllocationsQueryHandler(ILeaveAllocationRepository leaveAllocationRepository, IUserService userService,
    IMapper mapper) : IRequestHandler<GetAllLeaveAllocationsQuery, IReadOnlyList<LeaveAllocationDto>>
{
    public async Task<IReadOnlyList<LeaveAllocationDto>> Handle(GetAllLeaveAllocationsQuery request, CancellationToken cancellationToken)
    {
        if (!userService.IsAdmin)
            throw new ForbiddenAccessException();

        var leaveAllocations = await leaveAllocationRepository.GetAsync(
            filter: q => request.EmployeeId == null || q.EmployeeId == request.EmployeeId,
            include: q => q.Include(p => p.LeaveType)
            );

        return mapper.Map<IReadOnlyList<LeaveAllocationDto>>(leaveAllocations);
    }
}
