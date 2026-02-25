using HR.LeaveManagement.Application.Contracts.Presistance;
using HR.LeaveManagement.Application.Exceptions;
using MediatR;

namespace HR.LeaveManagement.Application.Features.LeaveType.Commands.DeleteLeaveType;

public class DeleteLeaveTypeCommandHandler(ILeaveTypeRepository leaveTypeRepository,
    ILeaveRequestRepository leaveRequestRepository,
    ILeaveAllocationRepository leaveAllocationRepository) : IRequestHandler<DeleteLeaveTypeCommand>
{
    public async Task Handle(DeleteLeaveTypeCommand request, CancellationToken cancellationToken)
    {

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