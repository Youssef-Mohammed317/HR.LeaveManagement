using HR.LeaveManagement.Application.Contracts.Email;
using HR.LeaveManagement.Application.Contracts.Identity;
using HR.LeaveManagement.Application.Contracts.Presistance;
using HR.LeaveManagement.Application.Exceptions;
using HR.LeaveManagement.Application.Model.Email;
using HR.LeaveManagement.Domain;
using MediatR;

namespace HR.LeaveManagement.Application.Features.LeaveRequest.Commands.CancelLeaveRequest;

public class CancelLeaveRequestCommandHandler(ILeaveRequestRepository leaveRequestRepository,
    ILeaveAllocationRepository leaveAllocationRepository,
    IUserService userService) : IRequestHandler<CancelLeaveRequestCommand>
{
    public async Task Handle(CancelLeaveRequestCommand request, CancellationToken cancellationToken)
    {
        var userId = userService.UserId
             ?? throw new ForbiddenAccessException();

        var leaveRequest = await leaveRequestRepository.GetByIdAsync(request.Id)
            ?? throw new NotFoundException(nameof(LeaveRequest), request.Id);



        if (!userService.IsAdmin && leaveRequest.EmployeeId != userId)
            throw new ForbiddenAccessException();

        if (leaveRequest.Status == LeaveRequestStatus.Cancelled)
            throw new BadRequestException("Request already cancelled.");

        if (!userService.IsAdmin && leaveRequest.Status != LeaveRequestStatus.Pending)
            throw new BadRequestException("Only pending requests can be canceled.");

        if (userService.IsAdmin && leaveRequest.Status == LeaveRequestStatus.Approved)
        {
            var days = (leaveRequest.EndDate - leaveRequest.StartDate).Days + 1;

            var allocation = await leaveAllocationRepository.GetFirstAsync(
                q => q.EmployeeId == leaveRequest.EmployeeId &&
                     q.LeaveTypeId == leaveRequest.LeaveTypeId &&
                     q.Period == leaveRequest.StartDate.Year)
                ?? throw new NotFoundException("You do not have any allocations for this leave type.");

            allocation.NumberOfDays += days;
            await leaveAllocationRepository.UpdateAsync(allocation);
        }



        leaveRequest.Status = LeaveRequestStatus.Cancelled;
        await leaveRequestRepository.UpdateAsync(leaveRequest);
       
    }
}