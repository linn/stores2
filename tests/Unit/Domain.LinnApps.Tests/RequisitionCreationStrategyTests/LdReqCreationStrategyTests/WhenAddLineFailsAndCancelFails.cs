﻿namespace Linn.Stores2.Domain.LinnApps.Tests.RequisitionCreationStrategyTests.LdReqCreationStrategyTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using FluentAssertions;

    using Linn.Stores2.Domain.LinnApps.Accounts;
    using Linn.Stores2.Domain.LinnApps.Exceptions;
    using Linn.Stores2.Domain.LinnApps.Requisitions;
    using Linn.Stores2.Domain.LinnApps.Requisitions.CreationStrategies;
    using Linn.Stores2.TestData.FunctionCodes;

    using NSubstitute;
    using NSubstitute.ExceptionExtensions;

    using NUnit.Framework;

    public class WhenAddLineFailsAndCancelFails : ContextBase
    {
        private Func<Task> action;

        [SetUp]
        public void SetUp()
        {
            var context = new RequisitionCreationContext
            {
                UserPrivileges = new List<string>(),
                PartNumber = null,
                DepartmentCode = "0001234",
                NominalCode = "0004321",
                CreatedByUserNumber = 12345,
                Lines = new List<LineCandidate>
                            {
                                new LineCandidate
                                    {
                                        Qty = 1,
                                        LineNumber = 1,
                                        TransactionDefinition = "DEF",
                                        PartNumber = "PART",
                                    }
                            },
                Function = new StoresFunction("LDREQ")
                {
                    DepartmentNominalRequired = "Y"
                }
            };
            this.EmployeeRepository.FindByIdAsync(context.CreatedByUserNumber).Returns(new Employee());

            this.RequisitionManager.Validate(
                Arg.Any<int>(),
                Arg.Any<string>(),
                Arg.Any<string>(),
                Arg.Any<int?>(),
                Arg.Any<string>(),
                Arg.Any<string>(),
                Arg.Any<string>()).ReturnsForAnyArgs(new RequisitionHeader(
                new Employee(),
                TestFunctionCodes.LinnDeptReq,
                "F",
                null,
                null,
                new Department(),
                new Nominal()));

            this.DepartmentRepository.FindByIdAsync(context.DepartmentCode).Returns(new Department());
            this.NominalRepository.FindByIdAsync(context.NominalCode).Returns(new Nominal());

            this.AuthService.HasPermissionFor(
                    AuthorisedActions.GetRequisitionActionByFunction(context.Function.FunctionCode),
                    Arg.Any<IEnumerable<string>>())
                .Returns(true);
            this.RequisitionManager.AddRequisitionLine(Arg.Any<RequisitionHeader>(), context.Lines.First())
                .Throws(new PickStockException("Can't pick"));

            this.RequisitionManager.CancelHeader(
                Arg.Any<int>(),
                12345,
                Arg.Any<IEnumerable<string>>(),
                Arg.Any<string>(),
                false).Throws(new CancelRequisitionException("Can't cancel either!"));
            this.action = () => this.Sut.Create(context);
        }

        [Test]
        public async Task ShouldThrow()
        {
            await this.action.Should().ThrowAsync<CreateRequisitionException>()
                .WithMessage(
                    "Warning - req failed to create: Can't pick. Header also failed to cancel: Can't cancel either!. Some cleanup may be required!");
        }
    }
}
