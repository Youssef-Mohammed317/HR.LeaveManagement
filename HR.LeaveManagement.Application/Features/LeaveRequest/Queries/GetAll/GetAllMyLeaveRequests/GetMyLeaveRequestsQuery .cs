using HR.LeaveManagement.Application.Features.LeaveRequest.Queries.GetLeaveRequestList;
using MediatR;

namespace HR.LeaveManagement.Application.Features.LeaveRequest.Queries.GetAll.GetAllMyLeaveRequests;

public record GetMyLeaveRequestsQuery
    : IRequest<IReadOnlyList<LeaveRequestDto>>;
