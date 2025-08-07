using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using ShiftManagementFE.DTOs;

namespace ShiftManagementFE.Services
{
    public class DepartmentService
    {
        private readonly HttpClient _http;

        public DepartmentService(HttpClient http)
        {
            _http = http;
        }

        public async Task<List<DepartmentDto>?> GetDepartmentsAsync()
        {
            // Đường dẫn API dạng tương đối, không có dấu "/" đầu nếu đã set BaseAddress
            return await _http.GetFromJsonAsync<List<DepartmentDto>>("api/Departments");
        }

        public async Task<DepartmentDto?> GetDepartmentAsync(int id)
        {
            return await _http.GetFromJsonAsync<DepartmentDto>($"api/Departments/{id}");
        }

        public async Task<ApiResult<DepartmentDto>> CreateDepartmentAsync(DepartmentDto dto)
        {
            var res = await _http.PostAsJsonAsync("api/Departments", dto);
            if (res.IsSuccessStatusCode)
            {
                var data = await res.Content.ReadFromJsonAsync<DepartmentDto>();
                return new ApiResult<DepartmentDto>(true, data!, null);
            }
            var err = await res.Content.ReadAsStringAsync();
            return new ApiResult<DepartmentDto>(false, null!, err);
        }

        public async Task<ApiResult> UpdateDepartmentAsync(int id, DepartmentDto dto)
        {
            var res = await _http.PutAsJsonAsync($"api/Departments/{id}", dto);
            if (res.IsSuccessStatusCode)
                return ApiResult.Success();
            var err = await res.Content.ReadAsStringAsync();
            return ApiResult.Fail(err);
        }

        public async Task<ApiResult> DeleteDepartmentAsync(int id)
        {
            var res = await _http.DeleteAsync($"api/Departments/{id}");
            if (res.IsSuccessStatusCode)
                return ApiResult.Success();
            var err = await res.Content.ReadAsStringAsync();
            return ApiResult.Fail(err);
        }
    }
}