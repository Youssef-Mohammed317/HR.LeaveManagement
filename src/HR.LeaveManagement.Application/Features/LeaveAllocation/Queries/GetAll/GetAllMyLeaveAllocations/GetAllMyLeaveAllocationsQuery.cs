using MediatR;

namespace HR.LeaveManagement.Application.Features.LeaveAllocation.Queries.GetAll.GetMyLeaveAllocations;

public class GetAllMyLeaveAllocationsQuery : IRequest<IReadOnlyList<LeaveAllocationDto>>
{
}
