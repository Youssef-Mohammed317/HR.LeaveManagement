using HR.LeaveManagement.Application.Features.LeaveRequest.Queries.GetLeaveRequestList;
using MediatR;

namespace HR.LeaveManagement.Application.Features.LeaveRequest.Queries.GetAll.GetAllMyLeaveRequests;

public record GetAllMyLeaveRequestsQuery
    : IRequest<IReadOnlyList<LeaveRequestDto>>;
