using HR.LeaveManagement.Application.Contracts.Presistance;
using HR.LeaveManagement.Application.Exceptions;
using MediatR;

namespace HR.LeaveManagement.Application.Features.LeaveAllocation.Commands.DeleteLeaveAllocation;

public record DeleteLeaveAllocationCommand(int Id) : IRequest;

public class DeleteLeaveAllocationCommandHandler(ILeaveAllocationRepository leaveAllocationRepository)
    : IRequestHandler<DeleteLeaveAllocationCommand>
{
    public async Task Handle(DeleteLeaveAllocationCommand request, CancellationToken cancellationToken)
    {
        var entity = await leaveAllocationRepository.GetByIdAsync(request.Id)
            ?? throw new NotFoundException(nameof(Domain.LeaveAllocation), request.Id);

        await leaveAllocationRepository.DeleteAsync(entity);
    }
}