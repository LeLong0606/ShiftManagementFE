using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using ShiftManagementFE.DTOs;

namespace ShiftManagementFE.Services
{
    public class UserRoleService
    {
        private readonly HttpClient _http;
        private readonly ErrorService _errorService;

        public UserRoleService(HttpClient http, ErrorService errorService)
        {
            _http = http;
            _errorService = errorService;
        }

        public async Task<List<UserRoleDto>?> GetUserRolesAsync(int? userId = null, int? roleId = null)
        {
            var url = "api/UserRoles";
            var parameters = new List<string>();
            if (userId.HasValue) parameters.Add($"userId={userId.Value}");
            if (roleId.HasValue) parameters.Add($"roleId={roleId.Value}");
            if (parameters.Count > 0) url += "?" + string.Join("&", parameters);
            return await _errorService.TryCatchHttpAsync(
                async () => await _http.GetFromJsonAsync<List<UserRoleDto>>(url),
                userMessage: "Không thể tải danh sách phân quyền người dùng!"
            );
        }

        public async Task<ApiResult<UserRoleDto>> AddUserRoleAsync(UserRoleCreateDto dto)
        {
            return await _errorService.TryCatchHttpAsync(
                async () =>
                {
                    var res = await _http.PostAsJsonAsync("api/UserRoles", dto);
                    if (res.IsSuccessStatusCode)
                    {
                        var data = await res.Content.ReadFromJsonAsync<UserRoleDto>();
                        return new ApiResult<UserRoleDto>(true, data!, null);
                    }
                    var err = await res.Content.ReadAsStringAsync();
                    return new ApiResult<UserRoleDto>(false, null!, err);
                },
                userMessage: "Không thể phân quyền cho người dùng!"
            ) ?? new ApiResult<UserRoleDto>(false, null!, "Lỗi hệ thống.");
        }

        public async Task<UserRoleDto?> GetUserRoleAsync(int userId, int roleId)
        {
            return await _errorService.TryCatchHttpAsync(
                async () => await _http.GetFromJsonAsync<UserRoleDto>($"api/UserRoles/{userId}/{roleId}"),
                userMessage: "Không thể lấy thông tin phân quyền!"
            );
        }

        public async Task<ApiResult> DeleteUserRoleAsync(int userId, int roleId)
        {
            return await _errorService.TryCatchHttpAsync(
                async () =>
                {
                    var res = await _http.DeleteAsync($"api/UserRoles/{userId}/{roleId}");
                    if (res.IsSuccessStatusCode)
                        return ApiResult.Success();
                    var err = await res.Content.ReadAsStringAsync();
                    return ApiResult.Fail(err);
                },
                userMessage: "Không thể xóa phân quyền!"
            ) ?? ApiResult.Fail("Lỗi hệ thống.");
        }

        public async Task<ApiResult> UpdateUserRoleAsync(int userId, int roleId, UserRoleUpdateDto dto)
        {
            return await _errorService.TryCatchHttpAsync(
                async () =>
                {
                    var res = await _http.PutAsJsonAsync($"api/UserRoles/{userId}/{roleId}", dto);
                    if (res.IsSuccessStatusCode)
                        return ApiResult.Success();
                    var err = await res.Content.ReadAsStringAsync();
                    return ApiResult.Fail(err);
                },
                userMessage: "Không thể cập nhật phân quyền!"
            ) ?? ApiResult.Fail("Lỗi hệ thống.");
        }
    }
}