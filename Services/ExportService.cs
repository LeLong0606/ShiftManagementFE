using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using ShiftManagementFE.DTOs;

namespace ShiftManagementFE.Services
{
    public class ExportService
    {
        private readonly HttpClient _http;

        public ExportService(HttpClient http)
        {
            _http = http;
        }

        public async Task<List<AttendanceExportDto>?> GetAttendanceExportDataAsync(int departmentId, string period)
        {
            // Đường dẫn API là tương đối, không có dấu "/" đầu
            var url = $"api/Export/attendance?departmentId={departmentId}&period={period}";
            return await _http.GetFromJsonAsync<List<AttendanceExportDto>>(url);
        }

        public async Task<byte[]?> ExportAttendanceToExcelAsync(List<AttendanceExportDto> dtoData)
        {
            var res = await _http.PostAsJsonAsync("api/Export/attendance/excel", dtoData);
            if (res.IsSuccessStatusCode)
                return await res.Content.ReadAsByteArrayAsync();
            return null;
        }

        public async Task<byte[]?> ExportAttendanceToPdfAsync(List<AttendanceExportDto> dtoData)
        {
            var res = await _http.PostAsJsonAsync("api/Export/attendance/pdf", dtoData);
            if (res.IsSuccessStatusCode)
                return await res.Content.ReadAsByteArrayAsync();
            return null;
        }
    }
}