using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using ShiftManagementFE.DTOs;

namespace ShiftManagementFE.Services
{
    public class HolidayService
    {
        private readonly HttpClient _http;

        public HolidayService(HttpClient http)
        {
            _http = http;
        }

        public async Task<List<HolidayDto>?> GetHolidaysAsync()
        {
            // Đường dẫn API là tương đối, không có dấu "/" đầu dòng
            return await _http.GetFromJsonAsync<List<HolidayDto>>("api/Holidays");
        }

        public async Task<HolidayDto?> GetHolidayAsync(int id)
        {
            return await _http.GetFromJsonAsync<HolidayDto>($"api/Holidays/{id}");
        }

        public async Task<ApiResult<HolidayDto>> CreateHolidayAsync(HolidayCreateDto dto)
        {
            var res = await _http.PostAsJsonAsync("api/Holidays", dto);
            if (res.IsSuccessStatusCode)
            {
                var data = await res.Content.ReadFromJsonAsync<HolidayDto>();
                return new ApiResult<HolidayDto>(true, data!, null);
            }
            var err = await res.Content.ReadAsStringAsync();
            return new ApiResult<HolidayDto>(false, null!, err);
        }

        public async Task<ApiResult> UpdateHolidayAsync(int id, HolidayUpdateDto dto)
        {
            var res = await _http.PutAsJsonAsync($"api/Holidays/{id}", dto);
            if (res.IsSuccessStatusCode)
                return ApiResult.Success();
            var err = await res.Content.ReadAsStringAsync();
            return ApiResult.Fail(err);
        }

        public async Task<ApiResult> DeleteHolidayAsync(int id)
        {
            var res = await _http.DeleteAsync($"api/Holidays/{id}");
            if (res.IsSuccessStatusCode)
                return ApiResult.Success();
            var err = await res.Content.ReadAsStringAsync();
            return ApiResult.Fail(err);
        }
    }
}