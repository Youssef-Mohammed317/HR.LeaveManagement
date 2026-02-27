using AutoMapper;
using HR.LeaveManagement.Application.Contracts.Presistance;
using HR.LeaveManagement.Application.Features.LeaveType.Queries.GetAllLeaveTypes;
using HR.LeaveManagement.Application.Features.LeaveType.Queries.GetLeaveTypeDetails;
using HR.LeaveManagement.Application.MappingProfiles;
using HR.LeaveManagement.Application.Tests.Mocks;
using Microsoft.Extensions.Logging;
using Moq;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace HR.LeaveManagement.Application.Features.LeaveType.Queries.GetLeaveTypeDetails.Tests
{
    public class GetLeaveTypeDetailsQueryHandlerTests
    {
        private Mock<ILeaveTypeRepository> mockRepo;
        private IMapper mapper;

        public GetLeaveTypeDetailsQueryHandlerTests()
        {
            mockRepo = MockLeaveTypeRepository.GetMockeaaveTypeRepository();
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<LeaveTypeProfile>();
            }, LoggerFactory.Create(builder => { }));
            mapper = config.CreateMapper();

        }
        [Fact()]
        public async Task Handle_GetLeaveTypeDetailsQueryHandler_Good_Test()
        {
            // arrange
            var handler = new GetLeaveTypeDetailsQueryHandler(mockRepo.Object, mapper);

            // action
            var leaveDto = await handler.Handle(new GetLeaveTypeDetailsQuery(1), CancellationToken.None);

            // assert

            leaveDto.ShouldNotBeNull();
            leaveDto.ShouldBeOfType<LeaveTypeDetailsDto>();
            leaveDto.Id.ShouldBe(1);
        }
    }
}