// namespace Linn.Stores2.Domain.LinnApps.Tests.RequisitionTests
// {
//     using System.Linq;
//
//     using FluentAssertions;
//
//     using Linn.Stores2.Domain.LinnApps.Parts;
//     using Linn.Stores2.Domain.LinnApps.Requisitions;
//
//     using NUnit.Framework;
//
//     public class WhenAddingLine : ContextBase
//     {
//         private Part part;
//
//         private StoresTransactionDefinition transactionType;
//
//         [SetUp]
//         public void SetUp()
//         {
//             this.transactionType = new StoresTransactionDefinition { TransactionCode = "T1" };
//             this.part = new Part { PartNumber = "P1" };
//             this.Sut.AddLine(new RequisitionLine(this.ReqNumber, 1, this.transactionType, this.part));
//         }
//
//         [Test]
//         public void ShouldAddLine()
//         {
//             this.Sut.Lines.Should().HaveCount(1);
//             this.Sut.Lines.First().LineNumber.Should().Be(1);
//         }
//     }
// }
