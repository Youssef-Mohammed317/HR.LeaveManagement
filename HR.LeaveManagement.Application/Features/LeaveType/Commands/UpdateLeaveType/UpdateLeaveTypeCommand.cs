using AutoMapper;
using FluentValidation;
using HR.LeaveManagement.Application.Contracts.Presistance;
using HR.LeaveManagement.Application.Exceptions;
using MediatR;

namespace HR.LeaveManagement.Application.Features.LeaveType.Commands.UpdateLeaveType;

public record UpdateLeaveTypeCommand(int Id, string Name, int DefaultDays) : IRequest;
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
                       .WithMessage("{PropertyName} must not exceed 70 characters.")
                       .MustAsync(LeaveTypeNameUnique).WithMessage("leave type already exist");

        RuleFor(p => p.DefaultDays)
            .InclusiveBetween(1, 100)
                .WithMessage("{PropertyName} must be between 1 and 100 days.");
        this.leaveTypeRepository = leaveTypeRepository;
    }

    private async Task<bool> LeaveTypeNameUnique(string name, CancellationToken token)
    {
        return await leaveTypeRepository.GetFirstAsync(p => p.Name.ToLower() == name.ToLower()) == null;
    }
}

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