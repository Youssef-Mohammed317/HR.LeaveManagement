using AutoMapper;
using HR.LeaveManagement.Application.Contracts.Presistance;
using HR.LeaveManagement.Application.Features.LeaveType.Queries.GetAllLeaveTypes;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HR.LeaveManagement.Application.Features.LeaveAllocation.Queries.GetLeaveAllocation;

public record GetLeaveAllocationListQuery : IRequest<IReadOnlyList<LeaveAllocationDto>>;

public class GetLeaveAllocationListQueryHandler(ILeaveAllocationRepository leaveAllocationRepository,
    IMapper mapper) : IRequestHandler<GetLeaveAllocationListQuery, IReadOnlyList<LeaveAllocationDto>>
{
    public async Task<IReadOnlyList<LeaveAllocationDto>> Handle(GetLeaveAllocationListQuery request, CancellationToken cancellationToken)
    {
        var leaveAllocations = await leaveAllocationRepository.GetAsync(
            include: q => q.Include(p => p.LeaveType)
            );

        return mapper.Map<IReadOnlyList<LeaveAllocationDto>>(leaveAllocations);
    }
}

public class LeaveAllocationDto
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