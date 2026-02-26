using AutoMapper;
using HR.LeaveManagement.Application.Contracts.Presistance;
using HR.LeaveManagement.Application.MappingProfiles;
using HR.LeaveManagement.Application.Tests.Mocks;
using Microsoft.Extensions.Logging;
using Moq;
using Shouldly;

namespace HR.LeaveManagement.Application.Features.LeaveType.Queries.GetAllLeaveTypes;

public class GetAllLeaveTyesQueryHandlerTests
{
    private Mock<ILeaveTypeRepository> mockRepo;
    private IMapper mapper;

    public GetAllLeaveTyesQueryHandlerTests()
    {
        mockRepo = MockLeaveTypeRepository.GetMockeaaveTypeRepository();
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<LeaveTypeProfile>();
        }, LoggerFactory.Create(builder => { }));
        mapper = config.CreateMapper();
    }

    [Fact]
    public async Task Handle_GetAllLeaveTypesQueryHandler_Good_Test()
    {
        // arrange
        var handler = new GetAllLeaveTyesQueryHandler(mockRepo.Object, mapper);

        // action
        var leavesDto = await handler.Handle(new GetAllLeaveTypesQuery(), CancellationToken.None);

        // assert

        leavesDto.ShouldNotBeNull();
        leavesDto.ShouldBeAssignableTo<IReadOnlyList<LeaveTypeDto>>();
        leavesDto.Count.ShouldBe(3);

    }

}