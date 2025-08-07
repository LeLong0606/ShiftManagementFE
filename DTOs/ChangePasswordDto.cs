using System.ComponentModel.DataAnnotations;

namespace ShiftManagementFE.DTOs
{
    /// <summary>
    /// DTO dùng cho API đổi mật khẩu.
    /// </summary>
    public class ChangePasswordDto
    {
        public string OldPassword { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
    }
}