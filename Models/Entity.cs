namespace Versioning.Models
{
    public class Entity
    {
        public int? Id { get; set; }
        public int? Version { get; set; }
        public int? Field_1 { get; set; }
        public int? Field_2 { get; set; }
        public List<CustomField>? CustomFields { get; set; }
    }
}
