using AutoMapper;
using HR.LeaveManagement.Application.Contracts.Presistance;
using MediatR;

namespace HR.LeaveManagement.Application.Features.LeaveType.Queries.GetAllLeaveTypes;

public class GetAllLeaveTyesQueryHandler(ILeaveTypeRepository leaveTypeRepository,
    IMapper mapper) : IRequestHandler<GetAllLeaveTypesQuery, IReadOnlyList<LeaveTypeDto>>
{
    public async Task<IReadOnlyList<LeaveTypeDto>> Handle(GetAllLeaveTypesQuery request, CancellationToken cancellationToken)
    {
        var leaveTypesEntites = await leaveTypeRepository.GetAsync();

        return mapper.Map<IReadOnlyList<LeaveTypeDto>>(leaveTypesEntites);
    }
}