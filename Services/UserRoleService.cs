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

        public UserRoleService(HttpClient http)
        {
            _http = http;
        }

        public async Task<List<UserRoleDto>?> GetUserRolesAsync(int? userId = null, int? roleId = null)
        {
            var url = "/api/UserRoles";
            var parameters = new List<string>();
            if (userId.HasValue) parameters.Add($"userId={userId.Value}");
            if (roleId.HasValue) parameters.Add($"roleId={roleId.Value}");
            if (parameters.Count > 0) url += "?" + string.Join("&", parameters);
            return await _http.GetFromJsonAsync<List<UserRoleDto>>(url);
        }

        public async Task<ApiResult<UserRoleDto>> AddUserRoleAsync(UserRoleCreateDto dto)
        {
            var res = await _http.PostAsJsonAsync("/api/UserRoles", dto);
            if (res.IsSuccessStatusCode)
            {
                var data = await res.Content.ReadFromJsonAsync<UserRoleDto>();
                return new ApiResult<UserRoleDto>(true, data!, null);
            }
            var err = await res.Content.ReadAsStringAsync();
            return new ApiResult<UserRoleDto>(false, null!, err);
        }

        public async Task<UserRoleDto?> GetUserRoleAsync(int userId, int roleId)
        {
            return await _http.GetFromJsonAsync<UserRoleDto>($"/api/UserRoles/{userId}/{roleId}");
        }

        public async Task<ApiResult> DeleteUserRoleAsync(int userId, int roleId)
        {
            var res = await _http.DeleteAsync($"/api/UserRoles/{userId}/{roleId}");
            if (res.IsSuccessStatusCode)
                return ApiResult.Success();
            var err = await res.Content.ReadAsStringAsync();
            return ApiResult.Fail(err);
        }

        public async Task<ApiResult> UpdateUserRoleAsync(int userId, int roleId, UserRoleUpdateDto dto)
        {
            var res = await _http.PutAsJsonAsync($"/api/UserRoles/{userId}/{roleId}", dto);
            if (res.IsSuccessStatusCode)
                return ApiResult.Success();
            var err = await res.Content.ReadAsStringAsync();
            return ApiResult.Fail(err);
        }
    }
}