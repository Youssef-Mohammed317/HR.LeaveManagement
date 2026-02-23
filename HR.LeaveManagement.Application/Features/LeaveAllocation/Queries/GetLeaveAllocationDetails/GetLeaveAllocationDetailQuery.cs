using AutoMapper;
using HR.LeaveManagement.Application.Contracts.Presistance;
using HR.LeaveManagement.Application.Exceptions;
using HR.LeaveManagement.Application.Features.LeaveType.Queries.GetAllLeaveTypes;
using MediatR;

namespace HR.LeaveManagement.Application.Features.LeaveAllocation.Queries.GetLeaveAllocationDetails;

public record GetLeaveAllocationDetailQuery(int Id) : IRequest<LeaveAllocationDetailsDto>;

public class GetLeaveAllocationDetailQueryHandler(ILeaveAllocationRepository leaveAllocationRepository,
    IMapper mapper) : IRequestHandler<GetLeaveAllocationDetailQuery, LeaveAllocationDetailsDto>
{
    public async Task<LeaveAllocationDetailsDto> Handle(GetLeaveAllocationDetailQuery request, CancellationToken cancellationToken)
    {
        var leaveAllocationEntity = await leaveAllocationRepository.GetByIdAsync(request.Id)
            ?? throw new NotFoundException(nameof(Domain.LeaveAllocation), request.Id);

        return mapper.Map<LeaveAllocationDetailsDto>(leaveAllocationEntity);
    }
}

public class LeaveAllocationDetailsDto
{
    public int Id { get; set; } = default!;
    public DateTime? DateCreated { get; set; }
    public DateTime? DateModified { get; set; }
    public string? CreatedBy { get; set; }
    public string? ModifiedBy { get; set; }
    public int NumberOfDays { get; set; }
    public int Period { get; set; }
    public string EmployeeId { get; set; } = string.Empty;
    // [ForeignKey(nameof(EmployeeId))]
    // Employee prop here
    public int LeaveTypeId { get; set; }
    public LeaveTypeDto LeaveType { get; set; } = null!;
}