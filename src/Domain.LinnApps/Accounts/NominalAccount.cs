namespace Linn.Stores2.Domain.LinnApps.Accounts
{
    public class NominalAccount
    {
        public int Id { get; set; }
        
        public string DepartmentCode { get; set; }

        public Department Department { get; set; }

        public string NominalCode { get; set; }

        public Nominal Nominal { get; set; }

        public string StoresPostsAllowed { get; set; }
    }
}
