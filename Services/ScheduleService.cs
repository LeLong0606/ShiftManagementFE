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

        public ScheduleService(HttpClient http)
        {
            _http = http;
        }

        public async Task<List<ShiftScheduleDto>?> GetSchedulesAsync(
            DateTime? from = null,
            DateTime? to = null,
            int? departmentId = null,
            int page = 1,
            int pageSize = 50)
        {
            var url = $"/api/Schedules?page={page}&pageSize={pageSize}";
            var parameters = new List<string>();
            if (from.HasValue) parameters.Add($"from={from.Value:O}");
            if (to.HasValue) parameters.Add($"to={to.Value:O}");
            if (departmentId.HasValue) parameters.Add($"departmentId={departmentId.Value}");
            if (parameters.Count > 0) url += "&" + string.Join("&", parameters);
            return await _http.GetFromJsonAsync<List<ShiftScheduleDto>>(url);
        }

        public async Task<ApiResult<ShiftScheduleDto>> CreateScheduleAsync(ShiftScheduleCreateDto dto, int createdById)
        {
            var res = await _http.PostAsJsonAsync($"/api/Schedules?createdBy={createdById}", dto);
            if (res.IsSuccessStatusCode)
            {
                var data = await res.Content.ReadFromJsonAsync<ShiftScheduleDto>();
                return new ApiResult<ShiftScheduleDto>(true, data!, null);
            }
            var err = await res.Content.ReadAsStringAsync();
            return new ApiResult<ShiftScheduleDto>(false, null!, err);
        }

        public async Task<ApiResult> UpdateScheduleAsync(int id, ShiftScheduleUpdateDto dto)
        {
            var res = await _http.PutAsJsonAsync($"/api/Schedules/{id}", dto);
            if (res.IsSuccessStatusCode)
                return ApiResult.Success();
            var err = await res.Content.ReadAsStringAsync();
            return ApiResult.Fail(err);
        }

        public async Task<ApiResult> DeleteScheduleAsync(int id)
        {
            var res = await _http.DeleteAsync($"/api/Schedules/{id}");
            if (res.IsSuccessStatusCode)
                return ApiResult.Success();
            var err = await res.Content.ReadAsStringAsync();
            return ApiResult.Fail(err);
        }
    }
}