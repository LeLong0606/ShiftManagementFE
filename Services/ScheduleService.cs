using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using ShiftManagementFE.DTOs;

namespace ShiftManagementFE.Services
{
    public class ScheduleService
    {
        private readonly HttpClient _http;
        private readonly ErrorService _errorService;

        public ScheduleService(HttpClient http, ErrorService errorService)
        {
            _http = http;
            _errorService = errorService;
        }

        public async Task<List<ShiftScheduleDto>?> GetSchedulesAsync(
            DateTime? from = null,
            DateTime? to = null,
            int? departmentId = null,
            int page = 1,
            int pageSize = 50)
        {
            var url = $"api/Schedules?page={page}&pageSize={pageSize}";
            var parameters = new List<string>();
            if (from.HasValue) parameters.Add($"from={from.Value:O}");
            if (to.HasValue) parameters.Add($"to={to.Value:O}");
            if (departmentId.HasValue) parameters.Add($"departmentId={departmentId.Value}");
            if (parameters.Count > 0) url += "&" + string.Join("&", parameters);

            return await _errorService.TryCatchHttpAsync(
                async () => await _http.GetFromJsonAsync<List<ShiftScheduleDto>>(url),
                userMessage: "Không thể tải danh sách lịch làm việc!"
            );
        }

        public async Task<ApiResult<ShiftScheduleDto>> CreateScheduleAsync(ShiftScheduleCreateDto dto, int createdById)
        {
            return await _errorService.TryCatchHttpAsync(
                async () =>
                {
                    var res = await _http.PostAsJsonAsync($"api/Schedules?createdBy={createdById}", dto);
                    if (res.IsSuccessStatusCode)
                    {
                        var data = await res.Content.ReadFromJsonAsync<ShiftScheduleDto>();
                        return new ApiResult<ShiftScheduleDto>(true, data!, null);
                    }
                    var err = await res.Content.ReadAsStringAsync();
                    return new ApiResult<ShiftScheduleDto>(false, null!, err);
                },
                userMessage: "Không thể tạo mới lịch làm việc!"
            ) ?? new ApiResult<ShiftScheduleDto>(false, null!, "Lỗi hệ thống.");
        }

        public async Task<ApiResult> UpdateScheduleAsync(int id, ShiftScheduleUpdateDto dto)
        {
            return await _errorService.TryCatchHttpAsync(
                async () =>
                {
                    var res = await _http.PutAsJsonAsync($"api/Schedules/{id}", dto);
                    if (res.IsSuccessStatusCode)
                        return ApiResult.Success();
                    var err = await res.Content.ReadAsStringAsync();
                    return ApiResult.Fail(err);
                },
                userMessage: "Không thể cập nhật lịch làm việc!"
            ) ?? ApiResult.Fail("Lỗi hệ thống.");
        }

        public async Task<ApiResult> DeleteScheduleAsync(int id)
        {
            return await _errorService.TryCatchHttpAsync(
                async () =>
                {
                    var res = await _http.DeleteAsync($"api/Schedules/{id}");
                    if (res.IsSuccessStatusCode)
                        return ApiResult.Success();
                    var err = await res.Content.ReadAsStringAsync();
                    return ApiResult.Fail(err);
                },
                userMessage: "Không thể xóa lịch làm việc!"
            ) ?? ApiResult.Fail("Lỗi hệ thống.");
        }
    }
}