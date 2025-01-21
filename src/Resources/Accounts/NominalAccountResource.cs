namespace Linn.Stores2.Resources.Accounts
{
    public class NominalAccountResource
    {
        public int Id { get; set; }
        
        public string DepartmentCode { get; set; }

        public DepartmentResource Department { get; set; }

        public string NominalCode { get; set; }

        public NominalResource Nominal { get; set; }

        public string StoresPostsAllowed { get; set; }
    }
}
