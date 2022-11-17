using System.Reflection;
using Versioning.Models;

namespace Versioning
{
    public interface IEntitiesService
    {
        int? Add(Entity entity);
        Entity? Get(int id);
        bool Update(Entity oldFile, UpdateEntityDto newFile);
    }

    public class EntitiesService : IEntitiesService
    {
        private readonly IDatabase _database;

        public EntitiesService(IDatabase database)
        {
            _database = database;
        }

        public int? Add(Entity entity)
        {
            _database.Add(entity);
            return entity.Id;
        }

        public Entity? Get(int id)
        {
            return _database.Entities.FirstOrDefault(e => e.Id == id);
        }

        public bool Update(Entity oldFile, UpdateEntityDto newFileDto)
        {
            Entity? dbEntry = _database.Entities.FirstOrDefault(e => e.Id == oldFile.Id);

            if (dbEntry == null)
                return false;

            Entity newFile = (Entity) newFileDto;
            PropertyInfo[] properties = newFile.GetType().GetProperties();

            foreach (PropertyInfo property in properties)
            {
                var newValue = property.GetValue(newFile);

                if (newValue == null)
                    continue;

                var oldValue = property.GetValue(oldFile);
                var dbValue = property.GetValue(dbEntry);

                if (dbValue.ToString() != oldValue.ToString()) // Merge conflict
                    return false;

                property.SetValue(dbEntry, newValue);
            }

            dbEntry.Version += 1;
            _database.Update(dbEntry);

            return true;
        }
    }
}
