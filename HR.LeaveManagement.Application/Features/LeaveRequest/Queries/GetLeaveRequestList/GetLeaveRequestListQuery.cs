using AutoMapper;
using HR.LeaveManagement.Application.Contracts.Identity;
using HR.LeaveManagement.Application.Contracts.Presistance;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HR.LeaveManagement.Application.Features.LeaveRequest.Queries.GetLeaveRequestList;

public record GetLeaveRequestListQuery(bool IsLoggedInUser = false) : IRequest<IReadOnlyList<LeaveRequestListDto>>;

public class GetLeaveRequestsWithDetailsQueryHandler(ILeaveRequestRepository leaveRequestRepository,
    IUserService userService,
    IMapper mapper) : IRequestHandler<GetLeaveRequestListQuery, IReadOnlyList<LeaveRequestListDto>>
{
    public async Task<IReadOnlyList<LeaveRequestListDto>> Handle(GetLeaveRequestListQuery request, CancellationToken cancellationToken)
    {
        if (request.IsLoggedInUser)
        {
            var leaveRequests = await leaveRequestRepository.GetAsync(filter: q => userService.UserId != null && q.RequestingEmployeeId == userService.UserId,
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
