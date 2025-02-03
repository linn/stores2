namespace Linn.Stores2.Integration.Tests.Extensions
{
    using Microsoft.EntityFrameworkCore;

    public static class DbSetExtensions
    {
        public static void AddAndSave<T>(
            this DbSet<T> set, DbContext context, T entity) where T : class
        {
            set.Add(entity);




            context.SaveChanges();
        }

        public static void RemoveAllAndSave<T>(
            this DbSet<T> set, DbContext context) where T : class
        {
            set.RemoveRange(set);
            context.SaveChanges();
        }
    }
}
