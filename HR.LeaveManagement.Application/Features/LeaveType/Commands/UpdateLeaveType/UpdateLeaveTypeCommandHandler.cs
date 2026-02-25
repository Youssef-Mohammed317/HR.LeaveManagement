using AutoMapper;
using HR.LeaveManagement.Application.Contracts.Presistance;
using HR.LeaveManagement.Application.Exceptions;
using MediatR;

namespace HR.LeaveManagement.Application.Features.LeaveType.Commands.UpdateLeaveType;

public class UpdateLeaveTypeCommandHandler(ILeaveTypeRepository leaveTypeRepository,
    IMapper mapper) : IRequestHandler<UpdateLeaveTypeCommand>
{
    public async Task Handle(UpdateLeaveTypeCommand request, CancellationToken cancellationToken)
    {

        var entity = await leaveTypeRepository.GetByIdAsync(request.Id)
            ?? throw new NotFoundException(nameof(Domain.LeaveType), request.Id);

        mapper.Map(request, entity);

        await leaveTypeRepository.UpdateAsync(entity);
    }
}