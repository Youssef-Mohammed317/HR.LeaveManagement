using MediatR;

namespace HR.LeaveManagement.Application.Features.LeaveRequest.Commands.CancelLeaveRequest;

public record CancelLeaveRequestCommand(int Id) : IRequest;
