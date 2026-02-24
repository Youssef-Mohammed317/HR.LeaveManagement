using HR.LeaveManagement.Application.Features.LeaveRequest.Commands.CancelLeaveRequest;
using HR.LeaveManagement.Application.Features.LeaveRequest.Commands.CreateLeaveRequest;
using HR.LeaveManagement.Application.Features.LeaveRequest.Commands.DeleteLeaveRequest;
using HR.LeaveManagement.Application.Features.LeaveRequest.Commands.ToggleApprovalLeaveRequest;
using HR.LeaveManagement.Application.Features.LeaveRequest.Commands.UpdateLeaveRequest;
using HR.LeaveManagement.Application.Features.LeaveRequest.Queries.GetLeaveRequestDetails;
using HR.LeaveManagement.Application.Features.LeaveRequest.Queries.GetLeaveRequestList;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HR.LeaveManagement.API.Controllers;

[Route("api/[controller]")]
[ApiController]
//[Authorize]
public class LeaveRequestsController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<LeaveRequestListDto>>> GetAll()
    {
        var leaveRequests = await mediator.Send(new GetLeaveRequestListQuery());

        return Ok(leaveRequests);
    }
    [HttpGet("{id:int}")]
    public async Task<ActionResult<IReadOnlyList<LeaveRequestListDto>>> GetById(int id)
    {
        var leaveRequest = await mediator.Send(new GetLeaveRequestDetailsQuery(id));

        return Ok(leaveRequest);
    }
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromBody] CreateLeaveRequestCommand command)
    {
        var id = await mediator.Send(command);

        return CreatedAtAction(nameof(GetById), new { id = id }, id);
    }
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateLeaveRequestCommand command)
    {
        command.Id = id;
        await mediator.Send(command);

        return NoContent();
    }
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        await mediator.Send(new DeleteLeaveRequestCommand(id));

        return NoContent();
    }

    [HttpPut("{id:int}/cancel")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Cancel([FromRoute] int id)
    {
        await mediator.Send(new CancelLeaveRequestCommand(id));

        return NoContent();
    }
    [HttpPut("{id:int}/approval")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> ChangeApproval([FromRoute] int id, [FromBody] ChangeLeaveRequestApprovalCommand command)
    {
        command.Id = id;
        await mediator.Send(command);
        return NoContent();
    }
}
