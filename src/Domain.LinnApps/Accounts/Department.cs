namespace Linn.Stores2.Domain.LinnApps.Accounts
{
    using System;

    public class Department
    {
        public string DepartmentCode { get; set; }

        public string Description { get; set; }

        public DateTime? DateClosed { get; set; }

        public string ObsoleteInStores { get; set; }

        public string ProjectDepartment { get; set; }
    }
}
