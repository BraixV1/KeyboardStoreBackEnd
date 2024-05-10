namespace App.DAL.DTO;

public class PartInBuildWithPart
{
        public Guid Id { get; set; }
        public Guid PartId { get; set; }
        public Part? Part { get; set; }
    
        public Guid KeyboardBuildId { get; set; }
}