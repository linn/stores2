namespace Linn.Stores2.Domain.LinnApps.Tests.RequisitionServiceTests
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using FluentAssertions;
    using Linn.Common.Domain;
    using Linn.Stores2.Domain.LinnApps.Accounts;
    using Linn.Stores2.Domain.LinnApps.Requisitions;
    using Linn.Stores2.TestData.Requisitions;
    using NSubstitute;
    using NUnit.Framework;

    public class WhenBooking : ContextBase
    {
        private User user;

        private RequisitionHeader result;

        private Func<Task> action;

        [SetUp]
        public void SetUp()
        {
            this.user = new User
            {
                UserNumber = 33087,
                Privileges = new List<string>()
            };

             var bookedReq = new ReqWithReqNumber(
                123,
                new Employee(),
                new StoresFunction { FunctionCode = "FUNC" },
                "F",
                123,
                "REQ",
                new Department(),
                new Nominal());
             bookedReq.Book(new Employee { Id = 1, Name = "Gabriel García Márquez" });
            this.ReqRepository.FindByIdAsync(bookedReq.ReqNumber).Returns(bookedReq);

            this.AuthService.HasPermissionFor(
                    AuthorisedActions.BookRequisition, Arg.Any<IEnumerable<string>>())
                .Returns(true);

            this.ReqStoredProcedures.DoRequisition(
                123,
                null,
                user.UserNumber).Returns(new ProcessResult(true, "Success"));

            this.result = this.Sut.BookRequisition(123, null, user).Result;
        }

        [Test]
        public void ShouldCallDoRequisition()
        {
            this.ReqStoredProcedures.Received()
                .DoRequisition(
                    123,
                    null,
                    33087);
        }

        [Test]
        public void ShouldReturnBookedReq()
        {
            this.result.ReqNumber.Should().Be(123);
            this.result.BookedBy.Should().NotBeNull();
            this.result.DateBooked.Should().NotBeNull();
        }
    }
}
