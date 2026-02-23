using AutoMapper;
using FluentValidation;
using HR.LeaveManagement.Application.Contracts.Presistance;
using HR.LeaveManagement.Application.Exceptions;
using MediatR;

namespace HR.LeaveManagement.Application.Features.LeaveAllocation.Commands.UpdateLeaveAllocation;

public record UpdateLeaveAllocationCommand(int Id, int NumberOfDays, int LeaveTypeId, int Period) : IRequest;

public class UpdateLeaveAllocationCommandValidator : AbstractValidator<UpdateLeaveAllocationCommand>
{
    private readonly ILeaveTypeRepository leaveTypeRepository;
    private readonly ILeaveAllocationRepository leaveAllocationRepository;

    public UpdateLeaveAllocationCommandValidator(ILeaveTypeRepository leaveTypeRepository,
        ILeaveAllocationRepository leaveAllocationRepository)
    {
        this.leaveTypeRepository = leaveTypeRepository;
        this.leaveAllocationRepository = leaveAllocationRepository;

        RuleFor(p => p.NumberOfDays)
            .GreaterThan(0)
            .WithMessage("{PropertyName} must greater than {ComparisonValue}");

        RuleFor(p => p.LeaveTypeId)
                   .Cascade(CascadeMode.Stop)
                   .GreaterThan(0)
                   .MustAsync(LeaveTypeMustExist)
                   .WithMessage("{PropertyName} does not exist.");

        RuleFor(p => p.Period)
            .GreaterThanOrEqualTo(DateTime.UtcNow.Year)
            .WithMessage("{PropertyName} must be after {ComparisonValue}");

        RuleFor(p => p.Id)
                 .Cascade(CascadeMode.Stop)
                   .GreaterThan(0).WithMessage("{PropertyName} must greater than {ComparisonValue}")
                   .NotNull()
                   .MustAsync(LeaveAllocationMustExist).WithMessage("{PropertyName} must be present");

    }

    private async Task<bool> LeaveAllocationMustExist(int id, CancellationToken token)
    {
        return await leaveAllocationRepository.GetByIdAsync(id) != null;
    }

    private async Task<bool> LeaveTypeMustExist(int id, CancellationToken token)
    {
        return await leaveTypeRepository.GetByIdAsync(id) != null;
    }
}

public class UpdateLeaveAllocationCommandHandler(ILeaveAllocationRepository leaveAllocationRepository,
    IMapper mapper) : IRequestHandler<UpdateLeaveAllocationCommand>
{
    public async Task Handle(UpdateLeaveAllocationCommand request, CancellationToken cancellationToken)
    {
        var entity = await leaveAllocationRepository.GetByIdAsync(request.Id)
            ?? throw new NotFoundException(nameof(Domain.LeaveAllocation), request.Id);

        mapper.Map(request, entity);

        await leaveAllocationRepository.UpdateAsync(entity);
    }
}