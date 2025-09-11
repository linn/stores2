namespace Linn.Stores2.Facade.ResourceBuilders
{
    using System;
    using System.Collections.Generic;

    using Linn.Common.Authorisation;
    using Linn.Common.Facade;
    using Linn.Stores2.Domain.LinnApps.Accounts;
    using Linn.Stores2.Resources.Accounts;

    public class DepartmentResourceBuilder : IBuilder<Department>
    {
        public DepartmentResource Build(Department department, IEnumerable<string> claims)
        {
            if (department == null)
            {
                return null;
            }

            return new DepartmentResource
            {
                DepartmentCode = department.DepartmentCode,
                Description = department.Description,
                DateClosed = department.DateClosed?.ToString("o"),
                ObsoleteInStores = department.ObsoleteInStores,
                ProjectDepartment = department.ProjectDepartment,
            };
        }

        public string GetLocation(Department model)
        {
            throw new NotImplementedException();
        }

        object IBuilder<Department>.Build(Department entity, IEnumerable<string> claims) =>
            this.Build(entity, claims);
    }
}
