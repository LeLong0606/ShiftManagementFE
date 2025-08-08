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
        private readonly ErrorService _errorService;

        public HolidayService(HttpClient http, ErrorService errorService)
        {
            _http = http;
            _errorService = errorService;
        }

        public async Task<List<HolidayDto>?> GetHolidaysAsync()
        {
            return await _errorService.TryCatchHttpAsync(
                async () => await _http.GetFromJsonAsync<List<HolidayDto>>("api/Holidays"),
                userMessage: "Không thể tải danh sách ngày nghỉ!"
            );
        }

        public async Task<HolidayDto?> GetHolidayAsync(int id)
        {
            return await _errorService.TryCatchHttpAsync(
                async () => await _http.GetFromJsonAsync<HolidayDto>($"api/Holidays/{id}"),
                userMessage: "Không thể tải thông tin ngày nghỉ!"
            );
        }

        public async Task<ApiResult<HolidayDto>> CreateHolidayAsync(HolidayCreateDto dto)
        {
            return await _errorService.TryCatchHttpAsync(
                async () =>
                {
                    var res = await _http.PostAsJsonAsync("api/Holidays", dto);
                    if (res.IsSuccessStatusCode)
                    {
                        var data = await res.Content.ReadFromJsonAsync<HolidayDto>();
                        return new ApiResult<HolidayDto>(true, data!, null);
                    }
                    var err = await res.Content.ReadAsStringAsync();
                    return new ApiResult<HolidayDto>(false, null!, err);
                },
                userMessage: "Không thể tạo mới ngày nghỉ!"
            ) ?? new ApiResult<HolidayDto>(false, null!, "Lỗi hệ thống.");
        }

        public async Task<ApiResult> UpdateHolidayAsync(int id, HolidayUpdateDto dto)
        {
            return await _errorService.TryCatchHttpAsync(
                async () =>
                {
                    var res = await _http.PutAsJsonAsync($"api/Holidays/{id}", dto);
                    if (res.IsSuccessStatusCode)
                        return ApiResult.Success();
                    var err = await res.Content.ReadAsStringAsync();
                    return ApiResult.Fail(err);
                },
                userMessage: "Không thể cập nhật ngày nghỉ!"
            ) ?? ApiResult.Fail("Lỗi hệ thống.");
        }

        public async Task<ApiResult> DeleteHolidayAsync(int id)
        {
            return await _errorService.TryCatchHttpAsync(
                async () =>
                {
                    var res = await _http.DeleteAsync($"api/Holidays/{id}");
                    if (res.IsSuccessStatusCode)
                        return ApiResult.Success();
                    var err = await res.Content.ReadAsStringAsync();
                    return ApiResult.Fail(err);
                },
                userMessage: "Không thể xóa ngày nghỉ!"
            ) ?? ApiResult.Fail("Lỗi hệ thống.");
        }
    }
}