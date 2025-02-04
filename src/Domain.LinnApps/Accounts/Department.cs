namespace Linn.Stores2.Domain.LinnApps.Accounts
{
    using System;

    public class Department
    {
        public Department()
        {
        }

        public Department(string code, string description, string obsInStores = "N", string projectDept = "N")
        {
            this.DepartmentCode = code;
            this.Description = description;
            this.ObsoleteInStores = obsInStores;
            this.ProjectDepartment = projectDept;
        }

        public string DepartmentCode { get; set; }

        public string Description { get; set; }

        public DateTime? DateClosed { get; set; }

        public string ObsoleteInStores { get; set; }

        public string ProjectDepartment { get; set; }
    }
}
