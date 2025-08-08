using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using ShiftManagementFE.DTOs;
using System;

namespace ShiftManagementFE.Services
{
    public class LogService
    {
        private readonly HttpClient _http;
        private readonly ErrorService _errorService;

        public LogService(HttpClient http, ErrorService errorService)
        {
            _http = http;
            _errorService = errorService;
        }

        public async Task<List<LogDto>?> GetLogsAsync(
            string? search = null,
            int? userId = null,
            DateTime? from = null,
            DateTime? to = null,
            int page = 1,
            int pageSize = 50)
        {
            var url = $"api/Logs?page={page}&pageSize={pageSize}";
            var parameters = new List<string>();
            if (!string.IsNullOrWhiteSpace(search)) parameters.Add($"search={search}");
            if (userId.HasValue) parameters.Add($"userId={userId.Value}");
            if (from.HasValue) parameters.Add($"from={from.Value:O}");
            if (to.HasValue) parameters.Add($"to={to.Value:O}");
            if (parameters.Count > 0) url += "&" + string.Join("&", parameters);

            return await _errorService.TryCatchHttpAsync(
                async () => await _http.GetFromJsonAsync<List<LogDto>>(url),
                userMessage: "Không thể tải danh sách log!"
            );
        }

        public async Task<LogDto?> GetLogAsync(int id)
        {
            return await _errorService.TryCatchHttpAsync(
                async () => await _http.GetFromJsonAsync<LogDto>($"api/Logs/{id}"),
                userMessage: "Không thể tải chi tiết log!"
            );
        }

        public async Task<ApiResult<LogDto>> CreateLogAsync(LogCreateDto dto)
        {
            return await _errorService.TryCatchHttpAsync(
                async () =>
                {
                    var res = await _http.PostAsJsonAsync("api/Logs", dto);
                    if (res.IsSuccessStatusCode)
                    {
                        var data = await res.Content.ReadFromJsonAsync<LogDto>();
                        return new ApiResult<LogDto>(true, data!, null);
                    }
                    var err = await res.Content.ReadAsStringAsync();
                    return new ApiResult<LogDto>(false, null!, err);
                },
                userMessage: "Không thể tạo mới log!"
            ) ?? new ApiResult<LogDto>(false, null!, "Lỗi hệ thống.");
        }

        public async Task<ApiResult> DeleteLogAsync(int id)
        {
            return await _errorService.TryCatchHttpAsync(
                async () =>
                {
                    var res = await _http.DeleteAsync($"api/Logs/{id}");
                    if (res.IsSuccessStatusCode)
                        return ApiResult.Success();
                    var err = await res.Content.ReadAsStringAsync();
                    return ApiResult.Fail(err);
                },
                userMessage: "Không thể xóa log!"
            ) ?? ApiResult.Fail("Lỗi hệ thống.");
        }
    }
}