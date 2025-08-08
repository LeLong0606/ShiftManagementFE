using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using ShiftManagementFE.DTOs;

namespace ShiftManagementFE.Services
{
    public class AttendanceService
    {
        private readonly HttpClient _http;
        private readonly ErrorService _errorService;

        public AttendanceService(HttpClient http, ErrorService errorService)
        {
            _http = http;
            _errorService = errorService;
        }

        public async Task<List<AttendanceReportDto>?> GetAttendanceReportAsync(int departmentId, string period)
        {
            var url = $"api/Attendance/report?departmentId={departmentId}&period={period}";
            return await _errorService.TryCatchHttpAsync(
                async () => await _http.GetFromJsonAsync<List<AttendanceReportDto>>(url),
                userMessage: "Không thể tải báo cáo chấm công!"
            );
        }

        public async Task<List<MyAttendanceDto>?> GetMyAttendanceAsync(int userId, string period)
        {
            var url = $"api/Attendance/my?userId={userId}&period={period}";
            return await _errorService.TryCatchHttpAsync(
                async () => await _http.GetFromJsonAsync<List<MyAttendanceDto>>(url),
                userMessage: "Không thể tải dữ liệu chấm công cá nhân!"
            );
        }
    }
}