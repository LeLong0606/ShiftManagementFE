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

        public StoreService(HttpClient http)
        {
            _http = http;
        }

        public async Task<List<StoreDto>?> GetStoresAsync(string? search = null, int page = 1, int pageSize = 50)
        {
            // Đường dẫn sử dụng dạng tương đối, không có dấu "/" đầu nếu đã cấu hình BaseAddress
            var url = $"api/Stores?page={page}&pageSize={pageSize}";
            if (!string.IsNullOrWhiteSpace(search))
                url += $"&search={search}";
            return await _http.GetFromJsonAsync<List<StoreDto>>(url);
        }

        public async Task<StoreDto?> GetStoreAsync(int id)
        {
            return await _http.GetFromJsonAsync<StoreDto>($"api/Stores/{id}");
        }

        public async Task<ApiResult<StoreDto>> CreateStoreAsync(StoreCreateDto dto)
        {
            var res = await _http.PostAsJsonAsync("api/Stores", dto);
            if (res.IsSuccessStatusCode)
            {
                var data = await res.Content.ReadFromJsonAsync<StoreDto>();
                return new ApiResult<StoreDto>(true, data!, null);
            }
            var err = await res.Content.ReadAsStringAsync();
            return new ApiResult<StoreDto>(false, null!, err);
        }

        public async Task<ApiResult> UpdateStoreAsync(int id, StoreUpdateDto dto)
        {
            var res = await _http.PutAsJsonAsync($"api/Stores/{id}", dto);
            if (res.IsSuccessStatusCode)
                return ApiResult.Success();
            var err = await res.Content.ReadAsStringAsync();
            return ApiResult.Fail(err);
        }

        public async Task<ApiResult> DeleteStoreAsync(int id)
        {
            var res = await _http.DeleteAsync($"api/Stores/{id}");
            if (res.IsSuccessStatusCode)
                return ApiResult.Success();
            var err = await res.Content.ReadAsStringAsync();
            return ApiResult.Fail(err);
        }
    }
}