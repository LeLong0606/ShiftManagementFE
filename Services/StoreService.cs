using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using ShiftManagementFE.DTOs;

namespace ShiftManagementFE.Services
{
    public class StoreService
    {
        private readonly HttpClient _http;
        private readonly ErrorService _errorService;

        public StoreService(HttpClient http, ErrorService errorService)
        {
            _http = http;
            _errorService = errorService;
        }

        public async Task<List<StoreDto>?> GetStoresAsync(string? search = null, int page = 1, int pageSize = 50)
        {
            var url = $"api/Stores?page={page}&pageSize={pageSize}";
            if (!string.IsNullOrWhiteSpace(search))
                url += $"&search={search}";
            return await _errorService.TryCatchHttpAsync(
                async () => await _http.GetFromJsonAsync<List<StoreDto>>(url),
                userMessage: "Không thể tải danh sách cửa hàng!"
            );
        }

        public async Task<StoreDto?> GetStoreAsync(int id)
        {
            return await _errorService.TryCatchHttpAsync(
                async () => await _http.GetFromJsonAsync<StoreDto>($"api/Stores/{id}"),
                userMessage: "Không thể tải thông tin cửa hàng!"
            );
        }

        public async Task<ApiResult<StoreDto>> CreateStoreAsync(StoreCreateDto dto)
        {
            return await _errorService.TryCatchHttpAsync(
                async () =>
                {
                    var res = await _http.PostAsJsonAsync("api/Stores", dto);
                    if (res.IsSuccessStatusCode)
                    {
                        var data = await res.Content.ReadFromJsonAsync<StoreDto>();
                        return new ApiResult<StoreDto>(true, data!, null);
                    }
                    var err = await res.Content.ReadAsStringAsync();
                    return new ApiResult<StoreDto>(false, null!, err);
                },
                userMessage: "Không thể tạo mới cửa hàng!"
            ) ?? new ApiResult<StoreDto>(false, null!, "Lỗi hệ thống.");
        }

        public async Task<ApiResult> UpdateStoreAsync(int id, StoreUpdateDto dto)
        {
            return await _errorService.TryCatchHttpAsync(
                async () =>
                {
                    var res = await _http.PutAsJsonAsync($"api/Stores/{id}", dto);
                    if (res.IsSuccessStatusCode)
                        return ApiResult.Success();
                    var err = await res.Content.ReadAsStringAsync();
                    return ApiResult.Fail(err);
                },
                userMessage: "Không thể cập nhật cửa hàng!"
            ) ?? ApiResult.Fail("Lỗi hệ thống.");
        }

        public async Task<ApiResult> DeleteStoreAsync(int id)
        {
            return await _errorService.TryCatchHttpAsync(
                async () =>
                {
                    var res = await _http.DeleteAsync($"api/Stores/{id}");
                    if (res.IsSuccessStatusCode)
                        return ApiResult.Success();
                    var err = await res.Content.ReadAsStringAsync();
                    return ApiResult.Fail(err);
                },
                userMessage: "Không thể xóa cửa hàng!"
            ) ?? ApiResult.Fail("Lỗi hệ thống.");
        }
    }
}