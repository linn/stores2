namespace Linn.Stores2.Facade.ResourceBuilders
{
    using System;
    using System.Collections.Generic;

    using Linn.Common.Facade;
    using Linn.Stores2.Domain.LinnApps.Accounts;
    using Linn.Stores2.Resources.Accounts;

    public class NominalAccountResourceBuilder : IBuilder<NominalAccount>
    {
        public NominalAccountResource Build(NominalAccount nominalAccount, IEnumerable<string> claims)
        {
            return new NominalAccountResource
                       {
                           Id = nominalAccount.Id,
                           DepartmentCode = nominalAccount?.Department.DepartmentCode,
                           Department = nominalAccount.Department == null
                                            ? null
                                            : new DepartmentResource
                                                  {
                                                      Description = nominalAccount.Department.Description,
                                                      DateClosed = nominalAccount.Department.DateClosed?.ToString("o"),
                                                      DepartmentCode = nominalAccount.Department.DepartmentCode,
                                                      ObsoleteInStores = nominalAccount.Department.ObsoleteInStores,
                                                      ProjectDepartment = nominalAccount.Department.ProjectDepartment
                                                  },
                           NominalCode = nominalAccount.Nominal?.NominalCode,
                           Nominal = nominalAccount.Nominal == null
                                         ? null
                                         : new NominalResource
                                               {
                                                   NominalCode = nominalAccount.Nominal.NominalCode,
                                                   Description = nominalAccount.Nominal.Description,
                                                   DateClosed = nominalAccount.Nominal.DateClosed?.ToString("o"),
                                               },
                           StoresPostsAllowed = nominalAccount.StoresPostsAllowed
                       };
        }

        public string GetLocation(NominalAccount model)
        {
            throw new NotImplementedException();
        }

        object IBuilder<NominalAccount>.Build(NominalAccount entity, IEnumerable<string> claims) =>
            this.Build(entity, claims);
    }
}
