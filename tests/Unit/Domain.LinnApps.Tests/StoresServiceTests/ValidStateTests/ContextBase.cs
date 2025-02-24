namespace Linn.Stores2.Domain.LinnApps.Tests.StoresServiceTests.ValidStateTests
{
    using System.Collections.Generic;

    using Linn.Stores2.Domain.LinnApps.Requisitions;
    using Linn.Stores2.Domain.LinnApps.Stores;

    using NUnit.Framework;

    public class ContextBase : StoresServiceContextBase
    {
        public StoresFunction StoresFunction { get; set; }

        public StoresTransactionState Type { get; set; }
        [SetUp]
        public void SetUpContext()
        {
            this.StoresFunction = new StoresFunction
                                      {
                                          FunctionCode = "F1",
                                          TransactionsTypes = new List<StoresFunctionTransaction>
                                                                  {
                                                                      new StoresFunctionTransaction
                                                                          {
                                                                              TransactionCode = "TR1"
                                                                          }
                                                                  }
                                      };
        }
    }
}
