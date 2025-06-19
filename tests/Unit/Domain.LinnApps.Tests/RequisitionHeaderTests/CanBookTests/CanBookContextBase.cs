namespace Linn.Stores2.Domain.LinnApps.Tests.RequisitionHeaderTests.CanBookTests
{
    using Linn.Common.Domain;
    using Linn.Stores2.Domain.LinnApps.Accounts;
    using Linn.Stores2.Domain.LinnApps.Requisitions;
    using Linn.Stores2.TestData.FunctionCodes;
    using Linn.Stores2.TestData.NominalAccounts;
    using Linn.Stores2.TestData.Parts;
    using Linn.Stores2.TestData.Transactions;

    using NUnit.Framework;

    public class CanBookContextBase 
    {
        public RequisitionHeader Sut { get; set; }

        public ProcessResult Result { get; set; }

        [SetUp]
        public void SetUpCanBookContext()
        {
            var line = new RequisitionLine(123, 1, TestParts.Cap003, 2, TestTransDefs.StockToLinnDept)
                           {
                               Moves = { new ReqMove(123, 1, 1, 2, 1, null, null, null, null, null) },
                           };
            line.AddPosting("D", 2, TestNominalAccounts.TestNomAcc);
            line.AddPosting("C", 2, TestNominalAccounts.AssetsRawMat);

            this.Sut = new RequisitionHeader(
                new Employee(),
                TestFunctionCodes.LinnDeptReq,
                "F",
                12345678,
                "TYPE",
                new Department(),
                new Nominal(),
                reference: null,
                comments: "A Good Book",
                quantity: 2);

            this.Sut.AddLine(line);
        }
    }
}
