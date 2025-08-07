﻿namespace ShiftManagementFE.DTOs
{
    public class RegisterDto
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public int? DepartmentID { get; set; }
        public int? StoreID { get; set; }
    }
}