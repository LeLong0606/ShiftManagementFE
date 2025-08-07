// ShiftManagement.DTOs/StoreUpdateDto.cs
using System.ComponentModel.DataAnnotations;

namespace ShiftManagementFE.DTOs
{
    public class StoreUpdateDto
    {
        public string StoreName { get; set; } = string.Empty;

        public string? Address { get; set; }

        public string? Phone { get; set; }
    }
}