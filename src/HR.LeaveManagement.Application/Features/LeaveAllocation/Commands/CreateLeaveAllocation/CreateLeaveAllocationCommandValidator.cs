using FluentValidation;
using HR.LeaveManagement.Application.Contracts.Presistance;

namespace HR.LeaveManagement.Application.Features.LeaveAllocation.Commands.CreateLeaveAllocation;

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
        return await leaveTypeRepository.ExistsAsync(q => q.Id == id);
    }
}
