using HR.LeaveManagement.Application.Contracts.Email;
using HR.LeaveManagement.Application.Contracts.Identity;
using HR.LeaveManagement.Application.Contracts.Presistance;
using HR.LeaveManagement.Application.Exceptions;
using HR.LeaveManagement.Application.Model.Email;
using HR.LeaveManagement.Domain;
using MediatR;

namespace HR.LeaveManagement.Application.Features.LeaveRequest.Commands.RejectLeaveRequest;

public class RejectLeaveRequestCommandHandler(ILeaveRequestRepository leaveRequestRepository,
    IUserService userService) : IRequestHandler<RejectLeaveRequestCommand>
{
    public async Task Handle(RejectLeaveRequestCommand request, CancellationToken cancellationToken)
    {
        if (!userService.IsAdmin)
            throw new ForbiddenAccessException();

        var leaveRequest = await leaveRequestRepository.GetByIdAsync(request.Id)
            ?? throw new NotFoundException(nameof(LeaveRequest), request.Id);


        if (leaveRequest.Status != LeaveRequestStatus.Pending)
            throw new BadRequestException("Only pending requests can be rejected.");


        leaveRequest.Status = LeaveRequestStatus.Rejected;

        await leaveRequestRepository.UpdateAsync(leaveRequest);
    }
}