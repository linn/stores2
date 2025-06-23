namespace Linn.Stores2.Facade.ResourceBuilders
{
    using System.Collections.Generic;
    using System.Linq;

    using Linn.Common.Facade;
    using Linn.Common.Resources;
    using Linn.Stores2.Domain.LinnApps.Accounts;
    using Linn.Stores2.Domain.LinnApps.Stock;
    using Linn.Stores2.Resources;
    using Linn.Stores2.Resources.Accounts;

    public class DepartmentResourceBuilder : IBuilder<Department>
    {
        public DepartmentResource Build(Department department, IEnumerable<string> claims)
        {
            var storageLocationResourceBuilder = new StorageLocationResourceBuilder();

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
            return $"/stores2/department/{model.DepartmentCode}";
        }

        object IBuilder<Department>.Build(Department entity, IEnumerable<string> claims) =>
            this.Build(entity, claims);

        private IEnumerable<LinkResource> BuildLinks(Department model, IEnumerable<string> claims)
        {
            if (model != null)
            {
                yield return new LinkResource { Rel = "self", Href = this.GetLocation(model) };
            }
        }
    }
}
