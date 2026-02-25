using AutoMapper;
using HR.LeaveManagement.Application.Contracts.Presistance;
using HR.LeaveManagement.Application.Exceptions;
using HR.LeaveManagement.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HR.LeaveManagement.Application.Features.LeaveAllocation.Commands.UpdateLeaveAllocation;

public class UpdateLeaveAllocationCommandHandler(ILeaveAllocationRepository leaveAllocationRepository,
    ILeaveRequestRepository leaveRequestRepository,
    IMapper mapper) : IRequestHandler<UpdateLeaveAllocationCommand>
{
    public async Task Handle(UpdateLeaveAllocationCommand request, CancellationToken cancellationToken)
    {
        var entity = await leaveAllocationRepository.GetByIdAsync(request.Id)
            ?? throw new NotFoundException(nameof(Domain.LeaveAllocation), request.Id);

        var approvedDays = await leaveRequestRepository.SumAsync(
            selector: r => EF.Functions.DateDiffDay(r.StartDate, r.EndDate) + 1,
            filter: r => r.RequestingEmployeeId == entity.EmployeeId &&
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
    }
}