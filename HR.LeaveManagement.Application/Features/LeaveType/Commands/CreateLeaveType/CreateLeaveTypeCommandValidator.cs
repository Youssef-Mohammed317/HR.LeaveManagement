using FluentValidation;
using HR.LeaveManagement.Application.Contracts.Presistance;

namespace HR.LeaveManagement.Application.Features.LeaveType.Commands.CreateLeaveType;

public class CreateLeaveTypeCommandValidator : AbstractValidator<CreateLeaveTypeCommand>
{
    private readonly ILeaveTypeRepository leaveTypeRepository;

    public CreateLeaveTypeCommandValidator(ILeaveTypeRepository leaveTypeRepository)
    {
        RuleFor(p => p.Name)
                   .NotEmpty()
                       .WithMessage("{PropertyName} is required.")
                   .NotNull()
                       .WithMessage("{PropertyName} cannot be null.")
                   .MaximumLength(70)
                       .WithMessage("{PropertyName} must not exceed 70 characters.")
                    .MustAsync(LeaveTypeNameUnique)
                       .WithMessage("leave type already exist");

        RuleFor(p => p.DefaultDays)
            .InclusiveBetween(1, 100)
                .WithMessage("{PropertyName} must be between 1 and 100 days.");

        this.leaveTypeRepository = leaveTypeRepository;
    }


    private async Task<bool> LeaveTypeNameUnique(string name, CancellationToken token)
    {
        return !await leaveTypeRepository.ExsistsAsync(p => p.Name.ToLower() == name.ToLower());
    }
}
