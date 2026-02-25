using MediatR;

namespace HR.LeaveManagement.Application.Features.LeaveAllocation.Queries.GetAll.GetAllLeaveAllocations;

public record GetAllLeaveAllocationsQuery(string? EmployeeId = null) : IRequest<IReadOnlyList<LeaveAllocationDto>>;
