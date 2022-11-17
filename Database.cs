using Versioning.Models;

namespace Versioning
{
    public interface IDatabase
    {
        List<Entity> Entities { get; set; }

        bool Update(Entity entity);
        void Add(Entity entity);
    }

    public class Database : IDatabase
    {
        public List<Entity> Entities { get; set; } = new List<Entity>();

        public bool Update(Entity entity)
        {
            Entity? oldEntry = Entities.Find(e => e.Id == entity.Id);

            if (oldEntry == null)
                return false;

            Entities.Remove(oldEntry);
            Entities.Add(entity);

            return true;
        }

        public void Add(Entity entity)
        {
            entity.Id = Entities.Count + 1;
            Entities.Add(entity);
        }
    }
}
