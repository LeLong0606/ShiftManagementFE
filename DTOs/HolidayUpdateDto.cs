﻿namespace ShiftManagementFE.DTOs
{
    public class HolidayUpdateDto
    {
        public int HolidayID { get; set; }
        public DateTime Date { get; set; }
        public int DefaultShiftCodeID { get; set; }
    }
}