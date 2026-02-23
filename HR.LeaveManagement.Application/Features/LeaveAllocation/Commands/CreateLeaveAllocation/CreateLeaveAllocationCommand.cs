using AutoMapper;
using FluentValidation;
using HR.LeaveManagement.Application.Contracts.Presistance;
using MediatR;

namespace HR.LeaveManagement.Application.Features.LeaveAllocation.Commands.CreateLeaveAllocation;

public record CreateLeaveAllocationCommand(int LeaveTypeId) : IRequest;

public class CreateLeaveAllocationCommandValidator : AbstractValidator<CreateLeaveAllocationCommand>
{
    private readonly ILeaveTypeRepository leaveTypeRepository;

    public CreateLeaveAllocationCommandValidator(ILeaveTypeRepository leaveTypeRepository)
    {
        this.leaveTypeRepository = leaveTypeRepository;

        RuleFor(p => p.LeaveTypeId)
            .Cascade(CascadeMode.Stop)
            .GreaterThan(0)
            .MustAsync(LeaveTypeMustExist)
            .WithMessage("{PropertyName} does not exist.");
    }

    private async Task<bool> LeaveTypeMustExist(int id, CancellationToken token)
    {
        return await leaveTypeRepository.GetByIdAsync(id) != null;
    }
}

public class CreateLeaveAllocationCommandHandler(ILeaveAllocationRepository leaveAllocationRepository,
    IMapper mapper) : IRequestHandler<CreateLeaveAllocationCommand>
{
    public async Task Handle(CreateLeaveAllocationCommand request, CancellationToken cancellationToken)
    {
   

        var entity = mapper.Map<Domain.LeaveAllocation>(request);

        await leaveAllocationRepository.CreateAsync(entity);
    }
}