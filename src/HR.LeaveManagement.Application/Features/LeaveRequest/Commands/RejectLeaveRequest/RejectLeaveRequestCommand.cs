using MediatR;

namespace HR.LeaveManagement.Application.Features.LeaveRequest.Commands.RejectLeaveRequest;

public record RejectLeaveRequestCommand(int Id) : IRequest;
