
using AutoMapper;
using HR.LeaveManagement.Application.Contracts.Presistance;
using HR.LeaveManagement.Application.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HR.LeaveManagement.Application.Features.LeaveRequest.Queries.GetLeaveRequestDetails;

public record GetLeaveRequestDetailsQuery(int Id) : IRequest<LeaveRequestDetailsDto>;

public class GetLeaveRequestDetailsQueryHandler(ILeaveRequestRepository leaveRequestRepository,
    IMapper mapper) : IRequestHandler<GetLeaveRequestDetailsQuery, LeaveRequestDetailsDto>
{
    public async Task<LeaveRequestDetailsDto> Handle(GetLeaveRequestDetailsQuery request, CancellationToken cancellationToken)
    {
        var entity = await leaveRequestRepository.GetFirstAsync(filter: q => q.Id == request.Id,
            include: q => q.Include(p => p.LeaveType))
            ?? throw new NotFoundException(nameof(Domain.LeaveRequest), request.Id);

        return mapper.Map<LeaveRequestDetailsDto>(entity);

    }
}
