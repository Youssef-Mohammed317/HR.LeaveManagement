using FluentValidation;
using HR.LeaveManagement.Application.Contracts.Presistance;

namespace HR.LeaveManagement.Application.Features.LeaveAllocation.Commands.UpdateLeaveAllocation;

public class UpdateLeaveAllocationCommandValidator : AbstractValidator<UpdateLeaveAllocationCommand>
{
    private readonly ILeaveAllocationRepository leaveAllocationRepository;

    public UpdateLeaveAllocationCommandValidator(ILeaveAllocationRepository leaveAllocationRepository)
    {
        this.leaveAllocationRepository = leaveAllocationRepository;

        RuleFor(p => p.NumberOfDays)
            .GreaterThan(0).WithMessage("{PropertyName} must greater than {ComparisonValue}")
                   .LessThanOrEqualTo(100).WithMessage("{PropertyName} must less than {ComparisonValue}");



        RuleFor(p => p.Id)
                 .Cascade(CascadeMode.Stop)

                   .NotNull()
                   .MustAsync(LeaveAllocationMustExist).WithMessage("{PropertyName} must be present");

    }

    private async Task<bool> LeaveAllocationMustExist(int id, CancellationToken token)
    {
        return await leaveAllocationRepository.ExistsAsync(q => q.Id == id);
    }


}
