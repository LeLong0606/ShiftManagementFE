using System.ComponentModel.DataAnnotations;

namespace ShiftManagementFE.DTOs
{
    public class ShiftScheduleDetailCreateDto
    {
        [Required] public int ShiftCodeID { get; set; }
        public string? ShiftType { get; set; }
        public int? WorkUnit { get; set; }
    }
}