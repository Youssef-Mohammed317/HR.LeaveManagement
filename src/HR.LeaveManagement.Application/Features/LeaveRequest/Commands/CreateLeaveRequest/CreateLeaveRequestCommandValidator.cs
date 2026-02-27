using FluentValidation;
using HR.LeaveManagement.Application.Contracts.Presistance;

namespace HR.LeaveManagement.Application.Features.LeaveRequest.Commands.CreateLeaveRequest;

public class CreateLeaveRequestCommandValidator : AbstractValidator<CreateLeaveRequestCommand>
{
    private ILeaveTypeRepository leaveTypeRepository;

    public CreateLeaveRequestCommandValidator(ILeaveTypeRepository leaveTypeRepository)
    {
        this.leaveTypeRepository = leaveTypeRepository;

        RuleFor(p => p.EndDate)
            .GreaterThanOrEqualTo(p => p.StartDate)
            .WithMessage("{PropertyName} must be on or after {ComparisonValue}");

        RuleFor(p => p.StartDate)
            .Must(date => date.Date >= DateTime.UtcNow.Date)
            .WithMessage("{PropertyName} cannot be in the past.");

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
