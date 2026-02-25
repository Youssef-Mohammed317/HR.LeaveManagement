using FluentValidation;
using HR.LeaveManagement.Application.Contracts.Presistance;

namespace HR.LeaveManagement.Application.Features.LeaveType.Commands.UpdateLeaveType;

public class UpdateLeaveTypeCommandValidator : AbstractValidator<UpdateLeaveTypeCommand>
{
    private readonly ILeaveTypeRepository leaveTypeRepository;

    public UpdateLeaveTypeCommandValidator(ILeaveTypeRepository leaveTypeRepository)
    {
        RuleFor(p => p.Name)
                   .NotEmpty()
                       .WithMessage("{PropertyName} is required.")
                   .NotNull()
                       .WithMessage("{PropertyName} cannot be null.")
                   .MaximumLength(70)
                       .WithMessage("{PropertyName} must not exceed 70 characters.");
        RuleFor(p => p)
         .MustAsync(LeaveTypeNameUnique)
         .WithMessage("Leave type already exists");

        RuleFor(p => p.DefaultDays)
            .InclusiveBetween(1, 100)
                .WithMessage("{PropertyName} must be between 1 and 100 days.");
        this.leaveTypeRepository = leaveTypeRepository;
    }

    private async Task<bool> LeaveTypeNameUnique(UpdateLeaveTypeCommand command, CancellationToken token)
    {
        return !await leaveTypeRepository.ExsistsAsync(p => p.Name.ToLower() == command.Name.ToLower() && command.Id != p.Id);
    }

}
