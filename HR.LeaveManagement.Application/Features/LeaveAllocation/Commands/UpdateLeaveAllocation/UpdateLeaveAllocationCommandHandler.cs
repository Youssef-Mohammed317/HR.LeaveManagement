using AutoMapper;
using HR.LeaveManagement.Application.Contracts.Email;
using HR.LeaveManagement.Application.Contracts.Identity;
using HR.LeaveManagement.Application.Contracts.Presistance;
using HR.LeaveManagement.Application.Exceptions;
using HR.LeaveManagement.Application.Model.Email;
using HR.LeaveManagement.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HR.LeaveManagement.Application.Features.LeaveAllocation.Commands.UpdateLeaveAllocation;

public class UpdateLeaveAllocationCommandHandler(ILeaveAllocationRepository leaveAllocationRepository, IEmailSender emailSender,
    ILeaveRequestRepository leaveRequestRepository,
    IUserService userService,
    IMapper mapper) : IRequestHandler<UpdateLeaveAllocationCommand, Unit>
{
    public async Task<Unit> Handle(UpdateLeaveAllocationCommand request, CancellationToken cancellationToken)
    {
        if (!userService.IsAdmin)
            throw new ForbiddenAccessException();

        var entity = await leaveAllocationRepository.GetFirstAsync(filter: f => f.Id == request.Id, include: c => c.Include(c => c.LeaveType))
            ?? throw new NotFoundException(nameof(Domain.LeaveAllocation), request.Id);

        var approvedDays = await leaveRequestRepository.SumAsync(
            selector: r => EF.Functions.DateDiffDay(r.StartDate, r.EndDate) + 1,
            filter: r => r.EmployeeId == entity.EmployeeId &&
                        r.LeaveTypeId == entity.LeaveTypeId &&
                        r.Status == LeaveRequestStatus.Approved &&
                        r.StartDate.Year == entity.Period);

        if (request.NumberOfDays < approvedDays)
        {
            throw new BadRequestException(
                "New allocation cannot be less than already used days.");
        }

        mapper.Map(request, entity);

        await leaveAllocationRepository.UpdateAsync(entity);

        return Unit.Value;
    }
}