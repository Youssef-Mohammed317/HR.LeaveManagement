
using AutoMapper;
using HR.LeaveManagement.Application.Contracts.Identity;
using HR.LeaveManagement.Application.Contracts.Presistance;
using HR.LeaveManagement.Application.Exceptions;
using HR.LeaveManagement.Domain.Utility;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HR.LeaveManagement.Application.Features.LeaveRequest.Queries.GetLeaveRequestDetails;

public class GetLeaveRequestDetailsQueryHandler(ILeaveRequestRepository leaveRequestRepository,
    IUserService userService,
    IMapper mapper) : IRequestHandler<GetLeaveRequestDetailsQuery, LeaveRequestDetailsDto>
{
    public async Task<LeaveRequestDetailsDto> Handle(GetLeaveRequestDetailsQuery request, CancellationToken cancellationToken)
    {
        var entity = await leaveRequestRepository.GetFirstAsync(filter: q => q.Id == request.Id,
            include: q => q.Include(p => p.LeaveType)) ??
            throw new NotFoundException(nameof(Domain.LeaveRequest), request.Id);

        if (!userService.IsAdmin && entity.EmployeeId != userService.UserId)
        {
            throw new ForbiddenAccessException();
        }

        var employee = await userService.GetEmployeeByIdAsync(entity.EmployeeId) ??
            throw new NotFoundException(nameof(Domain.Identity.ApplicationUser), request.Id);


        var dto = mapper.Map<LeaveRequestDetailsDto>(entity);
        dto.Employee = employee;
        return dto;

    }
}
