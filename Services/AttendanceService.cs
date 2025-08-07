using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using ShiftManagementFE.DTOs;

namespace ShiftManagementFE.Services
{
    public class AttendanceService
    {
        private readonly HttpClient _http;

        public AttendanceService(HttpClient http)
        {
            _http = http;
        }

        public async Task<List<AttendanceReportDto>?> GetAttendanceReportAsync(int departmentId, string period)
        {
            // Đường dẫn API là tương đối, không có dấu "/" đầu
            var url = $"api/Attendance/report?departmentId={departmentId}&period={period}";
            return await _http.GetFromJsonAsync<List<AttendanceReportDto>>(url);
        }

        public async Task<List<MyAttendanceDto>?> GetMyAttendanceAsync(int userId, string period)
        {
            var url = $"api/Attendance/my?userId={userId}&period={period}";
            return await _http.GetFromJsonAsync<List<MyAttendanceDto>>(url);
        }
    }
}