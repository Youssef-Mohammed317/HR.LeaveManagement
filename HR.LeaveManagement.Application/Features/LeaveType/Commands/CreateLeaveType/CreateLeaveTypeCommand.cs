using AutoMapper;
using FluentValidation;
using HR.LeaveManagement.Application.Contracts.Presistance;
using MediatR;

namespace HR.LeaveManagement.Application.Features.LeaveType.Commands.CreateLeaveType;

public record CreateLeaveTypeCommand(string Name, int DefaultDays) : IRequest<int>;

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