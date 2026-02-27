using AutoMapper;
using HR.LeaveManagement.Application.Contracts.Identity;
using HR.LeaveManagement.Application.Contracts.Presistance;
using HR.LeaveManagement.Application.Exceptions;
using HR.LeaveManagement.Domain.Identity;
using HR.LeaveManagement.Domain.Utility;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HR.LeaveManagement.Application.Features.LeaveAllocation.Queries.GetLeaveAllocationDetails;

public class GetLeaveAllocationDetailQueryHandler(ILeaveAllocationRepository leaveAllocationRepository,
    IUserService userService,
    IMapper mapper) : IRequestHandler<GetLeaveAllocationDetailQuery, LeaveAllocationDetailsDto>
{
    public async Task<LeaveAllocationDetailsDto> Handle(GetLeaveAllocationDetailQuery request, CancellationToken cancellationToken)
    {


        var leaveAllocationEntity = await leaveAllocationRepository.GetFirstAsync(filter: q => q.Id == request.Id,
            include: q => q.Include(p => p.LeaveType))
            ?? throw new NotFoundException(nameof(Domain.LeaveAllocation), request.Id);

        var userId = userService.UserId ??
            throw new ForbiddenAccessException();

        if (!userService.IsAdmin && leaveAllocationEntity.EmployeeId != userId)
        {
            throw new ForbiddenAccessException();
        }

        var employee = await userService.GetEmployeeByIdAsync(leaveAllocationEntity.EmployeeId)
            ??
            throw new NotFoundException(nameof(ApplicationUser), leaveAllocationEntity.EmployeeId);


        var dto = mapper.Map<LeaveAllocationDetailsDto>(leaveAllocationEntity);
        dto.Employee = employee;
        return dto;
    }
}
