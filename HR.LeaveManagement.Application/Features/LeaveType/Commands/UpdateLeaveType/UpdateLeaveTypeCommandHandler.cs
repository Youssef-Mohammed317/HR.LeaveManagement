using AutoMapper;
using HR.LeaveManagement.Application.Contracts.Identity;
using HR.LeaveManagement.Application.Contracts.Presistance;
using HR.LeaveManagement.Application.Exceptions;
using MediatR;

namespace HR.LeaveManagement.Application.Features.LeaveType.Commands.UpdateLeaveType;

public class UpdateLeaveTypeCommandHandler(ILeaveTypeRepository leaveTypeRepository, IUserService userService,
    IMapper mapper) : IRequestHandler<UpdateLeaveTypeCommand, Unit>
{
    public async Task<Unit> Handle(UpdateLeaveTypeCommand request, CancellationToken cancellationToken)
    {
        if (!userService.IsAdmin)
            throw new ForbiddenAccessException();

        var entity = await leaveTypeRepository.GetByIdAsync(request.Id)
            ?? throw new NotFoundException(nameof(Domain.LeaveType), request.Id);

        mapper.Map(request, entity);

        await leaveTypeRepository.UpdateAsync(entity);

        return Unit.Value;
    }
}