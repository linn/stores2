namespace Linn.Stores2.Facade.ResourceBuilders
{
    using System;
    using System.Collections.Generic;

    using Linn.Common.Facade;
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
        throw new NotImplementedException();
        }

    object IBuilder<Employee>.Build(Employee entity, IEnumerable<string> claims) =>
        this.Build(entity, claims);

}
}
