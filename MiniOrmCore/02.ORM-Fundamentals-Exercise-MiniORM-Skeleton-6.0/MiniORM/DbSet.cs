using System.Collections;

namespace MiniORM
{
    public class DbSet<TEntity> : ICollection<TEntity> where TEntity : class, new()
    {
        public IList<TEntity> Entities { get; set; }
        internal ChangeTracker<TEntity> ChangeTracker { get; set; }

        public DbSet(IEnumerable<TEntity> entities)
        {
            Entities = entities.ToList();
            ChangeTracker = new ChangeTracker<TEntity>(entities);                
        }

        public int Count => Entities.Count ;

        public bool IsReadOnly => Entities.IsReadOnly;

        public void Add(TEntity item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("Item cannot be null");
            }

            Entities.Add(item);
            ChangeTracker.Add(item);
        }

            public void Clear()
        {
            while (Entities.Any())
            {
                var entity = Entities.First();
                Remove(entity);
            }
        }

        public bool Contains(TEntity item) => Entities.Contains(item);

        public void CopyTo(TEntity[] array, int arrayIndex) => Entities.CopyTo(array, arrayIndex);   
        

        public bool Remove(TEntity item)
        {
            if(item == null)
            {
                throw new ArgumentNullException("Item cannot be null");
            }
            var removedSuccesfully= Entities.Remove(item);

            if (removedSuccesfully)
            {
                ChangeTracker.Remove(item);
            }

            return removedSuccesfully;
        }

        public IEnumerator<TEntity> GetEnumerator() => Entities.GetEnumerator();        

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
