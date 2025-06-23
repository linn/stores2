namespace Linn.Stores2.Facade.ResourceBuilders
{
    using System.Collections.Generic;

    using Linn.Common.Facade;
    using Linn.Common.Resources;
    using Linn.Stores2.Domain.LinnApps;
    using Linn.Stores2.Resources.External;

    public class EmployeeResourceBuilder : IBuilder<Employee>
{
    public EmployeeResource Build(Employee employee, IEnumerable<string> claims)
    {
        if (employee == null)
        {
            return null;
        }

        return new EmployeeResource
        {
            Id = employee.Id,
            Name = employee.Name,
        };
    }

    public string GetLocation(Employee model)
    {
        return $"/stores2/stock-pools/{model.Id}";
    }

    object IBuilder<Employee>.Build(Employee entity, IEnumerable<string> claims) =>
        this.Build(entity, claims);

    private IEnumerable<LinkResource> BuildLinks(Employee model, IEnumerable<string> claims)
    {
        if (model != null)
        {
            yield return new LinkResource { Rel = "self", Href = this.GetLocation(model) };
        }
    }
}
}
