
using HR.LeaveManagement.Application.Features.LeaveAllocation.Commands.CreateLeaveAllocation;
using HR.LeaveManagement.Application.Features.LeaveAllocation.Commands.DeleteLeaveAllocation;
using HR.LeaveManagement.Application.Features.LeaveAllocation.Commands.UpdateLeaveAllocation;
using HR.LeaveManagement.Application.Features.LeaveAllocation.Queries.GetAll;
using HR.LeaveManagement.Application.Features.LeaveAllocation.Queries.GetAll.GetAllLeaveAllocations;
using HR.LeaveManagement.Application.Features.LeaveAllocation.Queries.GetAll.GetMyLeaveAllocations;
using HR.LeaveManagement.Application.Features.LeaveAllocation.Queries.GetLeaveAllocationDetails;
using HR.LeaveManagement.Domain.Utility;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HR.LeaveManagement.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class LeaveAllocationsController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    [Authorize(Roles = Roles.Administrator)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<IReadOnlyList<LeaveAllocationDto>>> GetAll([FromQuery] string? employeeId = null)
    {
        var leaveAllocations = await mediator.Send(new GetAllLeaveAllocationsQuery(employeeId));

        return Ok(leaveAllocations);
    }

    [HttpGet("mine")]
    [Authorize(Roles = Roles.Employee)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<LeaveAllocationDto>>> GetAllMyLeaveAllocations()
    {
        var leaveAllocations = await mediator.Send(new GetAllMyLeaveAllocationsQuery());

        return Ok(leaveAllocations);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<LeaveAllocationDetailsDto>> GetById([FromRoute] int id)
    {
        var leaveAllocation = await mediator.Send(new GetLeaveAllocationDetailQuery(id));

        return Ok(leaveAllocation);
    }
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [Authorize(Roles = Roles.Administrator)]
    public async Task<IActionResult> Create([FromBody] CreateLeaveAllocationCommand command)
    {
        var id = await mediator.Send(command);

        return CreatedAtAction(nameof(GetById), new { id = id }, id);
    }
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [Authorize(Roles = Roles.Administrator)]
    public async Task<IActionResult> Edit([FromRoute] int id, [FromBody] UpdateLeaveAllocationCommand command)
    {
        command.Id = id;
        await mediator.Send(command);

        return NoContent();
    }
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [Authorize(Roles = Roles.Administrator)]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        await mediator.Send(new DeleteLeaveAllocationCommand(id));

        return NoContent();
    }
}
