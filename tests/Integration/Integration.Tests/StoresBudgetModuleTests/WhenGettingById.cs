// namespace Linn.Stores2.Integration.Tests.StoresBudgetModuleTests
// {
//     using System.Net;
//
//     using FluentAssertions;
//
//     using Linn.Stores2.Domain.LinnApps.Parts;
//     using Linn.Stores2.Domain.LinnApps.Requisitions;
//     using Linn.Stores2.Domain.LinnApps.Stores;
//     using Linn.Stores2.Integration.Tests.Extensions;
//     using Linn.Stores2.Resources.Stores;
//
//     using NUnit.Framework;
//
//     public class WhenGettingById : ContextBase
//     {
//         private StoresBudget budget;
//
//         private int budgetId;
//
//         [SetUp]
//         public void SetUp()
//         {
//             this.budgetId = 234978;
//             this.budget = new StoresBudget
//                               {
//                                   BudgetId = this.budgetId,
//                                   TransactionCode = "STST",
//                                   Requisition = new RequisitionHeader(),
//                                   PartNumber = "P1",
//                                   Part = new Part { PartNumber = "P1" }
//                               };
//             this.DbContext.StoresBudgets.AddAndSave(this.DbContext, this.budget);
//
//             this.Response = this.Client.Get(
//                 $"/stores2/budgets/{this.budgetId}",
//                 with =>
//                     {
//                         with.Accept("application/json");
//                     }).Result;
//         }
//
//         [Test]
//         public void ShouldReturnOk()
//         {
//             this.Response.StatusCode.Should().Be(HttpStatusCode.OK);
//         }
//
//         [Test]
//         public void ShouldReturnJsonContentType()
//         {
//             this.Response.Content.Headers.ContentType.Should().NotBeNull();
//             this.Response.Content.Headers.ContentType?.ToString().Should().Be("application/json");
//         }
//         
//         [Test]
//         public void ShouldReturnJsonBody()
//         {
//             var resource = this.Response.DeserializeBody<StoresBudgetResource>();
//             resource.BudgetId.Should().Be(this.budgetId);
//         }
//     }
// }
