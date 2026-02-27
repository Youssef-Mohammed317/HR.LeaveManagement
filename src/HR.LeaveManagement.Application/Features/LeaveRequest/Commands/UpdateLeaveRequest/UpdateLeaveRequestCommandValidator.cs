using FluentValidation;
using HR.LeaveManagement.Application.Contracts.Presistance;

namespace HR.LeaveManagement.Application.Features.LeaveRequest.Commands.UpdateLeaveRequest;

public class UpdateLeaveRequestCommandValidator : AbstractValidator<UpdateLeaveRequestCommand>
{
    private ILeaveTypeRepository leaveTypeRepository;
    private readonly ILeaveRequestRepository leaveRequestRepository;

    public UpdateLeaveRequestCommandValidator(ILeaveTypeRepository leaveTypeRepository, ILeaveRequestRepository leaveRequestRepository)
    {
        this.leaveTypeRepository = leaveTypeRepository;
        this.leaveRequestRepository = leaveRequestRepository;
        RuleFor(p => p.Id)
            .Cascade(CascadeMode.Stop)
            .GreaterThan(0)
            .MustAsync(LeaveRequestMustExist)
            .WithMessage("{PropertyName} must be present");

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

    private async Task<bool> LeaveRequestMustExist(int id, CancellationToken token)
    {
        return await leaveRequestRepository.ExistsAsync(q => q.Id == id);
    }

    private async Task<bool> LeaveTypeMustExist(int id, CancellationToken token)
    {
        return await leaveTypeRepository.ExistsAsync(q => q.Id == id);
    }
}
