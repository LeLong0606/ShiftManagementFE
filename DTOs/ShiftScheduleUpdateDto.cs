using System.ComponentModel.DataAnnotations;

namespace ShiftManagementFE.DTOs
{
    public class ShiftScheduleUpdateDto : ShiftScheduleCreateDto
    {
        // thừa kế hết, bạn chỉ cần nhận thêm ScheduleID nếu muốn
        [Required] public int ScheduleID { get; set; }
    }
}