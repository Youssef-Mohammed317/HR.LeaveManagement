using MediatR;

namespace HR.LeaveManagement.Application.Features.LeaveRequest.Commands.DeleteLeaveRequest;

public record DeleteLeaveRequestCommand(int Id) : IRequest;
