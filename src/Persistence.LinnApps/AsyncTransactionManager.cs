namespace Linn.Stores2.Persistence.LinnApps
{
    using System.Threading.Tasks;

    using Linn.Common.Persistence;

    using Microsoft.EntityFrameworkCore;

    // todo - delete this file once implemented in Linn.Common.Persistence.EntityFramework
    public class AsyncTransactionManager : ITransactionManager
    {
        private readonly DbContext serviceDbContext;

        public AsyncTransactionManager(DbContext serviceDbContext)
        {
            this.serviceDbContext = serviceDbContext;
        }

        public void Commit()
        {
            this.serviceDbContext.SaveChanges();
        }
        
        public async Task CommitAsync()
        {
            await this.serviceDbContext.SaveChangesAsync();
        }
    }
}
