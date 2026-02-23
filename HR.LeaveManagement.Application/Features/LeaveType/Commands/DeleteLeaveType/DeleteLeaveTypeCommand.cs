using HR.LeaveManagement.Application.Contracts.Presistance;
using HR.LeaveManagement.Application.Exceptions;
using MediatR;

namespace HR.LeaveManagement.Application.Features.LeaveType.Commands.DeleteLeaveType;

public record DeleteLeaveTypeCommand(int Id, string Name, int DefaultDays) : IRequest;

public class DeleteLeaveTypeCommandHandler(ILeaveTypeRepository leaveTypeRepository) : IRequestHandler<DeleteLeaveTypeCommand>
{
    public async Task Handle(DeleteLeaveTypeCommand request, CancellationToken cancellationToken)
    {

        var entity = await leaveTypeRepository.GetByIdAsync(request.Id)
            ?? throw new NotFoundException(nameof(Domain.LeaveType), request.Id);


        await leaveTypeRepository.DeleteAsync(entity);
    }
}