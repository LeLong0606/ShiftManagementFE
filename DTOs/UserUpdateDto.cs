using System.ComponentModel.DataAnnotations;

namespace ShiftManagementFE.DTOs
{
    /// <summary>
    /// DTO nhận thông tin cập nhật người dùng.
    /// Dùng cho API cập nhật user.
    /// </summary>
    public class UserUpdateDto
    {
        public string Username { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public int? DepartmentID { get; set; }
        public int? StoreID { get; set; }
        public bool Status { get; set; }
    }
}
