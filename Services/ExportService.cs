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
        private readonly ErrorService _errorService;

        public ExportService(HttpClient http, ErrorService errorService)
        {
            _http = http;
            _errorService = errorService;
        }

        public async Task<List<AttendanceExportDto>?> GetAttendanceExportDataAsync(int departmentId, string period)
        {
            var url = $"api/Export/attendance?departmentId={departmentId}&period={period}";
            return await _errorService.TryCatchHttpAsync(
                async () => await _http.GetFromJsonAsync<List<AttendanceExportDto>>(url),
                userMessage: "Không thể lấy dữ liệu xuất chấm công!"
            );
        }

        public async Task<byte[]?> ExportAttendanceToExcelAsync(List<AttendanceExportDto> dtoData)
        {
            return await _errorService.TryCatchHttpAsync(
                async () =>
                {
                    var res = await _http.PostAsJsonAsync("api/Export/attendance/excel", dtoData);
                    if (res.IsSuccessStatusCode)
                        return await res.Content.ReadAsByteArrayAsync();
                    return null;
                },
                userMessage: "Xuất Excel thất bại!"
            );
        }

        public async Task<byte[]?> ExportAttendanceToPdfAsync(List<AttendanceExportDto> dtoData)
        {
            return await _errorService.TryCatchHttpAsync(
                async () =>
                {
                    var res = await _http.PostAsJsonAsync("api/Export/attendance/pdf", dtoData);
                    if (res.IsSuccessStatusCode)
                        return await res.Content.ReadAsByteArrayAsync();
                    return null;
                },
                userMessage: "Xuất PDF thất bại!"
            );
        }
    }
}