using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Linn.Stores2.Domain.LinnApps.Accounts;
using Linn.Stores2.Domain.LinnApps.Requisitions;
using Linn.Stores2.TestData.FunctionCodes;
using Linn.Stores2.TestData.Requisitions;
using Linn.Stores2.TestData.Transactions;
using NUnit.Framework;

namespace Linn.Stores2.Domain.LinnApps.Tests.RequisitionHeaderTests
{
    public class WhenCheckingIsReverse 
    {
        private RequisitionHeader sut;

        [SetUp]
        public void SetUp()
        {
            this.sut = new RequisitionHeader(
                new Employee(),
                TestFunctionCodes.ReturnToSupplier,
                null,
                12345678,
                "RO",
                null,
                null,
                reference: null,
                comments: "Uno reverse",
                quantity: 1,
                originalReqNumber: 1234,
                isReverseTrans: "Y");
        }

        [Test]
        public void ShouldBeReverse()
        {
            this.sut.IsReverseTrans().Should().BeTrue();
        }
    }
}
