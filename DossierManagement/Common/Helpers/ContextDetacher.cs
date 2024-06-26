using Microsoft.EntityFrameworkCore;

namespace DossierManagement.Common.Helpers
{
    public static class ContextDetacher
    {
        public static void DetachAllEntitiesFromContext(DbContext context)
        {
            foreach (var entity in context.ChangeTracker.Entries().ToList())
            {
                if (entity.State != EntityState.Detached)
                {
                    context.Entry(entity.Entity).State = EntityState.Detached;
                }
            }
        }
    }
}
