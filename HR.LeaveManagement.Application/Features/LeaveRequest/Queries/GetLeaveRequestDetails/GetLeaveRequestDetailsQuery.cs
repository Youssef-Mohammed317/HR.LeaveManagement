
using AutoMapper;
using HR.LeaveManagement.Application.Contracts.Presistance;
using HR.LeaveManagement.Application.Exceptions;
using HR.LeaveManagement.Application.Features.LeaveType.Queries.GetAllLeaveTypes;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace HR.LeaveManagement.Application.Features.LeaveRequest.Queries.GetLeaveRequestDetails;

public record GetLeaveRequestDetailsQuery(int Id) : IRequest<LeaveRequestDetailsDto>;

public class GetLeaveRequestDetailsQueryHandler(ILeaveRequestRepository leaveRequestRepository,
    IMapper mapper) : IRequestHandler<GetLeaveRequestDetailsQuery, LeaveRequestDetailsDto>
{
    public async Task<LeaveRequestDetailsDto> Handle(GetLeaveRequestDetailsQuery request, CancellationToken cancellationToken)
    {
        var entity = await leaveRequestRepository.GetFirstAsync(filter: q => q.Id == request.Id,
            include: q => q.Include(p => p.LeaveType))
            ?? throw new NotFoundException(nameof(Domain.LeaveRequest), request.Id);

        return mapper.Map<LeaveRequestDetailsDto>(entity);

    }
}

public class LeaveRequestDetailsDto
{
    public int Id { get; set; }
    public DateTime? DateCreated { get; set; }
    public DateTime? DateModified { get; set; }
    public string? CreatedBy { get; set; }
    public string? ModifiedBy { get; set; }

    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    public DateTime DateRequested { get; set; }
    public string? RequestComments { get; set; }
    public bool? Approved { get; set; }
    public bool Cancelled { get; set; }

    public string RequestingEmployeeId { get; set; } = string.Empty;

    // Employee nav prop

    public int LeaveTypeId { get; set; }
    public LeaveTypeDto LeaveType { get; set; } = null!;
}