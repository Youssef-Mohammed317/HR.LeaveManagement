using AutoMapper;
using HR.LeaveManagement.Application.Contracts.Presistance;
using MediatR;

namespace HR.LeaveManagement.Application.Features.LeaveType.Commands.CreateLeaveType;

public class CreateLeaveTypeCommandHandler(ILeaveTypeRepository leaveTypeRepository,
    IMapper mapper) : IRequestHandler<CreateLeaveTypeCommand, int>
{
    public async Task<int> Handle(CreateLeaveTypeCommand request, CancellationToken cancellationToken)
    {
        var entity = mapper.Map<Domain.LeaveType>(request);

        await leaveTypeRepository.CreateAsync(entity);

        return entity.Id;
    }
}