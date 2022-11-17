namespace Versioning.Models
{
    public class Update
    {
        public Entity OldFile { get; set; }
        public UpdateEntityDto NewFile { get; set; }
    }
}
