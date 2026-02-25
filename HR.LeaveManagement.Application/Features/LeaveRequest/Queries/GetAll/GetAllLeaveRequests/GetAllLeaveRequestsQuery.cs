using HR.LeaveManagement.Application.Features.LeaveRequest.Queries.GetLeaveRequestList;
using MediatR;

namespace HR.LeaveManagement.Application.Features.LeaveRequest.Queries.GetAll.GetAllLeaveRequests;

public record GetAllLeaveRequestsQuery : IRequest<IReadOnlyList<LeaveRequestDto>>;
