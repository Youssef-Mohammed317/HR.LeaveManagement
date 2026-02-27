using HR.LeaveManagement.Application.Contracts.Identity;
using HR.LeaveManagement.Application.Contracts.Presistance;
using HR.LeaveManagement.Application.Exceptions;
using HR.LeaveManagement.Domain.Utility;
using MediatR;

namespace HR.LeaveManagement.Application.Features.LeaveType.Commands.DeleteLeaveType;

public class DeleteLeaveTypeCommandHandler(ILeaveTypeRepository leaveTypeRepository,
    ILeaveRequestRepository leaveRequestRepository, IUserService userService,
    ILeaveAllocationRepository leaveAllocationRepository) : IRequestHandler<DeleteLeaveTypeCommand>
{
    public async Task Handle(DeleteLeaveTypeCommand request, CancellationToken cancellationToken)
    {
        if (!userService.IsAdmin)
            throw new ForbiddenAccessException();

        var entity = await leaveTypeRepository.GetByIdAsync(request.Id)
            ?? throw new NotFoundException(nameof(Domain.LeaveType), request.Id);

        var leaveAllocation = await leaveAllocationRepository.GetFirstAsync(
             x => x.LeaveTypeId == request.Id);

        if (leaveAllocation != null)
            throw new BadRequestException("Cannot delete leave type assigned to employees.");

        var leaveRequest = await leaveRequestRepository.GetFirstAsync(
            x => x.LeaveTypeId == request.Id);

        if (leaveRequest != null)
            throw new BadRequestException("Cannot delete leave type used in leave requests.");


        await leaveTypeRepository.DeleteAsync(entity);
    }
}