using HR.LeaveManagement.Application.Features.LeaveRequest.Commands.ApproveLeaveRequest;
using HR.LeaveManagement.Application.Features.LeaveRequest.Commands.CancelLeaveRequest;
using HR.LeaveManagement.Application.Features.LeaveRequest.Commands.CreateLeaveRequest;
using HR.LeaveManagement.Application.Features.LeaveRequest.Commands.DeleteLeaveRequest;
using HR.LeaveManagement.Application.Features.LeaveRequest.Commands.RejectLeaveRequest;
using HR.LeaveManagement.Application.Features.LeaveRequest.Commands.UpdateLeaveRequest;
using HR.LeaveManagement.Application.Features.LeaveRequest.Queries.GetAll.GetAllLeaveRequests;
using HR.LeaveManagement.Application.Features.LeaveRequest.Queries.GetAll.GetAllMyLeaveRequests;
using HR.LeaveManagement.Application.Features.LeaveRequest.Queries.GetLeaveRequestDetails;
using HR.LeaveManagement.Application.Features.LeaveRequest.Queries.GetLeaveRequestList;
using HR.LeaveManagement.Domain.Utility;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HR.LeaveManagement.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class LeaveRequestsController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    [Authorize(Roles = Roles.Administrator)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]

    public async Task<ActionResult<IReadOnlyList<LeaveRequestDto>>> GetAll()
    {
        var leaveRequests = await mediator.Send(new GetAllLeaveRequestsQuery());

        return Ok(leaveRequests);
    }
    [HttpGet("mine")]
    [Authorize(Roles = Roles.Employee)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<LeaveRequestDto>>> GetAllMyLeaveRequests()
    {
        var leaveRequests = await mediator.Send(new GetAllMyLeaveRequestsQuery());

        return Ok(leaveRequests);
    }
    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<LeaveRequestDto>> GetById(int id)
    {
        var leaveRequest = await mediator.Send(new GetLeaveRequestDetailsQuery(id));

        return Ok(leaveRequest);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Create([FromBody] CreateLeaveRequestCommand command)
    {
        var id = await mediator.Send(command);

        return CreatedAtAction(nameof(GetById), new { id = id }, id);
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateLeaveRequestCommand command)
    {
        command.Id = id;
        await mediator.Send(command);

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [Authorize(Roles = Roles.Administrator)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        await mediator.Send(new DeleteLeaveRequestCommand(id));

        return NoContent();
    }

    [HttpPut("{id:int}/cancel")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Cancel([FromRoute] int id)
    {
        await mediator.Send(new CancelLeaveRequestCommand(id));

        return NoContent();
    }
    [HttpPut("{id:int}/approve")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [Authorize(Roles = Roles.Administrator)]
    public async Task<IActionResult> Approve([FromRoute] int id)
    {
        await mediator.Send(new ApproveLeaveRequestCommand(id));
        return NoContent();
    }

    [HttpPut("{id:int}/reject")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [Authorize(Roles = Roles.Administrator)]
    public async Task<IActionResult> Reject([FromRoute] int id)
    {
        await mediator.Send(new RejectLeaveRequestCommand(id));
        return NoContent();
    }
}
