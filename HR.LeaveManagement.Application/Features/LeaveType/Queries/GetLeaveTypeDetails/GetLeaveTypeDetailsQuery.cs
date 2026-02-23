using AutoMapper;
using HR.LeaveManagement.Application.Contracts.Presistance;
using HR.LeaveManagement.Application.Exceptions;
using MediatR;

namespace HR.LeaveManagement.Application.Features.LeaveType.Queries.GetLeaveTypeDetails;

public record GetLeaveTypeDetailsQuery(int Id) : IRequest<LeaveTypeDetailsDto>;


public class GetLeaveTypeDetailsQueryHandler(ILeaveTypeRepository leaveTypeRepository,
    IMapper mapper) : IRequestHandler<GetLeaveTypeDetailsQuery, LeaveTypeDetailsDto>
{
    public async Task<LeaveTypeDetailsDto> Handle(GetLeaveTypeDetailsQuery request, CancellationToken cancellationToken)
    {
        var leaveTypeEntity = await leaveTypeRepository.GetByIdAsync(request.Id)
            ?? throw new NotFoundException(nameof(Domain.LeaveType), request.Id);


        return mapper.Map<LeaveTypeDetailsDto>(leaveTypeEntity);

    }
}