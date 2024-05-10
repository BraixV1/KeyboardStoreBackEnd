namespace App.DTO.v1_0;

public class PartInBuildWithPart
{
        public Guid Id { get; set; }
        public Guid PartId { get; set; }
        public Part.Part? Part { get; set; }
    
        public Guid KeyboardBuildId { get; set; }
}