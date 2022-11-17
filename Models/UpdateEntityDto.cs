namespace Versioning.Models
{
    public class UpdateEntityDto
    {
        public int? Field_1 { get; set; }
        public int? Field_2 { get; set; }
        public List<CustomField>? CustomFields { get; set; }

        public static explicit operator Entity(UpdateEntityDto dto)
        {
            return new Entity
            {
                Id = null,
                Version = null,
                Field_1 = dto.Field_1,
                Field_2 = dto.Field_2,
                CustomFields = dto.CustomFields
            };
        }
    }
}
