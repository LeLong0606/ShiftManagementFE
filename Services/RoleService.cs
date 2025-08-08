using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using ShiftManagementFE.DTOs;

namespace ShiftManagementFE.Services
{
    public class RoleService
    {
        private readonly HttpClient _http;
        private readonly ErrorService _errorService;

        public RoleService(HttpClient http, ErrorService errorService)
        {
            _http = http;
            _errorService = errorService;
        }

        public async Task<List<RoleDto>?> GetRolesAsync(string? search = null, int page = 1, int pageSize = 50)
        {
            var url = $"api/Roles?page={page}&pageSize={pageSize}";
            if (!string.IsNullOrWhiteSpace(search))
                url += $"&search={search}";
            return await _errorService.TryCatchHttpAsync(
                async () => await _http.GetFromJsonAsync<List<RoleDto>>(url),
                userMessage: "Không thể tải danh sách vai trò!"
            );
        }

        public async Task<RoleDto?> GetRoleAsync(int id)
        {
            return await _errorService.TryCatchHttpAsync(
                async () => await _http.GetFromJsonAsync<RoleDto>($"api/Roles/{id}"),
                userMessage: "Không thể tải thông tin vai trò!"
            );
        }

        public async Task<ApiResult<RoleDto>> CreateRoleAsync(RoleDto dto)
        {
            return await _errorService.TryCatchHttpAsync(
                async () =>
                {
                    var res = await _http.PostAsJsonAsync("api/Roles", dto);
                    if (res.IsSuccessStatusCode)
                    {
                        var data = await res.Content.ReadFromJsonAsync<RoleDto>();
                        return new ApiResult<RoleDto>(true, data!, null);
                    }
                    var err = await res.Content.ReadAsStringAsync();
                    return new ApiResult<RoleDto>(false, null!, err);
                },
                userMessage: "Không thể tạo mới vai trò!"
            ) ?? new ApiResult<RoleDto>(false, null!, "Lỗi hệ thống.");
        }

        public async Task<ApiResult> UpdateRoleAsync(int id, RoleDto dto)
        {
            return await _errorService.TryCatchHttpAsync(
                async () =>
                {
                    var res = await _http.PutAsJsonAsync($"api/Roles/{id}", dto);
                    if (res.IsSuccessStatusCode)
                        return ApiResult.Success();
                    var err = await res.Content.ReadAsStringAsync();
                    return ApiResult.Fail(err);
                },
                userMessage: "Không thể cập nhật vai trò!"
            ) ?? ApiResult.Fail("Lỗi hệ thống.");
        }

        public async Task<ApiResult> DeleteRoleAsync(int id)
        {
            return await _errorService.TryCatchHttpAsync(
                async () =>
                {
                    var res = await _http.DeleteAsync($"api/Roles/{id}");
                    if (res.IsSuccessStatusCode)
                        return ApiResult.Success();
                    var err = await res.Content.ReadAsStringAsync();
                    return ApiResult.Fail(err);
                },
                userMessage: "Không thể xóa vai trò!"
            ) ?? ApiResult.Fail("Lỗi hệ thống.");
        }
    }
}