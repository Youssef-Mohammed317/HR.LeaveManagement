using HR.LeaveManagement.Application.Contracts.Identity;
using HR.LeaveManagement.Application.Contracts.Presistance;
using HR.LeaveManagement.Application.Exceptions;
using HR.LeaveManagement.Domain;
using MediatR;

namespace HR.LeaveManagement.Application.Features.LeaveAllocation.Commands.DeleteLeaveAllocation;

public class DeleteLeaveAllocationCommandHandler(ILeaveAllocationRepository leaveAllocationRepository,
    IUserService userService,
    ILeaveRequestRepository leaveRequestRepository)
    : IRequestHandler<DeleteLeaveAllocationCommand>
{
    public async Task Handle(DeleteLeaveAllocationCommand request, CancellationToken cancellationToken)
    {
        if (!userService.IsAdmin)
            throw new ForbiddenAccessException();

        var entity = await leaveAllocationRepository.GetByIdAsync(request.Id)
            ?? throw new NotFoundException(nameof(Domain.LeaveAllocation), request.Id);


        var hasHeaveRequest = await leaveRequestRepository.ExistsAsync(filter:
         r => r.EmployeeId == entity.EmployeeId
         && r.LeaveTypeId == entity.LeaveTypeId
         && r.Status == LeaveRequestStatus.Approved);


        if (hasHeaveRequest)
        {
            throw new BadRequestException("Cannot delete allocation with existing approve leave requests");
        }

        await leaveAllocationRepository.DeleteAsync(entity);
    }
}