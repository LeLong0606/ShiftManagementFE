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
        private readonly ErrorService _errorService;

        public DepartmentService(HttpClient http, ErrorService errorService)
        {
            _http = http;
            _errorService = errorService;
        }

        public async Task<List<DepartmentDto>?> GetDepartmentsAsync()
        {
            return await _errorService.TryCatchHttpAsync(
                async () => await _http.GetFromJsonAsync<List<DepartmentDto>>("api/Departments"),
                userMessage: "Không thể tải danh sách phòng ban!"
            );
        }

        public async Task<DepartmentDto?> GetDepartmentAsync(int id)
        {
            return await _errorService.TryCatchHttpAsync(
                async () => await _http.GetFromJsonAsync<DepartmentDto>($"api/Departments/{id}"),
                userMessage: "Không thể tải thông tin phòng ban!"
            );
        }

        public async Task<ApiResult<DepartmentDto>> CreateDepartmentAsync(DepartmentDto dto)
        {
            return await _errorService.TryCatchHttpAsync(
                async () =>
                {
                    var res = await _http.PostAsJsonAsync("api/Departments", dto);
                    if (res.IsSuccessStatusCode)
                    {
                        var data = await res.Content.ReadFromJsonAsync<DepartmentDto>();
                        return new ApiResult<DepartmentDto>(true, data!, null);
                    }
                    var err = await res.Content.ReadAsStringAsync();
                    return new ApiResult<DepartmentDto>(false, null!, err);
                },
                userMessage: "Không thể tạo mới phòng ban!"
            ) ?? new ApiResult<DepartmentDto>(false, null!, "Lỗi hệ thống.");
        }

        public async Task<ApiResult> UpdateDepartmentAsync(int id, DepartmentDto dto)
        {
            return await _errorService.TryCatchHttpAsync(
                async () =>
                {
                    var res = await _http.PutAsJsonAsync($"api/Departments/{id}", dto);
                    if (res.IsSuccessStatusCode)
                        return ApiResult.Success();
                    var err = await res.Content.ReadAsStringAsync();
                    return ApiResult.Fail(err);
                },
                userMessage: "Không thể cập nhật phòng ban!"
            ) ?? ApiResult.Fail("Lỗi hệ thống.");
        }

        public async Task<ApiResult> DeleteDepartmentAsync(int id)
        {
            return await _errorService.TryCatchHttpAsync(
                async () =>
                {
                    var res = await _http.DeleteAsync($"api/Departments/{id}");
                    if (res.IsSuccessStatusCode)
                        return ApiResult.Success();
                    var err = await res.Content.ReadAsStringAsync();
                    return ApiResult.Fail(err);
                },
                userMessage: "Không thể xóa phòng ban!"
            ) ?? ApiResult.Fail("Lỗi hệ thống.");
        }
    }
}