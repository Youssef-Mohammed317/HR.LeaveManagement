using MediatR;

namespace HR.LeaveManagement.Application.Features.LeaveRequest.Commands.ApproveLeaveRequest;

public record ApproveLeaveRequestCommand(int Id) : IRequest;
