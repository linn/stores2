namespace Linn.Stores2.Persistence.LinnApps.Repositories
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    using Linn.Common.Persistence;
    using Linn.Stores2.Domain.LinnApps;

    using Microsoft.EntityFrameworkCore;

    public class CountryRepository : IRepository<Country, string>
    {
        private readonly ServiceDbContext dbContext;

        public CountryRepository(ServiceDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        
        public Country FindById(string key)
        {
            throw new NotImplementedException();
        }
        
        public async Task<Country> FindByIdAsync(string key)
        {
            return await this.dbContext.Countries.FirstOrDefaultAsync(x => x.CountryCode == key);
        }
        
        public IQueryable<Country> FindAll()
        {
            return this.dbContext.Countries.AsQueryable();
        }
        
        public void Add(Country entity)
        {
            throw new NotImplementedException();
        }
        
        public async Task AddAsync(Country entity)
        {
            await this.dbContext.AddAsync(entity);
        }
        
        public void Remove(Country entity)
        {
            throw new NotImplementedException();
        }
        
        public Task RemoveAsync(Country entity)
        {
            throw new NotImplementedException();
        }
        
        public Country FindBy(Expression<Func<Country, bool>> expression)
        {
            throw new NotImplementedException();
        }
        
        public Task FindByAsync(Expression<Func<Country, bool>> expression)
        {
            throw new NotImplementedException();
        }
        
        public IQueryable<Country> FilterBy(Expression<Func<Country, bool>> expression)
        {
            throw new NotImplementedException();
        }
    }
}
