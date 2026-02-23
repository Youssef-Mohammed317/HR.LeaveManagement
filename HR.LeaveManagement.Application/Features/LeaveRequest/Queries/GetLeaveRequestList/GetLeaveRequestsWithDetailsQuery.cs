using AutoMapper;
using HR.LeaveManagement.Application.Contracts.Presistance;
using HR.LeaveManagement.Application.Features.LeaveType.Queries.GetAllLeaveTypes;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HR.LeaveManagement.Application.Features.LeaveRequest.Queries.GetLeaveRequestList;

public record GetLeaveRequestsWithDetailsQuery(bool IsLoggedInUser = false) : IRequest<IReadOnlyList<LeaveRequestListDto>>;

public class GetLeaveRequestsWithDetailsQueryHandler(ILeaveRequestRepository leaveRequestRepository,
    IMapper mapper) : IRequestHandler<GetLeaveRequestsWithDetailsQuery, IReadOnlyList<LeaveRequestListDto>>
{
    public async Task<IReadOnlyList<LeaveRequestListDto>> Handle(GetLeaveRequestsWithDetailsQuery request, CancellationToken cancellationToken)
    {
        if (request.IsLoggedInUser)
        {
            var leaveRequests = await leaveRequestRepository.GetAsync(filter: q => q.RequestingEmployeeId == "userId", // replaced later
              include: q => q.Include(p => p.LeaveType));
            return mapper.Map<IReadOnlyList<LeaveRequestListDto>>(leaveRequests);
        }
        else
        {
            var leaveRequests = await leaveRequestRepository.GetAsync(filter: q => !string.IsNullOrWhiteSpace(q.RequestingEmployeeId),
                include: q => q.Include(p => p.LeaveType));
            return mapper.Map<IReadOnlyList<LeaveRequestListDto>>(leaveRequests);
        }
    }
}

public class LeaveRequestListDto
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
    // Employee prop is here

    public int LeaveTypeId { get; set; }
    public LeaveTypeDto LeaveType { get; set; } = null!;
}