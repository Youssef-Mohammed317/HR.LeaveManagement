using AutoMapper;
using HR.LeaveManagement.Application.Contracts.Identity;
using HR.LeaveManagement.Application.Contracts.Presistance;
using HR.LeaveManagement.Application.Exceptions;
using HR.LeaveManagement.Application.Features.LeaveRequest.Queries.GetLeaveRequestList;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HR.LeaveManagement.Application.Features.LeaveRequest.Queries.GetAll.GetAllLeaveRequests;

public class GetAllLeaveRequestsQueryHandler(ILeaveRequestRepository leaveRequestRepository,IUserService userService,
    IMapper mapper) : IRequestHandler<GetAllLeaveRequestsQuery, IReadOnlyList<LeaveRequestDto>>
{
    public async Task<IReadOnlyList<LeaveRequestDto>> Handle(GetAllLeaveRequestsQuery request, CancellationToken cancellationToken)
    {
        if (!userService.IsAdmin)
            throw new ForbiddenAccessException();

        var leaveRequests = await leaveRequestRepository.GetAsync(
            include: q => q.Include(p => p.LeaveType));

        return mapper.Map<IReadOnlyList<LeaveRequestDto>>(leaveRequests);
    }
}
