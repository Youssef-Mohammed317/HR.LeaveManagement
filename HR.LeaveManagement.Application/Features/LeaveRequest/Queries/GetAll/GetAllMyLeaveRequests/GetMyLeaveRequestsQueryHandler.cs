using AutoMapper;
using HR.LeaveManagement.Application.Contracts.Identity;
using HR.LeaveManagement.Application.Contracts.Presistance;
using HR.LeaveManagement.Application.Features.LeaveRequest.Queries.GetLeaveRequestList;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HR.LeaveManagement.Application.Features.LeaveRequest.Queries.GetAll.GetAllMyLeaveRequests;

public class GetMyLeaveRequestsQueryHandler(ILeaveRequestRepository leaveRequestRepository,
    IUserService userService,
    IMapper mapper)
    : IRequestHandler<GetMyLeaveRequestsQuery, IReadOnlyList<LeaveRequestDto>>
{
    public async Task<IReadOnlyList<LeaveRequestDto>> Handle(GetMyLeaveRequestsQuery request, CancellationToken cancellationToken)
    {
        var leaveRequests = await leaveRequestRepository.GetAsync(
            filter: q => q.EmployeeId == userService.UserId,
            include: q => q.Include(p => p.LeaveType)
        );

        return mapper.Map<IReadOnlyList<LeaveRequestDto>>(leaveRequests);
    }
}