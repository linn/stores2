namespace Linn.Stores2.Domain.LinnApps.Tests.RequisitionManagerTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    using Linn.Stores2.Domain.LinnApps.Requisitions;

    using NSubstitute;
    using NUnit.Framework;

    public class WhenAddingBookInOrderDetailsAndExisting : ContextBase
    {
        private IList<BookInOrderDetail> bookInOrderDetailsToBeAdded;

        [SetUp]
        public async Task SetUp()
        {
            this.BookInOrderDetailRepository.FilterByAsync(Arg.Any<Expression<Func<BookInOrderDetail, bool>>>())
                .Returns(new List<BookInOrderDetail> { new BookInOrderDetail { Sequence = 3 } });
            this.bookInOrderDetailsToBeAdded = new List<BookInOrderDetail>
                                                   {
                                                       new BookInOrderDetail
                                                           {
                                                               Sequence = 1,
                                                               PartNumber = "SUNDRY",
                                                               DepartmentCode = "1111111111",
                                                               NominalCode = "2222222222",
                                                               Quantity = 3,
                                                               IsReverse = "N"
                                                           },
                                                       new BookInOrderDetail
                                                           {
                                                               Sequence = 2,
                                                               PartNumber = "SUNDRY",
                                                               DepartmentCode = "3333333333",
                                                               NominalCode = "4444444444",
                                                               Quantity = 4,
                                                               IsReverse = "N"
                                                           }
                                                   };
            await this.Sut.AddBookInOrderDetails(this.bookInOrderDetailsToBeAdded);
        }

        [Test]
        public void ShouldCheckForExisting()
        {
            this.BookInOrderDetailRepository.Received()
                .FilterByAsync(Arg.Any<Expression<Func<BookInOrderDetail, bool>>>());
        }

        [Test]
        public void ShouldRemoveExisting()
        {
            this.BookInOrderDetailRepository
                .Received()
                .Remove(Arg.Is<BookInOrderDetail>(m => m.Sequence == 3));
        }

        [Test]
        public void ShouldSaveBookInOrderDetails()
        {
            this.BookInOrderDetailRepository
                .Received(2)
                .AddAsync(Arg.Any<BookInOrderDetail>());
            this.BookInOrderDetailRepository
                .Received()
                .AddAsync(Arg.Is<BookInOrderDetail>(m => m.Sequence == 1));
            this.BookInOrderDetailRepository
                .Received()
                .AddAsync(Arg.Is<BookInOrderDetail>(m => m.Sequence == 2));
        }
    }
}
