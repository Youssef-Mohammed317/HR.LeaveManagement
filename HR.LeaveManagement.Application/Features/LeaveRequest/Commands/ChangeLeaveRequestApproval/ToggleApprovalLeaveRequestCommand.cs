using MediatR;

namespace HR.LeaveManagement.Application.Features.LeaveRequest.Commands.ToggleApprovalLeaveRequest;

public class ChangeLeaveRequestApprovalCommand : IRequest
{
    public int Id { get; set; }

    public bool Approved { get; set; }
}
