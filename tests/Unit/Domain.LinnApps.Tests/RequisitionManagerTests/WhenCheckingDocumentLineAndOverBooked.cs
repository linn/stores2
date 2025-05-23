﻿namespace Linn.Stores2.Domain.LinnApps.Tests.RequisitionManagerTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    using FluentAssertions;

    using Linn.Stores2.Domain.LinnApps.Exceptions;
    using Linn.Stores2.Domain.LinnApps.Parts;
    using Linn.Stores2.Domain.LinnApps.Requisitions;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenCheckingDocumentLineAndOverBooked : ContextBase
    {
        private Func<Task> action;

        [SetUp]
        public void SetUp()
        {
            var emp = new Employee { Id = 1, Name = "Jock Bairn" };
            var part = new Part { PartNumber = "KLYDE", Description = "Cartridge" };
            var function = new StoresFunction
                               {
                                   FunctionCode = "CUSTRET",
                                   Description = "RETURN GOODS FROM CUSTOMER TO STOCK/INSPECTION"
                               };

            var requisitions = new List<RequisitionHeader>
                                   {
                                       new RequisitionHeader(
                                           emp,
                                           function,
                                           null,
                                           1,
                                           "C",
                                           null,
                                           null,
                                           part: part,
                                           quantity: 6,
                                           document1Line: 1),
                                       new RequisitionHeader(
                                           emp,
                                           function,
                                           null,
                                           1,
                                           "C",
                                           null,
                                           null,
                                           part: part,
                                           quantity: 3,
                                           document1Line: 1),
                                   };

            var header = new RequisitionHeader(
                emp,
                function,
                null,
                1,
                "C",
                null,
                null,
                part: part,
                quantity: 4,
                document1Line: 1);

            var document = new DocumentResult("C", 1, 1, 1, part.PartNumber);

            this.ReqRepository.FilterByAsync(Arg.Any<Expression<Func<RequisitionHeader, bool>>>())
                .Returns(requisitions);

            this.action = async () => await this.Sut.CheckDocumentLineForOverAndFullyBooked(header, document);
        }

        [Test]
        public async Task ShouldThrow()
        {
            await this.action.Should().ThrowAsync<DocumentException>().WithMessage("Trying to overbook this line");
        }
    }
}
