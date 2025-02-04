namespace Linn.Stores2.Domain.LinnApps.Accounts
{
    public class NominalAccount
    {
        public NominalAccount()
        {
                
        }

        public NominalAccount(Department department, Nominal nominal, string storesPostsAllowed)
        {
            this.Department = department;
            this.Nominal = nominal;
            this.StoresPostsAllowed = storesPostsAllowed;
        }

        public int Id { get; set; }
        
        public Department Department { get; set; }

        public Nominal Nominal { get; set; }

        public string StoresPostsAllowed { get; set; }
    }
}
