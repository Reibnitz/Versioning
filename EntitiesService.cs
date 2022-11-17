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
            
            dbEntry = UpdateProperties(oldFile, newFile, dbEntry);

            dbEntry.Version += 1;
            _database.Update(dbEntry);

            return true;
        }

        private T UpdateProperties<T>(T oldEntity, T newEntity, T dbEntity)
        {
            PropertyInfo[] properties = newEntity.GetType().GetProperties();

            foreach (PropertyInfo property in properties)
            {
                var newValue = property.GetValue(newEntity);

                if (newValue == null)
                    continue;

                var oldValue = property.GetValue(oldEntity);
                var dbValue = property.GetValue(dbEntity);

                if (dbValue?.ToString() != oldValue?.ToString())
                    throw new Exception($"Merge conflict on {dbEntity.GetType()} - Field: {property.Name}");
                
                bool isList = property.PropertyType.FullName.Contains("Collection");
                bool isPrimitive = newValue.GetType().IsPrimitive || newValue is string;

                if (isList)
                {
                    List<CustomField> newValues = (List<CustomField>) newValue;
                    List<CustomField> oldValues = (List<CustomField>) oldValue;
                    List<CustomField> dbValues = (List<CustomField>) dbValue;

                    foreach(CustomField customField in newValues)
                    {
                        CustomField? dbCustomField = dbValues.Find(cf => cf.Name == customField.Name);

                        if (dbCustomField == null)
                            continue;

                        CustomField oldCustomField = oldValues.Find(cf => cf.Name == customField.Name);

                        if (oldCustomField.Value != dbCustomField.Value)
                            throw new Exception($"Merge conflict on {customField.GetType()} - Field: {customField.Name}");
                    }
                }
                else if (isPrimitive == false)
                    newValue = UpdateProperties<Object>(oldValue, newValue, dbValue);

                property.SetValue(dbEntity, newValue);
            }

            return dbEntity;
        }
    }
}
